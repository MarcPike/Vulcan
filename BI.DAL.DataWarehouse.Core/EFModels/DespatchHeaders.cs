using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class DespatchHeaders
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? BranchId { get; set; }
        public int? Number { get; set; }
        public int? OrderId { get; set; }
        public string VehicleRegistration { get; set; }
        public string TrailerReference { get; set; }
        public int? DeliverToId { get; set; }
        public string DeliverToNameOverride { get; set; }
        public int? DeliverToAddressId { get; set; }
        public int? DeliverFromBranchId { get; set; }
        public int? DeliverFromWarehouseId { get; set; }
        public string DeliverFromNameOverride { get; set; }
        public int? DeliverFromAddressId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? RaisedDate { get; set; }
        public DateTime? DespatchedDate { get; set; }
        public DateTime? EtaDate { get; set; }
        public int? TransportTypeId { get; set; }
        public string DeliveryPoint { get; set; }
        public int? CarrierId { get; set; }
        public decimal? TransportCostRate { get; set; }
        public int? TransportCostRateUnitId { get; set; }
        public decimal? TransportCostAmount { get; set; }
        public decimal? TransportChargeRate { get; set; }
        public int? TransportChargeRateUnitId { get; set; }
        public decimal? TransportChargeAmount { get; set; }
        public decimal? TransportExchangeRate { get; set; }
        public int? TransportExchangeRateTypeId { get; set; }
        public bool? Printed { get; set; }
        public int? CertificationsId { get; set; }
        public string DespatchText { get; set; }
        public bool? Completed { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? InvoiceId { get; set; }
        public bool? Ready { get; set; }
        public DateTime? ReadyDate { get; set; }
        public DateTime? PrintedDate { get; set; }
        public int? VehicleRunStopId { get; set; }
        public bool? Proforma { get; set; }
        public string Type { get; set; }
        public string OrderHeaderText { get; set; }
        public decimal? DespatchNoteValue { get; set; }
        public bool? CreditHold { get; set; }
        public DateTime? CreditHoldDate { get; set; }
        public int? CreditHoldReason { get; set; }
        public DateTime? CreditReleaseDate { get; set; }
        public int? CreditReleaseUserId { get; set; }
        public decimal? CreditReleaseAmount { get; set; }
        public string CreditReleaseNotes { get; set; }
        public decimal? DespatchNoteTaxValue { get; set; }
        public DateTime? LoadingNotePrintDate { get; set; }
        public int? CertificateOfConformityId { get; set; }
        public string ProductSpecificationNotes { get; set; }
        public string RemarksNotes { get; set; }
        public string ConditionNotes { get; set; }
        public string ReleaseRequirementNotes { get; set; }
        public string SignatureNotes { get; set; }
        public int? RunReference { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public decimal? TransportChargeTax { get; set; }
    }
}
