namespace imnobiliatiaNet.Models
{
    public class Paginador<T>
    {
        public IList<T> Items { get; set; } = new List<T>();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }
}
