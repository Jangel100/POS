using Entities;
using Entities.Models.Garantias;
using Entities.Models.Payback;
using Entities.viewsModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BL.Interface
{
    public interface IFranquiciasBL
    {
        HttpResponseMessage GetResponseAPI(string url);
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
        List<ListArticuloView> GetArticulosF();
        List<ListArticuloCVEView> GetArticulosFCve();
        List<ListArticuloCodeView> GetArticulosFCode();
        List<ListModeloView> GetModelosF(string Tipo);
        List<ListListOfPriceView> GetPriceF(string store, string SAPDB);
        string GetExistenciasF(string Parametrs);
        double GetUnitPriceF(string JsonUnitPrice);
        int GetIvaF(string Idstore);
        ChangePriceView GetChangePriceF(double Iva, double Price, string descuento, int cantidad);
        RequestRegisterPaybackSale CreateObjRegisterPaybackSale(string Idventa, string BranchShortName, DateTime EfectiveTime, decimal LegalAmountRLegalV, decimal LegalAmountTPV, decimal LoyaltyAmountRLoyalV, string monedero, string NIP, string PartnerShortName, string ReceipNumber, string RedemptionNumber, string ReferenceReceipNumber, string TypeTransaction, List<DetailsAcumulationRequest> details, bool status, string MessageError, decimal TotalPuntosPayback, decimal PuntosPaybackAcumulados, decimal PuntosPaybackRedimidos, bool SuccesRedemption, bool SuccesAcumulation);
        ResponseJgoView GetListJgoF(ChangePriceView price, string IdList, string ItemName, string Origen, string Cantidad, string ItemCode);
        ResponseJgoView GetBoxF(string JsonCombo);
        RequestRegisterPaybackSale CreateObjRegisterPaybackSaleF(string Idventa, string BranchShortName, DateTime EfectiveTime, decimal LegalAmountRLegalV, decimal LegalAmountTPV, decimal LoyaltyAmountRLoyalV, string monedero, string NIP, string PartnerShortName, string ReceipNumber, string RedemptionNumber, string ReferenceReceipNumber, string TypeTransaction, List<DetailsAcumulationRequest> details, bool status, string MessageError, decimal TotalPuntosPayback, decimal PuntosPaybackAcumulados, decimal PuntosPaybackRedimidos, string Idabono);
        List<ListPeriodo> GetPeriodsF(string idcliente);
        List<ListDia> GetDaysF(string JsonParams);
        List<ListFolio> GetFoliosF(string JsonParams);
        string ConfirmaEntrega(string Parametros);
        List<GarantiasIn> GetGarantias();

        List<GarantiasIn> GetGarantiasxAprobar();

        GarantiasIn ResponseAction(string JsonString);


    }

}
