using Entities.Models.Ventas;
using System.Collections.Generic;

namespace Entities.viewsModels
{
    public class SalesSearchVenta
    {
        public List<SalesDetailsVenta> lsDetailsVenta { get; set; }
        public List<VentasPago> lsDetailsAbonosVenta { get; set; }
    }
}
