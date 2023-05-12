using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Payback
{
    public class AdminPaybackView
    {
        public string AdminUser { get; set; }
        public string Password { get; set; }
        public string PartnerShortName { get; set; }
        public decimal ValueInPoints { get; set; }
        public string TypeWs { get; set; }
    }
}