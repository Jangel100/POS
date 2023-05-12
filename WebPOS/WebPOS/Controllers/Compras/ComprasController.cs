using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPOS.Security;
using BL.Abonos;
using BL.Interface;
using BL.Compras;
using Entities.viewsModels;
using Entities;
using Newtonsoft.Json;
using Entities.Models.Compras;
using System.Configuration;
using System.Net.Http;

namespace WebPOS.Controllers.Compras
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class ComprasController : Controller
    {
        readonly IComprasBL _ComprasBL;
        private String _token = ConfigurationManager.AppSettings["SecretWebToken"].ToString();
        // GET: Compras
        public ComprasController(IComprasBL comprasBL)
        {
            _ComprasBL = comprasBL;
        }
        public ComprasController()
        {
            _ComprasBL = new ComprasBL();
        }
        public ActionResult Index()
        {
            ArticulosBaseView ArticulosBaseView;
            var idusuaro = Convert.ToInt32(Session["AdminUserID"]);
            var usuarioCvecorta = 1142;
            ViewBag.Idstore = Session["IDSTORE"].ToString();
            ViewBag.Idusuario = Session["AdminUserID"].ToString();
            ViewBag.WhsID = Session["WHSID"].ToString();
            ViewBag.labelsel = "Modelo";
            ViewBag.DEFAULTLIST = Session["DEFAULTLIST"].ToString();
            if (idusuaro == usuarioCvecorta)
            {
                ViewBag.labelsel = "Clave corta";
            }
            ArticulosBaseView = Crearventa(idusuaro, usuarioCvecorta);
            return View("Compras", ArticulosBaseView);
        }
        public ActionResult ReimpresionCompras()
        {
            TiendasReimpresionComprasView TiendasReimpresionComprasView;
            TiendasReimpresionComprasView = GetReimpCompras();
            return View("ReimpresionCompras", TiendasReimpresionComprasView);
        }
        public ArticulosBaseView Crearventa(int idusuaro, int usuarioCvecorta)
        {
            string JsonResult = string.Empty;
            ArticulosBaseView ArticulosBaseView;
            List<ListArticuloView> ListArt;
            List<ListLineaView> ListLinea = new List<ListLineaView>();
            List<ListMedidaView> ListMedida = new List<ListMedidaView>();
            List<ListModeloView> ListModelo;
            ListRadioButton listB = new ListRadioButton();
            listB.CheckLine = true;
            try
            {
                ListArt = _ComprasBL.GetArticulos();
                if (idusuaro == usuarioCvecorta)
                {
                    ListModelo = _ComprasBL.GetModelosCveCorta("Linea"); //Clave corta
                }
                else
                {
                    ListModelo = _ComprasBL.GetModelos("Linea");
                }
                ArticulosBaseView = new ArticulosBaseView()
                {
                    ListArticulos = ListArt,
                    ListLinea = ListLinea,
                    ListMedida = ListMedida,
                    ListModelo = ListModelo,
                    CantidadBodega = "",
                    CantidadTienda = "",
                    CantidadBodegaDisp = 0,
                    CantidadTiendaDisp = 0,
                    DescriptionJuego = "",
                    ListRadioButton = listB
                };
                return ArticulosBaseView;
            }
            catch (Exception ex) { return null; }
        }

        public JsonResult GetSelectOnchangeCveC(string tipo)
        {
            string JsonResult = string.Empty;
            try
            {
                var result = _ComprasBL.GetModelosCveCorta(tipo);
                JsonResult = JsonConvert.SerializeObject(result);
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public JsonResult AddPurchase(string CapPurchase)
        {
            string url = string.Empty;
            var jsoncompra = JsonConvert.DeserializeObject<AddPurchase>(CapPurchase);
            AddPurchase Compra = new AddPurchase();
            string JsonResult = string.Empty;
            jsoncompra.webToken = _token;
            url = "api/AddPurchase";
            HttpResponseMessage response = _ComprasBL.ReadAsStringAsyncAPI(url, jsoncompra);
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsAsync<AddPurchase>().Result;
            if (result.statusresponse)
            {
                JsonResult = JsonConvert.SerializeObject(result);
                return Json(JsonResult);
            }
            else
            {

                JsonResult = "ERROR";
                return Json(JsonResult);
            }

        }
        public TiendasReimpresionComprasView GetReimpCompras()
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            TiendasReimpresionComprasView TiendasReimpresionComprasView = new TiendasReimpresionComprasView();
            var payback = new PaybackController();
            String LogWeb = AppDomain.CurrentDomain.BaseDirectory + @"logErrorPayback\LogWeb_" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace("T", "_").Replace("-", "_").Substring(0, 10) + ".txt";
            payback.anade_linea_archivo2(LogWeb, "Entra a Compras: ");
            try
            {
                url = "api2/GetStorebyUserNew";
                TiendaJsonView tiendaView = new TiendaJsonView() { AdminUserID = Session["AdminUserID"].ToString(), Franquicia = Session["FRCARDCODE"].ToString() };

                HttpResponseMessage response = _ComprasBL.ReadAsStringAsyncAPI(url, tiendaView);
                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsAsync<List<TiendaJsonView>>().Result;
                TiendasReimpresionComprasView.TiendaReimpresionJson = result;
                return TiendasReimpresionComprasView;

            }
            catch (Exception ex)
            {
                var error = "";
                error = ex.Message;
                // log
                payback.anade_linea_archivo2(LogWeb, "Error al entrar a compras ERROR: " + error);
                return null;
            }

            return null;
        }
        public JsonResult GetReimpComprasAP(ParametersReqReimpCO ObjCom)
        {
            try
            {
                string CONSTRING = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                var INITIALCATALOG = CONSTRING.Split(new char[] { ';', '=' }, StringSplitOptions.RemoveEmptyEntries)[9];//CONSTRING.Split(";")(1).Split("=").Clone(1) + ".DBO";
                string url = string.Empty;
                string JsonResult = string.Empty;
                ObjCom.SesionSAPDB = INITIALCATALOG + ".DBO";
                ObjCom.AdminUserID = Session["AdminUserID"].ToString();
                ObjCom.SesionFRCARDCODE = Session["FRCARDCODE"].ToString();
                url = "api2/GetReportsReimpNomina";
                HttpResponseMessage response = _ComprasBL.ReadAsStringAsyncAPI(url, ObjCom);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<ReimpresionComprasView>>().Result;
                if (result != null)
                {
                    var json = Json(result, JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = 500000000;
                    //JsonResult = serializer.Serialize(result);
                    return json;
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