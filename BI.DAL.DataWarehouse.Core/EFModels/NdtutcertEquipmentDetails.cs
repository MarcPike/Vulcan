using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class NdtutcertEquipmentDetails
    {
        public int Id { get; set; }
        public string Directory { get; set; }
        public string Equipment { get; set; }
        public DateTime? CalibrationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
    }
}
