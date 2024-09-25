using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maroon.Shop.Api.Data.Repositories
{
    public class BasketRepository
    {
        private readonly ShopContext _context;

        public BasketRepository(ShopContext context)
        {
            _context = context;
        }

        public BasketResponse? GetById(GetBasketRequest getBasketRequest)
        {
            // Query for a Basket with the given basketId.
            var query = _context.Baskets
                .Include(basket => basket.Customer)
                .Where(basket => basket.BasketId == getBasketRequest.BasketId);

            if (!query.Any())
            {
                // The Basket could not be found, return a 404 'Not Found' response.
                return null;
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

                return basketResponse;
            }
        }

        public PagedResponse<BasketResponse> GetBaskets(GetBasketsRequest getBasketsRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Baskets using pagination.
            var baskets = _context.Baskets
                .Include(basket => basket.Customer)
                .OrderBy(basket => basket.BasketId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((getBasketsRequest.PageNumber - 1) * getBasketsRequest.PageSize)
                .Take(getBasketsRequest.PageSize)
                .Select(b => new BasketResponse
                {
                    BasketId = b.BasketId,
                    CustomerId = b.Customer.CustomerId,
                    TotalPrice = b.TotalPrice,
                })
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.Baskets.Count();

            // Create the response.
            var pagedProductResponse = new PagedResponse<BasketResponse>(
                 data: baskets,
                 pageNumber: getBasketsRequest.PageNumber,
                 pageSize: getBasketsRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName
             );

            return pagedProductResponse;
        }

        public PagedResponse<BasketResponse> GetBasketsByCustomer(GetBasketsByCustomerRequest getBasketsByCustomerRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Baskets for the given Customer Id.
            var filteredBaskets = _context.Baskets
                .Include(basket => basket.Customer)
                .Where(basket => basket.Customer.CustomerId == getBasketsByCustomerRequest.CustomerId)
                .Select(b => new BasketResponse
                {
                    BasketId = b.BasketId,
                    CustomerId = b.Customer.CustomerId,
                    TotalPrice = b.TotalPrice,
                });

            // Apply Pagination.
            var filteredBasketsPaginated = filteredBaskets
                .Skip((getBasketsByCustomerRequest.PageNumber - 1) * getBasketsByCustomerRequest.PageSize)
                .Take(getBasketsByCustomerRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredBaskets.Count();

            // Create the response.
            var response = new PagedResponse<BasketResponse>(
                 data: filteredBasketsPaginated,
                 pageNumber: getBasketsByCustomerRequest.PageNumber,
                 pageSize: getBasketsByCustomerRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName,
                 routeValues: new { getBasketsByCustomerRequest.CustomerId } // Pass in the CustomerId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return response;
        }

        public BasketResponse? CreateBasket(CreateBasketRequest createBasketRequest)
        {
            // Check that the associated Customer Exists.
            var customer = _context.Customers.FirstOrDefault(customer => customer.CustomerId == createBasketRequest.CustomerId);
            if (customer == null)
            {
                return null;
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
            return basketResponse;
        }

        public void UpdateBasket(long basketId, UpdateBasketRequest updateBasketRequest)
        {
            // Attempt to get the Basket to be updated.
            var existingBasket = _context.Baskets.FirstOrDefault(basket => basket.BasketId == basketId);
            if (existingBasket == null)
            {
                // The Basket to be updated does not exist. Therefore, return a 404 'Not Found' response.
                return;
            }

            // Check that the associated Customer Exists.
            var customer = _context.Customers.FirstOrDefault(customer => customer.CustomerId == updateBasketRequest.CustomerId);
            if (customer == null)
            {
                // A Customer doesn't for the given Customer Id. Therefore, return a 400 'Bad Request' response.
                return;
            }

            // Update the existing Basket with the values from the provided Basket.
            existingBasket.Customer = customer;
            existingBasket.TotalPrice = updateBasketRequest.TotalPrice;

            // Save the changes to the database.
            _context.SaveChanges();
        }
    }
}