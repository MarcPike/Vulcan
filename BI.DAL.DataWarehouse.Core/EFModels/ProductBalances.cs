using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProductBalances
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? BranchId { get; set; }
        public int? ProductId { get; set; }
        public int? PhysicalPieces { get; set; }
        public decimal? PhysicalWeight { get; set; }
        public decimal? PhysicalQuantity { get; set; }
        public decimal? PhysicalValue { get; set; }
        public int? AveragingPieces { get; set; }
        public decimal? AveragingWeight { get; set; }
        public decimal? AveragingQuantity { get; set; }
        public decimal? AveragingValue { get; set; }
        public int? QuarantinePieces { get; set; }
        public decimal? QuarantineWeight { get; set; }
        public decimal? QuarantineQuantity { get; set; }
        public int? SalesAllocatedPieces { get; set; }
        public decimal? SalesAllocatedWeight { get; set; }
        public decimal? SalesAllocatedQuantity { get; set; }
        public int? SalesOrderPieces { get; set; }
        public decimal? SalesOrderWeight { get; set; }
        public decimal? SalesOrderQuantity { get; set; }
        public int? ProductionAllocatedPieces { get; set; }
        public decimal? ProductionAllocatedWeight { get; set; }
        public decimal? ProductionAllocatedQuantity { get; set; }
        public int? TransientPieces { get; set; }
        public decimal? TransientWeight { get; set; }
        public decimal? TransientQuantity { get; set; }
        public int? TransientAllocPieces { get; set; }
        public decimal? TransientAllocWeight { get; set; }
        public decimal? TransientAllocQuantity { get; set; }
        public int? IncomingPieces { get; set; }
        public decimal? IncomingWeight { get; set; }
        public decimal? IncomingQuantity { get; set; }
        public decimal? IncomingValue { get; set; }
        public int? ReservedPieces { get; set; }
        public decimal? ReservedWeight { get; set; }
        public decimal? ReservedQuantity { get; set; }
        public int? SalesReservedPieces { get; set; }
        public decimal? SalesReservedWeight { get; set; }
        public decimal? SalesReservedQuantity { get; set; }
        public decimal? LastAverageCost { get; set; }
        public int? ProductionDuePieces { get; set; }
        public decimal? ProductionDueQuantity { get; set; }
        public decimal? ProductionDueWeight { get; set; }
        public int? StockUnavailablePieces { get; set; }
        public decimal? StockUnavailableWeight { get; set; }
        public decimal? StockUnavailableQuantity { get; set; }
        public int? ProductionDueAllocatedPieces { get; set; }
        public decimal? ProductionDueAllocatedQuantity { get; set; }
        public decimal? ProductionDueAllocatedWeight { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? IncomingTransferPieces { get; set; }
        public decimal? IncomingTransferQuantity { get; set; }
        public decimal? IncomingTransferWeight { get; set; }
        public int? IncomingTransferAllocatedPieces { get; set; }
        public decimal? IncomingTransferAllocatedQuantity { get; set; }
        public decimal? IncomingTransferAllocatedWeight { get; set; }
    }
}
