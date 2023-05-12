using BL.Configuracion;
using BL.Interface;
using CrystalDecisions.CrystalReports.Engine;
using Entities.viewsModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebPOS.Security;
using WebPOS.Utilities;

namespace WebPOS.Controllers.Ventas
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class VentasRPTSNewController : Controller
    {
        readonly IVentasBL _VentasBL;
        // GET: Ventas
        public VentasRPTSNewController(IVentasBL ventasBL)
        {
            _VentasBL = ventasBL;
        }
        public VentasRPTSNewController()
        {
            _VentasBL = new VentasBL();
        }

        public ActionResult Download_AbonoVentaDormimundo_PDF(string IdAbono)
        {
            DBMaster oDB = new DBMaster();
            ReportDocument rd = new ReportDocument();
            AbonoMontoView abonoMontoView;
            try
            {
                var connPDF = ConfigurationManager.AppSettings["IP_BD_Server_"].ToString();
                var path = Server.MapPath("~/Reports/Abono/AbonoDormimundo.rpt");
                rd.Load(path);

                abonoMontoView = new AbonoMontoView() { IdAbono = Convert.ToInt32(IdAbono) };
                abonoMontoView = GetAbonoMontoConsult(abonoMontoView);

                rd.SetParameterValue(0, abonoMontoView.IdVenta);
                rd.SetParameterValue(1, abonoMontoView.IdAbono);
                rd.SetParameterValue(2, abonoMontoView.MontoTotal);

                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString);
                rd.DataSourceConnections[1].SetConnection("10.0.128.110", "DORMIMUNDO_PRODUCTIVA", false);
                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                rd.DataSourceConnections[0].SetConnection(connPDF, "DORMIMUNDOPOS", false);

                rd.DataSourceConnections[0].SetLogon("sa", "Dormimundo2015");
                rd.DataSourceConnections[1].SetLogon("sa", "Dormimundo2015");

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
              
                //using (MemoryStream memoryStream = new MemoryStream())
                //{

                //    Response.ClearHeaders();
                //    Response.ClearContent();
                //    Response.Charset = "";
                //    Response.AddHeader("Content-Type", "application/pdf");
                //    Response.Flush();
                //    Response.Close();
                //    Response.End();
                //}
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
        public ActionResult prueba()
        {
            return null;
        }
    }
}