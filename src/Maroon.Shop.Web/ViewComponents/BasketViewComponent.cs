namespace Maroon.Shop.Web.ViewComponents
{
    using Maroon.Shop.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class BasketViewComponent : ViewComponent
    {
        private readonly ShopContext _context;

        public BasketViewComponent(ShopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var custromerId = 1;

            var query = from b in _context.Baskets
                        from i in b.Items
                        where b.Customer.CustomerId == custromerId
                        select i.Quantity;

            int count = await query.SumAsync();

            return View(count);
        }
    }
}
