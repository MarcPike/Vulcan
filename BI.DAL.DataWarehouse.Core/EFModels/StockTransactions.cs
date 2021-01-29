using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class StockTransactions
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int Version { get; set; }
        public DateTime Cdate { get; set; }
        public int CuserId { get; set; }
        public DateTime Mdate { get; set; }
        public int MuserId { get; set; }
        public string Status { get; set; }
        public int? BranchId { get; set; }
        public string Type { get; set; }
        public int? Number { get; set; }
        public string OtherReference { get; set; }
        public int? Session { get; set; }
        public string Description { get; set; }
        public DateTime? ProcessingDate { get; set; }
        public DateTime? DocumentDate { get; set; }
        public int? StockAdjustmentReasonId { get; set; }
        public int? BatchNumber { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
    }
}
