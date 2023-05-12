using System.Collections.Generic;
namespace Entities.viewsModels
{
    public class ArticulosBaseView
    {
        public ListRadioButton ListRadioButton { get; set; }
        public List<ListArticuloView> ListArticulos { get; set; }
        public List<ListLineaView> ListLinea { get; set; }
        public List<ListMedidaView> ListMedida { get; set; }
        public List<ListModeloView> ListModelo { get; set; }
        public List<ListListOfPriceView> ListOfPrice { get; set; }
        public List<ModelosLista> ListModels { get; set; }
        public List<PromocionDosPorUno> DosPorUno { get; set; }
        public List<TipoPromocionView> TipoPromocion { get; set; }
        public string CantidadTienda { get; set; }
        public int CantidadTiendaDisp { get; set; }
        public string CantidadBodega { get; set; }
        public int CantidadBodegaDisp { get; set; }
        public string DescriptionJuego { get; set; }
        public string PrecioUnitario { get; set; }
        public string IVA { get; set; }
        public string Descuento { get; set; }
        public string DescuentoPorc { get; set; }
        public string Subtotal { get; set; }
        public string Total { get; set; }
        public string Monto { get; set; }
    }
    public class ModelosLista
    {
        public string code { get; set; }
        public string name { get; set; }
    }
}
