using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maroon.Shop.Api.Data.Repositories
{
    public class BasketItemRepository
    {
        private readonly ShopContext _context;

        public BasketItemRepository(ShopContext context)
        {
            _context = context;
        }

        public BasketItemResponse? GetById(GetBasketItemRequest getBasketItemRequest)
        {
            // Query for a Basket Item with the given basketItemId.
            var query = _context.BasketItems
                .Include(basketItem => basketItem.Product)
                .Include(basketItem => basketItem.Basket)
                .Where(basketItem => basketItem.BasketItemId == getBasketItemRequest.BasketItemId);

            if (!query.Any())
            {
                // The Product could not be found, return a 404 'Not Found' response.
                return null;
            }
            else
            {
                var basketItem = query.First();
                var basketItemResponse = new BasketItemResponse
                {
                    BasketItemId = basketItem.BasketItemId,
                    BasketId = basketItem.Basket.BasketId,
                    ProductId = basketItem.Product.ProductId,
                    Quantity = basketItem.Quantity,
                    UnitPrice = basketItem.UnitPrice,
                    TotalPrice = basketItem.TotalPrice
                };

                return basketItemResponse;
            }
        }

        public PagedResponse<BasketItemResponse> GetBasketItems(GetBasketItemsRequest getBasketItemsRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Basket Items using pagination.
            var basketItems = _context.BasketItems
                .Include(basketItem => basketItem.Product)
                .Include(basketItem => basketItem.Basket)
                .OrderBy(basketItem => basketItem.BasketItemId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((getBasketItemsRequest.PageNumber - 1) * getBasketItemsRequest.PageSize)
                .Take(getBasketItemsRequest.PageSize)
                .Select(bi => new BasketItemResponse
                {
                    BasketId = bi.Basket.BasketId,
                    BasketItemId = bi.BasketItemId,
                    ProductId = bi.Product.ProductId,
                    Quantity = bi.Quantity,
                    TotalPrice = bi.TotalPrice,
                    UnitPrice = bi.UnitPrice,
                })
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.BasketItems.Count();

            // Create the response.
            var pagedProductResponse = new PagedResponse<BasketItemResponse>(
                 data: basketItems,
                 pageNumber: getBasketItemsRequest.PageNumber,
                 pageSize: getBasketItemsRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName
             );

            return pagedProductResponse;
        }

        public PagedResponse<BasketItemResponse> GetBasketItemsByBasket(GetBasketItemsByBasketRequest getBasketItemsByBasketRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Basket Itemss for the given Basket Id.
            var filteredBasketItems = _context.BasketItems
                .Include(basketItem => basketItem.Basket)
                .Where(basketItem => basketItem.Basket.BasketId == getBasketItemsByBasketRequest.BasketId)
                .Select(bi => new BasketItemResponse
                {
                    BasketId = bi.Basket.BasketId,
                    BasketItemId = bi.BasketItemId,
                    ProductId = bi.Product.ProductId,
                    Quantity = bi.Quantity,
                    TotalPrice = bi.TotalPrice,
                    UnitPrice = bi.UnitPrice,
                });

            // Apply Pagination.
            var filteredBasketItemsPaginated = filteredBasketItems
                .Skip((getBasketItemsByBasketRequest.PageNumber - 1) * getBasketItemsByBasketRequest.PageSize)
                .Take(getBasketItemsByBasketRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredBasketItems.Count();

            // Create the response.
            var response = new PagedResponse<BasketItemResponse>(
                 data: filteredBasketItemsPaginated,
                 pageNumber: getBasketItemsByBasketRequest.PageNumber,
                 pageSize: getBasketItemsByBasketRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName,
                 routeValues: new { getBasketItemsByBasketRequest.BasketId } // Pass in the BasketId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return response;
        }

        public PagedResponse<BasketItemResponse> GetBasketItemsByProduct(GetBasketItemsByProductRequest getBasketItemsByProductRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Basket Itemss for the given Basket Id.
            var filteredBasketItems = _context.BasketItems
                .Include(basketItem => basketItem.Product)
                .Where(basketItem => basketItem.Product.ProductId == getBasketItemsByProductRequest.ProductId)
                .Select(bi => new BasketItemResponse
                {
                    BasketId = bi.Basket.BasketId,
                    BasketItemId = bi.BasketItemId,
                    ProductId = bi.Product.ProductId,
                    Quantity = bi.Quantity,
                    TotalPrice = bi.TotalPrice,
                    UnitPrice = bi.UnitPrice,
                });

            // Apply Pagination.
            var filteredBasketItemsPaginated = filteredBasketItems
                .Skip((getBasketItemsByProductRequest.PageNumber - 1) * getBasketItemsByProductRequest.PageSize)
                .Take(getBasketItemsByProductRequest.PageSize)
                .ToList();

            // Get the Total Record count.
            var totalRecords = filteredBasketItems.Count();

            // Create the response.
            var response = new PagedResponse<BasketItemResponse>(
                 data: filteredBasketItemsPaginated,
                 pageNumber: getBasketItemsByProductRequest.PageNumber,
                 pageSize: getBasketItemsByProductRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName,
                 routeValues: new { getBasketItemsByProductRequest.ProductId } // Pass in the ProductId Query Value to ensure it ends up in the Next and Previous Page URLs.
             );

            return response;
        }

        public BasketItemResponse? CreateBasketItem(CreateBasketItemRequest createBasketItemRequest)
        {
            // Check that the associated Basket Exists.
            var basket = _context.Baskets.FirstOrDefault(basket => basket.BasketId == createBasketItemRequest.BasketId);
            if (basket == null)
            {
                // A Basket doesn't exist for the given Basket Id. Therefore, return a 400 'Bad Request' response.
                return null;
            }

            // Check that the associated Product Exists.
            var product = _context.Products.FirstOrDefault(product => product.ProductId == createBasketItemRequest.ProductId);
            if (product == null)
            {
                // A Product doesn't exist for the given product Id. Therefore, return a 400 'Bad Request' response.
                return null;
            }

            // Map the request object to a new Basket Item entity.
            var newBasketItem = new BasketItem
            {
                Basket = basket,
                Product = product,
                Quantity = createBasketItemRequest.Quantity,
                UnitPrice = createBasketItemRequest.UnitPrice,
                TotalPrice = createBasketItemRequest.TotalPrice
            };

            // Add the new Basket Item to the Database Context.
            _context.BasketItems.Add(newBasketItem);
            _context.SaveChanges();

            // Map the Basket Item entity to a new response object.
            var basketItemResponse = new BasketItemResponse
            {
                BasketId = newBasketItem.Basket.BasketId,
                ProductId = newBasketItem.Product.ProductId,
                Quantity = newBasketItem.Quantity,
                UnitPrice = newBasketItem.UnitPrice,
                TotalPrice = newBasketItem.TotalPrice
            };

            // Return the created Basket with a 201 'Created' response.
            return basketItemResponse;
        }

        public void UpdateBasketItem(long basketItemId, UpdateBasketItemRequest updateBasketItemRequest)
        {
            // Attempt to get the Basket to be updated.
            var existingBasketItem = _context.BasketItems.FirstOrDefault(basketItem => basketItem.BasketItemId == basketItemId);
            if (existingBasketItem == null)
            {
                // The Basket Item to be updated does not exist. Therefore, return a 404 'Not Found' response.
                return;
            }

            // Check that the associated Basket Exists.
            var basket = _context.Baskets.FirstOrDefault(basket => basket.BasketId == updateBasketItemRequest.BasketId);
            if (basket == null)
            {
                // A Basket doesn't exist for the given Basket Id. Therefore, return a 400 'Bad Request' response.
                return;
            }

            // Check that the associated Product Exists.
            var product = _context.Products.FirstOrDefault(product => product.ProductId == updateBasketItemRequest.ProductId);
            if (product == null)
            {
                // A Product doesn't exist for the given product Id. Therefore, return a 400 'Bad Request' response.
                return;
            }

            // Update the existing Basket with the values from the provided Basket Item.
            existingBasketItem.Basket = basket;
            existingBasketItem.Product = product;
            existingBasketItem.Quantity = updateBasketItemRequest.Quantity;
            existingBasketItem.UnitPrice = updateBasketItemRequest.UnitPrice;
            existingBasketItem.TotalPrice = updateBasketItemRequest.TotalPrice;

            // Save the changes to the database.
            _context.SaveChanges();
        }
    }
}