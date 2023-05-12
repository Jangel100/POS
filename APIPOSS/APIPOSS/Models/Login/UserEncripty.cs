using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Login
{
    public class UserEncripty
    {
        public string UserName { get; set; }
        public string Paswoord { get; set; }
        public string PaswoordHassh { get; set; }
        public byte[] Salt { get; set; }
        public string SaltCadena { get; set; }
    }
}