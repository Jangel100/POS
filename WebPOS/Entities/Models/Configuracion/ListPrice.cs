using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models.Configuracion
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
