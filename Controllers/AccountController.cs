using IntranetHub.Data;
using IntranetHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IntranetHub.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);
            if (user != null)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
                if (user.IsAdmin) claims.Add(new Claim(ClaimTypes.Role, "Admin"));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Login inválido.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}
