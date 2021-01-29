using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVApobp
    {
        public string Coid { get; set; }
        public string Idvend { get; set; }
        public string Idinvc { get; set; }
        public decimal Cntpaymnbr { get; set; }
        public string Idrmit { get; set; }
        public decimal Datebus { get; set; }
        public short Transtype { get; set; }
        public decimal Cntseqnce { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public decimal Cntbtch { get; set; }
        public decimal Datebtch { get; set; }
        public decimal Amtpaymhc { get; set; }
        public decimal Amtpaymtc { get; set; }
        public string Codecurn { get; set; }
        public string Idratetype { get; set; }
        public decimal Rateexchhc { get; set; }
        public short Swovrdrate { get; set; }
        public string Idbank { get; set; }
        public short Trxtype { get; set; }
        public string Idmemoxref { get; set; }
        public string Idprepaid { get; set; }
        public string Idrmitvend { get; set; }
        public decimal Datermit { get; set; }
        public decimal Cntitem { get; set; }
        public string Fiscyr { get; set; }
        public string Fiscper { get; set; }
        public int Longserial { get; set; }
        public string Code1099 { get; set; }
        public decimal Amt1099 { get; set; }
        public decimal Ratedate { get; set; }
        public short Rateop { get; set; }
        public string Codetax { get; set; }
    }
}
