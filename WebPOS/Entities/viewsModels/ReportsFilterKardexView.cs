using Entities.Models.Configuracion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class ReportsFilterKardexView
    {
        public IEnumerable<TiendaJsonView> lsTiendas { get; set; }
        public IEnumerable<CatalogoView> lsArticulo { get; set; }
        public IEnumerable<SAPWS> lsSAPWS { get; set; }
    }
}
