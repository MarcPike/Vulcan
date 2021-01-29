using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Argro
    {
        public string Coid { get; set; }
        public string Idgrp { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Textdesc { get; set; }
        public short Swactv { get; set; }
        public decimal Dateinac { get; set; }
        public decimal Datelastmn { get; set; }
        public string Idacctset { get; set; }
        public string Idautocash { get; set; }
        public string Idbillcycl { get; set; }
        public string Idsvcchg { get; set; }
        public string Iddlnq { get; set; }
        public short Swbalfwd { get; set; }
        public string Codeterm { get; set; }
        public string Ratetype { get; set; }
        public short Swcrovrd { get; set; }
        public string Cdcrlmcur1 { get; set; }
        public decimal Amcrlmcur1 { get; set; }
        public string Cdcrlmcur2 { get; set; }
        public decimal Amcrlmcur2 { get; set; }
        public string Cdcrlmcur3 { get; set; }
        public decimal Amcrlmcur3 { get; set; }
        public string Cdcrlmcur4 { get; set; }
        public decimal Amcrlmcur4 { get; set; }
        public string Cdcrlmcur5 { get; set; }
        public decimal Amcrlmcur5 { get; set; }
        public int Values { get; set; }
        public string Codetaxgrp { get; set; }
        public short Taxstts1 { get; set; }
        public short Taxstts2 { get; set; }
        public short Taxstts3 { get; set; }
        public short Taxstts4 { get; set; }
        public short Taxstts5 { get; set; }
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
        public short Swprtstmt { get; set; }
        public short Swchklimit { get; set; }
        public short Swchkover { get; set; }
        public short Overdays { get; set; }
        public decimal Overamt1 { get; set; }
        public decimal Overamt2 { get; set; }
        public decimal Overamt3 { get; set; }
        public decimal Overamt4 { get; set; }
        public decimal Overamt5 { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
    }
}
