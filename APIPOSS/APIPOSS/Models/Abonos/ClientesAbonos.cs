using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Abonos
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
}