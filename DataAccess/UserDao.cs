using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace DataAccess
{
    public class UserDao : Connection
    {
        public bool validarUsuario(string _name)
        {
            bool aux;
            using (var conexion=getConnection())
            {
                conexion.Open();
                using (var command=new SQLiteCommand())
                {
                    command.Connection = conexion;
                    command.CommandText= "SELECT * FROM tbCliente WHERE tbCliente.Nombre = @name;";
                    command.Parameters.AddWithValue("@name", _name);
                    command.CommandType= CommandType.Text;
                    aux= command.ExecuteReader().HasRows? true: false;
                }
            }
            return aux;
        }

        public bool validarMascota(string _name)
        {
            bool aux;
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SQLiteCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "SELECT tbCliente.Nombre AS Dueño, tbMascota.Nombre AS Mascota"+ 
                                          "FROM tbCliente"+ 
                                          "JOIN tbMascota ON tbCliente.Id = tbMascota.Id_Cliente"+
                                          "WHERE Mascota = @name";
                    command.Parameters.AddWithValue("@name", _name);
                    command.CommandType = CommandType.Text; 
                    aux = command.ExecuteReader().HasRows ? true : false;
                }
            }
            return aux;
        }

        public int insertarNombre(string _name)
        {
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SQLiteCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "INSERT INTO tbCliente (Nombre) VALUES (@nombre);"+
                                          "SELECT last_insert_rowid(); ";
                    command.Parameters.AddWithValue("@name", _name);
                    command.CommandType = CommandType.Text;
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public int insertarMascota(int _idCliente, string _name)
        {
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SQLiteCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "INSERT INTO tbMascota (Nombre, Id_Cliente) VALUES (@name,@idCliente);"+
                                          "SELECT last_insert_rowid(); ";
                    command.Parameters.AddWithValue("@name", _name);
                    command.Parameters.AddWithValue("@idCliente", _idCliente);
                    command.CommandType = CommandType.Text;
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public void inicilizarAcciones(int _idMascota)
        {
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SQLiteCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "INSERT INTO tbOrdenes (\"Id_Mascota\", \"Accion\", \"OK\", \"NOK\") VALUES"+
                                          "(@idMascota, 1, 0, 0),(@idMascota, 2, 0, 0),(@idMascota, 3, 0, 0),(@idMascota, 4, 0, 0),"+
                                          "(@idMascota, 5, 0, 0),(@idMascota, 6, 0, 0),(@idMascota, 7, 0, 0),(@idMascota, 8, 0, 0); ";
                    command.Parameters.AddWithValue("@idMascota", _idMascota);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool leerAcciones(string _clientName, string _peetname)
        {
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SQLiteCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "SELECT tbCliente.Nombre AS Dueño, tbMascota.Nombre AS Mascota, tbOrdenes.Accion AS Accion, tbOrdenes.OK AS OK, tbOrdenes.NOK AS NOK"+
                                          "FROM tbCliente"+
                                          "JOIN tbMascota ON tbCliente.Id = tbMascota.Id_Cliente"+
                                          "JOIN tbOrdenes ON tbMascota.Id = tbOrdenes.Id_Mascota"+
                                          "WHERE Dueño = @clientName AND Mascota = @peetname";
                    command.Parameters.AddWithValue("@clientName", _clientName);
                    command.Parameters.AddWithValue("@peetname", _peetname);
                    command.CommandType = CommandType.Text;
                    SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetString(2) + ": " + reader.GetString(3) + " OK, " + reader.GetString(4) + " NOK");
                        }
                        return true;
                    }
                    return false;
                }
            }
        }

        public void actualizarRegistro(int _idMascota, int _accion, string _opcion)
        {
            using (var conexion = getConnection())
            {
                conexion.Open();
                using (var command = new SQLiteCommand())
                {
                    command.Connection = conexion;
                    command.CommandText = "UPDATE tbOrdenes SET @opcion = @opcion + 1 WHERE Id_Mascota = @idMascota AND Accion = @accion; ";
                    command.Parameters.AddWithValue("@opcion", _opcion);
                    command.Parameters.AddWithValue("@idMascota", _idMascota);
                    command.Parameters.AddWithValue("@accion", _accion);
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
