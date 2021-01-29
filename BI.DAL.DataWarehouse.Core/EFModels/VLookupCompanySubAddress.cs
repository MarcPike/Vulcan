using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VLookupCompanySubAddress
    {
        public string ViewId { get; set; }
        public string Coid { get; set; }
        public int CompanyId { get; set; }
        public int? CompanySubAddressId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
