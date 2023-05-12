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
using System.Configuration;
using System.Net;
using System.IO;
using Microsoft.VisualBasic;
using System.Text;

namespace WebPOS.Controllers.Ventas
{

    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class VentasConsultasController : Controller
    {
        readonly IVentasBL _VentasBL;
        public VentasConsultasController(IVentasBL ventasBL)
        {
            _VentasBL = ventasBL;
        }
        public VentasConsultasController()
        {
            _VentasBL = new VentasBL();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetInfoClienteConsultas(string Prefix)
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


                url = "api2/GetInfoClienteConsultas";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, clientes);
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
        public ActionResult GetPeriodoClienteConsultas(Clientes clientes)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {

                url = "api2/GetPeriodoClienteConsultas";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, clientes);
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
        public ActionResult GetDiaClienteConsultas(PeriodoView periodo)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {

                periodo.idStore = Convert.ToInt32(Session["IDSTORE"]);

                url = "api2/GetDiaClienteConsultas";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, periodo);
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
        public ActionResult GetFolioClienteConsultas(PeriodoView periodo)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {

                periodo.idStore = Convert.ToInt32(Session["IDSTORE"]);

                url = "api2/GetFolioClienteConsultas";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, periodo);
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
        public ActionResult GetClientePedidoConsult(PedidoParameterIntoView pedido)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {

                pedido.IdStore = Convert.ToInt32(Session["IDSTORE"]);

                url = "api2/GetClientePedidoConsult";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, pedido);
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

        public string Getpdf(string Año, string Tienda, string mes, string Archivo)
        {
            string nArchivo = "";
            var ftpAddress = ConfigurationManager.AppSettings["FTP"] + "/" + ConfigurationManager.AppSettings["PDFFOLDERANT"] + "/" + Año + "/" + mes + "/" + Tienda;
            FtpWebRequest fwr = (FtpWebRequest)WebRequest.Create(ftpAddress);
            fwr.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FTPUSR"], ConfigurationManager.AppSettings["FTPPASS"]);
            fwr.KeepAlive = false;
            fwr.Method = WebRequestMethods.Ftp.ListDirectory;
            fwr.Proxy = null;
            try
            {
                if ((Archivo.Contains("FGPE")))
                    Archivo = Archivo.Replace("FGPE", "GPE");
                System.IO.StreamReader sr = new System.IO.StreamReader(fwr.GetResponse().GetResponseStream());
                var lst = sr.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string file in lst)
                {

                    var filefor = file.Trim(); // remove any whitespace
                    string cadenaarchivo = Mid(file, 1, 8);
                    if ((!filefor.StartsWith(Archivo)))
                        continue;
                    sr.Close();
                    fwr.Abort();
                    fwr = null;
                    nArchivo = ConfigurationManager.AppSettings["PDFLOCALANT"] + file;
                    var source = ftpAddress + "/" + file;
                    if (System.IO.File.Exists(nArchivo) == false)
                    {
                        using (WebClient wc = new WebClient())
                        {
                            wc.Proxy = null;
                            wc.BaseAddress = ftpAddress;
                            // Authenticate, then download a file to the FTP server.
                            // The same approach also works for HTTP and HTTPS.
                            wc.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FTPUSR"].ToString(), ConfigurationManager.AppSettings["FTPPASS"].ToString());
                            wc.DownloadFile(source, nArchivo);
                        }
                    }

                   //nArchivo = file;
                   break;
                }

                return nArchivo;
            }

            catch (Exception ex)
            {

            }
            return nArchivo;
        }


        public static string Mid(string s, int a, int b)

        {

            string temp = s.Substring(a - 1, b);

            return temp;

        }

        public FileResult Download_FactPDF(string source)
        {
            try
            {
                return File(source, "application/pdf", Path.GetFileName(source));
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public ActionResult GetCFDI(string IdAbono, string Documento, string fechaventa)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;
            string SAPDB = "";
            LastSale LastSale;
            try
            {
                string CONSTRING = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                var INITIALCATALOG = CONSTRING.Split(new char[] { ';', '=' }, StringSplitOptions.RemoveEmptyEntries)[9];
                LastSale = new LastSale()
                {
                    IdAbono = IdAbono,
                    SAPDB = INITIALCATALOG + ".DBO",
                    Documento = Documento
                };
                var DateDocument = Convert.ToDateTime(fechaventa);
                var DateMigration = Convert.ToDateTime("04/08/2021"); // Fecha de migración cfdi's a sap
                int resultDate = DateTime.Compare(DateDocument, DateMigration);
                var relationship = "";
                if (resultDate < 0)
                    relationship = "anterior a";
                else if (resultDate == 0)
                    relationship = "el mismo dia";
                else
                    relationship = "es posterior a";

                if (relationship == "anterior a" || relationship == "el mismo dia")
                    LastSale.relacion = false;
                else
                    LastSale.relacion = true;

                url = "api/GetFacturaPDF";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, LastSale);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<LastSale>().Result;

                if (result != null)
                {
                    string mes = "";
                    string PDF = "";
                    string sYear = "";
                    string sTienda = "";
                    string sFolioInterno = "";
                    string sDia = "";
                    sYear = result.Year;
                    sFolioInterno = result.Foliointerno;
                    if (result.relacion)
                    {
                        sTienda = result.RFC + "/suc-" + result.Sucursal;
                        sDia = result.Dia;
                        sFolioInterno = sFolioInterno + "-" + result.Sufijo;
                        //PDF = Getpdf(sYear, sTienda, mes, sFolioInterno); FTP a otro server ConfigurationManager.AppSettings["PDFFOLDER"].ToString();

                        var directory = "";
                        if (Documento == "Factura")
                        {
                            directory = ConfigurationManager.AppSettings["Factura"].ToString();
                        }
                        else if (Documento == "Complemento")
                        {
                            directory = ConfigurationManager.AppSettings["Complemento"].ToString();
                        }
                        var Source = directory + sYear + "\\" + result.Mes + "\\" + sDia + "\\" + result.Archivo_FE + ".pdf";
                        bool ExisteFact = System.IO.File.Exists(Source);
                        if (ExisteFact == true)
                        {
                            return Json(Source, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("No existe archivo", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        switch (result.Mes)
                        {
                            case "1":
                                result.Mes = "01";
                                break;
                            case "2":
                                result.Mes = "02";
                                break;
                            case "3":
                                result.Mes = "03";
                                break;
                            case "4":
                                result.Mes = "04";
                                break;
                            case "5":
                                result.Mes = "05";
                                break;
                            case "6":
                                result.Mes = "06";
                                break;
                            case "7":
                                result.Mes = "07";
                                break;
                            case "8":
                                result.Mes = "08";
                                break;
                            case "9":
                                result.Mes = "09";
                                break;
                            case "10":
                                result.Mes = "10";
                                break;
                            case "11":
                                result.Mes = "11";
                                break;
                            case "12":
                                result.Mes = "12";
                                break;
                        }
                        sTienda = result.RFC + "/suc-" + result.Sucursal;
                        sFolioInterno = sFolioInterno + "-" + result.Sufijo;
                        PDF = Getpdf(sYear, sTienda, result.Mes, sFolioInterno);
                        bool ExisteFact = System.IO.File.Exists(PDF);
                        if (ExisteFact == true)
                        {
                            return Json(PDF, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("No existe archivo", JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }

            return null;
        }

    }
}