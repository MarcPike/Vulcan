using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class CustomerPartLengthLookup
    {
        public int Id { get; set; }
        public string Coid { get; set; }
        public string Customer { get; set; }
        public string CustomerPartNumber { get; set; }
        public decimal LengthPerPieceInches { get; set; }
        public string Plant { get; set; }
        public string IsConsignment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOnUtc { get; set; }
    }
}
