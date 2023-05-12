using BL.Abonos;
using BL.Interface;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.Mvc;
using WebPOS.Security;
using WebPOS.Utilities;

namespace WebPOS.Controllers.Abonos
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class AbonosRPTSController : Controller
    {
        private string connstringSAP;
        private string connstringWEB;
        readonly IAbonosBL _AbonosBL;
        // GET: AbonosRPTS
        public AbonosRPTSController(IAbonosBL abonosBL)
        {
            _AbonosBL = abonosBL;
        }
        public AbonosRPTSController()
        {
            _AbonosBL = new AbonosBL();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Download_Abono_PDF(int IDABONO)
        {
            string sQuery = "";
            DBMaster oDB = new DBMaster();
            DataTable dtC = new DataTable();
            int IDVENTA;
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
                sQuery = "" + "SELECT     SUM(Monto) as MONTOTOTAL, IDVenta" + Environment.NewLine + "FROM         VentasPagos" + Environment.NewLine + "WHERE     (IDVenta = (SELECT TOP 1 IDVENTA FROM VENTASPAGOS WHERE ID = " + IDABONO + "))" + Environment.NewLine + "and ID <= " + IDABONO + " " + Environment.NewLine + "GROUP BY IDVenta" + Environment.NewLine + "";
                dtC = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "EXPEDIDOEN", connstringWEB);
                IDVENTA = Convert.ToInt32(dtC.Rows[0]["IDVENTA"]);
                MONTOTOTAL = Convert.ToDouble(dtC.Rows[0]["MONTOTOTAL"]);

                ReportDocument rd = new ReportDocument();

                var path = Server.MapPath("~/Reports/Abono/AbonoDormimundo.rpt");

                rd.Load(path);


                rd.SetParameterValue(0, IDVENTA);
                rd.SetParameterValue(1, IDABONO);
                rd.SetParameterValue(2, MONTOTOTAL);

                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
                rd.DataSourceConnections[0].SetConnection(connPDF, nameBDPOS, false);
                oDB.ConectaDBConnString(ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString);
                rd.DataSourceConnections[1].SetConnection(connPDF, nameBDFDO, false);

                rd.DataSourceConnections[0].SetLogon(UserSQL, PasswordSQL);
                rd.DataSourceConnections[1].SetLogon(UserSQL, PasswordSQL);

                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                rd.Close();
                rd.Dispose();
                return File(stream, "application/pdf", "AbonoDormimundo_" + IDVENTA + ".pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}