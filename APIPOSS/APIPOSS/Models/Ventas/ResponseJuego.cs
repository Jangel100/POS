using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Ventas
{
    public class ResponseJuego
    {
        public string itemName { get; set; }
        public string itemCode { get; set; }
        public List<ListaPrecio> ListPrice { get; set; }
        public string MessageError { get; set; }
    }
    public class ListaPrecio
    {
        public string ListID { get; set; }
        public string ListName { get; set; }
    }
}