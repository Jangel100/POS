using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Garantias
{
    public class GarantiasIn
    {
        public string id_garantia { get; set; }
        public DateTime Fecha { get; set; }
        public string Franquicia { get; set; }
        public string Tienda { get; set; }
        public DateTime FechaVenta { get; set; }
        public DateTime FechaGaranti { get; set; }
        public string Cliente { get; set; }
        public string Direccion { get; set; }
        public string QuienSol { get; set; }
        public string ItemName { get; set; }
        public string Auditor { get; set; }
        public string e_mail { get; set; }
        public string Estatus { get; set; }
        public string Tipo { get; set; }
        public string Comentarios { get; set; }
        public Boolean ResponseAPI { get; set; }
        public string MessageAPI { get; set; }
    }
}