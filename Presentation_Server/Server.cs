using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer;

namespace Presentation_Server
{
    internal class Server
    {
        static UserModel user = new UserModel();
        static void Main(string[] args)
        {
            int opc;
            Console.WriteLine("ENTRENANDO A TU PERRO UN DÍA A LA VEZ\n");
            do
            {
                opc = Menu();
                if (opc == 1)
                {
                    Cliente _client;
                    Mascota _peet;
                    Console.WriteLine("Ingresa el nombre del dueño de la mascota: ");
                    string _name=Console.ReadLine();
                    _client = validar_crear_Cliente(_name);
                    Console.WriteLine("Ingresa el nombre de la mascota: ");
                    _name = Console.ReadLine();
                    _peet = validar_crear_Mascota(_client, _name);
                }

                if (opc == 2)
                {
                }

                if (opc == 3)
                {
                }

                if (opc == 4)
                {
                }
            } while (true);
        }

        public static int Menu()
        {
            int opc;
            do
            {
                Console.WriteLine("Que accion quieres realizar:\n" +
                                  "1. Consultar mascota\n" +
                                  "2. Generar nuevo registro de mascota\n" +
                                  "3. Listar informacion de ordenes\n" +
                                  "4. Cerrar conexión\n");
                Console.Write("Ingrese el número de la acción a realizar: ");
                opc = int.Parse(Console.ReadLine());
                if (opc < 1 || opc > 4)
                    Console.WriteLine("Acción Invalida!!!\n");
            } while (opc < 1 || opc > 4);
            return opc;
        }

        public static Cliente validar_crear_Cliente(string _name)
        {
            Cliente _client = new Cliente(_name);
            _client.Id = user.validarUsuario(_name);
            if (_client.Id == -1)
                _client.Id = user.insertarUsuario(_name);
            return _client;
        }

        public static bool validar_Cliente(string _name)
        {
            Cliente _client = new Cliente(_name);
            _client.Id = user.validarUsuario(_name);
            if (_client.Id == -1)
                return false;
            return true;
        }

        public static Mascota validar_crear_Mascota(Cliente _client, string _name)
        {
            Mascota _peet=new Mascota(_name,_client.Id);
            _peet.Id = user.validarMascota(_name);
            if (_peet.Id == -1)
            {
                Console.WriteLine("OK");
                Console.WriteLine("Registrando Mascota ...");
                _peet.Id = user.insertarMascota(_client.Id,_name);
                user.inicilizarAcciones(_peet.Id);
                Console.WriteLine("Se ha registrado exitosamente la mascota...");
            }
            else
            {
                Console.WriteLine("NOK");
            }
            return _peet;
        }

        public static bool validar_Mascota(Cliente _client, string _name)
        {
            Mascota _peet = new Mascota(_name, _client.Id);
            _peet.Id = user.validarMascota(_name);
            if (_peet.Id == -1)
                return false;
            return true;
        }
        
    }
}
