using Maroon.Shop.Api.Data.Repositories;
using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
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
        private readonly ProductRepository _productRepository;

        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
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
            var productResponse = _productRepository.GetById(getProductRequest);

            if (productResponse == null)
            {
                // The Product could not be found, return a 404 'Not Found' response.
                return NotFound();
            }
            else
            {                
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

            var product = await _productRepository.GetByNameAsync(productName);

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
        /// <returns>An <see cref="ActionResult{PagedResponse{ProductResponse}}"/> representing the Products found.</returns>
        [HttpGet]
        public ActionResult<PagedResponse<ProductResponse>> GetProducts([FromQuery] GetProductsRequest getProductsRequest)
        {
            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var pagedProductResponse = _productRepository.GetProducts(getProductsRequest, routeName, Url);

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

            // Map the Product entity to a new response object.
            var productResponse = _productRepository.CreateProduct(createProductRequest);

            if (productResponse == null)
            {
                return BadRequest();
            }

            // Return the created Product with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { productId = productResponse.ProductId }, productResponse);
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
            var existingProduct = _productRepository.GetById(new GetProductRequest { ProductId = productId });
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

            _productRepository.UpdateProduct(productId, updateProductRequest);

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}