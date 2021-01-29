using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProductLevelAllocation
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string AllocationType { get; set; }
        public int? SalesItemId { get; set; }
        public int? ProcessRequestId { get; set; }
        public int? ProcessGroupId { get; set; }
        public int? CacheOfProductIdForSearching { get; set; }
        public decimal? CacheOfDim1ForSearching { get; set; }
        public decimal? CacheOfDim2ForSearching { get; set; }
        public decimal? CacheOfDim3ForSearching { get; set; }
        public decimal? CacheOfDim4ForSearching { get; set; }
        public decimal? CacheOfDim5ForSearching { get; set; }
        public int? CacheOfBranchIdForSearching { get; set; }
        public DateTime? PrintedDate { get; set; }
        public bool? StockAllocated { get; set; }
        public bool? ReadyStockAllocated { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
