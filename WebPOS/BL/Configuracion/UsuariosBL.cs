using BL.Interface;
using DAL.Configuracion;
using System;
using System.Net.Http;

namespace BL.Configuracion
{
    public class UsuariosBL : IUsuariosBL
    {
        readonly DAL.Utilities.IResponseAPI _usuarios;
        public UsuariosBL(UsuarioDAL usuariosDAL)
        {
            _usuarios = usuariosDAL;
        }

        public UsuariosBL()
        {
            _usuarios = new UsuarioDAL();
        }

        public HttpResponseMessage GetResponseAPIUsuarios(string url)
        {
            try
            {
                return _usuarios.GetResponseAPI(url);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj)
        {
            try
            {
                return _usuarios.ReadAsStringAsyncAPI(url, obj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
