using imnobiliatiaNet.Models;

namespace imnobiliatiaNet.Repositorios
{
    public interface IInmuebleRepositorio
    {
        Task<int> AltaAsync(Inmueble i);
        Task<Inmueble?> ObtenerPorIdAsync(int id);
        Task<IList<Inmueble>> ObtenerTodosAsync(string? filtro = null, bool? disponibles = null);
        Task<bool> ModificarAsync(Inmueble i);
        Task<bool> BajaAsync(int id);
        Task<IList<Inmueble>> ObtenerDisponiblesEntreFechasAsync(DateTime inicio, DateTime fin);

        Task<Paginador<Inmueble>> ObtenerPaginadoAsync(string? filtro, bool? disponibles, int pagina, int tamPagina);


    }
}
