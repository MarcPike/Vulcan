using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVBkjenth
    {
        public string Coid { get; set; }
        public decimal Pstseq { get; set; }
        public int Sequenceno { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Entrynbr { get; set; }
        public string Bank { get; set; }
        public decimal Transdate { get; set; }
        public short Transtype { get; set; }
        public string Reference { get; set; }
        public string Comment { get; set; }
        public decimal Totsrceamt { get; set; }
        public decimal Totfuncamt { get; set; }
        public decimal Totsrcegro { get; set; }
        public decimal Totfuncgro { get; set; }
        public string Ratetype { get; set; }
        public string Srcecurn { get; set; }
        public decimal Ratedate { get; set; }
        public decimal Rate { get; set; }
        public decimal Ratespread { get; set; }
        public short Rateop { get; set; }
        public decimal Postdate { get; set; }
        public string Postyear { get; set; }
        public short Postperiod { get; set; }
        public short Completed { get; set; }
        public string Bigcomment { get; set; }
        public short Status { get; set; }
        public decimal Recdate { get; set; }
        public string Recyear { get; set; }
        public short Recperiod { get; set; }
        public int Lines { get; set; }
        public int Serial { get; set; }
        public int Runid { get; set; }
        public short Type { get; set; }
        public string Ofxtid { get; set; }
        public short Entrytype { get; set; }
        public string Dsetcode { get; set; }
        public string Bankd { get; set; }
        public string Bkacct { get; set; }
        public string Bkstmtcur { get; set; }
        public string Dsetcoded { get; set; }
        public decimal Totstmtamt { get; set; }
        public string Transcur { get; set; }
        public decimal Recpent { get; set; }
        public decimal Recpentrec { get; set; }
        public string Defentnum { get; set; }
        public short Processcmd { get; set; }
        public short Agerecld { get; set; }
        public short Retentno { get; set; }
    }
}
