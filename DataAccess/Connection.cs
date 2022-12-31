using System.Data.SQLite;

namespace DataAccess
{
    public abstract class Connection
    {
        private readonly string _connectionString;

        public Connection()
        {
            _connectionString = @"Data Source=.\dbMascotas.db;Version=3;";
        }

        protected SQLiteConnection getConnection()
        {
            return new SQLiteConnection(_connectionString);
        }
    }
}
