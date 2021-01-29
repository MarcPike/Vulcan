using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVGlasv
    {
        public string Coid { get; set; }
        public string Idseg { get; set; }
        public string Segval { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Segvaldesc { get; set; }
        public string Acctretern { get; set; }
    }
}
