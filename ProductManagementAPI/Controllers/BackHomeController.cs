using Microsoft.AspNetCore.Mvc;

namespace ProductManagementAPI.Controllers.Back
{
    [Route("admin")]
    public class BackHomeController : Controller
    {
        // GET /admin/
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Views/Back/Home/Index.cshtml");
        }
    }
}