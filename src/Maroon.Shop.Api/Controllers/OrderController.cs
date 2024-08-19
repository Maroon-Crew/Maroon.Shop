using Maroon.Shop.Data;
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
        /// <param name="orderId">A <see cref="long"/> representing the Id of the Order.</param>
        /// <returns>An <see cref="ActionResult{Order}"/> representing the Order found.</returns>
        [HttpGet]
        [Route("{orderId}")]
        public ActionResult<Order> GetById(long orderId)
        {
            // Query for a Order with the given orderId.
            var query = _context.Orders.Where(order => order.OrderId == orderId);

            if (!query.Any())
            {
                // The Order could not be found, return a 404 Not Found response.
                return NotFound();
            }
            else
            {
                // Return the first matching Order.
                return query.First();
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
            return _context.Orders;
        }

        /// <summary>
        /// Handles GET requests to "api/Order/ByCustomerId".
        /// Attempts to retrieve all Orders for the given customerId.
        /// </summary>
        /// <param name="customerId">A <see cref="long"/> representing the Id of the Customer.</param>
        /// <returns>An <see cref="ActionResult{IEnumerable{Order}}"/> representing the Orders found.</returns>
        [HttpGet("ByCustomerId")]
        public ActionResult<IEnumerable<Order>> GetOrdersByCustomer(long customerId)
        {
            // Query for all Orders with the given customerId.
            var query = _context.Orders.Where(order => order.Customer.CustomerId == (customerId));

            return query.ToList();
        }

        /// <summary>
        /// Handles GET requests to "api/Order/ByBillingAddressId".
        /// Attempts to retrieve all Orders for the given billingAddressId.
        /// </summary>
        /// <param name="billingAddressId">A <see cref="long"/> representing the Id of the Billing Address.</param>
        /// <returns>An <see cref="ActionResult{IEnumerable{Order}}"/> representing the Orders found.</returns>
        [HttpGet("ByBillingAddressId")]
        public ActionResult<IEnumerable<Order>> GetOrdersByBillingAddress(long billingAddressId)
        {
            // Query for all Orders with the given billingAddressId.
            var query = _context.Orders.Where(order => order.BillingAddressId == (billingAddressId));

            return query.ToList();
        }

        /// <summary>
        /// Handles GET requests to "api/Order/ByShippingAddressId".
        /// Attempts to retrieve all Orders for the given shippingAddressId.
        /// </summary>
        /// <param name="shippingAddressId">A <see cref="long"/> representing the Id of the Shipping Address.</param>
        /// <returns>An <see cref="ActionResult{IEnumerable{Order}}"/> representing the Orders found.</returns>
        [HttpGet("ByShippingAddressId")]
        public ActionResult<IEnumerable<Order>> GetOrdersByShippingAddress(long shippingAddressId)
        {
            // Query for all Orders with the given shippingAddressId.
            var query = _context.Orders.Where(order => order.ShippingAddressId == (shippingAddressId));
            
            return query.ToList();
        }
    }
}