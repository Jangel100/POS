using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Reportes
{
    public class ReportsFactPagosView
    {
        public string FechaPago { get; set; }
        public string FechaFactura { get; set; }
        public string Tienda { get; set; }
        public string Venta { get; set; }
        public string FolioPago { get; set; }
        public string FormaDePago { get; set; }
        public string Vendedor { get; set; }
        public string Cliente { get; set; }
        public string EstatusPago { get; set; }
        public string TotalVenta { get; set; }
        public string Saldo { get; set; }
        public string MontoPagado { get; set; }
        public string EstatusFactura { get; set; }
    }
}