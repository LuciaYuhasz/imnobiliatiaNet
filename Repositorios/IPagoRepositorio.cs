using imnobiliatiaNet.Models;

namespace imnobiliatiaNet.Repositorios
{
    public interface IPagoRepositorio
    {
        Task<int> CrearAsync(Pago p);
        Task<Pago?> ObtenerPorIdAsync(int id);
        Task<IList<Pago>> ObtenerPorContratoAsync(int contratoId);
        Task<bool> ActualizarConceptoAsync(Pago p);
        Task<bool> AnularAsync(int id);
    }
}
