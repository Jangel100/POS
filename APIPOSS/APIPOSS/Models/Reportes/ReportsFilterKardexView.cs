using APIPOSS.Models.Configuracion;
using APIPOSS.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Reportes
{
    public class ReportsFilterKardexView
    {
        public IEnumerable<TiendaJsonView> lsTiendas { get; set; }
        public IEnumerable<CatalogoView> lsArticulo { get; set; }
        public IEnumerable<SAPWS> lsSAPWS { get; set; }
    }
}