using Maroon.Shop.Api.Data.Repositories;
using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
using Microsoft.AspNetCore.Mvc;

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
        private readonly BasketRepository _basketRepository;
        private readonly CustomerRepository _customerRepository;

        public BasketController(BasketRepository basketRepository, CustomerRepository customerRepository)
        {
            _basketRepository = basketRepository;
            _customerRepository = customerRepository;
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

            var basketResponse = _basketRepository.GetById(getBasketRequest);

            if (basketResponse == null)
            {
                // The Basket could not be found, return a 404 'Not Found' response.
                return NotFound();
            }
            else
            {
                return Ok(basketResponse);
            }
        }

        /// <summary>
        /// Handles GET requests to "api/Basket/".
        /// Attempts to retrieve all Baskets.
        /// <param name="getBasketsRequest">A <see cref="GetBasketsRequest"/> representing the Baskets to get.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{BasketResponse}}"/> representing the Baskets found.</returns>
        [HttpGet]
        public ActionResult<PagedResponse<BasketResponse>> GetBaskets([FromQuery] GetBasketsRequest getBasketsRequest)
        {
            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var pagedProductResponse = _basketRepository.GetBaskets(getBasketsRequest, routeName, Url);

            return Ok(pagedProductResponse);
        }

        /// <summary>
        /// Handles GET requests to "api/Basket/ByCustomerId".
        /// Attempts to retrieve all Baskets for the given customerId.
        /// </summary>
        /// <param name="getBasketsByCustomerRequest">A <see cref="GetBasketsByCustomerRequest"/> representing the Baskets By Customer Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{BasketResponse}}"/> representing the Baskets found.</returns>
        [HttpGet("ByCustomerId")]
        public ActionResult<PagedResponse<BasketResponse>> GetBasketsByCustomer([FromQuery] GetBasketsByCustomerRequest getBasketsByCustomerRequest)
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

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = _basketRepository.GetBasketsByCustomer(getBasketsByCustomerRequest, routeName, Url);

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
            var customer = _customerRepository.GetById(new GetCustomerRequest { CustomerId = createBasketRequest.CustomerId });
            if (customer == null)
            {
                // A Customer doesn't exist for the given Customer Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Customer Id is invalid.");
            }

            // Map the Basket entity to a new response object.
            var basketResponse = _basketRepository.CreateBasket(createBasketRequest);

            // something else went wrong, we don't have the information to say what
            if (basketResponse == null)
            {
                return BadRequest();
            }

            // Return the created Basket with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { basketId = basketResponse?.BasketId }, basketResponse);
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
            var existingBasket = _basketRepository.GetById(new GetBasketRequest { BasketId = basketId });
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
            var customer = _customerRepository.GetById(new GetCustomerRequest { CustomerId = updateBasketRequest.CustomerId });
            if (customer == null)
            {
                // A Customer doesn't for the given Customer Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Customer Id is invalid.");
            }

            _basketRepository.UpdateBasket(basketId, updateBasketRequest);

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}