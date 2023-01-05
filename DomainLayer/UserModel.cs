using System.Collections.Generic;
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

        public List<string> leerAcciones(string _clientName, string _peetname)
        {
            return _userDao.leerAcciones(_clientName, _peetname);
        }

        public void actualizarRegistroOK(int _idMascota, int _accion)
        {
            _userDao.actualizarRegistroOK(_idMascota, _accion);
        }

        public void actualizarRegistroNOK(int _idMascota, int _accion)
        {
            _userDao.actualizarRegistroNOK(_idMascota, _accion);
        }

        public string buscarDueño(string _name)
        {
            return _userDao.buscarDueño(_name);
        }
    }
}
