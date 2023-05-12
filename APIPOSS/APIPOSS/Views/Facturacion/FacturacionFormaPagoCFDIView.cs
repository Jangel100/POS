using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Views.Facturacion
{
    public class FacturacionFormaPagoCFDIView
    {
        public string IdMetodoPago { get; set; }
        public IEnumerable<FormasDePagosView> lsFormaPagos { get; set; }
        public string FormaPago { get; set; }

    }
}