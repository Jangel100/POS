using APIPOSS.Models.ConsultasFacturacion;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIPOSS.Controllers
{
    public class RPTSController : ApiController
    {
        [Route("api2/GetAbonoMontoConsult")]
        [HttpPost]
        public AbonoMontoView GetAbonoMontoConsult(AbonoMontoView abonoMontoView)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdAbono", Value = abonoMontoView.IdAbono }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ClienteSumaTotalAbono", lsParameters, CommandType.StoredProcedure, "EXPEDIDOEN", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new AbonoMontoView
                              {
                                  IdVenta = rows["IdVenta"] is DBNull ? 0 : Convert.ToInt32(rows["IdVenta"]),
                                  MontoTotal = rows["MontoTotal"] is DBNull ? 0 : Convert.ToDecimal(rows["MontoTotal"])
                              }).FirstOrDefault();

                    ls.IdAbono = abonoMontoView.IdAbono;

                    return (ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetPedidoMontoConsult")]
        [HttpPost]
        public PedidoMontoView GetPedidoMontoConsult(AbonoMontoView abonoMontoView)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdAbono", Value = abonoMontoView.IdAbono }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ClienteSumaTotalPedido", lsParameters, CommandType.StoredProcedure, "EXPEDIDOEN", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new PedidoMontoView
                              {
                                  IdVenta  = rows["IdVenta"] is DBNull ? 0 : Convert.ToInt32(rows["IdVenta"]),
                                  MONTOABONOS = rows["MONTOABONOS"] is DBNull ? 0 : Convert.ToDecimal(rows["MONTOABONOS"]),
                                  MONTOVENTA = rows["MONTOVENTA"] is DBNull ? 0 : Convert.ToDecimal(rows["MONTOVENTA"])
                              }).FirstOrDefault();

                    return (ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetClienteSumaTotalPedido")]
        [HttpPost]
        public PedidoMontoView GetClienteSumaTotalPedido(AbonoMontoView abonoMontoView)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdVenta", Value = abonoMontoView.IdAbono }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ClienteSumaTotalVenta", lsParameters, CommandType.StoredProcedure, "EXPEDIDOEN", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new PedidoMontoView
                              {
                                  IdVenta = rows["IdVenta"] is DBNull ? 0 : Convert.ToInt32(rows["IdVenta"]),
                                  MONTOABONOS = rows["MONTOABONOS"] is DBNull ? 0 : Convert.ToDecimal(rows["MONTOABONOS"]),
                                  MONTOVENTA = rows["MONTOVENTA"] is DBNull ? 0 : Convert.ToDecimal(rows["MONTOVENTA"])
                              }).FirstOrDefault();

                    return (ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
