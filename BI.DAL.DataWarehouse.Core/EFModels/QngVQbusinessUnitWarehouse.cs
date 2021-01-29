using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class QngVQbusinessUnitWarehouse
    {
        public int Id { get; set; }
        public int BusinessUnitId { get; set; }
        public string Warehouse { get; set; }
        public string Directory { get; set; }
        public string IncludeForPoforecast { get; set; }
    }
}
