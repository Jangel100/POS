namespace Entities.viewsModels
{
    public class UserEncripty
    {
        public string UserName { get; set; }
        public string PaswoordToCheck { get; set; }
        public string Paswoord { get; set; }
        public string PaswoordHassh { get; set; }
        public byte[] Salt { get; set; }
    }
}
