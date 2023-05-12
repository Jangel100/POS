using BL.Interface;
using DAL.Abonos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Entities.viewsModels;
using Entities.Models.Payback;
using System.Linq;
namespace BL.Abonos
{
    public class AbonosBL : IAbonosBL
    {
        readonly DAL.Utilities.IResponseAPI _Abonos;
        public AbonosBL(AbonosDAL AbonosDAL)
        {
            _Abonos = AbonosDAL;
        }
        public AbonosBL()
        {
            _Abonos = new AbonosDAL();
        }
        public HttpResponseMessage GetResponseAPI(string ur)
        {
            return _Abonos.GetResponseAPI(ur);
        }
        public HttpResponseMessage GetResponseAPIVentasUsuarios(string url)
        {
            try
            {
                return _Abonos.GetResponseAPI(url);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj)
        {
            try
            {
                return _Abonos.ReadAsStringAsyncAPI(url, obj);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<ListPeriodo> GetPeriods(string idcliente)
        {
            List<ListPeriodo> PeriodsViewResult;
            try
            {
                var ResponsePer = GetResponseAPI($"api/GetPeriods?idcliente=" + idcliente + "");
                ResponsePer.EnsureSuccessStatusCode();
                PeriodsViewResult = ResponsePer.Content.ReadAsAsync<List<ListPeriodo>>().Result;

                return PeriodsViewResult;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ListDia> GetDays(string jsonparams)
        {
            List<ListDia> DaysView;
            try
            {
                var Responsedays = GetResponseAPI($"api/GetDays?jsonparams=" + jsonparams + "");
                Responsedays.EnsureSuccessStatusCode();
                DaysView = Responsedays.Content.ReadAsAsync<List<ListDia>>().Result;

                return DaysView;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ListFolio> GetFolios(string jsonparams)
        {
            List<ListFolio> FolioView;
            try
            {
                var Responsefolios = GetResponseAPI($"api/GetFolios?jsonparams=" + jsonparams + "");
                Responsefolios.EnsureSuccessStatusCode();
                FolioView = Responsefolios.Content.ReadAsAsync<List<ListFolio>>().Result;

                return FolioView;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public RequestRegisterPaybackSale CreateObjRegisterPaybackSale(string Idventa, string BranchShortName, DateTime EfectiveTime, decimal LegalAmountRLegalV, decimal LegalAmountTPV, decimal LoyaltyAmountRLoyalV, string monedero, string NIP, string PartnerShortName, string ReceipNumber, string RedemptionNumber, string ReferenceReceipNumber, string TypeTransaction, List<DetailsAcumulationRequest> details, bool status, string MessageError, decimal TotalPuntosPayback, decimal PuntosPaybackAcumulados, decimal PuntosPaybackRedimidos, string Idabono)
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
    }
}
