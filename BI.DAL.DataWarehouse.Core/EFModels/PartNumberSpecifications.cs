﻿using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class PartNumberSpecifications
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public DateTime? Mdate { get; set; }
        public int? CuserId { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public int? CustomerId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ProductId { get; set; }
        public int? DimensionsId { get; set; }
        public decimal? EntryQuantity { get; set; }
        public int? EntryQuantityUnitId { get; set; }
        public int? StandardPieces { get; set; }
        public decimal? StandardQuantity { get; set; }
        public decimal? StandardWeight { get; set; }
        public bool? OverrideAllowed { get; set; }
        public int? ConsumptionProductId { get; set; }
        public int? ConsumptionDimensionsId { get; set; }
        public decimal? ProductionFactor { get; set; }
        public decimal? ExpectedScrap { get; set; }
        public string WorksNotes { get; set; }
        public string ProductionNotes { get; set; }
        public string InvoiceNotes { get; set; }
        public string PartNotes { get; set; }
        public int? SalesGroupId { get; set; }
        public string SpecificationValue1 { get; set; }
        public string SpecificationValue2 { get; set; }
        public string SpecificationValue3 { get; set; }
        public string SpecificationValue4 { get; set; }
        public string SpecificationValue5 { get; set; }
        public bool? ShowPrices { get; set; }
        public bool? UseMinimumGrade { get; set; }
        public int? EndUseId { get; set; }
        public string DocumentNumber { get; set; }
        public string RevisionNumber { get; set; }
        public string DrawingNumber { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? LastUsedDate { get; set; }
        public string PartStatus { get; set; }
        public string CustomerOrderNumber { get; set; }
        public string DescriptionFormula { get; set; }
        public string CompositeKey { get; set; }
        public decimal? OutsideDiameter { get; set; }
        public decimal? OutsideDiameterMinimum { get; set; }
        public decimal? OutsideDiameterMaximum { get; set; }
        public decimal? PackHeight { get; set; }
        public decimal? PackHeightMinimum { get; set; }
        public decimal? PackHeightMaximum { get; set; }
        public decimal? PackWeight { get; set; }
        public decimal? PackWeightMinimum { get; set; }
        public decimal? PackWeightMaximum { get; set; }
        public decimal? InsideDiameter { get; set; }
        public decimal? InsideDiameterMinimum { get; set; }
        public decimal? InsideDiameterMaximum { get; set; }
        public string SpecificationValue6 { get; set; }
        public string SpecificationValue7 { get; set; }
        public string SpecificationValue8 { get; set; }
        public string SpecificationValue9 { get; set; }
        public string SpecificationValue10 { get; set; }
        public int? ProcessPlanId { get; set; }
        public int? PackCountMinimum { get; set; }
        public int? PackCountMaximum { get; set; }
        public string PartCompanyType { get; set; }
        public string InternalNotes { get; set; }
        public string GoodsInwardsNotes { get; set; }
        public int? PurchaseGroupId { get; set; }
        public decimal? AdjustmentPrice { get; set; }
        public decimal? YieldPercentage { get; set; }
        public string PriceCode { get; set; }
        public bool? MechanicalCert { get; set; }
        public bool? ShowCountryOfMaterialOrigin { get; set; }
        public bool? ShowCountryOfPrimaryProcessing { get; set; }
        public bool? ShowCountryOfFinalProcessing { get; set; }
        public string AcknowledgementNotes { get; set; }
        public string DespatchNotes { get; set; }
        public bool? InvoicePacking { get; set; }
        public int? PartProcessTypeId { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public int? FabricationId { get; set; }
        public int? FabricationProcessId { get; set; }
        public int? FabricationWorkCentreId { get; set; }
        public string BaseSpecification { get; set; }
        public string PartSpecification { get; set; }
        public string AdditionalSpecification1 { get; set; }
        public string AdditionalSpecification2 { get; set; }
        public string AdditionalSpecification3 { get; set; }
        public string AdditionalSpecification4 { get; set; }
    }
}
