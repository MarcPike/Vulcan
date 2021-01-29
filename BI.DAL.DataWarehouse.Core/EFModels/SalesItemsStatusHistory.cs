using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SalesItemsStatusHistory
    {
        public string SalesOrderStatusHistoryViewId { get; set; }
        public string Coid { get; set; }
        public long EventId { get; set; }
        public int SalesHeaderId { get; set; }
        public int? OrderNumber { get; set; }
        public int SalesItemId { get; set; }
        public int? ItemNumber { get; set; }
        public DateTime SalesItemCreatedDate { get; set; }
        public DateTime NewStatusUpdatedOn { get; set; }
        public string NewValues { get; set; }
        public string OldValues { get; set; }
        public long? RecordId { get; set; }
        public int? NewStatusId { get; set; }
        public int? OrderItemDaysAtStatus { get; set; }
        public int? StatusSeqNumber { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
    }
}
