using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class ReportsKardexView
    {
        public string Id { get; set; }
        public string Tipo_Mov { get; set; }
        public string Fecha_Movimiento { get; set; }
        public string Tienda { get; set; }
        public string Desc_Articulo { get; set; }
        public string Cantidad { get; set; }
        public string Cantidad_Acumulada { get; set; }
        public string Referencia { get; set; }
    }
}
