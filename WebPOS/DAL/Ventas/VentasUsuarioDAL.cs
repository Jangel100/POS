using System;
using System.Net.Http;

namespace DAL.Ventas
{
    public class VentasUsuarioDAL : Utilities.HttpClientDAL, Utilities.IResponseAPI
    {
        public HttpResponseMessage GetResponseAPILogin(string url)
        {
            try
            {
                return base.GetResponseAPI(url);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public HttpResponseMessage GetResponseAcceso(string url)
        {
            try
            {
                return base.GetResponseAPI(url);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
