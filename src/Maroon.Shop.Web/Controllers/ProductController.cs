using Azure.Core;
using Maroon.Shop.Data;
using Maroon.Shop.Web.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maroon.Shop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopContext _context;

        public ProductController(ShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var query = from p in _context.Products
                        select new ProductCardModel
                        {
                            ProductId = p.ProductId,
                            ImageUrl = p.ImageUrl,
                            Name = p.Name,
                            Price = p.Price,
                            UrlFriendlyName = p.UrlFriendlyName,                
                        };

            var products = await query.ToListAsync();

            return View(products);
        }

        [HttpGet]
        [Route("Product/{urlFrieldlyName}")]
        public async Task<IActionResult> Index(string urlFrieldlyName)
        {
            var query = from p in _context.Products
                        where p.UrlFriendlyName == urlFrieldlyName
                        select new ProductModel
                        {
                            ProductId = p.ProductId,
                            ImageUrl = p.ImageUrl,
                            Name = p.Name,
                            Price = p.Price,
                            UrlFriendlyName = p.UrlFriendlyName,
                            Description = p.Description,
                            PleaseNote = p.PleaseNote,
                        };

            var product = await query.FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return View("Product", product);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(ProductCardModel productCardModel)
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

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToBasket(ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Product", productModel);
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

            return View("Product", productModel);
        }
    }
}
