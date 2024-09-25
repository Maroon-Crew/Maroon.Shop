using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maroon.Shop.Api.Data.Repositories
{
    public class OrderItemRepository
    {
        private readonly ShopContext _context;

        public OrderItemRepository(ShopContext context)
        {
            _context = context;
        }

        public OrderItemResponse? GetById(GetOrderItemRequest getOrderItemRequest)
        {
            // Query for an Order Item with the given OrderItemId.
            var query = _context.OrderItems
                .Include(orderItem => orderItem.Order)
                .Include(orderItem => orderItem.Product)
                .Where(orderItem => orderItem.OrderItemId == getOrderItemRequest.OrderItemId);

            if (!query.Any())
            {
                // The Order Item could not be found, return a 404 'Not Found' response.
                return null;
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

                return orderItemResponse;
            }
        }

        public PagedResponse<OrderItemResponse> GetOrderItems(GetOrderItemsRequest getOrderItemsRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Order Items using pagination.
            var orderItems = _context.OrderItems
                .Include(orderItem => orderItem.Order)
                .Include(orderItem => orderItem.Product)
                .OrderBy(orderItem => orderItem.OrderItemId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((getOrderItemsRequest.PageNumber - 1) * getOrderItemsRequest.PageSize)
                .Take(getOrderItemsRequest.PageSize)
                .Select(oi => new OrderItemResponse
                {
                    TotalPrice = oi.TotalPrice,
                    OrderId = oi.Order.OrderId,
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.Product.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                })
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.OrderItems.Count();

            // Create the response.
            var pagedOrderResponse = new PagedResponse<OrderItemResponse>(
                 data: orderItems,
                 pageNumber: getOrderItemsRequest.PageNumber,
                 pageSize: getOrderItemsRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName
             );

            return pagedOrderResponse;
        }

        public PagedResponse<OrderItemResponse> GetOrderItemsByOrder(GetOrderItemsByOrderRequest getOrderItemsByOrderRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Order Items for the given Order Id.
            var filteredOrderItems = _context.OrderItems
                .Include(orderItem => orderItem.Order)
                .Include(orderItem => orderItem.Product)
                .Where(orderItem => orderItem.Order.OrderId == getOrderItemsByOrderRequest.OrderId)
                .Select(oi => new OrderItemResponse
                {
                    TotalPrice = oi.TotalPrice,
                    OrderId = oi.Order.OrderId,
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.Product.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                });

            // Apply Pagination.
            var filteredOrderItemsPaginated = filteredOrderItems
                .Skip((getOrderItemsByOrderRequest.PageNumber - 1) * getOrderItemsByOrderRequest.PageSize)
                .Take(getOrderItemsByOrderRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredOrderItems.Count();

            // Create the response.
            var response = new PagedResponse<OrderItemResponse>(
                 data: filteredOrderItemsPaginated,
                 pageNumber: getOrderItemsByOrderRequest.PageNumber,
                 pageSize: getOrderItemsByOrderRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName,
                 routeValues: new { getOrderItemsByOrderRequest.OrderId } // Pass in the OrderId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return response;
        }

        public PagedResponse<OrderItemResponse> GetOrderItemsByProduct(GetOrderItemsByProductRequest getOrderItemsByProductRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Order Items for the given Product Id.
            var filteredOrderItems = _context.OrderItems
                .Include(orderItem => orderItem.Order)
                .Include(orderItem => orderItem.Product)
                .Where(orderItem => orderItem.Product.ProductId == getOrderItemsByProductRequest.ProductId)
                .Select(oi => new OrderItemResponse
                {
                    TotalPrice = oi.TotalPrice,
                    OrderId = oi.Order.OrderId,
                    OrderItemId = oi.OrderItemId,
                    ProductId = oi.Product.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                });

            // Apply Pagination.
            var filteredOrderItemsPaginated = filteredOrderItems
                .Skip((getOrderItemsByProductRequest.PageNumber - 1) * getOrderItemsByProductRequest.PageSize)
                .Take(getOrderItemsByProductRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredOrderItems.Count();

            // Create the response.
            var response = new PagedResponse<OrderItemResponse>(
                 data: filteredOrderItemsPaginated,
                 pageNumber: getOrderItemsByProductRequest.PageNumber,
                 pageSize: getOrderItemsByProductRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName,
                 routeValues: new { getOrderItemsByProductRequest.ProductId } // Pass in the ProductId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return response;
        }

        public OrderItemResponse? CreateOrderItem(CreateOrderItemRequest createOrderItemRequest)
        {
            // Check that the associated Order Exists.
            var order = _context.Orders.FirstOrDefault(order => order.OrderId == createOrderItemRequest.OrderId);
            if (order == null)
            {
                // An Order doesn't exist for the given Order Id. Therefore, return a 400 'Bad Request' response.
                return null;
            }

            // Check that the associated Product Exists.
            var product = _context.Products.FirstOrDefault(product => product.ProductId == createOrderItemRequest.ProductId);
            if (product == null)
            {
                // A Product doesn't exist for the given Product Id. Therefore, return a 400 'Bad Request' response.
                return null;
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
            return orderItemResponse;
        }

        public void UpdateOrderItem(long orderItemId, UpdateOrderItemRequest updateOrderItemRequest)
        {
            // Attempt to get the Order Item to be updated.
            var existingOrderItem = _context.OrderItems.FirstOrDefault(orderItem => orderItem.OrderItemId == orderItemId);
            if (existingOrderItem == null)
            {
                // The Order Item to be updated does not exist. Therefore, return a 404 'Not Found' response.
                return;
            }

            // Check that the associated Order Exists.
            var order = _context.Orders.FirstOrDefault(order => order.OrderId == updateOrderItemRequest.OrderId);
            if (order == null)
            {
                // An Order doesn't exist for the given Order Id. Therefore, return a 400 'Bad Request' response.
                return;
            }

            // Check that the associated Product Exists.
            var product = _context.Products.FirstOrDefault(product => product.ProductId == updateOrderItemRequest.ProductId);
            if (product == null)
            {
                // A Product doesn't exist for the given Product Id. Therefore, return a 400 'Bad Request' response.
                return;
            }

            // Update the existing Order with the values from the provided Order.
            existingOrderItem.Order = order;
            existingOrderItem.Product = product;
            existingOrderItem.Quantity = updateOrderItemRequest.Quantity;
            existingOrderItem.UnitPrice = updateOrderItemRequest.UnitPrice;
            existingOrderItem.TotalPrice = updateOrderItemRequest.TotalPrice;

            // Save the changes to the database.
            _context.SaveChanges();
        }
    }
}