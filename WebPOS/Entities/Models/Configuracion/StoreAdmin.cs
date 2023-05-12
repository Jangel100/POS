using System.Collections.Generic;
namespace Entities.Models.Configuracion
{
    public class StoreAdmin
    {
        public int AdminStoreID { get; set; }
        public string StoreName { get; set; }
        public string Status { get; set; }
        public string StoreTypeID { get; set; }
        public string WHSID { get; set; }
        public string Telefono { get; set; }
        public string CalleNumero { get; set; }
        public string CodigoPostal { get; set; }
        public string Delegacion { get; set; }
        public string EstadoId { get; set; }
        public string Colonia { get; set; }
        public int? Socios { get; set; }
        public int? Clientes { get; set; }
        public string whsConsigID { get; set; }
        public string actIDPV { get; set; }
        public int? actCodeZonaImpo { get; set; }
        public int actIVA { get; set; }
        public int? actAdendaLiverpool { get; set; }
        public string FolderPDF { get; set; }
        public string emailTienda { get; set; }
        public string NumExt { get; set; }
        public string NumInt { get; set; }
        public string BodegaPropia { get; set; }
        public string MontoReabasto { get; set; }
        public bool checkfranquicia { get; set; }
        public bool checkGlobal { get; set; }       
        public string hdnDefault { get; set; }
        public string SelectedFranquicia { get; set; }
        public string AlmacenSapPropio { get; set; }
        public List<StoreAdminFolios> StoreAdminFolios { get; set; }
        public List<StoreAdminListas> StoreAdminListas { get; set; }
    }
}
