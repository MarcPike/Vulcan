using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Glpost
    {
        public string Coid { get; set; }
        public string Acctid { get; set; }
        public string Fiscalyr { get; set; }
        public string Fiscalperd { get; set; }
        public string Srcecurn { get; set; }
        public string Srceledger { get; set; }
        public string Srcetype { get; set; }
        public decimal Postingseq { get; set; }
        public decimal Cntdetail { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public decimal Jrnldate { get; set; }
        public string Batchnbr { get; set; }
        public string Entrynbr { get; set; }
        public decimal Transnbr { get; set; }
        public short Editallowd { get; set; }
        public short Consolidat { get; set; }
        public string Companyid { get; set; }
        public string Jnldtldesc { get; set; }
        public string Jnldtlref { get; set; }
        public decimal Transamt { get; set; }
        public decimal Transqty { get; set; }
        public string Scurndec { get; set; }
        public decimal Scurnamt { get; set; }
        public string Hcurncode { get; set; }
        public string Ratetype { get; set; }
        public string Scurncode { get; set; }
        public decimal Ratedate { get; set; }
        public decimal Convrate { get; set; }
        public decimal Ratespread { get; set; }
        public string Datemtchcd { get; set; }
        public string Rateoper { get; set; }
        public short Drilsrcty { get; set; }
        public decimal Drilldwnlk { get; set; }
        public string Drilapp { get; set; }
        public decimal Rptamt { get; set; }
        public int Values { get; set; }
        public decimal? Docdate { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
