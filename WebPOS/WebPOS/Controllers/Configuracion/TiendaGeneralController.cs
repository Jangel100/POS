using BL.Configuracion;
using BL.Interface;
using Entities.Models.Configuracion;
using Entities.viewsModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using WebPOS.Security;

namespace WebPOS.Controllers.Configuracion
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class TiendaGeneralController : Controller
    {
        readonly ITiendasBL _TiendasBL;
        public TiendaGeneralController(ITiendasBL TiendaBL)
        {
            _TiendasBL = TiendaBL;
        }
        public TiendaGeneralController()
        {
            _TiendasBL = new TiendasBL();
        }
        // GET: TiendaGeneral
        public ActionResult Index()
        {
            return View("");
        }
        public JsonResult GetConsultaTiendas()
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                url = $"api2/GetConsultaTiendas";
                HttpResponseMessage response = _TiendasBL.GetResponseAPITiendas(url);

                response.EnsureSuccessStatusCode();

                var usuarios = response.Content.ReadAsAsync<List<TiendaConfiguracionView>>().Result;

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
            TiendaConfigView TiendaConfigView = new TiendaConfigView();
            try
            {
                TiendaConfigView = GetTiendasConfig("");

                TiendaConfigView.Status = "A";
                TiendaConfigView.Accion = "Agregar";

                return View(TiendaConfigView);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public TiendaConfigView GetTiendasConfig(string idStore)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                //url = $"api2/GetTiendaConfig";
                url = $"api2/GetTiendaConfig?idStore=" + idStore + "";
                HttpResponseMessage response = _TiendasBL.GetResponseAPITiendas(url);

                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsAsync<TiendaConfigView>().Result;

                if (result != null)
                {
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public JsonResult Save(StoreAdmin StoreAdmin)
        {

            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                url = "api2/AddTiendas";
                HttpResponseMessage response = _TiendasBL.ReadAsStringAsyncAPI(url, StoreAdmin);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<StoreAdmin>().Result;

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

        public ActionResult Modify(string AdminStoreID, string Action)
        {
            TiendaConfigView TiendaConfigView = new TiendaConfigView();
            StoreAdmin storeAdmin;
            try
            {
                storeAdmin = SelectUserStore(AdminStoreID);

                if (storeAdmin != null)
                {
                    TiendaConfigView = GetTiendasConfig(AdminStoreID);

                    TiendaConfigView.AdminStoreID = Convert.ToInt32(AdminStoreID);

                    TiendaConfigView.Status = "A";
                    TiendaConfigView.Accion = "Modificar";
                    TiendaConfigView.StoreName = storeAdmin.StoreName;
                    TiendaConfigView.StoreTypeID = Convert.ToInt32(storeAdmin.StoreTypeID);
                    TiendaConfigView.WHSID = storeAdmin.WHSID;                                                                                                                                                                                                                                                                                                               
                    TiendaConfigView.CalleNumero = storeAdmin.CalleNumero;
                    TiendaConfigView.AdminStoreID = storeAdmin.AdminStoreID;
                    TiendaConfigView.NumExt = storeAdmin.NumExt;
                    TiendaConfigView.NumInt = storeAdmin.NumInt;
                    TiendaConfigView.Telefono = storeAdmin.Telefono;
                    TiendaConfigView.EstadoId = Convert.ToInt32(storeAdmin.EstadoId);
                    TiendaConfigView.whsConsigID = Convert.ToInt32(storeAdmin.whsConsigID);
                    TiendaConfigView.FolderPDF = storeAdmin.FolderPDF;
                    TiendaConfigView.Status = storeAdmin.Status;
                    TiendaConfigView.Delegacion = storeAdmin.Delegacion;
                    TiendaConfigView.CodigoPostal = storeAdmin.CodigoPostal;
                    TiendaConfigView.emailTienda = storeAdmin.emailTienda;
                    TiendaConfigView.AlmacenSapPropio = storeAdmin.AlmacenSapPropio;
                    TiendaConfigView.BodegaPropia = storeAdmin.BodegaPropia;
                    TiendaConfigView.actIVA = storeAdmin.actIVA;
                    TiendaConfigView.Colonia = storeAdmin.Colonia;

                    return View("Add", TiendaConfigView);
                }



                return View("Add", null);
            }
            catch (Exception ex)
            {

                throw;
            }

            return null;
        }

        public StoreAdmin SelectUserStore(string AdminStoreID)
        {
            string url;
            StoreAdmin storeAdmin;
            try
            {
                storeAdmin = new StoreAdmin() { AdminStoreID = Convert.ToInt32(AdminStoreID) };

                url = "api2/GetConsultaTiendasxId";
                HttpResponseMessage response = _TiendasBL.ReadAsStringAsyncAPI(url, storeAdmin);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<StoreAdmin>().Result;

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public JsonResult Update(StoreAdmin StoreAdmin)
        {

            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                url = "api2/UpdateStores";
                HttpResponseMessage response = _TiendasBL.ReadAsStringAsyncAPI(url, StoreAdmin);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<int>().Result;

                JsonResult = JsonConvert.SerializeObject(result);
                return Json(JsonResult);
            }
            catch (Exception ex)
            {

            }
            return Json(null);
        }
        public List<SelectedFolios> ListFolios(int StoreId, string Accion)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                url = "api2/SelectedFolios";
                HttpResponseMessage response = _TiendasBL.ReadAsStringAsyncAPI(url, StoreId);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<SelectedFolios>>().Result;
                return result;

            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public ActionResult Mensaje()
        {
            ViewBag.Franquicia = Session["Franquicia"].ToString();
            return View("Mensaje");
        }
        public JsonResult UpdateMessage(string mensaje, string Franquicia)
        {
            var jsondat = new
            {
                mensaje = mensaje,
                Franquicia = Franquicia
            };
            string jsondata = JsonConvert.SerializeObject(jsondat);
            string JsonResult = string.Empty;
            JsonResult = JsonConvert.SerializeObject(_TiendasBL.UpdateMessage(jsondata));
            return Json(JsonResult);
        }
        public ActionResult AprobDesc()
        {
            try
            {
                var Lista = new ListaApDesc();
                var url = "api4/GetInfoApDesc";
                HttpResponseMessage response = _TiendasBL.GetResponseAPITiendas(url);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<AprobacionDescuentosView>>().Result;
                Lista.ListaDescuento = result;
                return View("AprobDesc", Lista);
            }
            catch { }
            finally { }
            return View();
        }
        public JsonResult Btn_Aceptar(string iddescuento, int Status, string Descuento)
        {
            var message = new MessageView();
            var AdminUserID = Session["AdminUserID"].ToString();
            var url = "api4/GetAceptar?iddescuento=" + iddescuento + "&Status=" + Status + "&Descuento=" + Descuento + "&AdminUserID=" + AdminUserID + "";
            HttpResponseMessage response = _TiendasBL.GetResponseAPITiendas(url);
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsAsync<int>().Result;
            if (result > 0)
            { 
                message.Success = true;
            }
            else
            {
                message.Success = true;
                message.Message = "Error al aceptar descuento, por favor intente de nuevo";
            }
            var jsonSer = JsonConvert.SerializeObject(message);
            return Json(jsonSer);
        }
    }
}