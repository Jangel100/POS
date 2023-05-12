using Entities.Models.Franquicias;
using Entities.viewsModels;
using System;
using System.Net.Http;

namespace BL.Interface
{
    public interface ILoginBL
    {
        Users GetHtmlEncode(Users users);
        Boolean ValidatePassword(UserEncripty userEncripty);
        byte[] StringToByteArray(string password);
        HttpResponseMessage GetResponseAPILogin(string url);
        HttpResponseMessage GetHashSaltUsers(string password, string uri);
        HttpResponseMessage GetEncripty(string url);
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
    }
}
