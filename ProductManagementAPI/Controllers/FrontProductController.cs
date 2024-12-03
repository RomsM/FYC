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
                BaseAddress = new Uri("https://localhost:7250/api/") // Adresse de l'API
            };
        }

        // Afficher la liste des produits
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("product");
                var products = JsonConvert.DeserializeObject<List<Product>>(response);
                return View(products);
            }
            catch (Exception ex)
            {
                // Gérer les erreurs et afficher un message
                ViewBag.Error = "Erreur lors du chargement des produits.";
                ViewBag.ErrorDetails = ex.Message;
                return View(new List<Product>()); // Retourne une liste vide
            }
        }

        // Voir les détails d'un produit
        public async Task<IActionResult> View(int id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"product/{id}");
                var product = JsonConvert.DeserializeObject<Product>(response);
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur lors du chargement du produit.";
                ViewBag.ErrorDetails = ex.Message;
                return RedirectToAction("Index"); // Redirige vers la liste
            }
        }

        // Ajouter un produit (GET)
        public IActionResult Add()
        {
            return View();
        }

        // Ajouter un produit (POST)
        [HttpPost]
        public async Task<IActionResult> Add(Product product)
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
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Erreur lors de l'ajout du produit.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Une erreur s'est produite : {ex.Message}");
            }

            return View(product);
        }

        // Modifier un produit (GET)
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"product/{id}");
                var product = JsonConvert.DeserializeObject<Product>(response);
                if (product == null)
                {
                    ViewBag.Error = "Produit introuvable.";
                    return RedirectToAction("Index");
                }

                return View(product); // Passe le produit à la vue

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur lors du chargement du produit.";
                ViewBag.ErrorDetails = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // Modifier un produit (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
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
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Erreur lors de la mise à jour du produit.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Une erreur s'est produite : {ex.Message}");
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
                    return RedirectToAction("Index");
                }

                ViewBag.Error = "Erreur lors de la suppression du produit.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Une erreur s'est produite lors de la suppression.";
                ViewBag.ErrorDetails = ex.Message;
            }

            ViewBag.Success = "Suppression réussie";
            return RedirectToAction("Index");
        }
    }
}