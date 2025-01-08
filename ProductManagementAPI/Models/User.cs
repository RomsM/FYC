using Microsoft.AspNetCore.Identity;

namespace ProductManagementAPI.Models
{
    public class User : IdentityUser
    {
        public string Role {  get; set; } = string.Empty;
    }
}