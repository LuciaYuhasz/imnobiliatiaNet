using imnobiliatiaNet.Models; // (opcional si necesitas usar otras clases del mismo namespace)

namespace imnobiliatiaNet.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int NumeroPago { get; set; }
        public DateTime FechaPago { get; set; }
        public string Concepto { get; set; } = "";
        public decimal Importe { get; set; }
        public bool Anulado { get; set; } = false;

        public int? UsuarioAltaId { get; set; }
        public int? UsuarioAnulacionId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? MotivoAnulacion { get; set; }

        public Contrato? Contrato { get; set; }
        public Usuario? UsuarioAlta { get; set; }
        public Usuario? UsuarioAnulacion { get; set; }
    }
}

