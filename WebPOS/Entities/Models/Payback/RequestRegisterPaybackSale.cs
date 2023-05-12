using System;
using System.Collections.Generic;

namespace Entities.Models.Payback
{
    public class RequestRegisterPaybackSale
    {
        public string Idventa { get; set; }
        public string PartnerShortName { get; set; }
        public string BranchShortName { get; set; }
        public DateTime EfectiveTime { get; set; }
        public string ReceipNumber { get; set; }
        public string RedemptionNumber { get; set; }
        public string ReferenceReceipNumber { get; set; }
        public decimal LegalAmountRLegalV { get; set; }
        public decimal LoyaltyAmountRLoyalV { get; set; }
        public decimal LegalAmountTPV { get; set; }
        public string monedero { get; set; }
        public string NIP { get; set; }
        public string TypeTransaction { get; set; }
        public List<ListDetails> ListDetails { get; set; }
        public bool Status { get; set; }
        public string MessageError { get; set; }
        public decimal TotalPuntosPayback { get; set; }
        public decimal PuntosPaybackAcumulados { get; set; }
        public decimal PuntosPaybackRedimidos { get; set; }
        public string Idabono { get; set; }
        public bool SuccesRedemption { get; set; }
        public bool SuccesAcumulation { get; set; }
    }
    public class ListDetails
    {
        public string ArticleEanCode { get; set; }
        public string PartnerProductGroupCode { get; set; }
        public string PartnerProductCategoryCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalTurnoverAmount { get; set; }
        public decimal TotalRewardableAmount { get; set; }
    }
}
