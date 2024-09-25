using Maroon.Shop.Api.Data.Repositories;
using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
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
        private readonly OrderItemRepository _orderItemRepository;
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;

        public OrderItemController(OrderItemRepository orderItemRepository, OrderRepository orderRepository, ProductRepository productRepository)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
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

            var orderItemResponse = _orderItemRepository.GetById(getOrderItemRequest);

            if (orderItemResponse == null)
            {
                // The Order Item could not be found, return a 404 'Not Found' response.
                return NotFound();
            }
            else
            {
                return Ok(orderItemResponse);
            }
        }

        /// <summary>
        /// Handles GET requests to "api/OrderItem/".
        /// Attempts to retrieve all Order Items.
        /// <param name="getOrderItemsRequest">A <see cref="GetOrderItemsRequest"/> representing the Order Items to get.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{OrderItemResponse}}"/> representing the Order Items found.</returns>
        [HttpGet]
        public ActionResult<PagedResponse<OrderItemResponse>> GetOrderItems([FromQuery] GetOrderItemsRequest getOrderItemsRequest)
        {
            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var pagedOrderResponse = _orderItemRepository.GetOrderItems(getOrderItemsRequest, routeName, Url);

            return Ok(pagedOrderResponse);
        }

        /// <summary>
        /// Handles GET requests to "api/OrderItem/ByOrderId".
        /// Attempts to retrieve all Order Items for the given orderId.
        /// </summary>
        /// <param name="getOrderItemsByOrderRequest">A <see cref="GetOrderItemsByOrderRequest"/> representing the Order Items By Order Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{OrderItemResponse}}"/> representing the Order Items found.</returns>
        [HttpGet("ByOrderId")]
        public ActionResult<PagedResponse<OrderItemResponse>> GetOrderItemsByOrder([FromQuery] GetOrderItemsByOrderRequest getOrderItemsByOrderRequest)
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

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = _orderItemRepository.GetOrderItemsByOrder(getOrderItemsByOrderRequest, routeName, Url);

            return Ok(response);
        }

        /// <summary>
        /// Handles GET requests to "api/OrderItem/ByProductId".
        /// Attempts to retrieve all Order Items for the given productId.
        /// </summary>
        /// <param name="getOrderItemsByProductRequest">A <see cref="GetOrderItemsByProductRequest"/> representing the Order Items By Product Request.</param>
        /// <returns>An <see cref="ActionResult{PagedResponse{OrderItemResponse}}"/> representing the Order Items found.</returns>
        [HttpGet("ByProductId")]
        public ActionResult<PagedResponse<OrderItemResponse>> GetOrderItemsByProduct([FromQuery] GetOrderItemsByProductRequest getOrderItemsByProductRequest)
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

            // Get the Route Name from the current action.
            var routeName = ControllerContext.ActionDescriptor.AttributeRouteInfo?.Name ?? string.Empty;

            // Create the response.
            var response = _orderItemRepository.GetOrderItemsByProduct(getOrderItemsByProductRequest, routeName, Url);

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
            var order = _orderRepository.GetById(new GetOrderRequest { OrderId = createOrderItemRequest.OrderId });
            if (order == null)
            {
                // An Order doesn't exist for the given Order Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Order Id is invalid.");
            }

            // Check that the associated Product Exists.
            var product = _productRepository.GetById(new GetProductRequest { ProductId = createOrderItemRequest.ProductId });
            if (product == null)
            {
                // A Product doesn't exist for the given Product Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Product Id is invalid.");
            }

            // Map the Order Item entity to a new response object.
            var orderItemResponse = _orderItemRepository.CreateOrderItem(createOrderItemRequest);

            if (orderItemResponse == null)
            {
                return BadRequest();
            }

            // Return the created Order Item with a 201 'Created' response.
            return CreatedAtAction(nameof(GetById), new { orderItemId = orderItemResponse.OrderItemId }, orderItemResponse);
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
            var existingOrderItem = _orderItemRepository.GetById(new GetOrderItemRequest { OrderItemId = orderItemId });
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
            var order = _orderRepository.GetById(new GetOrderRequest { OrderId = updateOrderItemRequest.OrderId });
            if (order == null)
            {
                // An Order doesn't exist for the given Order Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Order Id is invalid.");
            }

            // Check that the associated Product Exists.
            var product = _productRepository.GetById(new GetProductRequest { ProductId = updateOrderItemRequest.ProductId });
            if (product == null)
            {
                // A Product doesn't exist for the given Product Id. Therefore, return a 400 'Bad Request' response.
                return BadRequest("Product Id is invalid.");
            }

            _orderItemRepository.UpdateOrderItem(orderItemId, updateOrderItemRequest);

            // Return a 204 'No Content' response to indicate that the update was successful.
            return NoContent();
        }
    }
}