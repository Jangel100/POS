namespace Entities.viewsModels
{
    public class RequestChangePrice
    {
        public string ItemCode { get; set; }
        public string IdList { get; set; }
        public string descuento { get; set; }
        public string cantidad { get; set; }
        public string Origen { get; set; }
        public bool Juego { get; set; }
    }
}
