using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class CustomerPartPlants
    {
        public int Id { get; set; }
        public string Coid { get; set; }
        public int CustomerIdiMetalInternal { get; set; }
        public string CustomerNumber { get; set; }
        public string PlantCode { get; set; }
        public string Plant { get; set; }
        public string IsConsignment { get; set; }
        public string Warehouse { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
    }
}
