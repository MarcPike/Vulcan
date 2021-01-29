using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VSuppliers
    {
        public string ViewId { get; set; }
        public string Coid { get; set; }
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string TypeCode { get; set; }
        public string TypeDescription { get; set; }
    }
}
