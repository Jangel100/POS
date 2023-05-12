using Entities.Models.Payback;
using Entities.Models.Ventas;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BL.Interface
{
    public interface IPaybackBL
    {
        HttpResponseMessage GetResponseAPI(string url);
        HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
        AdminPayback GetInfoAdminPayback(string ws, string type);
        string GetMessageView(string code);
        decimal ConvertPointsToAmount(decimal TotalPuntos, decimal pagado, decimal porcentaje);
        ListRquestPurchase QueryDatesRquestPurchase(string idstore, string idAbono, DateTime fecha, string TypeWs);
        decimal AmountToPoints(string monto, decimal valuePoints);
        string GetFolioSale(string idStore);
        bool RegisterPaybackSale(string json);
        string GetDateSale(string idVenta);
        DetailsAcumulationRequest GetDetails(List<ArrayArticulos> ArrayArticulos);
        string GetFolioSaleId(string idVenta);
        string GetFolioAbono(string idStore);
        ListRquestPurchase QueryDatesRquestPurchaseAbono(string idstore, string idAbono, DateTime fecha, string TypeWs);
    }
}
