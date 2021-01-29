using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVCscom
    {
        public string Coid { get; set; }
        public string Orgid { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Coname { get; set; }
        public string Addr01 { get; set; }
        public string Addr02 { get; set; }
        public string Addr03 { get; set; }
        public string Addr04 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postal { get; set; }
        public string Country { get; set; }
        public string Loctype { get; set; }
        public string Loccode { get; set; }
        public short Phonefmt { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Contact { get; set; }
        public string Cntrycode { get; set; }
        public string Branch { get; set; }
        public short Perdfsc { get; set; }
        public short Qtr4perd { get; set; }
        public string Homecur { get; set; }
        public short Multicursw { get; set; }
        public string Ratetype { get; set; }
        public short Warndays { get; set; }
        public short Eurocursw { get; set; }
        public string Reportcur { get; set; }
        public short Hndlckfsc { get; set; }
        public short Hndinaacct { get; set; }
        public short Hndnexacct { get; set; }
        public short Gnlssmthd { get; set; }
        public string Taxnbr { get; set; }
        public string Legalname { get; set; }
        public string Brn { get; set; }
        public string Emailhost { get; set; }
        public string Emailuser { get; set; }
        public byte[] Emailpswd { get; set; }
        public short Emailport { get; set; }
        public short Emailssl { get; set; }
        public string Emailaddr { get; set; }
        public short Usesmtp { get; set; }
        public string Ccaddr { get; set; }
        public string Bccaddr { get; set; }
    }
}
