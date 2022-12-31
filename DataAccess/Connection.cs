using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace DataAccess
{
    public abstract class Connection
    {
        private readonly string _connectionString;

        public Connection()
        {
            _connectionString = "Data Source=dbMascotas.db;Version=3;UTF8Encoding=True;";
        }

        protected SQLiteConnection getConnection()
        {
            return new SQLiteConnection(_connectionString);
        }
    }
}
