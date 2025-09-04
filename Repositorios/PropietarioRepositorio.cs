using imnobiliatiaNet.Data;
using imnobiliatiaNet.Models;
using MySqlConnector;
using System.Data;

namespace imnobiliatiaNet.Repositorios
{
    public class PropietarioRepositorio : IPropietarioRepositorio
    {
        private readonly Db _db;
        public PropietarioRepositorio(Db db) => _db = db;





        public async Task<int> CrearAsync(Propietario p)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"
        INSERT INTO Propietario (Dni, Apellido, Nombre, Telefono, Email)
        VALUES (@dni, @apellido, @nombre, @telefono, @correo);
        SELECT LAST_INSERT_ID();";
            Add(cmd, "@dni", p.Dni);
            Add(cmd, "@apellido", p.Apellido);
            Add(cmd, "@nombre", p.Nombre);
            Add(cmd, "@telefono", p.Telefono);
            Add(cmd, "@correo", p.Email);

            var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return id;
        }


        public async Task<Propietario?> ObtenerPorIdAsync(int id)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Propietario WHERE Id=@id;";
            Add(cmd, "@id", id);

            using var rd = await cmd.ExecuteReaderAsync();
            if (!await rd.ReadAsync()) return null;
            return Map(rd);
        }

        public async Task<IList<Propietario>> ListarAsync(string? filtro = null)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();

            if (string.IsNullOrWhiteSpace(filtro))
            {
                cmd.CommandText = @"SELECT * FROM Propietario ORDER BY Apellido, Nombre;";
            }
            else
            {
                cmd.CommandText = @"SELECT * FROM Propietario
                                    WHERE Apellido LIKE @f OR Nombre LIKE @f OR Dni LIKE @f
                                    ORDER BY Apellido, Nombre;";
                Add(cmd, "@f", $"%{filtro}%");
            }

            var lista = new List<Propietario>();
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
                lista.Add(Map(rd));

            return lista;
        }

        //metodo para el inmueble
        public async Task<IList<Propietario>> ObtenerTodosAsync()
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Propietario ORDER BY Apellido, Nombre;";

            var lista = new List<Propietario>();
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
                lista.Add(Map(rd));

            return lista;
        }


        public async Task<bool> ActualizarAsync(Propietario p)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Propietario
                SET Dni=@dni, Apellido=@apellido, Nombre=@nombre,
                    Telefono=@telefono, Email=@correo
                WHERE Id=@id;";
            Add(cmd, "@id", p.Id);
            Add(cmd, "@dni", p.Dni);
            Add(cmd, "@apellido", p.Apellido);
            Add(cmd, "@nombre", p.Nombre);
            Add(cmd, "@telefono", p.Telefono);
            Add(cmd, "@correo", p.Email);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> BorrarAsync(int id)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Propietario WHERE Id=@id;";
            Add(cmd, "@id", id);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        private static void Add(MySqlCommand cmd, string nombre, object? valor)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = nombre;
            p.Value = valor ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        private static Propietario Map(IDataRecord r) => new Propietario
        {
            Id = Convert.ToInt32(r["Id"]),
            Dni = r["Dni"]?.ToString() ?? "",
            Apellido = r["Apellido"]?.ToString() ?? "",
            Nombre = r["Nombre"]?.ToString() ?? "",
            Telefono = r["Telefono"]?.ToString() ?? "",
            Email = r["Email"]?.ToString() ?? ""
        };
    }
}
