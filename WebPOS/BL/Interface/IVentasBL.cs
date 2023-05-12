using Entities;
using Entities.Models.Payback;
using Entities.Models.Ventas;
using Entities.viewsModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;

namespace BL.Interface
{
    public interface IVentasBL
    {
        HttpResponseMessage GetResponseAPI(string url);
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
        List<ListArticuloView> GetArticulos();
        List<ListModeloView> GetModelos(string Tipo);
        List<ListLineaView> GetListLinea(string code);
        List<ListMedidaView> GetMedida(string code);
        string GetExistencias(string Parametrs);
        int GetIva(string Idstore);
        List<ListListOfPriceView> GetPrice(string store, string SAPDB);
        double GetUnitPrice(string JsonUnitPrice);
        ChangePriceView GetChangePrice(double Iva, double Price, string descuento, int cantidad);
        DataTable GetQueryDBconn(string Query);
        DataTable GetQueryDBconnSap(string idstore, string Modelo);
        List<ListModeloView> GetModelsSelected(string JsonParams);
        ResponseJuego GetCombo(string parameter);
        ResponseJgoView GetListJgo(ChangePriceView price, string IdList, string ItemName, string Origen, string Cantidad, string ItemCode);
        ResponseJgoView GetBox(string JsonCombo);
        RequestRegisterPaybackSale CreateObjRegisterPaybackSale(string Idventa,string BranchShortName,DateTime EfectiveTime,decimal LegalAmountRLegalV,decimal LegalAmountTPV,decimal LoyaltyAmountRLoyalV,string monedero,string NIP,string PartnerShortName,string ReceipNumber,string RedemptionNumber,string ReferenceReceipNumber,string TypeTransaction, List<DetailsAcumulationRequest> details,bool status,string MessageError,decimal TotalPuntosPayback, decimal PuntosPaybackAcumulados,decimal PuntosPaybackRedimidos,bool SuccesRedemption, bool SuccesAcumulation);
        AddSale Getdatepedido(string jsondata);
        string UpdateFechaEn(string Parametros);
        List<PromocionDosPorUno> GetInfoDosPorUno();
    }
}
