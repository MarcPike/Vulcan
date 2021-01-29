using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVTxclass
    {
        public string Coid { get; set; }
        public string Authority { get; set; }
        public short Classtype { get; set; }
        public short Classaxis { get; set; }
        public short Class { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Desc { get; set; }
        public short Exempt { get; set; }
    }
}
