using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.AdminStore
{
    public class SelectedFolios
    {
        public string TipoFolio { get; set; }
        public string Tienda_Name { get; set; }
        public int PrimerFolio { get; set; }
        public int UltimoFolio { get; set; }
        public int UltimoFolioAsignado { get; set; }
        public string AdminFolioType { get; set; }
        public int TiendaId { get; set; }
        public string Prefijo { get; set; }
        public string NoAprobacion { get; set; }
        public string AñoAprobacion { get; set; }
    }
}