using DataAccess;

namespace DomainLayer
{
    public class UserModel
    {
        UserDao _userDao=new UserDao();

        public int validarUsuario(string _name)
        {
            return _userDao.validarUsuario(_name);
        }

        public int validarMascota(string _name)
        {
            return _userDao.validarMascota(_name);
        }

        public int insertarUsuario(string _name)
        {
            return _userDao.insertarUsuario(_name);
        }

        public int insertarMascota(int _idCliente, string _name)
        {
            return _userDao.insertarMascota(_idCliente, _name);
        }

        public void inicilizarAcciones(int _idMascota)
        {
            _userDao.inicilizarAcciones(_idMascota);
        }

        public bool leerAcciones(string _clientName, string _peetname)
        {
            return _userDao.leerAcciones(_clientName, _peetname);
        }

        public void actualizarRegistro(int _idMascota, int _accion, string _opcion)
        {
            _userDao.actualizarRegistro(_idMascota, _accion, _opcion);
        }
    }
}
