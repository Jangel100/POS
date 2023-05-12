using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Configuracion
{
    public class MenuOptionInfoRol
    {
        public string Action { get; set; }
        public string TextBtn { get; set; }
        public int AdminRoleID { get; set; }
        public string FRCARDCODE { get; set; }
        public List<MenuInfoRol> MenuInfoRol { get; set; }
        public List<IGrouping<string,MenuInfoRol>> MenuInfoRolE { get; set; }
        public List<ListFranquiciasUser> ListFranquiciasUser { get; set; }
        public AdminRoleClas AdminRoleClas { get; set; }
        public StatusText StatusText { get; set; }
        public string RoleName { get; set; }
    }
}