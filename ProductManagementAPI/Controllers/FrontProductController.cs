using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.Models;
using Newtonsoft.Json;

namespace ProductManagementAPI.Controllers.Front
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7250/api/")
            };
        }

        // Front Product List
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("product");
                var products = JsonConvert.DeserializeObject<List<FrontProduct>>(response) ?? new List<FrontProduct>();
                return View("~/Views/Front/Product/Index.cshtml", products); // Front view
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("~/Views/Front/Product/Index.cshtml", new List<FrontProduct>()); // Empty fallback
            }
        }

        // Front View Product Details
        [HttpGet("{id:int}")]
        public async Task<IActionResult> View(int id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"product/{id}");
                var product = JsonConvert.DeserializeObject<FrontProduct>(response);
                return View("~/Views/Front/Product/View.cshtml", product); // Front view
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}