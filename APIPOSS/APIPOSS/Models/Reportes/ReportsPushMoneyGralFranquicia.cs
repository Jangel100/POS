using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Reportes
{
    public class ReportsPushMoneyGralFranquicia
    {
        public string CardCode { get; set; }
        public string Franquicia { get; set; }
        public string Subtotal { get; set; }
        public string Iva { get; set; }
        public string TotalPush { get; set; }
    }
}