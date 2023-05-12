using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BL.Interface
{
    public interface IReportesBL
    {
        HttpResponseMessage GetResponseAsycAPI(string url);
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
        HttpClient HttpClientBL();
    }
}
