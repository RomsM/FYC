using Microsoft.AspNetCore.Mvc;
using System.Text;
using ProductManagementAPI.Models;
using Newtonsoft.Json;

namespace ProductManagementAPI.Controllers.Back
{
    [Route("admin/product")]
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

        // Admin Product List
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("product");
                var frontProducts = JsonConvert.DeserializeObject<List<FrontProduct>>(response) ?? new List<FrontProduct>();

                var backProducts = frontProducts.Select(fp => new BackProduct
                {
                    Id = fp.Id,
                    Name = fp.Name,
                    Description = fp.Description,
                    Price = fp.Price,
                    Stock = fp.Stock,
                }).ToList();

                return View("~/Views/Back/Product/Index.cshtml", backProducts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("~/Views/Back/Product/Index.cshtml", new List<BackProduct>());
            }
        }

        // Admin Add Product (GET)
        [HttpGet("add")]
        public IActionResult Add()
        {
            return View("~/Views/Back/Product/Add.cshtml");
        }

        // Admin Add Product (POST)
        [HttpPost("add")]
        public async Task<IActionResult> Add(BackProduct product)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Back/Product/Add.cshtml", product);
            }

            try
            {
                var frontProduct = new FrontProduct
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock
                };

                var jsonProduct = JsonConvert.SerializeObject(frontProduct);
                var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("product", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

            return View("~/Views/Back/Product/Add.cshtml", product);
        }
    }
}