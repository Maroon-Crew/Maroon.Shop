using Maroon.Shop.Api.Requests;
using Maroon.Shop.Api.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maroon.Shop.Api.Controllers
{
    public class GetOrderByCustomerRequest
    {
        public long CustomerId { get; set; }
    }

    /// <summary>
    /// Order Controller Class, inherits from <see cref="Controller"/>.
    /// Handles requests routed to "api/[controller]", where [controller] is replaced by the name of the controller, in this case, "Order".
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises the Order Controller.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Data Context.</param>
        public OrderController(ShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles GET requests to "api/Order/{orderId}".
        /// Attempts to retrieve an Order for the given Order Id.
        /// </summary>
        /// <param name="getOrderRequest">A <see cref="GetOrderRequest"/> representing the Order requested.</param>
        /// <returns>An <see cref="ActionResult{OrderResponse}"/> representing the Basket found.</returns>
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

            // Query for an Order with the given OrderId.
            var query = _context.Orders
                .Include(order => order.Customer)
                .Include(order => order.BillingAddress)
                .Include(order => order.ShippingAddress)
                .Where(order => order.OrderId == getOrderRequest.OrderId);

            if (!query.Any())
            {
                // The Order could not be found, return a 404 'Not Found' response.
                return NotFound();
            }
            else
            {
                var order = query.First();
                var orderResponse = new OrderResponse
                {
                    OrderId = order.OrderId,
                    CustomerId = order.Customer.CustomerId,
                    TotalPrice = order.TotalPrice,
                    DateCreated = order.DateCreated,
                    BillingAddressId = order.BillingAddress.AddressId,
                    ShippingAddressId = order.ShippingAddress.AddressId
                };

                return Ok(orderResponse);
            }
        }

        /// <summary>
        /// Handles GET requests to "api/Order/All".
        /// Attempts to retrieve all Orders.
        /// </summary>
        /// <returns>An <see cref="ActionResult{IEnumerable{Order}}"/> representing the Orders found.</returns>
        [HttpGet("All")]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            // Return all Orders.
            return Ok(_context.Orders.Include(o => o.Customer));
        }

        /// <summary>
        /// Handles GET requests to "api/Order/".
        /// Attempts to retrieve all Orders.
        /// <param name="getOrdersRequest">A <see cref="GetOrdersRequest"/> representing the Orders to get.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{Order}}"/> representing the Orders found.</returns>
        [HttpGet]
        public ActionResult<PagedResponse<Order>> GetOrders([FromQuery] GetOrdersRequest getOrdersRequest)
        {
            // Retrieve all Orders using pagination.
            var orders = _context.Orders
                .Include(order => order.Customer)
                .Include(order => order.BillingAddress)
                .Include(order => order.ShippingAddress)
                .OrderBy(order => order.OrderId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((getOrdersRequest.PageNumber - 1) * getOrdersRequest.PageSize)
                .Take(getOrdersRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.Orders.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var pagedOrderResponse = new PagedResponse<Order>(
                 data: orders,
                 pageNumber: getOrdersRequest.PageNumber,
                 pageSize: getOrdersRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName
             );

            return Ok(pagedOrderResponse);
        }

        /// <summary>
        /// Handles GET requests to "api/Order/ByCustomerId".
        /// Attempts to retrieve all Orders for the given customerId.
        /// </summary>
        /// <param name="customerId">A <see cref="long"/> representing the Id of the Customer.</param>
        /// <returns>An <see cref="ActionResult{IEnumerable{Order}}"/> representing the Orders found.</returns>
        [HttpGet()]
        [Route("ByCustomerId/{CustomerId}")]
        public ActionResult<IEnumerable<Order>> GetOrdersByCustomer([FromRoute] GetOrderByCustomerRequest getOrderByCustomerRequest)
        {
            // Query for all Orders with the given customerId.
            var query = _context.Orders.Where(order => order.Customer.CustomerId == getOrderByCustomerRequest.CustomerId);

            return query.ToList();
        }

        /// <summary>
        /// Handles GET requests to "api/Order/ByCustomerId".
        /// Attempts to retrieve all Orders for the given customerId.
        /// </summary>
        /// <param name="getOrdersByCustomerRequest">A <see cref="GetOrdersByCustomerRequest"/> representing the Orders By Customer Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{Order}}"/> representing the Orders found.</returns>
        [HttpGet("ByCustomerId")]
        public ActionResult<PagedResponse<Order>> GetOrdersByCustomer([FromQuery] GetOrdersByCustomerRequest getOrdersByCustomerRequest)
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

            // Retrieve all Orders for the given Customer Id.
            var filteredOrders = _context.Orders
                .Include(order => order.Customer)
                .Include(order => order.BillingAddress)
                .Include(order => order.ShippingAddress)
                .Where(order => order.Customer.CustomerId == getOrdersByCustomerRequest.CustomerId);

            // Apply Pagination.
            var filteredOrdersPaginated = filteredOrders
                .Skip((getOrdersByCustomerRequest.PageNumber - 1) * getOrdersByCustomerRequest.PageSize)
                .Take(getOrdersByCustomerRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredOrders.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = new PagedResponse<Order>(
                 data: filteredOrdersPaginated,
                 pageNumber: getOrdersByCustomerRequest.PageNumber,
                 pageSize: getOrdersByCustomerRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName,
                 routeValues: new { getOrdersByCustomerRequest.CustomerId } // Pass in the CustomerId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return Ok(response);
        }

        /// <summary>
        /// Handles GET requests to "api/Order/ByBillingAddressId".
        /// Attempts to retrieve all Orders for the given billingAddressId.
        /// </summary>
        /// <param name="getOrdersByBillingAddressRequest">A <see cref="GetOrdersByBillingAddressRequest"/> representing the Orders By Billing Address Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{Order}}"/> representing the Orders found.</returns>
        [HttpGet("ByBillingAddressId")]
        public ActionResult<PagedResponse<Order>> GetOrdersByBillingAddress([FromQuery] GetOrdersByBillingAddressRequest getOrdersByBillingAddressRequest)
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

            // Retrieve all Orders for the given Billing Address Id.
            var filteredOrders = _context.Orders
                .Include(order => order.Customer)
                .Include(order => order.BillingAddress)
                .Include(order => order.ShippingAddress)
                .Where(order => order.BillingAddress.AddressId == getOrdersByBillingAddressRequest.BillingAddressId);

            // Apply Pagination.
            var filteredOrdersPaginated = filteredOrders
                .Skip((getOrdersByBillingAddressRequest.PageNumber - 1) * getOrdersByBillingAddressRequest.PageSize)
                .Take(getOrdersByBillingAddressRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredOrders.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = new PagedResponse<Order>(
                 data: filteredOrdersPaginated,
                 pageNumber: getOrdersByBillingAddressRequest.PageNumber,
                 pageSize: getOrdersByBillingAddressRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName,
                 routeValues: new { getOrdersByBillingAddressRequest.BillingAddressId } // Pass in the BillingAddressId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return Ok(response);
        }

        /// <summary>
        /// Handles GET requests to "api/Order/ByShippingAddressId".
        /// Attempts to retrieve all Orders for the given shippingAddressId.
        /// </summary>
        /// <param name="getOrdersByShippingAddressRequest">A <see cref="GetOrdersByShippingAddressRequest"/> representing the Orders By Shipping Address Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{Order}}"/> representing the Orders found.</returns>
        [HttpGet("ByShippingAddressId")]
        public ActionResult<PagedResponse<Order>> GetOrdersByShippingAddress([FromQuery] GetOrdersByShippingAddressRequest getOrdersByShippingAddressRequest)
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

            // Retrieve all Orders for the given Shipping Address Id.
            var filteredOrders = _context.Orders
                .Include(order => order.Customer)
                .Include(order => order.BillingAddress)
                .Include(order => order.ShippingAddress)
                .Where(order => order.ShippingAddress.AddressId == getOrdersByShippingAddressRequest.ShippingAddressId);

            // Apply Pagination.
            var filteredOrdersPaginated = filteredOrders
                .Skip((getOrdersByShippingAddressRequest.PageNumber - 1) * getOrdersByShippingAddressRequest.PageSize)
                .Take(getOrdersByShippingAddressRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredOrders.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = new PagedResponse<Order>(
                 data: filteredOrdersPaginated,
                 pageNumber: getOrdersByShippingAddressRequest.PageNumber,
                 pageSize: getOrdersByShippingAddressRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName,
                 routeValues: new { getOrdersByShippingAddressRequest.ShippingAddressId } // Pass in the ShippingAddressId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

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
            var customer = _context.Customers.FirstOrDefault(customer => customer.CustomerId == createOrderRequest.CustomerId);
            if (customer == null)
            {
                // A Customer doesn't exist for the given Customer Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Customer Id is invalid.");
            }

            // Check that the associated Billing Address Exists.
            var billingAddress = _context.Addresses.FirstOrDefault(address => address.AddressId == createOrderRequest.BillingAddressId);
            if (billingAddress == null)
            {
                // A Billing Address doesn't exist for the given Billing Address Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Billing Address Id is invalid.");
            }

            // Check that the associated Shipping Address Exists.
            var shippingAddress = _context.Addresses.FirstOrDefault(address => address.AddressId == createOrderRequest.ShippingAddressId);
            if (shippingAddress == null)
            {
                // A Shipping Address doesn't exist for the given Shipping Address Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Shipping Address Id is invalid.");
            }

            // Map the request object to a new Order entity.
            var newOrder = new Order
            {
                Customer = customer,
                TotalPrice = createOrderRequest.TotalPrice,
                DateCreated = createOrderRequest.DateCreated,
                BillingAddress = billingAddress,
                ShippingAddress = shippingAddress
            };

            // Add the new Order to the Database Context.
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            // Map the Order entity to a new response object.
            var orderResponse = new OrderResponse
            {
                OrderId = newOrder.OrderId,
                CustomerId = newOrder.Customer.CustomerId,
                TotalPrice = newOrder.TotalPrice,
                DateCreated = newOrder.DateCreated,
                BillingAddressId = newOrder.BillingAddress.AddressId,
                ShippingAddressId = newOrder.ShippingAddress.AddressId
            };

            // Return the created Basket with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { orderId = newOrder.OrderId }, orderResponse);
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
                return BadRequest("Basket Id mismatch.");
            }

            // Attempt to get the Order to be updated.
            var existingOrder = _context.Orders.FirstOrDefault(order => order.OrderId == orderId);
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
            var customer = _context.Customers.FirstOrDefault(customer => customer.CustomerId == updateOrderRequest.CustomerId);
            if (customer == null)
            {
                // A Customer doesn't for the given Customer Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Customer Id is invalid.");
            }

            // Check that the associated Billing Address Exists.
            var billingAddress = _context.Addresses.FirstOrDefault(address => address.AddressId == updateOrderRequest.BillingAddressId);
            if (billingAddress == null)
            {
                // A Billing Address doesn't exist for the given Billing Address Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Billing Address Id is invalid.");
            }

            // Check that the associated Shipping Address Exists.
            var shippingAddress = _context.Addresses.FirstOrDefault(address => address.AddressId == updateOrderRequest.ShippingAddressId);
            if (shippingAddress == null)
            {
                // A Shipping Address doesn't exist for the given Shipping Address Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Shipping Address Id is invalid.");
            }

            // Update the existing Order with the values from the provided Order.
            existingOrder.Customer = customer;
            existingOrder.TotalPrice = updateOrderRequest.TotalPrice;
            existingOrder.DateCreated = updateOrderRequest.DateCreated;
            existingOrder.BillingAddress = billingAddress;
            existingOrder.ShippingAddress = shippingAddress;

            // Save the changes to the database.
            _context.SaveChanges();

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}