namespace DomainLayer
{
    public class Mascota
    {
        private int _id;
        private string _nombre;
        private int _idCliente;

        public Mascota(string _nombre)
        {
            this._id = -1;
            this._nombre = _nombre;
            this._idCliente = -1;
        }

        public int Id { get { return _id; } }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public int IdCliente { get { return _idCliente; } }
    }
}
