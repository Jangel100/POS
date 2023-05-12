using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.ConsultasFacturacion
{
    public class PedidosView
    {
        public int id { get; set; }
        public string venta { get; set; }
        public string abono { get; set; }
        public string fechareciboOutput { get; set; }
        public string fechaventaOutput { get; set; }
        public Decimal motopagado { get; set; }
        public string Reimprimir { get; set; }
        public string btnDownloadFactura { get; set; }
        public string btnDownloadComplemento { get; set; }
        //public Decimal Restante { get; set; }
        public Boolean Confirmacion { get; set; }
        //public string Facturado { get; set; }
        //public Decimal Restante { get; set; }
        //public int Devolucion { get; set; }
        //public string NCE { get; set; }
    }
}