using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Ventas
{
    public class SalesSearchVenta
    {
        public List<SalesDetailsVenta> lsDetailsVenta { get; set; }
        public List<VentasPago> lsDetailsAbonosVenta { get; set; }
    }
}