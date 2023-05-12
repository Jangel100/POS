using BL.Interface;
using DAL.Configuracion;
using DAL.Utilities.Interface;
using Entities;
using Entities.Models.Payback;
using Entities.Models.Ventas;
using Entities.viewsModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace BL.Configuracion
{
    public class VentasBL : IVentasBL
    {
        readonly DAL.Utilities.IResponseAPI _Ventas;
        public VentasBL(VentasDAL VentasDAL)
        {
            _Ventas = VentasDAL;
        }

        public VentasBL()
        {
            _Ventas = new VentasDAL();
        }


        public HttpResponseMessage GetResponseAPI(string ur)
        {
            return _Ventas.GetResponseAPI(ur);
        }

        public HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj)
        {
            return _Ventas.ReadAsStringAsyncAPI(url, obj);
        }

        public List<ListArticuloView> GetArticulos()
        {

            string url = string.Empty;
            List<ListArticuloView> TiendasViewResult = new List<ListArticuloView>();
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetArticulos");
                ResponseVenta.EnsureSuccessStatusCode();
                TiendasViewResult = ResponseVenta.Content.ReadAsAsync<List<ListArticuloView>>().Result;

                return TiendasViewResult;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<ListModeloView> GetModelos(string Tipo)
        {

            List<ListModeloView> ModelosViewResult;
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetModelos?Tipo=" + Tipo + "");
                ResponseVenta.EnsureSuccessStatusCode();
                ModelosViewResult = ResponseVenta.Content.ReadAsAsync<List<ListModeloView>>().Result;

                return ModelosViewResult;
                //return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ListLineaView> GetListLinea(string code)
        {
            List<ListLineaView> LineaViewResult;
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetLinea?code=" + code + "");
                ResponseVenta.EnsureSuccessStatusCode();
                LineaViewResult = ResponseVenta.Content.ReadAsAsync<List<ListLineaView>>().Result;

                return LineaViewResult;
                //return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ListMedidaView> GetMedida(string code)
        {
            List<ListMedidaView> LineaViewResult;
            try
            {

                var ResponseVenta = GetResponseAPI($"api/GetMedida?JsonMedida=" + code + "");
                ResponseVenta.EnsureSuccessStatusCode();
                LineaViewResult = ResponseVenta.Content.ReadAsAsync<List<ListMedidaView>>().Result;

                return LineaViewResult;
                //return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetExistencias(string Parametrs)
        {

            try
            {

                var ResponseVenta = GetResponseAPI($"api/GetQuantityStoreAndwinery?JsonParametrs=" + Parametrs + "");
                ResponseVenta.EnsureSuccessStatusCode();
                var LineaViewResult = ResponseVenta.Content.ReadAsStringAsync().Result;

                return LineaViewResult;
                //return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ListListOfPriceView> GetPrice(string store, string SAPDB)
        {
            try
            {
                var StoreSap = new
                {
                    Store = store,
                    SAPDB = SAPDB
                };
                string json_data = JsonConvert.SerializeObject(StoreSap);
                var ResponseVenta = GetResponseAPI($"api/GetPrice?StoreSap=" + json_data + "");
                ResponseVenta.EnsureSuccessStatusCode();
                var PriceViewResult = ResponseVenta.Content.ReadAsAsync<List<ListListOfPriceView>>().Result;

                return PriceViewResult;
                //return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int GetIva(string Idstore)
        {
            int IvaResponse = 0;
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetIva?idStore=" + Idstore + "");
                ResponseVenta.EnsureSuccessStatusCode();
                IvaResponse = Convert.ToInt32(ResponseVenta.Content.ReadAsStringAsync().Result);

                return IvaResponse;
                //return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return IvaResponse;
            }
        }

        public double GetUnitPrice(string JsonUnitPrice)
        {
            double IvaResponse = 0;
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetUnitPrice?JsonUnitPrice=" + JsonUnitPrice + "");
                ResponseVenta.EnsureSuccessStatusCode();
                IvaResponse = Convert.ToDouble(ResponseVenta.Content.ReadAsStringAsync().Result);

                return IvaResponse;
                //return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return IvaResponse;
            }
        }

        public ChangePriceView GetChangePrice(double Iva, double Price, string descuento, int cantidad)
        {
            ChangePriceView ResponseChangePriceView = new ChangePriceView();
            double _Descuento = 0;
            double _dPrecioConDescuento = 0;
            double _dPrecioUnitario = 0;
            double _dIvaUnitario = 0;
            double _dIVATotal = 0;
            double _dMonto = 0;
            double _dTotal = 0;
            double _Subtotal = 0;
            double _DescuentoPorc = 0;
            _dPrecioUnitario = Price / (1 + Iva / 100);
            _Descuento = string.IsNullOrEmpty(descuento) ? 0 : Convert.ToDouble(descuento);
            ResponseChangePriceView.Descuento = _Descuento.ToString();
            _dPrecioConDescuento = _dPrecioUnitario * (1 - (_Descuento / 100));
            _dIvaUnitario = Price * (1 - (_Descuento / 100)) - _dPrecioConDescuento;
            //Convert.ToDouble(string.Format("{0:N2}", _dPrecioUnitario));           
            ResponseChangePriceView.PrecioUnitario = string.Format("{0:c}", _dPrecioUnitario);
            _dIVATotal = _dIvaUnitario * cantidad;
            ResponseChangePriceView.IVA = string.Format("{0:c}", _dIVATotal);
            _dMonto = (_dPrecioUnitario * cantidad) + _dIVATotal;
            _dTotal = (_dPrecioConDescuento * cantidad) + _dIVATotal;
            ResponseChangePriceView.Total = string.Format("{0:c}", _dTotal);           
            _DescuentoPorc = (_dMonto - _dIVATotal) * (_Descuento / 100);
            _Subtotal = (_dPrecioUnitario * cantidad) - _DescuentoPorc;//(cantidad - (_Descuento / 100));
            ResponseChangePriceView.Subtotal = string.Format("{0:c}", _Subtotal);
            ResponseChangePriceView.DescuentoPrec = string.Format("{0:c}", _DescuentoPorc);
            return ResponseChangePriceView;
        }



        public DataTable GetQueryDBconnSap(string idstore, string Modelo)
        {
            string Value = string.Empty;
            string squery = string.Empty;
            string Id = string.Empty;
            string pArticulo = string.Empty;
            string pLinea = string.Empty;
            string pMedida = string.Empty;
            string pModelo = string.Empty;
            string pCantidad = string.Empty;
            double pCantidadTienda = 0;
            double pCantidadBodega = 0;
            string pLista = string.Empty;
            double pPrecioUnitario = 0;
            double pIVA = 0;
            double pMonto = 0;
            double pDescuento = 0;
            double pTotal = 0;
            double tiempodesurtido = 0;
            bool Estienda = false;
            bool Esbodegac = false;
            bool Requeststore = false;
            bool Requestcellar = false;
            string SQuery = string.Empty;
            bool Origenoriginal = false;//debe validar el origen con la variable de sesion


            try
            {

                var ResponseVenta = GetResponseAPI($"api/GetQuantityStoreAndwinery?JsonParametrs=" + SQuery + "");
                ResponseVenta.EnsureSuccessStatusCode();
                var LineaViewResult = ResponseVenta.Content.ReadAsByteArrayAsync().Result;

                return null;
                //return Json(JsonResult);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ListModeloView> GetModelsSelected(string JsonParams)
        {
            List<ListModeloView> ModelosViewResult;
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetModelsSelected?JsonParams=" + JsonParams + "");
                ResponseVenta.EnsureSuccessStatusCode();
                ModelosViewResult = ResponseVenta.Content.ReadAsAsync<List<ListModeloView>>().Result;
                return ModelosViewResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetQueryDBconn(string Query)
        {
            throw new NotImplementedException();
        }
        public ResponseJuego GetCombo(string parameter)
        {
            try
            {

                var RequestJgo = GetResponseAPI($"api/GetCombo?Parameters=" + parameter + "");
                RequestJgo.EnsureSuccessStatusCode();
                var ResponseJuego = RequestJgo.Content.ReadAsAsync<ResponseJuego>().Result;
                //ReadAsByteArrayAsync().Result;

                //return null; ResponseVenta.Content.ReadAsAsync<List<ListListOfPriceView>>;
                return ResponseJuego;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ResponseJgoView GetListJgo(ChangePriceView price, string IdList, string ItemName, string Origen, string Cantidad, string ItemCode)
        {
            try
            {
                if (price != null)
                {
                    ResponseJgoView List = (new ResponseJgoView
                    {
                        Id = "1",
                        Modelo = ItemName,
                        Juego = "",
                        Cantidad = Cantidad,
                        CTienda = Origen.Equals("Tienda") ? Cantidad : "0",
                        CBodega = Origen.Equals("Bodega") ? Cantidad : "0",
                        Lista = IdList,
                        PrecioUnitario = price.PrecioUnitario,
                        Descuento = price.DescuentoPrec,
                        IVA = price.IVA,
                        Subtotal = price.Subtotal,
                        Total = price.Total,
                        ItemCode = ItemCode
                    }); ;

                    return List;
                }
                return null;
            }
            catch (Exception ex) { return null; }

        }

        public ResponseJgoView GetBox(string JsonCombo)
        {
            try
            {

                var RequestBox = GetResponseAPI($"api/GetComboLine?JsonComboLine=" + JsonCombo + "");
                RequestBox.EnsureSuccessStatusCode();
                var ResponseBox = RequestBox.Content.ReadAsAsync<ResponseJgoView>().Result;
                //ReadAsByteArrayAsync().Result;

                //return null; ResponseVenta.Content.ReadAsAsync<List<ListListOfPriceView>>;
                return ResponseBox;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public RequestRegisterPaybackSale CreateObjRegisterPaybackSale(string Idventa, string BranchShortName, DateTime EfectiveTime, decimal LegalAmountRLegalV, decimal LegalAmountTPV, decimal LoyaltyAmountRLoyalV, string monedero, string NIP, string PartnerShortName, string ReceipNumber, string RedemptionNumber, string ReferenceReceipNumber, string TypeTransaction, List<DetailsAcumulationRequest> details, bool status, string MessageError, decimal TotalPuntosPayback, decimal PuntosPaybackAcumulados, decimal PuntosPaybackRedimidos, bool SuccesRedemption, bool SuccesAcumulation)
        {
            var obj = (new RequestRegisterPaybackSale
            {
                Idventa = Idventa,
                BranchShortName = BranchShortName,
                EfectiveTime = EfectiveTime,
                LegalAmountRLegalV = LegalAmountRLegalV,
                LegalAmountTPV = LegalAmountTPV,
                LoyaltyAmountRLoyalV = LoyaltyAmountRLoyalV,
                monedero = monedero,
                NIP = NIP,
                PartnerShortName = PartnerShortName,
                ReceipNumber = ReceipNumber,
                RedemptionNumber = RedemptionNumber,
                ReferenceReceipNumber = ReferenceReceipNumber,
                TypeTransaction = TypeTransaction,
                Status = status,
                MessageError = MessageError,
                TotalPuntosPayback = TotalPuntosPayback,
                PuntosPaybackAcumulados = PuntosPaybackAcumulados,
                PuntosPaybackRedimidos = PuntosPaybackRedimidos,
                SuccesAcumulation = SuccesAcumulation,
                SuccesRedemption = SuccesRedemption,
                ListDetails =

                        (from DetailsAcumulationRequest rows in details
                         select new ListDetails
                         {
                             //PointsAmount = LegalAmountRLegalV,
                             ArticleEanCode = rows.ArticleEanCode,
                             PartnerProductGroupCode = rows.PartnerProductGroupCode,
                             PartnerProductCategoryCode = rows.PartnerProductCategoryCode,
                             //QuantityUnitCode = rows.QuantityUnitCode,
                             Quantity = Convert.ToDecimal(rows.QuantityUnitCode),
                             TotalTurnoverAmount = rows.SingleTurnoverAmount,
                             TotalRewardableAmount = rows.TotalRewardableAmount
                         }).ToList()

            });
            return obj;
        }
        public AddSale Getdatepedido(string Parameters)
        {
            try
            {
                AddSale Pedidopos = new AddSale();
                var ResponseVenta = GetResponseAPI($"api/Getdatepedido?JsonParameters=" + Parameters + "");
                ResponseVenta.EnsureSuccessStatusCode();
                var Results = ResponseVenta.Content.ReadAsStringAsync().Result;

                Pedidopos = ResponseVenta.Content.ReadAsAsync<AddSale>().Result;
                return Pedidopos;
                 
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string UpdateFechaEn(string Parametros)
        {
            try
            {
                var ResponseUpdate = GetResponseAPI($"api2/GetFechaEntrega?Parametros=" + Parametros + "");
                ResponseUpdate.EnsureSuccessStatusCode();
                var Responseupdate = ResponseUpdate.Content.ReadAsStringAsync().Result;

                return Responseupdate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<PromocionDosPorUno> GetInfoDosPorUno()
        {
            try
            {
                var ResponseUpdate = GetResponseAPI($"api5/GetInfoDosPorUno");
                ResponseUpdate.EnsureSuccessStatusCode();
                var Responseupdate = ResponseUpdate.Content.ReadAsAsync<List<PromocionDosPorUno>>().Result;

                return Responseupdate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
