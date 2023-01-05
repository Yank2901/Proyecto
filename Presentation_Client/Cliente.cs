using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Presentation_Client
{
    public class Cliente
    {
        static void Main(string[] args)
        {
            string serverAddress;
            IPAddress address;
            // Pedir al usuario que ingrese la dirección IP del servidor al que desea conectarse
            do
            {
                Console.Write("Ingresa la dirección IP del servidor: ");
                 serverAddress = Console.ReadLine();
            } while (!IPAddress.TryParse(serverAddress, out address));

            // Convertir la dirección IP ingresada por el usuario a un objeto IPAddress
            
            //serverAddress = GetServerIp();
            address = IPAddress.Parse(serverAddress);
            try
            {
                // Crear un socket para conectarse al servidor
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(new IPEndPoint(address, 5000));

                // Iniciar dos hilos para enviar y recibir mensajes
                Thread sendThread = new Thread(() => SendMessage(clientSocket));
                Thread receiveThread = new Thread(() => ReceiveMessage(clientSocket));
                sendThread.Start();
                receiveThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("No se pudo conectar al servidor");
            }
            Console.ReadLine();
        }

        static void SendMessage(Socket socket)
        {
            while (true)
            {
                try
                {
                    // Leer el mensaje del usuario
                    string message = Console.ReadLine();

                    // Convertir el mensaje a bytes y enviarlo a través del socket
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    socket.Send(data);
                }
                catch
                {
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
        }

        static void ReceiveMessage(Socket socket)
        {
            while (true)
            {
                try
                {
                    // Recivo una peticion del servidor
                    byte[] receiveData = new byte[1024];
                    int bytesReceived = socket.Receive(receiveData);
                    string receiveMessage = Encoding.UTF8.GetString(receiveData, 0, bytesReceived);
                    Console.WriteLine(receiveMessage);
                    if (receiveMessage == "Gracias por usar nuestro servicio")
                    {
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
                catch 
                {
                    Console.WriteLine("Ha sido desconectado del servidor");
                    break;
                }
            }
        }
    }
}
