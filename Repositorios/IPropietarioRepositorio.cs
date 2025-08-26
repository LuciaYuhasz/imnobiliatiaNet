using imnobiliatiaNet.Models;

namespace imnobiliatiaNet.Repositorios
{
    public interface IPropietarioRepositorio
    {
        Task<IList<Propietario>> ListarAsync(string? filtro = null);
        Task<Propietario?> ObtenerPorIdAsync(int id);
        Task<int> CrearAsync(Propietario p);
        Task<bool> ActualizarAsync(Propietario p);
        Task<bool> BorrarAsync(int id);
    }

}

