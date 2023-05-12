using BL.Configuracion;
using BL.Interface;
using CrystalDecisions.CrystalReports.Engine;
using Entities.viewsModels;
using System;
using System.Configuration;
using System.IO;
using System.Data;
using System.Net.Http;
using System.Web.Mvc;
using WebPOS.Security;
using WebPOS.Utilities;

namespace WebPOS.Controllers.Ventas
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class VentasRPTSController : Controller
    {
        readonly IVentasBL _VentasBL;
        // GET: Ventas
        public VentasRPTSController(IVentasBL ventasBL)
        {
            _VentasBL = ventasBL;
        }
        public VentasRPTSController()
        {
            _VentasBL = new VentasBL();
        }

        public ActionResult ReporteVentas(int IDVENTA)
        {
            try
            {
                return GetReportVenta(IDVENTA);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        //public string GetNameRPT(string TipoReporte)
        //{
        //    string result = "";
        //    try
        //    {
        //        switch (TipoReporte)
        //        {
        //            case "Faxturacion":
        //                result = "VentaDormimundo.rpt";
        //                break;
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        public ActionResult GetReportVenta(int IDVENTA)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            VentaDormimundoView ventaDormimundoView;

            try
            {
                ventaDormimundoView = new VentaDormimundoView() { idventa = IDVENTA };
                url = "api2/GetReportVenta";

                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, ventaDormimundoView);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<Stream>().Result;

                return File(result, "application/pdf");
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public ActionResult Download_VentaDormimundo_PDF(string IDVENTA)
        {
            string url;
            AbonoMontoView abonoMontoView;
            PedidoMontoView pedidoMontoView;
            Decimal MONTOABONOS, MONTOVENTA, SALDO;
            DBMaster oDB = new DBMaster();
            DataTable dt = new DataTable();
            var payback = new PaybackController();
            var connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            String LogWeb = AppDomain.CurrentDomain.BaseDirectory + @"logErrorPayback\LogWeb_" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace("T", "_").Replace("-", "_").Substring(0, 10) + ".txt";
            payback.anade_linea_archivo2(LogWeb, "descarga pdf idventa: " + IDVENTA);
            try
            {
                var connPDF = ConfigurationManager.AppSettings["connPDF"].ToString();
                var nameBDPOS = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
                var nameBDFDO = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
                var PasswordSQL = ConfigurationManager.AppSettings["PasswordSQL"].ToString();
                var UserSQL = ConfigurationManager.AppSettings["UserSQL"].ToString();
                ReportDocument rd = new ReportDocument();

                var path = Server.MapPath("~/Reports/Venta/VentaDormimundo.rpt");

                rd.Load(path);

                abonoMontoView = new AbonoMontoView() { IdAbono = Convert.ToInt32(IDVENTA) };
                pedidoMontoView = GetClienteSumaTotalPedido(abonoMontoView);

                MONTOABONOS = pedidoMontoView.MONTOABONOS;
                MONTOVENTA = pedidoMontoView.MONTOVENTA;
                SALDO = MONTOVENTA - MONTOABONOS;

                rd.SetParameterValue(0, IDVENTA);
                rd.SetParameterValue(1, SALDO);
                rd.SetParameterValue(2, MONTOABONOS);
                rd.SetParameterValue("@Idventa", IDVENTA);


                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString);
                rd.DataSourceConnections[0].SetConnection(connPDF, nameBDFDO, false);
                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                rd.DataSourceConnections[1].SetConnection(connPDF, nameBDPOS, false);

                rd.DataSourceConnections[0].SetLogon(UserSQL, PasswordSQL);
                rd.DataSourceConnections[1].SetLogon(UserSQL, PasswordSQL);

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();

                return File(stream, "application/pdf", "VentaDormimundo_" + IDVENTA + ".pdf");
            }
            catch (Exception ex)
            {
                var error = "";
                error = ex.Message;
                // log
                payback.anade_linea_archivo2(LogWeb, "Error al descargar pdf idventa: " + IDVENTA + "ERROR: "+ error);
                throw;
            }

        }
        public ActionResult Download_VentaDormimundoFranquicias_PDF(string IDVENTA)
        {
            string url;
            AbonoMontoView abonoMontoView;
            PedidoMontoView pedidoMontoView;
            Decimal MONTOABONOS, MONTOVENTA, SALDO;
            DBMaster oDB = new DBMaster();
            DataTable dt = new DataTable();
            var payback = new PaybackController();
            var connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            String LogWeb = AppDomain.CurrentDomain.BaseDirectory + @"logErrorPayback\LogWeb_" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace("T", "_").Replace("-", "_").Substring(0, 10) + ".txt";
            payback.anade_linea_archivo2(LogWeb, "descarga pdf franquicias idventa: " + IDVENTA);
            try
            {
                var connPDF = ConfigurationManager.AppSettings["connPDF"].ToString();
                var nameBDPOS = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
                var nameBDFDO = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
                var PasswordSQL = ConfigurationManager.AppSettings["PasswordSQL"].ToString();
                var UserSQL = ConfigurationManager.AppSettings["UserSQL"].ToString();
                ReportDocument rd = new ReportDocument();

                var path = Server.MapPath("~/Reports/Venta/VentaDormimundoOriginal.rpt");

                rd.Load(path);

                abonoMontoView = new AbonoMontoView() { IdAbono = Convert.ToInt32(IDVENTA) };
                pedidoMontoView = GetClienteSumaTotalPedido(abonoMontoView);

                MONTOABONOS = pedidoMontoView.MONTOABONOS;
                MONTOVENTA = pedidoMontoView.MONTOVENTA;
                SALDO = MONTOVENTA - MONTOABONOS;

                rd.SetParameterValue(0, IDVENTA);
                rd.SetParameterValue(1, SALDO);
                rd.SetParameterValue(2, MONTOABONOS);
                rd.SetParameterValue("@Idventa", IDVENTA);


                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString);
                rd.DataSourceConnections[0].SetConnection(connPDF, nameBDFDO, false);
                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                rd.DataSourceConnections[1].SetConnection(connPDF, nameBDPOS, false);

                rd.DataSourceConnections[0].SetLogon(UserSQL, PasswordSQL);
                rd.DataSourceConnections[1].SetLogon(UserSQL, PasswordSQL);

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();

                return File(stream, "application/pdf", "VentaDormimundo_" + IDVENTA + ".pdf");
            }
            catch (Exception ex)
            {
                var error = "";
                error = ex.Message;
                // log
                payback.anade_linea_archivo2(LogWeb, "Error al descargar pdf idventa: " + IDVENTA + "ERROR: " + error);
                throw;
            }

        }
        public ActionResult Download_AbonoVentaDormimundo_PDF(int IdAbono)
        {
            DBMaster oDB = new DBMaster();
            ReportDocument rd = new ReportDocument();
            AbonoMontoView abonoMontoView;
            try
            {
                var connPDF = ConfigurationManager.AppSettings["connPDF"].ToString();
                var nameBDPOS = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
                var nameBDFDO = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
                var PasswordSQL = ConfigurationManager.AppSettings["PasswordSQL"].ToString();
                var UserSQL = ConfigurationManager.AppSettings["UserSQL"].ToString();
                var path = Server.MapPath("~/Reports/Abono/AbonoDormimundo.rpt");
                rd.Load(path);

                abonoMontoView = new AbonoMontoView() { IdAbono = IdAbono };
                abonoMontoView = GetAbonoMontoConsult(abonoMontoView);

                rd.SetParameterValue(0, abonoMontoView.IdVenta);
                rd.SetParameterValue(1, abonoMontoView.IdAbono);
                rd.SetParameterValue(2, abonoMontoView.MontoTotal);

                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString);
                rd.DataSourceConnections[1].SetConnection(connPDF, nameBDFDO, false);
                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                rd.DataSourceConnections[0].SetConnection(connPDF, nameBDPOS, false);

                rd.DataSourceConnections[0].SetLogon(UserSQL, PasswordSQL);
                rd.DataSourceConnections[1].SetLogon(UserSQL, PasswordSQL);

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                return File(stream, "application/pdf", "_PDF_Abono" + abonoMontoView.IdAbono + ".pdf");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public AbonoMontoView GetAbonoMontoConsult(AbonoMontoView abonoMontoView)
        {
            string url = string.Empty;
            try
            {
                url = "api2/GetAbonoMontoConsult";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, abonoMontoView);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<AbonoMontoView>().Result;

                if (result != null)
                {
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public PedidoMontoView GetClienteSumaTotalPedido(AbonoMontoView abonoMontoView)
        {
            string url = string.Empty;
            try
            {
                url = "api2/GetClienteSumaTotalPedido";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, abonoMontoView);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<PedidoMontoView>().Result;

                if (result != null)
                {
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public ActionResult Download_PedidoVentaDormimundo_PDF(string IDVENTA)
        {
            DBMaster oDB = new DBMaster();
            ReportDocument rd = new ReportDocument();
            AbonoMontoView abonoMontoView;
            PedidoMontoView pedidoMontoView;
            Decimal MONTOABONOS, MONTOVENTA, SALDO;
            var payback = new PaybackController();
            var connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            String LogWeb = AppDomain.CurrentDomain.BaseDirectory + @"logErrorPayback\LogWeb_" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace("T", "_").Replace("-", "_").Substring(0, 10) + ".txt";
            payback.anade_linea_archivo2(LogWeb, "descarga pdf idventa: " + IDVENTA);
            try
            {
                var connPDF = ConfigurationManager.AppSettings["connPDF"].ToString();
                var nameBDPOS = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
                var nameBDFDO = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
                var PasswordSQL = ConfigurationManager.AppSettings["PasswordSQL"].ToString();
                var UserSQL = ConfigurationManager.AppSettings["UserSQL"].ToString();
                var path = Server.MapPath("~/Reports/Venta/VentaDormimundo.rpt");
                rd.Load(path);

                abonoMontoView = new AbonoMontoView() { IdAbono = Convert.ToInt32(IDVENTA) };
                pedidoMontoView = GetPedidoMontoConsult(abonoMontoView);

                MONTOABONOS = pedidoMontoView.MONTOABONOS;
                MONTOVENTA = pedidoMontoView.MONTOVENTA;
                SALDO = MONTOVENTA - MONTOABONOS;

                rd.SetParameterValue(0, pedidoMontoView.IdVenta);
                rd.SetParameterValue(1, SALDO);
                rd.SetParameterValue(2, MONTOABONOS);
                rd.SetParameterValue("@idventa", pedidoMontoView.IdVenta);

                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString);
                rd.DataSourceConnections[0].SetConnection(connPDF, nameBDFDO, false);
                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                rd.DataSourceConnections[1].SetConnection(connPDF, nameBDPOS, false);

                rd.DataSourceConnections[0].SetLogon(UserSQL, PasswordSQL);
                rd.DataSourceConnections[1].SetLogon(UserSQL, PasswordSQL);

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                return File(stream, "application/pdf", "_PDF_Report_" + pedidoMontoView.IdVenta + ".pdf");
            }
            catch (Exception ex)
            {
                var error = "";
                error = ex.Message;
                // log
                payback.anade_linea_archivo2(LogWeb, "Error al descargar pdf idventa consultas: " + IDVENTA + "ERROR: " + error);
                throw;
            }
        }
        public ActionResult Download_PedidoVentaDormimundoFranquicias_PDF(string IDVENTA)
        {
            DBMaster oDB = new DBMaster();
            ReportDocument rd = new ReportDocument();
            AbonoMontoView abonoMontoView;
            PedidoMontoView pedidoMontoView;
            Decimal MONTOABONOS, MONTOVENTA, SALDO;
            var payback = new PaybackController();
            var connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            String LogWeb = AppDomain.CurrentDomain.BaseDirectory + @"logErrorPayback\LogWeb_" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace("T", "_").Replace("-", "_").Substring(0, 10) + ".txt";
            payback.anade_linea_archivo2(LogWeb, "descarga pdf idventa: " + IDVENTA);
            try
            {
                var connPDF = ConfigurationManager.AppSettings["connPDF"].ToString();
                var nameBDPOS = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
                var nameBDFDO = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
                var PasswordSQL = ConfigurationManager.AppSettings["PasswordSQL"].ToString();
                var UserSQL = ConfigurationManager.AppSettings["UserSQL"].ToString();
                var path = Server.MapPath("~/Reports/Venta/VentaDormimundoOriginal.rpt");
                rd.Load(path);

                abonoMontoView = new AbonoMontoView() { IdAbono = Convert.ToInt32(IDVENTA) };
                pedidoMontoView = GetPedidoMontoConsult(abonoMontoView);

                MONTOABONOS = pedidoMontoView.MONTOABONOS;
                MONTOVENTA = pedidoMontoView.MONTOVENTA;
                SALDO = MONTOVENTA - MONTOABONOS;

                rd.SetParameterValue(0, pedidoMontoView.IdVenta);
                rd.SetParameterValue(1, SALDO);
                rd.SetParameterValue(2, MONTOABONOS);
                rd.SetParameterValue("@idventa", pedidoMontoView.IdVenta);

                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString);
                rd.DataSourceConnections[0].SetConnection(connPDF, nameBDFDO, false);
                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                rd.DataSourceConnections[1].SetConnection(connPDF, nameBDPOS, false);

                rd.DataSourceConnections[0].SetLogon(UserSQL, PasswordSQL);
                rd.DataSourceConnections[1].SetLogon(UserSQL, PasswordSQL);

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                return File(stream, "application/pdf", "_PDF_Report_" + pedidoMontoView.IdVenta + ".pdf");
            }
            catch (Exception ex)
            {
                var error = "";
                error = ex.Message;
                // log
                payback.anade_linea_archivo2(LogWeb, "Error al descargar pdf idventa consultas: " + IDVENTA + "ERROR: " + error);
                throw;
            }
        }
        public PedidoMontoView GetPedidoMontoConsult(AbonoMontoView abonoMontoView)
        {
            string url = string.Empty;
            try
            {
                url = "api2/GetPedidoMontoConsult";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, abonoMontoView);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<PedidoMontoView>().Result;

                if (result != null)
                {
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public ActionResult Download_Cancelacion_PDF(int IdDevolucion)
        {
            DBMaster oDB = new DBMaster();
            try
            {
                var connPDF = ConfigurationManager.AppSettings["connPDF"].ToString();
                var nameBDPOS = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
                var nameBDFDO = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
                var PasswordSQL = ConfigurationManager.AppSettings["PasswordSQL"].ToString();
                var UserSQL = ConfigurationManager.AppSettings["UserSQL"].ToString();
                ReportDocument rd = new ReportDocument();

                var path = Server.MapPath("~/Reports/CancelacionInterna/CancelaVentaDormimundo.rpt");

                rd.Load(path);

                rd.SetParameterValue(0, IdDevolucion);

                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString);
                rd.DataSourceConnections[1].SetConnection(connPDF, nameBDFDO, false);

                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                rd.DataSourceConnections[0].SetConnection(connPDF, nameBDPOS, false);

                rd.DataSourceConnections[0].SetLogon(UserSQL, PasswordSQL);
                rd.DataSourceConnections[1].SetLogon(UserSQL, PasswordSQL);

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                rd.Dispose();
                return File(stream, "application/pdf", "Report_" + IdDevolucion + ".pdf");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}