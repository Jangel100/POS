using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class ReportsVentasxArticulo
    {
        public List<TiendaJsonView> lsTienda { get; set; }
        public List<CatalogoView> lsModelo { get; set; }
        public List<CatalogoView> lsLinea { get; set; }
        public List<CatalogoView> lsMedida { get; set; }
        public List<CatalogoView> lsArticulo { get; set; }
    }
}
