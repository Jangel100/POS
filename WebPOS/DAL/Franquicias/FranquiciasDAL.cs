using System;
using System.Collections.Generic;
using System.Text;
using DAL.Utilities;
using System.Net.Http;
namespace DAL.Franquicias
{
    public class FranquiciasDAL :HttpClientDAL, IResponseAPI
    {
        public HttpResponseMessage GetResponseAPIFranquicias(string url)
        {
            try
            {
                return base.GetResponseAPI(url);
            }
            catch (Exception ex) { throw; }

        }
        public HttpResponseMessage ReadAsStringAsyncAPIFranquicias(string url, object obj)
        {
            try
            {
                return base.ReadAsStringAsyncAPI(url, obj);
            }
            catch (Exception ex) { throw; }

        }
    }
}
