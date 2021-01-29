using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ScmManualForecast
    {
        public int Id { get; set; }
        public string Coid { get; set; }
        public int ProductId { get; set; }
        public int BusinessUnitId { get; set; }
        public string Uom { get; set; }
        public int WeightUsagePerPeriod { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
