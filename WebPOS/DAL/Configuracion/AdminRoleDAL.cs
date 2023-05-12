using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DAL.Configuracion
{
    public class AdminRoleDAL : Utilities.HttpClientDAL, Utilities.IResponseAPI
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
