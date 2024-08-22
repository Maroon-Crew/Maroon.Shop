using Maroon.Shop.Api.Requests;
using Maroon.Shop.Api.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;

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
        /// <param name="productId">A <see cref="long"/> representing the Id of the Product.</param>
        /// <returns>An <see cref="ActionResult{ProductResponse}"/> representing the Product found.</returns>
        [HttpGet]
        [Route("{productId}")]
        public ActionResult<ProductResponse> GetById(long productId)
        {
            // Query for a Product with the given productId.
            var query = _context.Products.Where(product => product.ProductId == productId);

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
                    UrlFriendlyName = product.UrlFriendlyName,
                    Price = product.Price
                };
                
                return Ok(productResponse);
            }
        }

        /// <summary>
        /// Handles GET requests to "api/Product/All".
        /// Attempts to retrieve all Products.
        /// </summary>
        /// <param name="request">A <see cref="ProductsRequest"/> representing the Products to get.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{Product}}"/> representing the Products found.</returns>
        [HttpGet("All")]
        public ActionResult<PagedResponse<Product>> GetProducts([FromQuery] ProductsRequest request)
        {
            // Retrieve all Products using pagination.
            var products = _context.Products
                .OrderBy(products => products.ProductId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.Products.Count();

            // Create the response.
            var response = new PagedResponse<Product>
            {
                Data = products,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalRecords = totalRecords
            };

            return Ok(response);
        }

        /// <summary>
        /// Handles POST requests to "api/Product/Create".
        /// Attempts to create a new Product.
        /// </summary>
        /// <param name="request">A <see cref="CreateProductRequest"/> representing the new Product to be created.</param>
        /// <returns>An <see cref="ActionResult{ProductResponse}"/> representing the created Product.</returns>
        [HttpPost("Create")]
        public ActionResult<ProductResponse> CreateProduct([FromBody] CreateProductRequest request)
        {
            if (request == null)
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
            var product = new Product
            {
                Name = request.Name,
                UrlFriendlyName = request.UrlFriendlyName,
                Price = request.Price
            };

            // Add the new Product to the Database Context.
            _context.Products.Add(product);
            _context.SaveChanges();

            // Map the Product entity to a new response object.
            var response = new ProductResponse
            {
                ProductId = product.ProductId,
                Name = product.Name,
                UrlFriendlyName = product.UrlFriendlyName,
                Price = product.Price
            };

            // Return the created Product with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { productId = product.ProductId }, response);
        }

        /// <summary>
        /// Handles PUT requests to "api/Product/Update".
        /// Attempts to update an existing Product.
        /// </summary>
        /// <param name="productId">A <see cref="long"/> representing the Id of the Product to update.</param>
        /// <param name="request">An <see cref="UpdateProductRequest"/> representing the updated Product data.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.</returns>
        [HttpPut("Update")]
        public IActionResult UpdateProduct(long productId, [FromBody] UpdateProductRequest request)
        {
            if (request == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            if (request.ProductId != productId)
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

            if (!ModelState.IsValid)
            {
                // The Model State is invalid. Therefore, return a 400 'Bad Request' response with validation errors.
                return BadRequest(ModelState);
            }

            // Update the existing Product with the values from the provided Product.
            existingProduct.Name = request.Name;
            existingProduct.UrlFriendlyName = request.UrlFriendlyName;
            existingProduct.Price = request.Price;

            // Save the changes to the database.
            _context.SaveChanges();

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}