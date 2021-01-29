using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVArras
    {
        public string Coid { get; set; }
        public string Idacctset { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Textdesc { get; set; }
        public short Actvsw { get; set; }
        public decimal Dateainac { get; set; }
        public decimal Lastmntn { get; set; }
        public string Aridacct { get; set; }
        public string Idsusp { get; set; }
        public string Cashliab { get; set; }
        public string Acctdisc { get; set; }
        public string Acctwrof { get; set; }
        public string Curncode { get; set; }
        public string Unrlgain { get; set; }
        public string Unrlloss { get; set; }
        public string Rlzdgain { get; set; }
        public string Rlzdloss { get; set; }
        public string Acctadj { get; set; }
        public string Rtgacct { get; set; }
        public string Rndacct { get; set; }
    }
}
