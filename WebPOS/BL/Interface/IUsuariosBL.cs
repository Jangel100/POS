using System.Net.Http;

namespace BL.Interface
{
    public interface IUsuariosBL
    {
        HttpResponseMessage GetResponseAPIUsuarios(string url);
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
    }
}
