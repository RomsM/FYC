using Microsoft.AspNetCore.Mvc;

namespace ProductManagementAPI.Controllers
{
    [Route("")]
    public class FrontHomeController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Front/Home/Index.cshtml");
        }
    }
}