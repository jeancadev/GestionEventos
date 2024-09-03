using GestionEventos.Data;
using GestionEventos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionEventos.Controllers
{
    [Authorize]
    public class InvitadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvitadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invitados
        public async Task<IActionResult> Index()
        {
            return View(await _context.Invitados.Include(i => i.Categoria).ToListAsync());
        }

        // GET: Invitados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitado = await _context.Invitados
                .Include(i => i.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invitado == null)
            {
                return NotFound();
            }

            return View(invitado);
        }

        // GET: Invitados/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Invitados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Nombre,Profesion,Entretenimiento,Deporte,Alimentacion,CategoriaId")] Invitado invitado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invitado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre", invitado.CategoriaId);
            return View(invitado);
        }

        // GET: Invitados/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitado = await _context.Invitados.FindAsync(id);
            if (invitado == null)
            {
                return NotFound();
            }
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre", invitado.CategoriaId);
            return View(invitado);
        }

        // POST: Invitados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Profesion,Entretenimiento,Deporte,Alimentacion,CategoriaId")] Invitado invitado)
        {
            if (id != invitado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invitado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvitadoExists(invitado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Nombre", invitado.CategoriaId);
            return View(invitado);
        }

        // GET: Invitados/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitado = await _context.Invitados
                .Include(i => i.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invitado == null)
            {
                return NotFound();
            }

            return View(invitado);
        }

        // POST: Invitados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invitado = await _context.Invitados.FindAsync(id);
            _context.Invitados.Remove(invitado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvitadoExists(int id)
        {
            return _context.Invitados.Any(e => e.Id == id);
        }
    }
}