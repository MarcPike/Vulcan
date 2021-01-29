using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProductionStepProductSpecifications
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? ProductId { get; set; }
        public int? DimensionsId { get; set; }
        public string SpecificationValue1 { get; set; }
        public string SpecificationValue2 { get; set; }
        public string SpecificationValue3 { get; set; }
        public string SpecificationValue4 { get; set; }
        public string SpecificationValue5 { get; set; }
        public string SpecificationValue6 { get; set; }
        public string SpecificationValue7 { get; set; }
        public string SpecificationValue8 { get; set; }
        public string SpecificationValue9 { get; set; }
        public string SpecificationValue10 { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
