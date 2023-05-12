using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Ventas
{
    public class DetailsSales
    {
        public int ID { get; set; }
        public int IDLinea { get; set; }
        public int IDArticulo { get; set; }
        public string Juego { get; set; }
        public string Lista { get; set; }
        public string PrecioUnitario { get; set; }
        public string IVA { get; set; }
        public string Observaciones { get; set; }
        public string Descuento { get; set; }
        public string TotalLinea { get; set; }
        public string Cantidad { get; set; }
        public string StatusLinea { get; set; }
        public bool Importado { get; set; }
        public int IDStore { get; set; }
    }
}