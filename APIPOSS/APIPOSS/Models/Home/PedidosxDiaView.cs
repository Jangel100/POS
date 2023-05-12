using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Home
{
    public class PedidosxDiaView
    {
        public int IdVenta { get; set; }
        public string Pedido { get; set; }
        public string Vendedor { get; set; }
        public string Fecha_Creacion { get; set; }
        public string tienda { get; set; }
        public string Estatus { get; set; }
        public string Fecha_entrega { get; set; }
        public string RowNumber { get; set; }
        public string Totalcount { get; set; }
    }
}