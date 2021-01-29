using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Aprta
    {
        public string Coid { get; set; }
        public string Termscode { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Codedesc { get; set; }
        public short Swactv { get; set; }
        public decimal Dateinactv { get; set; }
        public decimal Datelastmn { get; set; }
        public short Swmultpaym { get; set; }
        public short Codevat { get; set; }
        public short Codedistyp { get; set; }
        public decimal Disdaystr1 { get; set; }
        public decimal Disdaystr2 { get; set; }
        public decimal Disdaystr3 { get; set; }
        public decimal Disdaystr4 { get; set; }
        public decimal Disdayend1 { get; set; }
        public decimal Disdayend2 { get; set; }
        public decimal Disdayend3 { get; set; }
        public decimal Disdayend4 { get; set; }
        public decimal Dismthadd1 { get; set; }
        public decimal Dismthadd2 { get; set; }
        public decimal Dismthadd3 { get; set; }
        public decimal Dismthadd4 { get; set; }
        public decimal Disdayuse1 { get; set; }
        public decimal Disdayuse2 { get; set; }
        public decimal Disdayuse3 { get; set; }
        public decimal Disdayuse4 { get; set; }
        public short Codeduetyp { get; set; }
        public decimal Duedaystr1 { get; set; }
        public decimal Duedaystr2 { get; set; }
        public decimal Duedaystr3 { get; set; }
        public decimal Duedaystr4 { get; set; }
        public decimal Duedayend1 { get; set; }
        public decimal Duedayend2 { get; set; }
        public decimal Duedayend3 { get; set; }
        public decimal Duedayend4 { get; set; }
        public decimal Duemthadd1 { get; set; }
        public decimal Duemthadd2 { get; set; }
        public decimal Duemthadd3 { get; set; }
        public decimal Duemthadd4 { get; set; }
        public decimal Duedayuse1 { get; set; }
        public decimal Duedayuse2 { get; set; }
        public decimal Duedayuse3 { get; set; }
        public decimal Duedayuse4 { get; set; }
        public decimal Cntentered { get; set; }
        public decimal Pctduetot { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
