using BL.Configuracion;
using BL.Interface;
using Entities.Models.Configuracion;
using Entities.viewsModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebPOS.Security;

namespace WebPOS.Controllers.AdminRole
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class AdminRoleController : Controller
    {
        readonly IAdminRoleBL _RolesBL;
        public AdminRoleController(IAdminRoleBL RolesBL)
        {
            _RolesBL = RolesBL;
        }
        public AdminRoleController()
        {
            _RolesBL = new AdminRoleBL();
        }
        // GET: AdminRole
        public ActionResult AdminRole()
        {
            return View();
        }
        public JsonResult GetInfoRoles()
        {
            string JsonResult = string.Empty;
            string JsonR = string.Empty;
            string Jsonparameter = string.Empty;
            string url = string.Empty;

            try
            {
                UsersView users = new UsersView()
                {
                    TypeRole = Session["TypeRole"].ToString(),
                    Franquicia = Session["Franquicia"].ToString()
                };

                JsonR = JsonConvert.SerializeObject(users);

                url = $"api2/GetInfoRole?json={JsonR}";

                HttpResponseMessage response = _RolesBL.GetResponseAPIRoles(url);
                response.EnsureSuccessStatusCode();
                var ViewResult = response.Content.ReadAsAsync<List<RolesView>>().Result;
                JsonResult = JsonConvert.SerializeObject(ViewResult);
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult Add()
        {
            string JsonResult = string.Empty;
            string JsonR = string.Empty;
            string url = string.Empty;
            try
            {
                UsersView users = new UsersView()
                {
                    AdminUserID = "0",
                    TypeRole = Session["TypeRole"].ToString(),
                    Franquicia = Session["Franquicia"].ToString()
                };

                JsonR = JsonConvert.SerializeObject(users);

                url = $"api2/GetMenuInfoRole?json={JsonR}";

                HttpResponseMessage response = _RolesBL.GetResponseAPIRoles(url);
                response.EnsureSuccessStatusCode();
                var ViewResult = response.Content.ReadAsAsync<MenuOptionInfoRol>().Result;
                ViewResult.MenuInfoRolE = ViewResult.MenuInfoRol.GroupBy(x => x.MenuTabName).ToList();
                ViewResult.AdminRoleID = Convert.ToInt32(0);
                //ViewResult.FRCARDCODE = Session["FRCARDCODE"].ToString();
                ViewResult.Action = "Agregar Rol";
                ViewResult.TextBtn = "Agregar";
                ViewResult.TextBtn = "Agregar";
                //JsonResult = JsonConvert.SerializeObject(ViewResult);
                return View("RolesView", ViewResult);
            }
            catch (Exception ex)
            {

                return View("RolesView", null);
            }
        }
        public ActionResult Modify(string AdminRoleID,string RoleName)
        {
            string JsonResult = string.Empty;
            string JsonR = string.Empty;
            string url = string.Empty;
            try
            {
                UsersView users = new UsersView()
                {
                    AdminUserID = AdminRoleID,
                    TypeRole = Session["TypeRole"].ToString(),
                    Franquicia = Session["Franquicia"].ToString(),
                    UserName = RoleName                    
                };

                JsonR = JsonConvert.SerializeObject(users);

                url = $"api2/GetMenuInfoRole?json={JsonR}";

                HttpResponseMessage response = _RolesBL.GetResponseAPIRoles(url);
                response.EnsureSuccessStatusCode();
                var ViewResult = response.Content.ReadAsAsync<MenuOptionInfoRol>().Result;
                ViewResult.MenuInfoRolE = ViewResult.MenuInfoRol.GroupBy(x => x.MenuTabName).ToList();
                ViewResult.AdminRoleID = Convert.ToInt32(AdminRoleID);
                //ViewResult.FRCARDCODE = Session["FRCARDCODE"].ToString();
                ViewResult.Action = "Editar Rol";
                ViewResult.TextBtn = "Actualizar";
                //ViewResult.RoleName = RoleName;
                //JsonResult = JsonConvert.SerializeObject(ViewResult);
                return View("RolesView", ViewResult);
            }
            catch (Exception ex)
            {

                return View("RolesView", null);
            }
        }
        [HttpPost]
        public ActionResult UpdateMenuOption(RequestModMenOp JsonMenuOp)
        {
            string JsonResult = string.Empty;
            try
            {
                JsonMenuOp.TypeRole = Session["TypeRole"].ToString();
                var JsonR = JsonConvert.SerializeObject(JsonMenuOp);
                var url = $"api2/GetNewMenuOptionUpdate?json={JsonR}";

                HttpResponseMessage response = _RolesBL.GetResponseAPIRoles(url);
                response.EnsureSuccessStatusCode();
                var ViewResult = response.Content.ReadAsAsync<MessageView>().Result;
                JsonResult = JsonConvert.SerializeObject(ViewResult);
                return Json(JsonResult);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult Delete(string AdminRoleID)
        {
            string JsonResult = string.Empty;
            try
            {
                var url = $"api2/GetDeleteRol?AdminRoleID={AdminRoleID}";

                HttpResponseMessage response = _RolesBL.GetResponseAPIRoles(url);
                response.EnsureSuccessStatusCode();
                var ViewResult = response.Content.ReadAsAsync<MessageView>().Result;
                JsonResult = JsonConvert.SerializeObject(ViewResult);
                return Json(JsonResult);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

    }
}