using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
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
                    verificarInscripcion();
                }

                if (opc == 2)
                {
                    actualizarRegistro();
                }

                if (opc == 3)
                {
                    imprimirAcciones();
                }

                if (opc == 4)
                {
                    Console.WriteLine("Conexion Terminada ...");
                    break;
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

        public static void verificarInscripcion()
        {
            Cliente _client;
            Mascota _peet;
            Console.WriteLine("Ingresa el nombre del dueño de la mascota: ");
            _client = new Cliente(Console.ReadLine());

            // Verificamos que el dueño exista caso contrario creamos el usuario
            _client.Id = user.validarUsuario(_client.Nombre);
            if (_client.Id == -1)
            {
                _client.Id=user.insertarUsuario(_client.Nombre);
            }

            Console.WriteLine("Ingresa el nombre de la mascota: ");
            _peet = new Mascota(Console.ReadLine(), _client.Id);
            // Verificamos que la mascota exista
            _peet.Id = user.validarMascota(_peet.Nombre);
            if (_peet.Id == -1)
            {
                Console.WriteLine("OK");
               //Registramos la nueva mascota
                _peet.Id = user.insertarMascota(_client.Id, _peet.Nombre);
                // Generamos una lista de acciones de la mascota y las iniciamos en 0 tanto OK y NOK
                user.inicilizarAcciones(_peet.Id);
            }
            else
            {
                Console.WriteLine("NOK");
            }
        }

        public static void actualizarRegistro()
        {
            string[] acciones = { "1. Siéntate", "2. Échate", "3. Giro 180", "4. Quédate", "5. Aquí", "6. Muertito", "7. Lugar", "8. Junto" };
            Cliente _client;
            Mascota _peet;

            Console.WriteLine("Ingrese la orden para generar un nuevo registro para una mascota: ");
            string[] _reg = Console.ReadLine().Split(',');

            // Validamos que la sentencia ingresada por consola tenga 4 atributos separados por ,
            if (_reg.Length != 4)
            {
                Console.WriteLine("Sentencia invalida");
                return;
            }

            // Verificamos que el dueño exista
            _client = new Cliente(_reg[0]);
            _client.Id = user.validarUsuario(_reg[0]);
            if (_client.Id == -1)
            {
                Console.WriteLine("Dueño no encontrado\nPuede añadir el nuevo dueño y su mascota en la opcion 1");
                return;
            }

            // Verificamos que la mascota exista
            _peet = new Mascota(_reg[1], _client.Id);
            _peet.Id = user.validarMascota(_peet.Nombre);
            if (_peet.Id == -1)
            {
                Console.WriteLine("Mascota no encontrado\nPuede añadir su mascota en la opcion 1");
                return;
            }

            //Verificamos que la opcion seleccionada este en el listado
            if (int.Parse(_reg[2]) < 1 || int.Parse(_reg[2]) > acciones.Length)
            {
                Console.WriteLine("Orden invalida, recuerda que las acciones disponibles son:");
                foreach (var a in acciones)
                {
                    Console.WriteLine(a);
                }
                return;
            }

            // Verificamos que la orden tenga el valor OK o NOK
            if (!string.Equals(_reg[3], "OK") && !string.Equals(_reg[3], "NOK"))
            { 
                Console.WriteLine("Resultado de la orden invalido\nRecuerda que el resultado de la orden solo puede ser:"); 
                Console.WriteLine("OK: en caso de cumplirse la orden\nNOK: en caso de no cumplirse la orden"); 
                return;
            }

            //Verificado todos los campos procedemos a cambiar el registro
            if (_reg[3] == "OK")
                user.actualizarRegistroOK(_peet.Id, int.Parse(_reg[2]));
            else
                user.actualizarRegistroNOK(_peet.Id, int.Parse(_reg[2]));

        }

        public static void imprimirAcciones()
        {
            List<string> lstAcciones;
            Cliente _client;
            Mascota _peet;

            Console.WriteLine("Ingrese la orden para listar los registros de la mascota: ");
            string[] _reg = Console.ReadLine().Split(':');

            // Validamos que la sentencia ingresada por consola tenga 2 atributos separados por :
            if (_reg.Length != 2)
            {
                Console.WriteLine("Sentencia invalida");
                return;
            }

            // Verificamos que el dueño exista
            _client = new Cliente(_reg[0]);
            _client.Id = user.validarUsuario(_reg[0]);
            if (_client.Id== -1)
            {
                Console.WriteLine("Dueño no encontrado\nPuede añadir el nuevo dueño y su mascota en la opcion 1");
                return;
            }

            // Verificamos que la mascota exista
            _peet = new Mascota(_reg[1], _client.Id);
            _peet.Id = user.validarMascota(_peet.Nombre);
            if (_peet.Id == -1)
            {
                Console.WriteLine("Mascota no encontrado\nPuede añadir su mascota en la opcion 1");
                return;
            }
            
            //Una vez verificado los parametros pasamos a realizar la peticion s la base de datos
            lstAcciones = user.leerAcciones(_client.Nombre, _peet.Nombre);
            foreach (string a in lstAcciones)
            {
                Console.WriteLine(a);
            }
        }
    }
}
