using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class ReportsClienteAvisaView
    {
        public int ID { get; set; }
        public string Fecha { get; set; }
        public string Tienda { get; set; }
        public string Venta { get; set; }
        public string Vendedor { get; set; }
        public string Cliente { get; set; }
        public string Estatus_Venta { get; set; }
        public string Codigo_Articulo { get; set; }
        public string Articulo { get; set; }
        public string Lista_de_Precios { get; set; }
        public int Cantidad { get; set; }
        public string Precio_Unitario { get; set; }
        public string Descuento { get; set; }
        public int IVA { get; set; }
        public string Total_Linea { get; set; }
        public string Total_Venta { get; set; }
        public string Monto_Pagado { get; set; }
        public string Saldo { get; set; }
    }
}
