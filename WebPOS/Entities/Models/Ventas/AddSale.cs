using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Ventas
{
	public class AddSale
	{
		public string Idventa { get; set; }
		public string idstore { get; set; }
		public string Tipodeventa { get; set; }
		public string Comentarios { get; set; }
		public string Fechaentrega { get; set; }
		public string Medios { get; set; }
		public string Idcliente { get; set; }
		public string Idusuario { get; set; }
		public string WhsID { get; set; }
		public bool saleresponse { get; set; }
		public string TipoOperacion { get; set; }
		public string MessageErrorPayback { get; set; }
		public string MessageErrorPBKAcumulation { get; set; }
		public string Monedero { get; set; }
		public string Nip { get; set; }
		public List<ArrayArticulos> ArrayArticulos { get; set; }
		public List<ArrayPagos> ArrayPagos { get; set; }
		public string CorreoCliente { get; set; }
		public string CorreoUsuario { get; set; }
		public string MetodoPago33 { get; set; }
		public string FormaPago33 { get; set; }
		public string TipoComp33 { get; set; }
		public string UsoCFDI33 { get; set; }
		public string TipoRel33 { get; set; }
		public string CFDI_Rel33 { get; set; }
		public string webToken { get; set; }
		public string StatusVenta { get; set; }
		public string FolioPOS { get; set; }
		public string fechaventa { get; set; }
		public string IsRequiredFactura { get; set; }
		//Franquicias dormimundo
		public int Entregain { get; set; }

		public string uidRelacion { get; set; }
		public string tipoRelacion { get; set; }
		public string tipoDocRelacionado { get; set; }
	}
	public class ArrayPagos
	{
		public string Id { get; set; }
		public string Formapago { get; set; }
		public string Monto { get; set; }
		public string Cuenta { get; set; }
		public string Tipotarjeta { get; set; }
		public string MSISub { get; set; }
		public string Afiliacion { get; set; }
		public string FormaPago33 { get; set; }
		public string MetodoPago33 { get; set; }
		public string TipoComp33 { get; set; }
		public string UsoCFDI33 { get; set; }
		public string TipoRel33 { get; set; }
		public string CFDI_Rel33 { get; set; }
		public string CorreoCliente { get; set; }
		public string CorreoUsuario { get; set; }
		public int Parcialidad { get; set; }
	}
	public class ArrayArticulos
	{
		public string Id { get; set; }
		public string Articulo { get; set; }
		public string Linea { get; set; }
		public string Medida { get; set; }
		public string Modelo { get; set; }
		public string Cantidad { get; set; }
		public string CantidadTienda { get; set; }
		public string CantidadBodega { get; set; }
		public string Lista { get; set; }
		public string PrecioUnitario { get; set; }
		public string IVA { get; set; }
		public string Monto { get; set; }
		public string Descuento { get; set; }
		public string Total { get; set; }
		public string Juego { get; set; }
		public string AlmacenBox { get; set; }
		public string CantidadBox { get; set; }
		public string subTotal { get; set; }
		public string comentarios { get; set; }
		public string DescuentoJgo { get; set; }
		//Franquicias dormimundo
		public string CantidadAlmacen { get; set; }


	}
}
