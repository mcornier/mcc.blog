using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BlogBackend.Data;
using BlogBackend.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using BCrypt.Net;

namespace BlogBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BlogContext _context;

        public AuthController(BlogContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email || u.Username == user.Username))
            {
                return BadRequest("User already exists.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == login.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(user.Username);
            return Ok(new { token });
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest();

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var user = _context.Users.SingleOrDefault(u => u.Email == email);

            if (user == null)
            {
                user = new User { Email = email, Username = email }; // Simplified for example
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            var token = GenerateJwtToken(user.Username);
            return Ok(new { token });
        }

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "your_issuer",
                audience: "your_audience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
