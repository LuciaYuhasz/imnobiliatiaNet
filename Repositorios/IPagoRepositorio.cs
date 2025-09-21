using imnobiliatiaNet.Models;

namespace imnobiliatiaNet.Repositorios
{
    public interface IPagoRepositorio
    {
        Task<int> CrearAsync(Pago p);
        Task<Pago?> ObtenerPorIdAsync(int id);
        Task<IList<Pago>> ObtenerPorContratoAsync(int contratoId);
        Task<bool> ActualizarConceptoAsync(Pago p);

        // Nuevo método con auditoría
        Task<bool> AnularAsync(int id, int usuarioId, string motivo);
    }
}
