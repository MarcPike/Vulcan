using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class Branches
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string TaxReference { get; set; }
        public string CompanyRegistration { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public int? StockWarehouseId { get; set; }
        public int? FinishedWarehouseId { get; set; }
        public int? OffcutWarehouseId { get; set; }
        public int? WipWarehouseId { get; set; }
        public int? QuarantineWarehouseId { get; set; }
        public int? ScrapWarehouseId { get; set; }
        public int? ClaimWarehouseId { get; set; }
        public int? CustownWarehouseId { get; set; }
        public int? NominalBranchId { get; set; }
        public int? SequenceBranchId { get; set; }
        public int? CorporateEntityId { get; set; }
        public int? AddressId { get; set; }
        public int? CustomerId { get; set; }
        public int? CuttingGroupCostingDefaultsId { get; set; }
        public string LedgerSegmentCode { get; set; }
        public int? DeliveryBranchId { get; set; }
        public int? InboundAllocationManagerId { get; set; }
        public int? DefaultBuyerId { get; set; }
        public bool? StockTransferClearsOriginalParent { get; set; }
        public string InternalMaterialTransferGl { get; set; }
        public string InternalTransportTransferGl { get; set; }
        public string InternalProductionTransferGl { get; set; }
        public string InternalMiscellaneousTransferGl { get; set; }
        public string InternalSurchargeTransferGl { get; set; }
        public string InternalCreditMaterialTransferGl { get; set; }
        public string InternalCreditTransportTransferGl { get; set; }
        public string InternalCreditProductionTransferGl { get; set; }
        public string InternalCreditMiscellaneousTransferGl { get; set; }
        public string InternalCreditSurchargeTransferGl { get; set; }
        public string ExternalMaterialTransferGl { get; set; }
        public string ExternalTransportTransferGl { get; set; }
        public string ExternalProductionTransferGl { get; set; }
        public string ExternalMiscellaneousTransferGl { get; set; }
        public string ExternalSurchargeTransferGl { get; set; }
        public string ExternalCreditMaterialTransferGl { get; set; }
        public string ExternalCreditTransportTransferGl { get; set; }
        public string ExternalCreditProductionTransferGl { get; set; }
        public string ExternalCreditMiscellaneousTransferGl { get; set; }
        public string ExternalCreditSurchargeTransferGl { get; set; }
        public int? DefaultCertPrinterId { get; set; }
        public string StockNumberingPrefix { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? OutworkWarehouseId { get; set; }
    }
}
