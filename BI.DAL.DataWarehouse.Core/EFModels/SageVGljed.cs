using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SageVGljed
    {
        public string Coid { get; set; }
        public string Batchnbr { get; set; }
        public string Journalid { get; set; }
        public string Transnbr { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public string Acctid { get; set; }
        public string Companyid { get; set; }
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
        public string Transdesc { get; set; }
        public string Transref { get; set; }
        public decimal Transdate { get; set; }
        public string Srceldgr { get; set; }
        public string Srcetype { get; set; }
        public int Values { get; set; }
        public string Descomp { get; set; }
        public short Route { get; set; }
    }
}
