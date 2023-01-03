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
