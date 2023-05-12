using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class ReportsFacturacionView
    {
        public int ID { get; set; }
        public string FechaPedido { get; set; }
        public string FechaFactura { get; set; }
        public string FechaCompletado { get; set; }
        public string Tienda { get; set; }
        public string Venta { get; set; }
        public string Vendedor { get; set; }
        public string Cliente { get; set; }
        public string EstatusVenta { get; set; }
        public string Facturado { get; set; }
        public string Total_Venta { get; set; }
        public string Monto_pagado { get; set; }
        public string Saldo { get; set; }
        public string Comentarios { get; set; }
        public string Estatus_Factura { get; set; }
    
    }
}
