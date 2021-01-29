using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class DropPolicy
    {
        public int Id { get; set; }
        public string Coid { get; set; }
        public int ProductCategoryId { get; set; }
        public decimal Discardlength { get; set; }
        public decimal MaximumOd { get; set; }
        public decimal MinimumOd { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
    }
}
