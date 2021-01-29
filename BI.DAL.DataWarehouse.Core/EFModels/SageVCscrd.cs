using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVCscrd
    {
        public string Homecur { get; set; }
        public string Ratetype { get; set; }
        public string Sourcecur { get; set; }
        public decimal? Ratedate { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public decimal Rate { get; set; }
        public decimal Spread { get; set; }
        public short Datematch { get; set; }
        public short Rateoper { get; set; }
    }
}
