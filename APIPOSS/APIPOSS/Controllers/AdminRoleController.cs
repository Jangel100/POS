using APIPOSS.Models;
using APIPOSS.Models.Configuracion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;

namespace APIPOSS.Controllers
{
    public class AdminRoleController : ApiController
    {
        [Route("api2/GetInfoRole")]
        public List<RolesView> GetInfoRole(string json)
        {
            UsersView users;
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                users = JsonConvert.DeserializeObject<UsersView>(json);
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TypeRole", Value = users.TypeRole },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = users.Franquicia },
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("AdminRoleList", lsParameters, CommandType.StoredProcedure, "AdminRoleList", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new RolesView
                              {
                                  AdminRoleID = rows["AdminRoleID"] is DBNull ? 0 : Convert.ToInt32(rows["AdminRoleID"]),
                                  RoleName = rows["RoleName"] is DBNull ? "" : (string)rows["RoleName"],
                                  Status = rows["Status"] is DBNull ? "" : (string)rows["Status"],
                                  CanBeDeleted = rows["CanBeDeleted"] is DBNull ? false : (bool)rows["CanBeDeleted"],
                                  Btn_Front = $"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a id='myLink' href='#' onclick='Roles.Modify({rows["AdminRoleID"].ToString()},{'"'+rows["RoleName"].ToString() +'"'});'>Editar</a> &nbsp;&nbsp;&nbsp;&nbsp;" + (rows["CanBeDeleted"] is DBNull ? "" : rows["CanBeDeleted"].Equals(true) ? $"<a id='myLink2' href='#' onclick='Roles.Delete({rows["AdminRoleID"].ToString()});'>Eliminar</a>" : "")
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetMenuInfoRole")]
        public MenuOptionInfoRol GetMenuInfoRole(string json)
        {
            UsersView users;
            DataTable dt;
            string connstringWEB;
            MenuOptionInfoRol menuOp = new MenuOptionInfoRol();
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                users = JsonConvert.DeserializeObject<UsersView>(json);
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@ID", Value = users.AdminUserID },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = users.Franquicia },
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("MenuOptionByRole", lsParameters, CommandType.StoredProcedure, "AdminRoleList", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new MenuInfoRol
                              {
                                  MenuTabID = rows["MenuTabID"] is DBNull ? 0 : Convert.ToInt32(rows["MenuTabID"]),
                                  MenuTabName = rows["MenuTabName"] is DBNull ? "" : (string)rows["MenuTabName"],
                                  MenuOptionID = rows["MenuOptionID"] is DBNull ? 0 : Convert.ToInt32(rows["MenuOptionID"]),
                                  OptionName = rows["OptionName"] is DBNull ? "" : (string)rows["OptionName"],
                                  AdminRoleID = rows["AdminRoleID"] is DBNull ? 0 : Convert.ToInt32(rows["AdminRoleID"]),
                                  Permiso = rows["Permiso"] is DBNull ? 0 : Convert.ToInt32(rows["Permiso"])
                              }).ToList();
                    menuOp.MenuInfoRol = ls;
                    var jsonStr = JsonConvert.SerializeObject(users);
                    menuOp.ListFranquiciasUser = GetListFranquiciasUser(users.TypeRole, users.Franquicia);
                    menuOp.AdminRoleClas = GetAdminRoleClas(users.TypeRole);
                    menuOp.StatusText = ActiveRole(users.AdminUserID);
                    menuOp.FRCARDCODE = menuOp.RoleName;
                    var info = ExistRol(users.UserName);
                    menuOp.RoleName = users.UserName;
                    menuOp.FRCARDCODE = info.Carcode;
                    return menuOp;
                }
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [Route("api2/GetNewMenuOptionUpdate")]
        public MessageView GetNewMenuOptionUpdate(string json)
        {
            RequestModMenOp MenuOptions;
            MessageView Mesage = new MessageView();
            Utilities.admin Master = new Utilities.admin();
            try
            {
                MenuOptions = JsonConvert.DeserializeObject<RequestModMenOp>(json);
                //Revisa si existe el Rol
                var exist = ExistRol(MenuOptions.NombreRol.Trim());
                if (Convert.ToInt32(MenuOptions.AdminRoleID) <= 0 || Convert.ToInt32(MenuOptions.AdminRoleID) >= 0 && MenuOptions.ModifiedName)
                {
                    if (exist.Exist)
                    {
                        throw new Exception("El nombre del Rol ya existe,por favor defina uno diferente.");
                    }
                }
                // 
                if (Convert.ToInt32(MenuOptions.AdminRoleID) > 0)
                {
                    //Actualiza Rol
                    Master.DBConn.ExecuteCMD("AdminRoleUpdate " + MenuOptions.AdminRoleID + ",'" + MenuOptions.NombreRol + "','" + MenuOptions.StatusActive + "','" + MenuOptions.FranquiciasUser + "','" + MenuOptions.TypeRole + "'");
                }
                else
                {
                    //Inserta Rol
                    Master.DBConn.ExecuteCMD("AdminRoleInsert '" + MenuOptions.NombreRol + "','" + MenuOptions.StatusActive + "'," + MenuOptions.AdminRoleID + ",'" + MenuOptions.TypeRole + "','" + MenuOptions.FranquiciasUser + "'");
                    var getID = ExistRol(MenuOptions.NombreRol.Trim());
                    if (getID.Exist) { MenuOptions.AdminRoleID = getID.ID; }

                }
                //Delete all Optios for Role
                Master.DBConn.ExecuteCMD("AdminRoleMenuOptionDeleteByRole " + MenuOptions.AdminRoleID);

                //Inserta todos los IDs de Menus activos para ese Rol
                foreach (var menu in MenuOptions.ArrayIdMenuOp)
                {
                    Master.DBConn.ExecuteCMD("AdminRoleMenuOptionInsert " + MenuOptions.AdminRoleID + ", " + menu.Id);
                }
                Mesage.Success = true;
                return Mesage;
            }
            catch (Exception ex)
            {
                Mesage.Success = false;
                Mesage.Message = ex.Message;
                return Mesage;
            }
        }
        public List<ListFranquiciasUser> GetListFranquiciasUser(string TypeRole, string Franquicia)
        {
            DataTable dt;
            string connstringWEB;
            var ls = new List<ListFranquiciasUser>();
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TypeRole", Value = TypeRole },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Franquicia", Value = Franquicia },
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ListFranquiciasUser", lsParameters, CommandType.StoredProcedure, "ListFranquiciasUser", connstringWEB);

                ls = (from DataRow rows in dt.Rows
                      select new ListFranquiciasUser
                      {
                          CardCode = rows["CardCode"] is DBNull ? "" : (string)rows["CardCode"],
                          CardName = rows["CardName"] is DBNull ? "" : (string)rows["CardName"],
                      }).ToList();
                return ls;
            }
            catch (Exception ex)
            {

                return ls;
            }
        }
        public AdminRoleClas GetAdminRoleClas(string TypeRole)
        {
            DataTable dt;
            string connstringWEB;
            var ls = new AdminRoleClas();
            try
            {

                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TypeRole", Value = TypeRole }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();

                dt = obj.EjecutaQry_Tabla("AdminRoleClas", lsParameters, CommandType.StoredProcedure, "AdminRoleClas", connstringWEB);

                foreach (DataRow item in dt.Rows)
                {
                    ls.TypeRole = item["TypeRole"] is DBNull ? "" : (string)item["TypeRole"];
                    ls.NombreAbreviatura = item["NombreAbreviatura"] is DBNull ? "" : (string)item["NombreAbreviatura"];
                }
                return ls;
            }
            catch (Exception ex)
            {

                return ls;
            }
        }
        public StatusText ActiveRole(string AdminRoleID)
        {
            string sQuery = string.Empty;
            string result = string.Empty;
            DataSet daActiveRol = null;
            StatusText Active = new StatusText();
            Utilities.admin Master = new Utilities.admin();
            try
            {
                sQuery = "SELECT AR.AdminRoleID, AR.RoleName, dbo.StatusToText(1, AR.Status) AS Status, dbo.AdminRoleCanBeDeleted(AR.AdminRoleID) AS CanBeDeleted FROM AdminRole AR where AR.AdminRoleID = " + AdminRoleID + " ORDER BY AR.RoleName";
                daActiveRol = Master.DBConn.GetQuerydts(sQuery);
                if (daActiveRol != null)
                {
                    foreach (DataRow item in daActiveRol.Tables[0].Rows)
                    {
                        result = item["Status"] is DBNull ? "" : item["Status"].ToString();
                    }
                    if (result.Equals("Activo"))
                    {
                        Active.StatusActive = true;
                    }
                    else if (result.Equals("Inactivo"))
                    {
                        Active.StatusInactive = true;
                    }
                }
                return Active;
            }
            catch (Exception ex)
            {

                return Active;
            }
        }
        public ExisteRol ExistRol(string name)
        {
            string sQuery = string.Empty;
            string result = string.Empty;
            DataSet daExisteRol = null;
            ExisteRol Exist = new ExisteRol();
            Utilities.admin Master = new Utilities.admin();
            try
            {
                sQuery = "SELECT AR.Franquicia,AR.AdminRoleID, AR.RoleName, dbo.StatusToText(1, AR.Status) AS Status, dbo.AdminRoleCanBeDeleted(AR.AdminRoleID) AS CanBeDeleted FROM AdminRole AR where UPPER(RoleName) = UPPER ('" + name + "') ORDER BY AR.RoleName";
                daExisteRol = Master.DBConn.GetQuerydts(sQuery);
                if (daExisteRol.Tables.Count > 0)
                {
                    foreach (DataRow item in daExisteRol.Tables[0].Rows)
                    {
                        result = item["AdminRoleID"] is DBNull ? "" : item["AdminRoleID"].ToString();
                        if (!string.IsNullOrEmpty(result))
                        {
                            Exist.Exist = true;
                            Exist.Carcode = item["Franquicia"] is DBNull ? "" : item["Franquicia"].ToString();
                            Exist.ID = result;
                            return Exist;
                        }
                    }
                }
                return Exist;
            }
            catch (Exception ex)
            {

                return Exist;
            }
        }
        [Route("api2/GetDeleteRol")]
        public MessageView GetDeleteRol(int AdminRoleID)
        {
            var message = new MessageView();
            Utilities.admin Master = new Utilities.admin();
            try
            {
                Master.DBConn.ExecuteCMD("AdminRoleDelete " + AdminRoleID);
                Master.DBConn.ExecuteCMD("AdminRoleMenuOptionDeleteByRole " + AdminRoleID);
                message.Success = true;
                return message;
            }
            catch (Exception ex)
            {
                message.Success = false;
                return message;
            }
        }
    }
}