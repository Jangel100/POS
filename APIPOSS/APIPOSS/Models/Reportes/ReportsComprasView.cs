using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Reportes
{
    public class ReportsComprasView
    {
        public int IDEntrada { get; set; }
        public string DocumentoSAP { get; set; }
        public string Artículo { get; set; }
        public decimal Cantidad { get; set; }
        public int LineaSBO { get; set; }
        public string FechaSBO { get; set; }
        public string FechaEntWeb { get; set; }
        public string SNSBO { get; set; }
        public string Usuario { get; set; }
        public string Tienda { get; set; }
        public string FolioPOS { get; set; }
    }
}