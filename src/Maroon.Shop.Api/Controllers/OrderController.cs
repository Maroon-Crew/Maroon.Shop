using Maroon.Shop.Api.Data.Repositories;
using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    /// <summary>
    /// Order Controller Class, inherits from <see cref="Controller"/>.
    /// Handles requests routed to "api/[controller]", where [controller] is replaced by the name of the controller, in this case, "Order".
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly OrderRepository _orderRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly AddressRepository _addressRepository;

        public OrderController(OrderRepository orderRepository, CustomerRepository customerRepository, AddressRepository addressRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
        }

        /// <summary>
        /// Handles GET requests to "api/Order/{orderId}".
        /// Attempts to retrieve an Order for the given Order Id.
        /// </summary>
        /// <param name="getOrderRequest">A <see cref="GetOrderRequest"/> representing the Order requested.</param>
        /// <returns>An <see cref="ActionResult{OrderResponse}"/> representing the Order found.</returns>
        [HttpGet]
        [Route("{OrderId}")]
        public ActionResult<OrderResponse> GetById([FromRoute] GetOrderRequest getOrderRequest)
        {
            if (getOrderRequest == null)
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

            var orderResponse = _orderRepository.GetById(new GetOrderRequest { OrderId = getOrderRequest.OrderId });

            if (orderResponse == null)
            {
                // The Order could not be found, return a 404 'Not Found' response.
                return NotFound();
            }
            else
            {                
                return Ok(orderResponse);
            }
        }

        /// <summary>
        /// Handles GET requests to "api/Order/".
        /// Attempts to retrieve all Orders.
        /// <param name="getOrdersRequest">A <see cref="GetOrdersRequest"/> representing the Orders to get.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{OrderResponse}}"/> representing the Orders found.</returns>
        [HttpGet]
        public ActionResult<PagedResponse<OrderResponse>> GetOrders([FromQuery] GetOrdersRequest getOrdersRequest)
        {
            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var pagedOrderResponse = _orderRepository.GetOrders(getOrdersRequest, routeName, Url);

            return Ok(pagedOrderResponse);
        }

        /// <summary>
        /// Handles GET requests to "api/Order/ByCustomerId".
        /// Attempts to retrieve all Orders for the given customerId.
        /// </summary>
        /// <param name="customerId">A <see cref="long"/> representing the Id of the Customer.</param>
        /// <returns>An <see cref="ActionResult{IEnumerable{OrderResponse}}"/> representing the Orders found.</returns>
        [HttpGet()]
        [Route("ByCustomerId/{CustomerId}")]
        public ActionResult<IEnumerable<OrderResponse>> GetOrdersByCustomer([FromRoute] GetOrderByCustomerRequest getOrderByCustomerRequest)
        {
            return Ok(_orderRepository.GetOrdersByCustomer(getOrderByCustomerRequest));
        }

        /// <summary>
        /// Handles GET requests to "api/Order/ByCustomerId".
        /// Attempts to retrieve all Orders for the given customerId.
        /// </summary>
        /// <param name="getOrdersByCustomerRequest">A <see cref="GetOrdersByCustomerRequest"/> representing the Orders By Customer Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{OrderResponse}}"/> representing the Orders found.</returns>
        [HttpGet("ByCustomerId")]
        public ActionResult<PagedResponse<OrderResponse>> GetOrdersByCustomer([FromQuery] GetOrdersByCustomerRequest getOrdersByCustomerRequest)
        {
            if (getOrdersByCustomerRequest == null)
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
            var response = _orderRepository.GetOrdersByCustomer(getOrdersByCustomerRequest, routeName, Url);

            return Ok(response);
        }

        /// <summary>
        /// Handles GET requests to "api/Order/ByBillingAddressId".
        /// Attempts to retrieve all Orders for the given billingAddressId.
        /// </summary>
        /// <param name="getOrdersByBillingAddressRequest">A <see cref="GetOrdersByBillingAddressRequest"/> representing the Orders By Billing Address Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{OrderResponse}}"/> representing the Orders found.</returns>
        [HttpGet("ByBillingAddressId")]
        public ActionResult<PagedResponse<OrderResponse>> GetOrdersByBillingAddress([FromQuery] GetOrdersByBillingAddressRequest getOrdersByBillingAddressRequest)
        {
            if (getOrdersByBillingAddressRequest == null)
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
            var response = _orderRepository.GetOrdersByBillingAddress(getOrdersByBillingAddressRequest, routeName, Url);

            return Ok(response);
        }

        /// <summary>
        /// Handles GET requests to "api/Order/ByShippingAddressId".
        /// Attempts to retrieve all Orders for the given shippingAddressId.
        /// </summary>
        /// <param name="getOrdersByShippingAddressRequest">A <see cref="GetOrdersByShippingAddressRequest"/> representing the Orders By Shipping Address Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{OrderResponse}}"/> representing the Orders found.</returns>
        [HttpGet("ByShippingAddressId")]
        public ActionResult<PagedResponse<OrderResponse>> GetOrdersByShippingAddress([FromQuery] GetOrdersByShippingAddressRequest getOrdersByShippingAddressRequest)
        {
            if (getOrdersByShippingAddressRequest == null)
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
            var response = _orderRepository.GetOrdersByShippingAddress(getOrdersByShippingAddressRequest, routeName, Url);

            return Ok(response);
        }

        /// <summary>
        /// Handles POST requests to "api/Order/Create".
        /// Attempts to create a new Order.
        /// </summary>
        /// <param name="createOrderRequest">A <see cref="CreateOrderRequest"/> representing the new Order to be created.</param>
        /// <returns>An <see cref="ActionResult{OrderResponse}"/> representing the created Order.</returns>
        [HttpPost("Create")]
        public ActionResult<OrderResponse> CreateOrder([FromBody] CreateOrderRequest createOrderRequest)
        {
            if (createOrderRequest == null)
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
            var customer = _customerRepository.GetById(new GetCustomerRequest { CustomerId = createOrderRequest.CustomerId });
            if (customer == null)
            {
                // A Customer doesn't exist for the given Customer Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Customer Id is invalid.");
            }

            // Check that the associated Billing Address Exists.
            var billingAddress = _addressRepository.GetById(new GetAddressRequest { AddressId = createOrderRequest.BillingAddressId });
            if (billingAddress == null)
            {
                // A Billing Address doesn't exist for the given Billing Address Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Billing Address Id is invalid.");
            }

            // Check that the associated Shipping Address Exists.
            var shippingAddress = _addressRepository.GetById(new GetAddressRequest { AddressId = createOrderRequest.ShippingAddressId });
            if (shippingAddress == null)
            {
                // A Shipping Address doesn't exist for the given Shipping Address Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Shipping Address Id is invalid.");
            }

            // Map the Order entity to a new response object.
            var orderResponse = _orderRepository.CreateOrder(createOrderRequest);

            if (orderResponse == null)
            {
                return BadRequest();
            }

            // Return the created Order with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { orderId = orderResponse.OrderId }, orderResponse);
        }

        /// <summary>
        /// Handles PUT requests to "api/Order/Update".
        /// Attempts to update an existing Order.
        /// </summary>
        /// <param name="orderId">A <see cref="long"/> representing the Id of the Order to update.</param>
        /// <param name="updateOrderRequest">An <see cref="UpdateOrderRequest"/> representing the updated Order data.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.</returns>
        [HttpPut("Update")]
        public IActionResult UpdateOrder(long orderId, [FromBody] UpdateOrderRequest updateOrderRequest)
        {
            if (updateOrderRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            if (updateOrderRequest.OrderId != orderId)
            {
                // The orderId does not match the Order. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Order Id mismatch.");
            }

            // Attempt to get the Order to be updated.
            var existingOrder = _orderRepository.GetById(new GetOrderRequest { OrderId = orderId });
            if (existingOrder == null)
            {
                // The Order to be updated does not exist. Therefore, return a 404 'Not Found' response.
                return NotFound();
            }

            // Validate the Request.
            if (!ModelState.IsValid)
            {
                // The Model State is invalid. Therefore, return a 400 'Bad Request' response with validation errors.
                return BadRequest(ModelState);
            }

            // Check that the associated Customer Exists.
            var customer = _customerRepository.GetById(new GetCustomerRequest { CustomerId = updateOrderRequest.CustomerId });
            if (customer == null)
            {
                // A Customer doesn't for the given Customer Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Customer Id is invalid.");
            }

            // Check that the associated Billing Address Exists.
            var billingAddress = _addressRepository.GetById(new GetAddressRequest { AddressId = updateOrderRequest.BillingAddressId });
            if (billingAddress == null)
            {
                // A Billing Address doesn't exist for the given Billing Address Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Billing Address Id is invalid.");
            }

            // Check that the associated Shipping Address Exists.
            var shippingAddress = _addressRepository.GetById(new GetAddressRequest { AddressId = updateOrderRequest.ShippingAddressId });
            if (shippingAddress == null)
            {
                // A Shipping Address doesn't exist for the given Shipping Address Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Shipping Address Id is invalid.");
            }

            _orderRepository.UpdateOrder(orderId, updateOrderRequest);

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}