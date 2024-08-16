using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Api.Controllers
{
    /// <summary>
    /// Order Item Controller Class, inherits from <see cref="Controller"/>.
    /// Handles requests routed to "api/[controller]", where [controller] is replaced by the name of the controller, in this case, "OrderItem".
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : Controller
    {
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises the Order Item Controller.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Data Context.</param>
        public OrderItemController(ShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles GET requests to "api/OrderItem/{orderItemId}".
        /// Attempts to retrieve an Order for the given Order Id.
        /// </summary>
        /// <param name="orderItemId">A <see cref="long"/> representing the Id of the Order item.</param>
        /// <returns>An <see cref="ActionResult{OrderItem}"/> representing the Order Item found.</returns>
        [HttpGet]
        [Route("{orderItemId}")]
        public ActionResult<OrderItem> GetById(long orderItemId)
        {
            // Query for a Order Item with the given orderItemId.
            var query = _context.OrderItems.Where(orderItem => orderItem.OrderItemId == orderItemId);

            if (!query.Any())
            {
                // The Order Item could not be found, return a 404 Not Found response.
                return NotFound();
            }
            else
            {
                // Return the first matching Order Item.
                return query.First();
            }
        }

        /// <summary>
        /// Handles GET requests to "api/OrderItem/All".
        /// Attempts to retrieve all Order Items.
        /// </summary>
        /// <returns>An <see cref="ActionResult{IEnumerable{OrderItem}}"/> representing the Order Items found.</returns>
        [HttpGet("All")]
        public ActionResult<IEnumerable<OrderItem>> GetOrderItems()
        {
            // Return all Order Items.
            return _context.OrderItems;
        }

        /// <summary>
        /// Handles GET requests to "api/OrderItem/ByOrderId".
        /// Attempts to retrieve all Order Items for the given orderId.
        /// </summary>
        /// <param name="orderId">A <see cref="long"/> representing the Id of the Order.</param>
        /// <returns>An <see cref="ActionResult{IEnumerable{OrderItem}}"/> representing the Order Items found.</returns>
        [HttpGet("ByOrderId")]
        public ActionResult<IEnumerable<OrderItem>> GetOrderItemsByOrder(long orderId)
        {
            // Query for all Order Items with the given orderId.
            var query = _context.OrderItems.Where(orderItem => orderItem.Order.OrderId == (orderId));

            return query.ToList();
        }

        /// <summary>
        /// Handles GET requests to "api/OrderItem/ByProductId".
        /// Attempts to retrieve all Order Items for the given productId.
        /// </summary>
        /// <param name="productId">A <see cref="long"/> representing the Id of the Product.</param>
        /// <returns>An <see cref="ActionResult{IEnumerable{OrderItem}}"/> representing the Order Items found.</returns>
        [HttpGet("ByProductId")]
        public ActionResult<IEnumerable<OrderItem>> GetOrderItemsByProduct(long productId)
        {
            // Query for all Order Items with the given productId.
            var query = _context.OrderItems.Where(orderItem => orderItem.Product.ProductId == (productId));

            return query.ToList();
        }
    }
}