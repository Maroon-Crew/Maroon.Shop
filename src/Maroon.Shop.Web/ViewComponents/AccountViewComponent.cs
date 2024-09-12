namespace Maroon.Shop.Web.ViewComponents
{
    using Maroon.Shop.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class AccountViewComponent : ViewComponent
    {
        private readonly ShopContext _context;

        public AccountViewComponent(ShopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return View((Customer)null);
            }

            string userId = User.Identity.Name;
            
            var customerId = int.Parse(userId);

            var query = from c in _context.Customers
                        where c.CustomerId == customerId
                        select c;

            var customer = await query.FirstAsync();

            return View(customer);
        }
    }
}
