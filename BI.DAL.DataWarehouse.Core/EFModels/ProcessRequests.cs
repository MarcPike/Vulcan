using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProcessRequests
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? ProcessAllocationId { get; set; }
        public int? ProductLevelAllocationId { get; set; }
        public int? ConsumedProductId { get; set; }
        public decimal? ConsumedDim1 { get; set; }
        public decimal? ConsumedDim2 { get; set; }
        public decimal? ConsumedDim3 { get; set; }
        public decimal? ConsumedDim4 { get; set; }
        public decimal? ConsumedDim5 { get; set; }
        public int? ConsumedPieces { get; set; }
        public decimal? ConsumedQuantity { get; set; }
        public decimal? ConsumedWeight { get; set; }
        public string ProcessNotes { get; set; }
        public int? BranchId { get; set; }
        public int? Number { get; set; }
        public DateTime? DueDate { get; set; }
        public int? ProcessPlanId { get; set; }
        public string SpecificationValue1 { get; set; }
        public string SpecificationValue2 { get; set; }
        public string SpecificationValue3 { get; set; }
        public string SpecificationValue4 { get; set; }
        public string SpecificationValue5 { get; set; }
        public string SpecificationValue6 { get; set; }
        public string SpecificationValue7 { get; set; }
        public string SpecificationValue8 { get; set; }
        public string SpecificationValue9 { get; set; }
        public string SpecificationValue10 { get; set; }
        public int? AllocatedCoilPieces { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
