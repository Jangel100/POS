using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using APIPOSS.Models.Abonos;
using APIPOSS.Utilities;
using System.Data;
using System.Web.Http;
using System.Configuration;
using System.Web.Http.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace APIPOSS.Controllers
{
    public class AbonosController : ApiController
    {
        returnpago infopago = new returnpago();
        private string connWEB;

        [System.Web.Http.Route("api/searchpedido")]
        public JsonResult<List<AddPay>>  searchpedido(AddPay abonos)
        {
            AddPay searchfolio = new AddPay();
            var Pedido= this.Ventas(abonos);
            if (Pedido != null)
            {
                return Json<List<AddPay>>(Pedido);
            }
            else {
                searchfolio.Id = null;
                return Json<List<AddPay>>(Pedido);
            }
            

            //return null;
        }
        private List<AddPay> Ventas(AddPay abonos)
        {

                List<AddPay> oList = new List<AddPay>();
                DataTable dt = new DataTable();
                DataTable dtpayback = new DataTable();
                DataTable dtidv = new DataTable();
                bool tieneMonedero = false;
                connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                Utilities.DBMaster oDB = new Utilities.DBMaster();
                string sQuery, Idventa;
                string CorreoCliente= "";
                string CorreoUsuario = "";
                string Statusventa = "";
                string sQueryIdv = "select * from VentasEncabezado where IDStore=" + abonos.Idstore + " and Folio=" + abonos.Pedido;
                dtidv = oDB.EjecutaQry_Tabla(sQueryIdv, CommandType.Text, "IDVenta", connWEB);
                try { 
                   Idventa = dtidv.Rows[0][0].ToString();
                   CorreoCliente = dtidv.Rows[0][34].ToString();
                   CorreoUsuario = dtidv.Rows[0][35].ToString();
                   Statusventa = dtidv.Rows[0][13].ToString();
                }
                 catch (Exception ex) { Idventa = ""; }
            if (Statusventa.Trim() == "D" || Statusventa.Trim() == "CA")
            { //cancelado o devuelto
                oList.Add(new AddPay("0"));
                return oList;
            }
            string sQueryPayback = "select * from Dat_TransactionPayback where idVenta=" + Idventa;
                dtpayback = oDB.EjecutaQry_Tabla(sQueryPayback, CommandType.Text, "Monederopayback", connWEB);
                if (dtpayback.Rows.Count >=1) { tieneMonedero = true; }
                sQuery = "set dateformat dmy " +  Environment.NewLine;
                sQuery = sQuery + " SELECT    TOP(1) ";
                sQuery = sQuery + " VE.ID AS ID";
                sQuery = sQuery + " , ve.Prefijo + '-' + ve.Folio as IDVenta";
                sQuery = sQuery + " ,VE.fecha as [Fecha Venta]";
                sQuery = sQuery + " ,(SELECT AdminUser.FirstName + ' ' + LastName FROM AdminUser WHERE AdminUserID=ve.IDUser) as Venderor";
                sQuery = sQuery + " ,(select isnull(SUM(monto),0) from VentasPagos where IDVenta=ve.ID) as [Pagado]";
                sQuery = sQuery + " ,(select isnull(SUM(totallinea),0) from VentasDetalle where IDVenta=ve.ID) - (select isnull(SUM(monto),0) from VentasPagos where IDVenta=ve.ID) as [Por Pagar]";
                sQuery = sQuery + " ,(select isnull(SUM(totallinea),0) from VentasDetalle where IDVenta=ve.ID) as [Total]";
                sQuery = sQuery + " ,'' as [Forma de Pago]";
                sQuery = sQuery + " ,'' as [TipoTarjeta]";
                sQuery = sQuery + " ,'' as [Terminacion Tarjeta]";
                sQuery = sQuery + " ,'' as [Monto]";
                sQuery = sQuery + " ,VE.Facturado";
                sQuery = sQuery + " , Case WHEN  VE.MetodoPago33 IS NULL THEN '' ELSE VE.MetodoPago33 END as [MetodoPago33]";
            sQuery = sQuery + " , (select top 1 isnull(Parcialidad,0) from VentasPagos where IDVenta=ve.ID order by Parcialidad desc) as [UltimaParcialidad]";
            if (tieneMonedero) {
                sQuery = sQuery + " , DT.Monedero";
                sQuery = sQuery + " , DT.Nip as [SecretPassword]";
            }
            else { 
                //nothing
            }
                sQuery = sQuery + " FROM VentasEncabezado VE LEFT OUTER JOIN ClientesFacturacion C ON C.IDVenta = VE.ID";
          
            if (tieneMonedero)
            {
                sQuery = sQuery + " Inner Join Dat_TransactionPayback DT on DT.idVenta=VE.ID";
            }
            else
            {
                //nothing
            }
            if (abonos.botton == false)
                {
                    sQuery = sQuery + " WHERE     (VE.IDCliente = '" + abonos.Cliente + "')";
                    if (abonos.Fecha != "")
                    {
                        sQuery = sQuery + " AND (CAST(DATENAME(month, VE.Fecha) AS varchar) + '-' + CAST(CAST(DATEPART(year, VE.Fecha) AS varchar) AS varchar)  = '" + abonos.Fecha + "') ";
                        sQuery = sQuery + " AND (VE.Folio = '" + abonos.Pedido + "') AND (VE.Prefijo = '" + abonos.Prefijo + "') ";
                    }
                    sQuery = sQuery + "  and VE.StatusVenta='O' and ((select isnull(SUM(totallinea),0) from VentasDetalle where IDVenta=ve.ID) - (select isnull(SUM(monto),0) from VentasPagos where IDVenta=ve.ID)) > 0 and ve.idstore=" + abonos.Idstore;
                }
                else
                    sQuery = sQuery + " where ve.Folio = '" + abonos.Pedido + "' and ve.idstore=" + abonos.Idstore;
                dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "Devoluciones", connWEB);

                var valor = "";
                var facturadoAnt = "";

                foreach (DataRow Drow in dt.Rows)
                {
                if (tieneMonedero)
                {
                    oList.Add(new AddPay(Drow["ID"].ToString(), Drow["IDVenta"].ToString(), Drow["Fecha Venta"].ToString(), Drow["Venderor"].ToString(), string.Format("{0:c}", Convert.ToDecimal(Drow["Pagado"])), string.Format("{0:c}", Convert.ToDecimal(Drow["Por Pagar"])), string.Format("{0:c}", Convert.ToDecimal(Drow["Total"])), Drow["Forma de Pago"].ToString(), Drow["TipoTarjeta"].ToString(), Drow["Terminacion Tarjeta"].ToString(), Drow["Monto"].ToString(), Drow["MetodoPago33"].ToString(), Convert.ToInt32(Drow["UltimaParcialidad"]), Drow["Monedero"].ToString(), Drow["SecretPassword"].ToString(), CorreoCliente, CorreoUsuario));

                }
                else
                {
                    oList.Add(new AddPay(Drow["ID"].ToString(), Drow["IDVenta"].ToString(), Drow["Fecha Venta"].ToString(), Drow["Venderor"].ToString(), string.Format("{0:c}", Convert.ToDecimal(Drow["Pagado"])) , string.Format("{0:c}", Convert.ToDecimal(Drow["Por Pagar"])), string.Format("{0:c}", Convert.ToDecimal(Drow["Total"])), Drow["Forma de Pago"].ToString(), Drow["TipoTarjeta"].ToString(), Drow["Terminacion Tarjeta"].ToString(), string.Format("{0:c}", Drow["Monto"].ToString()), Drow["MetodoPago33"].ToString(), Convert.ToInt32(Drow["UltimaParcialidad"]),CorreoCliente, CorreoUsuario));
                
                }
                valor = Drow["MetodoPago33"].ToString();
                facturadoAnt = Drow["Facturado"].ToString();
                }
            
                if (facturadoAnt == "False") 
                {
                //si no esta facturado no realiza abono
                //oList = null; 
                }
                else if (valor == "" | valor == "PUE")
                {
                }

                return oList;
          
        }

        [System.Web.Http.Route("api/AddPay")]
        [System.Web.Http.HttpPost]
        public JsonResult<AddPay> AddPay(AddPay Pago)
        {

            try
            {
                var idventa = Pago.Id;
                connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                if (realizaAbono(Pago) == true)
                {
                    Utilities.GeneralClass objlog = new Utilities.GeneralClass();
                    objlog.InsertaLog(Tipos.AB, infopago.IDPRINTPAGO, infopago.IDSTORE, infopago.WHSID);
                    AddPay ResponseP = new AddPay();
                    ResponseP.payresponse = true;
                    ResponseP.Id = infopago.IDPRINTPAGO.ToString();
                    ResponseP.IDVenta = idventa;


                    return Json<AddPay>(ResponseP);
                }
                else {
                    AddPay ResponseP = new AddPay();
                    ResponseP.payresponse = false;
                    return Json<AddPay>(ResponseP);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
          
        }

        public bool realizaAbono(AddPay AddPago)
        {
            string squery = "";
            string Afiliacion ="";
            StringBuilder result = new StringBuilder();
            bool IsDormicredit = false;
            string Tipodeventa = "";
            string Statusventa = "";
            string facturado = "";
            bool completedormi = false;
            var IDToFind = AddPago.Id;
            var idtienda = AddPago.Idstore;
            DataTable dt = new DataTable(), dty2 = new DataTable();
            Utilities.DBMaster oDB = new Utilities.DBMaster();

            string squery1 = "";
            string squery2 = "";
            var formapago = AddPago.FormaDePago;
            switch (formapago)
            {
                case "Terminal Banamex":
                    {
                        squery1 = "SELECT Banamex FROM Bancoafiliacion where idtienda=" + idtienda;
                        break;
                    }

                case "Terminal Bancomer":
                    {
                        squery1 = "SELECT Bancomer FROM Bancoafiliacion where idtienda=" + idtienda;
                        break;
                    }

                case "American Express":
                    {
                        squery1 = "SELECT SantanderDualAmex FROM Bancoafiliacion where idtienda=" + idtienda;
                        break;
                    }

                case "Terminal Santander":
                    {
                        squery1 = "SELECT SantanderDualGprs FROM Bancoafiliacion where idtienda=" + idtienda;
                        break;
                    }

                case "Terminal PROSA":
                    {
                        squery1 = "SELECT SantanderProsaGprs FROM Bancoafiliacion where idtienda=" + idtienda;
                        break;
                    }

                case "Efectivo":
                    {
                        break;
                    }

                case "Transf. Elect. Dormicredit St.":
                    {
                        break;
                    }

                case "Deposito / Cheque":
                    {
                        break;
                    }

                case "Nota de crédito":
                    {
                        break;
                    }

                case "Otros":
                    {
                        break;
                    }

                case "Exclusivo Oficina":
                    {
                        break;
                    }

                case "No Identificado":
                    {
                        break;
                    }

                case "Transferencia Electronica":
                    {
                        break;
                    }

                case "N/A":
                    {
                        break;
                    }
            }

            if (formapago != "Efectivo" & formapago != "Deposito / Cheque" & formapago != "Transferencia Electronica" & formapago != "Transf. Elect. Dormicredit St.")
            {
                dt = oDB.EjecutaQry_Tabla(squery1, CommandType.Text, "Afiliacion", connWEB);
                if (dt.Rows.Count == 0)
                    Afiliacion = "";
                else
                    Afiliacion = dt.Rows[0][0].ToString();
            }
            squery2 = "select ID, StatusVenta,Facturado,FechaConfirmacion,Tipodeventa from VentasEncabezado where ID=" + IDToFind;
            dty2 = oDB.EjecutaQry_Tabla(squery2, CommandType.Text, "tipodeventa", connWEB);
            try
            {
                Tipodeventa = dty2.Rows[0][4].ToString();
            }
            catch (Exception ex)
            {
                Tipodeventa = "ok";
            }


            if (string.IsNullOrEmpty(Tipodeventa))
                Tipodeventa = "ok";
            IsDormicredit =(Tipodeventa == "Dormicredit") ? true : false;
            Statusventa = dty2.Rows[0][1].ToString();
            facturado = dty2.Rows[0][2].ToString();
            double montoAbono;

            switch (AddPago.FormaDePago)
            {

                case "Efectivo":
                    AddPago.FormaPago33 = "01";
                    break;

                case "Terminal Banamex":
                case "Terminal Bancomer":
                case "Terminal Banorte":
                case "Terminal PROSA":
                case "Terminal Santander":
                case "American Express":
                    switch (AddPago.TipoTarjeta)
                    {
                        case "Credito": AddPago.FormaPago33 = "04"; break;
                        case "Debito": AddPago.FormaPago33 = "28"; break;
                        case "Crédito": AddPago.FormaPago33 = "04"; break;
                        case "Débito": AddPago.FormaPago33 = "28"; break;
                        case "": AddPago.FormaPago33 = ""; break;
                    }
                    break;
                case "Deposito / Cheque":
                    AddPago.FormaPago33 = "02";
                    break;
                case "Transferencia Electronica":
                    AddPago.FormaPago33 = "03"; break;
                case "Transferencia Electrónica":
                    AddPago.FormaPago33 = "03"; break;
                case "Transf. Elect. Dormicredit St.":
                   AddPago.FormaPago33 = "03"; break;
                case "Compensación":
                    AddPago.FormaPago33 = "17"; break;
                default:
                    AddPago.FormaPago33 = "99";
                    break;
            }
            if (AddPago.Monto !="")
                montoAbono = Convert.ToDouble(AddPago.Monto);
            else
                montoAbono = 0;


            if (montoAbono > Convert.ToDouble(AddPago.PorPagar))
            {
                //btnDevolucion.Enabled = true;
                //btnDevolucion.Disabled = false;
                //DoBind("C");
                return false;
            }
            double Porpagar = Convert.ToDouble(AddPago.PorPagar);

            double Saldo = Porpagar - montoAbono;
            if (Saldo <= 0.0 & IsDormicredit == true)
            {
                if (facturado == "True")
                    completedormi = true;
            }

            if (montoAbono <= 0)
            {
                //btnDevolucion.Enabled = true;
                //btnDevolucion.Disabled = false;
                //DoBind("C");
                return false;
            }
                squery = " insert into VentasPagos (IDVenta,AdminStoreID,TipoVenta,FormaPago,TipoTarjeta,Monto,Fecha,StatusPago,NoCuenta,Prefijo,Folio,Afiliacion,FormaPago33,MetodoPago33,TipoComp33,UsoCFDI33,TipoRel33,CFDI_Rel33,CorreoCliente,CorreoUsuario,Parcialidad,FechaPago,Importado,comentariosNoTimbrar,Facturado)" +  Environment.NewLine;
                squery = squery + " values (" + AddPago.Id + ",";
                squery = squery + idtienda + ",'VT'," + Environment.NewLine; 
                squery = squery + "'" + AddPago.FormaDePago + "'," +  Environment.NewLine;
                squery = squery + "'" + AddPago.TipoTarjeta + "'," +  Environment.NewLine;
                squery = squery + AddPago.Monto + "," +  Environment.NewLine;
                squery = squery + "getdate(),'O','" + AddPago.TerminacionTarjeta + "'," +  Environment.NewLine;
                squery = squery + "(select Prefijo from StoreFolios  where AdminStoreID=" + idtienda + " and AdminFolioType=2) ,(select CurrentFolio + 1 from StoreFolios  where AdminStoreID=" + idtienda + " and AdminFolioType=2)," +  Environment.NewLine;
                squery = squery + "'" + Afiliacion + "', '" + AddPago.FormaPago33 + "','" +  AddPago.MetodoPago33 + "', '" + AddPago.TipoComp33 + "','"  + AddPago.UsoCFDI33 + "', '" + AddPago.TipoRel33 + "','" + AddPago.CFDI_Rel33 +
                "','" + AddPago.CorreoCliente + "','" + AddPago.CorreoUsuario + "'," + AddPago.Parcialidad + ",'" + (!string.IsNullOrEmpty(AddPago.FechaPago) ? Convert.ToDateTime(AddPago.FechaPago).ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd")) +"',"+AddPago.Importado+",'"+AddPago.comentariosNoTimbrar+"',"+ AddPago.Importado+ ")" +  Environment.NewLine;

                oDB.EjecutaQry(squery, CommandType.Text, connWEB,"");

                squery = " Update  StoreFolios set CurrentFolio = CurrentFolio + 1 where AdminStoreID=" + idtienda + " and AdminFolioType=2" +  Environment.NewLine;
                oDB.EjecutaQry(squery, CommandType.Text, connWEB, "");


                var idAbono = getLastAbono(AddPago.Id);
                int IDPRINTABONO = Convert.ToInt32(idAbono);
                int IDSTORE = Convert.ToInt32(AddPago.Idstore);
                var WHSID = AddPago.WhsID; 
                returnpago infop = new returnpago(IDPRINTABONO, IDSTORE, WHSID);
                infopago = infop;
            if (completedormi)
                {
                    squery = "Update VentasEncabezado set StatusVenta='O' where id=" + IDToFind;
                    oDB.EjecutaQry(squery, CommandType.Text, connWEB, "");
                }

            return true;
        }
        public string getLastAbono(string idVenta)
        {
            string sQuery;
            DataTable dt = new DataTable();
            Utilities.DBMaster oDB = new Utilities.DBMaster();

            sQuery = "select max(id) as LastSale from VentasPagos where IDVenta=" + idVenta;

            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "LastSale", connWEB);

            foreach (DataRow Drow in dt.Rows)
            {
                if(Convert.IsDBNull(Drow["LastSale"]) == false)
                return Drow["LastSale"].ToString();
            }
           
            return "0";

            dt = null/* TODO Change to default(_) if this is not a reference type */;
            GC.Collect();
        }

        [System.Web.Http.Route("api/GetPeriods")]
        public JsonResult<List<ListPeriodo>> GetPeriods(string idcliente)
        {
            StringBuilder UserQuery = new StringBuilder();
            DataTable dt;
            try
            {
                connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                UserQuery.Append("select distinct CAST(DATENAME(month, VE.Fecha) AS varchar) + '-' + CAST(CAST(DATEPART(year, VE.Fecha) AS varchar) AS varchar) as Periodo from ventasEncabezado VE where VE.IDCliente =" + idcliente);
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Clientes", connWEB);

                if (dt != null)
                {
                    var dtPeriods = (from DataRow rows in dt.Rows
                                   select new ListPeriodo
                                   {
                                       Periodo = (string)rows["Periodo"].ToString(),
                                   }).ToList();
                    return Json<List<ListPeriodo>>(dtPeriods);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        [System.Web.Http.Route("api/GetDays")]
        public JsonResult<List<ListDia>> GetDays(string jsonparams)
        {
            StringBuilder UserQuery = new StringBuilder();
            DataTable dt;
            string jsonpa = string.Empty;
            dynamic data = JObject.Parse(jsonparams);
            string idcliente = data.idcliente;
            string date = data.date;
            string idstore = data.idstore;
            try
            {
                connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                UserQuery.Append("Select distinct RIGHT('00' + convert(varchar,CAST(DATENAME(day, VE.Fecha) AS varchar)),2) as dia from ventasencabezado VE, Clientes CL where VE.IDCliente = CL.ID and idStore = " + idstore + " and VE.IDCliente =" + idcliente + " and CAST(DATENAME(month, VE.Fecha) AS varchar) + '-' + CAST(CAST(DATEPART(year, VE.Fecha) AS varchar) AS varchar) ='" + date + "' order by dia");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Dias", connWEB);

                if (dt != null)
                {
                    var dtDays = (from DataRow rows in dt.Rows
                                     select new ListDia
                                     {
                                         Dia = (string)rows["Dia"].ToString(),
                                     }).ToList();
                    return Json<List<ListDia>>(dtDays);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        [System.Web.Http.Route("api/GetFolios")]
        public JsonResult<List<ListFolio>> GetFolios(string jsonparams)
        {
            StringBuilder UserQuery = new StringBuilder();
            DataTable dt;
            dynamic data = JObject.Parse(jsonparams);
            string idcliente = data.idcliente;
            string date = data.date;
            string idstore = data.idstore;
            string day = data.day;
            try
            {
                connWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                UserQuery.Append("select distinct VE.folio, VE.Prefijo  + '-' + VE.Folio  as 'FolioPref' from ventasencabezado VE, Clientes CL where VE.IDCliente = CL.ID and idStore= " + idstore + "and VE.IDCliente = " + idcliente + " and CAST(DATENAME(month, VE.Fecha) AS varchar) + '-' + CAST(CAST(DATEPART(year, VE.Fecha) AS varchar) AS varchar) ='" + date + "' and CAST(DATENAME(DAY, VE.Fecha) AS varchar) = " + day + " and (select count(*) from ventasDetalle VDE,ventasEncabezado VEN where VEN.ID=VDE.IDVenta and statusLinea='O' and VEN.Folio=VE.Folio) <> 0");
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla(UserQuery.ToString(), CommandType.Text, "Dias", connWEB);

                if (dt != null)
                {
                    var dtFolios = (from DataRow rows in dt.Rows
                                  select new ListFolio
                                  {
                                      Folio = (string)rows["Folio"].ToString(),
                                      FolioPref = (string)rows["FolioPref"].ToString(),
                                  }).ToList();
                    return Json<List<ListFolio>>(dtFolios);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

    }
}