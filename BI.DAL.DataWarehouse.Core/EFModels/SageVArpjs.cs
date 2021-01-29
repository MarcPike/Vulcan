using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVArpjs
    {
        public string Coid { get; set; }
        public string Typebtch { get; set; }
        public decimal Postseqnce { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public decimal Dateposted { get; set; }
        public decimal Datebus { get; set; }
        public short Swprinted { get; set; }
        public short Swpostgl { get; set; }
        public decimal Datepostgl { get; set; }
        public short Swglconsl { get; set; }
        public string Pgmver { get; set; }
    }
}
