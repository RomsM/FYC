using System.ComponentModel.DataAnnotations;

namespace ProductManagementAPI.Models
{
    public class FrontProduct
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}