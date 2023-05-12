using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.viewsModels
{
    public class UsersConfigView
    {
        public string Accion { get; set; }
        public int AdminUserID { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CorreoElectronico { get; set; }
        public string EmployeeNum { get; set; }
        public string NTUserAccount { get; set; }
        public string NTUserDomain { get; set; }
        public string Status { get; set; }
        public Estatus Estatus { get; set; }
        public string NoCelular { get; set; }
        public int AdminRoleID { get; set; }
        public List<RolesView> lsRoles { get; set; }
        public List<AdminStore> lsTiendas { get; set; }
    }
}
