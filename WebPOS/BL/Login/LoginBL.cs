using BL.Interface;
using DAL.Login;
using DAL.Utilities.Interface;
using Entities.Models.Franquicias;
using Entities.viewsModels;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace BL.Login
{
    public class LoginBL : ILoginBL
    {
        readonly ILogin _Login;

        public LoginBL(LoginDAL login)
        {
            _Login = login;
        }
        public LoginBL()
        {
            _Login = new LoginDAL();
        }

        public LoginBL(string acessToken)
        {
            _Login = new LoginDAL(acessToken);
        }

        public HttpResponseMessage GetResponseAPILogin(string url)
        {
            try
            {
                return _Login.GetResponseAPI(url);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Users GetHtmlEncode(Users users)
        {
            try
            {
                var usuario = HttpUtility.HtmlEncode(users.usuario);
                var password = HttpUtility.HtmlEncode(users.password);

                usuario = Regex.Replace(usuario, "[^a-z0-9]", "", RegexOptions.IgnoreCase);
                usuario = usuario.Replace("/", "");
                usuario = usuario.Replace("<", "");

                password = Regex.Replace(password, "[^a-z0-9-#]", "", RegexOptions.IgnoreCase);
                password = password.Replace("/", "");
                password = password.Replace("<", "");

                users.usuario = usuario.ToString();
                users.password = password.ToString();

                return users;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public HttpResponseMessage GetEncripty(string url)
        {
            try
            {
                return _Login.GetResponseAPI(url);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public HttpResponseMessage GetHashSaltUsers(string password, string url)
        {
            try
            {

                UserEncripty result = Security.SecurityCrypto.Encriptar(password);

                var JsonUser = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                string uri = "api/GetUsersSecurity?JsonUsers=" + JsonUser + "";

                HttpResponseMessage response = _Login.GetResponseAPI(uri);

                response.EnsureSuccessStatusCode();
                var Users = response.Content.ReadAsAsync<string>().Result;

                return _Login.GetResponseAPI(url);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public Boolean ValidatePassword(UserEncripty userEncripty)
        {
            try
            {

                var user = Security.SecurityCrypto.HashPasswordVerificar(userEncripty.PaswoordToCheck, userEncripty.Salt);

                if (user != null)
                {
                    if (userEncripty.PaswoordHassh == user.PaswoordHassh)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public byte[] StringToByteArray(string password)
        {
            //Int32 caracteres = password.Length;
            //byte[] array_datos = new byte[caracteres / 2];
            //for (int i = 0; i < caracteres; i += 2)
            //    array_datos[i / 2] = Convert.ToByte(password.Substring(i, 2), 16);

            return System.Text.Encoding.UTF8.GetBytes(password);
        }

        public HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj)
        {
            try
            {
                return _Login.ReadAsStringAsyncAPI(url, obj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
