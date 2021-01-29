using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class NdtutcertMaintenanceDetails
    {
        public int Id { get; set; }
        public string Directory { get; set; }
        public string Couplant { get; set; }
        public string Result { get; set; }
        public string SurfaceCondition { get; set; }
        public string TestRange { get; set; }
        public string TestGain { get; set; }
        public string TestBlocks { get; set; }
        public string ScanPlan { get; set; }
        public string Probe { get; set; }
        public string Frequency { get; set; }
        public string TransferLoss { get; set; }
        public string ExaminationProcedure { get; set; }
        public string AcceptanceProcedure { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
    }
}
