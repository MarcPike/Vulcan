using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class GlbctlUsaarc
    {
        public string Batchid { get; set; }
        public decimal Audtdate { get; set; }
        public decimal Audttime { get; set; }
        public string Audtuser { get; set; }
        public string Audtorg { get; set; }
        public short Activesw { get; set; }
        public string Btchdesc { get; set; }
        public string Srceledgr { get; set; }
        public decimal Datecreat { get; set; }
        public decimal Dateedit { get; set; }
        public string Batchtype { get; set; }
        public string Batchstat { get; set; }
        public decimal Postngseq { get; set; }
        public decimal Debittot { get; set; }
        public decimal Credittot { get; set; }
        public decimal Qtytotal { get; set; }
        public decimal Entrycnt { get; set; }
        public decimal Nextentry { get; set; }
        public decimal Errorcnt { get; set; }
        public string Origstatus { get; set; }
        public short Swprinted { get; set; }
        public short Swict { get; set; }
        public short Swrvrecog { get; set; }
    }
}
