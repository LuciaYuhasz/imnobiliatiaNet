using imnobiliatiaNet.Models;

namespace imnobiliatiaNet.Repositorios
{
    public interface IContratoRepositorio
    {
        Task<int> AltaAsync(Contrato c);
        Task<Contrato?> ObtenerPorIdAsync(int id);
        Task<IList<Contrato>> ObtenerTodosAsync();
        Task<bool> ModificarAsync(Contrato c);
        Task<bool> BajaAsync(int id);
    }
}
