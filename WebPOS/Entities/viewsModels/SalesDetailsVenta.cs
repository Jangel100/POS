using System;

namespace Entities.viewsModels
{
    public class SalesDetailsVenta
    {
        public int IdVenta { get; set; }
        public int IdArticulo { get; set; }
        public string Modelo { get; set; }
        public string Juego { get; set; }
        public Decimal Cantidad { get; set; }
        public string Tienda { get; set; }
        public string Bodega { get; set; }
        public string Lista { get; set; }
        public Decimal Unitario { get; set; }
        public Decimal Descuento { get; set; }
        public Decimal Subtotal { get; set; }
        public Decimal IVA { get; set; }
        public Decimal Total { get; set; }
        public string Itemcode { get; set; }
        public string Opcion { get; set; }
    }
}
