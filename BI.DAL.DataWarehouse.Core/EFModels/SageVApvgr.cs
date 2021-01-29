using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVApvgr
    {
        public string Coid { get; set; }
        public string Groupid { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Descriptn { get; set; }
        public short Activesw { get; set; }
        public decimal Inactivedt { get; set; }
        public decimal Lstmntdate { get; set; }
        public string Acctsetid { get; set; }
        public string Curncode { get; set; }
        public string Ratetypeid { get; set; }
        public string Bankid { get; set; }
        public short Prtsepchks { get; set; }
        public string Distsetid { get; set; }
        public string Distcode { get; set; }
        public string Glacctid { get; set; }
        public string Termcode { get; set; }
        public short Duplinvc { get; set; }
        public short Duplamt { get; set; }
        public short Dupldate { get; set; }
        public string Taxgrp { get; set; }
        public short Taxclass1 { get; set; }
        public short Taxclass2 { get; set; }
        public short Taxclass3 { get; set; }
        public short Taxclass4 { get; set; }
        public short Taxclass5 { get; set; }
        public short Taxrptsw { get; set; }
        public short Subjwthhsw { get; set; }
        public string Classid { get; set; }
        public string Paymcode { get; set; }
        public short Swdistby { get; set; }
        public short Swtxinc1 { get; set; }
        public short Swtxinc2 { get; set; }
        public short Swtxinc3 { get; set; }
        public short Swtxinc4 { get; set; }
        public short Swtxinc5 { get; set; }
        public int Values { get; set; }
    }
}
