using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Ventas
{
    public class SaleSearchView
    {
        public int ID { get; set; }
        public int IdStore { get; set; }
        public string Folio { get; set; }
        public string Fecha { get; set; }
    }
}