using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.ConsultasFacturacion
{
    public class ConsultaFacturacionView
    {
        public List<string> Periodo { get; set; }
        public List<string> Dia { get; set; }
        public List<string> Folio { get; set; }
    }
}