using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProductManagementAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            return BadRequest("All fields are required.");
        }

        var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            return Ok(new { message = "User created successfully" });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
            return BadRequest("Username and password are required.");
        }

        var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || string.IsNullOrEmpty(user.UserName))
            {
                return Unauthorized("Invalid login attempt.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
            {
                return StatusCode(500, "JWT Key is missing from configuration.");
            }

            var keyBytes = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { token = tokenHandler.WriteToken(token) });
        }
        return Unauthorized("Invalid login attempt.");
    }
}
