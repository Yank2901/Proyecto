namespace DomainLayer
{
    public class Cliente
    {
        private int _id;
        private string _nombre;

        public Cliente(string _nombre)
        {
            this._id = -1;
            this._nombre = _nombre;
        }

        public int Id { get { return _id; } }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
    }
}
