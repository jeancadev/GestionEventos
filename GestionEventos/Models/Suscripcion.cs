namespace GestionEventos.Models
{
    public class Suscripcion
    {
        public int Id { get; set; }
        public int InvitadoId { get; set; }
        public Invitado Invitado { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; set; }
        public DateTime FechaInscripcion { get; set; }
    }
}