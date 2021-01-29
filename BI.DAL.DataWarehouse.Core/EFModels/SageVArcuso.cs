using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVArcuso
    {
        public string Coid { get; set; }
        public string Idcust { get; set; }
        public string Optfield { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Value { get; set; }
        public short Type { get; set; }
        public short Length { get; set; }
        public short Decimals { get; set; }
        public short Allownull { get; set; }
        public short Validate { get; set; }
        public short Swset { get; set; }
    }
}
