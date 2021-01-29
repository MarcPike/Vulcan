using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Uomconversion
    {
        public string FromUom { get; set; }
        public string ToUom { get; set; }
        public decimal Factor { get; set; }
    }
}
