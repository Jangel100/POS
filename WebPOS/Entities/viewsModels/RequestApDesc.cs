using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
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
