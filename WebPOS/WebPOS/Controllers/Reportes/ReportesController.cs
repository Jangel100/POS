using BL.Interface;
using BL.Reportes;
using CrystalDecisions.CrystalReports.Engine;
using Entities.viewsModels;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebPOS.Security;
using WebPOS.Utilities;
using WebPOS.Views.Reportes;
using System.Data;


namespace WebPOS.Controllers.Reportes
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class ReportesController : Controller
    {
        private string connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        private string connstringSAP;
        readonly IReportesBL _VentasReportesBL;
        public ReportesController(ReportesBL login)
        {
            _VentasReportesBL = login;
        }

        public ReportesController()
        {
            _VentasReportesBL = new ReportesBL();
        }
        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VentasReports()
        {
            return View();
        }
        public ActionResult FacturacionReports()
        {
            return View();
        }
        public ActionResult ClienteAvisaReports()
        {
            return View();
        }
        public ActionResult TotalVentasReports()
        {
            return View();
        }

        public ActionResult VentasxArticuloReports()
        {
            return View();
        }
        public ActionResult IngresosReports()
        {
            return View();
        }
        public ActionResult VTxTiendaReports()
        {
            return View();
        }
        public ActionResult VTxVendedorReports()
        {
            return View();
        }

        public ActionResult TransferenciaReports()
        {
            return View();
        }
        public ActionResult ComprasReports()
        {
            return View();
        }

        public ActionResult KardexReports()
        {
            return View();
        }
        public ActionResult PushMoneyReports()
        {
            return View();
        }
        public ActionResult FactPagosReports()
        {
            return View();
        }
        public ActionResult NominaReports()
        {
            return View();
        }
        public ActionResult GetViewReports(string typeReports)
        {
            IEnumerable<TiendaJsonView> tienda = new List<TiendaJsonView>();
            ReportsFilterKardexView reportsView;
            ReportsView reports;
            try
            {

                if (typeReports.Equals("Kardex"))
                {
                    reportsView = new ReportsFilterKardexView();
                    reportsView = GetFilterKardex();

                    reports = new ReportsView()
                    {
                        reportsFilterKardex = reportsView,
                        TiendaJson = tienda,
                        TypeReports = "Kardex"
                    };
                }
                else
                {
                    tienda = GetStorebyUser();
                    reports = new ReportsView()
                    {
                        TypeReports = typeReports,
                        TiendaJson = tienda
                    };
                }

                return PartialView("../ViewPartial/Reportes/_PartialReports", reports);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<TiendaJsonView> GetStorebyUser()
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetStorebyUserNew";
                TiendaJsonView tiendaView = new TiendaJsonView() { AdminUserID = Session["AdminUserID"].ToString(), Franquicia = Session["FRCARDCODE"].ToString() };

                HttpClient client = _VentasReportesBL.HttpClientBL();
                HttpResponseMessage responses = client.PostAsJsonAsync(url, tiendaView).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<TiendaJsonView>>().Result;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetVendedorByStore(VendedorView vendedor)
        {            
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetVendedorByStore";

                HttpClient client = _VentasReportesBL.HttpClientBL();
                HttpResponseMessage responses = client.PostAsJsonAsync(url, vendedor).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<VendedorView>>().Result;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

            }
            return null;

        }

        //TRAE LA LISTA DE LOS VENDEDORES PARA LLENAR LISTA
        public JsonResult getVendedores()
        {            
             connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            string sQuery;
            DataTable dt = new DataTable();
        retry:
            ;
            sQuery = "SELECT AdminUserID, FirstName+' '+LastName AS Nombre FROM [DORMIMUNDOPOS].[dbo].[AdminUser]  WHERE Status ='A' order by FirstName asc";
            Utilities.DBMaster obj = new Utilities.DBMaster();
            dt = obj.EjecutaQry_Tabla(sQuery.ToString(), CommandType.Text, "AdminUser", connstringWEB);

            var ls = (from DataRow rows in dt.Rows
                      select new VendedorView
                      {
                          AdminUserID = rows["AdminUserID"] is DBNull ? 0 : Convert.ToInt32(rows["AdminUserID"]),
                          Nombre = rows["Nombre"] is DBNull ? "" : (string)rows["Nombre"]
                      }).ToList();
            
            return Json(ls, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetReportsVentas(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetReportsVentas";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<string>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }

                //if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                //{
                //    var result = responses.Content.ReadAsAsync<List<ReportsVentasView>>().Result;

                //    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                //    jsonRes.MaxJsonLength = int.MaxValue;

                //    return jsonRes;
                //}
                //else
                //{
                //    return null;
                //}
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsFacturacion(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetReportsFacturacion";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<ReportsFacturacionView>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsClienteAvisa(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetReportsClienteAvisa";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<ReportsClienteAvisaView>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsTotalVenta(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetReportsTotalVenta";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<ReportsTotalVentaView>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetViewVentaxArticuloReports()
        {
            IEnumerable<TiendaJsonView> tienda = new List<TiendaJsonView>();
            ReportsVentasxArticulo reportsView;
            ReportsView reports;
            try
            {
                reportsView = new ReportsVentasxArticulo();
                reportsView = GetViewVentaxArticulo();

                reports = new ReportsView()
                {
                    reportsVentasxArticulo = reportsView
                };

                return PartialView("../ViewPartial/Reportes/_PartialVentaxArticulo", reports);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ReportsVentasxArticulo GetViewVentaxArticulo()
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            ReportsVentasInputView reports;
            try
            {
                url = "api2/GetDataReportsVentaxArticulo";

                reports = new ReportsVentasInputView();
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.Franquicia = Session["TiendaSesion"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<ReportsVentasxArticulo>().Result;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsVentaxArticulo(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetReportsVentaxArticulo";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<ReportsVentaxArticuloView>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsIngresos(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetReportsIngresos";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<IEnumerable<ReportsIngresosView>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsVTotalxTienda(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetReportsVTotalxTienda";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<string>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsVTotalxVendedor(ReportsVentasInputView reports)
        {
            string url = string.Empty;

            try
            {
                url = "api2/GetReportsVTotalxVendedor";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<string>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsTransferencia(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetReportsTransferencia";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<IEnumerable<ReportsTransferenciasView>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult Download_TrasFerenciasDormimundo_PDF(int IdAbono)
        {
            DBMaster oDB = new DBMaster();
            ReportDocument rd = new ReportDocument();
            try
            {
                var connPDF = ConfigurationManager.AppSettings["connPDF"].ToString();
                var path = Server.MapPath("~/Reports/Reportes/TransferenciaDormimundo.rpt");
                rd.Load(path);

                rd.SetParameterValue(0, IdAbono);


                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                rd.DataSourceConnections[0].SetConnection("10.0.128.110", "DORMIMUNDOPOS", false);
                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString);
                rd.DataSourceConnections[1].SetConnection("10.0.128.110", "DORMIMUNDO_PRODUCTIVA", false);

                rd.DataSourceConnections[0].SetLogon("sa", "SAPB1Admin");
                rd.DataSourceConnections[1].SetLogon("sa", "SAPB1Admin");

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                return File(stream, "application/pdf", "_PDF_Report_" + IdAbono + ".pdf");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public ActionResult GetReportsCompras(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetReportsCompras";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<IEnumerable<ReportsComprasView>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsKardex(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetReportsKardex";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<IEnumerable<ReportsKardexView>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ReportsFilterKardexView GetFilterKardex()
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            ReportsVentasInputView reports;
            try
            {
                url = "api2/GetFilterKardex";

                reports = new ReportsVentasInputView();
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.Franquicia = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                //client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<ReportsFilterKardexView>().Result;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetFranquiciasPushMoney(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetFranquiciasPushMoney";

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<IEnumerable<AdminFranquiciasView>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetVendedorPushMoney(ReportsVentasInputView usersView)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetVendedorPushMoney";

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, usersView).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<IEnumerable<UsersView>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsPushMoney(ReportsVentasInputView usersView)
        {
            string url = string.Empty;

            try
            {
                url = "api2/GetReportsPushMoney";

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, usersView).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<IEnumerable<ReportsPushMoneyVendedorView>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsPushMoneyxVendedor(ReportsVentasInputView usersView)
        {
            string url = string.Empty;

            try
            {
                url = "api2/GetReportsPushMoneyxVendedor";

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, usersView).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<IEnumerable<ReportsPushMoneyVendedor>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsPushMoneyxGralFranquicia(ReportsVentasInputView usersView)
        {
            string url = string.Empty;

            try
            {
                url = "api2/GetReportsPushMoneyxGralFranquicia";

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, usersView).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<IEnumerable<ReportsPushMoneyGralFranquicia>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsFactPagos(ReportsVentasInputView reports)
        {
            string url = string.Empty;

            try
            {
                url = "api2/GetReportsFactPagos";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<IEnumerable<ReportsFactPagosView>>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public List<TiendaJsonView> GetDataStoreWhsID()
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetDataStoreWhsID";
                TiendaJsonView tiendaView = new TiendaJsonView() { AdminUserID = Session["AdminUserID"].ToString(), Franquicia = Session["FRCARDCODE"].ToString() };

                HttpClient client = _VentasReportesBL.HttpClientBL();
                HttpResponseMessage responses = client.PostAsJsonAsync(url, tiendaView).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<TiendaJsonView>>().Result;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetReportsNomina(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetReportsNomina";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<string>().Result;

                    var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
                    jsonRes.MaxJsonLength = int.MaxValue;

                    return jsonRes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
        public ActionResult GetFiltersPayback()
        {
            IEnumerable<TiendaJsonView> tienda = new List<TiendaJsonView>();
            ReportsFilterKardexView reportsView;
            ReportsView reports;
            try
            {

                tienda = GetStorebyUser();
                reports = new ReportsView()
                {
                    TypeReports = "Reporte Payback",
                    TiendaJson = tienda
                };


                return View("ReportPayback", reports);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult GetFiltersDesc()
        {
            IEnumerable<TiendaJsonView> tienda = new List<TiendaJsonView>();
            ReportsFilterKardexView reportsView;
            ReportsView reports;
            try
            {

                tienda = GetStorebyUser();
                reports = new ReportsView()
                {
                    TypeReports = "Reporte Descuento",
                    TiendaJson = tienda
                };


                return View("ReportAprobacionDescView", reports);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult GetReportsPayback(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api3/GetReportsFacturacionPayback";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<ReportsFacturacionPayback>>().Result;

                    var jsonRes = JsonConvert.SerializeObject(result);

                    return Json(jsonRes);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }
        public ActionResult GetReportsDesc(ReportsVentasInputView reports)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api4/GetReportsDesc";
                reports.AdminUserID = Session["AdminUserID"].ToString();
                reports.FRCARDCODE = Session["FRCARDCODE"].ToString();

                HttpClient client = _VentasReportesBL.HttpClientBL();
                client.Timeout = TimeSpan.FromMinutes(10);
                HttpResponseMessage responses = client.PostAsJsonAsync(url, reports).Result;

                if (responses.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var result = responses.Content.ReadAsAsync<List<ReportsFacturacionPayback>>().Result;

                    var jsonRes = JsonConvert.SerializeObject(result);

                    return Json(jsonRes);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }
        public ActionResult ReceipExcel(HttpPostedFileBase FileUpload)
        {
            var Response = new ResponsePayback();
            try
            {
                string squery = string.Empty;
                string sError = string.Empty;
                DBMaster oDB = new DBMaster();
                List<InfoArchivoBuzon> InfoArchivoBuzon = new List<InfoArchivoBuzon>();
                List<InfoArchivoBuzon> InfoArchivoExist = new List<InfoArchivoBuzon>();
                var document = FileUpload;
                Stream documentConverted = document.InputStream;
                IWorkbook MiExcel = new XSSFWorkbook(documentConverted);
                ISheet hoja = MiExcel.GetSheetAt(0);
                var nameHoja = hoja.GetRow(0).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue;
                if (!nameHoja.Equals("Reporte de  Archivos Payback")) {
                    Response.Message = "El documento seleccionado no es un documento válido, favor de revisar e intentar de nuevo";
                    return Json(JsonConvert.SerializeObject(Response)); 
                }
                if (hoja != null)
                {
                    foreach (IRow file in hoja)
                    {
                        if (file.RowNum > 2)
                        {
                            var row = new InfoArchivoBuzon();
                            row.Fechadetransaccion = file.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue != null ? file.GetCell(2, MissingCellPolicy.RETURN_NULL_AND_BLANK).StringCellValue : "";
                            row.FechaConciliacion = DateTime.Now;
                            row.Cupon = file.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue != null ? file.GetCell(11, MissingCellPolicy.RETURN_NULL_AND_BLANK).StringCellValue : "";
                            row.TipoDoc = file.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue != null ? file.GetCell(0, MissingCellPolicy.RETURN_NULL_AND_BLANK).StringCellValue : "";
                            row.Recibo = file.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue != null ? file.GetCell(3, MissingCellPolicy.RETURN_NULL_AND_BLANK).StringCellValue : "";
                            row.NumIntPayback = file.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue != null ? file.GetCell(5, MissingCellPolicy.RETURN_NULL_AND_BLANK).StringCellValue : "";
                            row.NumeroPayback = file.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue != null ? file.GetCell(4, MissingCellPolicy.RETURN_NULL_AND_BLANK).StringCellValue : "";
                            row.NombreArchivo = FileUpload.FileName;
                            row.NumeroTarjeta = file.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue != null ? file.GetCell(6, MissingCellPolicy.RETURN_NULL_AND_BLANK).StringCellValue : "";
                            row.Socio = file.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue != null ? file.GetCell(9, MissingCellPolicy.RETURN_NULL_AND_BLANK).StringCellValue : "";
                            row.Sucursal = file.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue != null ? file.GetCell(10, MissingCellPolicy.RETURN_NULL_AND_BLANK).StringCellValue : "";
                            row.Moneda = file.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).StringCellValue != null ? file.GetCell(7, MissingCellPolicy.RETURN_NULL_AND_BLANK).StringCellValue : "";
                            row.TotalCompra = file.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).NumericCellValue;
                            row.PuntosOtorgados = file.GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).NumericCellValue;
                            row.Total = file.GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).NumericCellValue;
                            InfoArchivoBuzon.Add(row);
                        }
                    }
                }
                else
                {

                }
                if (InfoArchivoBuzon.Count > 0)
                {
                    Response.Total = InfoArchivoBuzon.Count;
                    foreach (var row in InfoArchivoBuzon)
                    {
                        try
                        {
                            squery = $"select idConciliacion from Dat_ConciliacionPayback where Recibo = '{row.Recibo}'";
                            var DtInfo = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "InfoPayback", connstringWEB);
                            if (DtInfo.Rows.Count <= 0)
                            {
                                squery = $"UPDATE Dat_TransactionPayback set Conciliado = 1 WHERE ReceiptNumber = '{row.Recibo}'" + Environment.NewLine;
                                squery += $"Insert Into Dat_ConciliacionPayback (TipoDoc,Fecha,FechaConciliacion,Recibo,NumeroPayback,NumeroInterno,NumeroTarjeta,Moneda,TotalCompra,Socio,Sucursal,Cupon,PuntosOtorgados,Total,NombreArchivo) VALUES('{row.TipoDoc}','{Convert.ToDateTime(row.Fechadetransaccion).ToString("yyyy-MM-dd HH:mm:ss")}','{Convert.ToDateTime(row.FechaConciliacion).ToString("yyyy-MM-dd HH:mm:ss")}','{row.Recibo}','{row.NumeroPayback}','{row.NumIntPayback}','{row.NumeroTarjeta}','{row.Moneda}',{row.TotalCompra},'{row.Socio}','{row.Sucursal}','{row.Cupon}',{row.PuntosOtorgados},{row.Total},'{row.NombreArchivo}')";
                                oDB.EjecutaQry(squery, CommandType.Text, connstringWEB, sError);
                            }
                            else
                            {
                                squery = $"UPDATE Dat_TransactionPayback set Conciliado = 1 WHERE ReceiptNumber = '{row.Recibo}'" + Environment.NewLine;
                                oDB.EjecutaQry(squery, CommandType.Text, connstringWEB, sError);
                            }
                            Response.Conciliados += 1;
                        }
                        catch (Exception ex)
                        {
                            Response.Erroneos += 1;
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {               
                //return;
            }
            Response.Succes = true;
            Response.Message = "¡El Documento se ha cargado correctamente!";
            return Json(JsonConvert.SerializeObject(Response));
        }
    }
}