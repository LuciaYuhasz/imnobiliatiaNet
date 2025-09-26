using imnobiliatiaNet.Data;
using imnobiliatiaNet.Models;
using MySqlConnector;
using System.Data;

namespace imnobiliatiaNet.Repositorios
{
    public class PagoRepositorio : IPagoRepositorio
    {
        private readonly Db _db;
        public PagoRepositorio(Db db) => _db = db;

        public async Task<int> CrearAsync(Pago p)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"
        INSERT INTO Pago 
        (ContratoId, NumeroPago, FechaPago, Concepto, Importe, UsuarioAltaId, FechaCreacion, FechaModificacion)
        VALUES 
        (@contratoId, @numero, @fecha, @concepto, @importe, @usuarioAltaId, curdate(), curdate());
        SELECT LAST_INSERT_ID();";
            Add(cmd, "@contratoId", p.ContratoId);
            Add(cmd, "@numero", p.NumeroPago);
            Add(cmd, "@fecha", p.FechaPago);
            Add(cmd, "@concepto", p.Concepto);
            Add(cmd, "@importe", p.Importe);
            Add(cmd, "@usuarioAltaId", p.UsuarioAltaId);

            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }


        public async Task<Pago?> ObtenerPorIdAsync(int id)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Pago WHERE Id=@id;";
            Add(cmd, "@id", id);

            using var rd = await cmd.ExecuteReaderAsync();
            if (!await rd.ReadAsync()) return null;
            return Map(rd);
        }

        public async Task<IList<Pago>> ObtenerPorContratoAsync(int contratoId)
        {
            var lista = new List<Pago>();
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Pago WHERE ContratoId=@id ORDER BY NumeroPago;";
            Add(cmd, "@id", contratoId);

            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
                lista.Add(Map(rd));

            return lista;
        }

        public async Task<bool> ActualizarConceptoAsync(Pago p)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = "UPDATE Pago SET Concepto=@concepto WHERE Id=@id;";
            Add(cmd, "@id", p.Id);
            Add(cmd, "@concepto", p.Concepto);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> AnularAsync(int id, int usuarioId, string motivo)
        {
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();
            cmd.CommandText = @"
        UPDATE Pago SET 
            Anulado=TRUE, 
            UsuarioAnulacionId=@usuarioId, 
            MotivoAnulacion=@motivo, 
            FechaModificacion=curdate()
        WHERE Id=@id;";
            Add(cmd, "@id", id);
            Add(cmd, "@usuarioId", usuarioId);
            Add(cmd, "@motivo", motivo);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
        public async Task<Paginador<Pago>> ObtenerPaginadoAsync(int contratoId, int pagina, int tamPagina)
        {
            var resultado = new Paginador<Pago> { PaginaActual = pagina };
            using var conn = _db.OpenConnection();
            using var cmd = (MySqlCommand)conn.CreateCommand();

            // Total de registros
            cmd.CommandText = @"
        SELECT COUNT(*) FROM Pago
        WHERE ContratoId = @contratoId;";
            Add(cmd, "@contratoId", contratoId);
            resultado.TotalPaginas = (int)Math.Ceiling(Convert.ToInt32(await cmd.ExecuteScalarAsync()) / (double)tamPagina);

            // Datos paginados
            cmd.CommandText = @"
        SELECT p.*, u.Nombre AS UsuarioAltaNombre
        FROM Pago p
        LEFT JOIN Usuario u ON p.UsuarioAltaId = u.Id
        WHERE p.ContratoId = @contratoId
        ORDER BY p.FechaPago DESC
        LIMIT @limit OFFSET @offset;";
            Add(cmd, "@limit", tamPagina);
            Add(cmd, "@offset", (pagina - 1) * tamPagina);

            using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                var pago = new Pago
                {
                    Id = rd.GetInt32("Id"),
                    ContratoId = rd.GetInt32("ContratoId"),
                    NumeroPago = rd.GetInt32("NumeroPago"),
                    FechaPago = rd.GetDateTime("FechaPago"),
                    Concepto = rd.GetString("Concepto"),
                    Importe = rd.GetDecimal("Importe"),
                    Anulado = rd.GetBoolean("Anulado"),
                    UsuarioAltaId = rd.GetInt32("UsuarioAltaId"),
                    UsuarioAlta = new Usuario
                    {
                        Id = rd.GetInt32("UsuarioAltaId"),
                        Nombre = rd.GetString("UsuarioAltaNombre")
                    }
                };
                resultado.Items.Add(pago);
            }

            return resultado;
        }


        private static void Add(MySqlCommand cmd, string nombre, object? valor)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = nombre;
            p.Value = valor ?? DBNull.Value;
            cmd.Parameters.Add(p);
        }

        private static Pago Map(IDataRecord r) => new Pago
        {
            Id = Convert.ToInt32(r["Id"]),
            ContratoId = Convert.ToInt32(r["ContratoId"]),
            NumeroPago = Convert.ToInt32(r["NumeroPago"]),
            FechaPago = Convert.ToDateTime(r["FechaPago"]),
            Concepto = r["Concepto"].ToString() ?? "",
            Importe = Convert.ToDecimal(r["Importe"]),
            Anulado = Convert.ToBoolean(r["Anulado"]),
            UsuarioAltaId = r["UsuarioAltaId"] as int?,
            UsuarioAnulacionId = r["UsuarioAnulacionId"] as int?,
            FechaCreacion = r["FechaCreacion"] as DateTime?,
            FechaModificacion = r["FechaModificacion"] as DateTime?,
            MotivoAnulacion = r["MotivoAnulacion"]?.ToString()
        };

    }
}
