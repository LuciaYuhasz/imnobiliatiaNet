using System.Data;
using imnobiliatiaNet.Data;
using imnobiliatiaNet.Models;
using MySqlConnector;

namespace imnobiliatiaNet.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly Db _db;
        public UsuarioRepositorio(Db db) => _db = db;

        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Usuario WHERE Email=@email;";
            cmd.Parameters.AddWithValue("@email", email);

            using var rd = await cmd.ExecuteReaderAsync();
            if (!await rd.ReadAsync()) return null;

            return Map(rd);
        }

        public async Task<int> CrearAsync(Usuario u)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Usuario (Email, Nombre, Rol, ClaveHash, AvatarUrl)
                VALUES (@email, @nombre, @rol, @claveHash, @avatar);
                SELECT LAST_INSERT_ID();";
            cmd.Parameters.AddWithValue("@email", u.Email);
            cmd.Parameters.AddWithValue("@nombre", u.Nombre);
            cmd.Parameters.AddWithValue("@rol", u.Rol);
            cmd.Parameters.AddWithValue("@claveHash", u.ClaveHash);
            cmd.Parameters.AddWithValue("@avatar", u.AvatarUrl ?? (object)DBNull.Value);

            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<Usuario?> AutenticarAsync(string email, string clave)
        {
            var usuario = await ObtenerPorEmailAsync(email);
            if (usuario != null && BCrypt.Net.BCrypt.Verify(clave, usuario.ClaveHash))
                return usuario;
            return null;
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Usuario WHERE Id=@id;";
            cmd.Parameters.AddWithValue("@id", id);

            using var rd = await cmd.ExecuteReaderAsync();
            if (!await rd.ReadAsync()) return null;

            return Map(rd);
        }

        public async Task<IList<Usuario>> ObtenerTodosAsync()
        {
            var lista = new List<Usuario>();
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Usuario;";

            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
                lista.Add(Map(rd));

            return lista;
        }

        public async Task<bool> ActualizarAsync(Usuario u)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Usuario SET 
                    Nombre=@nombre, Rol=@rol, ClaveHash=@claveHash, AvatarUrl=@avatar
                WHERE Id=@id;";
            cmd.Parameters.AddWithValue("@id", u.Id);
            cmd.Parameters.AddWithValue("@nombre", u.Nombre);
            cmd.Parameters.AddWithValue("@rol", u.Rol);
            cmd.Parameters.AddWithValue("@claveHash", u.ClaveHash);
            cmd.Parameters.AddWithValue("@avatar", u.AvatarUrl ?? (object)DBNull.Value);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> BorrarAsync(int id)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Usuario WHERE Id=@id;";
            cmd.Parameters.AddWithValue("@id", id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        private static Usuario Map(IDataRecord r) => new Usuario
        {
            Id = Convert.ToInt32(r["Id"]),
            Email = r["Email"].ToString() ?? "",
            Nombre = r["Nombre"].ToString() ?? "",
            Rol = r["Rol"].ToString() ?? "",
            ClaveHash = r["ClaveHash"].ToString() ?? "",
            AvatarUrl = r["AvatarUrl"]?.ToString()
        };
    }
}

