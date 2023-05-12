using System;

namespace Entities.Models.Ventas
{
    public class VentasPago
    {
        public int ID { get; set; }
        public int IDVenta { get; set; }
        public int AdminStoreID { get; set; }
        public char TipoVenta { get; set; }
        public string FormaPago { get; set; }
        public Decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public Boolean Importado { get; set; }
        public DateTime Fecha_Importado { get; set; }
        public Boolean Error { get; set; }
        public string Detalle_Error { get; set; }
        public string StatusPago { get; set; }
        public string NoCuenta { get; set; }
        public string Prefijo { get; set; }
        public string Folio { get; set; }
        public string FacturaSinDesc { get; set; }
        public string Afiliacion { get; set; }
        public string Ticket { get; set; }
        public string Contrato { get; set; }
        public string TipoTarjeta { get; set; }
        public int MSI { get; set; }
        public DateTime FechaCFDI { get; set; }
        public Boolean Facturado { get; set; }
        public string Archivo_FE { get; set; }
        public string ClaveRastreo { get; set; }
    }
}
