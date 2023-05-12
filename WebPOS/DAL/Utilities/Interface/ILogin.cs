using System.Net.Http;

namespace DAL.Utilities.Interface
{
    public interface ILogin
    {
        HttpResponseMessage GetResponseAPI(string ur);
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
    }
}
