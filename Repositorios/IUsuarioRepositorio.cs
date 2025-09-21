

using imnobiliatiaNet.Models;

namespace imnobiliatiaNet.Repositorios
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario?> AutenticarAsync(string email, string clave);
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<IList<Usuario>> ObtenerTodosAsync();
        Task<int> CrearAsync(Usuario u);
        Task<bool> ActualizarAsync(Usuario u); // o ModificarAsync, pero no ambos
        Task<bool> BorrarAsync(int id);
        Task<Usuario?> ObtenerPorEmailAsync(string email);
    }
}
