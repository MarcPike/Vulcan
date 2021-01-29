using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Appjh
    {
        public string Coid { get; set; }
        public string Typebtch { get; set; }
        public decimal Postseqnce { get; set; }
        public decimal Cntbtch { get; set; }
        public decimal Cntitem { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Idvend { get; set; }
        public string Idinvc { get; set; }
        public decimal Cntpaym { get; set; }
        public short Transtype { get; set; }
        public short Trxtype { get; set; }
        public string Idgrp { get; set; }
        public string Idacctset { get; set; }
        public string Codetaxgrp { get; set; }
        public string Codeterm { get; set; }
        public string Idrmitto { get; set; }
        public decimal Dateinvc { get; set; }
        public decimal Datedisc { get; set; }
        public decimal Datedue { get; set; }
        public decimal Datebtch { get; set; }
        public decimal Pctdisc { get; set; }
        public short Swnonrcvbl { get; set; }
        public short Swstatus { get; set; }
        public string Glbatch { get; set; }
        public string Glentry { get; set; }
        public decimal Cntadjnbr { get; set; }
        public string Fiscyr { get; set; }
        public string Fiscper { get; set; }
        public string Idinvcappl { get; set; }
        public string Desc { get; set; }
        public string Idbank { get; set; }
        public string Idrmit { get; set; }
        public int Longserial { get; set; }
        public string Miscrmitto { get; set; }
        public string Codecurntc { get; set; }
        public string Ratetypetc { get; set; }
        public decimal Ratedatetc { get; set; }
        public decimal Rateexchtc { get; set; }
        public short Rateoptc { get; set; }
        public short Swratetc { get; set; }
        public string Codecurnbc { get; set; }
        public string Ratetypebc { get; set; }
        public decimal Ratedatebc { get; set; }
        public decimal Rateexchbc { get; set; }
        public short Rateopbc { get; set; }
        public short Swratebc { get; set; }
        public decimal Amtinvctc { get; set; }
        public short Swjob { get; set; }
        public short Swrtg { get; set; }
        public string Rtgterms { get; set; }
        public decimal Rtgdatedue { get; set; }
        public short Swrtgrate { get; set; }
        public string Rtgapplyto { get; set; }
        public decimal Rtgamttc { get; set; }
        public decimal Rtgamthc { get; set; }
        public int Values { get; set; }
        public string Origcomp { get; set; }
        public string Iddistset { get; set; }
        public decimal Amttc { get; set; }
        public decimal Amthc { get; set; }
        public decimal Amtbc { get; set; }
        public string Paymcode { get; set; }
        public short Paymtype { get; set; }
        public string Textref { get; set; }
        public decimal Datebus { get; set; }
        public short Revinvc { get; set; }
        public string Idn { get; set; }
        public string Enteredby { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
