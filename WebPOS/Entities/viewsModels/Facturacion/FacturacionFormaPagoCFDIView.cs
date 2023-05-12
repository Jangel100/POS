using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels.Facturacion
{
    public class FacturacionFormaPagoCFDIView
    {
        public string IdMetodoPago { get; set; }
        public IEnumerable<FormasDePagosView> lsFormaPagos { get; set; }
        public string FormaPago { get; set; }
    }
}
