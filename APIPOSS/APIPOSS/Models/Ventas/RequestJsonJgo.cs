using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Ventas
{
    public class RequestJsonJgo
    {
        public string modelo { get; set; }
        public string idStore { get; set; }
        public int Iva { get; set; }
        public string cantidad { get; set; }
        public string Origen { get; set; }
        public string Lista { get; set; }
        public double Price { get; set; }
        public string Descuento { get; set; }
        public string PriceUnit { get; set; }
        public string Total { get; set; }        
    }
}