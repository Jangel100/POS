using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models.Configuracion
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
