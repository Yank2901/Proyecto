using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation_Server
{
    internal class Server
    {
        static void Main(string[] args)
        {
            int opc;
            do
            {
                opc = Menu();
                if (opc == 1)
                {
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
                {
                    Console.WriteLine("Acción Invalida!!!\n");
                }
            } while (opc < 1 || opc > 4);
            return opc;
        }
    }
}
