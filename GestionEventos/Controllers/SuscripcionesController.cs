using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionEventos.Data;
using GestionEventos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionEventos.Controllers
{
    [Authorize]
    public class SuscripcionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuscripcionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Suscripciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Suscripciones.Include(s => s.Invitado).Include(s => s.Evento).ToListAsync());
        }

        // GET: Suscripciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripciones = await _context.Suscripciones
                .Include(s => s.Invitado)
                .Include(s => s.Evento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suscripciones == null)
            {
                return NotFound();
            }

            return View(suscripciones);
        }

        // GET: Suscripciones/Create
        public IActionResult Create()
        {
            ViewBag.Invitados = new SelectList(_context.Invitados, "Id", "Nombre");
            ViewBag.Eventos = new SelectList(_context.Eventos, "Id", "Nombre");
            return View();
        }

        // POST: Suscripciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InvitadoId,EventoId")] Suscripcion suscripcion)
        {
            if (ModelState.IsValid)
            {
                suscripcion.FechaInscripcion = DateTime.Now;
                _context.Add(suscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Invitados = new SelectList(_context.Invitados, "Id", "Nombre", suscripcion.InvitadoId);
            ViewBag.Eventos = new SelectList(_context.Eventos, "Id", "Nombre", suscripcion.EventoId);
            return View(suscripcion);
        }

        // GET: Suscripciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripciones.FindAsync(id);
            if (suscripcion == null)
            {
                return NotFound();
            }
            ViewBag.Invitados = _context.Invitados.ToList();
            ViewBag.Eventos = _context.Eventos.ToList();
            return View(suscripcion);
        }

        // POST: Suscripciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InvitadoId,EventoId,FechaInscripcion")] Suscripcion suscripcion)
        {
            if (id != suscripcion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suscripcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuscripcionExists(suscripcion.Id))
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
            ViewBag.Invitados = _context.Invitados.ToList();
            ViewBag.Eventos = _context.Eventos.ToList();
            return View(suscripcion);
        }

        // GET: Suscripciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripciones
                .Include(s => s.Invitado)
                .Include(s => s.Evento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            return View(suscripcion);
        }

        // POST: Suscripciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suscripcion = await _context.Suscripciones.FindAsync(id);
            _context.Suscripciones.Remove(suscripcion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuscripcionExists(int id)
        {
            return _context.Suscripciones.Any(e => e.Id == id);
        }
    }
}