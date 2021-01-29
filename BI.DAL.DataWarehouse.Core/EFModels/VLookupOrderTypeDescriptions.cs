using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VLookupOrderTypeDescriptions
    {
        public int ViewId { get; set; }
        public string OrderTypeCode { get; set; }
        public string OrderTypeDescription { get; set; }
    }
}
