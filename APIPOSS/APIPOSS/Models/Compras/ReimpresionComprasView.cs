using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Compras
{
    public class ReimpresionComprasView
    {
        public int IdInterno { get; set; }
        public string DocumentoSAP { get; set; }
        public string ReferenciaSAP { get; set; }
        public string Articulo { get; set; }
        public decimal Cantidad { get; set; }
        public int LineaSAP { get; set; }
        public string FechaSAP { get; set; }
        public string FechaWEB { get; set; }
        public string Socio { get; set; }
        public string Usuario { get; set; }
        public string Tienda { get; set; }
        public string U_FOLPOS { get; set; }
        public string btnDownloadcompra { get; set; }
    }
}