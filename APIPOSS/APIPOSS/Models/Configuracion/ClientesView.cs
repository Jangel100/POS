using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Configuracion
{
    public class ClientesView
    {
        public Clientes Clientes { get; set; }
        public Clientes ClientesFacturacion { get; set; }
        public List<Entidades> Estados { get; set; }
    }
}