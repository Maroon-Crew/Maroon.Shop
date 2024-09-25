using Azure.Core;
using Maroon.Shop.Api.Client;
using Maroon.Shop.Api.Client.Api.Product;
using Maroon.Shop.Data;
using Maroon.Shop.Web.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Maroon.Shop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopContext _context;
        private readonly ProductClient _productClient;
        private readonly MaroonClient _maroonClient;

        public ProductController(ShopContext context, ProductClient productClient, MaroonClient maroonClient)
        {
            _context = context;
            _productClient = productClient;
            _maroonClient = maroonClient;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _maroonClient.Api.Product.GetAsync(c => c.QueryParameters = new ProductRequestBuilder.ProductRequestBuilderGetQueryParameters { PageNumber = 1, PageSize = 9 });

            var models = products.Data.Select(p => new ProductCardModel
            { 
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Price = p.Price.Value,
                ProductId = p.ProductId.Value,
                UrlFriendlyName = p.UrlFriendlyName,
            });

            return View(models);
        }

        [HttpGet]
        [Route("Product/{urlFrieldlyName}")]
        public async Task<IActionResult> Index(string urlFrieldlyName)
        {
            var product = await _maroonClient.Api.Product[urlFrieldlyName].GetAsync();

            if (product == null)
            {
                return NotFound();
            }

            var productModel = new ProductModel
            { 
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Price = product.Price.Value,
                PleaseNote = product.PleaseNote,
                ProductId = product.ProductId.Value,
                UrlFriendlyName = product.UrlFriendlyName,
            };

            return View("Product", productModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(ProductCardModel productCardModel)
        {
            return await AddToBasket(productCardModel, nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddToSimonBasket(ProductCardModel productCardModel)
        {
            return await AddToBasket(productCardModel, nameof(SimonProducts));
        }

        public async Task<IActionResult> AddToBasket(ProductCardModel productCardModel, string viewName)
        {
            // TODO: we need a logged in user and customer id, if no user logged in, redirect to login
            var customerId = 1;

            var customer = await _context.Customers.FirstAsync(c => c.CustomerId == customerId);
            var basket = await _context.Baskets.Include(b => b.Items).FirstOrDefaultAsync(b => b.Customer.CustomerId == customerId);

            if (basket == null)
            {
                basket = new Basket
                {
                    Customer = customer,
                    TotalPrice = 0.0m,
                };

                _context.Baskets.Add(basket);
            }

            basket.Items = basket.Items ?? new List<BasketItem>();

            var product = await _context.Products.FirstAsync(p => p.ProductId == productCardModel.ProductId);

            var item = basket.Items.FirstOrDefault(b => b.Product == product);

            if (item != null)
            {
                item.Quantity++;
                item.TotalPrice = item.Quantity * item.UnitPrice;
            }
            else
            {
                basket.Items.Add(new BasketItem
                {
                    Basket = basket,
                    Product = product,
                    Quantity = 1,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price,
                });
            }

            // recalculate the basket total
            basket.TotalPrice = basket.Items.Sum(i => i.TotalPrice);

            await _context.SaveChangesAsync();

            return RedirectToAction(viewName);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToBasket(ProductModel productModel)
        {
            return await AddProductToBasket(productModel, "Product");
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToSimonBasket(ProductModel productModel)
        {
            return await AddProductToBasket(productModel, "SimonProduct");
        }

        public async Task<IActionResult> AddProductToBasket(ProductModel productModel, string viewName)
        {
            if (!ModelState.IsValid)
            {
                return View(viewName, productModel);
            }

            // TODO: we need a logged in user and customer id, if no user logged in, redirect to login
            var customerId = 1;

            var customer = await _context.Customers.FirstAsync(c => c.CustomerId == customerId);
            var basket = await _context.Baskets.Include(b => b.Items).FirstOrDefaultAsync(b => b.Customer.CustomerId == customerId);

            if (basket == null)
            {
                basket = new Basket
                {
                    Customer = customer,
                    TotalPrice = 0.0m,
                };

                _context.Baskets.Add(basket);
            }

            basket.Items = basket.Items ?? new List<BasketItem>();

            var product = await _context.Products.FirstAsync(p => p.ProductId == productModel.ProductId);

            var item = basket.Items.FirstOrDefault(b => b.Product == product);

            if (item != null)
            {
                item.Quantity += productModel.SelectedQuantity.Value;
                item.TotalPrice = item.Quantity * item.UnitPrice;
            }
            else
            {
                basket.Items.Add(new BasketItem
                {
                    Basket = basket,
                    Product = product,
                    Quantity = productModel.SelectedQuantity.Value,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * productModel.SelectedQuantity.Value,
                });
            }

            // recalculate the basket total
            basket.TotalPrice = basket.Items.Sum(i => i.TotalPrice);

            await _context.SaveChangesAsync();

            return View(viewName, productModel);
        }

        [HttpGet]
        [Route("Product/SimonProducts")]
        public async Task<IActionResult> SimonProducts()
        {
            var productsQuery = from product in _context.Products
                        select new ProductCardModel
                        {
                            ProductId = product.ProductId,
                            ImageUrl = product.ImageUrl,
                            Name = product.Name,
                            Price = product.Price,
                            UrlFriendlyName = product.UrlFriendlyName,
                        };

            var products = await productsQuery.ToListAsync();

            return View(products);
        }

        [HttpGet]
        [Route("Product/SimonProducts/{urlFrieldlyName}")]
        public async Task<IActionResult> SimonProducts(string urlFrieldlyName)
        {
            var productQuery = from products in _context.Products
                               where products.UrlFriendlyName == urlFrieldlyName
                               select new ProductModel
                               {
                                   ProductId = products.ProductId,
                                   ImageUrl = products.ImageUrl,
                                   Name = products.Name,
                                   Price = products.Price,
                                   UrlFriendlyName = products.UrlFriendlyName,
                                   Description = products.Description,
                                   PleaseNote = products.PleaseNote,
                               };

            var product = await productQuery.FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return View("SimonProduct", product);
        }
    }
}