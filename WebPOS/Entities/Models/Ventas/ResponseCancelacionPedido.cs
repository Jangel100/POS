using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models.Ventas
{
    public class ResponseCancelacionPedido
    {
        public string idDevolucion { get; set; }
        public bool statusCancelacion { get; set; }
        public string idVenta { get; set; }
        public List<InfoPaypackCancel> InfoPaypackCancel { get; set; }
    }
    public class InfoPaypackCancel
    {
        public int idTransaction { get; set; }
        public string PartnerShortName { get; set; }
        public string BranchShortName { get; set; }
        public string ReceiptNumber { get; set; }
        public string RedemptionNumber { get; set; }
        public string Monedero { get; set; }
        public bool StatusTrans { get; set; }
        public string TypeTransaction { get; set; }
    }
}
