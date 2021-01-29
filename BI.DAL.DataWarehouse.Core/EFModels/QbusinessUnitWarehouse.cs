using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class QbusinessUnitWarehouse
    {
        public int Id { get; set; }
        public int BusinessUnitId { get; set; }
        public string Warehouse { get; set; }
        public string Directory { get; set; }
        public string IncludeForPoforecast { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
    }
}
