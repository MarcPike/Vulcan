using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class UnitOfMeasure
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public int? TypeId { get; set; }
        public decimal? Scale { get; set; }
        public decimal? Modifier { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public int? ChargeQuantityDecimalPlaces { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int DescriptionDecimalPlaces { get; set; }
        public int ImperialAndMetricFormatType { get; set; }
    }
}
