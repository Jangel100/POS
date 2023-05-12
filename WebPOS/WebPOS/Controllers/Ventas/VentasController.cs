using BL.Configuracion;
using BL.Interface;
using Entities;
using Entities.Models.Payback;
using Entities.Models.Ventas;
using Entities.viewsModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using WebPOS.Security;
using WebPOS.Utilities;
using System.IO;
using Entities.viewsModels.Facturacion;
using System.Net;
using Entities.Models.Configuracion;
using System.Text;
using System.Threading;
using APIPOSS.Utilities;
using System.Globalization;

namespace WebPOS.Controllers.Ventas
{
    [CustomAuthorize(Roles = "AG, US, AF, LC, VL, AG2, AG3, AG4, TO")]
    public class VentasController : Controller
    {
        private string connstringWEB;
        private string connstringSAP;
        readonly IVentasBL _VentasBL;
        returnventa infoventas = new returnventa();
        private String _token = ConfigurationManager.AppSettings["SecretWebToken"].ToString();
        private string stock;
        private string DireccionFis;
        public string IDPRINTVENTA = "";
        public string DirPDF;
        public Boolean btnPDF = false;
        // GET: Ventas
        public VentasController(IVentasBL ventasBL)
        {
            _VentasBL = ventasBL;
        }
        public VentasController()
        {
            _VentasBL = new VentasBL();
        }
        public ActionResult Index()
        {
            ArticulosBaseView ArticulosBaseView;
            try
            {
                ViewBag.Idstore = Session["IDSTORE"].ToString();
                ViewBag.Idusuario = Session["AdminUserID"].ToString();
                ViewBag.WhsID = Session["WHSID"].ToString();
                ArticulosBaseView = Crearventa();
                return View("CrearVenta", ArticulosBaseView);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult capturarpago([Bind(Include = "Id,FormaPago,MontoPagado,NoCuenta,TipoTarjeta,MSISub")] Pago pago)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //db.Ventas.Add(pago);
        //        //db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View("Crearventa", pago);
        //}
        public ArticulosBaseView Crearventa()
        {
            string JsonResult = string.Empty;
            ArticulosBaseView ArticulosBaseView;
            List<ListArticuloView> ListArt;
            List<ListLineaView> ListLinea = new List<ListLineaView>();
            List<ListMedidaView> ListMedida = new List<ListMedidaView>();
            List<ListModeloView> ListModelo;
            List<ListListOfPriceView> ListPrice;
            ListRadioButton listB = new ListRadioButton();
            List<PromocionDosPorUno> ListPromocionDosPorUno;
            List<TipoPromocionView> ListTipoPromocion = new List<TipoPromocionView>();

            ///LLena tipo promocion
            var List1 = new TipoPromocionView();
            List1.Code = "Ninguno";
            List1.Name = "Ninguno";
            //var List2 = new TipoPromocionView();
            //List2.Code = "Juego";
            //List2.Name = "Juego";
            var List3 = new TipoPromocionView();
            List3.Code = "DosPorUno";
            List3.Name = "2 X 1";
            ListTipoPromocion.Add(List1);
            //ListTipoPromocion.Add(List2);
            ListTipoPromocion.Add(List3);
            listB.CheckLine = true;
            try
            {
                ListArt = _VentasBL.GetArticulos();
                ListModelo = _VentasBL.GetModelos("Linea");
                ListPrice = _VentasBL.GetPrice(Session["IDSTORE"].ToString(), Session["SAPDB"] == null ? "0" : Session["SAPDB"].ToString());
                ListPromocionDosPorUno = _VentasBL.GetInfoDosPorUno();
                ArticulosBaseView = new ArticulosBaseView()
                {
                    ListArticulos = ListArt,
                    ListLinea = ListLinea,
                    ListMedida = ListMedida,
                    ListModelo = ListModelo,
                    ListOfPrice = ListPrice,
                    DosPorUno = ListPromocionDosPorUno,
                    TipoPromocion = ListTipoPromocion,
                    CantidadBodega = "",
                    CantidadTienda = "",
                    CantidadBodegaDisp = 0,
                    CantidadTiendaDisp = 0,
                    DescriptionJuego = "",


                    //Descuento = 0,
                    //IVA = _VentasBL.GetIva(Session["IDSTORE"].ToString()),
                    ListRadioButton = listB
                    //PrecioUnitario = 0,
                    //Total = 0,
                };
                return ArticulosBaseView;
            }
            catch (Exception ex) { return null; }
        }
        public JsonResult GetSelectedMedida(string code)
        {
            string JsonResult = string.Empty;
            try
            {
                JsonResult = JsonConvert.SerializeObject(_VentasBL.GetMedida(code));
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public JsonResult GetSelectedModels(string articulo, string modelo, string medida)
        {
            string jsonparams = string.Empty;
            var Jsonquery = new
            {
                articulo = articulo,
                modelo = modelo,
                medida = medida
            };
            jsonparams = JsonConvert.SerializeObject(Jsonquery);
            string JsonResult = string.Empty;
            try
            {
                var result = _VentasBL.GetModelsSelected(jsonparams);
                if (result != null) { JsonResult = JsonConvert.SerializeObject(result); }
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetSelectedLinea(string code)
        {
            //var json1 = JsonConvert.DeserializeObject<ListRadioButton>(JsonRes);
            string JsonResult = string.Empty;
            try
            {
                var result = _VentasBL.GetListLinea(code);
                if (result != null) { JsonResult = JsonConvert.SerializeObject(result); }
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public JsonResult GetExistencias(string Parametrs)
        {
            var jsondata = new
            {
                IdStore = Session["IDSTORE"].ToString(),
                ItemCode = Parametrs
            };
            string json_data = JsonConvert.SerializeObject(jsondata);
            string JsonResult = string.Empty;
            try
            {
                var result = _VentasBL.GetExistencias(json_data);
                if (result != null) { JsonResult = JsonConvert.SerializeObject(result); }
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public JsonResult GetSelectChanchePrice(string JsonChangePrice)
        {
            //string ItemCode, string IdList, string descuento,string cantidad, string Origen 
            var RJsonChangePrice = JsonConvert.DeserializeObject<RequestChangePrice>(JsonChangePrice);
            string JsonResult = string.Empty;
            string json_UnitPrice = string.Empty;
            var Icon = RJsonChangePrice.IdList.Length > 2 ? "" : RJsonChangePrice.IdList;
            var JsonUnitPrice = new
            {
                ItemCode = RJsonChangePrice.ItemCode,
                IdList = RJsonChangePrice.IdList,
                Ion = Icon
            };
            json_UnitPrice = JsonConvert.SerializeObject(JsonUnitPrice);
            try
            {
                var Price = _VentasBL.GetUnitPrice(json_UnitPrice);
                var Iva = _VentasBL.GetIva(Session["IDSTORE"].ToString());

                var ResultChangeView = _VentasBL.GetChangePrice(Iva, Price, RJsonChangePrice.descuento, Convert.ToInt32(RJsonChangePrice.cantidad));
                JsonResult = JsonConvert.SerializeObject(ResultChangeView);
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public JsonResult GetSelectedJgo(string JsonPrice, string Name, string TipoPromocion)
        {
            //string ItemCode, string IdList, string descuento,string cantidad, string Origen 
            var ListJgoResponseview = new List<ResponseJgoView>();
            var RJsonChangePrice = JsonConvert.DeserializeObject<RequestChangePrice>(JsonPrice);
            var Promocion = JsonConvert.DeserializeObject<PromocionDosPorUno>(TipoPromocion);
            string JsonResult = string.Empty;
            string json_UnitPrice = string.Empty;
            var Icon = RJsonChangePrice.IdList.Length > 2 ? "" : RJsonChangePrice.IdList;
            var JsonUnitPrice = new
            {
                ItemCode = RJsonChangePrice.ItemCode,
                IdList = RJsonChangePrice.IdList,
                Ion = Icon
            };
            json_UnitPrice = JsonConvert.SerializeObject(JsonUnitPrice);
            try
            {
                var Price = _VentasBL.GetUnitPrice(json_UnitPrice);
                var Iva = _VentasBL.GetIva(Session["IDSTORE"].ToString());

                var ResultChangeView = _VentasBL.GetChangePrice(Iva, Price, RJsonChangePrice.descuento, Convert.ToInt32(RJsonChangePrice.cantidad));

                var ListJgo = _VentasBL.GetListJgo(ResultChangeView, RJsonChangePrice.IdList, Name, RJsonChangePrice.Origen, RJsonChangePrice.cantidad, RJsonChangePrice.ItemCode);
                //if (ListJgo != null) { ListJgoResponseview.Add(ListJgo); }
                if (ListJgo != null)
                {
                    ListJgoResponseview.Add(ListJgo);
                    if (!RJsonChangePrice.Juego)//valida que no sea un juego
                    {
                        if (Promocion != null)
                        {
                            if (Promocion.TipoPromocion.Equals("DosPorUno"))//Verifica si tiene promocion 2 x 1
                            {
                                double priceUnit;
                                double priceTotal;
                                bool AplicaCeros = true;
                                var Unit_Price2X1 = new
                                {
                                    ItemCode = Promocion.ItemCode,
                                    IdList = RJsonChangePrice.IdList,
                                    Ion = Icon
                                };
                                json_UnitPrice = JsonConvert.SerializeObject(Unit_Price2X1);
                                var Price2X1 = _VentasBL.GetUnitPrice(json_UnitPrice);
                                //Crea Objeto final y lo agrega a la lista
                                var DosChangeView = _VentasBL.GetChangePrice(Iva, Price2X1, "", Convert.ToInt32(1));

                                var DosListJgo = _VentasBL.GetListJgo(DosChangeView, RJsonChangePrice.IdList, Promocion.ItemName, RJsonChangePrice.Origen, "1", Promocion.ItemCode);
                                if (ListJgo.ItemCode.Equals(Promocion.ItemCode))
                                {
                                    priceUnit = Convert.ToDouble(ListJgo.PrecioUnitario.Replace("$", "")) - 0.01;
                                    priceTotal = Convert.ToDouble(ListJgo.Total.Replace("$", "")) - 0.01;
                                    ListJgoResponseview.FirstOrDefault().PrecioUnitario = "$" + priceUnit;
                                    ListJgoResponseview.FirstOrDefault().Subtotal = "$" + priceUnit;
                                    ListJgoResponseview.FirstOrDefault().Total = "$" + priceTotal;
                                }
                                else
                                {

                                    if (Price > Price2X1)
                                    {
                                        priceUnit = Convert.ToDouble(ListJgo.PrecioUnitario.Replace("$", "")) - 0.01;
                                        priceTotal = Convert.ToDouble(ListJgo.Total.Replace("$", "")) - 0.01;
                                        ListJgoResponseview.FirstOrDefault().PrecioUnitario = "$" + priceUnit;
                                        ListJgoResponseview.FirstOrDefault().Subtotal = "$" + priceUnit;
                                        ListJgoResponseview.FirstOrDefault().Total = "$" + priceTotal;
                                    }
                                    else
                                    {
                                        priceUnit = Convert.ToDouble(DosListJgo.PrecioUnitario.Replace("$", "")) - 0.01;
                                        priceTotal = Convert.ToDouble(DosListJgo.Total.Replace("$", "")) - 0.01;
                                        ListJgoResponseview.FirstOrDefault().PrecioUnitario = "$" + 0.01;
                                        ListJgoResponseview.FirstOrDefault().Subtotal = "$" + 0.01;
                                        ListJgoResponseview.FirstOrDefault().Total = "$" + 0.01;
                                        ListJgoResponseview.FirstOrDefault().IVA = "$0.00";
                                        AplicaCeros = false;
                                    }
                                }


                                var Obj2X1 = new ResponseJgoView
                                {
                                    Id = "2",
                                    Juego = "2X1",
                                    Cantidad = "1",
                                    CBodega = string.IsNullOrEmpty(ListJgo.CBodega) ? "0" : ListJgo.CBodega,
                                    CTienda = string.IsNullOrEmpty(ListJgo.CTienda) ? "0" : ListJgo.CTienda,
                                    ItemCode = Promocion.ItemCode,
                                    Descuento = string.Format("{0:c}", 0),
                                    IVA = AplicaCeros ? "$0.00" : DosListJgo.IVA,
                                    Lista = RJsonChangePrice.IdList,
                                    Modelo = Promocion.ItemName,
                                    Subtotal = AplicaCeros ? "$0.01" : DosListJgo.Subtotal,
                                    PrecioUnitario = AplicaCeros ? "$0.01" : DosListJgo.PrecioUnitario,
                                    Total = AplicaCeros ? "0.01" : DosListJgo.Total
                                };
                                ListJgoResponseview.Add(Obj2X1);
                            }
                        }

                        JsonResult = JsonConvert.SerializeObject(ListJgoResponseview);
                        return Json(JsonResult);
                    }
                }
                if (ListJgoResponseview.Count > 0)
                {
                    var Unit_PriceB = new
                    {
                        ItemCode = RJsonChangePrice.ItemCode,
                        IdList = RJsonChangePrice.IdList,
                        Ion = Icon
                    };
                    json_UnitPrice = JsonConvert.SerializeObject(Unit_PriceB);
                    var PriceBox = _VentasBL.GetUnitPrice(json_UnitPrice);
                    var JsonCombo = new
                    {
                        modelo = RJsonChangePrice.ItemCode,
                        idStore = Session["IDSTORE"].ToString(),
                        Iva = Iva,
                        cantidad = RJsonChangePrice.cantidad,
                        Origen = RJsonChangePrice.Origen,
                        Lista = RJsonChangePrice.IdList,
                        Price = PriceBox,
                        Descuento = ResultChangeView.Descuento,
                        PriceUnit = ListJgo.PrecioUnitario,
                        Total = ListJgo.Total
                    };
                    var json_UnitJgo = JsonConvert.SerializeObject(JsonCombo);
                    var ResponseBox = _VentasBL.GetBox(json_UnitJgo);
                    if (ResponseBox != null)
                    {
                        //Ajuste de Saldo                        
                        if (!string.IsNullOrEmpty(ResponseBox.AjusteTotal))
                        {
                            ListJgoResponseview.FirstOrDefault().Subtotal = ResponseBox.AjustePriceSub;
                            ListJgoResponseview.FirstOrDefault().Total = ResponseBox.AjusteTotal;
                            ListJgoResponseview.FirstOrDefault().PrecioUnitario = ResponseBox.AjustePriceUnit;
                        }
                        ListJgoResponseview.Add(ResponseBox);
                    }
                }
                JsonResult = JsonConvert.SerializeObject(ListJgoResponseview);
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public JsonResult GetSelectOnchangeModelo(string tipo)
        {
            string JsonResult = string.Empty;
            try
            {
                var result = _VentasBL.GetModelos(tipo);
                JsonResult = JsonConvert.SerializeObject(result);
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public JsonResult BtnCapturar(string ParCaptured)
        {
            // var obj = JsonConvert.DeserializeObject<ArticulosBaseView>(ParCaptured);

            TableView CapturedItems = new TableView();
            string JsonResult = string.Empty;

            try
            {
                //var result = _VentasBL.get

                JsonResult = JsonConvert.SerializeObject(CapturedItems);
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult AddSale(string Paymentscap)
        {

            decimal _TotalV = 0;
            decimal _MontoPayback = 0;
            var json1sale = JsonConvert.DeserializeObject<AddSale>(Paymentscap);
            string url = string.Empty;
            string urlAcumulation = string.Empty;
            string JsonResult = string.Empty;
            bool SuccesRedemption = false;
            bool SuccesAcumulation = false;
            string MessageAcumulacion = string.Empty;
            string MessageRedemption = string.Empty;
            decimal TotalPuntosPayback = 0;
            decimal PuntosPaybackRedimidos = 0;
            var payback = new PaybackController();
            AdminPayback AdminR = new AdminPayback();





            if (json1sale.MetodoPago33.Equals("Seleccione metodo de pago") || json1sale.UsoCFDI33.Equals("Seleccione uso CFDI") || json1sale.UsoCFDI33.Equals("-1") || json1sale.FormaPago33.Equals("Seleccione forma de pago"))
            {
                AddSale result = new AddSale();
                result.MessageErrorPayback = "Algun campo esta vacio comprube que sus datos esten correctamente por favor";
                JsonResult = JsonConvert.SerializeObject(result);
                return Json(JsonResult);

            }
            ListRquestPurchase requestR = new ListRquestPurchase();
            String archivo_log = AppDomain.CurrentDomain.BaseDirectory + @"logErrorPayback\LogTimbrado_" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace("T", "_").Replace("-", "_").Substring(0, 10) + ".txt";
            String LogWeb = AppDomain.CurrentDomain.BaseDirectory + @"logErrorPayback\LogWeb_" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace("T", "_").Replace("-", "_").Substring(0, 10) + ".txt";
            payback.anade_linea_archivo2(LogWeb, "Crear venta nueva tienda: " + json1sale.idstore);
            try
            {

                var ExistePagoPayback = json1sale.ArrayPagos.Any(x => x.Formapago.Equals("Monedero Payback"));//verifica si existe un pago con monedero Payback                
                var SumaPagos = json1sale.ArrayPagos.Sum(x => Convert.ToDecimal(x.Monto.Replace("$", "")));//Verifica si se paga el monto total y consume Ws Acumulacion
                _TotalV = json1sale.ArrayArticulos.Sum(x => Convert.ToDecimal(x.Total.Replace("$", "")));//calcula el total de los articulos
                #region WsRedencion Payback
                if (ExistePagoPayback)
                {

                    _MontoPayback = json1sale.ArrayPagos.Where(x => x.Formapago.Equals("Monedero Payback")).Sum(x => Convert.ToDecimal(x.Monto));

                    AdminR = payback.GetInfoAdminPayback1("Redemption", "Redencion");
                    payback.anade_linea_archivo2(archivo_log, "IDSTORE :" + Session["IDSTORE"].ToString() + " TypeWs :" + AdminR.TypeWs + " Password: " + AdminR.Password + " ValueInPoints: " + AdminR.ValueInPoints + " AdminUser :" + AdminR.AdminUser + " PartnerShortName: " + AdminR.PartnerShortName);
                    requestR = payback.QueryDatesPurch1(Session["IDSTORE"].ToString(), "", AdminR.TypeWs);
                    payback.anade_linea_archivo2(archivo_log, "Consulta de puntos: " + MessageRedemption + " Prefijo : " + requestR.branchShortName + " ReceipNumber : " + requestR.ReceipNumber + " fecha :" + requestR.Fecha + " total puntos : " + _MontoPayback);
                    var pagoPayback = payback.ProcessDirectRedemptionEvent(json1sale.Monedero, AdminR.AdminUser, AdminR.Password, json1sale.Nip, AdminR.PartnerShortName, requestR.branchShortName, requestR.Fecha, requestR.ReceipNumber, payback.AmountToPoints1(_MontoPayback.ToString(), AdminR.ValueInPoints), _MontoPayback, _TotalV);
                    if (pagoPayback.FaultMessage == null)
                    {
                        payback.anade_linea_archivo2(archivo_log, "Redencion exitosa: " + MessageRedemption + " Prefijo : " + requestR.branchShortName + " ReceipNumber : " + requestR.ReceipNumber + " fecha :" + requestR.Fecha);
                        SuccesRedemption = true;//Redencion exitosa
                        TotalPuntosPayback = payback.PointsToAmount(pagoPayback.AccountBalanceDetails.TotalPointsAmount, AdminR.ValueInPoints);
                        PuntosPaybackRedimidos = payback.PointsToAmount(pagoPayback.Transactions.FirstOrDefault().TotalPoints.LoyaltyAmount, AdminR.ValueInPoints);
                    }
                    else
                    {
                        //Redencion erronea
                        MessageRedemption = payback.GetDescriptionErrorPayback(pagoPayback.FaultMessage.Code);//llena mensaje con error a mostrar en pantalla
                        payback.anade_linea_archivo2(archivo_log, "Error en Redencion : " + MessageRedemption + " Prefijo : " + requestR.branchShortName + " ReceipNumber : " + requestR.ReceipNumber + " fecha :" + requestR.Fecha);
                    }

                }
                #endregion

                if (ExistePagoPayback && SuccesRedemption || !ExistePagoPayback)
                {
                    json1sale.webToken = _token;
                    Metodopago33Pagos(json1sale);
                    ParcialidadPagos(json1sale);
                    //url = "api/AddSale";
                    //HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, json1sale);
                    //response.EnsureSuccessStatusCode();
                    //var result = response.Content.ReadAsAsync<AddSale>().Result;
                    var result = AddSaleInsert(json1sale);
                    if (SuccesRedemption)
                    {
                        var details = new List<DetailsAcumulationRequest>();
                        var obj = _VentasBL.CreateObjRegisterPaybackSale(result.Idventa, requestR.branchShortName, requestR.Fecha, payback.AmountToPoints1(_MontoPayback.ToString(), AdminR.ValueInPoints), _TotalV, payback.AmountToPoints1(_MontoPayback.ToString(), AdminR.ValueInPoints), json1sale.Monedero, json1sale.Nip, AdminR.PartnerShortName, requestR.ReceipNumber, "", "", "RedencionVenta", details, true, "", TotalPuntosPayback, 0, PuntosPaybackRedimidos, SuccesRedemption, SuccesAcumulation);//construye objeto a registrar en BD
                        payback.RegisterPaybackSales(obj); //>Registra la transaccion en BD
                    }
                    if (result.saleresponse)
                    {

                        var objR = new RequestRegisterPaybackSale();
                        if (!string.IsNullOrEmpty(json1sale.Monedero))// si cumple con las condiciones entra y acumula puntos
                        {
                            var MontoAcumulacion = _TotalV;// * (decimal)0.01;
                            var AdminAc = payback.GetInfoAdminPayback1("Acumulation", "Acumulacion");
                            var request1 = payback.QueryDatesPurch1(Session["IDSTORE"].ToString(), result.Idventa, AdminAc.TypeWs);

                            if (json1sale.ArrayArticulos.Count > 0) //valida la cantidad de los detalles
                            {
                                url = "api/GetDetailsPayback";
                                HttpResponseMessage responseAc = _VentasBL.ReadAsStringAsyncAPI(url, json1sale.ArrayArticulos);
                                responseAc.EnsureSuccessStatusCode();
                                var resultAc = responseAc.Content.ReadAsAsync<List<DetailsAcumulationRequest>>().Result;//obtiene los detalles a enviar

                                if (resultAc != null)//valida si la respuesta es diferente de null
                                {
                                    if (SumaPagos >= _TotalV)
                                    {
                                        var Acumulation = payback.ProcessPurchaseAndPromotionEvent(json1sale.Monedero, AdminAc.AdminUser, AdminAc.Password, AdminAc.PartnerShortName, request1.branchShortName, request1.Fecha, request1.ReceipNumber, MontoAcumulacion, MontoAcumulacion, MontoAcumulacion, resultAc);

                                        if (Acumulation.FaultMessage == null)
                                        {
                                            SuccesAcumulation = true;
                                            objR = _VentasBL.CreateObjRegisterPaybackSale(result.Idventa, request1.branchShortName, request1.Fecha, payback.AmountToPoints1(MontoAcumulacion.ToString(), AdminAc.ValueInPoints), _TotalV, payback.AmountToPoints1(MontoAcumulacion.ToString(), AdminAc.ValueInPoints), json1sale.Monedero, json1sale.Nip, AdminAc.PartnerShortName, request1.ReceipNumber, "", "", "AcumulacionVenta", resultAc, true, "", payback.PointsToAmount(Acumulation.AccountBalanceDetails.TotalPointsAmount, AdminAc.ValueInPoints), payback.PointsToAmount(Acumulation.Transactions.FirstOrDefault().TotalPoints.LoyaltyAmount, AdminAc.ValueInPoints), 0, SuccesRedemption, SuccesAcumulation);

                                        }
                                        else
                                        {
                                            result.MessageErrorPBKAcumulation = payback.GetDescriptionErrorPayback(Acumulation.FaultMessage.Code);
                                            //result.MessageErrorPBKAcumulation = "Error prueba, Notificar a Sistemas";
                                            objR = _VentasBL.CreateObjRegisterPaybackSale(result.Idventa, request1.branchShortName, request1.Fecha, payback.AmountToPoints1(MontoAcumulacion.ToString(), AdminAc.ValueInPoints), _TotalV, payback.AmountToPoints1(MontoAcumulacion.ToString(), AdminAc.ValueInPoints), json1sale.Monedero, json1sale.Nip, AdminAc.PartnerShortName, request1.ReceipNumber, "", "", "AcumulacionVenta", resultAc, false, result.MessageErrorPBKAcumulation, 0, 0, 0, false, false);

                                        }
                                        payback.RegisterPaybackSales(objR);
                                    }
                                    else
                                    {
                                        objR = _VentasBL.CreateObjRegisterPaybackSale(result.Idventa, request1.branchShortName, request1.Fecha, 0, _TotalV, 0, json1sale.Monedero, json1sale.Nip, AdminAc.PartnerShortName, "", "", "", "AcumulacionVenta", resultAc, false, "", 0, 0, 0, false, false);
                                        payback.RegisterPaybackSales(objR);
                                    }

                                }

                            }


                        }

                        JsonResult = JsonConvert.SerializeObject(result);
                        return Json(JsonResult);
                    }
                }
                else if (!string.IsNullOrEmpty(MessageRedemption))
                {
                    var result = new AddSale();
                    result.MessageErrorPayback = MessageRedemption;
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(JsonResult);
                }

                return Json(null);
            }
            catch (Exception ex)
            {
                payback.anade_linea_archivo2(archivo_log, ex.ToString());
                payback.anade_linea_archivo2(LogWeb, "Error en metodo AddSale: " + ex.ToString());
                if (ex.Message.Contains("Response status code does not indicate success: 404"))
                {

                    var IDventa = RetornaultimoIDventa(json1sale.idstore);
                    payback.anade_linea_archivo2(LogWeb, "Entra a metodo de respaldo para descargar pdf con el siguiente idventa: " + IDventa);
                    var result = new AddSale();
                    result.Idventa = IDventa;
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(JsonResult);
                }
            }
            return Json(null);
        }

        public JsonResult SearchModel(string Prefix, string tipo)
        {
            //// autocomplete
            string JsonResult = string.Empty;
            string squery = "";
            DBMaster oDB = new DBMaster();
            DataTable dt = new DataTable();
            var idstore = Session["IDSTORE"].ToString();
            connstringSAP = ConfigurationManager.ConnectionStrings["DBConnSAP"].ConnectionString;
            try
            {
                if (tipo.Equals("Linea"))
                {
                    squery = "select distinct ItemCode as code, ItemName as name from OITM where QryGroup7='Y' and (QryGroup43='N' or QryGroup42='Y') and ItemName LIKE '%" + Prefix + "%'";
                }
                else if (tipo.Equals("Descontinuados"))
                {
                    squery = "select distinct ItemCode as code, ItemName as name from OITM where QryGroup7='N' and ItemName LIKE '%" + Prefix + "%'";
                }

                dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "Modelos", connstringSAP);
                if (dt != null)
                {
                    var Modelos = (from DataRow rows in dt.Rows
                                   select new ModelosLista
                                   {
                                       code = (string)rows["code"].ToString(),
                                       name = (string)rows["name"].ToString()
                                   }).ToList();
                    //return Json(Modelos);
                    return new JsonResult { Data = Modelos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {

            }
            return Json(null);
        }
        public JsonResult GetSelectJuego(string modelo)
        {
            string JsonResult = string.Empty;
            try
            {
                var StoreSap = new
                {
                    Modelo = modelo,
                    IdStore = Session["IDSTORE"].ToString()
                };
                string json_data = JsonConvert.SerializeObject(StoreSap);

                //HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, json1sale);
                //response.EnsureSuccessStatusCode();
                //var result = response.Content.ReadAsAsync<ResponseJuego>().Result;
                //return Json(result);
                var result = _VentasBL.GetCombo(json_data);
                JsonResult = JsonConvert.SerializeObject(result);
                return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult UpdateSale(SaleView Sale)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                url = "api2/UpdateSale";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, Sale);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<int>().Result;

                if (result == 1)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        [HttpPost]
        public ActionResult GetSearchVenta(SaleSearchView Sale)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                Sale.IdStore = Convert.ToInt32(Session["IDSTORE"]);

                url = "api2/GetSearchVenta";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, Sale);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<List<SaleSearchView>>().Result;

                if (result != null)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        [HttpPost]
        public ActionResult GetSearchDetalleVenta(SaleSearchView Sale)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;

            try
            {
                Sale.IdStore = Convert.ToInt32(Session["IDSTORE"]);

                url = "api2/GetSearchDetalleVenta";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, Sale);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<SalesSearchVenta>().Result;

                if (result != null)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        [HttpPost]
        public ActionResult GetDataListFacturacion()
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            SaleSearchView Sale = new SaleSearchView();

            try
            {
                url = "api2/GetDataListFacturacion";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, Sale);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<FacturacionCFDIView>().Result;

                if (result != null)
                {

                    result.CorreoUsuario = Session["CorreoUsuario"].ToString();

                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        [HttpPost]
        public ActionResult GetDataListFormaPagoFacturacion(FacturacionFormaPagoCFDIView metodoPago)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;

            try
            {
                url = "api2/GetDataListFormaPagoFacturacion";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, metodoPago);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<IEnumerable<FormasDePagosView>>().Result;

                if (result != null)
                {
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        [HttpPost]
        public ActionResult CancelaPedidoInt(string idVenta)
        {
            string url = string.Empty;
            string JsonResult = string.Empty;
            string parameterJson = string.Empty;
            string Squery = string.Empty;
            string sError1 = string.Empty;
            DBMaster oDB = new DBMaster();
            var payback = new PaybackController();
            AdminPayback AdminR = new AdminPayback();
            ListRquestPurchase requestR = new ListRquestPurchase();
            connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            try
            {

                //var json1sale = JsonConvert.DeserializeObject<AddSale>(Paymentscap);
                var IdStore = Convert.ToInt32(Session["IDSTORE"]);
                var AdminUser = Session["AdminUserID"].ToString();
                var Obj = new
                {
                    IdStore = IdStore,
                    AdminUserID = AdminUser,
                    idVenta = idVenta
                };
                parameterJson = JsonConvert.SerializeObject(Obj);
                url = "api2/CancelaPedidoInt";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, Obj);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<ResponseCancelacionPedido>().Result;

                if (result != null)
                {
                    result.idVenta = idVenta;
                    if (result.InfoPaypackCancel != null)//Verifica si existen pagos con monedero
                    {
                        foreach (var date in result.InfoPaypackCancel)
                        {
                            Squery = "";
                            if (date.TypeTransaction.Equals("RedencionVenta"))
                            {
                                AdminR = payback.GetInfoAdminPayback1("Redemption", "ReversoRedencion");
                                requestR = payback.QueryDatesPurch1(Session["IDSTORE"].ToString(), idVenta, AdminR.TypeWs);
                                var CanceladoR = payback.ReverseRedemptionEvent(date.Monedero, AdminR.AdminUser, AdminR.Password, AdminR.PartnerShortName, requestR.branchShortName, requestR.Fecha, requestR.ReceipNumber, date.ReceiptNumber);

                                if (CanceladoR.FaultMessage == null)
                                {// payback.PointsToAmount(pagoPayback.Transactions.FirstOrDefault().TotalPoints.LoyaltyAmount, AdminR.ValueInPoints)
                                    Squery = "update Dat_TransactionPayback set StatusCancel=1 where idTransaction=" + date.idTransaction + Environment.NewLine;
                                    Squery = Squery + "Update DevolucionesEncabezado set TotalPuntosPayback = " + CanceladoR.AccountBalanceDetails.TotalPointsAmount + ", PuntosPaybackRedimidos = " + payback.PointsToAmount(CanceladoR.Transactions.FirstOrDefault().TotalPoints.LoyaltyAmount, AdminR.ValueInPoints) + " WHERE ID =" + result.idDevolucion;
                                }
                                else
                                {
                                    Squery = "update Dat_TransactionPayback set StatusCancel=0, MessageErrorCancel='" + CanceladoR.FaultMessage + "' where idTransaction=" + date.idTransaction;
                                }
                                oDB.EjecutaQry(Squery, CommandType.Text, connstringWEB, sError1);
                            }
                            else if (date.TypeTransaction.Equals("AcumulacionVenta"))
                            {
                                var AdminAc = payback.GetInfoAdminPayback1("Acumulation", "ReversoAcumulacion");
                                var request1 = payback.QueryDatesPurch1(Session["IDSTORE"].ToString(), idVenta, AdminAc.TypeWs);

                                var CanceladoAc = payback.ReverseCollectEvent(date.Monedero, AdminAc.AdminUser, AdminAc.Password, AdminAc.PartnerShortName, request1.branchShortName, request1.Fecha, request1.ReceipNumber, date.ReceiptNumber);

                                if (CanceladoAc.FaultMessage == null)
                                {
                                    Squery = "update Dat_TransactionPayback set StatusCancel=1 where idTransaction=" + date.idTransaction + Environment.NewLine;
                                    Squery = Squery + "Update DevolucionesEncabezado set TotalPuntosPayback = " + CanceladoAc.AccountBalanceDetails.TotalPointsAmount + ", PuntosPaybackAcumulados = " + payback.PointsToAmount(CanceladoAc.Transactions.FirstOrDefault().TotalPoints.LoyaltyAmount, AdminAc.ValueInPoints) + " WHERE ID =" + result.idDevolucion;
                                }
                                else
                                {
                                    Squery = "update Dat_TransactionPayback set StatusCancel=0, MessageErrorCancel='" + CanceladoAc.FaultMessage + "' where idTransaction=" + date.idTransaction;
                                }
                                oDB.EjecutaQry(Squery, CommandType.Text, connstringWEB, sError1);
                            }

                        }
                    }
                    JsonResult = JsonConvert.SerializeObject(result);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                return Json(null);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        //Actualización de fecha para entrega de pedidos
        public ActionResult Updatedatein()
        {
            try
            {
                ViewBag.Idstore = Session["IDSTORE"].ToString();
                ViewBag.Idusuario = Session["AdminUserID"].ToString();
                ViewBag.WhsID = Session["WHSID"].ToString();
                return View("~/Views/Modificaciones/ActualizaFechaEntrega.cshtml");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //Actualización de fecha para entrega de pedidos busca pedido
        public JsonResult BuscaFolio(string folio, string tienda)
        {
            var jsondataf = new
            {
                IdStore = tienda,
                folio = folio
            };
            string jsondata = JsonConvert.SerializeObject(jsondataf);
            string JsonResult = string.Empty;
            JsonResult = JsonConvert.SerializeObject(_VentasBL.Getdatepedido(jsondata));
            return Json(JsonResult);
        }
        //Actualización de fecha para entrega de pedidos busca pedido
        public JsonResult UpdateFechaE(string idventa, string fechaentrega)
        {
            var jsondataf = new
            {
                idventa = idventa,
                fechaentrega = fechaentrega
            };
            string jsondata = JsonConvert.SerializeObject(jsondataf);
            string JsonResult = string.Empty;
            JsonResult = JsonConvert.SerializeObject(_VentasBL.UpdateFechaEn(jsondata));
            return Json(JsonResult);
        }
        [HttpPost]
        public JsonResult RequestApDesc(string ItemCode, string monto, string idcliente, string Observaciones, int Lista, string PriceFDO)
        {
            MessageView message = new MessageView();
            try
            {
                var JsonDesc = (new RequestApDesc
                {
                    Cantdescuento = monto,
                    IdArticulo = ItemCode,
                    Idstored = Session["IDSTORE"].ToString(),
                    Iduser = Session["AdminUserID"].ToString(),
                    IdCliente = idcliente,
                    Observaciones = Observaciones,
                    idList = Lista,
                    PriceFDO = Convert.ToDouble(PriceFDO.Replace("$", ""))
                });
                //string Apdesc = JsonConvert.SerializeObject(JsonDesc);
                var url = "api2/GetInfoApDesc";
                HttpResponseMessage response = _VentasBL.ReadAsStringAsyncAPI(url, JsonDesc);
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsAsync<MessageView>().Result;


                var JsonResult = JsonConvert.SerializeObject(result);
                return Json(JsonResult);


            }
            catch (Exception ex)
            {

                throw;
            }

        }
        [HttpPost]
        public JsonResult InsertCodigo(string codigo, string ItemCode, string IdCliente)
        {
            DBMaster oDB = new DBMaster();
            string Squery = string.Empty;
            MessageView message = new MessageView();
            connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            try
            {

                var Excodigo = ExisteCodigo(codigo, ItemCode, IdCliente);

                if (!string.IsNullOrEmpty(Excodigo))
                {
                    if (!CodigoExpirado(codigo))
                    {
                        Squery = "update Autdescuentos set Utilizado = 1 where Codigo = '" + codigo.Trim() + "'";
                        oDB.EjecutaQry(Squery, CommandType.Text, connstringWEB, "");
                        message.Success = true;
                        message.Message = Excodigo;
                    }
                    else
                    {
                        message.Success = false;
                        message.Message = $"El codigo : {codigo} ha expirado, Favor de solicitar su descuento nuevamente";
                    }
                }
                else
                {
                    message.Success = false;
                    message.Message = $"El codigo : {codigo} No existe o fue utilizado anteriormente";
                }

                var objDesc = JsonConvert.SerializeObject(message);
                return Json(objDesc);


            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public string ExisteCodigo(string codigo, string ItemCode, string IdCliente)
        {
            DBMaster oDB = new DBMaster();
            string Squery = string.Empty;
            string Codigo = string.Empty;
            connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            Squery = "select Codigo,(case when utilizado is null then 0 else utilizado end) utilizado,(case when NewDesc is null then Cantdescuento when NewDesc = 0 then Cantdescuento else NewDesc end) Cantdescuento from Autdescuentos where Codigo = '" + codigo.Trim() + "' and Statusdesc = 1 and IdArticulo = '" + ItemCode + "' and IdCliente = '" + IdCliente + "'";
            var result = oDB.EjecutaQry_Tabla(Squery, CommandType.Text, "ExisteCodigo", connstringWEB);
            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow item in result.Rows)
                {
                    if (!string.IsNullOrEmpty(item[0].ToString()))
                    {
                        if (item[1].ToString().Equals("1"))
                        {
                            return Codigo;
                        }
                        return item[2].ToString();
                    }
                    else { return Codigo; }
                }
            }
            return Codigo;
        }
        public bool CodigoExpirado(string codigo)
        {
            DBMaster oDB = new DBMaster();
            string Squery = string.Empty;
            System.DateTime FechaCod;
            System.DateTime FechaHoy = DateTime.Now;
            connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            int ExpiredHour = Convert.ToInt32(ConfigurationManager.AppSettings["ExpiredHours"]);
            try
            {
                Squery = "select Fechas from Autdescuentos where Codigo = '" + codigo + "'";
                var result = oDB.EjecutaQry_Tabla(Squery, CommandType.Text, "Expirado", connstringWEB);
                if (result != null)
                {
                    foreach (DataRow item in result.Rows)
                    {
                        if (!string.IsNullOrEmpty(item[0].ToString()))
                        {
                            FechaCod = Convert.ToDateTime(item[0]);
                            var diferencia = FechaHoy - FechaCod;
                            if (diferencia.TotalHours < ExpiredHour)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return true;
        }
        //Obtiene el metodo de pago de los Pagos agregados
        public void Metodopago33Pagos(AddSale DatosVenta)
        {
            if (DatosVenta.MetodoPago33 == "PUE")
            {
                DatosVenta.ArrayPagos.ToList().ForEach(
                pay =>
                 {
                     pay.MetodoPago33 = "PUE";
                 });
            }
            else
            {
                DatosVenta.ArrayPagos.ToList().ForEach(
                pay =>
                 {
                     pay.MetodoPago33 = "PPD";
                 });

            }
        }
        public void ParcialidadPagos(AddSale PagosVenta)
        {
            var count = PagosVenta.ArrayPagos.Count();
            var parcialidad = 0;
            foreach (var Pago in PagosVenta.ArrayPagos)
            {
                parcialidad++;
                Pago.Parcialidad = parcialidad;
            }
        }

        public string RetornaultimoIDventa(string IdStore)
        {
            string IdVenta = string.Empty;
            DBMaster oDB = new DBMaster();
            connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            DataTable dt;
            List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdStore", Value = IdStore }
                    };
            dt = oDB.EjecutaQry_Tabla("UltimaIDVentaXTienda", lsParameters, CommandType.StoredProcedure, "UltimaIDVentaXTienda", connstringWEB);
            if (dt.Rows.Count > 0)
            {
                IdVenta = dt.Rows[0]["ID"].ToString();
            }
            return IdVenta;
        }


        public AddSale AddSaleInsert(AddSale Sale)
        {
            StringBuilder Userquery = new StringBuilder();
            DataTable dt;
            string connstringWEB;
            //Log logApi = new Log(patherror);
            var ExitDescTotal = Sale.ArrayArticulos.Any(x => x.Juego != "JGO");
            var ExistTotalCero = Sale.ArrayArticulos.Any(x => Convert.ToDouble(x.Total.Replace("$", "")) == 0);
            if (ExitDescTotal && ExistTotalCero)
            {
                var GetArrayArt = GetNewObjAdd(Sale.ArrayArticulos);
                if (GetArrayArt != null)
                {
                    Sale.ArrayArticulos = GetArrayArt;
                }
                else { return null; }
            }
            try
            {
                if (Sale.webToken.Equals(_token))
                {
                    connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                    if (addVenta(Sale) == true)
                    {
                        ServiceMessage(infoventas.IDSTORE, infoventas.IDPRINTVENTA);   //Envia mensajes de texto
                        GeneralClass objlog = new GeneralClass();
                        objlog.InsertaLog(Tipos.VE, infoventas.IDPRINTVENTA, infoventas.IDSTORE, infoventas.WHSID);
                        CFactura(infoventas.IDPRINTVENTA, infoventas.IDSTORE);   //Envia hoja Roja
                        CheckOrder(Sale, infoventas.IDPRINTVENTA.ToString()); //Revisa si es pedido urgente y envia notificacion

                        AddSale ResponseVenta = new AddSale();
                        ResponseVenta.saleresponse = true;
                        ResponseVenta.Idventa = infoventas.IDPRINTVENTA.ToString();
                        return ResponseVenta;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public List<ArrayArticulos> GetNewObjAdd(List<ArrayArticulos> ArtS)
        {
            List<ArrayArticulos> ListarrayArticulos = new List<ArrayArticulos>();
            try
            {
                double PriceBox = 0.01;
                bool AplicateDescount = false;
                var GetTotalcero = ArtS.Where(x => Convert.ToDouble(x.Total.Replace("$", "")) == 0).ToList();
                //var CalcValue = Convert.ToInt32(GetTotalcero.FirstOrDefault().Cantidad) * PriceBox;
                var Art = ArtS.OrderByDescending(x => Convert.ToDouble(x.Total.Replace("$", "")));
                foreach (var item in Art)
                {
                    if (Convert.ToDouble(item.Total.Replace("$", "")) == 0)
                    {
                        var array = (new ArrayArticulos
                        {
                            AlmacenBox = item.AlmacenBox,
                            Total = "$" + PriceBox,
                            Articulo = item.Articulo,
                            Cantidad = item.Cantidad,
                            CantidadBodega = item.CantidadBodega,
                            CantidadBox = item.CantidadBox,
                            CantidadTienda = item.CantidadTienda,
                            comentarios = item.comentarios,
                            Descuento = "$0.00",
                            Id = item.Id,
                            IVA = item.IVA,
                            Juego = item.Juego,
                            Linea = item.Linea,
                            Lista = item.Lista,
                            Medida = item.Medida,
                            Modelo = item.Modelo,
                            Monto = "$" + PriceBox,
                            PrecioUnitario = item.PrecioUnitario,
                            subTotal = "$" + PriceBox,
                            DescuentoJgo = item.Descuento
                        });
                        ListarrayArticulos.Add(array);
                    }
                    else if (!AplicateDescount)
                    {
                        var DescPU = Convert.ToDouble(item.PrecioUnitario.Replace("$", "")) - PriceBox;
                        var DescT = Convert.ToDouble(item.Total.Replace("$", "")) - PriceBox;
                        var DescSub = Convert.ToDouble(item.subTotal.Replace("$", "")) - PriceBox;
                        var array = (new ArrayArticulos
                        {
                            AlmacenBox = item.AlmacenBox,
                            Total = "$" + DescT.ToString(),
                            Articulo = item.Articulo,
                            Cantidad = item.Cantidad,
                            CantidadBodega = item.CantidadBodega,
                            CantidadBox = item.CantidadBox,
                            CantidadTienda = item.CantidadTienda,
                            comentarios = item.comentarios,
                            Descuento = item.Descuento,
                            Id = item.Id,
                            IVA = item.IVA,
                            Juego = item.Juego,
                            Linea = item.Linea,
                            Lista = item.Lista,
                            Medida = item.Medida,
                            Modelo = item.Modelo,
                            Monto = item.Monto,
                            PrecioUnitario = "$" + DescPU.ToString(),
                            subTotal = "$" + DescSub.ToString()
                        });
                        AplicateDescount = true;
                        ListarrayArticulos.Add(array);
                    }
                    else { ListarrayArticulos.Add(item); }

                }
                return ListarrayArticulos;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public string ServiceMessage(object IdTienda, object IdVenta)
        {
            try
            {
                DataTable dt = new DataTable();
                Utilities.DBMaster oDB = new Utilities.DBMaster();
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                string squery;
                squery = "select(Case WHEN a.CorreoElectronico IS NULL THEN '' ELSE a.CorreoElectronico END) as COrreo,v.Folio,(Case WHEN a.NoCelular IS NULL THEN '' ELSE a.NoCelular END) as NoCelularUser,(Case WHEN c.Correo IS NULL THEN '' ELSE c.Correo END) as CorreoClient,(Case WHEN c.NoCelular IS NULL THEN '' ELSE c.NoCelular END) as NoCelularClient from VentasEncabezado v LEFT OUTER JOIN AdminUser a ON v.IDUser = a.AdminUserID INNER JOIN Clientes c ON c.id = v.IDCliente where v.ID=" + IdVenta.ToString() + " AND v.IDStore=" + IdTienda.ToString() + "";
                dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "VentasEncabezado", connstringWEB);
                string textmessagevend = "Buen dia estimado Vendedor, el pedido N° " + dt.Rows[0][1].ToString() + " ha sido generado con exito";
                string textmessageclie = "Estimado Cliente su pedido " + dt.Rows[0][1].ToString() + " ha sido generado con exito";
                // ****************
                Threads Messagepedido = new Threads("MessagePedido", 1, dt.Rows[0][2].ToString(), textmessagevend, dt.Rows[0][0].ToString(), dt.Rows[0][4].ToString(), dt.Rows[0][3].ToString(), textmessageclie, 1);
                Thread Messped = new Thread(Messagepedido.Messagepedido);
                Messped.Start();
                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public void CFactura(int IDVENTA, int IDSTORE)
        {
            try
            {
                DataTable dt = new DataTable();
                Utilities.DBMaster oDB = new Utilities.DBMaster();
                connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
                string squery = "";
                int IDABONO = 0;
                squery = squery + "select top 1 ID from VentasPagos where IDVenta=" + IDVENTA;
                dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "SelectFecha", connstringWEB);
                if (dt.Rows.Count > 0)
                {
                    IDABONO = Convert.ToInt32(dt.Rows[0][0]);
                }
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IDTIENDA", Value = IDSTORE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IDABONO", Value = IDABONO }
                    };
                Utilities.DBMaster obj = new Utilities.DBMaster();
                dt = obj.EjecutaQry_Tabla("HredInfo", lsParameters, CommandType.StoredProcedure, connstringWEB);
                bool sError = false;
                int exist_ = 0;
                string aux, tipoT = "";
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Origen"].ToString() == "Tienda")
                    {
                        exist_ = Convert.ToInt32(row["ExistenciaTienda"]);
                        aux = "ExistenciaTienda";
                    }
                    else
                    {
                        exist_ = Convert.ToInt32(row["ExistenciaBodega"]);
                        aux = "ExistenciaBodega";
                    }
                    if (aux == "ExistenciaTienda")
                    {
                        //nothing
                    }
                    else
                    {
                        if (Convert.ToInt32(row["Cantidad"]) > exist_)
                        {
                            // var msg="Existencia insuficiente del articulo " & "<br>" & row["Articulo"] & "<br>" & "<br>Existencia : " & row(aux) & "<br> Origen de Venta : " & tipoT & "<br> Se a enviado la información al distribuidor para surtir el producto a la brevedad posible "
                            stock = row[aux].ToString();
                            ImprimirHojaRoja(IDABONO, IDSTORE);
                            break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void ImprimirHojaRoja(int IDABONO, int IDSTORE)
        {
            try
            {
                string mensajeME, mensajeMEFinal, mensajeME1, asuntoN, rutaPDFSend, franq, mensajeTab2;
                DataTable dt = new DataTable(), dto = new DataTable(), dtos = new DataTable(), dtosi = new DataTable(), dtosro = new DataTable();
                string squery, squery1, squery2, squery3, squery4, squery5;
                string iddeventa, banderaconfirmacion;
                DateTime Fecha;
                Nullable<DateTime> FechaCFDI;
                Nullable<int> numeroemail = 0;
                int envoy = 0;
                int idhre = 0;
                DateTime dtcreafactura = DateTime.Now;
                int Dias;
                int Hrs;
                TimeSpan Calculodia;
                TimeSpan Calculohr;
                Utilities.DBMaster oDB = new Utilities.DBMaster();
                DBMaster oDB1 = new DBMaster();
                int horalimite = 16;
                int hora = dtcreafactura.Hour;
                bool bandhrs = false;
                string sError = "";
                if (hora >= horalimite)
                    bandhrs = true;
                DireccionFis = "";
                squery = "select IDVenta, Fecha,FechaCFDI from VentasPagos where  ID = " + IDABONO + "";
                dt = oDB.EjecutaQry_Tabla(squery, CommandType.Text, "IDVenta", connstringWEB);
                iddeventa = dt.Rows[0][0].ToString();
                Fecha = Convert.ToDateTime(dt.Rows[0][1]);
                try
                {
                    FechaCFDI = Convert.ToDateTime(dt.Rows[0][2]);
                }
                catch (Exception ex)
                {
                    FechaCFDI = dtcreafactura;
                }

                squery5 = "select top 1 idhre,mailenviados from HojaRojaEncabezado where IdVenta = " + iddeventa + "";
                dtosi = oDB.EjecutaQry_Tabla(squery5, CommandType.Text, "IDVenta", connstringWEB);
                try
                {
                    idhre = Convert.ToInt32(dtosi.Rows[0][0]);
                    numeroemail = Convert.ToInt32(dtosi.Rows[0][1]);
                }
                catch (Exception ex)
                {
                    envoy = 0;
                }
                if (numeroemail != 0)
                    envoy = Convert.ToInt32(numeroemail);
                if (dtosi.Rows.Count == 0)
                {
                    banderaconfirmacion = "";
                    asuntoN = "URGENTE STOCK";
                    if (bandhrs == true)
                    {
                        asuntoN = "INFORME DE HOJA ROJA";
                        envoy = 0;
                    }
                    else
                        envoy = 1;
                }
                else
                {
                    banderaconfirmacion = dtosi.Rows[0][0].ToString();
                    asuntoN = "RECORDATORIO URGENTE STOCK";

                    if (bandhrs == true)
                    {
                        asuntoN = "INFORME DE HOJA ROJA";
                        //envoy = envoy;

                        squery3 = "update HojaRojaEncabezado set mailenviados = " + envoy + ",ultimafechaemail =GETDATE() where idhre=" + idhre;
                        oDB1.EjecutaQry(squery3, CommandType.Text, connstringWEB, sError);
                    }
                    else
                    {
                        envoy = envoy + 1;
                        squery3 = "update HojaRojaEncabezado set mailenviados = " + envoy + ",ultimafechaemail =GETDATE() where idhre=" + idhre;
                        oDB1.EjecutaQry(squery3, CommandType.Text, connstringWEB, sError);
                    }
                }

                mensajeME1 = "<Table border=" + 1 + "> <Tr><Th>Articulo</Th><Th>CANTIDAD</Th></Tr>";
                mensajeTab2 = " <br /> <Table border=" + 2 + "> <Tr><Th colspan=" + 2 + "> COMPLEMENTO DEL PEDIDO </Th></Tr>";
                mensajeTab2 += "<Tr><Th>Articulo</Th><Th>CANTIDAD</Th></Tr>";
                List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IDTIENDA", Value = IDSTORE },
                        new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IDVENTA", Value = iddeventa }
                    };
                dto = oDB.EjecutaQry_Tabla("ArticulosxPedido", lsParameters, CommandType.StoredProcedure, connstringWEB);
                string namefranqui;
                string franqui;
                double cantidad = 0;
                // insert de hoja roja
                namefranqui = dto.Rows[0][2].ToString();
                franqui = dto.Rows[0][3].ToString();
                cantidad = Convert.ToDouble(dto.Rows[0][1]);
                if (banderaconfirmacion == "")
                {
                    sError = "";
                    if (bandhrs == true)
                    {
                        squery3 = "insert into HojaRojaEncabezado (fecha, IdVenta, IdStore, almacen, mailenviados,ultimafechaemail) values (getdate()," + iddeventa + ",(select IDStore from VentasEncabezado  where ID = " + iddeventa + ") ," + franqui + "," + envoy + ",GETDATE())";
                        oDB1.EjecutaQry(squery3, CommandType.Text, connstringWEB, sError);
                    }
                    else
                    {
                        squery3 = "insert into HojaRojaEncabezado (fecha, IdVenta, IdStore, almacen, mailenviados,ultimafechaemail) values (getdate()," + iddeventa + ",(select IDStore from VentasEncabezado  where ID = " + iddeventa + ") ," + franqui + "," + envoy + ",GETDATE())";
                        oDB1.EjecutaQry(squery3, CommandType.Text, connstringWEB, sError);
                    }
                }
                string cantidadparainsert = "";
                string cantidadpara = "";
                string descarticulo = "";
                string tipoT = "";
                double Cantidadex;
                bool band = true;
                bool band2 = false;
                foreach (DataRow Drow in dto.Rows)
                {
                    double extienda = Convert.ToDouble(Drow["ExistenciaTienda"]);
                    double exbodega = Convert.ToDouble(Drow["ExistenciaBodega"]);
                    band = true;
                    cantidad = Convert.ToDouble(Drow["Cantidad"]);
                    Cantidadex = Convert.ToDouble(Drow["Cantidad"]);
                    if (Drow["Origen"].ToString() == "Bodega Consignacion")
                        tipoT = "Bodega";
                    else
                        tipoT = "Tienda";
                    if (tipoT == "Tienda")
                    {
                        if (extienda < cantidad)
                        {
                            double stockk = cantidad - extienda;
                            if (extienda < 0)
                                extienda = 0;
                            double resta = cantidad - extienda;
                            mensajeME1 += "<tr><th>" + Drow["Articulo"].ToString() + "</td><td>" + resta + ".00" + "</td></Tr>";
                            franq = Drow["IDstore"].ToString();
                            cantidadparainsert = stockk.ToString();
                            cantidadpara = Drow["IdArticulo"].ToString();
                            descarticulo = Drow["Articulo"].ToString();
                            band = false;
                            Cantidadex = extienda;
                        }
                    }
                    else if (tipoT == "Bodega")
                    {
                        if (exbodega < cantidad)
                        {
                            double stockk = cantidad - exbodega;
                            if (exbodega < 0)
                                exbodega = 0;
                            double resta = cantidad - exbodega;
                            mensajeME1 += "<tr><th>" + Drow["Articulo"].ToString() + "</td><td>" + resta + ".00" + "</td></Tr>";
                            franq = Drow["IDstore"].ToString();
                            cantidadparainsert = stockk.ToString();
                            cantidadpara = Drow["IdArticulo"].ToString();
                            descarticulo = Drow["Articulo"].ToString();
                            band = false;
                            Cantidadex = exbodega;
                        }
                    }


                    if (Cantidadex > 0.0)
                    {
                        mensajeTab2 += "<tr><th>" + Drow["Articulo"].ToString() + "</td><td>" + Cantidadex + ".00" + "</td></Tr>";
                        franq = Drow["IDstore"].ToString();
                        cantidadparainsert = Cantidadex.ToString();
                        cantidadpara = Drow["IdArticulo"].ToString();
                        descarticulo = Drow["Articulo"].ToString();
                    }

                    // insert de hoja roja
                    if (banderaconfirmacion == "")
                    {
                        sError = "";
                        squery4 = "insert into HojaRojaDetalle (IdVenta, fecha, IdArticulo, Descripcion,  Cantidad, Stock, Estatus, IdHRE) values (" + iddeventa + ", GETDATE(),'" + cantidadpara + "','" + descarticulo + "' , " + cantidadparainsert + ", " + stock + " ,'E', (select top 1 IdHRE from  HojaRojaEncabezado where IdVenta = " + iddeventa + " order by IdHRE desc))";
                        oDB1.EjecutaQry(squery4, CommandType.Text, connstringWEB, sError);
                    }
                }
                mensajeME1 += "</table>";
                mensajeTab2 += "</table>";
                try
                {
                    squery1 = "select top 1 fecha from HojaRojaDetalle where IdHRE = " + idhre + "";
                    dtosro = oDB.EjecutaQry_Tabla(squery1, CommandType.Text, "hojarojadetalle", connstringWEB);
                    DataRow columna = dtosro.Rows[0];
                    DateTime fecharoja = Convert.ToDateTime(columna["fecha"]);
                    var now = fecharoja.ToString("HH:mm");
                    now = now.Replace(":", ".");//para comparar las horas lo convertimos a decimal
                    decimal houred = Convert.ToDecimal(now);
                    decimal hourconstant = 16.00m;
                    DateTime horaActual = DateTime.Now;
                    var boool = (envoy == 1) ? true : false;
                    if (houred >= hourconstant)
                    {
                        if (boool)
                        {
                            Hrs = 0;
                        }
                        else
                        {
                            Calculohr = Convert.ToDateTime(dtcreafactura).Subtract(Convert.ToDateTime(fecharoja));
                            Hrs = Convert.ToInt32(Calculohr.TotalHours);
                        }
                    }
                    else
                    {
                        Calculohr = Convert.ToDateTime(dtcreafactura).Subtract(Convert.ToDateTime(fecharoja));
                        Hrs = Convert.ToInt32(Calculohr.TotalHours);
                    }
                }
                catch (Exception ex)
                {
                    Hrs = 0;
                }
                if (banderaconfirmacion == "" & bandhrs != true)
                {
                    mensajeME = "<br />Esta es una solicitud de un pedido urgente:";
                    mensajeME += "<br /> Numero de intentos de facturacion :  " + envoy;
                    mensajeME += "<br /> Pedido POS numero: " + iddeventa + " ";
                    mensajeME += "<br /> Fecha y hora de pedido: " + Fecha + " ";
                    mensajeME += "<br /> Fecha y hora del CFDI: " + FechaCFDI + "";
                    mensajeME += "<br />Tienda: " + namefranqui + " " + franqui + "<br />";
                    if (Hrs < 24)
                        mensajeME += "<p><font size=5 color='green'>" + Hrs + " Horas transcurridas </font> </p><br />";
                    else if (Hrs >= 24 & Hrs < 36)
                        mensajeME += "<p><font size=5 color='orange'>" + Hrs + " Horas transcurridas </font> </p><br />";
                    else if (Hrs >= 36)
                        mensajeME += "<p><font size=5 color='red'>" + Hrs + " Horas transcurridas </font> </p><br />";
                }
                else if (bandhrs == true)
                {
                    mensajeME = "<br />Se intento facturar el pedido:" + iddeventa + " despues de las 4 de la tarde";
                    mensajeME += "<br /> Pedido POS numero: " + iddeventa + " ";
                    mensajeME += "<br /> Fecha y hora de pedido: " + Fecha + " ";
                    mensajeME += "<br /> Fecha y hora del CFDI: " + FechaCFDI + "";
                    mensajeME += "<br />Tienda: " + namefranqui + " " + franqui + "<br />";
                    mensajeME += "<br /> NOTA: La contabilizacion de este intento comenzara el siguiente dia laboral apartir de las 8 a.m. ";
                }
                else
                {
                    mensajeME = "<br />La notificaci&oacute;n inicial de falta de mercanc&iacute;a del pedido: " + iddeventa + " seguimos en la espera de la notificaci&oacute;n del envi&oacute;, agradecemos tu pronta respuesta";
                    mensajeME += "<br /> Numero de intentos de facturacion :  " + envoy;
                    mensajeME += "<br /> Fecha y hora de pedido: " + Fecha + " ";
                    mensajeME += "<br /> Fecha y hora del CFDI: " + FechaCFDI + "";
                    mensajeME += "<br />Tienda: " + namefranqui + " " + franqui + "";
                    if (Hrs < 24)
                        mensajeME += "<p><font size=5 color='green'>" + Hrs + " Horas transcurridas </font> </p><br />";
                    else if (Hrs >= 24 & Hrs < 36)
                        mensajeME += "<p><font size=5 color='orange'>" + Hrs + " Horas transcurridas </font> </p><br />";
                    else if (Hrs >= 36)
                        mensajeME += "<p><font size=5 color='red'>" + Hrs + " Horas transcurridas </font> </p><br />";
                }
                mensajeME += "<br />Detalles: <br />";

                mensajeMEFinal = mensajeME + mensajeME1 + mensajeTab2;

                squery2 = "select IDVenta from VentasPagos where ID=" + IDABONO;
                dtos = oDB1.EjecutaQry_Tabla(squery2, CommandType.Text, "LastSale", connstringWEB);

                string idVenta = dt.Rows[0][0].ToString();
                IDPRINTVENTA = idVenta;
                btnPDF = true;
                //Threads GeneraRtpVenta = new Threads(IDPRINTVENTA);
                //Thread CreaPDF = new Thread(GeneraRtpVenta.CreaPDFventa);
                //CreaPDF.Start();
                btnPDF = false;
                var mail = ConfigurationManager.AppSettings.Get("CorreosHojaRoja");
                Threads MailsEnvoy = new Threads("sistemaslerma@dormimundo.com.mx", "Sysfdo&91$", mensajeMEFinal, asuntoN, mail, IDPRINTVENTA);
                Thread EnviaMail = new Thread(MailsEnvoy.CreaPDFventaYenviaCorreo);
                EnviaMail.Start();
            }
            catch (Exception ex)
            {

            }
        }

        public void CheckOrder(AddSale Infoventa, string IDPRINTVENTA)
        {
            try
            {
                Threads Instancia = new Threads(Infoventa, IDPRINTVENTA);
                Thread CheckOrder = new Thread(Instancia.PedidoUrgente);
                CheckOrder.Start();
            }
            catch (Exception ex)
            {

            }
        }

        public bool addVenta(AddSale AddVenta)
        {
            string value; //COMO SE ENTERO DE NOSOTROS
            value = AddVenta.Medios;
            string typesale = AddVenta.Tipodeventa; //TIPO DE VENTA
            string campo;
            string fechanueva = AddVenta.Fechaentrega; //FECHA DE ENTREGA
            string anexo; //FECHA DE ENTREGA
            campo = ",FechaEntrega";
            anexo = ",'" + fechanueva + "'";
            string squery;
            string idcliente = AddVenta.Idcliente;
            connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            DataTable dt = new DataTable();
            Utilities.DBMaster obj = new Utilities.DBMaster();
            var LogWEB = new PaybackController();
            String LogWeb = AppDomain.CurrentDomain.BaseDirectory + @"logErrorPayback\LogWeb_" + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace("T", "_").Replace("-", "_").Substring(0, 10) + ".txt";
            LogWEB.anade_linea_archivo2(LogWeb, "Inicia Creación de Transaccion para insertar venta");
            try
            {
                #region
                if (AddVenta.IsRequiredFactura.Equals("false"))
                {
                    List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                                new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdCliente", Value = idcliente }
                            };

                    int i = obj.EjecutaQry_Tabl("UpdateRFCGenericoCliente", "", lsParameters, CommandType.StoredProcedure, connstringWEB);
                }
                else
                {
                    List<System.Data.SqlClient.SqlParameter> lsParameters = new List<System.Data.SqlClient.SqlParameter>(){
                                    new System.Data.SqlClient.SqlParameter(){ ParameterName = "@IdCliente", Value = idcliente }
                                };

                    int i = obj.EjecutaQry_Tabl("UpdateClienteDatosFacturacion", "", lsParameters, CommandType.StoredProcedure, connstringWEB);
                }
                #endregion

                squery = "DECLARE @intErrorCode INT" + Environment.NewLine + Environment.NewLine;
                squery = squery + " begin transaction" + Environment.NewLine + Environment.NewLine;
                squery = squery + " DECLARE @Tienda as int" + Environment.NewLine;
                squery = squery + " DECLARE @TipoFolio as int" + Environment.NewLine;
                squery = squery + " DECLARE @Folio as int" + Environment.NewLine;
                squery = squery + " DECLARE @Prefijo as varchar(20)" + Environment.NewLine;
                squery = squery + " DECLARE @Sufijo as varchar(20)" + Environment.NewLine;
                squery = squery + " DECLARE @UltimaVenta as int" + Environment.NewLine;
                squery = squery + " DECLARE @LineasAInsertar as int" + Environment.NewLine;
                squery = squery + " DECLARE @LineasInsertadas as int" + Environment.NewLine + Environment.NewLine;
                squery = squery + " DECLARE @ICCS as VARCHAR(200)=''" + Environment.NewLine;
                squery = squery + " DECLARE @IMEIS as VARCHAR(200)=''" + Environment.NewLine;
                squery = squery + " DECLARE @Inventario as int" + Environment.NewLine;
                squery = squery + " DECLARE @EmailTienda as VARCHAR(200)=''" + Environment.NewLine + Environment.NewLine;
                squery = squery + " DECLARE @ERROR as VARCHAR(200)" + Environment.NewLine + Environment.NewLine;


                squery = squery + " SET @ERROR = 'Ocurrio un error'" + Environment.NewLine;

                string STipoCliente = "";

                STipoCliente = "C";

                string sCurrentFolio;
                sCurrentFolio = "1";
                squery = squery + " SET @TipoFolio = " + sCurrentFolio + Environment.NewLine;
                squery = squery + " SET @Tienda = " + AddVenta.idstore + Environment.NewLine;
                squery = squery + " SET @Folio= (select (currentfolio + 1) as [NuevoFolio] from storeFolios where AdminStoreID=@Tienda and AdminFolioType=@TipoFolio)" + Environment.NewLine;
                squery = squery + " SET @Prefijo = LTRIM(RTRIM((select prefijo from storeFolios where AdminStoreID=@Tienda and AdminFolioType=@TipoFolio)))" + Environment.NewLine;
                squery = squery + " SET @Sufijo = LTRIM(RTRIM((select NoAprobacion from storeFolios where AdminStoreID=@Tienda and AdminFolioType=@TipoFolio)))" + Environment.NewLine;
                squery = squery + " SET @EmailTienda= (select emailTienda from Adminstore where adminstoreid=" + AddVenta.idstore + ")" + Environment.NewLine;
                squery = squery + " print @Folio" + Environment.NewLine;
                squery = squery + " print @Prefijo" + Environment.NewLine;
                // Captura de encabezado----------------------------------------------------------------------------------
                squery = squery + "Insert into ventasencabezado (IDCliente,IDUser,IDStore,Fecha,Folio,Prefijo,SUFIJO,TipoVenta,StatusCierre,StatusVenta,Comentarios" + campo + ",Entero,Tipodeventa, Correocliente, CorreoUsuario, FormaPago33, MetodoPago33, TipoComp33, UsoCFDI33, TipoRel33, CFDI_Rel33";

                if (AddVenta.uidRelacion != "")
                {
                    squery = squery + ",uidRelacion, tipoRelacion, tipoDocRelacionado";
                }
                    squery = squery + ") values(";
                

                squery = squery + "'" + idcliente + "'";
                squery = squery + ",'" + AddVenta.Idusuario + "'"; ;
                squery = squery + ",@Tienda";
                squery = squery + ",getdate()";

                STipoCliente = "C";

                squery = squery + ",@Folio";
                squery = squery + ",@Prefijo";
                squery = squery + ",@Sufijo";
                squery = squery + ",'VM'";

                squery = squery + ",'O','O',UPPER('" + AddVenta.Comentarios + "')" + anexo + "," + value + ",'" + typesale + "', '" + AddVenta.CorreoUsuario + "'," + "@EmailTienda " + ",'" + AddVenta.FormaPago33 + "','" + AddVenta.MetodoPago33 + "','" + AddVenta.TipoComp33 + "','" + AddVenta.UsoCFDI33 + "','" + AddVenta.TipoRel33 + "','" + AddVenta.CFDI_Rel33+"'";
               
                if (AddVenta.uidRelacion != "")
                {
                    squery = squery +",'" + AddVenta.uidRelacion + "','" + AddVenta.tipoRelacion + "','" + AddVenta.tipoDocRelacionado+"'";
                }
                
                
                    squery = squery + ")" + Environment.NewLine + Environment.NewLine;

                squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;

                squery = squery + " SET @UltimaVenta = (select MAX(ve.ID) from VentasEncabezado ve)" + Environment.NewLine;
                squery = squery + " Print @UltimaVenta" + Environment.NewLine;

                squery = squery + "Update storeFolios set currentfolio=@Folio where AdminStoreID=" + AddVenta.idstore + " and AdminFolioType=" + sCurrentFolio + "" + Environment.NewLine;
                squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;

                string sQueryEfectivo = "";
                double dEfectivo = 0;
                string FormaPago33 = "";
                string MetodoPago33 = "";
                string TipoComp33 = "";
                string UsoCFDI33 = "";
                string TipoRel33 = "";
                string CFDI_Rel33 = "";
                string CorreoCliente = "";
                string CorreoUsuario = "";
                int Parcialidad = 0;

                foreach (ArrayPagos pagos in AddVenta.ArrayPagos)
                {
                    var Forma = Obtenformapago(pagos);
                    if (pagos.Formapago != "Efectivo")
                    {
                        squery = squery + " insert into VentasPagos (IDVenta,AdminStoreID,TipoVenta,FormaPago,Monto,Fecha,StatusPago,NoCuenta,Prefijo,Folio,Afiliacion,TipoTarjeta,MSI,FormaPago33,MetodoPago33,TipoComp33,UsoCFDI33,TipoRel33,CFDI_Rel33,CorreoCliente,CorreoUsuario,Parcialidad,FechaPago) values (@UltimaVenta,@Tienda,'VT','" + pagos.Formapago + "'," + pagos.Monto + ",getdate(),'O','" + pagos.Cuenta + "',(select Prefijo from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2) ,(select CurrentFolio + 1 from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2),'" + pagos.Afiliacion + "','" + pagos.Tipotarjeta + "','" + pagos.MSISub + "', '" + pagos.FormaPago33 + "','" + pagos.MetodoPago33 + "', '" + pagos.TipoComp33 + "','" + pagos.UsoCFDI33 + "', '" + pagos.TipoRel33 + "'" + ",CONCAT((select Prefijo from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2), '-', @Folio)" + ",'" + pagos.CorreoCliente + "'," + "(select emailTienda from Adminstore where adminstoreid=" + AddVenta.idstore + ")," + pagos.Parcialidad + ", getdate())" + Environment.NewLine;
                        squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;
                        squery = squery + " Update  StoreFolios set CurrentFolio = CurrentFolio + 1 where AdminStoreID=@Tienda and AdminFolioType=2" + Environment.NewLine;
                        squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;
                        squery = squery + " update VentasPagos set TipoTarjeta='' where TipoTarjeta is null" + Environment.NewLine;
                    }
                    else
                        dEfectivo = dEfectivo + Convert.ToDouble(pagos.Monto);
                    FormaPago33 = pagos.FormaPago33;
                    MetodoPago33 = pagos.MetodoPago33;
                    TipoComp33 = pagos.TipoComp33;
                    UsoCFDI33 = pagos.UsoCFDI33;
                    TipoRel33 = pagos.TipoRel33;
                    CFDI_Rel33 = pagos.CFDI_Rel33;
                    CorreoCliente = pagos.CorreoCliente;
                    CorreoUsuario = pagos.CorreoUsuario;

                }

                if (dEfectivo > 0)
                {
                    var result = AddVenta.ArrayPagos.Where(x => x.Formapago == "Efectivo").ToList();
                    foreach (var item in result)
                    {
                        Parcialidad = item.Parcialidad;
                    }
                    squery = squery + "insert into VentasPagos (IDVenta,AdminStoreID,TipoVenta,FormaPago,Monto,Fecha,StatusPago,NoCuenta,Prefijo,Folio,TipoTarjeta,MSI,FormaPago33,MetodoPago33,TipoComp33,UsoCFDI33,TipoRel33,CFDI_Rel33,CorreoCliente,CorreoUsuario,Parcialidad,FechaPago) values (@UltimaVenta,@Tienda,'VT','Efectivo'," + dEfectivo + ",getdate(),'O','',(select Prefijo from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2) ,(select CurrentFolio + 1 from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2),'','','" + FormaPago33 + "','" + MetodoPago33 + "', '" + TipoComp33 + "','" + UsoCFDI33 + "', '" + TipoRel33 + "'" + ",CONCAT((select Prefijo from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2), '-', @Folio)" +
                ",'" + CorreoCliente + "'," + "(select emailTienda from Adminstore where adminstoreid=" + AddVenta.idstore + ")," + Parcialidad + ", getdate())" + Environment.NewLine;
                    squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;
                    squery = squery + " Update  StoreFolios set CurrentFolio = CurrentFolio + 1 where AdminStoreID=@Tienda and AdminFolioType=2" + Environment.NewLine;
                    squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;
                    squery = squery + " update VentasPagos set TipoTarjeta='' where TipoTarjeta is null" + Environment.NewLine;
                }
                var abonocero = Convert.ToBoolean(ConfigurationManager.AppSettings["abonocero"]);
                if (abonocero)
                {
                    dEfectivo = 0;
                    squery = squery + "insert into VentasPagos (IDVenta,AdminStoreID,TipoVenta,FormaPago,Monto,Fecha,StatusPago,NoCuenta,Prefijo,Folio,TipoTarjeta,MSI) values (@UltimaVenta,@Tienda,'VT','Efectivo'," + dEfectivo + ",getdate(),'O','',(select Prefijo from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2) ,(select CurrentFolio + 1 from StoreFolios  where AdminStoreID=@Tienda and AdminFolioType=2),'','')" + Environment.NewLine;
                    squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;
                    squery = squery + " Update  StoreFolios set CurrentFolio = CurrentFolio + 1 where AdminStoreID=@Tienda and AdminFolioType=2" + Environment.NewLine;
                    squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;
                    squery = squery + " update VentasPagos set TipoTarjeta='' where TipoTarjeta is null" + Environment.NewLine;
                }


                squery = squery + Environment.NewLine;

                // Captura de detalle y actualiza Inventarios/Series-------------------------------------------------------------------------

                string subtotal4Decimales;
                string subtotal2Decimales;

                string impuesto2Decimales;
                string impuesto4Decimales;

                string totalLinea2Decimales;

                int rowCount = 1;
                int lineasAInsertar = 0;
                int Detalles = 0;
                Detalles = AddVenta.ArrayArticulos.Count();
                if (Detalles > 0)
                {
                    foreach (ArrayArticulos oVenta in AddVenta.ArrayArticulos)
                    {
                    tagCheck:
                        ;
                        try
                        {
                            squery = squery + "Insert into ventasdetalle (";

                            squery = squery + "IDVenta,";
                            squery = squery + "IDLinea,";
                            squery = squery + "IDArticulo,";
                            squery = squery + "Juego,";
                            squery = squery + "Lista,";
                            squery = squery + "PrecioUnitario,";
                            squery = squery + "IVA,";
                            squery = squery + "Observaciones,";
                            squery = squery + "Descuento,";
                            if (!string.IsNullOrEmpty(oVenta.DescuentoJgo)) { squery = squery + "DescuentoJgo,"; }
                            squery = squery + "TotalLinea,";
                            squery = squery + "Cantidad,";
                            squery = squery + "StatusLinea,";
                            squery = squery + "IDStore,";
                            squery = squery + "Fecha";
                            squery = squery + ",JaliscoConsigna";
                            squery = squery + ",CodigoIva";
                            squery = squery + ") values(";
                            squery = squery + "@UltimaVenta"; // IDVenta
                            squery = squery + ",'" + rowCount + "'"; // IDLinea
                            squery = squery + ",'" + getItemID(oVenta.Linea) + "'"; // IDArticulo
                            squery = squery + ",'" + oVenta.Juego + "'"; // Juego
                            squery = squery + ",'" + oVenta.Lista + "'"; // Lista

                            double digito = double.Parse(oVenta.PrecioUnitario, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                            string Punitario = digito.ToString("F4");
                            double digitoiva = double.Parse(oVenta.IVA, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                            string Iva = digitoiva.ToString("F4");
                            double digitodesc = double.Parse(oVenta.Descuento, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                            string descuento = digitodesc.ToString("F4");
                            double digitototal = double.Parse(oVenta.Total, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                            string total = digitototal.ToString("F4");

                            squery = squery + ",'" + Punitario + "'";
                            squery = squery + ",'" + Iva + "'"; // IVA
                            squery = squery + ",'" + "v2" + "'"; // Observaciones
                            squery = squery + ",'" + descuento + "'"; // Descuento
                            if (!string.IsNullOrEmpty(oVenta.DescuentoJgo))
                            {
                                double digitodescJgo = double.Parse(oVenta.DescuentoJgo, NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                                string descuentoJgo = digitodescJgo.ToString("F4");
                                squery = squery + ",'" + descuentoJgo + "'";
                            }
                            squery = squery + ",'" + total + "'"; // TotalLinea
                            var cantidadgeneral = (Convert.ToInt32(oVenta.CantidadTienda) + Convert.ToInt32(oVenta.CantidadBodega));
                            double digitocantge = double.Parse(cantidadgeneral.ToString(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                            string cantigen = digitocantge.ToString("F4");
                            squery = squery + ",'" + cantigen + "'"; // Cantidad NUEVA FORMA
                            squery = squery + ",'O'"; // StatusLinea

                            if (Convert.ToInt32(oVenta.CantidadTienda) > 0)
                                squery = squery + "," + AddVenta.idstore; // IDStore
                            else
                                squery = squery + ",(select whsconsigid from AdminStore where AdminStoreID=" + AddVenta.idstore + ")";
                            squery = squery + ",getdate()"; // Fecha
                            squery = squery + ",0" + Environment.NewLine + Environment.NewLine;
                            squery = squery + ",(select COD_IVA_SAP from IVA where PORCENTAJE=(select actIVA from AdminStore where AdminStoreID=" + AddVenta.idstore + ")))" + Environment.NewLine + Environment.NewLine;
                            squery = squery + " SELECT @intErrorCode = @@ERROR IF (@intErrorCode <> 0) GOTO PROBLEM" + Environment.NewLine;

                            lineasAInsertar = lineasAInsertar + 1;

                            rowCount = rowCount + 1;
                        }
                        catch (Exception ex)
                        {
                            // log
                            string sQuery1 = "";
                            // aproximado a 1 idventa
                            sQuery1 = "insert into Log_tras (id_venta,id_store,funcion,Message,fecha) values ( (select MAX(ve.ID) from VentasEncabezado ve) ," + AddVenta.idstore + "," + "'PEDIDO SIN DETALLAES'" + ",'" + Detalles + "'" + ",Getdate())";
                            dt = obj.EjecutaQry_Tabla(sQuery1.ToString(), CommandType.Text, "Error", connstringWEB);
                            // 'cometemos eror de la siguiente linea para que no inserte nada
                            squery = squery + "Insert into ventasdetalle (";
                        }
                    }
                }
                else
                {
                    // log
                    string sQuery1 = "";
                    // aproximado a 1 idventa
                    sQuery1 = "insert into Log_tras (id_venta,id_store,funcion,Message,fecha) values ( (select MAX(ve.ID) from VentasEncabezado ve) ," + AddVenta.idstore + "," + "'PEDIDO SIN DETALLAES'" + ",'" + Detalles + "'" + ",Getdate())";
                    dt = obj.EjecutaQry_Tabla(sQuery1.ToString(), CommandType.Text, "Error", connstringWEB);
                    // 'cometemos eror de la siguiente linea para que no inserte nada
                    squery = squery + "Insert into ventasdetalle (";
                }
                squery = squery + " SET @LineasAInsertar = " + lineasAInsertar + Environment.NewLine;

                squery = squery + " SET @LineasInsertadas = (select COUNT(*) from VentasDetalle where IDVenta=@UltimaVenta)" + Environment.NewLine;
                squery = squery + " PRINT @LineasInsertadas" + Environment.NewLine + Environment.NewLine;

                squery = squery + " IF (@LineasAInsertar <> @LineasInsertadas) GOTO PROBLEM" + Environment.NewLine;

                squery = squery + " IF (" + idcliente + " = 70) GOTO PROBLEM" + Environment.NewLine; ///Aqui va el id de cliente

                squery = squery + " Print 'Transaccion Exitosa'" + Environment.NewLine + Environment.NewLine;

                squery = squery + " commit transaction" + Environment.NewLine + Environment.NewLine;

                squery = squery + " PROBLEM:" + Environment.NewLine;
                squery = squery + " IF (@intErrorCode <> 0) or (@LineasAInsertar <> @LineasInsertadas) OR (@IMEIS <> '') OR (@ICCS<>'') OR (@intErrorCode<>0) or (" + idcliente + " = 70) BEGIN" + Environment.NewLine;
                squery = squery + " Print 'Ocurrio un error'" + Environment.NewLine;
                squery = squery + " ROLLBACK transaction" + Environment.NewLine;
                squery = squery + "  RAISERROR(@ERROR,18,1)" + Environment.NewLine;
                squery = squery + " End" + Environment.NewLine + Environment.NewLine;

                string sError = "";

                if (obj.EjecutaQry(squery, CommandType.Text, connstringWEB, sError) == 2)
                {
                    LogWEB.anade_linea_archivo2(LogWeb, "Error al insertar venta: " + sError);
                    //Ext.Net.Notification msg = new Ext.Net.Notification();
                    //Ext.Net.NotificationConfig nconf = new Ext.Net.NotificationConfig();
                    //nconf.IconCls = "icon-exclamation";

                    //if (sError.Contains("EL IMEI") | sError.Contains("EL ICC"))
                    //    nconf.Html = sError;
                    //else
                    //    nconf.Html = "Se encontro un problema al procesar la venta, por favor seleccione nuevamente al cliente e intente de nuevo <br> Si el problema persiste por favor cancele la venta y vuélvalo a intentar<BR>" + sError;

                    //nconf.Title = "Error";
                    //msg.Configure(nconf);
                    //msg.Show();
                    return false;
                }
                try
                {
                    System.Data.SqlClient.SqlParameter Parameter = new System.Data.SqlClient.SqlParameter()
                    {
                        ParameterName = "@Id",
                        Value = Convert.ToInt32(idcliente)
                    };
                    int res = obj.EjecutaQry_T("UpdateImportSAPCustomer", "", Parameter, CommandType.StoredProcedure, connstringWEB);
                }
                catch (Exception exce)
                {
                    LogWEB.anade_linea_archivo2(LogWeb, "Error al actualizar cliente importado id= " + idcliente + "Error: " + exce.Message);
                }
                var ventaid = getLastSale(AddVenta.Idusuario);
                int IDPRINTVENTA = Convert.ToInt32(ventaid);
                int IDSTORE = Convert.ToInt32(AddVenta.idstore);
                var WHSID = AddVenta.WhsID;
                returnventa infoventa = new returnventa(IDPRINTVENTA, IDSTORE, WHSID);
                infoventas = infoventa;
                return true;
            }
            catch (Exception ex)
            {
                var error = "";
                error = ex.Message;
                LogWEB.anade_linea_archivo2(LogWeb, "Error en metodo addVenta: " + error);
                return false;
            }
        }
        public Boolean Obtenformapago(ArrayPagos pagos)
        {
            Boolean formapago33 = true;
            switch (pagos.FormaPago33)
            {

                case "Efectivo":
                    pagos.FormaPago33 = "01";
                    break;

                case "Terminal Banamex":
                case "Terminal Bancomer":
                case "Terminal Banorte":
                case "Terminal PROSA":
                case "Terminal Santander":
                case "American Express":
                    switch (pagos.Tipotarjeta)
                    {
                        case "Credito": pagos.FormaPago33 = "04"; break;
                        case "Debito": pagos.FormaPago33 = "28"; break;
                        case "Crédito": pagos.FormaPago33 = "04"; break;
                        case "Débito": pagos.FormaPago33 = "28"; break;
                        case "": pagos.FormaPago33 = ""; break;
                    }
                    break;
                case "Deposito / Cheque":
                    pagos.FormaPago33 = "02";
                    break;
                case "Transferencia Electronica":
                    pagos.FormaPago33 = "03"; break;
                case "Transferencia Electrónica":
                    pagos.FormaPago33 = "03"; break;
                case "Transf. Elect. Dormicredit St.":
                    pagos.FormaPago33 = "03"; break;
                case "Compensación":
                    pagos.FormaPago33 = "17"; break;
                default:
                    pagos.FormaPago33 = "99";
                    break;
            }
            return formapago33;
        }
        public string getLastSale(string user)
        {
            string sQuery;
            DataTable dt = new DataTable();
            Utilities.DBMaster oDB = new Utilities.DBMaster();
            sQuery = "select max(id) as LastSale from ventasencabezado where iduser=" + user;

            dt = oDB.EjecutaQry_Tabla(sQuery, CommandType.Text, "LastSale", connstringWEB);

            foreach (DataRow Drow in dt.Rows)
            {
                if (Convert.IsDBNull(Drow["LastSale"]) == false)
                    return Drow["LastSale"].ToString();
            }

            return "0";

            dt = null/* TODO Change to default(_) if this is not a reference type */;
            GC.Collect();
        }
        public string getItemID(string Item)
        {
            connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
            string sQuery;
            DataTable dt = new DataTable();
        retry:
            ;
            sQuery = "select IDArticulo from articulos where ArticuloSBO = '" + Item + "'";
            Utilities.DBMaster obj = new Utilities.DBMaster();
            dt = obj.EjecutaQry_Tabla(sQuery.ToString(), CommandType.Text, "IDArticulo", connstringWEB);

            foreach (DataRow Drow in dt.Rows)
                return Drow["IDArticulo"].ToString();
            goto retry;
            return "0";
            dt = null;
            GC.Collect();
        }

        public JsonResult GetFacturaUID(string uid)
        {
            ComodinFacturaUIDDOC ObFact = new ComodinFacturaUIDDOC();

            connstringWEB = ConfigurationManager.ConnectionStrings["FactDB"].ConnectionString;
            string sQuery;
            DataTable dt = new DataTable();
        retry:
            ;
            sQuery = "select DocEntry,U_FOLPOS, DocNum from OINV where U_BXP_UUID = '" + uid + "'";
            Utilities.DBMaster obj = new Utilities.DBMaster();
            dt = obj.EjecutaQry_Tabla(sQuery.ToString(), CommandType.Text, "DocEntry,U_FOLPOS,DocNum", connstringWEB);
                        
            foreach (DataRow Drow in dt.Rows)
            {
                ObFact.docEntry = Drow["DocEntry"].ToString();
                ObFact.fPos = Drow["U_FOLPOS"].ToString();
                ObFact.fact= Drow["DocNum"].ToString();
            }
            dt.ToString();
            return Json(ObFact, JsonRequestBehavior.AllowGet);
        }

        //METODO PARA EXTRAER AL CLIENTE POR ID
        //public JsonResult getClienteId(string idCliente)
        //{           
        //    connstringWEB = ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString;
        //    string sQuery;
        //    DataTable dt = new DataTable();
        //retry:
        //    ;
        //    sQuery = "select * from clientes where id = "+idCliente;
        //    Utilities.DBMaster obj = new Utilities.DBMaster();
        //    dt = obj.EjecutaQry_Tabla(sQuery.ToString(), CommandType.Text, "*", connstringWEB);

           
        //    dt.ToString();
        //    return Json(dt, JsonRequestBehavior.AllowGet);
        //}



    }
}