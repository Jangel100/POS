using Entities.Models.Configuracion;
using System.Collections.Generic;

namespace Entities.viewsModels
{
    public class TiendaConfigView
    {
        public int AdminStoreID { get; set; }
        public string StoreName { get; set; }
        public string Status { get; set; }
        public int StoreTypeID { get; set; }
        public string WHSID { get; set; }
        public string Telefono { get; set; }
        public string CalleNumero { get; set; }
        public string CodigoPostal { get; set; }
        public string Delegacion { get; set; }
        public int EstadoId { get; set; }
        public string Colonia { get; set; }
        public int Socios { get; set; }
        public int Clientes { get; set; }
        public int whsConsigID { get; set; }
        public string actIDPV { get; set; }
        public int actCodeZonaImpo { get; set; }
        public int actIVA { get; set; }
        public int actAdendaLiverpool { get; set; }
        public string FolderPDF { get; set; }
        public string emailTienda { get; set; }
        public string NumExt { get; set; }
        public string NumInt { get; set; }
        public string BodegaPropia { get; set; }
        public string MontoReabasto { get; set; }
        public string Accion { get; set; }
        public string DefaultList { get; set; }
        public string IDDefaultList { get; set; }
        public string DefaultCustomer { get; set; }
        public bool checkGlobal { get; set; }
        public string AlmacenSapPropio { get; set; }
        public List<TypeStore> lsTypeStore { get; set; }
        public List<SAPWS> lsSAPWS { get; set; }
        public List<Store> lsStoreBodegaPropia { get; set; }
        public List<Store> lsStoreClasificaciones { get; set; }
        public List<StoreIVA> lsIVA { get; set; }
        public List<Entidades> lsEntidades { get; set; }
        public List<SelectedFolios> lsFolios { get; set; }
        public List<ListPrice> lsListaPrecios { get; set; }
        public List<Franquicias> lsFranquicias { get; set; }
    }
}
