using System;

namespace Entities.Models.Payback
{
    public class ListRquestPurchase
    {
        public string partnerShortName { get; set; }//nombre del cliente
        public string branchShortName { get; set; }//sucursal
        public string ReceipNumber { get; set; }//numero de movimiento
        public string FolioAbono { get; set; }
        public DateTime Fecha { get; set; }
    }
}
