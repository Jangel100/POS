using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Configuracion
{
    public class AdminStore
    {
        public bool Asignadas { get; set; }
        public bool Traspaso { get; set; }
        public int AdminStoreID { get; set; }
        public string StoreName { get; set; }
        public int AdminUserID { get; set; }
        public int AdminStoreToSendID { get; set; }
    }
}