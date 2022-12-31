namespace DomainLayer
{
    public class Mascota
    {
        private int _id;
        private string _nombre;
        private int _idCliente;

        public Mascota(string _nombre, int _idCliente)
        {
            this._id = -1;
            this._nombre = _nombre;
            this._idCliente = _idCliente;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public int IdCliente
        {
            get { return _idCliente; }
            set { _idCliente = value; }
        }
    }
}
