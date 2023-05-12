using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Abonos
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


        public AddPay(string pId
                       , string pIDVenta
                       , string pFecha
                        , string pVendedor
                        , string pPagado
                        , string pPorPagar
                        , string pTotal
                        , string pFormaDePago
                        , string pTipoTarjeta
                        , string pTerminacionTarjeta
                        , string pMonto
                        , string pMetodoPago33
                        , int pParcialidad
                        , string pMonedero
                        , string pSecretPassword
                        , string pCorreoCliente
                        , string pCorreoUsuario
                        )
        {
            Id = pId;
            IDVenta = pIDVenta;
            Fecha = pFecha;
            Vendedor = pVendedor;
            Pagado = pPagado;
            PorPagar = pPorPagar;
            Total = pTotal;
            FormaDePago = pFormaDePago;
            TipoTarjeta = pTipoTarjeta;
            TerminacionTarjeta = pTerminacionTarjeta;
            Monto = pMonto; // 
            MetodoPago33 = pMetodoPago33;
            Parcialidad = pParcialidad;
            Monedero = pMonedero;
            SecretPassword = pSecretPassword;
            CorreoCliente = pCorreoCliente;
            CorreoUsuario = pCorreoUsuario;
        }
        public AddPay(string pId
               , string pIDVenta
               , string pFecha
                , string pVendedor
                , string pPagado
                , string pPorPagar
                , string pTotal
                , string pFormaDePago
                , string pTipoTarjeta
                , string pTerminacionTarjeta
                , string pMonto
                , string pMetodoPago33
                , int pParcialidad
                , string pCorreoCliente
                , string pCorreoUsuario
                )
        {
            Id = pId;
            IDVenta = pIDVenta;
            Fecha = pFecha;
            Vendedor = pVendedor;
            Pagado = pPagado;
            PorPagar = pPorPagar;
            Total = pTotal;
            FormaDePago = pFormaDePago;
            TipoTarjeta = pTipoTarjeta;
            TerminacionTarjeta = pTerminacionTarjeta;
            Monto = pMonto; // 
            MetodoPago33 = pMetodoPago33;
            Parcialidad = pParcialidad;
            CorreoCliente = pCorreoCliente;
            CorreoUsuario = pCorreoUsuario;
        }
        public AddPay()
        {
        }
        public AddPay(string pId)//cuando esta cancelado o devuelto
        {
            Id = pId;
        }
    }


}