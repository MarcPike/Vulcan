using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VSalesItemsStatusHistory
    {
        public string SalesOrderStatusHistoryViewId { get; set; }
        public string Coid { get; set; }
        public long EventId { get; set; }
        public int SalesHeaderId { get; set; }
        public int? OrderNumber { get; set; }
        public int SalesItemId { get; set; }
        public int? ItemNumber { get; set; }
        public DateTime SalesItemCreatedDate { get; set; }
        public int? StatusSeqNumber { get; set; }
        public int? NewStatusId { get; set; }
        public string NewStatus { get; set; }
        public string NewStatusCode { get; set; }
        public string NewStatusDescription { get; set; }
        public DateTime NewStatusUpdatedOn { get; set; }
        public long? StatusSeqNumberDesc { get; set; }
        public int? OrderItemDaysAtStatus { get; set; }
    }
}
