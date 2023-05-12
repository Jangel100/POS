using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class ListaApDesc
    {
        public List<AprobacionDescuentosView> ListaDescuento { get; set; }
    }
   
    public class AprobacionDescuentosView
    {
        public string ID { get; set; }
        public string Vendedor { get; set; }
        public string Articulo { get; set; }
        public string itemName { get; set; }
        public string Descuento { get; set; }
        public string Tienda { get; set; }
        public string Fecha { get; set; }
        public string Status { get; set; }
        public string Observaciones { get; set; }
        public double PrecioSMU { get; set; }
        public double PrecioFDO { get; set; }
        public double Margen { get; set; }
    }
}
