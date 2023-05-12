using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Ventas
{
    public class RequestCancelacion
    {
        public int IdStore { get; set; }
        public string AdminUserID { get; set; }
        public string idVenta { get; set; }
    }
}