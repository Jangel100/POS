using DAL.Utilities.Interface;
using System;
using System.Net.Http;

namespace DAL.Login
{
    public class LoginDAL : Utilities.HttpClientDAL, ILogin, IClientApi
    {
        public LoginDAL()
        {
            _accessToken = "";
        }
        public LoginDAL(string accessToken)
        {
            _accessToken = accessToken;
        }

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
        public HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj)
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
