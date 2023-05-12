using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class EntregaPedidoView
    {
        public int Identrega { get; set; }
        public int IdVenta { get; set; }
        public string RutaQR { get; set; }
        public string Statusen { get; set; }
        public byte[] imagen { get; set; }
        public string Numerochecks { get; set; }
        public string Numdiasintentos { get; set; }
        public string IdStore { get; set; }
        public string QRENTRE { get; set; }
    }
}
