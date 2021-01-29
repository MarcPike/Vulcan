using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VulcanCrmQuoteItemProductionCost
    {
        public int Id { get; set; }
        public int Qiid { get; set; }
        public int QuoteId { get; set; }
        public int QuoteItemId { get; set; }
        public string Coid { get; set; }
        public string LocationName { get; set; }
        public string ResourceType { get; set; }
        public string PerType { get; set; }
        public string TypeName { get; set; }
        public decimal InternalCost { get; set; }
        public decimal ProductionCost { get; set; }
        public decimal MinimumCost { get; set; }
        public decimal TotalPounds { get; set; }
        public decimal TotalInches { get; set; }
        public decimal TotalPieces { get; set; }
        public decimal TotalInternalCost { get; set; }
        public decimal TotalProductionCost { get; set; }
        public DateTime? ImportDateTimeUtc { get; set; }
        public string BaseCurrency { get; set; }
        public decimal BaseInternalCost { get; set; }
        public decimal BaseMinimumCost { get; set; }
        public decimal BaseProductionCost { get; set; }
        public decimal BaseTotalInternalCost { get; set; }
        public decimal BaseTotalProductionCost { get; set; }

        public virtual VulcanCrmQuoteItem Qi { get; set; }
    }
}
