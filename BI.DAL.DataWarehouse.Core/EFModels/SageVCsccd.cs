using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVCsccd
    {
        public string Curid { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Curname { get; set; }
        public string Symbol { get; set; }
        public short Decimals { get; set; }
        public short Symbolpos { get; set; }
        public string Thoussep { get; set; }
        public string Decsep { get; set; }
        public short Negdisp { get; set; }
    }
}
