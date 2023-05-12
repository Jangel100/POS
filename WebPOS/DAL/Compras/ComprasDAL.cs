using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
namespace DAL.Compras
{
    public class ComprasDAL : Utilities.HttpClientDAL, DAL.Utilities.IResponseAPI
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
