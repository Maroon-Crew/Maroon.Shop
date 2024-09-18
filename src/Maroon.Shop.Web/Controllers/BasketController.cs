using Maroon.Shop.Data;
using Maroon.Shop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maroon.Shop.Web.Controllers
{
    /// <summary>
    /// Basket Controller Class. Defines the Basket Controller.
    /// </summary>
    public class BasketController : Controller
    {
        // Private backing fields.
        private readonly ShopContext _context;

        /// <summary>
        /// Constructor. Initialises an instance of <see cref="BasketController"/> and sets up the Data Context.
        /// </summary>
        /// <param name="context">A <see cref="ShopContext"/> representing the Maroon Shop Data Context.</param>
        public BasketController(ShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles GET to '../Basket/'.
        /// Redirects to the 'SimonBasket' action, which shows the basket details.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> redirecting to the SimonBasket Action.</returns>
        public IActionResult Index()
        {
            // Redirect to SimonBasket action.
            return RedirectToAction("SimonBasket");
        }

        /// <summary>
        /// Handles GET to '../Basket/SimonBasket'.
        /// Displays the user's Basket details. 
        /// If the user is not authenticated, returns the Log-in View.
        /// </summary>
        /// <returns> A <see cref="Task{IActionResult}"/> representing the Basket View with Basket Model data.</returns>
        public async Task<IActionResult> SimonBasket()
        {
            // Check if the user is authenticated.
            if (User.Identity?.IsAuthenticated != true)
            {
                // The user is not authenticated. Therefore return the Log-in View.
                return View(0);
            }

            // Get the logged-in user's account name (Customer Id).
            string userAccountId = User.Identity?.Name ?? "0";
            var customerId = int.Parse(userAccountId);

            // Build the query to fetch the Basket and it's Basket Items for the logged-in user.
            var basketQuery = from b in _context.Baskets
                              where b.Customer.CustomerId == customerId
                              select new BasketModel
                              {
                                  BasketId = b.BasketId,
                                  CustomerId = b.Customer.CustomerId,
                                  TotalPrice = b.TotalPrice,
                                  BasketItems = b.Items.Select(i => new BasketItemModel
                                  {
                                      BasketItemId = i.BasketItemId,
                                      BasketId = i.Basket.BasketId,
                                      ProductId = i.Product.ProductId,
                                      ProductName = i.Product.Name,
                                      Quantity = i.Quantity,
                                      UnitPrice = i.UnitPrice,
                                      TotalPrice = i.TotalPrice
                                  }).ToList()
                              };

            // Execute the query and fetch the Basket data.
            var basket = await basketQuery.AsNoTracking().FirstOrDefaultAsync();

            // Return the SimonBasket view with the Basket model.
            return View("SimonBasket", basket);
        }

        /// <summary>
        /// Handles POST to '../Basket/UpdateBasketItemQuantity'.
        /// Updates the Quantity for a given Basket Item and recalculates the Total Price of the Basket.
        /// </summary>
        /// <param name="basketItemId">A <see cref="long"/> representing the 'Id' of the Basket Item to update.</param>
        /// <param name="quantity">An <see cref="int"/> representing the new 'Quantity' value for the Basket Item.</param>
        /// <returns>An <see cref="IActionResult"/> containing a JSON object holding the new Basket Item Total Price and the Total Price of the entire Basket.</returns>
        [HttpPost]
        public IActionResult UpdateBasketItemQuantity(long basketItemId, int quantity)
        {
            // Fetch the Basket Item from the database, including the related Product and Basket data.
            var basketItem = _context.BasketItems
                .Include(basketItem => basketItem.Product)
                .Include(basketItem => basketItem.Basket)
                .FirstOrDefault(basketItem => basketItem.BasketItemId == basketItemId);

            if (basketItem == null)
            {
                // a Basket Item was not found.
                return NotFound();
            }

            // Update the Quantity and Total Price for the Basket Item.
            basketItem.Quantity = quantity;
            basketItem.TotalPrice = quantity * basketItem.UnitPrice;
            _context.SaveChanges();

            // Recalculate the Basket's Total Price.
            basketItem.Basket.TotalPrice = CalculateBasketTotalPrice(basketItem.Basket.BasketId);
            _context.SaveChanges();

            // Return a JSON response with the updated Basket Item Total Price and Basket Total Prices.
            return Json(new
            {
                totalPrice = basketItem.TotalPrice, // Total Price for the Basket Item.
                basketTotalPrice = basketItem.Basket.TotalPrice // Total Price for the Basket.
            });
        }

        /// <summary>
        /// Attempts to get all Basket Items for the given Basket and calculate the Total Price of the Basket.
        /// </summary>
        /// <param name="basketId">A <see cref="long"/> representing the 'Id' of the Basket.</param>
        /// <returns>A <see cref="decimal"/> representing the Total Price of the Basket.</returns>
        public decimal CalculateBasketTotalPrice(long basketId)
        {
            // Fetch all Basket Items for the Basket.
            var basketItems = _context.BasketItems
                .Include(bi => bi.Basket)
                .Where(bi => bi.Basket.BasketId == basketId);

            // Sum the Total Prices of the Basket Items, or return 0 if there are no Basket Items.
            return basketItems?.Sum(bi => bi.TotalPrice) ?? 0;
        }

        /// <summary>
        /// Attempts to Delee the given Basket Item and recalculate the Total Price of the Basket.
        /// </summary>
        /// <param name="basketItemId">A <see cref="long"/> representing the 'Id' of the Basket.</param>
        /// <returns>An <see cref="IActionResult"/> containing a JSON object holding the new Total Price of the entire Basket.</returns>
        [HttpPost]
        public IActionResult RemoveBasketItem(long basketItemId)
        {
            // Fetch the basket item from the database
            var basketItem = _context.BasketItems
                .Include(bi => bi.Basket)
                .FirstOrDefault(bi => bi.BasketItemId == basketItemId);

            if (basketItem == null)
            {
                // If the basket item doesn't exist, return NotFound
                return NotFound();
            }

            // Remove the item from the database and save changes
            _context.BasketItems.Remove(basketItem);
            _context.SaveChanges();

            // Recalculate the total price of the basket
            var basketTotalPrice = CalculateBasketTotalPrice(basketItem.Basket.BasketId);

            // Return success response with updated basket total price
            return Json(new
            {
                success = true,
                basketTotalPrice
            });
        }
    }
}