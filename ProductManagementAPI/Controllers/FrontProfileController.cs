using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.Models;
using Newtonsoft.Json;
using System.Text;

namespace ProductManagementAPI.Controllers.Front
{
    [Route("profile")]
    public class FrontProfileController : Controller
    {
        private readonly HttpClient _httpClient;

        public FrontProfileController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7250/api/")
            };
        }

        [HttpGet, HttpPost]
        public async Task<IActionResult> Index(User model)
        {
            if (Request.Method == HttpMethods.Post)
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Front/Profile/Index.cshtml", model);
                }

                try
                {
                    if (!string.IsNullOrEmpty(model.Email))
                    {
                        var emailCheckResponse = await _httpClient.GetAsync($"user/check-email?email={model.Email}");
                        if (emailCheckResponse.IsSuccessStatusCode)
                        {
                            var emailExists = JsonConvert.DeserializeObject<bool>(await emailCheckResponse.Content.ReadAsStringAsync());
                            if (emailExists)
                            {
                                ModelState.AddModelError("Email", "Cet email est déjà utilisé.");
                                return View("~/Views/Front/Profile/Index.cshtml", model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de la vérification de l'email.");
                            return View("~/Views/Front/Profile/Index.cshtml", model);
                        }
                    }

                    var jsonUser = JsonConvert.SerializeObject(model);
                    var content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

                    var response = await _httpClient.PutAsync($"user/{model.Id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["SuccessMessage"] = "Profil mis à jour avec succès.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Une erreur s'est produite lors de la mise à jour du profil.";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating user profile: {ex.Message}");
                    TempData["ErrorMessage"] = "Une erreur interne s'est produite.";
                }

                return View("~/Views/Front/Profile/Index.cshtml", model);
            }

            try
            {
                int userId = 1;
                var response = await _httpClient.GetStringAsync($"user/{userId}");
                var user = JsonConvert.DeserializeObject<User>(response);

                if (user == null)
                {
                    return View("~/Views/Front/Profile/Index.cshtml", new User());
                }

                return View("~/Views/Front/Profile/Index.cshtml", user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user profile: {ex.Message}");
                TempData["ErrorMessage"] = "Impossible de charger votre profil.";
                return View("~/Views/Front/Profile/Index.cshtml", new User());
            }
        }
    }
}