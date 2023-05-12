using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPOS.Security;
using BL.Interface;
using BL.Franquicias;
using Entities.Models.Configuracion;
using Entities.Models.Garantias;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

namespace WebPOS.Controllers.Garantias
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class GarantiasController : Controller
    {
        readonly IFranquiciasBL _GarantiasBL;
        // GET: Garantias
        public GarantiasController(IFranquiciasBL garantiasBL)
        {
            _GarantiasBL = garantiasBL;
        }
        public GarantiasController()
        {
            _GarantiasBL = new FranquiciasBL();
        }
        public ActionResult Index()
        {
            return View("Index");
        }
        public JsonResult LoadDatas()
        {
            string JsonResult = string.Empty;
            List<GarantiasIn> ListGarantias;
            try
            {
                ListGarantias = _GarantiasBL.GetGarantias();
                if (ListGarantias != null)
                {
                    JsonResult = JsonConvert.SerializeObject(ListGarantias);
                    return Json(JsonResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(null);
        }
        public ActionResult ApruebaGarantia()
        {
            return View();
        }
        public JsonResult GarantiasxAprobar()
        {
            string JsonResult = string.Empty;
            List<GarantiasIn> ListaGaxAprobar;
            try
            {
                ListaGaxAprobar = _GarantiasBL.GetGarantiasxAprobar();
                if (ListaGaxAprobar != null)
                {
                    JsonResult = JsonConvert.SerializeObject(ListaGaxAprobar);
                    return Json(JsonResult);
                }
            }
            catch(Exception ex)
            {


            }
            return Json(null);
        }

        public JsonResult RealizaAccion(string JsonObj)
        {
            try
            {
                string folderPath = @"C:\UploadedFiles";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);

                }
                string JsonString = string.Empty;
                var ResponseAction = _GarantiasBL.ResponseAction(JsonObj);
                var JsonResult = JsonConvert.SerializeObject(ResponseAction);
                return Json(JsonResult);
            }
            catch (Exception Ex)
            {

            }

            return Json(null);
        }

        public ActionResult SolicitudGarantia()
        {
            return View();
        }
    }
}