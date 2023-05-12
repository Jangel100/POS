using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.AdminStore
{
    public class ListPrice
    {
        public string idBtn { get; set; }
        public int listnum { get; set; }
        public string listname { get; set; }
        public bool isAvailable { get; set; }
        public bool CheckedList { get; set; }
    }  

}