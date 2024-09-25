using Maroon.Shop.Api.Data.Requests;
using Maroon.Shop.Api.Data.Responses;
using Maroon.Shop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maroon.Shop.Api.Data.Repositories
{
    public class ProductRepository
    {
        private readonly ShopContext _context;

        public ProductRepository(ShopContext context)
        {
            _context = context;
        }

        public ProductResponse? GetById(GetProductRequest getProductRequest)
        {
            // Query for a Product with the given productId.
            var query = _context.Products.Where(product => product.ProductId == getProductRequest.ProductId);

            if (!query.Any())
            {
                // The Product could not be found, return a 404 'Not Found' response.
                return null;
            }
            else
            {
                var product = query.First();
                var productResponse = new ProductResponse
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    PleaseNote = product.PleaseNote,
                    UrlFriendlyName = product.UrlFriendlyName,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price
                };

                return productResponse;
            }
        }

        public async Task<ProductResponse?> GetByNameAsync(string productName)
        {
            var query = from p in _context.Products
                        where p.UrlFriendlyName == productName
                        select new ProductResponse
                        {
                            ProductId = p.ProductId,
                            ImageUrl = p.ImageUrl,
                            Name = p.Name,
                            Price = p.Price,
                            UrlFriendlyName = p.UrlFriendlyName,
                            Description = p.Description,
                            PleaseNote = p.PleaseNote,
                        };

            return await query.FirstOrDefaultAsync();
        }

        public PagedResponse<ProductResponse> GetProducts(GetProductsRequest getProductsRequest, string routeName, IUrlHelper urlHelper)
        {
            // Retrieve all Products using pagination.
            var products = _context.Products
                .OrderBy(products => products.ProductId) // Note: Without an OrderBy, the data could come out randomly.
                .Skip((getProductsRequest.PageNumber - 1) * getProductsRequest.PageSize)
                .Take(getProductsRequest.PageSize)
                .Select(p => new ProductResponse
                {
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Name = p.Name,
                    PleaseNote = p.PleaseNote,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    UrlFriendlyName = p.UrlFriendlyName,
                })
                .ToList();

            // Get the Total Record count.
            var totalRecords = _context.Products.Count();

            // Create the response.
            var pagedProductResponse = new PagedResponse<ProductResponse>(
                 data: products,
                 pageNumber: getProductsRequest.PageNumber,
                 pageSize: getProductsRequest.PageSize,
                 totalRecords: totalRecords,
                 urlHelper: urlHelper,
                 routeName: routeName
             );

            return pagedProductResponse;
        }

        public ProductResponse? CreateProduct(CreateProductRequest createProductRequest)
        {
            // Map the request object to a new Product entity.
            var newProduct = new Product
            {
                Name = createProductRequest.Name,
                UrlFriendlyName = createProductRequest.UrlFriendlyName,
                Price = createProductRequest.Price,
                Description = createProductRequest.Description,
                PleaseNote = createProductRequest.PleaseNote,
                ImageUrl = createProductRequest.ImageUrl,
            };

            // Add the new Product to the Database Context.
            _context.Products.Add(newProduct);
            _context.SaveChanges();

            // Map the Product entity to a new response object.
            var productResponse = new ProductResponse
            {
                ProductId = newProduct.ProductId,
                Name = newProduct.Name,
                Description = newProduct.Description,
                PleaseNote = newProduct.PleaseNote,
                UrlFriendlyName = newProduct.UrlFriendlyName,
                ImageUrl = newProduct.ImageUrl,
                Price = newProduct.Price
            };

            // Return the created Product with a 201 'Created' response.
            return productResponse;
        }

        public void UpdateProduct(long productId, UpdateProductRequest updateProductRequest)
        {
            // Attempt to get the Product to be updated.
            var existingProduct = _context.Products.FirstOrDefault(product => product.ProductId == productId);
            if (existingProduct == null)
            {
                // The Product to be updated does not exist. Therefore, return a 404 'Not Found' response.
                return;
            }

            // Update the existing Product with the values from the provided Product.
            existingProduct.Name = updateProductRequest.Name;
            existingProduct.UrlFriendlyName = updateProductRequest.UrlFriendlyName;
            existingProduct.Price = updateProductRequest.Price;
            existingProduct.Description = updateProductRequest.Description;
            existingProduct.PleaseNote = updateProductRequest.PleaseNote;
            existingProduct.ImageUrl = updateProductRequest.ImageUrl;

            // Save the changes to the database.
            _context.SaveChanges();
        }
    }
}