using imnobiliatiaNet.Models;

namespace imnobiliatiaNet.Repositorios
{
    public interface IInquilinoRepositorio
    {
        Task<int> CrearAsync(Inquilino i);
        Task<Inquilino?> ObtenerPorIdAsync(int id);
        Task<IList<Inquilino>> ListarAsync(string? filtro = null);
        Task<bool> ActualizarAsync(Inquilino i);
        Task<bool> BorrarAsync(int id);
    }
}

