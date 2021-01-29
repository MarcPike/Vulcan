using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVGljec
    {
        public string Coid { get; set; }
        public string Batchnbr { get; set; }
        public string Journalid { get; set; }
        public string Transnbr { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Jecomment { get; set; }
    }
}
