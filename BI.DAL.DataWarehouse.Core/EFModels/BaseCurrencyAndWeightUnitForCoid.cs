using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class BaseCurrencyAndWeightUnitForCoid
    {
        public string BaseCoid { get; set; }
        public string CurrencyType { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal ConversionFactor { get; set; }
        public string Coiddescription { get; set; }
    }
}
