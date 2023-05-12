using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels.Facturacion
{
    public class FacturacionCFDIView
    {
        public IEnumerable<UsoCFDIView> lsUsoCFDI { get; set; }
        public IEnumerable<MetodoPagosView> lsMetodoPagos { get; set; }
        public string CorreoCliente { get; set; }
        public string CorreoUsuario { get; set; }
    }
}
