using System.ComponentModel.DataAnnotations;

namespace ProductManagementAPI.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Nom d'utilisateur obligatoire")]
        public string Username { get; set; } = string.Empty;


        [Required(ErrorMessage = "Email obligatoire")]
        [EmailAddress(ErrorMessage = "Adresse email invalide")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password obligatoire")]
        [StringLength(100, ErrorMessage = "Le mot de pass doit comporter au minimum 8 caractères.", MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
