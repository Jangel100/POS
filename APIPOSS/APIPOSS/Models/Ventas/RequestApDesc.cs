using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Ventas
{
    public class RequestApDesc
    {
        public string IdCliente { get; set; }
        public string Iduser { get; set; }
        public string IdArticulo { get; set; }
        public int Statushide { get; set; }
        public int Statusdesc { get; set; }
        public string Idstored { get; set; }
        public string Cantdescuento { get; set; }
        public string Observaciones { get; set; }
        public int idList { get; set; }
        public double PriceFDO { get; set; }
    }
}