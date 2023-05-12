using APIPOSS.Models.Configuracion;
using APIPOSS.Models.ConsultasFacturacion;
using APIPOSS.Models.Ventas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace APIPOSS.Controllers
{
    public class VentasConsultasFranquiciasController : ApiController
    {
        private string _NameBDPos = ConfigurationManager.AppSettings["nameBDPOS"].ToString();
        private string _NameBDSap = ConfigurationManager.AppSettings["nameBDFDO"].ToString();
        [Route("api/GetInfoClienteConsultasF")]
        [HttpPost]
        public JsonResult<List<Clientes>> GetInfoClienteF(Clientes clientes)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@idStore", Value = clientes.IdStore },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Nombre", Value = clientes.Nombre }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ClienteInfoConsultas", lsParameters, CommandType.StoredProcedure, "Clientes", connstringWEB);

                if (dt != null)


                {

                    var ls = (from DataRow rows in dt.Rows
                              select new Clientes
                              {
                                  Id = (Convert.ToInt32(rows["Id"]).ToString()),
                                  Nombre = (string)rows["Nombre"],
                                  RFC = (string)rows["RFC"]
                              }).ToList();

                    return Json<List<Clientes>>(ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [Route("api/GetPeriodoClienteConsultasF")]
        [HttpPost]
        public List<PeriodoView> GetPeriodoClienteConsultasF(Clientes clientes)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>()
                {
                    new System.Data.SqlClient.SqlParameter(){ ParameterName = "@idCliente", Value = clientes.Id }
                };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ClienteInfoPeridoConsultas", lsParameters, CommandType.StoredProcedure, "Periodos", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new PeriodoView
                              {
                                  Periodo = (string)rows["Periodo"]
                              }).ToList();

                    return (ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api/GetDiaClienteConsultasF")]
        [HttpPost]
        public List<DiaView> GetDiaClienteConsultasF(PeriodoView Periodo)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@idStore", Value = Periodo.idStore },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@idCliente", Value = Periodo.idCliente },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Periodo", Value = Periodo.Periodo }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ClienteInfoDiaConsultas", lsParameters, CommandType.StoredProcedure, "Dias", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new DiaView
                              {
                                  Dia = (string)rows["dia"]
                              }).ToList();

                    return (ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api/GetFolioClienteConsultasF")]
        [HttpPost]
        public List<FolioView> GetFolioClienteConsultasF(PeriodoView Periodo)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@idStore", Value = Periodo.idStore },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@idCliente", Value = Periodo.idCliente },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Periodo", Value = Periodo.Periodo },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Dia", Value = Periodo.Dia }
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ClienteInfoFolioConsultas", lsParameters, CommandType.StoredProcedure, "Folios", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new FolioView
                              {
                                  FolioPref = (string)rows["FolioPref"]
                              }).ToList();

                    return (ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api/GetClientePedidoConsultF")]
        [HttpPost]
        public List<PedidosView> GetClientePedidoConsultF(PedidoParameterIntoView pedido)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@TipoFactura", Value = pedido.TipoFactura },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdStore", Value = pedido.IdStore },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Boton", Value = pedido.Boton },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdCliente", Value = pedido.Boton == 0 ? pedido.IdCliente: 0 },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Fecha", Value = pedido.Fecha },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Prefijo", Value = pedido.Prefijo },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@Folio", Value = pedido.Boton == 0 ? pedido.Folio: pedido.Pedido },
                    };

                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("ClientePedidoConsultFranquciatarios", lsParameters, CommandType.StoredProcedure, "Devoluciones", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new PedidosView
                              {
                                  id = rows["id"] is DBNull ? 0 : Convert.ToInt32(rows["id"]),
                                  venta = rows["venta"] is DBNull ? "" : (string)rows["venta"],
                                  abono = rows["abono"] is DBNull ? "" : (string)rows["abono"],
                                  fechareciboOutput = rows["fecharecibo"] is DBNull ? "" : Convert.ToString((DateTime)rows["fecharecibo"]),
                                  fechaventaOutput = rows["fechaventa"] is DBNull ? "" : Convert.ToString((DateTime)rows["fechaventa"]),
                                  motopagado = rows["montopagado"] is DBNull ? 0 : (Decimal)rows["montopagado"],
                                  Reimprimir = rows["Reimprimir"] is DBNull ? "" : (string)rows["Reimprimir"],
                                  //Restante = rows["Restante"] is DBNull ? 0 : (Decimal)rows["Restante"],
                                  Confirmacion = (Convert.ToDecimal(rows["Restante"]) <= 0) ? true : false,
                              }).ToList();

                    return (ls);
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [Route("api/GetConfirmaEntregaF")]
        public string GetConfirmaEntregaF(string JsonParametrs)
        {
            DataTable dt;
            string connstringWEB;
            string squery = string.Empty;
            Utilities.DBMaster obj = new Utilities.DBMaster();
            string sError = string.Empty;
            string idVenta = string.Empty;
            string IdAbono = JsonParametrs;
            string ResponseC = string.Empty;
            try
            {
                //dynamic data = JObject.Parse(JsonParametrs);
                //string IdAbono = data.IdAbono;
                AddSale ResponseVenta = new AddSale();
                squery = "select IDVenta,(select 'FACTURA_' + Prefijo + RIGHT('0000' + convert(varchar,Folio),5) + '_' + Prefijo from VentasEncabezado where ID= VentasPagos.IDVenta) from VentasPagos where ID=" + IdAbono;
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                dt = obj.EjecutaQry_Tabla(squery, CommandType.Text, "LastSale", connstringWEB);
                if (dt.Rows.Count == 0) 
                {
                    //nothing
                    ResponseC = "false";
                    return ResponseC;
                }
                else
                {
                   idVenta = dt.Rows[0][0].ToString();
                }
                squery = "update ventasencabezado set FechaCFDI=getdate(),Facturado=1,Archivo_FE='" + ConfigurationManager.AppSettings["DirectorioFacturasTXT"] + dt.Rows[0][1] + "' where id=" + idVenta;
                obj.EjecutaQry(squery, CommandType.Text, connstringWEB, sError);
                squery = "insert into Inventarios (IDArticulo,Cantidad,Almacen)  select  VD.IDArticulo ,0 ,VD.IDStore  from VentasDetalle VD where (select COUNT(*) from Inventarios I where I.Almacen=vd.IDStore and i.IDArticulo=vd.IDArticulo)=0 and IDVenta=" + idVenta;
                obj.EjecutaQry(squery, CommandType.Text, connstringWEB, sError);
                squery = "UPDATE    Inventarios set Inventarios.Cantidad = Inventarios.Cantidad - VentasDetalle.Cantidad FROM         VentasDetalle INNER JOIN Inventarios ON VentasDetalle.IDArticulo = Inventarios.IDArticulo AND VentasDetalle.IDStore = Inventarios.Almacen WHERE (VentasDetalle.IDVenta =)" + idVenta;
                obj.EjecutaQry(squery, CommandType.Text, connstringWEB, sError);
                squery = "select Archivo_FE from ventasEncabezado where id=(select IDVenta from VentasPagos where ID=)" + IdAbono;
                dt = obj.EjecutaQry_Tabla(squery, CommandType.Text, "LastSale", connstringWEB);
                squery = "INSERT INTO PushMoney2 (UserId,Tienda,Franquicia,FechaVenta,FechaCFDI,Factura,Marca,IdArticulo,PushMoney,Cantidad,Total) SELECT VE.IDUser,ADS.AdminStoreID,OC.CardCode, VE.Fecha,VE.FechaCFDI,(VE.SUFIJO+' '+VE.Folio),OI.U_BXP_MARCA,OI.ItemCode,OI.U_PM,VD.Cantidad,(VD.Cantidad * OI.U_PM)AS TotalPush  FROM "+ _NameBDPos + ".dbo.VentasEncabezado VE JOIN " + _NameBDPos + ".dbo.VentasDetalle VD ON VD.IDVenta = VE.ID JOIN " + _NameBDPos + ".dbo.AdminStore ADS ON ADS.AdminStoreID = VE.IDStore JOIN " + _NameBDSap + ".dbo.OCRD OC ON OC.CardCode = ADS.DefaultCustomer JOIN " + _NameBDPos + ".dbo.Articulos ART ON ART.IDArticulo = VD.IDArticulo JOIN " + _NameBDSap + ".dbo.OITM OI ON ART.ArticuloSBO = OI.ItemCode WHERE VE.ID =" + idVenta;
                obj.EjecutaQry(squery, CommandType.Text, connstringWEB, sError);
                if (String.IsNullOrEmpty(sError))
                {
                    ResponseC = "true";
                }
                else
                {
                    ResponseC = "false";
                }
                return ResponseC;
            }
            catch (Exception ex)
            {

            }
            return null;
        }





    }
}
