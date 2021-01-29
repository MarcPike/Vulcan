using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class BakerImportedForecast
    {
        public int Id { get; set; }
        public string Coid { get; set; }
        public int? ProductId { get; set; }
        public int BusinessUnitId { get; set; }
        public string ProductCode { get; set; }
        public string BusinessUnit { get; set; }
        public string DefaultUom { get; set; }
        public decimal ForecastBase { get; set; }
        public decimal? ForecastLbs { get; set; }
        public decimal? ForecastKgs { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
