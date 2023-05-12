using System;
using System.Net.Http;

namespace DAL.Configuracion
{
    public class PaybackDAL : Utilities.HttpClientDAL, DAL.Utilities.IResponseAPI
    {
        public HttpResponseMessage GetResponseAPIUsuarios(string url)
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



        public HttpResponseMessage ReadAsStringAsyncAPIUsuarios(string url, object obj)
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
