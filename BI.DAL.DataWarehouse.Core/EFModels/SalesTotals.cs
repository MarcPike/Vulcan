using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class SalesTotals
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int Version { get; set; }
        public DateTime Cdate { get; set; }
        public DateTime Mdate { get; set; }
        public int CuserId { get; set; }
        public int MuserId { get; set; }
        public string Status { get; set; }
        public int? RequiredPieces { get; set; }
        public decimal? RequiredQuantity { get; set; }
        public decimal? RequiredWeight { get; set; }
        public int? DeliveredPieces { get; set; }
        public decimal? DeliveredQuantity { get; set; }
        public decimal? DeliveredWeight { get; set; }
        public int? BalancePieces { get; set; }
        public decimal? BalanceQuantity { get; set; }
        public decimal? BalanceWeight { get; set; }
        public decimal? CustomerMaterialValue { get; set; }
        public decimal? CustomerTransportValue { get; set; }
        public decimal? CustomerProductionValue { get; set; }
        public decimal? CustomerMiscellaneousValue { get; set; }
        public decimal? CustomerSurchargeValue { get; set; }
        public decimal? InternalMaterialValue { get; set; }
        public decimal? InternalTransportValue { get; set; }
        public decimal? InternalProductionValue { get; set; }
        public decimal? InternalMiscellaneousValue { get; set; }
        public decimal? InternalSurchargeValue { get; set; }
        public decimal? EstimatedMaterialCost { get; set; }
        public decimal? EstimatedTransportCost { get; set; }
        public decimal? EstimatedProductionCost { get; set; }
        public decimal? EstimatedMiscellaneousCost { get; set; }
        public decimal? EstimatedSurchargeCost { get; set; }
        public decimal? ActualMaterialCost { get; set; }
        public decimal? ActualTransportCost { get; set; }
        public decimal? ActualProductionCost { get; set; }
        public decimal? ActualMiscellaneousCost { get; set; }
        public decimal? ActualSurchargeCost { get; set; }
        public decimal? VatValue1 { get; set; }
        public decimal? VatValue2 { get; set; }
        public decimal? VatValue3 { get; set; }
        public decimal? VatValue4 { get; set; }
        public decimal? VatRate1 { get; set; }
        public decimal? VatRate2 { get; set; }
        public decimal? VatRate3 { get; set; }
        public decimal? VatRate4 { get; set; }
        public decimal? InvoicedValue { get; set; }
        public decimal? InvoicedTaxValue { get; set; }
        public decimal? BalanceValue { get; set; }
        public decimal? BalanceTaxValue { get; set; }
        public decimal? BaseInvoicedValue { get; set; }
        public decimal? BaseInvoicedTaxValue { get; set; }
        public decimal? BaseBalanceValue { get; set; }
        public decimal? BaseBalanceTaxValue { get; set; }
        public decimal? BaseCustomerMaterialValue { get; set; }
        public decimal? BaseCustomerTransportValue { get; set; }
        public decimal? BaseCustomerProductionValue { get; set; }
        public decimal? BaseCustomerMiscellaneousValue { get; set; }
        public decimal? BaseCustomerSurchargeValue { get; set; }
        public decimal? BaseInternalMaterialValue { get; set; }
        public decimal? BaseInternalTransportValue { get; set; }
        public decimal? BaseInternalProductionValue { get; set; }
        public decimal? BaseInternalMiscellaneousValue { get; set; }
        public decimal? BaseInternalSurchargeValue { get; set; }
        public decimal? BaseVatValue1 { get; set; }
        public decimal? BaseVatValue2 { get; set; }
        public decimal? BaseVatValue3 { get; set; }
        public decimal? BaseVatValue4 { get; set; }
        public decimal? OriginalExchangeRate { get; set; }
        public int? VatType1Id { get; set; }
        public int? VatType2Id { get; set; }
        public int? VatType3Id { get; set; }
        public int? VatType4Id { get; set; }
        public decimal? VatValue5 { get; set; }
        public decimal? BaseVatValue5 { get; set; }
        public decimal? VatRate5 { get; set; }
        public decimal? StockMaterialCost { get; set; }
        public decimal? StockTransportCost { get; set; }
        public decimal? StockProductionCost { get; set; }
        public decimal? StockMiscellaneousCost { get; set; }
        public decimal? StockSurchargeCost { get; set; }
        public decimal? InvoicedTotalCost { get; set; }
        public decimal? BalanceTotalCost { get; set; }
        public DateTime EtlcreateDate { get; set; }
        public DateTime EtlupdateDate { get; set; }
        public decimal? CallOffValue { get; set; }
        public decimal? CallOffTaxValue { get; set; }
        public decimal? BaseCallOffValue { get; set; }
        public decimal? BaseCallOffTaxValue { get; set; }
    }
}
