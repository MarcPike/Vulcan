using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VDistinctPurchaseCategoryCodes
    {
        public Guid? ViewId { get; set; }
        public string Coid { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
