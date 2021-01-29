using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SupplierDocuments
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int BranchId { get; set; }
        public string DocumentType { get; set; }
        public int Number { get; set; }
        public int? BatchId { get; set; }
        public int? AccountingPeriod { get; set; }
        public string SupplierDocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? SupplierId { get; set; }
        public int? RemittanceAddressId { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal DocumentAmount { get; set; }
        public string TaxGroupCode { get; set; }
        public int? TaxClass1 { get; set; }
        public int? TaxClass2 { get; set; }
        public int? TaxClass3 { get; set; }
        public int? TaxClass4 { get; set; }
        public int? TaxClass5 { get; set; }
        public string TaxAuthority1 { get; set; }
        public string TaxAuthority2 { get; set; }
        public string TaxAuthority3 { get; set; }
        public string TaxAuthority4 { get; set; }
        public string TaxAuthority5 { get; set; }
        public bool TaxExempt1 { get; set; }
        public bool TaxExempt2 { get; set; }
        public bool TaxExempt3 { get; set; }
        public bool TaxExempt4 { get; set; }
        public bool TaxExempt5 { get; set; }
        public decimal VatRate1 { get; set; }
        public decimal VatRate2 { get; set; }
        public decimal VatRate3 { get; set; }
        public decimal VatRate4 { get; set; }
        public decimal VatRate5 { get; set; }
        public decimal VatAmount1 { get; set; }
        public decimal VatAmount2 { get; set; }
        public decimal VatAmount3 { get; set; }
        public decimal VatAmount4 { get; set; }
        public decimal VatAmount5 { get; set; }
        public string DocumentStatus { get; set; }
        public bool PaymentHold { get; set; }
        public string ChequeComments { get; set; }
        public int? TermsId { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public int? DiscountDays { get; set; }
        public DateTime? DiscountDate { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountableAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MatchedMaterialAmount { get; set; }
        public decimal MatchedProductionAmount { get; set; }
        public decimal MatchedTransportAmount { get; set; }
        public decimal MatchedMiscellaneousAmount { get; set; }
        public decimal MatchedSurchargeAmount { get; set; }
        public bool ProjectRelated { get; set; }
        public int? GatewayBatchId { get; set; }
        public int? PurchaseOrderId { get; set; }
        public string HeldReasons { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public bool? TaxOnly { get; set; }
        public string RemitToCode { get; set; }
        public string RemitToName { get; set; }
    }
}
