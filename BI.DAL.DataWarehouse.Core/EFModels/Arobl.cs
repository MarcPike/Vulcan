﻿using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Arobl
    {
        public string Coid { get; set; }
        public string Idcust { get; set; }
        public string Idinvc { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Idrmit { get; set; }
        public string Idordernbr { get; set; }
        public string Idcustpo { get; set; }
        public decimal Datedue { get; set; }
        public string Idnatacct { get; set; }
        public string Idcustshpt { get; set; }
        public short Trxtypeid { get; set; }
        public short Trxtypetxt { get; set; }
        public decimal Datebtch { get; set; }
        public decimal Cntbtch { get; set; }
        public decimal Cntitem { get; set; }
        public string Idgrp { get; set; }
        public string Descinvc { get; set; }
        public decimal Dateinvc { get; set; }
        public decimal Dateasof { get; set; }
        public string Codeterm { get; set; }
        public decimal Datedisc { get; set; }
        public string Codecurn { get; set; }
        public string Idratetype { get; set; }
        public short Swrateovrd { get; set; }
        public decimal Exchratehc { get; set; }
        public decimal Amtinvchc { get; set; }
        public decimal Amtduehc { get; set; }
        public decimal Amttxblhc { get; set; }
        public decimal Amtnontxhc { get; set; }
        public decimal Amttaxhc { get; set; }
        public decimal Amtdischc { get; set; }
        public decimal Amtinvctc { get; set; }
        public decimal Amtduetc { get; set; }
        public decimal Amttxbltc { get; set; }
        public decimal Amtnontxtc { get; set; }
        public decimal Amttaxtc { get; set; }
        public decimal Amtdisctc { get; set; }
        public short Swpaid { get; set; }
        public decimal Datelstact { get; set; }
        public decimal Datelststm { get; set; }
        public decimal Datelstdlq { get; set; }
        public short Codedlqsts { get; set; }
        public decimal Cnttotpaym { get; set; }
        public decimal Cntlstpaid { get; set; }
        public decimal Cntlstpyst { get; set; }
        public decimal Amtremit { get; set; }
        public decimal Cntlastseq { get; set; }
        public short Swtaxinput { get; set; }
        public string Codetax1 { get; set; }
        public string Codetax2 { get; set; }
        public string Codetax3 { get; set; }
        public string Codetax4 { get; set; }
        public string Codetax5 { get; set; }
        public decimal Amtbase1hc { get; set; }
        public decimal Amtbase2hc { get; set; }
        public decimal Amtbase3hc { get; set; }
        public decimal Amtbase4hc { get; set; }
        public decimal Amtbase5hc { get; set; }
        public decimal Amttax1hc { get; set; }
        public decimal Amttax2hc { get; set; }
        public decimal Amttax3hc { get; set; }
        public decimal Amttax4hc { get; set; }
        public decimal Amttax5hc { get; set; }
        public decimal Amtbase1tc { get; set; }
        public decimal Amtbase2tc { get; set; }
        public decimal Amtbase3tc { get; set; }
        public decimal Amtbase4tc { get; set; }
        public decimal Amtbase5tc { get; set; }
        public decimal Amttax1tc { get; set; }
        public decimal Amttax2tc { get; set; }
        public decimal Amttax3tc { get; set; }
        public decimal Amttax4tc { get; set; }
        public decimal Amttax5tc { get; set; }
        public string Codeslsp1 { get; set; }
        public string Codeslsp2 { get; set; }
        public string Codeslsp3 { get; set; }
        public string Codeslsp4 { get; set; }
        public string Codeslsp5 { get; set; }
        public decimal Pctsasplt1 { get; set; }
        public decimal Pctsasplt2 { get; set; }
        public decimal Pctsasplt3 { get; set; }
        public decimal Pctsasplt4 { get; set; }
        public decimal Pctsasplt5 { get; set; }
        public string Fiscyr { get; set; }
        public string Fiscper { get; set; }
        public string Idprepaid { get; set; }
        public decimal Datebus { get; set; }
        public decimal Ratedate { get; set; }
        public short Rateop { get; set; }
        public string Yplastact { get; set; }
        public string Idbank { get; set; }
        public decimal Depstnbr { get; set; }
        public decimal Postseqnce { get; set; }
        public short Swjob { get; set; }
        public short Swrtg { get; set; }
        public short Swrtgout { get; set; }
        public decimal Rtgdatedue { get; set; }
        public decimal Rtgoamthc { get; set; }
        public decimal Rtgamthc { get; set; }
        public decimal Rtgoamttc { get; set; }
        public decimal Rtgamttc { get; set; }
        public string Rtgterms { get; set; }
        public short Swrtgrate { get; set; }
        public string Rtgapplyto { get; set; }
        public int Values { get; set; }
        public string Srceappl { get; set; }
        public string Arversion { get; set; }
        public short Invctype { get; set; }
        public int Depseq { get; set; }
        public int Depline { get; set; }
        public string Typebtch { get; set; }
        public int Cntoblj { get; set; }
        public string Codecurnrc { get; set; }
        public decimal Raterc { get; set; }
        public string Ratetyperc { get; set; }
        public decimal Ratedaterc { get; set; }
        public short Rateoprc { get; set; }
        public short Swraterc { get; set; }
        public short Swtxrtgrpt { get; set; }
        public string Codetaxgrp { get; set; }
        public int Taxversion { get; set; }
        public short Swtxctlrc { get; set; }
        public short Taxclass1 { get; set; }
        public short Taxclass2 { get; set; }
        public short Taxclass3 { get; set; }
        public short Taxclass4 { get; set; }
        public short Taxclass5 { get; set; }
        public decimal Txbsert1tc { get; set; }
        public decimal Txbsert2tc { get; set; }
        public decimal Txbsert3tc { get; set; }
        public decimal Txbsert4tc { get; set; }
        public decimal Txbsert5tc { get; set; }
        public decimal Txamtrt1tc { get; set; }
        public decimal Txamtrt2tc { get; set; }
        public decimal Txamtrt3tc { get; set; }
        public decimal Txamtrt4tc { get; set; }
        public decimal Txamtrt5tc { get; set; }
        public string Idshipnbr { get; set; }
        public decimal Datefrstbk { get; set; }
        public decimal Datelstrvl { get; set; }
        public decimal Orate { get; set; }
        public string Oratetype { get; set; }
        public decimal Oratedate { get; set; }
        public short Orateop { get; set; }
        public short Oswrate { get; set; }
        public string Idacctset { get; set; }
        public decimal Datepaid { get; set; }
        public short Swnonrcvbl { get; set; }
        public string Codeterr { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
