namespace Entities.Models.Payback
{
    public class RequestWsRedemption
    {
        public string Alias { get; set; }
        public string Principal { get; set; }
        public string Credential { get; set; }
        public string secret { get; set; }
        public string PartnerShortName { get; set; }
        public decimal BranchShortName { get; set; }
        public decimal LegalAmountRLV { get; set; }
        public decimal LegalAmountTPV { get; set; }
    }
}
