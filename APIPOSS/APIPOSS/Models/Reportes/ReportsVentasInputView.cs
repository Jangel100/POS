using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Reportes
{
    public class ReportsVentasInputView
	{
		public string AdminUserID { get; set; }
		public string FRCARDCODE { get; set; }
		public int Usuarios { get; set; }
		public int Tiendas { get; set; }
		public string Articulo { get; set; }
		public string Linea { get; set; }
		public string Medida { get; set; }
		public string Modelo { get; set; }
		public int Estatus { get; set; }
		public string Fecha1 { get; set; }
		public string Fecha2 { get; set; }
		public string Franquicia { get; set; }
		public string TipoPago { get; set; }
		public string Whs { get; set; }
		public string Vendedor { get; set; }
		public string TipoReporte { get; set; }
		public string Store { get; set; }
	}
}