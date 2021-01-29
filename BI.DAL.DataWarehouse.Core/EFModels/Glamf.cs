using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Glamf
    {
        public string Coid { get; set; }
        public string Acctid { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public decimal Createdate { get; set; }
        public string Acctdesc { get; set; }
        public string Accttype { get; set; }
        public string Acctbal { get; set; }
        public short Activesw { get; set; }
        public short Consldsw { get; set; }
        public short Qtysw { get; set; }
        public string Uom { get; set; }
        public short Allocsw { get; set; }
        public string Acctofset { get; set; }
        public string Acctsrty { get; set; }
        public short Mcsw { get; set; }
        public short Specsw { get; set; }
        public string Acctgrpcod { get; set; }
        public short Ctrlacctsw { get; set; }
        public string Srceldgid { get; set; }
        public decimal Alloctot { get; set; }
        public string Abrkid { get; set; }
        public decimal Yracctclos { get; set; }
        public string Acctfmttd { get; set; }
        public string Acsegval01 { get; set; }
        public string Acsegval02 { get; set; }
        public string Acsegval03 { get; set; }
        public string Acsegval04 { get; set; }
        public string Acsegval05 { get; set; }
        public string Acsegval06 { get; set; }
        public string Acsegval07 { get; set; }
        public string Acsegval08 { get; set; }
        public string Acsegval09 { get; set; }
        public string Acsegval10 { get; set; }
        public string Acctsegval { get; set; }
        public string Acctgrpscd { get; set; }
        public string Postosegid { get; set; }
        public string Defcurncod { get; set; }
        public int Ovalues { get; set; }
        public int Tovalues { get; set; }
        public short Rollupsw { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
