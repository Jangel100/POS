using BL.Configuracion;
using BL.Interface;
using Entities.Models.Franquicias;
using Entities.viewsModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using WebPOS.Security;

namespace WebPOS.Controllers.Configuracion
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class UsuariosController : Controller
    {
        readonly IUsuariosBL _UsuariosBL;
        public UsuariosController(IUsuariosBL UsuariosBL)
        {
            _UsuariosBL = UsuariosBL;
        }
        public UsuariosController()
        {
            _UsuariosBL = new UsuariosBL();
        }
        // GET: Usuarios
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetConsultaUsuarios()
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {

                UsersView UserView = new UsersView() { AdminUserID = Session["AdminUserID"].ToString(), TypeRole = Session["TypeRole"].ToString(), Franquicia = Session["Franquicia"].ToString() };
                parameterJson = JsonConvert.SerializeObject(UserView);

                url = $"api/GetConsultaUsuarios?json={parameterJson}";
                HttpResponseMessage response = _UsuariosBL.GetResponseAPIUsuarios(url);

                response.EnsureSuccessStatusCode();

                var usuarios = response.Content.ReadAsAsync<List<UsuariosConfiguracionView>>().Result;

                if (usuarios != null)
                {
                    JsonResult = JsonConvert.SerializeObject(usuarios);
                    return Json(new { data = usuarios }, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult Add()
        {
            UsersConfigView usersConfigView;
            try
            {
                usersConfigView = GetusersConfig();

                return View(usersConfigView);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public JsonResult Save(UsersAdmin usersAdmin)
        {

            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                url = "api/AddUsuarios";
                HttpResponseMessage response = _UsuariosBL.ReadAsStringAsyncAPI(url, usersAdmin);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<UsersAdmin>().Result;
                if (result != null)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(JsonResult);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }
            return Json(null);
        }
        public JsonResult AddStore(List<AdminStore> AdminStore)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {

                var ls = (from p in AdminStore
                          select p).FirstOrDefault();

                UsersView UserView = new UsersView() { AdminUserID = ls.AdminUserID.ToString(), Franquicia = Session["Franquicia"].ToString() };

                //StoreDeleteByUSer(UserView);

                url = "api/AddStore";
                HttpResponseMessage response = _UsuariosBL.ReadAsStringAsyncAPI(url, AdminStore);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<string>().Result;

                if (result != null)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(JsonResult);
                }

                return Json(null);
            }
            catch (Exception exp)
            {
                return null;
            }
        }
        public int StoreDeleteByUSer(UsersView users)
        {

            string url = string.Empty;
            try
            {

                url = "api/StoreDeleteByUSer";
                HttpResponseMessage response = _UsuariosBL.ReadAsStringAsyncAPI(url, users);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<int>().Result;

                if (result > 0)
                {
                    return result;
                }

                return 0;
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
        public JsonResult StoreByUserInsert(UsersAdmin usersAdmin)
        {

            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                parameterJson = JsonConvert.SerializeObject(usersAdmin);

                url = "api/AddUsuarios";
                HttpResponseMessage response = _UsuariosBL.ReadAsStringAsyncAPI(url, usersAdmin);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<UsersAdmin>().Result;

                if (result != null)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(JsonResult);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }
            return Json(null);
        }
        public UsersConfigView GetusersConfig()
        {
            List<RolesView> lsrolesViews;
            List<AdminStore> lsadminStores;
            try
            {

                UsersView users = new UsersView()
                {
                    AdminUserID = Session["AdminUserID"].ToString(),
                    Franquicia = Session["Franquicia"].ToString()
                };

                lsrolesViews = GetListaRoles();
                lsadminStores = GetListaAdminTiendas(users);

                UsersConfigView usersConfigView = new UsersConfigView()
                {
                    Accion = "Agregar",
                    AdminRoleID = 0,
                    Status = "A",
                    lsRoles = lsrolesViews,
                    lsTiendas = lsadminStores
                };

                return usersConfigView;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<RolesView> GetListaRoles()
        {
            string JsonResult = string.Empty;
            string Json = string.Empty;
            string Jsonparameter = string.Empty;
            string url = string.Empty;

            try
            {
                UsersView users = new UsersView()
                {
                    TypeRole = Session["TypeRole"].ToString(),
                    Franquicia = Session["Franquicia"].ToString(),
                    IDSTORE = "0"
                };

                Json = JsonConvert.SerializeObject(users);

                url = $"api/GetListaRoles?json={Json}";

                HttpResponseMessage response = _UsuariosBL.GetResponseAPIUsuarios(url);
                response.EnsureSuccessStatusCode();
                var ViewResult = response.Content.ReadAsAsync<List<RolesView>>().Result;

                return ViewResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<AdminStore> GetListaAdminTiendas(UsersView users)
        {
            string JsonResult = string.Empty;
            string Json = string.Empty;
            string Jsonparameter = string.Empty;
            string url = string.Empty;

            try
            {
                Json = JsonConvert.SerializeObject(users);

                url = $"api/GetListaAdminTiendas?json={Json}";

                HttpResponseMessage response = _UsuariosBL.GetResponseAPIUsuarios(url);
                response.EnsureSuccessStatusCode();
                var ViewResult = response.Content.ReadAsAsync<List<AdminStore>>().Result;

                return ViewResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int DeleteStore(UsersView usersView)
        {
            string url;
            try
            {
                url = "api/AddStore";
                HttpResponseMessage response = _UsuariosBL.ReadAsStringAsyncAPI(url, usersView);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<int>().Result;

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult Modify(string AdminUserID, string Action)
        {
            UsersAdmin usersAdmin;
            UsersView usersView = new UsersView();
            UsersConfigView usersConfigView;
            List<RolesView> lsrolesViews;
            List<AdminStore> lsAdminStore;
            try
            {
                usersView.AdminUserID = AdminUserID;
                usersAdmin = SelectUserStore(usersView);

                UsersView users = new UsersView()
                {
                    AdminUserID = AdminUserID,
                    Franquicia = Session["Franquicia"].ToString()
                };

                lsAdminStore = GetStoreByUserSelected(users);

                lsrolesViews = GetListaRoles();

                usersConfigView = new UsersConfigView()
                {
                    Accion = "Modificar",
                    AdminUserID = usersAdmin.AdminUserID,
                    AdminRoleID = Convert.ToInt32(usersAdmin.AdminRoleID),
                    CorreoElectronico = usersAdmin.CorreoElectronico,
                    EmployeeNum = usersAdmin.EmployeeNum,
                    FirstName = usersAdmin.FirstName,
                    LastName = usersAdmin.LastName,
                    NoCelular = usersAdmin.NoCelular,
                    NTUserAccount = usersAdmin.NTUserAccount,
                    NTUserDomain = usersAdmin.NTUserDomain,
                    Status = usersAdmin.Status,
                    lsRoles = lsrolesViews,
                    lsTiendas = lsAdminStore
                };

                return View("Add", usersConfigView);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public UsersAdmin SelectUserStore(UsersView usersView)
        {
            string url;
            try
            {
                url = "api/SelectUser";
                HttpResponseMessage response = _UsuariosBL.ReadAsStringAsyncAPI(url, usersView);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<UsersAdmin>().Result;

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<AdminStore> GetStoreByUserSelected(UsersView users)
        {
            string JsonResult = string.Empty;
            string Json = string.Empty;
            string Jsonparameter = string.Empty;
            string url = string.Empty;

            try
            {
                Json = JsonConvert.SerializeObject(users);

                url = $"api2/GetStoreSelected?json={Json}";

                HttpResponseMessage response = _UsuariosBL.GetResponseAPIUsuarios(url);
                response.EnsureSuccessStatusCode();
                var ViewResult = response.Content.ReadAsAsync<List<AdminStore>>().Result;

                return ViewResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public JsonResult Update(UsersAdmin usersAdmin)
        {

            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;
            usersAdmin.Franquicia = Session["Franquicia"].ToString();
            try
            {

                url = "api2/UdapteUser";
                HttpResponseMessage response = _UsuariosBL.ReadAsStringAsyncAPI(url, usersAdmin);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<UsersAdmin>().Result;

                if (result != null)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(JsonResult);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }
            return Json(null);
        }

    }
}