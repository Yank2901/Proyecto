using System;

namespace DomainLayer
{
    public class Ordenes
    {
        private int _id;
        private int _idMascota;
        private Acciones _accion;
        private int _ok;
        private int _nok;

        public Ordenes(int _accion, int _ok, int _nok)
        {
            this._id = -1;
            this._idMascota = -1;
            this._accion = (Acciones)_accion;
            this._ok = _ok;
            this._nok = _nok;
        }

        public Ordenes(string _accion, int _ok, int _nok)
        {
            this._id = -1;
            this._idMascota = -1;
            this._accion = (Acciones)Enum.Parse(typeof(Acciones), _accion);
            this._ok = _ok;
            this._nok = _nok;
        }

        public int Id { get { return _id; } }
        public int IdMascota { get { return _idMascota; } }
        public  Acciones Accion { get { return _accion; } }
        public int Ok { get { return _ok; } }
        public int Nok { get { return _nok; } }
    }
}
