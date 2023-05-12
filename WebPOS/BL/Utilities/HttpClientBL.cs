using DAL.Utilities;
using System;
using System.Net.Http;

namespace BL.Utilities
{
    public class HttpClientBL : HttpClientDAL
    {
        private HttpClientDAL _httpclient = new HttpClientDAL();
        public HttpClient GetHttpClientBL()
        {
            try
            {
                return _httpclient.Client;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
