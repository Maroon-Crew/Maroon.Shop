using Maroon.Shop.Api.Requests;
using Maroon.Shop.Api.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises the Basket Item Controller.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Data Context.</param>
        public BasketItemController(ShopContext context)
        {
            _context = context;
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

            // Query for a Basket Item with the given basketItemId.
            var query = _context.BasketItems
                .Include(basketItem => basketItem.Product)
                .Include(basketItem => basketItem.Basket)
                .Where(basketItem => basketItem.BasketItemId == getBasketItemRequest.BasketItemId);

            if (!query.Any())
            {
                // The Product could not be found, return a 404 'Not Found' response.
                return NotFound();
            }
            else
            {
                var basketItem = query.First();
                var basketItemResponse = new BasketItemResponse
                {
                    BasketItemId = basketItem.BasketItemId,
                    BasketId = basketItem.Basket.BasketId,
                    ProductId = basketItem.Product.ProductId,
                    Quantity = basketItem.Quantity,
                    UnitPrice = basketItem.UnitPrice,
                    TotalPrice = basketItem.TotalPrice
                };

                return Ok(basketItemResponse);
            }
        }

        /// <summary>
        /// Handles GET requests to "api/BasketItem/".
        /// Attempts to retrieve all Basket Items.
        /// <param name="getBasketItemsRequest">A <see cref="GetBasketItemsRequest"/> representing the Basket Items to get.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{BasketItem}}"/> representing the Basket Items found.</returns>
        [HttpGet]
        public ActionResult<PagedResponse<Basket>> GetBasketItems([FromQuery] GetBasketItemsRequest getBasketItemsRequest)
        {
            // Retrieve all Basket Items using pagination.
            var basketItems = _context.BasketItems
                .Include(basketItem => basketItem.Product)
                .Include(basketItem => basketItem.Basket)
                .OrderBy(basketItem => basketItem.BasketItemId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((getBasketItemsRequest.PageNumber - 1) * getBasketItemsRequest.PageSize)
                .Take(getBasketItemsRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.BasketItems.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var pagedProductResponse = new PagedResponse<BasketItem>(
                 data: basketItems,
                 pageNumber: getBasketItemsRequest.PageNumber,
                 pageSize: getBasketItemsRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName
             );

            return Ok(pagedProductResponse);
        }

        /// <summary>
        /// Handles GET requests to "api/BasketItem/ByBasketId".
        /// Attempts to retrieve all Basket Items for the given basketId.
        /// </summary>
        /// <param name="getBasketItemsByBasketRequest">A <see cref="GetBasketItemsByBasketRequest"/> representing the Basket Items By Basket Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{BasketItem}}"/> representing the Basket Items found.</returns>
        [HttpGet("ByBasketId")]
        public ActionResult<PagedResponse<BasketItem>> GetBasketItemsByBasket([FromQuery] GetBasketItemsByBasketRequest getBasketItemsByBasketRequest)
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

            // Retrieve all Basket Itemss for the given Basket Id.
            var filteredBasketItems = _context.BasketItems
                .Include(basketItem => basketItem.Basket)
                .Where(basketItem => basketItem.Basket.BasketId == getBasketItemsByBasketRequest.BasketId);

            // Apply Pagination.
            var filteredBasketItemsPaginated = filteredBasketItems
                .Skip((getBasketItemsByBasketRequest.PageNumber - 1) * getBasketItemsByBasketRequest.PageSize)
                .Take(getBasketItemsByBasketRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredBasketItems.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = new PagedResponse<BasketItem>(
                 data: filteredBasketItemsPaginated,
                 pageNumber: getBasketItemsByBasketRequest.PageNumber,
                 pageSize: getBasketItemsByBasketRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName,
                 routeValues: new { getBasketItemsByBasketRequest.BasketId } // Pass in the BasketId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return Ok(response);
        }

        /// <summary>
        /// Handles GET requests to "api/BasketItem/ByBasketId".
        /// Attempts to retrieve all Basket Items for the given basketId.
        /// </summary>
        /// <param name="getBasketItemsByProductRequest">A <see cref="GetBasketItemsByProductRequest"/> representing the Basket Items By Product Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{BasketItem}}"/> representing the Basket Items found.</returns>
        [HttpGet("ByProductId")]
        public ActionResult<PagedResponse<BasketItem>> GetBasketItemsByProduct([FromQuery] GetBasketItemsByProductRequest getBasketItemsByProductRequest)
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

            // Retrieve all Basket Itemss for the given Basket Id.
            var filteredBasketItems = _context.BasketItems
                .Include(basketItem => basketItem.Product)
                .Where(basketItem => basketItem.Product.ProductId == getBasketItemsByProductRequest.ProductId);

            // Apply Pagination.
            var filteredBasketItemsPaginated = filteredBasketItems
                .Skip((getBasketItemsByProductRequest.PageNumber - 1) * getBasketItemsByProductRequest.PageSize)
                .Take(getBasketItemsByProductRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredBasketItems.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = new PagedResponse<BasketItem>(
                 data: filteredBasketItemsPaginated,
                 pageNumber: getBasketItemsByProductRequest.PageNumber,
                 pageSize: getBasketItemsByProductRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName,
                 routeValues: new { getBasketItemsByProductRequest.ProductId } // Pass in the ProductId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

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
            var basket = _context.Baskets.FirstOrDefault(basket => basket.BasketId == createBasketItemRequest.BasketId);
            if (basket == null)
            {
                // A Basket doesn't exist for the given Basket Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Basket Id is invalid.");
            }

            // Check that the associated Product Exists.
            var product = _context.Products.FirstOrDefault(product => product.ProductId == createBasketItemRequest.ProductId);
            if (product == null)
            {
                // A Product doesn't exist for the given product Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Product Id is invalid.");
            }

            // Map the request object to a new Basket Item entity.
            var newBasketItem = new BasketItem
            {
                Basket = basket,
                Product = product,
                Quantity = createBasketItemRequest.Quantity,
                UnitPrice = createBasketItemRequest.UnitPrice,
                TotalPrice = createBasketItemRequest.TotalPrice
            };

            // Add the new Basket Item to the Database Context.
            _context.BasketItems.Add(newBasketItem);
            _context.SaveChanges();

            // Map the Basket Item entity to a new response object.
            var basketItemResponse = new BasketItemResponse
            {
                BasketId = newBasketItem.Basket.BasketId,
                ProductId = newBasketItem.Product.ProductId,
                Quantity = newBasketItem.Quantity,
                UnitPrice = newBasketItem.UnitPrice,
                TotalPrice = newBasketItem.TotalPrice
            };

            // Return the created Basket with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { basketItemId = newBasketItem.BasketItemId }, basketItemResponse);
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
            var existingBasketItem = _context.BasketItems.FirstOrDefault(basketItem => basketItem.BasketItemId == basketItemId);
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
            var basket = _context.Baskets.FirstOrDefault(basket => basket.BasketId == updateBasketItemRequest.BasketId);
            if (basket == null)
            {
                // A Basket doesn't exist for the given Basket Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Basket Id is invalid.");
            }

            // Check that the associated Product Exists.
            var product = _context.Products.FirstOrDefault(product => product.ProductId == updateBasketItemRequest.ProductId);
            if (product == null)
            {
                // A Product doesn't exist for the given product Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Product Id is invalid.");
            }

            // Update the existing Basket with the values from the provided Basket Item.
            existingBasketItem.Basket = basket;
            existingBasketItem.Product = product;
            existingBasketItem.Quantity = updateBasketItemRequest.Quantity;
            existingBasketItem.UnitPrice = updateBasketItemRequest.UnitPrice;
            existingBasketItem.TotalPrice = updateBasketItemRequest.TotalPrice;

            // Save the changes to the database.
            _context.SaveChanges();

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}