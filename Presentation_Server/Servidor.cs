using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DomainLayer;
namespace Presentation_Server
{
    public class Servidor
    {
        static UserModel user=new UserModel();
        static void Main(string[] args)
        {
            IPAddress localAddress = GetLocalIPAddress();
            // Crea un socket del servidor y espera a recibir conexiones de múltiples clientes
            TcpListener server = new TcpListener(localAddress, 5000);
            server.Start();
            Console.WriteLine("Esperando a conexiones de clientes...");

            while (true)
            {
                // Acepta una conexión del cliente y crea un hilo de ejecución para manejar la conexión con ese cliente
                TcpClient client = server.AcceptTcpClient();
                Thread thread = new Thread(new ParameterizedThreadStart(HandleClient));
                thread.Start(client);
            }
        }

        static void HandleClient(object obj)
        {
            string opc;
            // Convierte el objeto recibido en un TcpClient
            TcpClient client = (TcpClient)obj;
            Console.WriteLine("Conexión establecida con el cliente.");

            NetworkStream stream = client.GetStream();
            sendMessage("ENTRENANDO A TU PERRO UN DÍA A LA VEZ\n", stream);
            do
            {
                // Envía el menú al cliente a través del socket
                string menu = "Que accion quieres realizar:\n" +
                              "1. Consultar mascota\n" +
                              "2. Generar nuevo registro de mascota\n" +
                              "3. Listar informacion de ordenes\n" +
                              "4. Cerrar conexión\n\n" +
                              "Ingrese el número de la acción a realizar: ";
                sendMessage(menu, stream);
                // Recibe la respuesta del cliente y almacenala en una variable
                opc = receiveMessage(stream);
                // Utiliza una estructura de control de flujo para determinar qué acción debes realizar en base a la respuesta del cliente
                if (opc == "1")
                {
                    verificarInscripcion(stream);
                }
                else if (opc == "2")
                {
                    actualizarRegistro(stream);
                }
                else if (opc == "3")
                {
                    imprimirAcciones(stream);
                }
                else if (opc == "4")
                {
                    sendMessage("Gracias por usar nuestro servicio",stream);
                    client.Close();
                    Console.WriteLine("Se ha desconectado un cliente"); 
                    break;
                }
                else
                {
                    sendMessage("Acción Invalida\n\n",stream);   
                }
            } while (true);
            stream.Close();
        }

        static void sendMessage(string message, NetworkStream stream)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
            catch
            {
            }
        }

        static string receiveMessage(NetworkStream stream)
        {
            try
            {
                byte[] responseData = new byte[1024];
                int bytesReceived = stream.Read(responseData, 0, responseData.Length);
                string response = Encoding.UTF8.GetString(responseData, 0, bytesReceived);
                return response;
            }
            catch
            {
                return null;
            }
        }

        static void verificarInscripcion(NetworkStream stream)
        {
            Cliente _client;
            Mascota _peet;
            sendMessage("Ingrese la orden para listar los registros de la mascota: ", stream);
            _client = new Cliente(receiveMessage(stream));

            // Verificamos que el dueño exista caso contrario creamos el usuario
            _client.Id = user.validarUsuario(_client.Nombre);
            if (_client.Id == -1)
            {
                _client.Id = user.insertarUsuario(_client.Nombre);
            }

            sendMessage("Ingresa el nombre de la mascota: ",stream);
            _peet = new Mascota(receiveMessage(stream), _client.Id);
            // Verificamos que la mascota exista
            _peet.Id = user.validarMascota(_peet.Nombre);
            if (_peet.Id == -1)
            {
                sendMessage("OK",stream);
                //Registramos la nueva mascota
                _peet.Id = user.insertarMascota(_client.Id, _peet.Nombre);
                // Generamos una lista de acciones de la mascota y las iniciamos en 0 tanto OK y NOK
                user.inicilizarAcciones(_peet.Id);
            }
            else
            {
                sendMessage("NOK",stream);
            }
        }

