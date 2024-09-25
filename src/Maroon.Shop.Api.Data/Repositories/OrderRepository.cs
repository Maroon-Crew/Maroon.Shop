using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maroon.Shop.Api.Data.Repositories
{
    public class OrderRepository
    {
        private readonly ShopContext _context;

        public OrderRepository(ShopContext context)
        {
            _context = context;
        }

        public OrderResponse? GetById(GetOrderRequest getOrderRequest)
        {
            // Query for an Order with the given OrderId.
            var query = _context.Orders
                .Include(order => order.Customer)
                .Include(order => order.BillingAddress)
                .Include(order => order.ShippingAddress)
                .Where(order => order.OrderId == getOrderRequest.OrderId);

            if (!query.Any())
            {
                // The Order could not be found, return a 404 'Not Found' response.
                return null;
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

                return orderResponse;
            }
        }

        public PagedResponse<OrderResponse> GetOrders(GetOrdersRequest getOrdersRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Orders using pagination.
            var orders = _context.Orders
                .Include(order => order.Customer)
                .Include(order => order.BillingAddress)
                .Include(order => order.ShippingAddress)
                .OrderBy(order => order.OrderId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((getOrdersRequest.PageNumber - 1) * getOrdersRequest.PageSize)
                .Take(getOrdersRequest.PageSize)
                .Select(o => new OrderResponse
                {
                    BillingAddressId = o.BillingAddress.AddressId,
                    CustomerId = o.Customer.CustomerId,
                    DateCreated = o.DateCreated,
                    OrderId = o.OrderId,
                    ShippingAddressId = o.ShippingAddress.AddressId,
                    TotalPrice = o.TotalPrice,
                })
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.Orders.Count();

            // Create the response.
            var pagedOrderResponse = new PagedResponse<OrderResponse>(
                 data: orders,
                 pageNumber: getOrdersRequest.PageNumber,
                 pageSize: getOrdersRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName
             );

            return pagedOrderResponse;
        }

        public IEnumerable<OrderResponse> GetOrdersByCustomer(GetOrderByCustomerRequest getOrderByCustomerRequest)
        {
            // Query for all Orders with the given customerId.
            var query = _context.Orders.Where(order => order.Customer.CustomerId == getOrderByCustomerRequest.CustomerId)
                .Select(o => new OrderResponse
                {
                    BillingAddressId = o.BillingAddress.AddressId,
                    CustomerId = o.Customer.CustomerId,
                    DateCreated = o.DateCreated,
                    OrderId = o.OrderId,
                    ShippingAddressId = o.ShippingAddress.AddressId,
                    TotalPrice = o.TotalPrice,
                });

            return query.ToList();
        }

        public PagedResponse<OrderResponse> GetOrdersByCustomer(GetOrdersByCustomerRequest getOrdersByCustomerRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Orders for the given Customer Id.
            var filteredOrders = _context.Orders
                .Include(order => order.Customer)
                .Include(order => order.BillingAddress)
                .Include(order => order.ShippingAddress)
                .Where(order => order.Customer.CustomerId == getOrdersByCustomerRequest.CustomerId)
                .Select(o => new OrderResponse
                {
                    BillingAddressId = o.BillingAddress.AddressId,
                    CustomerId = o.Customer.CustomerId,
                    DateCreated = o.DateCreated,
                    OrderId = o.OrderId,
                    ShippingAddressId = o.ShippingAddress.AddressId,
                    TotalPrice = o.TotalPrice,
                });

            // Apply Pagination.
            var filteredOrdersPaginated = filteredOrders
                .Skip((getOrdersByCustomerRequest.PageNumber - 1) * getOrdersByCustomerRequest.PageSize)
                .Take(getOrdersByCustomerRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredOrders.Count();

            // Create the response.
            var response = new PagedResponse<OrderResponse>(
                 data: filteredOrdersPaginated,
                 pageNumber: getOrdersByCustomerRequest.PageNumber,
                 pageSize: getOrdersByCustomerRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName,
                 routeValues: new { getOrdersByCustomerRequest.CustomerId } // Pass in the CustomerId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return response;
        }

        public PagedResponse<OrderResponse> GetOrdersByBillingAddress(GetOrdersByBillingAddressRequest getOrdersByBillingAddressRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Orders for the given Billing Address Id.
            var filteredOrders = _context.Orders
                .Include(order => order.Customer)
                .Include(order => order.BillingAddress)
                .Include(order => order.ShippingAddress)
                .Where(order => order.BillingAddress.AddressId == getOrdersByBillingAddressRequest.BillingAddressId)
                .Select(o => new OrderResponse
                {
                    BillingAddressId = o.BillingAddress.AddressId,
                    CustomerId = o.Customer.CustomerId,
                    DateCreated = o.DateCreated,
                    OrderId = o.OrderId,
                    ShippingAddressId = o.ShippingAddress.AddressId,
                    TotalPrice = o.TotalPrice,
                });

            // Apply Pagination.
            var filteredOrdersPaginated = filteredOrders
                .Skip((getOrdersByBillingAddressRequest.PageNumber - 1) * getOrdersByBillingAddressRequest.PageSize)
                .Take(getOrdersByBillingAddressRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredOrders.Count();

            // Create the response.
            var response = new PagedResponse<OrderResponse>(
                 data: filteredOrdersPaginated,
                 pageNumber: getOrdersByBillingAddressRequest.PageNumber,
                 pageSize: getOrdersByBillingAddressRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName,
                 routeValues: new { getOrdersByBillingAddressRequest.BillingAddressId } // Pass in the BillingAddressId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return response;
        }

        public PagedResponse<OrderResponse> GetOrdersByShippingAddress(GetOrdersByShippingAddressRequest getOrdersByShippingAddressRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Orders for the given Shipping Address Id.
            var filteredOrders = _context.Orders
                .Include(order => order.Customer)
                .Include(order => order.BillingAddress)
                .Include(order => order.ShippingAddress)
                .Where(order => order.ShippingAddress.AddressId == getOrdersByShippingAddressRequest.ShippingAddressId)
                .Select(o => new OrderResponse
                {
                    BillingAddressId = o.BillingAddress.AddressId,
                    CustomerId = o.Customer.CustomerId,
                    DateCreated = o.DateCreated,
                    OrderId = o.OrderId,
                    ShippingAddressId = o.ShippingAddress.AddressId,
                    TotalPrice = o.TotalPrice,
                });

            // Apply Pagination.
            var filteredOrdersPaginated = filteredOrders
                .Skip((getOrdersByShippingAddressRequest.PageNumber - 1) * getOrdersByShippingAddressRequest.PageSize)
                .Take(getOrdersByShippingAddressRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredOrders.Count();

            // Create the response.
            var response = new PagedResponse<OrderResponse>(
                 data: filteredOrdersPaginated,
                 pageNumber: getOrdersByShippingAddressRequest.PageNumber,
                 pageSize: getOrdersByShippingAddressRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName,
                 routeValues: new { getOrdersByShippingAddressRequest.ShippingAddressId } // Pass in the ShippingAddressId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return response;
        }

        public OrderResponse? CreateOrder(CreateOrderRequest createOrderRequest)
        {
            // Check that the associated Customer Exists.
            var customer = _context.Customers.FirstOrDefault(customer => customer.CustomerId == createOrderRequest.CustomerId);
            if (customer == null)
            {
                // A Customer doesn't exist for the given Customer Id. Therefore, return a 400 'Bad Request' response.
                return null;
            }

            // Check that the associated Billing Address Exists.
            var billingAddress = _context.Addresses.FirstOrDefault(address => address.AddressId == createOrderRequest.BillingAddressId);
            if (billingAddress == null)
            {
                // A Billing Address doesn't exist for the given Billing Address Id. Therefore, return a 400 'Bad Request' response.
                return null;
            }

            // Check that the associated Shipping Address Exists.
            var shippingAddress = _context.Addresses.FirstOrDefault(address => address.AddressId == createOrderRequest.ShippingAddressId);
            if (shippingAddress == null)
            {
                // A Shipping Address doesn't exist for the given Shipping Address Id. Therefore, return a 400 'Bad Request' response.
                return null;
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

            // Return the created Order with a 201 'Created' response.
            return orderResponse;
        }

        public void UpdateOrder(long orderId, UpdateOrderRequest updateOrderRequest)
        {
            // Attempt to get the Order to be updated.
            var existingOrder = _context.Orders.FirstOrDefault(order => order.OrderId == orderId);
            if (existingOrder == null)
            {
                // The Order to be updated does not exist. Therefore, return a 404 'Not Found' response.
                return;
            }

            // Check that the associated Customer Exists.
            var customer = _context.Customers.FirstOrDefault(customer => customer.CustomerId == updateOrderRequest.CustomerId);
            if (customer == null)
            {
                // A Customer doesn't for the given Customer Id. Therefore, return a 400 'Bad Request' response.
                return;
            }

            // Check that the associated Billing Address Exists.
            var billingAddress = _context.Addresses.FirstOrDefault(address => address.AddressId == updateOrderRequest.BillingAddressId);
            if (billingAddress == null)
            {
                // A Billing Address doesn't exist for the given Billing Address Id. Therefore, return a 400 'Bad Request' response.
                return;
            }

            // Check that the associated Shipping Address Exists.
            var shippingAddress = _context.Addresses.FirstOrDefault(address => address.AddressId == updateOrderRequest.ShippingAddressId);
            if (shippingAddress == null)
            {
                // A Shipping Address doesn't exist for the given Shipping Address Id. Therefore, return a 400 'Bad Request' response.
                return;
            }

            // Update the existing Order with the values from the provided Order.
            existingOrder.Customer = customer;
            existingOrder.TotalPrice = updateOrderRequest.TotalPrice;
            existingOrder.DateCreated = updateOrderRequest.DateCreated;
            existingOrder.BillingAddress = billingAddress;
            existingOrder.ShippingAddress = shippingAddress;

            // Save the changes to the database.
            _context.SaveChanges();
        }
    }
}