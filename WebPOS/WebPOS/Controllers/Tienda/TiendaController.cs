using BL.Interface;
using BL.Login;
using CrystalDecisions.CrystalReports.Engine;
using Entities.viewsModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebPOS.Security;
using WebPOS.Utilities;

namespace WebPOS.Controllers.Tienda
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class TiendaController : Controller
    {
        private string _token = ConfigurationManager.AppSettings["SecretWebToken"].ToString();
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        readonly ILoginBL _login;
        public TiendaController(ILoginBL login)
        {
            _login = login;
        }

        public TiendaController()
        {
            _login = new LoginBL();
        }

        public HttpResponseMessage HttpResponseMessageTiendas(string url, string Franquicia)
        {
            try
            {
                string tiendaLoginJson = string.Empty;

                TiendaJsonView tiendaView = new TiendaJsonView() { AdminUserID = Session["AdminUserID"].ToString(), Franquicia = Franquicia };
                tiendaLoginJson = JsonConvert.SerializeObject(tiendaView);

                HttpResponseMessage response = _login.GetResponseAPILogin("api/TiendasApi?tiendaLoginJson=" + tiendaLoginJson + "");

                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public JsonResult GetTiendas()
        {
            string JsonResult = string.Empty;
            string url = string.Empty;

            try
            {
                url = "api/TiendasApi?tiendaLoginJson=";

                HttpResponseMessage response = HttpResponseMessageTiendas(url, Session["Franquicia"].ToString());

                response.EnsureSuccessStatusCode();
                var tiendaViewResult = response.Content.ReadAsAsync<List<TiendaView>>().Result;

                //Serializa el objecto para enviarlo en forma de json
                JsonResult = JsonConvert.SerializeObject(tiendaViewResult);

                return Json(JsonResult);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void loadSesionStore(TiendaSelectedView Store)
        {
            Session["TiendaSesion"] = Store.Franquicia;
            Session["WHSID"] = Store.WhsId;
            Session["FRCARDCODE"] = Store.Franquicia;
            Session["IDSTORE"] = Store.AdminStoreID;
            Session["DEFAULTLIST"] = Store.DefaultList; ;
            Session["STORETYPEID"] = Store.StoreTypeID;
            Session["WHSCONSIGID"] = Store.whsConsigID;
            Session["IVA"] = Store.actIVA;
            Session["DEFAULTCUSTOMER"] = Store.DefaultCustomer;
            Session["ESCONSIGNA"] = Store.ESCONSIGNA;
            Session["FRDBNAME"] = Store.DBName;
            Session["FRSBOUSER"] = Store.SBOUser;
            Session["FRSBOPASS"] = Store.SBOPAss;
            Session["FRTRANSITWHSID"] = Store.TransitWhsID;
            Session["CurrentClient"] = null;
        }

        [HttpPost]
        public JsonResult GetSelectedTienda(viewAccestoken Accestoken)
        {
            try
            {
                string JsonResult = string.Empty;
                string valuetiendaJson = string.Empty;
                string Usuario = string.Empty;
                valuetiendaJson = JsonConvert.SerializeObject(Accestoken.id+","+_token);
                
                Accestoken.webtoken = _token;
                var login = new LoginBL(Accestoken.webtoken);

                HttpResponseMessage response = _login.GetResponseAPILogin("api/GetStore?valuetiendaJson=" + valuetiendaJson + "");
                response.EnsureSuccessStatusCode();
                var FirstViewResult = response.Content.ReadAsAsync<TiendaSelectedView>().Result;


                if (FirstViewResult != null)
                {
                    loadSesion(FirstViewResult);

                    var TextoMarquesina = GetTextoMarquesina(FirstViewResult.Franquicia);
                   // var jsonTextoMArquesina = JsonConvert.DeserializeObject<string>(TextoMarquesina);

                    PedidoDiaTextMarquesinaView pedidoDiaTextMarquesinaView = new PedidoDiaTextMarquesinaView()
                    {
                        lsPedidoDiaView = "",
                        TextoMarquesina = TextoMarquesina,
                        Franquicia = FirstViewResult.Franquicia
                    };

                    var JsonpedidoDiaTextMarquesina = JsonConvert.SerializeObject(pedidoDiaTextMarquesinaView);

                    return Json(JsonpedidoDiaTextMarquesina);
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

        public string GetTextoMarquesina(string frCardCode)
        {
            try
            {
                string JsonResult = string.Empty;
                string frCardCodeJson = string.Empty;
                frCardCodeJson = JsonConvert.SerializeObject(frCardCode);

                HttpResponseMessage response = _login.GetResponseAPILogin("api/GetTextoMarquesina?frCardCodeJson=" + frCardCodeJson + "");
                response.EnsureSuccessStatusCode();
                var TextoMarquesinaViewResult = response.Content.ReadAsAsync<string>().Result;

                return TextoMarquesinaViewResult;
            }
            catch (Exception ex)
            {
                return "false";
            }
        }
        public ActionResult GetEnvioDia()
        {
            try
            {
                string JsonResult = string.Empty;
                string StoreJson;

                var IDSTORE = Session["IDSTORE"].ToString();

                PaginationTableView paginationTableView = new PaginationTableView() { Id = Convert.ToInt32(IDSTORE) };

                StoreJson = JsonConvert.SerializeObject(paginationTableView);

                HttpResponseMessage response = _login.GetResponseAPILogin("api/GetEnvioToday?StoreJson=" + StoreJson + "");
                response.EnsureSuccessStatusCode();
                var tiendaViewResult = response.Content.ReadAsAsync<List<PedidosxDiaView>>().Result;

                return Json(new { data = tiendaViewResult }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult EnviosDelDiaPartial(List<Entities.viewsModels.PedidosxDiaView> pedidosxDiaViews)
        {
            //May be you want to pass the posted model to the parial view?
            return PartialView("_PartialEnviosDelDia", pedidosxDiaViews);
        }
        public JsonResult GetFranquicia()
        {
            try
            {
                string JsonResult = string.Empty;

                HttpResponseMessage response = _login.GetResponseAPILogin("api/GetFranquicias");
                response.EnsureSuccessStatusCode();
                
                var ViewResult = response.Content.ReadAsAsync<List<FranquiciasView>>().Result;

                //Serializa el objecto para enviarlo en forma de json
                JsonResult = JsonConvert.SerializeObject(ViewResult);

                return Json(JsonResult);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public JsonResult GetFranquiciaTiendas(string frCardCode)
        {
            string JsonResult = string.Empty;
            string url = string.Empty;

            try
            {
                url = "api/GetFranquiciaTiendas?frCardCodeJson=";

                HttpResponseMessage response = HttpResponseMessageTiendas(url, frCardCode);

                response.EnsureSuccessStatusCode();
                var ViewResult = response.Content.ReadAsAsync<List<TiendaView>>().Result;

                //Serializa el objecto para enviarlo en forma de json
                JsonResult = JsonConvert.SerializeObject(ViewResult);

                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private void loadSesion(TiendaSelectedView Store)
        {
            System.Web.HttpContext.Current.Session["TiendaSesion"] = Store.Franquicia;
            System.Web.HttpContext.Current.Session["WHSID"] = Store.WhsId;
            System.Web.HttpContext.Current.Session["FRCARDCODE"] = Store.Franquicia;
            System.Web.HttpContext.Current.Session["IDSTORE"] = Store.AdminStoreID;
            System.Web.HttpContext.Current.Session["DEFAULTLIST"] = Store.DefaultList; ;
            System.Web.HttpContext.Current.Session["STORETYPEID"] = Store.StoreTypeID;
            System.Web.HttpContext.Current.Session["WHSCONSIGID"] = Store.whsConsigID;
            System.Web.HttpContext.Current.Session["IVA"] = Store.actIVA;
            System.Web.HttpContext.Current.Session["DEFAULTCUSTOMER"] = Store.DefaultCustomer;
            System.Web.HttpContext.Current.Session["ESCONSIGNA"] = Store.ESCONSIGNA;
            System.Web.HttpContext.Current.Session["FRDBNAME"] = Store.DBName;
            System.Web.HttpContext.Current.Session["FRSBOUSER"] = Store.SBOUser;
            System.Web.HttpContext.Current.Session["FRSBOPASS"] = Store.SBOPAss;
            System.Web.HttpContext.Current.Session["FRTRANSITWHSID"] = Store.TransitWhsID;
            System.Web.HttpContext.Current.Session["CurrentClient"] = null;
        }

    }
}