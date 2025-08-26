using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace imnobiliatiaNet.Data
{
    public class Db
    {
        private readonly string _cs;
        public Db(IConfiguration config)
        {
            _cs = config.GetConnectionString("Default")!;
        }

        public IDbConnection OpenConnection()
        {
            var conn = new MySqlConnection(_cs);
            conn.Open();
            return conn;
        }
    }
}
