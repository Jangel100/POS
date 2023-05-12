using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPOS.Security;
using BL.Interface;
using BL.Franquicias;
using Entities.Models.Configuracion;
using System.Net.Http;
using Newtonsoft.Json;
using Entities.viewsModels;
using System.Configuration;
using Entities.Models.Ventas;

namespace WebPOS.Controllers.VentasFranquicias
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class VentasConsultasFranquiciasController : Controller
    {
        readonly IFranquiciasBL _VentasFranquiciasBL;
        public VentasConsultasFranquiciasController(IFranquiciasBL ventasfranquiciasBL)
        {
            _VentasFranquiciasBL = ventasfranquiciasBL;
        }
        public VentasConsultasFranquiciasController()
        {
            _VentasFranquiciasBL = new FranquiciasBL();
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetInfoClienteConsultasF(string Prefix)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            Clientes clientes;
            try
            {
                clientes = new Clientes()
                {
                    IdStore = Convert.ToInt32(Session["IDSTORE"]),
                    Nombre = Prefix
                };


                url = "api/GetInfoClienteConsultasF";
                HttpResponseMessage response = _VentasFranquiciasBL.ReadAsStringAsyncAPI(url, clientes);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<Clientes>>().Result;

                if (result != null)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public ActionResult GetPeriodoClienteConsultasF(Clientes clientes)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {

                url = "api/GetPeriodoClienteConsultasF";
                HttpResponseMessage response = _VentasFranquiciasBL.ReadAsStringAsyncAPI(url, clientes);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<PeriodoView>>().Result;

                if (result != null)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public ActionResult GetDiaClienteConsultasF(PeriodoView periodo)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                periodo.idStore = Convert.ToInt32(Session["IDSTORE"]);

                url = "api/GetDiaClienteConsultasF";
                HttpResponseMessage response = _VentasFranquiciasBL.ReadAsStringAsyncAPI(url, periodo);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<DiaView>>().Result;

                if (result != null)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public ActionResult GetFolioClienteConsultasF(PeriodoView periodo)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {

                periodo.idStore = Convert.ToInt32(Session["IDSTORE"]);
                url = "api/GetFolioClienteConsultasF";
                HttpResponseMessage response = _VentasFranquiciasBL.ReadAsStringAsyncAPI(url, periodo);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<FolioView>>().Result;

                if (result != null)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public ActionResult GetClientePedidoConsultF(PedidoParameterIntoView pedido)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;
            try
            {
                pedido.IdStore = Convert.ToInt32(Session["IDSTORE"]);
                url = "api/GetClientePedidoConsultF";
                HttpResponseMessage response = _VentasFranquiciasBL.ReadAsStringAsyncAPI(url, pedido);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<PedidosView>>().Result;
                if (result != null)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public ActionResult ConfirmaEntregaF(string IdAbono)
        {
            string JsonResult = string.Empty;
            string url = "api/GetConfirmaEntregaF";           
            JsonResult = _VentasFranquiciasBL.ConfirmaEntrega(IdAbono);
            if (JsonResult != null)
            {
                return Json(JsonResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null);
            }


        }

    }
}