using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIPOSS.Models.Payback
{
    public class DetailsAcumulationRA
    {
        public string ArticleEanCode { get; set; }
        public string PartnerProductGroupCode { get; set; }
        public string PartnerProductCategoryCode { get; set; }
        public string QuantityUnitCode { get; set; }
        public decimal SingleTurnoverAmount { get; set; }
        public decimal TotalRewardableAmount { get; set; }
    }
}