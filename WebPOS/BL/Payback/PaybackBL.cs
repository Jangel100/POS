using BL.Interface;
using DAL.Configuracion;
using Entities.Models.Payback;
using Entities.Models.Ventas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace BL.Configuracion
{
    public class PaybackBL : IPaybackBL
    {
        readonly DAL.Utilities.IResponseAPI _Payback;
        public PaybackBL(PaybackDAL PaybackDAL)
        {
            _Payback = PaybackDAL;
        }

        public PaybackBL()
        {
            _Payback = new PaybackDAL();
        }
        public AdminPayback GetInfoAdminPayback(string ws, string type)
        {
            var obj = new
            {
                ws = ws,
                TypeWs = type
            };
            string json_data = JsonConvert.SerializeObject(obj);
            var ResponseAdminInfoPayback = GetResponseAPI($"api/GetAdminPayback?JsonWs=" + json_data + "");
            ResponseAdminInfoPayback.EnsureSuccessStatusCode();
            var AdminPaybackView = ResponseAdminInfoPayback.Content.ReadAsAsync<AdminPayback>().Result;
            return AdminPaybackView;
        }

        public HttpResponseMessage GetResponseAPI(string url)
        {
            return _Payback.GetResponseAPI(url);
        }

        public HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj)
        {
            return _Payback.ReadAsStringAsyncAPI(url, obj);
        }

        public string GetMessageView(string code)
        {
            var ResponseAdminInfoPayback = GetResponseAPI($"api/GetMessageErrorPayback?code=" + code + " ");
            ResponseAdminInfoPayback.EnsureSuccessStatusCode();
            var result = ResponseAdminInfoPayback.Content.ReadAsAsync<string>().Result;
            return result;
        }

        public decimal ConvertPointsToAmount(decimal TotalPuntos, decimal totalventa, decimal porcentaje)
        {
            var amountpesos = TotalPuntos * porcentaje;
            if (amountpesos > totalventa)
            {
                amountpesos = totalventa;
            }
            else
            {
                return amountpesos;
            }
            return amountpesos;
        }
        public ListRquestPurchase QueryDatesRquestPurchase(string idstore, string idAbono, DateTime fecha, string TypeWs)
        {
            var Values = new ListRquestPurchase();
            Values.branchShortName = GetPrefijoSale(idstore);
            if (string.IsNullOrEmpty(idAbono))
            {
                Values.FolioAbono = GetFolioSale(idstore);
            }
            else { Values.FolioAbono = GetFolioSaleId(idAbono); }
            Values.Fecha = DateTime.Now;
            Values.ReceipNumber = (Regex.IsMatch("3|4", TypeWs) ? "RSIES" : "SIES") + "-" + Values.branchShortName + "-" + Values.FolioAbono + "-" + Values.Fecha.ToString("dd/MM/yy") + "-" + TypeWs;
            return Values;
        }
        public ListRquestPurchase QueryDatesRquestPurchaseAbono(string idstore, string idAbono, DateTime fecha, string TypeWs)
        {
            var Values = new ListRquestPurchase();
            Values.branchShortName = GetPrefijoSale(idstore);
            Values.FolioAbono = GetFolioAbono(idstore);
            Values.Fecha = DateTime.Now;
            Values.ReceipNumber = "SIES" + "-" + Values.branchShortName + "-" + Values.FolioAbono + "-" + Values.Fecha.ToString("dd/MM/yy") + "-" + TypeWs;
            return Values;
        }
        public decimal AmountToPoints(string monto, decimal valuePoints)
        {
            decimal TotalPoints = 0;
            try
            {
                var amount = decimal.Parse(ConvertTo2D(monto));
                TotalPoints = amount / valuePoints;
                if ((TotalPoints.ToString().Contains(".")))
                {
                    var pointsArray = Convert.ToString(TotalPoints).Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    TotalPoints = Convert.ToInt32(pointsArray[0]) + 1;
                }
            }
            catch (Exception ex)
            {
                return TotalPoints;
            }
            return TotalPoints;
        }
        public static string ConvertTo2D(string a, bool requerido = true, bool zeros = true, bool comas = false, bool pesosSign = false)
        {
            var result = a;
            var format = zeros ? (!comas ? "{0:0.00}" : "{0:#,##0.00}") : (!comas ? "{0:0.##}" : "{0:#,##.##}");
            var cifra = (!string.IsNullOrEmpty(result) ? result : "").Replace(",", "").Trim();

            if (!string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(cifra))
            {
                decimal b = 0;

                if (decimal.TryParse(cifra, out b))
                    result = string.Format(format, b);
            }
            else
                result = requerido ? "0.00" : a;

            if (!string.IsNullOrEmpty(result) && !result.Equals("N/A") && pesosSign)
                result = "$ " + result;
            return result;
        }
        public string GetPrefijoSale(string idStore)
        {
            var ResponseAdminInfoPayback = GetResponseAPI($"api/GetPrefijoVenta?idStore=" + idStore + " ");
            ResponseAdminInfoPayback.EnsureSuccessStatusCode();
            var result = ResponseAdminInfoPayback.Content.ReadAsAsync<string>().Result;
            return result;
        }
        public string GetDateSale(string idVenta)
        {
            var ResponseAdminInfoPayback = GetResponseAPI($"api/GetDateSale?idVenta=" + idVenta + " ");
            ResponseAdminInfoPayback.EnsureSuccessStatusCode();
            var result = ResponseAdminInfoPayback.Content.ReadAsAsync<string>().Result;
            return result;
        }

        public bool RegisterPaybackSale(string TPBK)
        {
            try
            {
                var ResponseAdminInfoPayback = GetResponseAPI($"api/GetRegisterTransactionPBK?TPBK=" + TPBK + "");
                //ResponseAdminInfoPayback.EnsureSuccessStatusCode();
                //var result = Convert.ToBoolean(ResponseAdminInfoPayback.Content.ReadAsStringAsync().Result);
                //return result;
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }

        public string GetFolioSale(string idStore)
        {
            var ResponseAdminInfoPayback = GetResponseAPI($"api/GetFolioSale?idStore=" + idStore + " ");
            ResponseAdminInfoPayback.EnsureSuccessStatusCode();
            var result = ResponseAdminInfoPayback.Content.ReadAsAsync<string>().Result;
            return result;
        }
        public string GetFolioAbono(string idStore)
        {
            var ResponseAdminInfoPayback = GetResponseAPI($"api/GetFolioAbono?idStore=" + idStore + " ");
            ResponseAdminInfoPayback.EnsureSuccessStatusCode();
            var result = ResponseAdminInfoPayback.Content.ReadAsAsync<string>().Result;
            return result;
        }
        public DetailsAcumulationRequest GetDetails(List<ArrayArticulos> ArrayArticulos)
        {
            var lista = JsonConvert.SerializeObject(ArrayArticulos);
            var ResponseAdminInfoPayback = GetResponseAPI($"api/GetDetailsPayback?idVenta=" + lista + " ");
            ResponseAdminInfoPayback.EnsureSuccessStatusCode();
            var result = ResponseAdminInfoPayback.Content.ReadAsAsync<DetailsAcumulationRequest>().Result;
            return result;
        }

        public string GetFolioSaleId(string idVenta)
        {
            var ResponseAdminInfoPayback = GetResponseAPI($"api/GetFolioSaleId?idVenta=" + idVenta + " ");
            ResponseAdminInfoPayback.EnsureSuccessStatusCode();
            var result = ResponseAdminInfoPayback.Content.ReadAsAsync<string>().Result;
            return result;
        }
    }
}
