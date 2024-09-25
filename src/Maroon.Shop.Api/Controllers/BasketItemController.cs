using Maroon.Shop.Api.Data.Repositories;
using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    /// <summary>
    /// Basket Item Controller Class, inherits from <see cref="Controller"/>.
    /// Handles requests routed to "api/[controller]", where [controller] is replaced by the name of the controller, in this case, "BasketItem".
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemController : Controller
    {
        private readonly BasketItemRepository _basketItemRepository;
        private readonly BasketRepository _basketRepository;
        private readonly ProductRepository _productRepository;

        public BasketItemController(BasketItemRepository basketItemRepository, BasketRepository basketRepository, ProductRepository productRepository)
        {
            _basketItemRepository = basketItemRepository;
            _basketRepository = basketRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Handles GET requests to "api/BasketItem/{basketItemId}".
        /// Attempts to retrieve a Basket Item for the given Basket Item Id.
        /// </summary>
        /// <param name="getBasketItemRequest">A <see cref="GetBasketItemRequest"/> representing the Basket Item requested.</param>
        /// <returns>An <see cref="ActionResult{BasketItemResponse}"/> representing the Basket Item found.</returns>
        [HttpGet]
        [Route("{BasketItemId}")]
        public ActionResult<BasketItemResponse> GetById([FromRoute] GetBasketItemRequest getBasketItemRequest)
        {
            if (getBasketItemRequest == null)
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

            var basketItemResponse = _basketItemRepository.GetById(getBasketItemRequest);

            if (basketItemResponse == null)
            {
                // The Product could not be found, return a 404 'Not Found' response.
                return NotFound();
            }
            else
            {
                return Ok(basketItemResponse);
            }
        }

        /// <summary>
        /// Handles GET requests to "api/BasketItem/".
        /// Attempts to retrieve all Basket Items.
        /// <param name="getBasketItemsRequest">A <see cref="GetBasketItemsRequest"/> representing the Basket Items to get.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{BasketItemResponse}}"/> representing the Basket Items found.</returns>
        [HttpGet]
        public ActionResult<PagedResponse<BasketItemResponse>> GetBasketItems([FromQuery] GetBasketItemsRequest getBasketItemsRequest)
        {
            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var pagedProductResponse = _basketItemRepository.GetBasketItems(getBasketItemsRequest, routeName, Url);

            return Ok(pagedProductResponse);
        }

        /// <summary>
        /// Handles GET requests to "api/BasketItem/ByBasketId".
        /// Attempts to retrieve all Basket Items for the given basketId.
        /// </summary>
        /// <param name="getBasketItemsByBasketRequest">A <see cref="GetBasketItemsByBasketRequest"/> representing the Basket Items By Basket Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{BasketItemResponse}}"/> representing the Basket Items found.</returns>
        [HttpGet("ByBasketId")]
        public ActionResult<PagedResponse<BasketItemResponse>> GetBasketItemsByBasket([FromQuery] GetBasketItemsByBasketRequest getBasketItemsByBasketRequest)
        {
            if (getBasketItemsByBasketRequest == null)
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

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = _basketItemRepository.GetBasketItemsByBasket(getBasketItemsByBasketRequest, routeName, Url);

            return Ok(response);
        }

        /// <summary>
        /// Handles GET requests to "api/BasketItem/ByBasketId".
        /// Attempts to retrieve all Basket Items for the given basketId.
        /// </summary>
        /// <param name="getBasketItemsByProductRequest">A <see cref="GetBasketItemsByProductRequest"/> representing the Basket Items By Product Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{BasketItemResponse}}"/> representing the Basket Items found.</returns>
        [HttpGet("ByProductId")]
        public ActionResult<PagedResponse<BasketItemResponse>> GetBasketItemsByProduct([FromQuery] GetBasketItemsByProductRequest getBasketItemsByProductRequest)
        {
            if (getBasketItemsByProductRequest == null)
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

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = _basketItemRepository.GetBasketItemsByProduct(getBasketItemsByProductRequest, routeName, Url);

            return Ok(response);
        }

        /// <summary>
        /// Handles POST requests to "api/BasketItem/Create".
        /// Attempts to create a new Basket Item.
        /// </summary>
        /// <param name="createBasketItemRequest">A <see cref="CreateBasketItemRequest"/> representing the new Basket Item to be created.</param>
        /// <returns>An <see cref="ActionResult{BasketItemResponse}"/> representing the created Basket Item.</returns>
        [HttpPost("Create")]
        public ActionResult<BasketItemResponse> CreateBasketItem([FromBody] CreateBasketItemRequest createBasketItemRequest)
        {
            if (createBasketItemRequest == null)
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

            // Check that the associated Basket Exists.
            var basket = _basketRepository.GetById(new GetBasketRequest { BasketId = createBasketItemRequest.BasketId });
            if (basket == null)
            {
                // A Basket doesn't exist for the given Basket Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Basket Id is invalid.");
            }

            // Check that the associated Product Exists.
            var product = _productRepository.GetById(new GetProductRequest { ProductId = createBasketItemRequest.ProductId });
            if (product == null)
            {
                // A Product doesn't exist for the given product Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Product Id is invalid.");
            }

            // Map the Basket Item entity to a new response object.
            var basketItemResponse = _basketItemRepository.CreateBasketItem(createBasketItemRequest);

            if (basketItemResponse == null)
            {
                return BadRequest();
            }

            // Return the created Basket with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { basketItemId = basketItemResponse?.BasketItemId }, basketItemResponse);
        }

        /// <summary>
        /// Handles PUT requests to "api/BasketItem/Update".
        /// Attempts to update an existing Basket Item.
        /// </summary>
        /// <param name="basketItemId">A <see cref="long"/> representing the Id of the Basket Item to update.</param>
        /// <param name="updateBasketItemRequest">An <see cref="UpdateBasketItemRequest"/> representing the updated Basket Item data.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.</returns>
        [HttpPut("Update")]
        public IActionResult UpdateBasketItem(long basketItemId, [FromBody] UpdateBasketItemRequest updateBasketItemRequest)
        {
            if (updateBasketItemRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            if (updateBasketItemRequest.BasketItemId != basketItemId)
            {
                // The basketItemId does not match the Basket Item. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Basket Item Id mismatch.");
            }

            // Attempt to get the Basket to be updated.
            var existingBasketItem = _basketItemRepository.GetById(new GetBasketItemRequest { BasketItemId = basketItemId });
            if (existingBasketItem == null)
            {
                // The Basket Item to be updated does not exist. Therefore, return a 404 'Not Found' response.
                return NotFound();
            }

            // Validate the Request.
            if (!ModelState.IsValid)
            {
                // The Model State is invalid. Therefore, return a 400 'Bad Request' response with validation errors.
                return BadRequest(ModelState);
            }

            // Check that the associated Basket Exists.
            var basket = _basketRepository.GetById(new GetBasketRequest { BasketId = updateBasketItemRequest.BasketId });
            if (basket == null)
            {
                // A Basket doesn't exist for the given Basket Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Basket Id is invalid.");
            }

            // Check that the associated Product Exists.
            var product = _productRepository.GetById(new GetProductRequest { ProductId = updateBasketItemRequest.ProductId });
            if (product == null)
            {
                // A Product doesn't exist for the given product Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Product Id is invalid.");
            }

            _basketItemRepository.UpdateBasketItem(basketItemId, updateBasketItemRequest);

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}