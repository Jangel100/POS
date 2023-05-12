using BL.Compras;
using BL.Interface;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPOS.Security;
using WebPOS.Utilities;

namespace WebPOS.Controllers.Compras
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class ComprasRPTSController : Controller
    {
        private string connstringSAP;
        private string connstringWEB;
        readonly IComprasBL _ComprasBL;
        // GET: ComprasRPTS
        public ComprasRPTSController(IComprasBL comprasBL)
        {
            _ComprasBL = comprasBL;
        }
        public ComprasRPTSController()
        {
            _ComprasBL = new ComprasBL();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Download_CompraDormimundo_PDF(int IDCOMPRA)
        {
            string sQuery = "";
            DataTable dtC = new DataTable();
            DBMaster oDB = new DBMaster();
            double MONTOTOTAL = 0;
            try
            {
                var connPDF = ConfigurationManager.AppSettings["connPDF"].ToString();
                connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                var nameBDPOS = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
                var nameBDFDO = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
                var PasswordSQL = ConfigurationManager.AppSettings["PasswordSQL"].ToString();
                var UserSQL = ConfigurationManager.AppSettings["UserSQL"].ToString();
                ReportDocument report = new ReportDocument();
                var path = Server.MapPath("~/Reports/Compras/ComprasDormimundo.rpt");
                report.Load(path);
                report.SetParameterValue(0, IDCOMPRA);

                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                report.DataSourceConnections[0].SetConnection(connPDF, nameBDPOS, false);
                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString);
                report.DataSourceConnections[1].SetConnection(connPDF, nameBDFDO, false);

                report.DataSourceConnections[0].SetLogon(UserSQL, PasswordSQL);
                report.DataSourceConnections[1].SetLogon(UserSQL, PasswordSQL);

                Stream stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);


                return File(stream, "application/pdf", "CompraDormimundo_" + IDCOMPRA + ".pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }


    }
}