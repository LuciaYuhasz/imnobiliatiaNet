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
        INSERT INTO Contrato 
        (FechaInicio, FechaFin, Monto, InmuebleId, InquilinoId, UsuarioCreadorId) 
        VALUES (@inicio, @fin, @monto, @inmuebleId, @inquilinoId, @usuarioCreadorId);
        SELECT LAST_INSERT_ID();";
            Add(cmd, "@inicio", c.FechaInicio);
            Add(cmd, "@fin", c.FechaFin);
            Add(cmd, "@monto", c.Monto);
            Add(cmd, "@inmuebleId", c.InmuebleId);
            Add(cmd, "@inquilinoId", c.InquilinoId);
            Add(cmd, "@usuarioCreadorId", c.UsuarioCreadorId);

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
            c.FechaTerminacionAnticipada, c.MontoMulta, c.UsuarioCreadorId, c.UsuarioTerminadorId,
            i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
            im.Direccion AS InmuebleDireccion, im.Tipo AS InmuebleTipo,
            uc.Nombre AS UsuarioCreadorNombre,
            ut.Nombre AS UsuarioTerminadorNombre
        FROM Contrato c
        JOIN Inquilino i ON c.InquilinoId = i.Id
        JOIN Inmueble im ON c.InmuebleId = im.Id
        JOIN Usuario uc ON c.UsuarioCreadorId = uc.Id
        LEFT JOIN Usuario ut ON c.UsuarioTerminadorId = ut.Id
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
                FechaTerminacionAnticipada = rd.IsDBNull("FechaTerminacionAnticipada") ? null : rd.GetDateTime("FechaTerminacionAnticipada"),
                MontoMulta = rd.IsDBNull("MontoMulta") ? null : rd.GetDecimal("MontoMulta"),
                UsuarioCreadorId = rd.GetInt32("UsuarioCreadorId"),
                UsuarioTerminadorId = rd.IsDBNull("UsuarioTerminadorId") ? null : rd.GetInt32("UsuarioTerminadorId"),
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
                },
                UsuarioCreador = new Usuario
                {
                    Id = rd.GetInt32("UsuarioCreadorId"),
                    Nombre = rd.GetString("UsuarioCreadorNombre")
                },
                UsuarioTerminador = rd.IsDBNull("UsuarioTerminadorId") ? null : new Usuario
                {
                    Id = rd.GetInt32("UsuarioTerminadorId"),
                    Nombre = rd.GetString("UsuarioTerminadorNombre")
                }
            };
        }




        public async Task<IList<Contrato>> ObtenerTodosAsync()
        {
            var lista = new List<Contrato>();
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();

            cmd.CommandText = @"
        SELECT 
            c.Id, c.FechaInicio, c.FechaFin, c.Monto, c.InmuebleId, c.InquilinoId,
            c.FechaTerminacionAnticipada, c.MontoMulta, c.UsuarioCreadorId, c.UsuarioTerminadorId,
            i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
            im.Direccion AS InmuebleDireccion,
            uc.Nombre AS UsuarioCreadorNombre,
            ut.Nombre AS UsuarioTerminadorNombre
        FROM Contrato c
        JOIN Inquilino i ON c.InquilinoId = i.Id
        JOIN Inmueble im ON c.InmuebleId = im.Id
        JOIN Usuario uc ON c.UsuarioCreadorId = uc.Id
        LEFT JOIN Usuario ut ON c.UsuarioTerminadorId = ut.Id
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
                    FechaTerminacionAnticipada = rd.IsDBNull("FechaTerminacionAnticipada") ? null : rd.GetDateTime("FechaTerminacionAnticipada"),
                    MontoMulta = rd.IsDBNull("MontoMulta") ? null : rd.GetDecimal("MontoMulta"),
                    UsuarioCreadorId = rd.GetInt32("UsuarioCreadorId"),
                    UsuarioTerminadorId = rd.IsDBNull("UsuarioTerminadorId") ? null : rd.GetInt32("UsuarioTerminadorId"),
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
                    },
                    UsuarioCreador = new Usuario
                    {
                        Id = rd.GetInt32("UsuarioCreadorId"),
                        Nombre = rd.GetString("UsuarioCreadorNombre")
                    },
                    UsuarioTerminador = rd.IsDBNull("UsuarioTerminadorId") ? null : new Usuario
                    {
                        Id = rd.GetInt32("UsuarioTerminadorId"),
                        Nombre = rd.GetString("UsuarioTerminadorNombre")
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
            InmuebleId=@inmuebleId, InquilinoId=@inquilinoId,
            FechaTerminacionAnticipada=@terminacion, MontoMulta=@multa,
            UsuarioTerminadorId=@usuarioTerminadorId
        WHERE Id=@id;";
            Add(cmd, "@id", c.Id);
            Add(cmd, "@inicio", c.FechaInicio);
            Add(cmd, "@fin", c.FechaFin);
            Add(cmd, "@monto", c.Monto);
            Add(cmd, "@inmuebleId", c.InmuebleId);
            Add(cmd, "@inquilinoId", c.InquilinoId);
            Add(cmd, "@terminacion", c.FechaTerminacionAnticipada);
            Add(cmd, "@multa", c.MontoMulta);
            Add(cmd, "@usuarioTerminadorId", c.UsuarioTerminadorId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }



        public decimal CalcularMulta(Contrato contrato, DateTime fechaTerminacion)
        {
            var duracionTotal = (contrato.FechaFin - contrato.FechaInicio).TotalDays;
            var duracionCumplida = (fechaTerminacion - contrato.FechaInicio).TotalDays;

            if (duracionCumplida < duracionTotal / 2)
                return contrato.Monto * 2; // Menos de la mitad → 2 meses
            else
                return contrato.Monto;     // Mitad o más → 1 mes
        }
        public async Task<bool> TerminarAnticipadamenteAsync(int contratoId, DateTime fechaTerminacion, int usuarioId)
        {
            var contrato = await ObtenerPorIdAsync(contratoId);
            if (contrato == null) return false;

            var multa = CalcularMulta(contrato, fechaTerminacion);

            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"
        UPDATE Contrato SET 
            FechaTerminacionAnticipada=@fecha, MontoMulta=@multa, UsuarioTerminadorId=@usuarioId
        WHERE Id=@id;";
            Add(cmd, "@id", contratoId);
            Add(cmd, "@fecha", fechaTerminacion);
            Add(cmd, "@multa", multa);
            Add(cmd, "@usuarioId", usuarioId);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
        public async Task<bool> ExisteSuperposicionAsync(int inmuebleId, DateTime inicio, DateTime fin, int? contratoId = null)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();

            cmd.CommandText = @"
        SELECT COUNT(*) FROM Contrato
        WHERE InmuebleId = @inmuebleId
        AND NOT (FechaFin < @inicio OR FechaInicio > @fin)
        AND (@contratoId IS NULL OR Id <> @contratoId);";

            cmd.Parameters.AddWithValue("@inmuebleId", inmuebleId);
            cmd.Parameters.AddWithValue("@inicio", inicio);
            cmd.Parameters.AddWithValue("@fin", fin);
            cmd.Parameters.AddWithValue("@contratoId", contratoId ?? (object)DBNull.Value);

            var count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return count > 0;
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

