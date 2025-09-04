namespace imnobiliatiaNet.Models
{
    public class Inmueble
    {
        public int Id { get; set; }
        public string Direccion { get; set; } = "";
        public string Uso { get; set; } = ""; // "Residencial" o "Comercial"
        public string Tipo { get; set; } = ""; // Casa, Depto, Local...
        public int Ambientes { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        //public decimal? Latitud { get; set; }
        //      public decimal? Longitud { get; set; }
        // FK
        public int PropietarioId { get; set; }
        public Propietario? Propietario { get; set; }
    }
}
