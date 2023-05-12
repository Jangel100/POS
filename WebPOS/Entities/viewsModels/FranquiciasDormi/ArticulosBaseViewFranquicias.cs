using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.viewsModels.FranquiciasDormi
{
    public class ArticulosBaseViewFranquicias
    {
        public ListRadioButton ListRadioButton { get; set; }
        public List<ListArticuloView> ListArticulos { get; set; }
        public List<ListLineaView> ListLinea { get; set; }
        public List<ListMedidaView> ListMedida { get; set; }
        public List<ListModeloView> ListModelo { get; set; }
        public List<ListListOfPriceView> ListOfPrice { get; set; }
        public List<ModelosLista> ListModels { get; set; }
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
        //Franquicias de dormimundo
        public List<ListArticuloCVEView> ListArticulosCve { get; set; }
        public List<ListArticuloCodeView> ListArticulosCode { get; set; }
        public string CantidadAlmacen { get; set; }
        public int CantidadAlmacenDisp { get; set; }
    }
    public class ModelosLista
    {
        public string code { get; set; }
        public string name { get; set; }
    }
}