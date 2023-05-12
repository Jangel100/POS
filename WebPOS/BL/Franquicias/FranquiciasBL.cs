using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using BL.Interface;
using DAL.Franquicias;
using DAL.Utilities;
using Entities;
using Entities.Models.Payback;
using Entities.viewsModels;
using Newtonsoft.Json;
using Entities.Models.Garantias;

namespace BL.Franquicias
{
    public class FranquiciasBL : IFranquiciasBL
    {
        readonly IResponseAPI _Franquicias;

        public FranquiciasBL(FranquiciasDAL FranquiciasDAL)
        {
            _Franquicias = FranquiciasDAL;
        }
        public FranquiciasBL()
        {
            _Franquicias = new FranquiciasDAL();
        }
        public HttpResponseMessage GetResponseAPI(string ur)
        {
            return _Franquicias.GetResponseAPI(ur);
        }
        public HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj)
        {
            try
            {
                return _Franquicias.ReadAsStringAsyncAPI(url, obj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<ListListOfPriceView> GetPriceF(string store, string SAPDB)
        {
            try
            {
                var StoreSap = new
                {
                    Store = store,
                    SAPDB = SAPDB
                };
                string json_data = JsonConvert.SerializeObject(StoreSap);
                var ResponseList = GetResponseAPI($"api/GetPriceF?StoreSap=" + json_data + "");
                ResponseList.EnsureSuccessStatusCode();
                var PriceViewFResult = ResponseList.Content.ReadAsAsync<List<ListListOfPriceView>>().Result;

                return PriceViewFResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ListModeloView> GetModelosF(string Tipo)
        {

            List<ListModeloView> ModelosViewResult;
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetModelosF?Tipo=" + Tipo + "");
                ResponseVenta.EnsureSuccessStatusCode();
                ModelosViewResult = ResponseVenta.Content.ReadAsAsync<List<ListModeloView>>().Result;

                return ModelosViewResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ListArticuloView> GetArticulosF()
        {

            string url = string.Empty;
            List<ListArticuloView> GetArticulosF = new List<ListArticuloView>();
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetArticulosF");
                ResponseVenta.EnsureSuccessStatusCode();
                GetArticulosF = ResponseVenta.Content.ReadAsAsync<List<ListArticuloView>>().Result;

                return GetArticulosF;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public List<ListArticuloCVEView> GetArticulosFCve()
        {

            string url = string.Empty;
            List<ListArticuloCVEView> GetArticulosFCve = new List<ListArticuloCVEView>();
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetArticulosFCve");
                ResponseVenta.EnsureSuccessStatusCode();
                GetArticulosFCve = ResponseVenta.Content.ReadAsAsync<List<ListArticuloCVEView>>().Result;

                return GetArticulosFCve;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public List<ListArticuloCodeView> GetArticulosFCode()
        {

            string url = string.Empty;
            List<ListArticuloCodeView> GetArticulosFCode = new List<ListArticuloCodeView>();
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetArticulosFCode");
                ResponseVenta.EnsureSuccessStatusCode();
                GetArticulosFCode = ResponseVenta.Content.ReadAsAsync<List<ListArticuloCodeView>>().Result;

                return GetArticulosFCode;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public string GetExistenciasF(string Parametrs)
        {
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetQuantityStoreAndwineryF?JsonParametrs=" + Parametrs + "");
                ResponseVenta.EnsureSuccessStatusCode();
                var LineaViewResult = ResponseVenta.Content.ReadAsStringAsync().Result;
                return LineaViewResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public double GetUnitPriceF(string JsonUnitPrice)
        {
            double IvaResponse = 0;
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetUnitPriceF?JsonUnitPrice=" + JsonUnitPrice + "");
                ResponseVenta.EnsureSuccessStatusCode();
                IvaResponse = Convert.ToDouble(ResponseVenta.Content.ReadAsStringAsync().Result);

                return IvaResponse;
            }
            catch (Exception ex)
            {
                return IvaResponse;
            }
        }
        public int GetIvaF(string Idstore)
        {
            int IvaResponse = 0;
            try
            {
                var ResponseVenta = GetResponseAPI($"api/GetIvaF?idStore=" + Idstore + "");
                ResponseVenta.EnsureSuccessStatusCode();
                IvaResponse = Convert.ToInt32(ResponseVenta.Content.ReadAsStringAsync().Result);

                return IvaResponse;
            }
            catch (Exception ex)
            {
                return IvaResponse;
            }
        }
        public ChangePriceView GetChangePriceF(double Iva, double Price, string descuento, int cantidad)
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
        public ResponseJgoView GetListJgoF(ChangePriceView price, string IdList, string ItemName, string Origen, string Cantidad, string ItemCode)
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
                        CAlmacen = Origen.Equals("Almacen") ? Cantidad :"0",
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
        public ResponseJgoView GetBoxF(string JsonCombo)
        {
            try
            {

                var RequestBox = GetResponseAPI($"api/GetComboLineF?JsonComboLine=" + JsonCombo + "");
                RequestBox.EnsureSuccessStatusCode();
                var ResponseBox = RequestBox.Content.ReadAsAsync<ResponseJgoView>().Result;
                return ResponseBox;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public RequestRegisterPaybackSale CreateObjRegisterPaybackSaleF(string Idventa, string BranchShortName, DateTime EfectiveTime, decimal LegalAmountRLegalV, decimal LegalAmountTPV, decimal LoyaltyAmountRLoyalV, string monedero, string NIP, string PartnerShortName, string ReceipNumber, string RedemptionNumber, string ReferenceReceipNumber, string TypeTransaction, List<DetailsAcumulationRequest> details, bool status, string MessageError, decimal TotalPuntosPayback, decimal PuntosPaybackAcumulados, decimal PuntosPaybackRedimidos, string Idabono)
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
                Idabono = Idabono,
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

        public List<ListFolio> GetFoliosF(string jsonparams)
        {
            List<ListFolio> FolioView;
            try
            {
                var Responsefolios = GetResponseAPI($"api/GetFoliosF?jsonparams=" + jsonparams + "");
                Responsefolios.EnsureSuccessStatusCode();
                FolioView = Responsefolios.Content.ReadAsAsync<List<ListFolio>>().Result;

                return FolioView;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ListDia> GetDaysF(string jsonparams)
        {
            List<ListDia> DaysView;
            try
            {
                var Responsedays = GetResponseAPI($"api/GetDaysF?jsonparams=" + jsonparams + "");
                Responsedays.EnsureSuccessStatusCode();
                DaysView = Responsedays.Content.ReadAsAsync<List<ListDia>>().Result;

                return DaysView;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ListPeriodo> GetPeriodsF(string idcliente)
        {
            List<ListPeriodo> PeriodsViewResult;
            try
            {
                var ResponsePer = GetResponseAPI($"api/GetPeriodsF?idcliente=" + idcliente + "");
                ResponsePer.EnsureSuccessStatusCode();
                PeriodsViewResult = ResponsePer.Content.ReadAsAsync<List<ListPeriodo>>().Result;

                return PeriodsViewResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<GarantiasIn> GetGarantias()
        {
            List<GarantiasIn> GetGarantias = new List<GarantiasIn>();
            try
            {
                var ResponGarantias = GetResponseAPI($"api/GetGarantias");
                ResponGarantias.EnsureSuccessStatusCode();
                GetGarantias = ResponGarantias.Content.ReadAsAsync<List<GarantiasIn>>().Result;
                return GetGarantias;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string ConfirmaEntrega(string Parametros)
        {
            try
            {
                var ResponseConfirma = GetResponseAPI($"api/GetConfirmaEntregaF?JsonParametrs=" + Parametros + "");
                            //var result = response.Content.ReadAsAsync<AddSale>().Result;
                ResponseConfirma.EnsureSuccessStatusCode();
                var LineaViewResult = ResponseConfirma.Content.ReadAsStringAsync().Result;
                return LineaViewResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<GarantiasIn> GetGarantiasxAprobar()
        {
            List<GarantiasIn> GetGaraxAprobar = new List<GarantiasIn>();
            try
            {
                var ResponseGaraxAprobar = GetResponseAPI($"api/GetGarantiasxAprobar");
                ResponseGaraxAprobar.EnsureSuccessStatusCode();
                GetGaraxAprobar = ResponseGaraxAprobar.Content.ReadAsAsync<List<GarantiasIn>>().Result;
                return GetGaraxAprobar;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public GarantiasIn ResponseAction(string JsonString)
        {
            try
            {
                GarantiasIn ResponseGa = new GarantiasIn();
                var ResponseGarantia = GetResponseAPI($"api/GetResponseAction?JsonString=" + JsonString + "");
                ResponseGarantia.EnsureSuccessStatusCode();
                ResponseGa = ResponseGarantia.Content.ReadAsAsync<GarantiasIn>().Result;
               
                return ResponseGa;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
