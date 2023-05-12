using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Configuracion
{
    public class RolesView
    {
        public int AdminRoleID { get; set; }
        public string RoleName { get; set; }
        public string Status { get; set; }
        public bool CanBeDeleted { get; set; }
        public string Btn_Front { get; set; }
    }
}