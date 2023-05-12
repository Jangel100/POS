using Entities.Models.Franquicias;
using System.Net.Http;

namespace DAL.Utilities.Interface
{
    public interface ILoginClient
    {
        HttpClient Client { get; set; }
        HttpResponseMessage GetResponseAcceso(string url, Users users);
    }
}
