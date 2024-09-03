using GestionEventos.Data;
using GestionEventos.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace GestionEventos.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            _logger.LogInformation($"Intento de login - Usuario: {username}");

            if (ModelState.IsValid)
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
                if (user != null)
                {
                    _logger.LogInformation($"Usuario encontrado: {user.Username}, Rol: {user.Rol}");
                    _logger.LogInformation($"Contraseña almacenada (hash): {user.Password}");
                    var inputHash = HashPassword(password);
                    _logger.LogInformation($"Contraseña ingresada (hash): {HashPassword(password)}");
                    _logger.LogInformation($"¿Contraseñas coinciden?: {user.Password == inputHash}");

                    if (VerifyPassword(password, user.Password))
                    {
                        _logger.LogInformation("Contraseña verificada correctamente");
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.Role, user.Rol)
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));

                        _logger.LogInformation($"Login exitoso para el usuario: {username}");
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        _logger.LogWarning("Verificación de contraseña fallida");
                    }
                }
                else
                {
                    _logger.LogWarning("Usuario no encontrado");
                }
                ModelState.AddModelError(string.Empty, "Intento de inicio de sesión no válido.");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Iniciando proceso de cierre de sesión");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("Sesión cerrada correctamente");
            return RedirectToAction("LoggedOut", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            return storedPassword == HashPassword(enteredPassword);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}