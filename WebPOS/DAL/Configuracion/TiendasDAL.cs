using System;
using System.Net.Http;

namespace DAL.Configuracion
{
    public class TiendasDAL : Utilities.HttpClientDAL, Utilities.IResponseAPI
    {
        public HttpResponseMessage GetResponseAPITiendas(string url)
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
        public HttpResponseMessage ReadAsStringAsyncAPITiendas(string url, object obj)
        {
            try
            {
                return base.ReadAsStringAsyncAPI(url, obj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}

