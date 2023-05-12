using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Configuracion
{
    public class MenuInfoRol
    {
        public int MenuTabID { get; set; }
        public string MenuTabName { get; set; }
        public int MenuOptionID { get; set; }
        public string OptionName { get; set; }
        public int AdminRoleID { get; set; }
        public int Permiso { get; set; }        
    }
}