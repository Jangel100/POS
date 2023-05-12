using System;

namespace Entities.viewsModels
{
    public class TiendaConfiguracionView
    {
        public int AdminStoreID { get; set; }
        public string StoreName { get; set; }
        public string Status { get; set; }
        public Boolean CanBeDeleted { get; set; }
        public string StoreType { get; set; }
    }
}
