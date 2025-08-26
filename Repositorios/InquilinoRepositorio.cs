using imnobiliatiaNet.Data;
using imnobiliatiaNet.Models;
using MySqlConnector;
using System.Data;

namespace imnobiliatiaNet.Repositorios
{
    public class InquilinoRepositorio : IInquilinoRepositorio
    {
        private readonly Db _db;
        public InquilinoRepositorio(Db db) => _db = db;

        public async Task<int> CrearAsync(Inquilino i)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Inquilino (Nombre, Apellido, DNI, Telefono, Email)
                VALUES (@nombre, @apellido, @dni, @telefono, @email);
                SELECT LAST_INSERT_ID();";
            Add(cmd, "@nombre", i.Nombre);
            Add(cmd, "@apellido", i.Apellido);
            Add(cmd, "@dni", i.DNI);
            Add(cmd, "@telefono", i.Telefono);
            Add(cmd, "@email", i.Email);

            var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return id;
        }

        public async Task<Inquilino?> ObtenerPorIdAsync(int id)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Inquilino WHERE Id=@id;";
            Add(cmd, "@id", id);

            using var rd = await cmd.ExecuteReaderAsync();
            if (!await rd.ReadAsync()) return null;
            return Map(rd);
        }

        public async Task<IList<Inquilino>> ListarAsync(string? filtro = null)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();

            if (string.IsNullOrWhiteSpace(filtro))
            {
                cmd.CommandText = @"SELECT * FROM Inquilino ORDER BY Apellido, Nombre;";
            }
            else
            {
                cmd.CommandText = @"SELECT * FROM Inquilino
                                    WHERE Apellido LIKE @f OR Nombre LIKE @f OR DNI LIKE @f
                                    ORDER BY Apellido, Nombre;";
                Add(cmd, "@f", $"%{filtro}%");
            }

            var lista = new List<Inquilino>();
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
                lista.Add(Map(rd));

            return lista;
        }

        public async Task<bool> ActualizarAsync(Inquilino i)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Inquilino
                SET Nombre=@nombre, Apellido=@apellido, DNI=@dni,
                    Telefono=@telefono, Email=@email
                WHERE Id=@id;";
            Add(cmd, "@id", i.Id);
            Add(cmd, "@nombre", i.Nombre);
            Add(cmd, "@apellido", i.Apellido);
            Add(cmd, "@dni", i.DNI);
            Add(cmd, "@telefono", i.Telefono);
            Add(cmd, "@email", i.Email);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> BorrarAsync(int id)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Inquilino WHERE Id=@id;";
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

        private static Inquilino Map(IDataRecord r) => new Inquilino
        {
            Id = Convert.ToInt32(r["Id"]),
            Nombre = r["Nombre"]?.ToString() ?? "",
            Apellido = r["Apellido"]?.ToString() ?? "",
            DNI = r["DNI"]?.ToString() ?? "",
            Telefono = r["Telefono"]?.ToString() ?? "",
            Email = r["Email"]?.ToString() ?? ""
        };
    }
}


