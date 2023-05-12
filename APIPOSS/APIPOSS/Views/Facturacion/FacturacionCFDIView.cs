using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Views.Facturacion
{
    public class FacturacionCFDIView
    {
        public IEnumerable<UsoCFDIView> lsUsoCFDI { get; set; }
        public IEnumerable<MetodoPagosView> lsMetodoPagos { get; set; }
        public string CorreoCliente { get; set; }
        public string CorreoUsuario { get; set; }
    }
}