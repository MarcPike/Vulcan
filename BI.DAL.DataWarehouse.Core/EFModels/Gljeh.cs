using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Gljeh
    {
        public string Coid { get; set; }
        public string Batchid { get; set; }
        public string Btchentry { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Srceledger { get; set; }
        public string Srcetype { get; set; }
        public string Fscsyr { get; set; }
        public string Fscsperd { get; set; }
        public short Swedit { get; set; }
        public short Swreverse { get; set; }
        public string Jrnldesc { get; set; }
        public decimal Jrnldr { get; set; }
        public decimal Jrnlcr { get; set; }
        public decimal Jrnlqty { get; set; }
        public decimal Dateentry { get; set; }
        public short Drilsrcty { get; set; }
        public decimal Drilldwnlk { get; set; }
        public string Drilapp { get; set; }
        public string Revyr { get; set; }
        public string Revperd { get; set; }
        public int Errbatch { get; set; }
        public int Errentry { get; set; }
        public string Origcomp { get; set; }
        public decimal Detailcnt { get; set; }
        public string Enteredby { get; set; }
        public decimal? Docdate { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
