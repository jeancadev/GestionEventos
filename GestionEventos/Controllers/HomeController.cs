using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GestionEventos.Models;
using GestionEventos.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GestionEventos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Accediendo a la página de inicio");
            var proximosEventos = await _context.Eventos
                .Where(e => e.Fecha >= DateTime.Today)
                .OrderBy(e => e.Fecha)
                .Take(5)
                .ToListAsync();
            return View(proximosEventos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult LoggedOut()
        {
            _logger.LogInformation("Usuario ha cerrado sesión");
            return View();
        }
    }
}