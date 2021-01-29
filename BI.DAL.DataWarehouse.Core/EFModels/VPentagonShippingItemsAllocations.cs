using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VPentagonShippingItemsAllocations
    {
        public string Coid { get; set; }
        public int? DespatchNumber { get; set; }
        public int? DespatchItemNumber { get; set; }
        public long? StockItemSeqNumber { get; set; }
        public string TagNumber { get; set; }
        public string WarehouseCode { get; set; }
        public string MillCode { get; set; }
        public string CastNumber { get; set; }
        public int? PhysicalPieces { get; set; }
        public decimal? PhysicalQuantity { get; set; }
        public decimal? PhysicalWeight { get; set; }
        public int? AllocatedPieces { get; set; }
        public decimal? AllocatedQuantity { get; set; }
        public decimal? AllocatedWeight { get; set; }
        public decimal? PackingWeight { get; set; }
        public decimal? InvoiceWeight { get; set; }
    }
}
