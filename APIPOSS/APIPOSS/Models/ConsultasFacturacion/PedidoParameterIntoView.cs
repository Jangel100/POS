using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.ConsultasFacturacion
{
    public class PedidoParameterIntoView
    {
        public string TipoFactura { get; set; }
        public int IdStore { get; set; }
        public int Boton { get; set; }
        public int IdCliente { get; set; }
        public string Fecha { get; set; }
        public string Prefijo { get; set; }
        public string Folio { get; set; }
        public string Pedido { get; set; }
    }
}