using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Configuracion
{
    public class UsersAdmin
    {
        public int AdminUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CorreoElectronico { get; set; }
        public string EmployeeNum { get; set; }
        public string NTUserAccount { get; set; }
        public string NTUserDomain { get; set; }
        public string Status { get; set; }
        public string NoCelular { get; set; }
        public int AdminRoleID { get; set; }
        public string Franquicia { get; set; }
        public List<AdminStore> lsTiendas { get; set; }
    }
}