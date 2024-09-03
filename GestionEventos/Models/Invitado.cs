using System.ComponentModel.DataAnnotations;

namespace GestionEventos.Models
{
    public class Invitado
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(100)]
        public string Profesion { get; set; }

        [StringLength(100)]
        public string Entretenimiento { get; set; }

        [StringLength(100)]
        public string Deporte { get; set; }

        [StringLength(100)]
        public string Alimentacion { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}