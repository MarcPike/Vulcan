using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProcessGroupProcessingTimes
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? ProcessGroupId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Dim1 { get; set; }
        public decimal? Dim1NegativeTolerance { get; set; }
        public decimal? Dim1PositiveTolerance { get; set; }
        public decimal? Dim2 { get; set; }
        public decimal? Dim2NegativeTolerance { get; set; }
        public decimal? Dim2PositiveTolerance { get; set; }
        public decimal? Dim3 { get; set; }
        public decimal? Dim3NegativeTolerance { get; set; }
        public decimal? Dim3PositiveTolerance { get; set; }
        public decimal? Dim4 { get; set; }
        public decimal? Dim4NegativeTolerance { get; set; }
        public decimal? Dim4PositiveTolerance { get; set; }
        public decimal? Dim5 { get; set; }
        public decimal? Dim5NegativeTolerance { get; set; }
        public decimal? Dim5PositiveTolerance { get; set; }
        public string ComponentReference { get; set; }
        public decimal? SetupTime { get; set; }
        public decimal? RunTime { get; set; }
        public decimal? WeightThroughput { get; set; }
        public int? Passes { get; set; }
        public int? SetupTimeUnitId { get; set; }
        public int? RunTimeUnitId { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
