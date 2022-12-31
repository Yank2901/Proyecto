using System.Collections.Generic;

namespace DomainLayer
{
    public class listaAcciones
    {
        List <Acciones> lstAcciones;

        public listaAcciones()
        {
            lstAcciones = new List<Acciones>();
        }

        public void AddAcciones (Acciones a)
        {
            lstAcciones.Add(a);
        }
    }
}
