using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Home
{
    public class PedidoDiaView
    {
        public string Pedido { get; set; }
        public string Vendedor { get; set; }
        public string Fecha_Creacion { get; set; }
        public string Tienda { get; set; }
        public string Estatus { get; set; }
        public string Fecha_Entrega { get; set; }
        public int IdVenta { get; set; }
        public int IdStore { get; set; }

    }
}