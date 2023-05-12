using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class ReportsView
    {
       public string TypeReports { get; set;}  
       public IEnumerable<TiendaJsonView> TiendaJson { get; set; }
       public ReportsVentasxArticulo reportsVentasxArticulo { get; set; }
       public ReportsFilterKardexView  reportsFilterKardex { get; set; }
    }
}
