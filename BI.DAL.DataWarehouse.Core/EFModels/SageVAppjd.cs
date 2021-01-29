using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVAppjd
    {
        public string Coid { get; set; }
        public string Typebtch { get; set; }
        public decimal Postseqnce { get; set; }
        public decimal Cntbtch { get; set; }
        public decimal Cntitem { get; set; }
        public int Cntseqence { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Idvend { get; set; }
        public string Idinvc { get; set; }
        public decimal Cntpaym { get; set; }
        public short Transtype { get; set; }
        public short Idtrans { get; set; }
        public decimal Datebus { get; set; }
        public decimal Dateinvc { get; set; }
        public decimal Datedisc { get; set; }
        public decimal Datedue { get; set; }
        public string Idbank { get; set; }
        public string Idrmit { get; set; }
        public int Longserial { get; set; }
        public decimal Cntline { get; set; }
        public string Fiscyr { get; set; }
        public string Fiscper { get; set; }
        public string Idacct { get; set; }
        public short Accttype { get; set; }
        public string Srcetype { get; set; }
        public string Glref { get; set; }
        public string Gldesc { get; set; }
        public string Codecurn { get; set; }
        public string Idratetype { get; set; }
        public decimal Ratedate { get; set; }
        public decimal Rateexchhc { get; set; }
        public short Swtaxtype { get; set; }
        public string Iddist { get; set; }
        public decimal Amtextndhc { get; set; }
        public decimal Amtextndtc { get; set; }
        public decimal Basetaxhc { get; set; }
        public decimal Basetaxtc { get; set; }
        public decimal Amttaxhc { get; set; }
        public decimal Amttaxtc { get; set; }
        public string Glbatch { get; set; }
        public string Glentry { get; set; }
        public decimal Cntadjnbr { get; set; }
        public decimal Amtadjhcur { get; set; }
        public decimal Amtadjtcur { get; set; }
        public decimal Amtdschcur { get; set; }
        public decimal Amtdsctcur { get; set; }
        public decimal Rtgdatedue { get; set; }
        public short Swrtgrate { get; set; }
        public decimal Rtgamttc { get; set; }
        public decimal Rtgamthc { get; set; }
        public int Values { get; set; }
        public string Descomp { get; set; }
        public short Route { get; set; }
        public string Glcomment { get; set; }
        public decimal Ratedoc { get; set; }
    }
}
