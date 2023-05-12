using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Reportes
{
    public class ReportsVentaxArticuloView
	{
		public int ID { get; set; }
		public string FechaFactura { get; set; }
		public string Tienda { get; set; }
		public string Venta { get; set; }
		public string Vendedor { get; set; }
		public string Cliente { get; set; }
		public string Estatus_Venta { get; set; }
		public string Codigo_Articulo { get; set; }
		public string Articulo { get; set; }
		public string Lista_de_Precio { get; set; }
		public int Cantidad { get; set; }
		public string Precio_unitario { get; set; }
		public string Descuento { get; set; }
		public string IVA { get; set; }
		public string Total_Linea { get; set; }
		public string Total_Venta { get; set; }
		public string Monto_pagado { get; set; }
		public string Saldo { get; set; }
		public string Push_Money { get; set; }
	}
}