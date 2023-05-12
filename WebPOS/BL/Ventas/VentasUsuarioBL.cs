using BL.Interface;
using BL.Utilities;
using DAL.Ventas;
using System;
using System.Net.Http;

namespace BL.Ventas
{
    public class VentasUsuarioBL : HttpClientBL, IVentasUsuarioBL
    {
        readonly DAL.Utilities.IResponseAPI _VentasUsuarios;
        public VentasUsuarioBL(VentasUsuarioDAL VentasUsuarioDAL)
        {
            _VentasUsuarios = VentasUsuarioDAL;
        }
        public VentasUsuarioBL()
        {
            _VentasUsuarios = new VentasUsuarioDAL();
        }
        public HttpResponseMessage GetResponseAPIVentasUsuarios(string url)
        {
            try
            {
                return _VentasUsuarios.GetResponseAPI(url);
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
                return _VentasUsuarios.ReadAsStringAsyncAPI(url, obj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public HttpClient HttpClientBL()
        {
            try
            {
                return GetHttpClientBL(); ;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
