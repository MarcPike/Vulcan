using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class VCustomerPartNumberSpecifications
    {
        public string ViewId { get; set; }
        public string Coid { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string SalesGroupCode { get; set; }
        public string SalesGroup { get; set; }
        public string SalesGroupTypeCode { get; set; }
        public string SalesGroupType { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public DateTime? LastUsedDate { get; set; }
        public string PartStatus { get; set; }
        public string CompanyType { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string CustomerOrderNumber { get; set; }
        public string DescriptionFormula { get; set; }
        public string DocumentNumber { get; set; }
        public string DrawingNumber { get; set; }
        public decimal? EntryQuantity { get; set; }
        public decimal? ExpectedScrap { get; set; }
        public decimal? InsideDiameter { get; set; }
        public decimal? InsideDiameterMaximum { get; set; }
        public decimal? InsideDiameterMinimum { get; set; }
        public string InvoiceNotes { get; set; }
        public decimal? OutsideDiameter { get; set; }
        public decimal? OutsideDiameterMaximum { get; set; }
        public decimal? OutsideDiameterMinimum { get; set; }
        public bool? OverrideAllowed { get; set; }
        public decimal? PackHeight { get; set; }
        public decimal? PackHeightMaximum { get; set; }
        public decimal? PackHeightMinimum { get; set; }
        public decimal? PackWeight { get; set; }
        public decimal? PackWeightMaximum { get; set; }
        public decimal? PackWeightMinimum { get; set; }
        public string PartNotes { get; set; }
        public decimal? ProductionFactor { get; set; }
        public string ProductionNotes { get; set; }
        public string RevisionNumber { get; set; }
        public bool? ShowPrices { get; set; }
        public string SpecificationValue1 { get; set; }
        public string SpecificationValue2 { get; set; }
        public string SpecificationValue3 { get; set; }
        public string SpecificationValue4 { get; set; }
        public string SpecificationValue5 { get; set; }
        public string SpecificationValue6 { get; set; }
        public string SpecificationValue7 { get; set; }
        public string SpecificationValue8 { get; set; }
        public string SpecificationValue9 { get; set; }
        public string SpecificationValue10 { get; set; }
        public int? StandardPieces { get; set; }
        public decimal? StandardQuantity { get; set; }
        public decimal? StandardWeight { get; set; }
        public bool? UseMinimumGrade { get; set; }
        public string WorkNotes { get; set; }
        public string PartCreatedBy { get; set; }
        public DateTimeOffset? PartCreatedDate { get; set; }
        public string PartModifiedBy { get; set; }
        public DateTimeOffset? PartModifiedDate { get; set; }
        public string HasExternalCosts { get; set; }
        public string HasInternalCosts { get; set; }
        public string HasCharges { get; set; }
        public decimal? Price { get; set; }
        public string PriceUom { get; set; }
        public decimal? Dim1 { get; set; }
        public decimal? Dim2 { get; set; }
        public decimal? Dim3 { get; set; }
        public decimal? Dim4 { get; set; }
        public decimal? Dim5 { get; set; }
        public string Grade { get; set; }
        public string Category { get; set; }
        public string Size { get; set; }
        public string Condition { get; set; }
        public string SizeDescription { get; set; }
        public string InvoicePacking { get; set; }
        public decimal? TheoWeightPerInch { get; set; }
        public decimal? TheoWeight { get; set; }
        public string ProcessPlanCreated { get; set; }
        public string IdealConsumptionProductCode { get; set; }
        public string IdealConsumptionProductCategory { get; set; }
        public string IdealConsumptionProductSize { get; set; }
        public string IdealConsumptionSizeDescription { get; set; }
        public string IdealConsumptionProductCondition { get; set; }
        public decimal? IdealConsumptionProductTheoWeightPerInch { get; set; }
        public decimal? IdealConsumptionProductTheoWeight { get; set; }
        public decimal? IdealConsumptionProductLength { get; set; }
        public string EndUseCode { get; set; }
        public string EndUseDescription { get; set; }
    }
}
