using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels
{
    public class ReportsFacturacionPayback
    {
        public int ID { get; set; }
        public string FechaPedido { get; set; }
        public string FechaFactura { get; set; }
        public string FechaCompletado { get; set; }
        public string Tienda { get; set; }
        public string Venta { get; set; }
        public string Vendedor { get; set; }
        public string Cliente { get; set; }
        public string EstatusVenta { get; set; }
        public string Facturado { get; set; }
        public string Total_Venta { get; set; }
        public string Monto_pagado { get; set; }
        public string Saldo { get; set; }
        public string Estatus_Factura { get; set; }
        public string NumeroRecibo { get; set; }
        public string TipoTransaccion { get; set; }
        public string PagadoPuntos { get; set; }
        public string PagadoPesos { get; set; }
        public string AbonadoPuntos { get; set; }
        public string AbonadoPesos { get; set; }
        public string AcumuladoPuntos { get; set; }
        public string AcumuladoPesos { get; set; }
        public string EstatusTransaccion { get; set; }
        public string ErrorMessage { get; set; }
        public string Descuento { get; set; }
        public string Codigo { get; set; }
        public string Utilizado { get; set; }
        public string Observaciones { get; set; }
        public string DescuentoModificado { get; set; }
        public string Articulo { get; set; }
        public string PrecioSMU { get; set; }
        public string PrecioFDO { get; set; }
        public string Margen { get; set; }
        public string Conciliado { get; set; }
        public string FechaPayback { get; set; }
        public string FechaConciliacion { get; set; }
        public string Socio { get; set; }
        public string NombreArchivo { get; set; }
        public string FechaTrans { get; set; }
    }
}
