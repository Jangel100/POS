using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DAL.Reportes
{
    public class ReportesDAL : Utilities.HttpClientDAL, Utilities.IResponseAPI
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
