

namespace imnobiliatiaNet.Models
{
    public class Contrato
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Monto { get; set; }
        public int InmuebleId { get; set; }
        public int InquilinoId { get; set; }

        // Nuevos campos
        public DateTime? FechaTerminacionAnticipada { get; set; }
        public decimal? MontoMulta { get; set; }
        public int UsuarioCreadorId { get; set; }
        public int? UsuarioTerminadorId { get; set; }

        // Relaciones
        public Inmueble? Inmueble { get; set; }
        public Inquilino? Inquilino { get; set; }
        public Usuario? UsuarioCreador { get; set; }
        public Usuario? UsuarioTerminador { get; set; }
    }
}
