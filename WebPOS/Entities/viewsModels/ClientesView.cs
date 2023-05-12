using Entities.Models.Configuracion;
using System.Collections.Generic;

namespace Entities.viewsModels
{
    public class ClientesView
    {
        public Clientes Clientes { get; set; }
        public ClientesFacturacion ClientesFacturacion { get; set; }
        public List<Entidades> Estados { get; set; }
    }
}
