using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Configuracion
{
    public class RequestModMenOp
    {
        public string AdminRoleID { get; set; }
        public string NombreRol { get; set; }
        public string FranquiciasUser { get; set; }
        public string StatusActive { get; set; }
        public string TypeRole { get; set; }
        public List<ArrayIdMenuOp> ArrayIdMenuOp { get; set; }
        public bool ModifiedName { get; set; }
    }
    public class ArrayIdMenuOp
    {
        public string Id { get; set; }
    }
}