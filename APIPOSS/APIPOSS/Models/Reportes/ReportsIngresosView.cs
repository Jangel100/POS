using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Reportes
{
    public class ReportsIngresosView
    {
        public int ID { get; set; }
        public string Fecha { get; set; }
        public string Tienda { get; set; }
        public string Venta { get; set; }
        public string Vendedor { get; set; }
        public string Cliente { get; set; }
        public string Monto { get; set; }
        public string Forma_de_Pago { get; set; }
        public string Recibo { get; set; }
    }
}