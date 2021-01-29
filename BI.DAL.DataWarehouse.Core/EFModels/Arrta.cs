using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Arrta
    {
        public string Coid { get; set; }
        public string Codeterm { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Textdesc { get; set; }
        public short Activesw { get; set; }
        public decimal Inacdate { get; set; }
        public decimal Lastmntn { get; set; }
        public short Multipaym { get; set; }
        public short Vatcodem { get; set; }
        public short Disctype { get; set; }
        public decimal Discpct { get; set; }
        public decimal Discnbr { get; set; }
        public decimal Discday { get; set; }
        public decimal Ddaystrt1 { get; set; }
        public decimal Ddaystrt2 { get; set; }
        public decimal Ddaystrt3 { get; set; }
        public decimal Ddaystrt4 { get; set; }
        public decimal Ddayend1 { get; set; }
        public decimal Ddayend2 { get; set; }
        public decimal Ddayend3 { get; set; }
        public decimal Ddayend4 { get; set; }
        public decimal Dmnthadd1 { get; set; }
        public decimal Dmnthadd2 { get; set; }
        public decimal Dmnthadd3 { get; set; }
        public decimal Dmnthadd4 { get; set; }
        public decimal Ddayuse1 { get; set; }
        public decimal Ddayuse2 { get; set; }
        public decimal Ddayuse3 { get; set; }
        public decimal Ddayuse4 { get; set; }
        public short Duetype { get; set; }
        public decimal Cntdueday { get; set; }
        public decimal Duenbrdays { get; set; }
        public decimal Dudayst1 { get; set; }
        public decimal Dudayst2 { get; set; }
        public decimal Dudayst3 { get; set; }
        public decimal Dudayst4 { get; set; }
        public decimal Dudayend1 { get; set; }
        public decimal Dudayend2 { get; set; }
        public decimal Dudayend3 { get; set; }
        public decimal Dudayend4 { get; set; }
        public decimal Dumnthad1 { get; set; }
        public decimal Dumnthad2 { get; set; }
        public decimal Dumnthad3 { get; set; }
        public decimal Dumnthad4 { get; set; }
        public decimal Dudayuse1 { get; set; }
        public decimal Dudayuse2 { get; set; }
        public decimal Dudayuse3 { get; set; }
        public decimal Dudayuse4 { get; set; }
        public short Dteduesync { get; set; }
        public short Dtedscsync { get; set; }
        public decimal Cntentered { get; set; }
        public decimal Pctduetot { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
