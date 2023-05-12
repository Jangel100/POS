using Entities.viewsModels;
using System.Collections.Generic;
using System.Net.Http;
using Entities.Models.Payback;
using System;

namespace BL.Interface
{
    public interface IAbonosBL
    {
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
        List<ListPeriodo> GetPeriods(string idcliente);
        List<ListDia> GetDays(string JsonParams);
        List<ListFolio> GetFolios(string JsonParams);
        RequestRegisterPaybackSale CreateObjRegisterPaybackSale(string Idventa, string BranchShortName, DateTime EfectiveTime, decimal LegalAmountRLegalV, decimal LegalAmountTPV, decimal LoyaltyAmountRLoyalV, string monedero, string NIP, string PartnerShortName, string ReceipNumber, string RedemptionNumber, string ReferenceReceipNumber, string TypeTransaction, List<DetailsAcumulationRequest> details, bool status, string MessageError, decimal TotalPuntosPayback, decimal PuntosPaybackAcumulados, decimal PuntosPaybackRedimidos, string Idabono);
    }
}
