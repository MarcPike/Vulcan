using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVGlacgrp
    {
        public string Coid { get; set; }
        public string Acctgrpcod { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Acctgrpdes { get; set; }
        public string Sortcode { get; set; }
        public short Grpcod { get; set; }
    }
}
