using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.ConsultasFacturacion
{
    public class PedidoMontoView
    {
        public int IdVenta { get; set; }
        public Decimal MONTOABONOS { get; set; }
        public Decimal MONTOVENTA { get; set; }
    }
}