using imnobiliatiaNet.Models;

namespace imnobiliatiaNet.Repositorios
{
    public interface IContratoRepositorio
    {
        Task<int> AltaAsync(Contrato c);
        Task<Contrato?> ObtenerPorIdAsync(int id);
        Task<IList<Contrato>> ObtenerTodosAsync();
        Task<bool> ModificarAsync(Contrato c);

        Task<bool> TerminarAnticipadamenteAsync(int contratoId, DateTime fechaTerminacion, int usuarioId);
        decimal CalcularMulta(Contrato contrato, DateTime fechaTerminacion);
        Task<bool> ExisteSuperposicionAsync(int inmuebleId, DateTime inicio, DateTime fin, int? contratoId = null);
        //Task<Paginador<Contrato>> ObtenerPaginadoAsync(int pagina, int tamPagina);
        Task<Paginador<Contrato>> ObtenerPaginadoAsync(string? filtro, int pagina, int tamPagina);


    }
}

