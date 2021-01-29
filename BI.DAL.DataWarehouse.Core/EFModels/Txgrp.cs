using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Txgrp
    {
        public string Coid { get; set; }
        public string Groupid { get; set; }
        public short Ttype { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Desc { get; set; }
        public string Srccurn { get; set; }
        public string Authority1 { get; set; }
        public string Authority2 { get; set; }
        public string Authority3 { get; set; }
        public string Authority4 { get; set; }
        public string Authority5 { get; set; }
        public short Taxable1 { get; set; }
        public short Taxable2 { get; set; }
        public short Taxable3 { get; set; }
        public short Taxable4 { get; set; }
        public short Taxable5 { get; set; }
        public short Calcmethod { get; set; }
        public decimal Lastmaint { get; set; }
        public short Surtax1 { get; set; }
        public short Surtax2 { get; set; }
        public short Surtax3 { get; set; }
        public short Surtax4 { get; set; }
        public short Surtax5 { get; set; }
        public string Surauth1 { get; set; }
        public string Surauth2 { get; set; }
        public string Surauth3 { get; set; }
        public string Surauth4 { get; set; }
        public string Surauth5 { get; set; }
        public string Tratetype { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
