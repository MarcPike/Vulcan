using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Glabk
    {
        public string Coid { get; set; }
        public string Acctblkid { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Statusactv { get; set; }
        public string Ablktype { get; set; }
        public string Ablkdesc { get; set; }
        public decimal Ablklen { get; set; }
        public string Ablkvalue { get; set; }
        public decimal Abrkusage { get; set; }
        public decimal Avhusage { get; set; }
        public decimal Arhusage { get; set; }
        public decimal Dateeff { get; set; }
        public short Reqdsw { get; set; }
        public short Closesw { get; set; }
        public string Ablkdelm { get; set; }
        public byte[] Numofkey { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
    }
}
