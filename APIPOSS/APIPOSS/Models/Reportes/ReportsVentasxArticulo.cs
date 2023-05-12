using APIPOSS.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Reportes
{
    public class ReportsVentasxArticulo
    {
        public List<TiendaJsonView> lsTienda { get; set; }
        public List<CatalogoView> lsModelo { get; set; }
        public List<CatalogoView> lsLinea{ get; set; }
        public List<CatalogoView> lsMedida { get; set; }
        public List<CatalogoView> lsArticulo { get; set; }
    }
}