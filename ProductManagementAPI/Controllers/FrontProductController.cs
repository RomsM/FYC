using Microsoft.AspNetCore.Mvc;
using System.Text;
using ProductManagementAPI.Models;
using Newtonsoft.Json;

namespace ProductManagementAPI.Controllers
{
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

        // Afficher la liste des produits
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("product");
                var products = JsonConvert.DeserializeObject<List<FrontProduct>>(response);
                return View(products);
            }
            catch (Exception)
            {
                return View(new List<Product>());
            }
        }

        // Voir les détails d'un produit
        public async Task<IActionResult> View(int id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"product/{id}");
                var product = JsonConvert.DeserializeObject<FrontProduct>(response);
                return View(product);
            }
            catch (Exception)
            {
                return Redirect("/product");
            }
        }

        // Ajouter un produit (GET)
        public IActionResult Add()
        {
            return View();
        }

        // Ajouter un produit (POST)
        [HttpPost]
        public async Task<IActionResult> Add(FrontProduct product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            try
            {
                var jsonProduct = JsonConvert.SerializeObject(product);
                var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("product", content);
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("/product");
                }
            }
            catch (Exception)
            {
                return Redirect("/product");
            }

            return View(product);
        }

        // Modifier un produit (GET)
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"product/{id}");
                var product = JsonConvert.DeserializeObject<FrontProduct>(response);
                if (product == null)
                {
                    return Redirect("/product");
                }

                return View(product);

            }
            catch (Exception)
            {
                return Redirect("/product");
            }
        }

        // Modifier un produit (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(FrontProduct product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            try
            {
                var jsonProduct = JsonConvert.SerializeObject(product);
                var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"product/{product.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("/product");
                }
            }
            catch (Exception)
            {
                return Redirect("/product");
            }

            return View(product);
        }

        // Supprimer un produit
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Redirect("/product");
                }
            }
            catch (Exception)
            {
                return Redirect("/product");
            }

            return Redirect("/product");
        }
    }
}