using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class InfoArchivoBuzon
    {
        public string Cupon { get; set; }
        public string Socio { get; set; }
        public string NumIntPayback { get; set; }
        public string TipoDoc { get; set; }
        public string Recibo { get; set; }
        public string NumeroPayback { get; set; }
        public string NombreArchivo { get; set; }
        public string Fechadetransaccion { get; set; }
        public DateTime FechaConciliacion { get; set; }
        public bool Existe { get; set; }
        public string NumeroTarjeta { get; set; }
        public string Moneda { get; set; }
        public string Sucursal { get; set; }
        public Double PuntosOtorgados { get; set; }
        public Double Total { get; set; }
        public Double TotalCompra { get; set; }
    }
}
