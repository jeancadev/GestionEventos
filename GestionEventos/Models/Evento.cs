using System.ComponentModel.DataAnnotations;

namespace GestionEventos.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        public TimeSpan HoraFin { get; set; }

        public int? CupoLimite { get; set; }

        [StringLength(200)]
        public string Tema { get; set; }

        [StringLength(200)]
        public string Lugar { get; set; }

        [Required]
        [StringLength(50)]
        public string Medio { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoEvento { get; set; }
    }
}