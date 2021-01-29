using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VDeliveryPerformanceReport
    {
        public string Id { get; set; }
        public string Coid { get; set; }
        public int SalesOrderItemId { get; set; }
        public int? SalesOrderBranchId { get; set; }
        public string SalesOrderBranch { get; set; }
        public DateTime? AccountingPeriod { get; set; }
        public string SalesOrderReference { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerOrderNumber { get; set; }
        public string DespatchOrderReference { get; set; }
        public DateTime? SalesOrderItemCreatedDate { get; set; }
        public DateTime? SalesOrderItemDueDate { get; set; }
        public DateTime? SalesOrderItemOriginalDueDate { get; set; }
        public DateTime? DespatchedDate { get; set; }
        public string ProductCode { get; set; }
        public int? DaysLate { get; set; }
        public int? SalesOrderNumber { get; set; }
        public int? SalesOrderItemNumber { get; set; }
        public string Salesperson { get; set; }
        public string SalesGroup { get; set; }
        public string LateReason { get; set; }
        public string SubLateReason { get; set; }
        public string DeliverToCode { get; set; }
        public int? DeliveredPieces { get; set; }
    }
}
