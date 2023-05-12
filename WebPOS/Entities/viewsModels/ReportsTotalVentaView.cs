using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class ReportsTotalVentaView
    {
        public int ID { get; set; }
        public string Fecha { get; set; }
        public string FechaCFDI { get; set; }
        public string Tienda { get; set; }
        public string Venta { get; set; }
        public string Vendedor { get; set; }
        public string Cliente { get; set; }
        public string Estatus_Venta { get; set; }
        public string Total_Venta { get; set; }
        public string Monto_pagado { get; set; }
        public string Saldo { get; set; }
        public string Estatus_Factura { get; set; }
    }
}
