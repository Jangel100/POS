using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPOS.Security;
using BL.Interface;
using BL.Franquicias;
using Entities.viewsModels;
using System.Configuration;
using WebPOS.Utilities;
using System.Data;
using Entities.Models.Abonos;
using Newtonsoft.Json;
using System.Net.Http;
using Entities.Models.Payback;
using Entities.Models.Ventas;

namespace WebPOS.Controllers.AbonosFranquicias
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class AbonosFranquiciasController : Controller
    {
        private string connstringWEB;
        readonly IFranquiciasBL _AbonosFBL;
        // GET: AbonosFranquicias
        public AbonosFranquiciasController(IFranquiciasBL abonosFBL)
        {
            _AbonosFBL = abonosFBL;

        }
        public AbonosFranquiciasController()
        {
            _AbonosFBL = new FranquiciasBL();
        }

        public ActionResult Index()
        {
            ClentesAbonosFranquicias ClentesAbonosFranquicias;
            ViewBag.Idstore = Session["IDSTORE"].ToString();
            ViewBag.WhsID = Session["WHSID"].ToString();
            ClentesAbonosFranquicias = CargaclientesF();
            return View("Index", ClentesAbonosFranquicias);
        }
        public ClentesAbonosFranquicias CargaclientesF()
        {
            try
            {
                ClentesAbonosFranquicias ClentesAb;
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                List<ListClientes> listabcli = new List<ListClientes>();
                List<ListPeriodo> ListPeriodos = new List<ListPeriodo>();
                List<ListDia> ListDia = new List<ListDia>();
                List<ListFolio> ListFolio = new List<ListFolio>();
                var ListClientes = GetClineteabonoF(connstringWEB);
                ClentesAb = new ClentesAbonosFranquicias()
                {
                    ListClientes = ListClientes,
                    ListPeriodos = ListPeriodos,
                    ListDia = ListDia,
                    ListFolio = ListFolio
                };
                return ClentesAb;
            }
            catch (Exception ex) { return null; }
        }
        public List<ListClientes> GetClineteabonoF(string connstringWEB)
        {
            List<ListClientes> listcli = new List<ListClientes>();
            List<ListClientes> Clientes;
            DBMaster oDB = new DBMaster();
            DataTable dt = new DataTable();
            var idstore = Session["IDSTORE"].ToString();
            string squery = "";
            try
            {
                squery = "select distinct CL.ID,  CL.Nombre + ' ' +CL.RFC+ ' Nc:' + CONVERT(varchar(10), CL.ID) as Nombre from ventasencabezado VE, Clientes CL where VE.IDCliente = CL.ID and idStore = " + idstore + " order by Nombre ASC";
                dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "Clientes", connstringWEB);
                if (dt != null)
                {
                    Clientes = (from DataRow rows in dt.Rows
                                select new ListClientes
                                {
                                    ID = (string)rows["ID"].ToString(),
                                    Nombre = (string)rows["Nombre"].ToString(),
                                }).ToList();
                    listcli = Clientes;
                }

                return listcli;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public JsonResult BuscapedidoF(string folio)
        {
            AddPay pago = new AddPay();
            pago.Pedido = folio;
            string JsonResult = string.Empty;
            pago.botton = true;
            AddPay resultpay = new AddPay();
            var resultado = DoBind(pago);
            JsonResult = JsonConvert.SerializeObject(resultado);
            return Json(JsonResult);
        }
        public List<AddPay> DoBind(AddPay abono)
        {
            Session["Abonos"] = null;
            var idstore = Session["IDSTORE"].ToString();
            abono.Idstore = idstore;
            string urlp = string.Empty;

            List<AddPay> ModelosPedResult;
            try
            {
                urlp = "api/searchpedidoF";
                HttpResponseMessage response = _AbonosFBL.ReadAsStringAsyncAPI(urlp, abono);
                response.EnsureSuccessStatusCode();
                ModelosPedResult = response.Content.ReadAsAsync<List<AddPay>>().Result;

                if (abono.botton == true)
                {
                    return ModelosPedResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        public JsonResult AddAbonoF(string Abono)
        {
            var jsonpago = JsonConvert.DeserializeObject<AddPay>(Abono);
            if (string.IsNullOrEmpty(jsonpago.MetodoPago33)) { jsonpago.MetodoPago33 = "PPD"; } //PPD 90 dias 
            decimal _TotalV = 0;
            decimal _MontoPayback = 0;
            var ExistePagoPayback = (jsonpago.FormaDePago.Equals("Monedero Payback")) ? true : false;
            bool SuccesAcumulation = false;
            bool ErrorAcomulation = false;
            string MessageRedemption = string.Empty;
            string Idabono = "";
            AddPay PaymentFault = new AddPay();
            PaybackController payback = new PaybackController();
            AdminPayback AdminR = new AdminPayback();
            ListRquestPurchase requestR = new ListRquestPurchase();
            string url = string.Empty;
            string JsonResult = string.Empty;
            string IdVenta = jsonpago.Id;
            String archivo_logpago = AppDomain.CurrentDomain.BaseDirectory + @"logErrorPayback\LogWebFranquicias_" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace("T", "_").Replace("-", "_").Substring(0, 10) + ".txt";
            decimal TotalPuntosPayback = 0;
            decimal PuntosPaybackRedimidos = 0;
            connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            try
            {
                if (Convert.ToDouble(jsonpago.Monto) > Convert.ToDouble(jsonpago.PorPagar))
                {
                    PaymentFault.payresponse = false;
                    PaymentFault.Monedero = "Insertarste un valor mayor al monto por pagar verifica tus datos";
                    JsonResult = JsonConvert.SerializeObject(PaymentFault);
                    return Json(JsonResult);
                }

                if (ExistePagoPayback)
                {
                    if (Monederos(jsonpago, PaymentFault, connstringWEB))
                    {
                        //nothing
                    }
                    else
                    {

                        PaymentFault.payresponse = false;
                        JsonResult = JsonConvert.SerializeObject(PaymentFault);
                        return Json(JsonResult);
                    }
                    _TotalV = Convert.ToDecimal(jsonpago.Total);
                    _MontoPayback = Convert.ToDecimal(jsonpago.Monto);
                    AdminR = payback.GetInfoAdminPayback1("Redemption", "Redencion");
                    requestR = payback.QueryDatesPurch1abono(Session["IDSTORE"].ToString(), "", AdminR.TypeWs);
                    var pagoPayback = payback.ProcessDirectRedemptionEvent(jsonpago.Monedero, AdminR.AdminUser, AdminR.Password, jsonpago.SecretPassword, AdminR.PartnerShortName, requestR.branchShortName, requestR.Fecha, requestR.ReceipNumber, payback.AmountToPoints1(_MontoPayback.ToString(), AdminR.ValueInPoints), _MontoPayback, _TotalV);
                    if (pagoPayback.FaultMessage == null)
                    {
                        SuccesAcumulation = true;//Redencion exitosa
                        TotalPuntosPayback = payback.PointsToAmount(pagoPayback.AccountBalanceDetails.TotalPointsAmount, AdminR.ValueInPoints);
                        PuntosPaybackRedimidos = payback.PointsToAmount(pagoPayback.Transactions.FirstOrDefault().TotalPoints.LoyaltyAmount, AdminR.ValueInPoints);

                    }
                    else
                    {
                        //Redencion erronea
                        ErrorAcomulation = true;
                        MessageRedemption = payback.GetDescriptionErrorPayback(pagoPayback.FaultMessage.Code);//llena mensaje con error a mostrar en pantalla
                        payback.anade_linea_archivo2(archivo_logpago, "Error en Redencion : " + MessageRedemption + " Prefijo : " + requestR.branchShortName + " ReceipNumber : " + requestR.ReceipNumber + " fecha :" + requestR.Fecha);
                        PaymentFault.payresponse = false;
                        PaymentFault.Monedero = "Error en Redencion : " + MessageRedemption + " Prefijo : " + requestR.branchShortName + " ReceipNumber : " + requestR.ReceipNumber + " fecha :" + requestR.Fecha;
                        JsonResult = JsonConvert.SerializeObject(PaymentFault);
                    }
                }

                //Registra acomulacion

                if (ErrorAcomulation == false)
                {
                    url = "api/AddPayF";
                    HttpResponseMessage response = _AbonosFBL.ReadAsStringAsyncAPI(url, jsonpago);
                    response.EnsureSuccessStatusCode();
                    var result = response.Content.ReadAsAsync<AddPay>().Result;
                    if (result.payresponse)
                    {
                        Idabono = result.Id;
                        if (SuccesAcumulation)
                        {
                            var details = new List<DetailsAcumulationRequest>();
                            var obj = _AbonosFBL.CreateObjRegisterPaybackSaleF(IdVenta, requestR.branchShortName, requestR.Fecha, payback.AmountToPoints1(_MontoPayback.ToString(), AdminR.ValueInPoints), _TotalV, payback.AmountToPoints1(_MontoPayback.ToString(), AdminR.ValueInPoints), jsonpago.Monedero, jsonpago.SecretPassword, AdminR.PartnerShortName, requestR.ReceipNumber, "", "", "RedencionVenta", details, true, "", TotalPuntosPayback, 0, PuntosPaybackRedimidos, Idabono);//construye objeto a registrar en BD
                            payback.RegisterPaybackSales(obj); //>Registra la transaccion en BD
                        }
                        //Acomulacion de puntos
                        if (Monederos(jsonpago, PaymentFault, connstringWEB))
                        {
                            //nothing
                        }
                        else
                        {
                            PaymentFault.payresponse = false;
                            JsonResult = JsonConvert.SerializeObject(PaymentFault);
                            return Json(JsonResult);
                        }
                        var acomulapuntos = acomulation(jsonpago, payback, archivo_logpago, PaymentFault, Idabono);
                        result.Monedero = PaymentFault.Monedero;
                        JsonResult = JsonConvert.SerializeObject(result);
                    }
                }

                return Json(JsonResult);

            }
            catch (Exception ex)
            {

            }
            return Json(null);
        }
        public bool acomulation(AddPay jsonpago, PaybackController payback, String archivo_logpago, AddPay PaymentFault, string Idabono)             //Registra acomulacion
        {
            decimal _TotalV = 0;
            var acomulacion = false;
            string MessageErrorPBKAcumulation = "";
            string SqueryTrans = "";
            int IDVenta = Convert.ToInt32(jsonpago.Id);
            var objR = new RequestRegisterPaybackSale();
            DBMaster oDB = new DBMaster();
            connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            DataTable dtTrans = new DataTable();
            string url = string.Empty;
            string idtransaction = "";
            List<DetailsAcumulationRequest> resultAc = new List<DetailsAcumulationRequest>();
            try
            {

                SqueryTrans = "select * from Dat_TransactionPayback where idVenta=" + jsonpago.Id + " and TypeTransaction = 'AcumulacionVenta'";
                dtTrans = oDB.EjecutaQry_Tabla(SqueryTrans, CommandType.Text, "IdTransaction", connstringWEB);
                if (dtTrans.Rows.Count > 0)
                {
                    idtransaction = dtTrans.Rows[0][0].ToString();
                    SqueryTrans = "";
                    SqueryTrans = "select * from Dat_DetailsTransactionPayback where idTransPK=" + idtransaction;
                    dtTrans.Clear();
                    dtTrans = oDB.EjecutaQry_Tabla(SqueryTrans, CommandType.Text, "IdTransaction", connstringWEB);
                    if (dtTrans != null)
                    {
                        foreach (DataRow rows in dtTrans.Rows)
                        {

                            var ls = new DetailsAcumulationRequest();
                            ls.ArticleEanCode = rows["ArticleEanCode"].ToString();
                            ls.PartnerProductCategoryCode = rows["PartnerProductGroupCode"].ToString();
                            ls.PartnerProductGroupCode = rows["PartnerProductCategoryCode"].ToString();
                            ls.QuantityUnitCode = rows["Quantity"].ToString();
                            ls.SingleTurnoverAmount = Convert.ToDecimal(rows["TotalTurnoverAmount"]);
                            ls.TotalRewardableAmount = Convert.ToDecimal(rows["TotalRewardableAmount"]);
                            resultAc.Add(ls);

                        }
                    }
                }
                else
                {
                    SqueryTrans = "select  distinct AR.ArticuloSBO,VD.Cantidad, VD.TotalLinea from VentasDetalle VD Inner join Articulos AR on AR.IDArticulo=VD.IDArticulo  where IDVenta=" + jsonpago.Id;
                    List<ArrayArticulos> ListaItemsAcumulation = new List<ArrayArticulos>();
                    var result = oDB.EjecutaQry_Tabla(SqueryTrans, CommandType.Text, "Detallesventa", connstringWEB);
                    if (result != null)
                    {

                        foreach (DataRow rows in result.Rows)
                        {
                            var Saledetails = new ArrayArticulos();
                            Saledetails.Linea = rows["ArticuloSBO"].ToString();
                            Saledetails.Cantidad = rows["Cantidad"].ToString();
                            Saledetails.Total = rows["TotalLinea"].ToString();
                            ListaItemsAcumulation.Add(Saledetails);

                        }
                        url = "api/GetDetailsPayback";
                        HttpResponseMessage responseAc = _AbonosFBL.ReadAsStringAsyncAPI(url, ListaItemsAcumulation);
                        responseAc.EnsureSuccessStatusCode();
                        resultAc = responseAc.Content.ReadAsAsync<List<DetailsAcumulationRequest>>().Result;//obtiene los detalles a enviar
                    }
                }

                if (!string.IsNullOrEmpty(jsonpago.Monedero))// si cumple con las condiciones entra y acumula puntos
                {


                    var MontoAcumulacion = Convert.ToDecimal(jsonpago.Total); ;// * (decimal)0.01;
                    var AdminAc = payback.GetInfoAdminPayback1("Acumulation", "Acumulacion");
                    var request1 = payback.QueryDatesPurch1(Session["IDSTORE"].ToString(), jsonpago.Id, AdminAc.TypeWs);
                    _TotalV = Convert.ToDecimal(jsonpago.Total);
                    double Porpagar = Convert.ToDouble(jsonpago.PorPagar);
                    double montoAbono = Convert.ToDouble(jsonpago.Monto);
                    double Saldo = Porpagar - montoAbono;
                    if (Saldo <= 0.0)
                    {
                        var Acumulation = payback.ProcessPurchaseAndPromotionEvent(jsonpago.Monedero, AdminAc.AdminUser, AdminAc.Password, AdminAc.PartnerShortName, request1.branchShortName, request1.Fecha, request1.ReceipNumber, MontoAcumulacion, MontoAcumulacion, MontoAcumulacion, resultAc);
                        if (Acumulation.FaultMessage == null)
                        {
                            objR = _AbonosFBL.CreateObjRegisterPaybackSaleF(jsonpago.Id, request1.branchShortName, request1.Fecha, payback.AmountToPoints1(MontoAcumulacion.ToString(), AdminAc.ValueInPoints), _TotalV, payback.AmountToPoints1(MontoAcumulacion.ToString(), AdminAc.ValueInPoints), jsonpago.Monedero, jsonpago.SecretPassword, AdminAc.PartnerShortName, request1.ReceipNumber, "", "", "AcumulacionVenta", resultAc, true, "", payback.PointsToAmount(Acumulation.AccountBalanceDetails.TotalPointsAmount, AdminAc.ValueInPoints), payback.PointsToAmount(Acumulation.Transactions.FirstOrDefault().TotalPoints.LoyaltyAmount, AdminAc.ValueInPoints), 0, Idabono);
                            acomulacion = true;
                        }
                        else
                        {
                            MessageErrorPBKAcumulation = payback.GetDescriptionErrorPayback(Acumulation.FaultMessage.Code);
                            payback.anade_linea_archivo2(archivo_logpago, "Error en Acomulacion : " + MessageErrorPBKAcumulation + " Prefijo : " + request1.branchShortName + " ReceipNumber : " + request1.ReceipNumber + " fecha :" + request1.Fecha);
                            objR = _AbonosFBL.CreateObjRegisterPaybackSaleF(jsonpago.Id, request1.branchShortName, request1.Fecha, payback.AmountToPoints1(MontoAcumulacion.ToString(), AdminAc.ValueInPoints), _TotalV, payback.AmountToPoints1(MontoAcumulacion.ToString(), AdminAc.ValueInPoints), jsonpago.Monedero, jsonpago.SecretPassword, AdminAc.PartnerShortName, request1.ReceipNumber, "", "", "AcumulacionVenta", resultAc, false, MessageErrorPBKAcumulation, 0, 0, 0, Idabono);
                            acomulacion = false;
                            PaymentFault.payresponse = true; //si se genero el abono mostrar mensaje de error en respuesta abono
                            PaymentFault.Monedero = "Error en Acomulacion : " + MessageErrorPBKAcumulation + " Prefijo : " + request1.branchShortName + " ReceipNumber : " + request1.ReceipNumber + " fecha :" + request1.Fecha;
                        }
                        payback.RegisterPaybackSales(objR);

                    }
                    else
                    {
                        objR = _AbonosFBL.CreateObjRegisterPaybackSaleF(jsonpago.Id, request1.branchShortName, request1.Fecha, 0, _TotalV, 0, jsonpago.Monedero, jsonpago.SecretPassword, AdminAc.PartnerShortName, "", "", "", "AcumulacionVenta", resultAc, false, "", 0, 0, 0, Idabono);
                        payback.RegisterPaybackSales(objR);
                        acomulacion = false;
                    }

                    return acomulacion;

                }

                else { return acomulacion; }
            }
            catch (Exception ex)
            {
                acomulacion = false;
            }
            return acomulacion;
        }

        public bool Monederos(AddPay jsonpago, AddPay PaymentFault, string connstringWEB)             //Valida Monederos
        {
            string SQuery = string.Empty;
            string Monedero = "";
            bool Mismomonedero = true;
            DataTable dtvalida;
            Utilities.DBMaster oDB = new Utilities.DBMaster();
            SQuery = "select Monedero from Dat_TransactionPayback where idVenta =" + jsonpago.Id + " and TypeTransaction = 'AcumulacionVenta'";
            dtvalida = oDB.EjecutaQry_Tabla(SQuery.ToString(), CommandType.Text, "Existeacomulacion", connstringWEB);
            if (dtvalida.Rows.Count >= 1)
            {
                foreach (DataRow row in dtvalida.Rows)
                {
                    Monedero = row["Monedero"].ToString();
                }
            }
            else if (dtvalida.Rows.Count == 0)
            {
                return true;
            }
            if (!jsonpago.Monedero.Equals(Monedero))
            {
                PaymentFault.Monedero = "El monedero debe ser el mismo para acomular y redimir puntos payback";
                return false;
            }
            else
            {
                return true;
            }
        }

        [HttpPost]
        public JsonResult GetFoliosF(string idcliente, string date, string day)
        {
            string jsonparams = string.Empty;
            var idstore = Session["IDSTORE"].ToString();
            var Jsonquery = new
            {
                date = date,
                idstore = idstore,
                idcliente = idcliente,
                day = day
            };
            jsonparams = JsonConvert.SerializeObject(Jsonquery);
            string JsonResult = string.Empty;
            try
            {
                var result = _AbonosFBL.GetFoliosF(jsonparams);
                if (result != null) { JsonResult = JsonConvert.SerializeObject(result); }
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetDaysF(string idcliente, string date)
        {
            string jsonparams = string.Empty;
            var idstore = Session["IDSTORE"].ToString();
            var Jsonquery = new
            {
                date = date,
                idstore = idstore,
                idcliente = idcliente
            };
            jsonparams = JsonConvert.SerializeObject(Jsonquery);
            string JsonResult = string.Empty;
            try
            {
                var result = _AbonosFBL.GetDaysF(jsonparams);
                if (result != null) { JsonResult = JsonConvert.SerializeObject(result); }
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetPeriodsF(string idcliente)
        {
            string JsonResult = string.Empty;
            try
            {
                var result = _AbonosFBL.GetPeriodsF(idcliente);
                if (result != null) { JsonResult = JsonConvert.SerializeObject(result); }
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}