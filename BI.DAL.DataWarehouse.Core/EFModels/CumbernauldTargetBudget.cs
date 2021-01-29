using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class CumbernauldTargetBudget
    {
        public int Id { get; set; }
        public DateTime AccountingPeriod { get; set; }
        public string Location { get; set; }
        public decimal Budget { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
