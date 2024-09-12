namespace Maroon.Shop.Web.ViewComponents
{
    using Maroon.Shop.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration.UserSecrets;
    using System.Security.Claims;

    public class BasketViewComponent : ViewComponent
    {
        private readonly ShopContext _context;

        public BasketViewComponent(ShopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return View(0);
            }

            string userId = User.Identity?.Name ?? "0";

            var custromerId = int.Parse(userId);

            var query = from b in _context.Baskets
                        from i in b.Items
                        where b.Customer.CustomerId == custromerId
                        select i.Quantity;

            int count = await query.SumAsync();

            return View(count);
        }
    }
}
