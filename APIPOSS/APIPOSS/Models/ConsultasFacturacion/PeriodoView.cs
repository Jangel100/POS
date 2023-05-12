using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.ConsultasFacturacion
{
    public class PeriodoView
    {
        public int idStore { get; set; }
        public int idCliente { get; set; }
        public string Periodo { get; set; }
        public string Dia { get; set; }
    }
}