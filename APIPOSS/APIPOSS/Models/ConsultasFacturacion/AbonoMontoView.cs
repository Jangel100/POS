using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.ConsultasFacturacion
{
    public class AbonoMontoView
    {
        public int IdAbono { get; set; }
        public int IdVenta { get; set; }
        public Decimal MontoTotal { get; set; }
    }
}