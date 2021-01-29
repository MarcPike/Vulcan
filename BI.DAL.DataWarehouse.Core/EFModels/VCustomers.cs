using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VCustomers
    {
        public string ViewId { get; set; }
        public string Directory { get; set; }
        public string BranchCode { get; set; }
        public string CustomerNumber { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string GroupCode { get; set; }
        public string Group { get; set; }
        public string SegmentCode { get; set; }
        public string Segment { get; set; }
        public string Currency { get; set; }
    }
}
