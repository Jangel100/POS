using BL.Interface;
using BL.Login;
using Entities.Models.Franquicias;
using Entities.viewsModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Web.Mvc;
using WebPOS.Models;
using WebPOS.Security;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Text;
using System.Security.Claims;
using System.Configuration;

namespace WebPOS.Controllers
{
    public class LoginController : Controller
    {
        readonly ILoginBL _login;
        public const int SaltByteSize = 24;
        public LoginController(ILoginBL login)
        {
            _login = login;
        }

        public LoginController()
        {
            _login = new LoginBL();

        }

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            SessionPersister.TypeRole = string.Empty;
            return RedirectToAction("Index");
        }
        public ActionResult GetUsuarioSesion()
        {
            try
            {
                string Usuario = string.Empty;
                string Franquicia = string.Empty;

                if (Session["UserName"] != null)
                {
                    Usuario = Session["UserName"].ToString();
                }

                return Json(new { Usuario = Usuario });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private ActionResult getPasswordSaltUsers()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult GetHashSaltUsers(Users users)
        {
            try
            {
                string uri = string.Empty;
                string JsonUser = string.Empty;
                Users usersEncode = _login.GetHtmlEncode(users);

                JsonUser = JsonConvert.SerializeObject(usersEncode);
                uri = "api/GetUsersEncripty?JsonUsers=" + JsonUser + "";

                _login.GetHashSaltUsers(usersEncode.password, uri);

                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public JsonResult Login(Users user)
        {
            //VentasUsuarioController _ventaUsuario = new VentasUsuarioController();
            try
            {
                if (user != null)
                {

                    string url = string.Empty;
                    string JsonUser = string.Empty;
                    string JsonResult = string.Empty;

                    var Users = GetLogin(user);

                    if (Users != null)
                    {
                        ClearSesion();
                        AddSesion(Users);

                        SessionPersister.TypeRole = Users.TypeRole;

                        AccountModel accountModel = new AccountModel();

                        string[] roles = { Users.TypeRole };
                        if (accountModel.find(roles))
                        {
                            JsonResult = JsonConvert.SerializeObject(Users);
                            return Json(JsonResult);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }


                // Se implementa validar password con tabla AdminUserssecurity
                //var UsersEncripty = GetEncripty(user);

                //if (UsersEncripty != null)
                //{
                //    UsersEncripty.PaswoordToCheck = user.password;

                //    if (_login.ValidatePassword(UsersEncripty))
                //    {
                //        //JsonUser = JsonConvert.SerializeObject(user);
                //        //url = $"api/GetLogin?JsonUsers= {JsonUser}";

                //        //HttpResponseMessage response = _login.GetResponseAPILogin(url);

                //        //response.EnsureSuccessStatusCode();
                //        var Users = GetLogin(user);

                //        if (Users != null)
                //        {
                //            ClearSesion();
                //            AddSesion(Users);

                //            SessionPersister.TypeRole = Users.TypeRole;

                //            AccountModel accountModel = new AccountModel();

                //            string[] roles = { Users.TypeRole };
                //            if (accountModel.find(roles))
                //            {
                //                JsonResult = JsonConvert.SerializeObject(Users);
                //                return Json(JsonResult);
                //            }
                //            else
                //            {
                //                return null;
                //            }
                //        }
                //        else
                //        {
                //            return null;
                //        }
                //    }
                //    else
                //    {
                //        return null;
                //    }
                //}


                #region codigo para Encriptar
                // Esta funcion se utiliza para encriptar passwordy se registra en la tabla  AdminUserssecurity

                //user.password = user.password.Replace("#", "|");

                //var lsUsers = _ventaUsuario.GetAdminUserInfoAll();

                //var count = 0;
                //foreach (var row in lsUsers)
                //{
                //    count++;
                //    //byte[] salt = SecurityCrypto.saltAleatorio();
                //    //row.passwordByte = salt;

                //    var cryptoProvider = new RNGCryptoServiceProvider();
                //    byte[] salt = new byte[SaltByteSize];
                //    cryptoProvider.GetBytes(salt);
                //    row.passwordByte = salt;

                //    string PaswoordHassh = SecurityCrypto.Encriptar(row.password, salt);

                //    UserEncripty userEncripty = new UserEncripty()
                //    {
                //        UserName = row.usuario,
                //        Paswoord = row.password,
                //        PaswoordHassh = PaswoordHassh,
                //        Salt = salt
                //    };
                //    var rsult = _ventaUsuario.GetUsersSecurity(userEncripty);
                //}
                #endregion
        }
        catch (Exception ex)
        {

        }

            return null;
       }

        [HttpPost]
        public UserEncripty GetEncripty(Users user)
        {
            try
            {
                if (user != null)
                {
                    string url = string.Empty;
                    string JsonUser = string.Empty; ;

                    JsonUser = JsonConvert.SerializeObject(user);
                    url = $"api/GetEncripty?JsonUsers={JsonUser}";

                    HttpResponseMessage response = _login.GetEncripty(url);

                    response.EnsureSuccessStatusCode();
                    var Users = response.Content.ReadAsAsync<UserEncripty>().Result;

                    return Users;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void ClearSesion()
        {
            // Clean Session Object
            Session["AdminUserID"] = null;
            Session["UserName"] = null;
            Session["NTUserAccount"] = null;
            Session["IDSTORE"] = null;
            Session["Franquicia"] = null;
            Session["WHSID"] = null;
            Session["STORENAME"] = null;
            Session["TypeRole"] = null;
        }
        private void AddSesion(UsersView users)
        {
            System.Web.HttpContext.Current.Session["AdminUserID"] = users.AdminUserID;
            System.Web.HttpContext.Current.Session["UserName"] = users.UserName;
            System.Web.HttpContext.Current.Session["NTUserAccount"] = users.NTUserAccount;
            System.Web.HttpContext.Current.Session["IDSTORE"] = users.IDSTORE;
            System.Web.HttpContext.Current.Session["Franquicia"] = users.Franquicia;
            System.Web.HttpContext.Current.Session["WHSID"] = users.WHSID;
            System.Web.HttpContext.Current.Session["STORENAME"] = users.STORENAME;
            System.Web.HttpContext.Current.Session["CorreoUsuario"] = users.CorreoUsuario;
            System.Web.HttpContext.Current.Session["TypeRole"] = users.TypeRole;
        }

        public UsersView GetLogin(Users users)
        {
            UsersView usersviews = null;
            SqlDataReader dr;
            StringBuilder UserQuery = new StringBuilder();

            try
            {
                //users.password = Encoding.UTF8.GetString(Convert.FromBase64String(users.passwordByte));
                ;
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = users.usuario.ToUpper() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Password", Value = users.password }
                       // new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Status", Value ='A' }
                    };

                UserQuery.Append("SELECT TOP 1 * FROM AdminUser AU JOIN AdminRole AR ON AR.AdminRoleID = AU.AdminRoleID ");
                UserQuery.Append("WHERE NTUserAccount = @AdminUserID ");
                //UserQuery.Append("AND AU.Status = @Status ");
                UserQuery.Append("AND NTUserDomain = @Password ");

                //UserQuery.Append("AND AR.AdminRoleID = @AdminRoleID");

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(UserQuery.ToString(), con))
                    {
                        com.CommandType = System.Data.CommandType.Text;
                        com.Parameters.AddRange(lsParameters.ToArray());
                        dr = com.ExecuteReader();

                        if (dr != null)
                        {
                            while (dr.Read())
                            {
                                usersviews = new UsersView()
                                {
                                    AdminUserID = dr["AdminUserID"].ToString() == null ? "" : dr["AdminUserID"].ToString(),
                                    UserName = dr["FirstName"].ToString() == null ? "" : dr["FirstName"].ToString() + " " + dr["LastName"].ToString() == null ? "" : dr["LastName"].ToString(),
                                    Franquicia = dr["Franquicia"].ToString() == null ? "" : dr["Franquicia"].ToString(),
                                    NTUserAccount = dr["NTUserAccount"].ToString() == null ? "" : dr["NTUserAccount"].ToString(),
                                    TypeRole = dr["TypeRole"].ToString() == null ? "" : dr["TypeRole"].ToString(),
                                    CorreoUsuario = dr["CorreoElectronico"].ToString() == null ? "" : dr["CorreoElectronico"].ToString(),
                                    Status = dr["Status"].ToString() == null ? "" : dr["Status"].ToString()
                                };

                                usersviews.WebTokenJWT = usersviews.UserName;

                            }

                            return usersviews;
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}