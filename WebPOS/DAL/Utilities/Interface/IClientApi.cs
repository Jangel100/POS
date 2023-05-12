using System.Net.Http;

namespace DAL.Utilities.Interface
{
    interface IClientApi
    {
        string _accessToken { get; set; }
        HttpClient Client { get; set; }
        HttpResponseMessage GetResponseAPI(string url);
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
    }
}
