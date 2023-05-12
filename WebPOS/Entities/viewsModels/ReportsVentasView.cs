using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.viewsModels
{
    public class ReportsVentasView
    {
        public int ID { get; set; }
        public string Fecha { get; set; }
        public string FechaCFDI { get; set; }
        public string Tienda { get; set; }
        public string Venta { get; set; }
        public string Tipo_Documento { get; set; }
        public string Documento { get; set; }
        public string Facturado { get; set; }
        public string Vendedor { get; set; }
        public string Cliente { get; set; }
        public string Estatus_Venta { get; set; }
        public string Codigo_Articulo { get; set; }
        public string Linea { get; set; }
        public string Articulo { get; set; }
        public string Lista_de_Precios { get; set; }
        public Decimal Cantidad { get; set; }
        public string Origen { get; set; }
        public string Precio_unitario { get; set; }
        public string Descuento { get; set; }
        public string SubTotal { get; set; }
        public string IVA { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,0.00}", ApplyFormatInEditMode = true)]
        public string Total_Linea { get; set; }
        public string Total_Venta { get; set; }
        public string Monto_pagado { get; set; }
        public string Saldo { get; set; }
        public string Push_Money { get; set; }
        public string Codigo_Cliente_SAP { get; set; }
        public string Cliente_SAP { get; set; }
        public string Fecha_Completado { get; set; }
    }
}
