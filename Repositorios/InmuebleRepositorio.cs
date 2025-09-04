using System.Data;
using imnobiliatiaNet.Data;
using imnobiliatiaNet.Models;
using MySqlConnector;

namespace imnobiliatiaNet.Repositorios
{
    public class InmuebleRepositorio : IInmuebleRepositorio
    {
        private readonly Db _db;
        public InmuebleRepositorio(Db db) => _db = db;

        public async Task<int> AltaAsync(Inmueble i)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();

            cmd.CommandText = $@"
        INSERT INTO Inmueble
        ({nameof(i.Direccion)}, {nameof(i.Uso)}, {nameof(i.Tipo)}, {nameof(i.Ambientes)},
         {nameof(i.Precio)}, {nameof(i.Disponible)}, {nameof(i.Latitud)}, {nameof(i.Longitud)}, {nameof(i.PropietarioId)})
        VALUES (@{nameof(i.Direccion)}, @{nameof(i.Uso)}, @{nameof(i.Tipo)}, @{nameof(i.Ambientes)},
                @{nameof(i.Precio)}, @{nameof(i.Disponible)}, @{nameof(i.Latitud)}, @{nameof(i.Longitud)}, @{nameof(i.PropietarioId)});
        SELECT LAST_INSERT_ID();";

            Add(cmd, "@" + nameof(i.Direccion), i.Direccion);
            Add(cmd, "@" + nameof(i.Uso), i.Uso);
            Add(cmd, "@" + nameof(i.Tipo), i.Tipo);
            Add(cmd, "@" + nameof(i.Ambientes), i.Ambientes);
            Add(cmd, "@" + nameof(i.Precio), i.Precio);
            Add(cmd, "@" + nameof(i.Disponible), i.Disponible);
            Add(cmd, "@" + nameof(i.Latitud), i.Latitud);
            Add(cmd, "@" + nameof(i.Longitud), i.Longitud);
            Add(cmd, "@" + nameof(i.PropietarioId), i.PropietarioId);

            var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return id;
        }


        public async Task<Inmueble?> ObtenerPorIdAsync(int id)
        {
            using var conn = _db.OpenConnection();
            //            using var cmd = conn.CreateCommand();
            using var cmd = (MySqlCommand)conn.CreateCommand();

            cmd.CommandText = @"SELECT i.*, p.Apellido, p.Nombre
                                FROM Inmueble i
                                JOIN Propietario p ON p.Id = i.PropietarioId
                                WHERE i.Id=@id;";
            Add(cmd, "@id", id);

            using var rd = await cmd.ExecuteReaderAsync();
            if (!await rd.ReadAsync()) return null;
            return Map(rd);
        }

        public async Task<IList<Inmueble>> ObtenerTodosAsync(string? filtro = null, bool? disponibles = null)
        {
            using var conn = _db.OpenConnection();
            // using var cmd = conn.CreateCommand();

            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"SELECT i.*, p.Apellido, p.Nombre
                                FROM Inmueble i
                                JOIN Propietario p ON p.Id = i.PropietarioId
                                WHERE 1=1 ";
            if (!string.IsNullOrWhiteSpace(filtro))
            {
                cmd.CommandText += " AND (i.Direccion LIKE @f OR i.Tipo LIKE @f) ";
                Add(cmd, "@f", $"%{filtro}%");
            }
            if (disponibles.HasValue)
            {
                cmd.CommandText += " AND i.Disponible=@disp ";
                Add(cmd, "@disp", disponibles.Value);
            }
            cmd.CommandText += " ORDER BY i.Direccion;";

            var lista = new List<Inmueble>();
            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
                lista.Add(Map(rd));
            return lista;
        }

        public async Task<bool> ModificarAsync(Inmueble i)
        {
            using var conn = _db.OpenConnection();
            // using var cmd = conn.CreateCommand();
            using var cmd = (MySqlCommand)conn.CreateCommand();

            cmd.CommandText = @"
                UPDATE Inmueble SET
                Direccion=@dir, Uso=@uso, Tipo=@tipo,
                Ambientes=@amb, Precio=@precio, Disponible=@disp,
                Latitud=@lat, Longitud=@long, PropietarioId=@propId
                WHERE Id=@id;";
            Add(cmd, "@id", i.Id);
            Add(cmd, "@dir", i.Direccion);
            Add(cmd, "@uso", i.Uso);
            Add(cmd, "@tipo", i.Tipo);
            Add(cmd, "@amb", i.Ambientes);
            Add(cmd, "@precio", i.Precio);
            Add(cmd, "@disp", i.Disponible);
            Add(cmd, "@lat", i.Latitud);
            Add(cmd, "@long", i.Longitud);
            Add(cmd, "@propId", i.PropietarioId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> BajaAsync(int id)
        {
            using var conn = _db.OpenConnection();
            // using var cmd = conn.CreateCommand();
            using var cmd = (MySqlCommand)conn.CreateCommand();

            cmd.CommandText = "DELETE FROM Inmueble WHERE Id=@id;";
            Add(cmd, "@id", id);
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        private static void Add(IDbCommand cmd, string nombre, object? valor)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = nombre;
            p.Value = valor ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        private static Inmueble Map(IDataRecord r) => new Inmueble
        {
            Id = Convert.ToInt32(r["Id"]),
            Direccion = r["Direccion"].ToString() ?? "",
            Uso = r["Uso"].ToString() ?? "",
            Tipo = r["Tipo"].ToString() ?? "",
            Ambientes = Convert.ToInt32(r["Ambientes"]),
            Precio = Convert.ToDecimal(r["Precio"]),
            Disponible = Convert.ToBoolean(r["Disponible"]),
            Latitud = r["Latitud"] as double?,
            Longitud = r["Longitud"] as double?,
            //Latitud = r["Latitud"] != DBNull.Value ? Convert.ToDecimal(r["Latitud"]) : null,
            //Longitud = r["Longitud"] != DBNull.Value ? Convert.ToDecimal(r["Longitud"]) : null,

            PropietarioId = Convert.ToInt32(r["PropietarioId"]),
            Propietario = new Propietario
            {
                Id = Convert.ToInt32(r["PropietarioId"]),
                Apellido = r["Apellido"].ToString() ?? "",
                Nombre = r["Nombre"].ToString() ?? ""
            }
        };
    }
}