        static void actualizarRegistro(NetworkStream stream)
        {
            string[] acciones = { "1. Siéntate", "2. Échate", "3. Giro 180", "4. Quédate", "5. Aquí", "6. Muertito", "7. Lugar", "8. Junto" };
            Cliente _client;
            Mascota _peet;

            sendMessage( "Ingrese la orden para generar un nuevo registro para una mascota: ",stream);
            string[] _reg = receiveMessage(stream).Split(',');

            // Validamos que la sentencia ingresada por consola tenga 4 atributos separados por ,
            if (_reg.Length != 4)
            {
                sendMessage( "Sentencia invalida",stream);
                return;
            }

            // Verificamos que el dueño exista
            _client = new Cliente(_reg[0]);
            _client.Id = user.validarUsuario(_reg[0]);
            if (_client.Id == -1)
            {
                sendMessage( "Dueño no encontrado\nPuede añadir el nuevo dueño y su mascota en la opcion 1", stream);
                return;
            }

            // Verificamos que la mascota exista
            _peet = new Mascota(_reg[1], _client.Id);
            _peet.Id = user.validarMascota(_peet.Nombre);
            if (_peet.Id == -1)
            {
                sendMessage( "Mascota no encontrado\nPuede añadir su mascota en la opcion 1",stream);
                return;
            }

            //Verificamos que la opcion seleccionada este en el listado
            if (int.Parse(_reg[2]) < 1 || int.Parse(_reg[2]) > acciones.Length)
            {
                sendMessage("Orden invalida, recuerda que las acciones disponibles son:",stream);
                foreach (var a in acciones)
                {
                    sendMessage(a+"\n",stream);
                }
                return;
            }

            // Verificamos que la orden tenga el valor OK o NOK
            if (!string.Equals(_reg[3], "OK") && !string.Equals(_reg[3], "NOK"))
            {
                sendMessage("Resultado de la orden invalido\nRecuerda que el resultado de la orden solo puede ser:\n"+ 
                            "OK: en caso de cumplirse la orden\nNOK: en caso de no cumplirse la orden",stream);
                return;
            }

            //Verificado todos los campos procedemos a cambiar el registro
            if (_reg[3] == "OK")
                user.actualizarRegistroOK(_peet.Id, int.Parse(_reg[2]));
            else
                user.actualizarRegistroNOK(_peet.Id, int.Parse(_reg[2]));
        }

        static void imprimirAcciones(NetworkStream stream)
        {
            List<string> lstAcciones;
            Cliente _client;
            Mascota _peet;

            sendMessage("Ingrese el nombre de la mascota a consultar: ",stream);
            string[] _reg =receiveMessage(stream).Split(':');

            // Validamos que la sentencia ingresada por consola tenga 2 atributos separados por :
            if (_reg.Length != 2)
            {
                sendMessage("Sentencia invalida",stream);
                return;
            }

            // Verificamos que el dueño exista
            _client = new Cliente(_reg[0]);
            _client.Id = user.validarUsuario(_reg[0]);
            if (_client.Id == -1)
            {
                sendMessage("Dueño no encontrado\nPuede añadir el nuevo dueño y su mascota en la opcion 1",stream);
                return;
            }

            // Verificamos que la mascota exista
            _peet = new Mascota(_reg[1], _client.Id);
            _peet.Id = user.validarMascota(_peet.Nombre);
            if (_peet.Id == -1)
            {
                sendMessage("Mascota no encontrado\nPuede añadir su mascota en la opcion 1",stream);
                return;
            }

            //Una vez verificado los parametros pasamos a realizar la peticion s la base de datos
            string acciones="";
            lstAcciones = user.leerAcciones(_client.Nombre, _peet.Nombre);
            foreach (string a in lstAcciones)
            {
                acciones += a + "\n";
            }
            sendMessage(acciones, stream);
        }

        static IPAddress GetLocalIPAddress()
        {
            // Obtener la dirección IP de la interfaz de red local
            IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(address))
                {
                    return address;
                }
            }
            throw new Exception("No se ha podido obtener la dirección IP local.");
        }


    }
}