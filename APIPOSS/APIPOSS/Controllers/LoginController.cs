using APIPOSS.Models;
using APIPOSS.Models.Login;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;

namespace APIPOSS.Controllers
{
    [AllowAnonymous]
    public class LoginController : ApiController
    {
        // GET api/values
        public void Get()
        {

        }

        // GET api/GetLogin?JsonUsers
        [Route("api/GetLogin")]
        public JsonResult<UsersView> GetLogin(string JsonUsers)
        {
            UsersView usersviews = null;
            SqlDataReader dr;
            StringBuilder UserQuery = new StringBuilder();
            ClaimsPrincipal principal;

            try
            {
                Users users = JsonConvert.DeserializeObject<Users>(JsonUsers);
                //users.password = Encoding.UTF8.GetString(Convert.FromBase64String(users.passwordByte));
                ;
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = users.usuario.ToUpper() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Password", Value = users.password.Replace("|","#") },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Status", Value = 'A' }
                    };

                UserQuery.Append("SELECT TOP 1 * FROM AdminUser AU JOIN AdminRole AR ON AR.AdminRoleID = AU.AdminRoleID ");
                UserQuery.Append("WHERE NTUserAccount = @AdminUserID ");
                UserQuery.Append("AND AU.Status = @Status ");
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
                                    CorreoUsuario = dr["CorreoElectronico"].ToString() == null ? "" : dr["CorreoElectronico"].ToString()

                                };

                                usersviews.WebTokenJWT = usersviews.UserName;

                            }

                            return Json<UsersView>(usersviews);
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
        [Route("api2/GetUsersSecurity")]
        public JsonResult<string> GetUsersSecurity(UserEncripty users)
        {
            SqlDataReader dr;
            StringBuilder UserQuery = new StringBuilder();

            try
            {

                //var result = formattedPasswordEncripty(users.Salt);

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@UserName", Value = users.UserName.ToUpper() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Paswoord", Value = users.Paswoord.ToUpper() },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@PaswoordHassh", Value = users.PaswoordHassh },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Salt",  Value = users.Salt}
                    };


                UserQuery.Append("InsertUserSecurity");
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(UserQuery.ToString(), con))
                    {
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.Parameters.AddRange(lsParameters.ToArray());
                        int i = com.ExecuteNonQuery();

                        if (i > 0)
                        {
                            return Json<string>("true");
                        }

                        return null;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string formattedPasswordEncripty(byte[] salt)
        {
            try
            {
                string result = string.Empty;
                foreach (byte b in salt)
                {
                    result += String.Format("{0:00-}", b);
                }

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //private byte[] FormattedSalToArrayByte(string SaltdEncripty)
        //{
        //    try
        //    {
        //        /*ar Salt = SaltdEncripty.Split('-');*/

        //        arrayPassword = Convert.FromBase64String(SaltdEncripty);

        //        //arrayPassword = new byte[26];

        //        //foreach (var i in Salt)
        //        //{
        //        //    if (i!="")
        //        //    {
        //        //        var result = Convert.ToByte(i);
        //        //        arrayPassword.Append(result);
        //        //    }
        //        //}

        //        return arrayPassword;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        [Route("api/GetEncripty")]
        public JsonResult<UserEncripty> GetEncripty(string JsonUsers)
        {
            try
            {
                Users usersJson = JsonConvert.DeserializeObject<Users>(JsonUsers);

                var users = GetPasword(usersJson);

                return Json<UserEncripty>(users);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public UserEncripty GetPasword(Users users)
        {
            UserEncripty usersviews = null;
            SqlDataReader dr;
            StringBuilder UserQuery = new StringBuilder();

            try
            {
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@UserName", Value = users.usuario.ToUpper() }
                    };

                UserQuery.Append("SELECT Top 1 [UserName], [Paswoord], [PaswoordHassh], [Salt] FROM AdminUserSecurity ");
                UserQuery.Append("WHERE UserName = @UserName ");

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

                                usersviews = new UserEncripty()
                                {
                                    UserName = dr["UserName"].ToString(),
                                    Paswoord = dr["Paswoord"].ToString(),
                                    PaswoordHassh = dr["PaswoordHassh"].ToString(),
                                    Salt = (byte[])dr["Salt"]
                                };
                        }

                        return usersviews;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string ByteArrayToString(byte[] password)
        {
            StringBuilder cadena = new StringBuilder(password.Length * 2);
            foreach (byte b in password)
            {
                cadena.AppendFormat("{0:x2}", b);
            }
            return cadena.ToString();
        }
       
    }
}
