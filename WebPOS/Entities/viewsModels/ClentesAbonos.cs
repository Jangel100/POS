using System.Collections.Generic;

namespace Entities.viewsModels
{
    public class ClentesAbonos
    {
        public List<ListClientes> ListClientes { get; set; }
        public List<ListPeriodo> ListPeriodos { get; set; }
        public List<ListDia> ListDia { get; set; }
        public List<ListFolio> ListFolio { get; set; }
    }
    public class ListClientes
    {
        public string Nombre { get; set; }
        public string ID { get; set; }
    }
    public class ListPeriodo
    {
        public string Periodo { get; set; }

    }
    public class ListDia
    {
        public string Dia { get; set; }
    }
    public class ListFolio
    {
        public string Folio { get; set; }
        public string FolioPref { get; set; }
    }
    //Franquicias Dormimundo

    public class ClentesAbonosFranquicias
    {
        public List<ListClientes> ListClientes { get; set; }
        public List<ListPeriodo> ListPeriodos { get; set; }
        public List<ListDia> ListDia { get; set; }
        public List<ListFolio> ListFolio { get; set; }
    }
}
