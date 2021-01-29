using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VulcanCrmQuoteItemTestPieces
    {
        public int Id { get; set; }
        public int Qiid { get; set; }
        public int QuoteId { get; set; }
        public int QuoteItemId { get; set; }
        public string Coid { get; set; }
        public string TestName { get; set; }
        public string StartingProductCode { get; set; }
        public int Pieces { get; set; }
        public decimal Quantity { get; set; }
        public string QuantityType { get; set; }
        public DateTime? ImportDateTimeUtc { get; set; }

        public virtual VulcanCrmQuoteItem Qi { get; set; }
    }
}
