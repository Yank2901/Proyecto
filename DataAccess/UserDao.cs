using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class UserDao : Connection
    {
        public int validarUsuario(string _name)
        {
            int aux = -1;
            using (var conexion=getConnection())
            {
                conexion.Open();
                using (var command=new SqlCommand())
                {
                    command.Connection = conexion;
                    command.CommandText= "SELECT * FROM tbCliente WHERE tbCliente.nombre = @name;";
                    command.Parameters.AddWithValue("@name", _name);
                    command.CommandType= CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) 
                    { 
                        aux = reader.GetInt32(0); 
                    }
                }
            }
            return aux;
        }

        public int validarMascota(string _name)
        {
            int aux = -1;
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "SELECT tbMascota.id, tbMascota.nombre AS Mascota, tbCliente.nombre AS Dueño " +
                                          "FROM tbCliente JOIN tbMascota ON tbCliente.id = tbMascota.idCliente  " +
                                          "WHERE (tbMascota.nombre = @name)";
                    command.Parameters.AddWithValue("@name", _name);
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read()) 
                    { 
                        aux= reader.GetInt32(0); 
                    }
                }
            }
            return aux;
        }

        public int insertarUsuario(string _name)
        {
            int aux;
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "INSERT INTO tbCliente(nombre) VALUES (@name); " +
                                          "SELECT SCOPE_IDENTITY() AS 'LastInsertedId'";
                    command.Parameters.AddWithValue("@name", _name);
                    command.CommandType = CommandType.Text;
                    aux = int.Parse(command.ExecuteScalar().ToString());
                }
            }
            return aux;
        }

        public int insertarMascota(int _idCliente, string _name)
        {
            int aux;
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "INSERT INTO tbMascota(nombre, idCliente) VALUES (@name,@idCliente) " +
                                          "SELECT SCOPE_IDENTITY() AS 'LastInsertedId' ";
                    command.Parameters.AddWithValue("@name", _name);
                    command.Parameters.AddWithValue("@idCliente", _idCliente);
                    command.CommandType = CommandType.Text;
                    aux= int.Parse(command.ExecuteScalar().ToString());
                }
            }
            return aux;
        }

        public void inicilizarAcciones(int _idMascota)
        {
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "INSERT INTO tbOrdenes(idMascota,accion,ok,nok) VALUES " +
                                          "(@idMascota, 1, 0, 0),(@idMascota, 2, 0, 0),(@idMascota, 3, 0, 0),(@idMascota, 4, 0, 0),"+
                                          "(@idMascota, 5, 0, 0),(@idMascota, 6, 0, 0),(@idMascota, 7, 0, 0),(@idMascota, 8, 0, 0); ";
                    command.Parameters.AddWithValue("@idMascota", _idMascota);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                conexion.Close();
            }
        }

        public List<string> leerAcciones(string _clientName, string _peetname)
        {
            List<string> lstAcciones= new List<string>();
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "SELECT tbCliente.nombre AS Dueño, tbMascota.nombre AS Mascota, tbOrdenes.accion AS Accion, tbOrdenes.ok AS OK, tbOrdenes.nok AS NOK " +
                                          "FROM tbCliente " +
                                          "JOIN tbMascota ON tbCliente.id = tbMascota.idCliente " +
                                          "JOIN tbOrdenes ON tbMascota.id = tbOrdenes.idMascota " +
                                          "WHERE tbCliente.nombre = @clientName AND tbMascota.nombre = @peetname";
                    command.Parameters.AddWithValue("@clientName", _clientName);
                    command.Parameters.AddWithValue("@peetname", _peetname);
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    { 
                        lstAcciones.Add(reader.GetInt32(2) + ": " + reader.GetInt32(3) + " OK, " + reader.GetInt32(4) + " NOK");
                    }
                }
            }
            return lstAcciones;
        }

        public void actualizarRegistroOK(int _idMascota, int _accion)
        {
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "UPDATE tbOrdenes SET ok = ok + 1 WHERE idMascota = @idMascota AND accion = @accion; ";
                    command.Parameters.AddWithValue("@idMascota", _idMascota);
                    command.Parameters.AddWithValue("@accion", _accion);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void actualizarRegistroNOK(int _idMascota, int _accion)
        {
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "UPDATE tbOrdenes SET nok = nok + 1 WHERE idMascota = @idMascota AND accion = @accion; ";
                    command.Parameters.AddWithValue("@idMascota", _idMascota);
                    command.Parameters.AddWithValue("@accion", _accion);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
