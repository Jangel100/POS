namespace Entities.Models.Payback
{
    public class DetailsAcumulationRequest
    {
        public string ArticleEanCode { get; set; }
        public string PartnerProductGroupCode { get; set; }
        public string PartnerProductCategoryCode { get; set; }
        public string QuantityUnitCode { get; set; }
        public decimal SingleTurnoverAmount { get; set; }
        public decimal TotalRewardableAmount { get; set; }
    }
}
