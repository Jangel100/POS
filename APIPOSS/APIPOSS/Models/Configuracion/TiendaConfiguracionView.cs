using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Configuracion
{
    public class TiendaConfiguracionView
    {
        public int AdminStoreID { get; set; }
        public string StoreName { get; set; }
        public string Status { get; set; }
        public bool CanBeDeleted { get; set; }
        public string StoreType { get; set; }
    }
}