using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SalesItemsExt
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public string Discriminator { get; set; }
        public int? ItemNumber { get; set; }
        public int? SalesHeaderId { get; set; }
        public int? ProductId { get; set; }
        public int? InternalStatusId { get; set; }
        public string OrderItemInternalStatusCode { get; set; }
        public string OrderItemInternalStatusDescription { get; set; }
        public string OrderItemStatusCode { get; set; }
        public string OrderItemStatusDescription { get; set; }
        public string SalesGroupCode { get; set; }
        public string SalesGroupDescription { get; set; }
        public string SalesGroupTypeCode { get; set; }
        public string SalesGroupTypeDescription { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public decimal? Length { get; set; }
        public string OrderItemPartNumber { get; set; }
        public string OrderItemPartName { get; set; }
        public string Abc { get; set; }
        public string LeadTime { get; set; }
        public DateTime? DueDate { get; set; }
        public int? DespatchBranchId { get; set; }
        public int? DespatchHeaderId { get; set; }
        public int? DespatchItemId { get; set; }
        public string DespatchBranchCode { get; set; }
        public int? DespatchNumber { get; set; }
        public int? DespatchItemNumber { get; set; }
        public string DespatchItemReference { get; set; }
        public int? OrderBranchId { get; set; }
        public int? OrderHeaderId { get; set; }
        public int? OrderItemId { get; set; }
        public string OrderBranchCode { get; set; }
        public int? OrderNumber { get; set; }
        public int? OrderItemNumber { get; set; }
        public string SoitemReference { get; set; }
        public DateTime? OrderSaleDate { get; set; }
        public string DrawingNumber { get; set; }
        public string EndUseCode { get; set; }
        public string EndUseDescription { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
