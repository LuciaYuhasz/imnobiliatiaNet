using imnobiliatiaNet.Models;

namespace imnobiliatiaNet.Repositorios
{
    public interface IContratoRepositorio
    {
        Task<int> AltaAsync(Contrato c);
        Task<Contrato?> ObtenerPorIdAsync(int id);
        Task<IList<Contrato>> ObtenerTodosAsync();
        Task<bool> ModificarAsync(Contrato c);

        // Este método ya no se usa, podés eliminarlo si querés
        // Task<bool> BajaAsync(int id);

        Task<bool> TerminarAnticipadamenteAsync(int contratoId, DateTime fechaTerminacion, int usuarioId);
        decimal CalcularMulta(Contrato contrato, DateTime fechaTerminacion);
        Task<bool> ExisteSuperposicionAsync(int inmuebleId, DateTime inicio, DateTime fin, int? contratoId = null);

    }
}

