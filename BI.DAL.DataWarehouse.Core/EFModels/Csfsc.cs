using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Csfsc
    {
        public string Coid { get; set; }
        public string Fscyear { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public short Periods { get; set; }
        public short Qtr4perd { get; set; }
        public short Active { get; set; }
        public decimal Bgndate1 { get; set; }
        public decimal Bgndate2 { get; set; }
        public decimal Bgndate3 { get; set; }
        public decimal Bgndate4 { get; set; }
        public decimal Bgndate5 { get; set; }
        public decimal Bgndate6 { get; set; }
        public decimal Bgndate7 { get; set; }
        public decimal Bgndate8 { get; set; }
        public decimal Bgndate9 { get; set; }
        public decimal Bgndate10 { get; set; }
        public decimal Bgndate11 { get; set; }
        public decimal Bgndate12 { get; set; }
        public decimal Bgndate13 { get; set; }
        public decimal Enddate1 { get; set; }
        public decimal Enddate2 { get; set; }
        public decimal Enddate3 { get; set; }
        public decimal Enddate4 { get; set; }
        public decimal Enddate5 { get; set; }
        public decimal Enddate6 { get; set; }
        public decimal Enddate7 { get; set; }
        public decimal Enddate8 { get; set; }
        public decimal Enddate9 { get; set; }
        public decimal Enddate10 { get; set; }
        public decimal Enddate11 { get; set; }
        public decimal Enddate12 { get; set; }
        public decimal Enddate13 { get; set; }
        public short Statusadj { get; set; }
        public short Statuscls { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
