using GestionEventos.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionEventos.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Invitado> Invitados { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Suscripcion> Suscripciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}