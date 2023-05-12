namespace Entities.Models.Payback
{
    public class AdminPayback
    {
        public string AdminUser { get; set; }
        public string Password { get; set; }
        public string PartnerShortName { get; set; }
        public decimal ValueInPoints { get; set; }
        public string TypeWs { get; set; }
    }
}