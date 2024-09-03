using Microsoft.AspNetCore.Mvc;

namespace Maroon.Shop.Web.Controllers
{
    public class WireframeController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        [Route("Wireframe/{path}")]

        public IActionResult Index(string path)
        {
            return View(path);
        }
    }
}
