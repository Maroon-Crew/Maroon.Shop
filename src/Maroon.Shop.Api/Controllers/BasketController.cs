using Maroon.Shop.Api.Requests;
using Maroon.Shop.Api.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maroon.Shop.Api.Controllers
{
    /// <summary>
    /// Basket Controller Class, inherits from <see cref="Controller"/>.
    /// Handles requests routed to "api/[controller]", where [controller] is replaced by the name of the controller, in this case, "Basket".
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : Controller
    {
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises the Basket Controller.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Data Context.</param>
        public BasketController(ShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles GET requests to "api/Basket/{BasketId}".
        /// Attempts to retrieve a Basket for the given Basket Id.
        /// </summary>
        /// <param name="getBasketRequest">A <see cref="GetBasketRequest"/> representing the Basket requested.</param>
        /// <returns>An <see cref="ActionResult{BasketResponse}"/> representing the Basket found.</returns>
        [HttpGet]
        [Route("{BasketId}")]
        public ActionResult<BasketResponse> GetById([FromRoute] GetBasketRequest getBasketRequest)
        {
            if (getBasketRequest == null)
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

            // Query for a Basket with the given basketId.
            var query = _context.Baskets
                .Include(basket => basket.Customer)
                .Where(basket => basket.BasketId == getBasketRequest.BasketId);

            if (!query.Any())
            {
                // The Basket could not be found, return a 404 'Not Found' response.
                return NotFound();
            }
            else
            {
                var basket = query.First();
                var basketResponse = new BasketResponse
                {
                    BasketId = basket.BasketId,
                    CustomerId = basket.Customer.CustomerId,
                    TotalPrice = basket.TotalPrice
                };

                return Ok(basketResponse);
            }
        }

        /// <summary>
        /// Handles GET requests to "api/Basket/".
        /// Attempts to retrieve all Baskets.
        /// <param name="getBasketsRequest">A <see cref="GetBasketsRequest"/> representing the Baskets to get.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{Basket}}"/> representing the Baskets found.</returns>
        [HttpGet]
        public ActionResult<PagedResponse<Basket>> GetBaskets([FromQuery] GetBasketsRequest getBasketsRequest)
        {
            // Retrieve all Baskets using pagination.
            var baskets = _context.Baskets
                .Include(basket => basket.Customer)
                .OrderBy(basket => basket.BasketId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((getBasketsRequest.PageNumber - 1) * getBasketsRequest.PageSize)
                .Take(getBasketsRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.Baskets.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var pagedProductResponse = new PagedResponse<Basket>(
                 data: baskets,
                 pageNumber: getBasketsRequest.PageNumber,
                 pageSize: getBasketsRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName
             );

            return Ok(pagedProductResponse);
        }

        /// <summary>
        /// Handles GET requests to "api/Basket/ByCustomerId".
        /// Attempts to retrieve all Baskets for the given customerId.
        /// </summary>
        /// <param name="getBasketsByCustomerRequest">A <see cref="GetBasketsByCustomerRequest"/> representing the Baskets By Customer Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{Basket}}"/> representing the Baskets found.</returns>
        [HttpGet("ByCustomerId")]
        public ActionResult<PagedResponse<Basket>> GetBasketsByCustomer([FromQuery] GetBasketsByCustomerRequest getBasketsByCustomerRequest)
        {
            if (getBasketsByCustomerRequest == null)
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

            // Retrieve all Baskets for the given Customer Id.
            var filteredBaskets = _context.Baskets
                .Include(basket => basket.Customer)
                .Where(basket => basket.Customer.CustomerId == getBasketsByCustomerRequest.CustomerId);

            // Apply Pagination.
            var filteredBasketsPaginated = filteredBaskets
                .Skip((getBasketsByCustomerRequest.PageNumber - 1) * getBasketsByCustomerRequest.PageSize)
                .Take(getBasketsByCustomerRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredBaskets.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = new PagedResponse<Basket>(
                 data: filteredBasketsPaginated,
                 pageNumber: getBasketsByCustomerRequest.PageNumber,
                 pageSize: getBasketsByCustomerRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName,
                 routeValues: new { getBasketsByCustomerRequest.CustomerId } // Pass in the CustomerId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return Ok(response);
        }

        /// <summary>
        /// Handles POST requests to "api/Basket/Create".
        /// Attempts to create a new Basket.
        /// </summary>
        /// <param name="createBasketRequest">A <see cref="CreateBasketRequest"/> representing the new Basket to be created.</param>
        /// <returns>An <see cref="ActionResult{BasketResponse}"/> representing the created Basket.</returns>
        [HttpPost("Create")]
        public ActionResult<BasketResponse> CreateBasket([FromBody] CreateBasketRequest createBasketRequest)
        {
            if (createBasketRequest == null)
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

            // Check that the associated Customer Exists.
            var customer = _context.Customers.FirstOrDefault(customer => customer.CustomerId == createBasketRequest.CustomerId);
            if (customer == null)
            {
                // A Customer doesn't exist for the given Customer Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Customer Id is invalid.");
            }

            // Map the request object to a new Basket entity.
            var newBasket = new Basket
            {
                Customer = customer,
                TotalPrice = createBasketRequest.TotalPrice
            };

            // Add the new Basket to the Database Context.
            _context.Baskets.Add(newBasket);
            _context.SaveChanges();

            // Map the Basket entity to a new response object.
            var basketResponse = new BasketResponse
            {
                BasketId = newBasket.BasketId,
                CustomerId = newBasket.Customer.CustomerId,
                TotalPrice = newBasket.TotalPrice
            };

            // Return the created Basket with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { basketId = newBasket.BasketId }, basketResponse);
        }

        /// <summary>
        /// Handles PUT requests to "api/Basket/Update".
        /// Attempts to update an existing Basket.
        /// </summary>
        /// <param name="basketId">A <see cref="long"/> representing the Id of the Basket to update.</param>
        /// <param name="updateBasketRequest">An <see cref="UpdateBasketRequest"/> representing the updated Basket data.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.</returns>
        [HttpPut("Update")]
        public IActionResult UpdateBasket(long basketId, [FromBody] UpdateBasketRequest updateBasketRequest)
        {
            if (updateBasketRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            if (updateBasketRequest.BasketId != basketId)
            {
                // The basketId does not match the Basket. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Basket Id mismatch.");
            }

            // Attempt to get the Basket to be updated.
            var existingBasket = _context.Baskets.FirstOrDefault(basket => basket.BasketId == basketId);
            if (existingBasket == null)
            {
                // The Basket to be updated does not exist. Therefore, return a 404 'Not Found' response.
                return NotFound();
            }

            // Validate the Request.
            if (!ModelState.IsValid)
            {
                // The Model State is invalid. Therefore, return a 400 'Bad Request' response with validation errors.
                return BadRequest(ModelState);
            }

            // Check that the associated Customer Exists.
            var customer = _context.Customers.FirstOrDefault(customer => customer.CustomerId == updateBasketRequest.CustomerId);
            if (customer == null)
            {
                // A Customer doesn't for the given Customer Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Customer Id is invalid.");
            }

            // Update the existing Basket with the values from the provided Basket.
            existingBasket.Customer = customer;
            existingBasket.TotalPrice = updateBasketRequest.TotalPrice;

            // Save the changes to the database.
            _context.SaveChanges();

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}