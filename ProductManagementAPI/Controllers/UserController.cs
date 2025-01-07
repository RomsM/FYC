using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.Models;
using Newtonsoft.Json;
using System.Text;

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

        // View user details
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

        // Add a new user (GET)
        [HttpGet("add")]
        public IActionResult Add()
        {
            return View("~/Views/Back/User/Add.cshtml");
        }

        // Add a new user (POST)
        [HttpPost("add")]
        public async Task<IActionResult> Add(User user)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Back/User/Add.cshtml", user);
            }

            try
            {
                var jsonUser = JsonConvert.SerializeObject(user);
                var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("user", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
            }

            return View("~/Views/Back/User/Add.cshtml", user);
        }

        // Edit an existing user (GET)
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"user/{id}");
                var user = JsonConvert.DeserializeObject<User>(response);

                if (user == null)
                {
                    return NotFound();
                }

                return View("~/Views/Back/User/Edit.cshtml", user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user for editing: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        // Edit an existing user (POST)
        [HttpPost("edit/{id:int}")]
        public async Task<IActionResult> Edit(User user)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Back/User/Edit.cshtml", user);
            }

            try
            {
                var jsonUser = JsonConvert.SerializeObject(user);
                var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"user/{user.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error editing user: {ex.Message}");
            }

            return View("~/Views/Back/User/Edit.cshtml", user);
        }

        // Delete a user
        [HttpPost("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"user/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
            }

            return RedirectToAction("Index");
        }
    }
}