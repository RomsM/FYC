using Microsoft.AspNetCore.Mvc;

namespace ProductManagementAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}