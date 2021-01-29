using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VLookupCustomers
    {
        public string ViewId { get; set; }
        public string Coid { get; set; }
        public string BranchCode { get; set; }
        public int CustomerId { get; set; }
        public int CompanyId { get; set; }
        public string CustomerNumber { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string GroupCode { get; set; }
        public string GroupDescription { get; set; }
        public string SegmentCode { get; set; }
        public string SegmentDescription { get; set; }
        public string Currency { get; set; }
    }
}
