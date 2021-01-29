using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVGlsrce
    {
        public string Coid { get; set; }
        public string Srceledger { get; set; }
        public string Srcetype { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Srcedesc { get; set; }
    }
}
