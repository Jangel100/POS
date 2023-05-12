using System.Collections.Generic;

namespace Entities.viewsModels
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
