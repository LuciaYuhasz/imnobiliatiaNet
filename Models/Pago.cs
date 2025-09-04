using System;

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

        public Contrato? Contrato { get; set; }
    }
}
