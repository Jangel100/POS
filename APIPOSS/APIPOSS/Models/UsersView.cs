using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models
{
    public class UsersView
    {
        public string AdminUserID { get; set; }
        public string UserName { get; set; }
        public string NTUserAccount { get; set; }
        public string TypeRole { get; set; }
        public string Franquicia { get; set; }
        public string WHSID { get; set; }
        public string IDSTORE { get; set; }
        public string STORENAME { get; set; }
        public string CorreoUsuario { get; set; }
        public string WebTokenJWT { get; set; }
    }
}