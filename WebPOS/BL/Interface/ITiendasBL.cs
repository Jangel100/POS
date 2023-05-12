using System.Net.Http;

namespace BL.Interface
{
    public interface ITiendasBL
    {
        HttpResponseMessage GetResponseAPITiendas(string url);
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
        string UpdateMessage(string Parametros);
    }
}
