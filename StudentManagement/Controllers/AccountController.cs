using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace StudentManagement.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly StudentDbContext _db;

        public AccountController(IConfiguration configuration, StudentDbContext db)
        {
            _configuration = configuration;
            _db = db;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async System.Threading.Tasks.Task<IActionResult> Login(User model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _db.Users.FirstOrDefault(u => u.Username == model.Username);
            var hasher = new PasswordHasher<User>();
            var isValid = false;
            if (user != null)
            {
                var res = hasher.VerifyHashedPassword(user, user.PasswordHash, model.PasswordHash);
                isValid = res == PasswordVerificationResult.Success;
            }

            // log attempt
            var log = new LoginLog
            {
                Username = model.Username,
                Successful = isValid,
                Timestamp = DateTime.UtcNow,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
            };
            _db.LoginLogs.Add(log);
            _db.SaveChanges();

            if (!isValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials");
                return View(model);
            }

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key not configured");

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, user?.Role ?? "User")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = tokenHandler.WriteToken(token);

            // Sign in using cookie authentication so MVC redirects and Authorize works
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            // Also set JWT cookie for API usage if desired
            Response.Cookies.Append("AuthToken", tokenString, new CookieOptions { HttpOnly = true, Secure = true });

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Logout()
        {
            // Sign out the cookie authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Remove JWT cookie if present
            if (Request.Cookies.ContainsKey("AuthToken"))
            {
                Response.Cookies.Delete("AuthToken");
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet("logout")]
        public async System.Threading.Tasks.Task<IActionResult> LogoutGet()
        {
            return await Logout();
        }
    }
}
