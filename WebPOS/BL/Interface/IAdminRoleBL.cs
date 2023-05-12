using System.Net.Http;

namespace BL.Interface
{
    public interface IAdminRoleBL
    {
        HttpResponseMessage GetResponseAPIRoles(string url);
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
    }
}
