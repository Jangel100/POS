using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Reportes
{
    public class ReportsPushMoneyVendedorView
    {
        public string Tienda { get; set; }
        public string Franquicia { get; set; }
        public string Fecha { get; set; }
        public string Factura { get; set; }
        public string Producto { get; set; }
        public string PushMoney { get; set; }
        public string Cantidad { get; set; }
        public string Total { get; set; }
    }
}