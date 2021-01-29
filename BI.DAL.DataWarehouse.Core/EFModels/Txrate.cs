using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Txrate
    {
        public string Coid { get; set; }
        public string Authority { get; set; }
        public short Ttype { get; set; }
        public short Buyerclass { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public decimal Itemrate1 { get; set; }
        public decimal Itemrate2 { get; set; }
        public decimal Itemrate3 { get; set; }
        public decimal Itemrate4 { get; set; }
        public decimal Itemrate5 { get; set; }
        public decimal Itemrate6 { get; set; }
        public decimal Itemrate7 { get; set; }
        public decimal Itemrate8 { get; set; }
        public decimal Itemrate9 { get; set; }
        public decimal Itemrate10 { get; set; }
        public decimal Lastmaint { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
