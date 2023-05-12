using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Reportes
{
    public class ReportsTransferenciasView
    {
        public int IDEnvio { get; set; }
        public string ArticuloSBO { get; set; }
        public string Descripcion { get; set; }
        public string Usuario { get; set; }
        public string FechaEnvio { get; set; }
        public int Cantidad { get; set; }
        public string TiendaOrigen { get; set; }
        public string TiendaDestino { get; set; }
        public int FechaDif { get; set; }
        public string Status { get; set; }
        public string Comentarios { get; set; }
    }
}