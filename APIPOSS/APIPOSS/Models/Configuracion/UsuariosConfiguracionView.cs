using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Configuracion
{
    public class UsuariosConfiguracionView
    {
        public int AdminUserID { get; set; }
        public string Usuario { get; set; }
        public string Rol { get; set; }
        public string Estatus { get; set; }
    }
}