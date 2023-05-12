using APIPOSS.Models;
using APIPOSS.Models.Configuracion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;

namespace APIPOSS.Controllers
{
    public class UsuariosApiController : ApiController
    {
        [Route("api/GetConsultaUsuarios")]
        public JsonResult<List<UsuariosConfiguracionView>> GetConsultaUsuarios(string Json)
        {
            UsersView users;
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                users = JsonConvert.DeserializeObject<UsersView>(Json);

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@SearchFullName", Value = null},
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TypeRole", Value = users.TypeRole },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = users.Franquicia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@ID", Value = users.AdminUserID }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminUserListWithFilters", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (dt!=null)
                {
                    var usuarios = (from DataRow rows in dt.Rows
                                    select new UsuariosConfiguracionView
                                    {
                                        AdminUserID = Convert.ToInt32(rows["AdminUserID"]==null?0: rows["AdminUserID"]),
                                        Usuario = (string)rows["FullName"],
                                        Rol = (string)rows["RoleName"],
                                        Estatus = (string)rows["Status"]
                                    }
                                    ).ToList();
                    
                    if (usuarios!=null)
                    {
                        return Json<List<UsuariosConfiguracionView>>(usuarios);
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return null;
        }

        [Route("api/GetListaRoles")]
        public JsonResult<List<RolesView>> GetListaRoles(string json)
        {
            UsersView users;
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                users = JsonConvert.DeserializeObject<UsersView>(json);

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TypeRole", Value = users.TypeRole },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = users.Franquicia },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@ID", Value = 0 }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminRoleActiveList", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {
                    var roles = (from DataRow rows in dt.Rows
                                 select new RolesView
                                 {
                                     AdminRoleID = Convert.ToInt32(rows["AdminRoleID"] == null ? 0 : rows["AdminRoleID"]),
                                     RoleName = (string)rows["RoleName"]
                                 }
                                 ).ToList();

                    if (roles != null)
                    {
                        return Json<List<RolesView>>(roles);
                    }

                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api/GetListaAdminTiendas")]
        public JsonResult<List<AdminStore>> GetListaAdminTiendas(string json)
        {
            UsersView users;
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                users = JsonConvert.DeserializeObject<UsersView>(json);

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = users.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = users.Franquicia }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("StoreByUser", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {
                    var roles = (from DataRow rows in dt.Rows
                                 select new AdminStore
                                 {
                                     AdminStoreID = Convert.ToInt32(rows["AdminStoreID"] == null ? 0 : rows["AdminStoreID"]),
                                     StoreName = (string)rows["StoreName"],
                                     AdminUserID = Convert.ToInt32(rows["AdminUserID"].ToString()==""?0: rows["AdminUserID"]),
                                     AdminStoreToSendID = Convert.ToInt32(rows["AdminStoreToSendID"].ToString() == "" ? 0: rows["AdminStoreToSendID"])
                                 }).ToList();

                    if (roles != null)
                    {
                        return Json<List<AdminStore>>(roles);
                    }

                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [Route("api/AddUsuarios")]
        [HttpPost]
        public JsonResult<UsersAdmin> AddUsuarios(UsersAdmin users)
        {
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;
            string json="";
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FirstName", Value = users.FirstName },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@LastName", Value = users.LastName },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CorreoElectronico", Value = users.CorreoElectronico },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminRoleID", Value = Convert.ToInt32(users.AdminRoleID) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@EmployeeNum", Value = users.EmployeeNum },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NTUserAccount", Value = users.NTUserAccount },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NTUserDomain", Value = users.NTUserDomain },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Status", Value = users.Status },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NoCelular", Value = users.NoCelular }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                int result = obj.EjecutaQry_Tabl("AdminUserInsert", "@AdminUserID", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (result != -1)
                {
                    UsersAdmin admin = new UsersAdmin() { AdminUserID = result };
                    return Json<UsersAdmin>(admin);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/UdapteUser")]
        public JsonResult<UsersAdmin> UdapteUser(UsersAdmin users)
        {
            UsersAdmin result = new UsersAdmin();
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                //users = JsonConvert.DeserializeObject<UsersAdmin>(json);

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = users.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FirstName", Value = users.FirstName },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@LastName", Value = users.LastName },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@CorreoElectronico", Value = users.CorreoElectronico },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminRoleID", Value = Convert.ToInt32(users.AdminRoleID) },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@EmployeeNum", Value = users.EmployeeNum },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NTUserAccount", Value = users.NTUserAccount },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NTUserDomain", Value = users.NTUserDomain },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Status", Value = users.Status },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@NoCelular", Value = users.NoCelular }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminUserUpdate", lsParameters, CommandType.StoredProcedure, connstringWEB);

                result.AdminUserID = users.AdminUserID;
                if (dt != null)
                {
                    StoreByUserUpdate(users.AdminUserID, users.Franquicia);
                    return Json<UsersAdmin>(result);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api/SelectUser")]
        public JsonResult<UsersAdmin> SelectUser(UsersView users)
        {
            UsersAdmin usersAdmin;
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = users.AdminUserID },
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminUserInfo", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {

                    foreach (DataRow rows in dt.Rows)
                    {
                        usersAdmin = new UsersAdmin()
                        {
                            AdminUserID = Convert.ToInt32(rows["AdminUserID"] is DBNull ? 0 : rows["AdminUserID"]),
                            FirstName = rows["FirstName"] is DBNull ? "" : (string)rows["FirstName"],
                            LastName = rows["LastName"] is DBNull ? "" : (string)rows["LastName"],
                            CorreoElectronico = rows["CorreoElectronico"] is DBNull ? "" : (string)rows["CorreoElectronico"],
                            EmployeeNum = rows["EmployeeNum"] is DBNull ? "" : (string)rows["EmployeeNum"],
                            NTUserAccount = rows["NTUserAccount"] is DBNull ? "" : (string)rows["NTUserAccount"],
                            NTUserDomain = rows["NTUserDomain"] is DBNull ? "" : (string)rows["NTUserDomain"],
                            Status = rows["Status"] == null ? "" : (string)rows["Status"],
                            NoCelular = rows["NoCelular"] is DBNull ? "" : (string)rows["NoCelular"],
                            AdminRoleID = rows["AdminRoleID"] is DBNull ? 0 : Convert.ToInt32(rows["AdminRoleID"])
                        };
                        return Json<UsersAdmin>(usersAdmin);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api/StoreDeleteByUSer")]
        [HttpPost]
        public JsonResult<int> DeleteStore(UsersView users)
        {
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = users.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = users.Franquicia }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                int result = obj.EjecutaQry_Tabl("StoreDeleteByUSerN", "", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (result != -1)
                {
                    return Json<int>(result);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api/AddStore")]
        [HttpPost]
        public JsonResult<string> AddStore(List<AdminStore> users)
        {
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;
            string json = "";
            string Asignadas;
            string Traspaso;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                if (users!=null)
                {

                    var ls = (from p in users
                              where (p.Asignadas == true) || (p.Traspaso == true)
                              select p).ToList();

                    int AdminStoreID = 0;
                    int AdminStoreToSendID = 0;
                    foreach (var item in ls)
                    {
                        Asignadas = null;
                        Traspaso = null;

                            List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>();
                            lsParameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@AdminUserID", Value = item.AdminUserID });
                            if (item.Asignadas == true)
                            {
                                Asignadas = item.AdminStoreID.ToString()==""?item.AdminStoreToSendID.ToString(): item.AdminStoreID.ToString();
                                lsParameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@StoreID", Value = Convert.ToInt32(Asignadas) });
                            }
                            else
                            {
                                lsParameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@StoreID", Value = DBNull.Value });
                            }

                            if (item.Traspaso == true)
                            {
                                Traspaso = item.AdminStoreID.ToString();

                                lsParameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@AdminStoreToSendID", Value = Convert.ToInt32(Traspaso) });
                            }
                            else
                            {
                                lsParameters.Add(new System.Data.SqlClient.SqlParameter() { ParameterName = "@AdminStoreToSendID", Value = DBNull.Value });
                            }

                           Utilities.DBMaster obj = new Utilities.DBMaster();
                           int result = obj.EjecutaQry_Tabl("StoreByUserInsert", "", lsParameters, CommandType.StoredProcedure, connstringWEB);
                    }
                      
                 return Json("1");
                }
                
            }
            catch (Exception ex)
            {
                return Json("-1");
            }
            return null;
        }
        [Route("api2/GetStoreSelected")]
        public JsonResult<List<AdminStore>> GetStoreSelected(string json)
        {
            StringBuilder Userquery = new StringBuilder();
            UsersView users;
            DataTable dt;
            string connstringWEB;

            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                users = JsonConvert.DeserializeObject<UsersView>(json);

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@AdminUserID", Value = users.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = users.Franquicia }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("StoreByUser", lsParameters, CommandType.StoredProcedure, connstringWEB);

                if (dt != null)
                {
                    var roles = (from DataRow rows in dt.Rows
                                 select new AdminStore
                                 {
                                     Asignadas = (Convert.ToInt32(users.AdminUserID) == Convert.ToInt32(rows["AdminUserID"] is DBNull ? 0 : rows["AdminUserID"])) ? true : false,
                                     Traspaso = (Convert.ToInt32(users.AdminUserID) == Convert.ToInt32(rows["AdminStoreToSendID"] is DBNull ? 0 : rows["AdminStoreToSendID"])) ? true : false,
                                     AdminStoreID = Convert.ToInt32(rows["AdminStoreID"] is DBNull ? 0 : rows["AdminStoreID"]),
                                     StoreName = (string)rows["StoreName"],
                                     AdminUserID = Convert.ToInt32(rows["AdminUserID"] is DBNull ? 0 : rows["AdminUserID"]),
                                     AdminStoreToSendID = Convert.ToInt32(rows["AdminStoreToSendID"] is DBNull ? 0 : rows["AdminStoreToSendID"])

                                 }).ToList();

                    if (roles != null)
                    {
                        return Json<List<AdminStore>>(roles);
                    }

                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        private void StoreByUserUpdate(int AdminUserID, string Franquicia)
        {
            Utilities.admin Master = new Utilities.admin();
            // Delete all Optios 
            Master.DBConn.ExecuteCMD("StoreDeleteByUSer " + AdminUserID + "," + Franquicia);
        }
    }
}