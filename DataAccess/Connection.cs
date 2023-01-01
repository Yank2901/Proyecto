using System.Data.SqlClient;

namespace DataAccess
{
    public abstract class Connection
    {
        private readonly string _connectionString;

        public Connection()
        {
            _connectionString = @"Data Source=LAPTOP-54CSS05Q\SQLEXPRESS;Initial Catalog=dbMascotas;Integrated Security=True";
        }

        protected SqlConnection getConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
