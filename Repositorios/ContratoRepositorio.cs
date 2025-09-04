using System.Data;
using imnobiliatiaNet.Data;
using imnobiliatiaNet.Models;
using MySqlConnector;

namespace imnobiliatiaNet.Repositorios
{
    public class ContratoRepositorio : IContratoRepositorio
    {
        private readonly Db _db;
        public ContratoRepositorio(Db db) => _db = db;

        public async Task<int> AltaAsync(Contrato c)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Contrato (FechaInicio, FechaFin, Monto, InmuebleId, InquilinoId) 
                VALUES (@inicio, @fin, @monto, @inmuebleId, @inquilinoId);
                SELECT LAST_INSERT_ID();";
            Add(cmd, "@inicio", c.FechaInicio);
            Add(cmd, "@fin", c.FechaFin);
            Add(cmd, "@monto", c.Monto);
            Add(cmd, "@inmuebleId", c.InmuebleId);
            Add(cmd, "@inquilinoId", c.InquilinoId);

            var id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return id;
        }
        public async Task<Contrato?> ObtenerPorIdAsync(int id)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();

            cmd.CommandText = @"
        SELECT 
            c.Id, c.FechaInicio, c.FechaFin, c.Monto, c.InmuebleId, c.InquilinoId,
            i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
            im.Direccion AS InmuebleDireccion, im.Tipo AS InmuebleTipo
        FROM Contrato c
        JOIN Inquilino i ON c.InquilinoId = i.Id
        JOIN Inmueble im ON c.InmuebleId = im.Id
        WHERE c.Id = @id;";
            Add(cmd, "@id", id);

            using var rd = await cmd.ExecuteReaderAsync();
            if (!await rd.ReadAsync()) return null;

            return new Contrato
            {
                Id = rd.GetInt32("Id"),
                FechaInicio = rd.GetDateTime("FechaInicio"),
                FechaFin = rd.GetDateTime("FechaFin"),
                Monto = rd.GetDecimal("Monto"),
                InquilinoId = rd.GetInt32("InquilinoId"),
                InmuebleId = rd.GetInt32("InmuebleId"),
                Inquilino = new Inquilino
                {
                    Id = rd.GetInt32("InquilinoId"),
                    Nombre = rd.GetString("InquilinoNombre"),
                    Apellido = rd.GetString("InquilinoApellido")
                },
                Inmueble = new Inmueble
                {
                    Id = rd.GetInt32("InmuebleId"),
                    Direccion = rd.GetString("InmuebleDireccion"),
                    Tipo = rd.GetString("InmuebleTipo")
                }
            };
        }

        /*public async Task<Contrato?> ObtenerPorIdAsync(int id)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Id, FechaInicio, FechaFin, Monto, InmuebleId, InquilinoId 
                FROM Contrato WHERE Id = @id;";
            Add(cmd, "@id", id);

            using var rd = await cmd.ExecuteReaderAsync();
            if (!await rd.ReadAsync()) return null;

            return Map(rd);
        }*/
        public async Task<IList<Contrato>> ObtenerTodosAsync()
        {
            var lista = new List<Contrato>();
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();

            cmd.CommandText = @"
        SELECT 
            c.Id, c.FechaInicio, c.FechaFin, c.Monto, c.InmuebleId, c.InquilinoId,
            i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
            im.Direccion AS InmuebleDireccion
        FROM Contrato c
        JOIN Inquilino i ON c.InquilinoId = i.Id
        JOIN Inmueble im ON c.InmuebleId = im.Id
        ORDER BY c.FechaInicio DESC;";

            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var contrato = new Contrato
                {
                    Id = rd.GetInt32("Id"),
                    FechaInicio = rd.GetDateTime("FechaInicio"),
                    FechaFin = rd.GetDateTime("FechaFin"),
                    Monto = rd.GetDecimal("Monto"),
                    InquilinoId = rd.GetInt32("InquilinoId"),
                    InmuebleId = rd.GetInt32("InmuebleId"),
                    Inquilino = new Inquilino
                    {
                        Id = rd.GetInt32("InquilinoId"),
                        Nombre = rd.GetString("InquilinoNombre"),
                        Apellido = rd.GetString("InquilinoApellido")
                    },
                    Inmueble = new Inmueble
                    {
                        Id = rd.GetInt32("InmuebleId"),
                        Direccion = rd.GetString("InmuebleDireccion")
                    }
                };

                lista.Add(contrato);
            }

            return lista;
        }


        public async Task<bool> ModificarAsync(Contrato c)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE Contrato SET 
                    FechaInicio=@inicio, FechaFin=@fin, Monto=@monto, 
                    InmuebleId=@inmuebleId, InquilinoId=@inquilinoId 
                WHERE Id=@id;";
            Add(cmd, "@id", c.Id);
            Add(cmd, "@inicio", c.FechaInicio);
            Add(cmd, "@fin", c.FechaFin);
            Add(cmd, "@monto", c.Monto);
            Add(cmd, "@inmuebleId", c.InmuebleId);
            Add(cmd, "@inquilinoId", c.InquilinoId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> BajaAsync(int id)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Contrato WHERE Id=@id;";
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

        private static Contrato Map(IDataRecord r) => new Contrato
        {
            Id = Convert.ToInt32(r["Id"]),
            FechaInicio = Convert.ToDateTime(r["FechaInicio"]),
            FechaFin = Convert.ToDateTime(r["FechaFin"]),
            Monto = Convert.ToDecimal(r["Monto"]),
            InmuebleId = Convert.ToInt32(r["InmuebleId"]),
            InquilinoId = Convert.ToInt32(r["InquilinoId"])
        };
    }
}

