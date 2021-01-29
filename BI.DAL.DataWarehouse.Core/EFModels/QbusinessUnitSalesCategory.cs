using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class QbusinessUnitSalesCategory
    {
        public int Id { get; set; }
        public int BusinessUnitId { get; set; }
        public string SalesCategory { get; set; }
        public string Directory { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
    }
}
