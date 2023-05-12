using System.Net.Http;

namespace BL.Interface
{
    public interface IVentasUsuarioBL
    {
        HttpResponseMessage GetResponseAPIVentasUsuarios(string url);
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
        HttpClient HttpClientBL();
    }
}
