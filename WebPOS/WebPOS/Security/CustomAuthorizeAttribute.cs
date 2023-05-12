using Entities.viewsModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Mvc;
using System.Web.Routing;
using WebPOS.Models;
using WebPOS.Utilities;
using System.Linq;
using System.Web;

namespace WebPOS.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            #region Roles MenuOption
            var Controller = filterContext.Controller as Controller;
            var UserId = HttpContext.Current.Session["AdminUserID"];
            var ListMenuOption = new List<ListMenuOption>();
            Controller.ViewBag.Ve_Abo_Con = ListMenuOption;
            //Controller.ViewBag.Ventas = ListMenuOption; //Declara variable
            //Controller.ViewBag.Abonos = ListMenuOption;
            //Controller.ViewBag.Consultas = ListMenuOption;
            Controller.ViewBag.Compras = ListMenuOption;
            Controller.ViewBag.Reportes = ListMenuOption;
            Controller.ViewBag.Modificaciones = ListMenuOption;
            Controller.ViewBag.Configuracion = ListMenuOption;
            Controller.ViewBag.Ve_Abo_Con_Franquicias = ListMenuOption;
            Controller.ViewBag.Garantias = ListMenuOption;
            Controller.ViewBag.TypeRole = HttpContext.Current.Session["TypeRole"];
            if (UserId != null) 
            { 
            ListMenuOption = GetMenuOptions(UserId.ToString());

            var GroupListMenuOption = ListMenuOption.GroupBy(x => x.MenuTabName).ToList();
            var Ve_Abo_Con = GroupListMenuOption.Where(w => w.Key == "Ventas Disposición").ToList();
            var Ve_Abo_Con_Franquicias = GroupListMenuOption.Where(w => w.Key == "Ventas Franquicias").ToList();
                //var Ventas = GroupListMenuOption.Where(w => w.Key == "Ventas Disposición").ToList();
                //var Abonos = GroupListMenuOption.Where(w => w.Key == "Ventas Disposición");
                //var Consultas = GroupListMenuOption.Where(w => w.Key == "Ventas Disposición");
                var Compras = GroupListMenuOption.Where(w => w.Key == "Compras").ToList();
            var Reportes = GroupListMenuOption.Where(w => w.Key == "Reportes").ToList();
            var Modificaciones = GroupListMenuOption.Where(w => w.Key == "Modificaciónes").ToList();
            var Configuracion = GroupListMenuOption.Where(w => w.Key == "Configuración").ToList();
            var Garantias = GroupListMenuOption.Where(w => w.Key == "Garantías").ToList();
            var Prueba = GroupListMenuOption.Where(w => w.Key == "Config").ToList();

            if (Ve_Abo_Con.Count > 0)
            {
                Controller.ViewBag.Ve_Abo_Con = Ve_Abo_Con.FirstOrDefault();
            }
            if (Ve_Abo_Con_Franquicias.Count > 0)
            {
                Controller.ViewBag.Ve_Abo_Con_Franquicias = Ve_Abo_Con_Franquicias.FirstOrDefault();
            }
            if (Compras.Count > 0)
            {
                Controller.ViewBag.Compras = Compras.FirstOrDefault();
            }
            if (Reportes.Count > 0)
            {
                Controller.ViewBag.Reportes = Reportes.FirstOrDefault();
            }
            if (Modificaciones.Count > 0)
            {
                Controller.ViewBag.Modificaciones = Modificaciones.FirstOrDefault();
            }
            if (Configuracion.Count > 0)
            {
                Controller.ViewBag.Configuracion = Configuracion.FirstOrDefault();
            }
            if (Garantias.Count > 0)
            {
                Controller.ViewBag.Garantias = Garantias.FirstOrDefault();
            }
        }
            #endregion    

            if (string.IsNullOrEmpty(SessionPersister.TypeRole))
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new
                    {
                        controller = "AccessDenied",
                        action = "Index"
                    }));
            else
            {
                AccountModel am = new AccountModel();

                string[] roles;
                roles = Roles.Split(',');

                if (!am.find(roles))
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new
                        {
                            controller = "AccessDenied",
                            action = "Index"
                        }));
            }
        }
        public List<ListMenuOption> GetMenuOptions(string idUser)
        {
            DataTable dt = new DataTable();
            string squery = string.Empty;
            DBMaster oDB = new DBMaster();
            try
            {
                var connstringWeb = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                squery = "select MT.MenuTabName,M.OptionURL,M.OptionName,M.OptionOrder from MenuOption M INNER JOIN MenuTab MT ON M.MenuTabID = MT.MenuTabID INNER JOIN AdminRoleMenuOption ARM ON ARM.MenuOptionID = M.MenuOptionID INNER JOIN AdminUser AU ON AU.AdminRoleID = ARM.AdminRoleID WHERE AU.AdminUserID = " + idUser + " and  M.Status = 'A'  order by OptionOrder asc";
                dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "MenuOption", connstringWeb);
                if (dt != null)
                {
                    var ls = (from DataRow rows in dt.Rows
                              orderby rows["OptionOrder"] ascending
                              select new ListMenuOption
                              {
                                  MenuTabName = rows["MenuTabName"] is DBNull ? "" : (string)rows["MenuTabName"],
                                  OptionName = rows["OptionName"] is DBNull ? "" : (string)rows["OptionName"],
                                  OptionURL = rows["OptionURL"] is DBNull ? "" : (string)rows["OptionURL"],
                                  OptionOrder = rows["OptionOrder"] is DBNull ? 0 : (byte)rows["OptionOrder"]
                              }).ToList();

                    return (ls);
                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}