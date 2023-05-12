namespace Entities.Models.Abonos
{
    public class AddPay
    {
        public string Id { get; set; }
        public string IDVenta { get; set; }
        public string Fecha { get; set; }
        public string Vendedor { get; set; }
        public string Pagado { get; set; }
        public string PorPagar { get; set; }
        public string Total { get; set; }
        public string FormaDePago { get; set; }
        public string TipoTarjeta { get; set; }
        public string TerminacionTarjeta { get; set; }
        public string Monto { get; set; }
        public string Pedido { get; set; }
        public bool botton { get; set; }
        public string Cliente { get; set; }
        public string Prefijo { get; set; }
        public string Idstore { get; set; }
        public string Afiliacion { get; set; }
        public string WhsID { get; set; }
        public bool payresponse { get; set; }
        public string Monedero { get; set; }
        public string SecretPassword { get; set; }
        public string MessageErrorPBKAcumulation { get; set; }
        public string FormaPago33 { get; set; }
        public string MetodoPago33 { get; set; }
        public string TipoComp33 { get; set; }
        public string UsoCFDI33 { get; set; }
        public string TipoRel33 { get; set; }
        public string CFDI_Rel33 { get; set; }
        public string CorreoCliente { get; set; }
        public string CorreoUsuario { get; set; }
        public int? Parcialidad { get; set; }
        public string FechaPago { get; set; }

        public int Importado { get; set; }
        public string comentariosNoTimbrar { get; set; }


    }
}
