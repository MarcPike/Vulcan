using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Coiddefaults
    {
        public string Coid { get; set; }
        public string Coiddescription { get; set; }
        public string DefaultCurrency { get; set; }
        public string DefaultWeightUom { get; set; }
    }
}
