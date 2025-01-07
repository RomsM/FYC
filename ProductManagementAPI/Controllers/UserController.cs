using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.Models;
using Newtonsoft.Json;

namespace ProductManagementAPI.Controllers
{
    [Route("admin/user")] // Base route for this controller
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7250/api/")
            };
        }

        // List all users
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("user");
                var users = JsonConvert.DeserializeObject<List<User>>(response);
                return View("~/Views/Back/User/Index.cshtml", users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return View("~/Views/Back/User/Index.cshtml", new List<User>());
            }
        }

        //view user details
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"user/{id}");
                var user = JsonConvert.DeserializeObject<User>(response);

                if (user == null)
                {
                    return NotFound();
                }

                return View("~/Views/Back/User/View.cshtml", user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user details: {ex.Message}");
                return RedirectToAction("Index");
            }
        }
    }
}