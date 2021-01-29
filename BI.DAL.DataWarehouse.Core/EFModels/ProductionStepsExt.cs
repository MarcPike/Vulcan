using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProductionStepsExt
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string BranchCode { get; set; }
        public int? Number { get; set; }
        public string ProductionStepReference { get; set; }
        public int? ProcessGroupId { get; set; }
        public int? SalesItemId { get; set; }
        public int? NextStepId { get; set; }
        public int? PreviousStepId { get; set; }
        public int? ReplacementStepId { get; set; }
        public DateTime? PreviousStepCompletionDate { get; set; }
        public DateTime? NextStepDueDate { get; set; }
        public decimal? SequenceNumber { get; set; }
        public bool? IsCurrentStep { get; set; }
        public bool? IsCurrentScheduledStep { get; set; }
        public int? ProductionStepDaysAtStatus { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
