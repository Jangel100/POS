using APIPOSS.Models.Configuracion;
using APIPOSS.Models.ConsultasFacturacion;
using APIPOSS.Models.Ventas;
using APIPOSS.Views.Facturacion;
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
    public class VentasConsultasController : ApiController
    {
        [Route("api2/GetInfoClienteConsultas")]
        [HttpPost]
        public JsonResult<List<Clientes>> GetInfoCliente(Clientes clientes)
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

        [Route("api2/GetInfoDetalleClienteConsultas")]
        [HttpPost]
        public JsonResult<string> GetInfoDetalleClienteConsultas(Clientes clientes)
        {
            try
            {
              
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [Route("api2/GetPeriodoClienteConsultas")]
        [HttpPost]
        public List<PeriodoView> GetPeriodoClienteConsultas(Clientes clientes)
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
        [Route("api2/GetDiaClienteConsultas")]
        [HttpPost]
        public List<DiaView> GetDiaClienteConsultas(PeriodoView Periodo)
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
        [Route("api2/GetFolioClienteConsultas")]
        [HttpPost]
        public List<FolioView> GetFolioClienteConsultas(PeriodoView Periodo)
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
        [Route("api2/GetClientePedidoConsult")]
        [HttpPost]
        public List<PedidosView> GetClientePedidoConsult(PedidoParameterIntoView pedido)
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
                dt = obj.EjecutaQry_Tabla("ClientePedidoConsult", lsParameters, CommandType.StoredProcedure, "Devoluciones", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new PedidosView
                              {
                                 id = rows["id"] is DBNull?0 : Convert.ToInt32(rows["id"]),
                                 venta = rows["venta"] is DBNull ? "" : (string)rows["venta"],
                                 abono = rows["abono"] is DBNull ? "" : (string)rows["abono"],
                                 fechareciboOutput = rows["fecharecibo"] is DBNull ? "": Convert.ToString((DateTime)rows["fecharecibo"]),
                                 fechaventaOutput = rows["fechaventa"] is DBNull ? "":  Convert.ToString((DateTime)rows["fechaventa"]),
                                 motopagado = rows["montopagado"] is DBNull ? 0 : (Decimal)rows["montopagado"],
                                 Reimprimir = rows["Reimprimir"] is DBNull ? "" : (string)rows["Reimprimir"],
                                  btnDownloadFactura = "<button type='button' onclick='Consultas.GetFacturaPDF(" + (rows["id"] is DBNull ? "0" : (string)rows["id"].ToString()) + ",1,"+'"' + Convert.ToDateTime(rows["fechaventa"]).ToString("dd/MM/yyyy") + '"' + ")' name='Imprimir1' class='btn btn-info btn-sm '>" + "<i class='fa fa-download'/></i>Descargar</button>",
                                  btnDownloadComplemento = "<button type='button' onclick='Consultas.GetFacturaPDF(" + (rows["id"] is DBNull ? "0" : (string)rows["id"].ToString()) + ",2," + '"' + Convert.ToDateTime(rows["fecharecibo"]).ToString("dd/MM/yyyy") +'"' + ")' name='Imprimir2' class='btn btn-primary btn-sm '>" + "<i class='fa fa-download'/></i>Descargar</button>"
                                  //Facturado = rows["Facturado"] is DBNull ? "" : (string)rows["Facturado"]
                                  //Restante = rows["Restante"] is DBNull ? 0 : (Decimal)rows["Restante"],

                                  //Devolucion = rows["Devolucion"] is DBNull ? 0 :  Convert.ToInt32(rows["Devolucion"]),
                                  //NCE = rows["NCE"] is DBNull ? "" : (string)rows["NCE"],
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
        [Route("api2/GetDataListFacturacion")]
        [HttpPost]
        public FacturacionCFDIView GetDataListFacturacion(SaleSearchView SaleView)
        {
            FacturacionCFDIView facturacionCFDIView = new FacturacionCFDIView();
            IEnumerable<UsoCFDIView> lsUsoCFDI;
            IEnumerable<MetodoPagosView> lsMetodoPagos;
            try
            {
                lsUsoCFDI = GetDataUsoCFDI();
                lsMetodoPagos = GetDataMetodoPagos();

                facturacionCFDIView.lsUsoCFDI = lsUsoCFDI;
                facturacionCFDIView.lsMetodoPagos = lsMetodoPagos;
                return facturacionCFDIView;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public IEnumerable<UsoCFDIView> GetDataUsoCFDI()
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("FacturacionUsoCFDI", CommandType.StoredProcedure, "Cat_Uso_CFDI", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new UsoCFDIView
                              {
                                  Id = rows["id_uso_cfdi"] is DBNull ? "" :  Convert.ToInt32(rows["id_uso_cfdi"]).ToString(),
                                  UsoCFDI = rows["uso_CFDI"] is DBNull ? "" : (string)rows["uso_CFDI"],
                                  Descripcion = rows["Descripcion"] is DBNull ? "" : (string)rows["Descripcion"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public IEnumerable<MetodoPagosView> GetDataMetodoPagos()
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("FacturacionMetodosPagos", CommandType.StoredProcedure, "Cat_MetodoPago", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new MetodoPagosView
                              {
                                  Id = rows["id_metodoPago"] is DBNull ? "" : Convert.ToInt32(rows["id_metodoPago"]).ToString(),
                                  MetodoPago = rows["MetodoPago"] is DBNull ? "" : (string)rows["MetodoPago"],
                                  Descripcion = rows["Descripcion"] is DBNull ? "" : (string)rows["Descripcion"]
                              }).ToList();

                    return ls;
                }
                return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        [Route("api2/GetDataListFormaPagoFacturacion")]
        [HttpPost]
        public IEnumerable<FormasDePagosView> GetDataListFormaPagoFacturacion(FacturacionFormaPagoCFDIView metodoPago)
        {
            DataTable dt;
            string connstringWEB;
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;

                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@MetodoPago", Value = metodoPago.IdMetodoPago },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@FormaPago", Value = metodoPago.FormaPago }
                    }; 

                Utilities.DBMaster obj = new Utilities.DBMaster();
                 dt = obj.EjecutaQry_Tabla("FacturacionFormaPago", lsParameters, CommandType.StoredProcedure, "Cat_FormaPago", connstringWEB);

                if (dt != null)
                {

                    var ls = (from DataRow rows in dt.Rows
                              select new FormasDePagosView
                              {
                                  Id           = rows["id_Formapago"] is DBNull ? "" : Convert.ToInt32(rows["id_Formapago"]).ToString(),
                                  FormaDePago  = rows["FormaPago"] is DBNull ? "" : Convert.ToInt32(rows["FormaPago"]).ToString(),
                                  Descripcion  = rows["Descripcion"] is DBNull ? "" : (string)rows["Descripcion"]
                              }).ToList();                    

                    return (ls);
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
        [Route("api/GetFacturaPDF")]
        [HttpPost]
        public JsonResult<LastSale> GetFacturaPDF(LastSale LastSale)
        {
            DataTable dt;
            string connstringWEB;
            Utilities.DBMaster oDB = new Utilities.DBMaster();
            LastSale ResponseLastSaleNull = new LastSale();
            try
            {
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                string sQuery="";
                if (LastSale.relacion)
                {

                    if(LastSale.Documento == "Factura")
                    {
                         sQuery = "SELECT " + Environment.NewLine + "(SELECT top 1 IIF(OINV.Series = 74, 'DRC', iif(OINV.series = 76, 'F','') )+ CAST(OINV.DocNum AS varchar(15)) AS Factura FROM  SMU_VF_SI.DBO.INV1 INNER JOIN SMU_VF_SI.DBO.OINV ON INV1.U_FOLPOS=OINV.U_FOLPOS WHERE INV1.U_IDDOCPOS=VE.ID) AS ARCHIVO_FE," + Environment.NewLine + "	YEAR((SELECT top 1 OINV.DocDate AS Factura FROM  SMU_VF_SI.DBO.INV1 INNER JOIN SMU_VF_SI.DBO.OINV ON INV1.U_FOLPOS=OINV.U_FOLPOS WHERE INV1.U_IDDOCPOS=VE.ID)) AS Year," + Environment.NewLine + "	FolderPDF," + Environment.NewLine + "	MONTH((SELECT top 1 OINV.DocDate AS Factura FROM  SMU_VF_SI.DBO.INV1 INNER JOIN SMU_VF_SI.DBO.OINV ON INV1.U_FOLPOS=OINV.U_FOLPOS WHERE INV1.U_IDDOCPOS=VE.ID)) AS Mes," + Environment.NewLine + "	Day((SELECT top 1 OINV.DocDate AS Factura FROM  SMU_VF_SI.DBO.INV1 INNER JOIN SMU_VF_SI.DBO.OINV ON INV1.U_FOLPOS=OINV.U_FOLPOS WHERE INV1.U_IDDOCPOS=VE.ID)) AS Dia," + Environment.NewLine + "	VE.Prefijo + RIGHT('0000' + CONVERT(VARCHAR, VE.FOLIO),5) AS Foliointerno," + Environment.NewLine + "	VE.Sufijo," + Environment.NewLine + "	VE.Prefijo AS Sucursal," + Environment.NewLine + "	VE.Folio AS Folio," + Environment.NewLine + "	(SELECT LicTradNum FROM " + LastSale.SAPDB + ".OCRD WHERE OCRD.CardCode=ADS.DefaultCustomer) as RFC" + Environment.NewLine + "FROM" + Environment.NewLine + "	VentasEncabezado VE INNER JOIN" + Environment.NewLine + "	AdminStore ADS ON VE.IDStore = ADS.AdminStoreID " + Environment.NewLine + "WHERE VE.ID=(SELECT VP.IDVENTA FROM VentasPagos VP WHERE VP.ID=" + LastSale.IdAbono + ")	" + Environment.NewLine + "";
                    }
                    else if(LastSale.Documento == "Complemento")
                    {
                        sQuery = "SELECT " + Environment.NewLine + "(SELECT top 1 'P'+ CAST(ORCT.DocNum AS varchar(15)) AS Factura FROM  SMU_VF_SI.DBO.RCT2 INNER JOIN SMU_VF_SI.DBO.ORCT ON ORCT.DocEntry=RCT2.DocNum inner join SMU_VF_SI.DBO.OINV on RCT2.DocEntry = OINV.DocEntry WHERE OINV.DocNum=(SELECT top 1 OINV.DocNum FROM  SMU_VF_SI.DBO.INV1 INNER JOIN SMU_VF_SI.DBO.OINV ON INV1.U_FOLPOS=OINV.U_FOLPOS WHERE INV1.U_IDDOCPOS=VE.ID) and ORCT.Canceled!='Y' and ORCT.DocTotal=(SELECT VP.Monto FROM VentasPagos VP WHERE VP.ID=" + LastSale.IdAbono + ")) AS ARCHIVO_FE," + Environment.NewLine + "	YEAR((SELECT top 1 ORCT.DocDate FROM  SMU_VF_SI.DBO.RCT2 INNER JOIN SMU_VF_SI.DBO.ORCT ON ORCT.DocEntry=RCT2.DocNum inner join SMU_VF_SI.DBO.OINV on RCT2.DocEntry = OINV.DocEntry WHERE OINV.DocNum=(SELECT top 1 OINV.DocNum FROM  SMU_VF_SI.DBO.INV1 INNER JOIN SMU_VF_SI.DBO.OINV ON INV1.U_FOLPOS=OINV.U_FOLPOS WHERE INV1.U_IDDOCPOS=VE.ID) and ORCT.DocTotal=(SELECT VP.Monto FROM VentasPagos VP WHERE VP.ID=" + LastSale.IdAbono + "))) AS Year," + Environment.NewLine + "	FolderPDF," + Environment.NewLine + "MONTH((SELECT top 1 ORCT.DocDate FROM  SMU_VF_SI.DBO.RCT2 INNER JOIN SMU_VF_SI.DBO.ORCT ON ORCT.DocEntry=RCT2.DocNum inner join SMU_VF_SI.DBO.OINV on RCT2.DocEntry = OINV.DocEntry WHERE OINV.DocNum=(SELECT top 1 OINV.DocNum FROM  SMU_VF_SI.DBO.INV1 INNER JOIN SMU_VF_SI.DBO.OINV ON INV1.U_FOLPOS=OINV.U_FOLPOS WHERE INV1.U_IDDOCPOS=VE.ID) and ORCT.DocTotal=(SELECT VP.Monto FROM VentasPagos VP WHERE VP.ID=" + LastSale.IdAbono + "))) AS Mes," + Environment.NewLine + "	Day((SELECT top 1 ORCT.DocDate FROM  SMU_VF_SI.DBO.RCT2 INNER JOIN SMU_VF_SI.DBO.ORCT ON ORCT.DocEntry=RCT2.DocNum inner join SMU_VF_SI.DBO.OINV on RCT2.DocEntry = OINV.DocEntry WHERE OINV.DocNum=(SELECT top 1 OINV.DocNum FROM  SMU_VF_SI.DBO.INV1 INNER JOIN SMU_VF_SI.DBO.OINV ON INV1.U_FOLPOS=OINV.U_FOLPOS WHERE INV1.U_IDDOCPOS=VE.ID) and ORCT.Canceled!='Y' and ORCT.DocTotal=(SELECT VP.Monto FROM VentasPagos VP WHERE VP.ID=" + LastSale.IdAbono + "))) AS Dia," + Environment.NewLine + "	VE.Prefijo + RIGHT('0000' + CONVERT(VARCHAR, VE.FOLIO),5) AS Foliointerno," + Environment.NewLine + "	VE.Sufijo," + Environment.NewLine + "	VE.Prefijo AS Sucursal," + Environment.NewLine + "	VE.Folio AS Folio," + Environment.NewLine + "	(SELECT LicTradNum FROM " + LastSale.SAPDB + ".OCRD WHERE OCRD.CardCode=ADS.DefaultCustomer) as RFC" + Environment.NewLine + "FROM" + Environment.NewLine + "	VentasEncabezado VE INNER JOIN" + Environment.NewLine + "	AdminStore ADS ON VE.IDStore = ADS.AdminStoreID " + Environment.NewLine + "WHERE VE.ID=(SELECT VP.IDVENTA FROM VentasPagos VP WHERE VP.ID=" + LastSale.IdAbono + ")	" + Environment.NewLine + "";
                    }
                }
                else
                {
                    // Para facturas anteriores a SAP 
                    if (LastSale.Documento == "Factura")
                    {
                    sQuery =
                    "SELECT " + Environment.NewLine +
                    "	ARCHIVO_FE," + Environment.NewLine +
                    "	YEAR(VE.fechaCFDI)," + Environment.NewLine +
                    "	FolderPDF," + Environment.NewLine +
                    "	MONTH(VE.FechaCFDI) AS MES," + Environment.NewLine +
                    "	DAY(VE.FechaCFDI) AS DIA," + Environment.NewLine +
                    "	VE.Prefijo + RIGHT('0000' + CONVERT(VARCHAR, VE.FOLIO),5) AS FOLIOINTERNO," + Environment.NewLine +
                    "	VE.SUFIJO," + Environment.NewLine +
                    "	VE.Prefijo AS SUCURSAL," + Environment.NewLine +
                    "	VE.Folio AS FOLIO," + Environment.NewLine +
                    "	(SELECT LicTradNum FROM " + LastSale.SAPDB + ".OCRD WHERE OCRD.CardCode=ADS.DefaultCustomer) as RFC" + Environment.NewLine +
                    "FROM" + Environment.NewLine +
                    "	VentasEncabezado VE INNER JOIN" + Environment.NewLine +
                    "	AdminStore ADS ON VE.IDStore = ADS.AdminStoreID " + Environment.NewLine +
                    "WHERE VE.ID=(SELECT VP.IDVENTA FROM VentasPagos VP WHERE VP.ID=" + LastSale.IdAbono + ")	";
                    }
                    else if (LastSale.Documento == "Complemento")
                    {

                        sQuery =
                           "SELECT " + Environment.NewLine +
                           "ARCHIVO_FE," + Environment.NewLine +
                           "YEAR(VP.fechaCFDI)AS AÑO," + Environment.NewLine +
                           "FolderPDF," + Environment.NewLine +
                           "MONTH(VP.FechaCFDI) AS MES," + Environment.NewLine +
                           "DAY(VP.FechaCFDI) AS DIA," + Environment.NewLine +
                           "VP.Prefijo + RIGHT('000000' + CONVERT(VARCHAR, VP.FOLIO),6) AS FOLIOINTERNO," + Environment.NewLine +
                           "''," + Environment.NewLine +
                           "VP.Prefijo AS SUCURSAL," + Environment.NewLine +
                           "VP.Folio AS FOLIO," + Environment.NewLine +
                           "(SELECT LicTradNum FROM " + LastSale.SAPDB + ".OCRD WHERE OCRD.CardCode=ADS.DefaultCustomer) as RFC" + Environment.NewLine +
                          "FROM" + Environment.NewLine +
                           "VentasPagos VP INNER JOIN" + Environment.NewLine +
                           "AdminStore ADS ON  vp.AdminStoreID=ADS.AdminStoreID " + Environment.NewLine +
                          "WHERE(VP.ID = " + LastSale.IdAbono + ")" + Environment.NewLine;
                    }

                }

                dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "LastSale", connstringWEB);
                if (dt.Rows.Count > 0)
                {
                    LastSale ResponseLastSale = new LastSale();

                    ResponseLastSale.Archivo_FE = dt.Rows[0][0].ToString();
                    ResponseLastSale.Year = dt.Rows[0][1].ToString();
                    ResponseLastSale.FolderPDF = dt.Rows[0][2].ToString();
                    ResponseLastSale.Mes = dt.Rows[0][3].ToString();
                    ResponseLastSale.Dia = dt.Rows[0][4].ToString();
                    ResponseLastSale.Foliointerno = dt.Rows[0][5].ToString();
                    ResponseLastSale.Sufijo = dt.Rows[0][6].ToString();
                    ResponseLastSale.Sucursal = dt.Rows[0][7].ToString();
                    ResponseLastSale.Folio = dt.Rows[0][8].ToString();
                    ResponseLastSale.RFC = dt.Rows[0][9].ToString();
                    ResponseLastSale.relacion = LastSale.relacion;
                    return Json<LastSale>(ResponseLastSale);
                }
                return Json<LastSale>(ResponseLastSaleNull);
            }
            catch (Exception ex)
            {

            }
            return Json<LastSale>(ResponseLastSaleNull);
        }
    }
}

