using Maroon.Shop.Api.Requests;
using Maroon.Shop.Api.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        /// Attempts to retrieve an Order Item for the given Orde Item Id.
        /// </summary>
        /// <param name="getOrderItemRequest">A <see cref="GetOrderItemRequest"/> representing the Order Item requested.</param>
        /// <returns>An <see cref="ActionResult{OrderItemResponse}"/> representing the Order Item found.</returns>
        [HttpGet]
        [Route("{OrderItemId}")]
        public ActionResult<OrderItemResponse> GetById([FromRoute] GetOrderItemRequest getOrderItemRequest)
        {
            if (getOrderItemRequest == null)
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

            // Query for an Order Item with the given OrderItemId.
            var query = _context.OrderItems
                .Include(orderItem => orderItem.Order)
                .Include(orderItem => orderItem.Product)
                .Where(orderItem => orderItem.OrderItemId == getOrderItemRequest.OrderItemId);

            if (!query.Any())
            {
                // The Order Item could not be found, return a 404 'Not Found' response.
                return NotFound();
            }
            else
            {
                var orderItem = query.First();
                var orderItemResponse = new OrderItemResponse
                {
                    OrderItemId = orderItem.OrderItemId,
                    OrderId = orderItem.Order.OrderId,
                    ProductId = orderItem.Product.ProductId,
                    Quantity = orderItem.Quantity,
                    UnitPrice = orderItem.UnitPrice,
                    TotalPrice = orderItem.TotalPrice
                };

                return Ok(orderItemResponse);
            }
        }

        /// <summary>
        /// Handles GET requests to "api/OrderItem/".
        /// Attempts to retrieve all Order Items.
        /// <param name="getOrderItemsRequest">A <see cref="GetOrderItemsRequest"/> representing the Order Items to get.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{OrderItem}}"/> representing the Order Items found.</returns>
        [HttpGet]
        public ActionResult<PagedResponse<OrderItem>> GetOrderItems([FromQuery] GetOrderItemsRequest getOrderItemsRequest)
        {
            // Retrieve all Order Items using pagination.
            var orderItems = _context.OrderItems
                .Include(orderItem => orderItem.Order)
                .Include(orderItem => orderItem.Product)
                .OrderBy(orderItem => orderItem.OrderItemId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((getOrderItemsRequest.PageNumber - 1) * getOrderItemsRequest.PageSize)
                .Take(getOrderItemsRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.OrderItems.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var pagedOrderResponse = new PagedResponse<OrderItem>(
                 data: orderItems,
                 pageNumber: getOrderItemsRequest.PageNumber,
                 pageSize: getOrderItemsRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName
             );

            return Ok(pagedOrderResponse);
        }

        /// <summary>
        /// Handles GET requests to "api/OrderItem/ByOrderId".
        /// Attempts to retrieve all Order Items for the given orderId.
        /// </summary>
        /// <param name="getOrderItemsByOrderRequest">A <see cref="GetOrderItemsByOrderRequest"/> representing the Order Items By Order Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{OrderItem}}"/> representing the Order Items found.</returns>
        [HttpGet("ByOrderId")]
        public ActionResult<PagedResponse<OrderItem>> GetOrderItemsByOrder([FromQuery] GetOrderItemsByOrderRequest getOrderItemsByOrderRequest)
        {
            if (getOrderItemsByOrderRequest == null)
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

            // Retrieve all Order Items for the given Order Id.
            var filteredOrderItems = _context.OrderItems
                .Include(orderItem => orderItem.Order)
                .Include(orderItem => orderItem.Product)
                .Where(orderItem => orderItem.Order.OrderId == getOrderItemsByOrderRequest.OrderId);

            // Apply Pagination.
            var filteredOrderItemsPaginated = filteredOrderItems
                .Skip((getOrderItemsByOrderRequest.PageNumber - 1) * getOrderItemsByOrderRequest.PageSize)
                .Take(getOrderItemsByOrderRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredOrderItems.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = new PagedResponse<OrderItem>(
                 data: filteredOrderItemsPaginated,
                 pageNumber: getOrderItemsByOrderRequest.PageNumber,
                 pageSize: getOrderItemsByOrderRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName,
                 routeValues: new { getOrderItemsByOrderRequest.OrderId } // Pass in the OrderId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return Ok(response);
        }

        /// <summary>
        /// Handles GET requests to "api/OrderItem/ByProductId".
        /// Attempts to retrieve all Order Items for the given productId.
        /// </summary>
        /// <param name="getOrderItemsByProductRequest">A <see cref="GetOrderItemsByProductRequest"/> representing the Order Items By Product Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{OrderItem}}"/> representing the Order Items found.</returns>
        [HttpGet("ByProductId")]
        public ActionResult<PagedResponse<OrderItem>> GetOrderItemsByProduct([FromQuery] GetOrderItemsByProductRequest getOrderItemsByProductRequest)
        {
            if (getOrderItemsByProductRequest == null)
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

            // Retrieve all Order Items for the given Product Id.
            var filteredOrderItems = _context.OrderItems
                .Include(orderItem => orderItem.Order)
                .Include(orderItem => orderItem.Product)
                .Where(orderItem => orderItem.Product.ProductId == getOrderItemsByProductRequest.ProductId);

            // Apply Pagination.
            var filteredOrderItemsPaginated = filteredOrderItems
                .Skip((getOrderItemsByProductRequest.PageNumber - 1) * getOrderItemsByProductRequest.PageSize)
                .Take(getOrderItemsByProductRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredOrderItems.Count();

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = new PagedResponse<OrderItem>(
                 data: filteredOrderItemsPaginated,
                 pageNumber: getOrderItemsByProductRequest.PageNumber,
                 pageSize: getOrderItemsByProductRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: Url,
                 routeName: routeName,
                 routeValues: new { getOrderItemsByProductRequest.ProductId } // Pass in the ProductId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return Ok(response);
        }

        /// <summary>
        /// Handles POST requests to "api/OrderItem/Create".
        /// Attempts to create a new Order Item.
        /// </summary>
        /// <param name="createOrderItemRequest">A <see cref="CreateOrderItemRequest"/> representing the new Order Item to be created.</param>
        /// <returns>An <see cref="ActionResult{OrderItemResponse}"/> representing the created Order Item.</returns>
        [HttpPost("Create")]
        public ActionResult<OrderItemResponse> CreateOrderItem([FromBody] CreateOrderItemRequest createOrderItemRequest)
        {
            if (createOrderItemRequest == null)
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

            // Check that the associated Order Exists.
            var order = _context.Orders.FirstOrDefault(order => order.OrderId == createOrderItemRequest.OrderId);
            if (order == null)
            {
                // An Order doesn't exist for the given Order Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Order Id is invalid.");
            }

            // Check that the associated Product Exists.
            var product = _context.Products.FirstOrDefault(product => product.ProductId == createOrderItemRequest.ProductId);
            if (product == null)
            {
                // A Product doesn't exist for the given Product Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Product Id is invalid.");
            }

            // Map the request object to a new Order Item entity.
            var newOrderItem = new OrderItem
            {
                Order = order,
                Product = product,
                Quantity = createOrderItemRequest.Quantity,
                UnitPrice = createOrderItemRequest.UnitPrice,
                TotalPrice = createOrderItemRequest.TotalPrice
            };

            // Add the new Order Item to the Database Context.
            _context.OrderItems.Add(newOrderItem);
            _context.SaveChanges();

            // Map the Order Item entity to a new response object.
            var orderItemResponse = new OrderItemResponse
            {
                OrderItemId = newOrderItem.OrderItemId,
                OrderId = newOrderItem.Order.OrderId,
                ProductId = newOrderItem.Product.ProductId,
                Quantity = newOrderItem.Quantity,
                UnitPrice = newOrderItem.UnitPrice,
                TotalPrice = newOrderItem.TotalPrice
            };

            // Return the created Order Item with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { orderItemId = newOrderItem.OrderItemId }, orderItemResponse);
        }

        /// <summary>
        /// Handles PUT requests to "api/OrderItem/Update".
        /// Attempts to update an existing Order Item.
        /// </summary>
        /// <param name="orderItemId">A <see cref="long"/> representing the Id of the Order Item to update.</param>
        /// <param name="updateOrderItemRequest">An <see cref="UpdateOrderItemRequest"/> representing the updated Order Item data.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the update operation.</returns>
        [HttpPut("Update")]
        public IActionResult UpdateOrderItem(long orderItemId, [FromBody] UpdateOrderItemRequest updateOrderItemRequest)
        {
            if (updateOrderItemRequest == null)
            {
                // The request is null. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Request cannot be null.");
            }

            if (updateOrderItemRequest.OrderItemId != orderItemId)
            {
                // The orderItemId does not match the Order Item. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Order Item Id mismatch.");
            }

            // Attempt to get the Order Item to be updated.
            var existingOrderItem = _context.OrderItems.FirstOrDefault(orderItem => orderItem.OrderItemId == orderItemId);
            if (existingOrderItem == null)
            {
                // The Order Item to be updated does not exist. Therefore, return a 404 'Not Found' response.
                return NotFound();
            }

            // Validate the Request.
            if (!ModelState.IsValid)
            {
                // The Model State is invalid. Therefore, return a 400 'Bad Request' response with validation errors.
                return BadRequest(ModelState);
            }

            // Check that the associated Order Exists.
            var order = _context.Orders.FirstOrDefault(order => order.OrderId == updateOrderItemRequest.OrderId);
            if (order == null)
            {
                // An Order doesn't exist for the given Order Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Order Id is invalid.");
            }

            // Check that the associated Product Exists.
            var product = _context.Products.FirstOrDefault(product => product.ProductId == updateOrderItemRequest.ProductId);
            if (product == null)
            {
                // A Product doesn't exist for the given Product Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Product Id is invalid.");
            }

            // Update the existing Order with the values from the provided Order.
            existingOrderItem.Order = order;
            existingOrderItem.Product = product;
            existingOrderItem.Quantity = updateOrderItemRequest.Quantity;
            existingOrderItem.UnitPrice = updateOrderItemRequest.UnitPrice;
            existingOrderItem.TotalPrice = updateOrderItemRequest.TotalPrice;

            // Save the changes to the database.
            _context.SaveChanges();

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}