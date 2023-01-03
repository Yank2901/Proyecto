using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation_Client
{
    public class Cliente
    {
        /*
        static void Main(string[] args)
        {
            // Crea un socket del cliente y establece una conexión con el servidor
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 1234);

            // Recibe el menú del servidor a través del socket
            NetworkStream stream = client.GetStream();
            byte[] data = new byte[1024];
            byte[] responseData= new byte[1024];
            int bytesReceived;
            string message;
            string response;
            while (true)
            {
                bytesReceived = stream.Read(data, 0, data.Length);
                message = Encoding.UTF8.GetString(data, 0, bytesReceived);
                Console.WriteLine(message);
                if (message == "Gracias por usar nuestro servicio")
                {
                    break;
                }
                response=Console.ReadLine();
                 responseData= Encoding.UTF8.GetBytes(response);
                stream.Write(responseData, 0, responseData.Length);
            }

            stream.Close();
            client.Close();
        }
    */
        /*
        static void Main(string[] args)
        {
            // Crea un socket del cliente y establece una conexión con el servidor
            TcpClient client = new TcpClient();
            client.Connect("127.0.0.1", 1234);

            // Crea un hilo para recibir mensajes del servidor
            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start(client);

            // Pide al usuario que elija una opción del menú
            string response = Console.ReadLine();

            // Envía la respuesta del usuario al servidor a través del socket
            NetworkStream stream = client.GetStream();
            byte[] responseData = Encoding.UTF8.GetBytes(response);
            stream.Write(responseData, 0, responseData.Length);

            // Espera a que el hilo termine
            receiveThread.Join();

            stream.Close();
            client.Close();
        }

        static void ReceiveMessages(object obj)
        {
            TcpClient client = (TcpClient)obj;

            // Recibe mensajes del servidor hasta que el servidor envía un mensaje de "Gracias por usar nuestro servicio"
            while (true)
            {
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[1024];
                int bytesReceived = stream.Read(data, 0, data.Length);
                string message = Encoding.UTF8.GetString(data, 0, bytesReceived);
                Console.WriteLine(message);

                if (message == "Gracias por usar nuestro servicio")
                {
                    break;
                }
            }
        }
        */
        static void Main(string[] args)
        {
            // Pedir al usuario que ingrese la dirección IP del servidor al que desea conectarse
            //Console.Write("Ingresa la dirección IP del servidor: ");
            //string serverAddress = Console.ReadLine();

            // Convertir la dirección IP ingresada por el usuario a un objeto IPAddress
            IPAddress address = IPAddress.Parse("127.0.0.1");


            // Crear un socket para conectarse al servidor
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(address, 1234));

            // Iniciar dos hilos para enviar y recibir mensajes
            Thread sendThread = new Thread(() => SendMessage(clientSocket));
            Thread receiveThread = new Thread(() => ReceiveMessage(clientSocket));
            sendThread.Start();
            receiveThread.Start();
        }

        static void SendMessage(Socket socket)
        {
            while (true)
            {
                // Leer el mensaje del usuario
                string message = Console.ReadLine();

                // Convertir el mensaje a bytes y enviarlo a través del socket
                byte[] data = Encoding.UTF8.GetBytes(message);
                socket.Send(data);
            }
        }

        static void ReceiveMessage(Socket socket)
        {
            while (true)
            {
                // Recivo una peticion del servidor
                byte[] receiveData = new byte[1024];
                int bytesReceived = socket.Receive(receiveData);
                string receiveMessage = Encoding.UTF8.GetString(receiveData, 0, bytesReceived);
                Console.WriteLine(receiveMessage);
            }
        }
    }
}
