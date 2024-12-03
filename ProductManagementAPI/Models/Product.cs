using System.ComponentModel.DataAnnotations;

namespace ProductManagementAPI.Models
{
    public class FrontProduct
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "La description est requise.")]

        public required string Description { get; set; }

        [Required(ErrorMessage = "Le prix est requis.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Le stock est requis.")]
        [Range(0, int.MaxValue, ErrorMessage = "Le stock doit être un nombre positif.")]
        public int Stock { get; set; }
    }
}