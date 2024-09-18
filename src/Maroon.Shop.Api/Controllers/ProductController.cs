using Maroon.Shop.Api.Requests;
using Maroon.Shop.Api.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maroon.Shop.Api.Controllers
{
    /// <summary>
    /// Product Controller Class, inherits from <see cref="Controller"/>.
    /// Handles requests routed to "api/[controller]", where [controller] is replaced by the name of the controller, in this case, "Product".
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises the Product Controller.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Data Context.</param>
        public ProductController(ShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles GET requests to "api/Product/{productId}".
        /// Attempts to retrieve a Product for the given Product Id.
        /// </summary>
        /// <param name="getProductRequest">A <see cref="GetProductRequest"/> representing the Product requested.</param>
        /// <returns>An <see cref="ActionResult{ProductResponse}"/> representing the Product found.</returns>
        [HttpGet]
        [Route("GetById/{ProductId}")]
        public ActionResult<ProductResponse> GetById([FromRoute] GetProductRequest getProductRequest)
        {
            if (getProductRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            // Query for a Product with the given productId.
            var query = _context.Products.Where(product => product.ProductId == getProductRequest.ProductId);

            if (!query.Any())
            {
                // The Product could not be found, return a 404 'Not Found' response.
                return NotFound();
            }
            else
            {
                var product = query.First();
                var productResponse = new ProductResponse
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    PleaseNote = product.PleaseNote,
                    UrlFriendlyName = product.UrlFriendlyName,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price
                };
                
                return Ok(productResponse);
            }
        }

        [HttpGet]
        [Route("{productName}")]
        public async Task<ActionResult<ProductResponse>> GetByName([FromRoute] string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest($"{nameof(productName)} cannot be null.");
            }

            var query = from p in _context.Products
                        where p.UrlFriendlyName == productName
                        select new ProductResponse
                        {
                            ProductId = p.ProductId,
                            ImageUrl = p.ImageUrl,
                            Name = p.Name,
                            Price = p.Price,
                            UrlFriendlyName = p.UrlFriendlyName,
                            Description = p.Description,
                            PleaseNote = p.PleaseNote,
                        };

            var product = await query.FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        /// <summary>
        /// Handles GET requests to "api/Product/".
        /// Attempts to retrieve all Products.
        /// </summary>
        /// <param name="getProductsRequest">A <see cref="GetProductsRequest"/> representing the Products to get.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{Product}}"/> representing the Products found.</returns>
        [HttpGet]
        public ActionResult<PagedResponse<Product>> GetProducts([FromQuery] GetProductsRequest getProductsRequest)
        {
            // Retrieve all Products using pagination.
            var products = _context.Products
                .OrderBy(products => products.ProductId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((getProductsRequest.PageNumber - 1) * getProductsRequest.PageSize)
                .Take(getProductsRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.Products.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var pagedProductResponse = new PagedResponse<Product>(
                 data: products,
                 pageNumber: getProductsRequest.PageNumber,
                 pageSize: getProductsRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName
             );

            return Ok(pagedProductResponse);
        }

        /// <summary>
        /// Handles POST requests to "api/Product/Create".
        /// Attempts to create a new Product.
        /// </summary>
        /// <param name="createProductRequest">A <see cref="CreateProductRequest"/> representing the new Product to be created.</param>
        /// <returns>An <see cref="ActionResult{ProductResponse}"/> representing the created Product.</returns>
        [HttpPost("Create")]
        public ActionResult<ProductResponse> CreateProduct([FromBody] CreateProductRequest createProductRequest)
        {
            if (createProductRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            // Validate the Request.
            if (!ModelState.IsValid)
            {
                // The Model State is invalid. Therefore, return a 400 'Bad Request' response with validation errors.
                return BadRequest(ModelState);
            }

            // Map the request object to a new Product entity.
            var newProduct = new Product
            {
                Name = createProductRequest.Name,
                UrlFriendlyName = createProductRequest.UrlFriendlyName,
                Price = createProductRequest.Price,
                Description = createProductRequest.Description,
                PleaseNote = createProductRequest.PleaseNote,
                ImageUrl = createProductRequest.ImageUrl,
            };

            // Add the new Product to the Database Context.
            _context.Products.Add(newProduct);
            _context.SaveChanges();

            // Map the Product entity to a new response object.
            var productResponse = new ProductResponse
            {
                ProductId = newProduct.ProductId,
                Name = newProduct.Name,
                Description = newProduct.Description,
                PleaseNote = newProduct.PleaseNote,
                UrlFriendlyName = newProduct.UrlFriendlyName,
                ImageUrl = newProduct.ImageUrl,
                Price = newProduct.Price
            };

            // Return the created Product with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { productId = newProduct.ProductId }, productResponse);
        }

        /// <summary>
        /// Handles PUT requests to "api/Product/Update".
        /// Attempts to update an existing Product.
        /// </summary>
        /// <param name="productId">A <see cref="long"/> representing the Id of the Product to update.</param>
        /// <param name="updateProductRequest">An <see cref="UpdateProductRequest"/> representing the updated Product data.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.</returns>
        [HttpPut("Update")]
        public IActionResult UpdateProduct(long productId, [FromBody] UpdateProductRequest updateProductRequest)
        {
            if (updateProductRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            if (updateProductRequest.ProductId != productId)
            {
                // The productId does not match the Product. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Product Id mismatch.");
            }

            // Attempt to get the Product to be updated.
            var existingProduct = _context.Products.FirstOrDefault(product => product.ProductId == productId);
            if (existingProduct == null)
            {
                // The Product to be updated does not exist. Therefore, return a 404 'Not Found' response.
                return NotFound();
            }

            // Validate the Request.
            if (!ModelState.IsValid)
            {
                // The Model State is invalid. Therefore, return a 400 'Bad Request' response with validation errors.
                return BadRequest(ModelState);
            }

            // Update the existing Product with the values from the provided Product.
            existingProduct.Name = updateProductRequest.Name;
            existingProduct.UrlFriendlyName = updateProductRequest.UrlFriendlyName;
            existingProduct.Price = updateProductRequest.Price;
            existingProduct.Description = updateProductRequest.Description;
            existingProduct.PleaseNote = updateProductRequest.PleaseNote;
            existingProduct.ImageUrl = updateProductRequest.ImageUrl;

            // Save the changes to the database.
            _context.SaveChanges();

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}