using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Txauth
    {
        public string Coid { get; set; }
        public string Authority { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Desc { get; set; }
        public string Scurn { get; set; }
        public decimal Maxtax { get; set; }
        public decimal Mintax { get; set; }
        public short Txbase { get; set; }
        public short Includable { get; set; }
        public short Liabtiming { get; set; }
        public string Liability { get; set; }
        public string Defer { get; set; }
        public string Deferrecov { get; set; }
        public short Discbase { get; set; }
        public short Auditlevel { get; set; }
        public short Recoverabl { get; set; }
        public decimal Raterecov { get; set; }
        public string Acctrecov { get; set; }
        public short Expseparte { get; set; }
        public string Acctexp { get; set; }
        public decimal Lastmaint { get; set; }
        public short Taxtype { get; set; }
        public short Txrtgctl { get; set; }
        public string Refnumber { get; set; }
        public string Refname { get; set; }
        public string Acctwht { get; set; }
        public string Inputacct { get; set; }
        public string Outputacct { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
