using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProductsTheoreticalWeight
    {
        public string Coid { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string ProductSizeDescription { get; set; }
        public string ProductCategory { get; set; }
        public string ProductGrade { get; set; }
        public string ProductSize { get; set; }
        public int? Dim1TypeId { get; set; }
        public string Dim1Type { get; set; }
        public decimal? Dim1StaticDimension { get; set; }
        public int? Dim2TypeId { get; set; }
        public string Dim2Type { get; set; }
        public decimal? Dim2StaticDimension { get; set; }
        public int? Dim3TypeId { get; set; }
        public string Dim3Type { get; set; }
        public decimal? Dim3StaticDimension { get; set; }
        public int? ProductcontrolsDim1TypeId { get; set; }
        public string ProductcontrolsDim1Type { get; set; }
        public int? ProductcontrolsDim1UnitsId { get; set; }
        public string ProductcontrolsDim1Units { get; set; }
        public int? ProductcontrolsDim2TypeId { get; set; }
        public string ProductcontrolsDim2Type { get; set; }
        public int? ProductcontrolsDim2UnitsId { get; set; }
        public string ProductcontrolsDim2Units { get; set; }
        public int? ProductcontrolsDim3TypeId { get; set; }
        public string ProductcontrolsDim3Type { get; set; }
        public int? ProductcontrolsDim3UnitsId { get; set; }
        public string ProductcontrolsDim3Units { get; set; }
        public string ProductSpec { get; set; }
        public string ProductCondition { get; set; }
        public string ProductItem { get; set; }
        public string ProductBatch { get; set; }
        public string ProductCusSpec { get; set; }
        public string ProductAbc { get; set; }
        public string ProductLeadTime { get; set; }
        public string ProductStratificationGrade { get; set; }
        public int? PiecesUnitId { get; set; }
        public string PiecesUom { get; set; }
        public int? QuantityUnitId { get; set; }
        public string QuantityUom { get; set; }
        public int? WeightUnitId { get; set; }
        public string WeightUom { get; set; }
        public bool? Pcs { get; set; }
        public decimal? Lgt { get; set; }
        public decimal? Wdt { get; set; }
        public decimal? Hgt { get; set; }
        public decimal? OutsideDiameter { get; set; }
        public decimal? InsideDiameter { get; set; }
        public decimal? Qtd { get; set; }
        public decimal? Avd { get; set; }
        public int? ProductControlsId { get; set; }
        public string ProductControlsCode { get; set; }
        public string ProductControlsDescription { get; set; }
        public string Pcs2wgtCalc { get; set; }
        public decimal? TheoreticalWeight { get; set; }
        public int LengthMultiplier { get; set; }
        public int? ProductCategoryId { get; set; }
        public string StockAnalysisCode1 { get; set; }
        public string StockAnalysisDesc1 { get; set; }
        public string StockAnalysisCode2 { get; set; }
        public string StockAnalysisDesc2 { get; set; }
        public int? AlternateProductId { get; set; }
        public string AlternateProductCode { get; set; }
        public string AlternateProductDescription { get; set; }
        public string AlternateProductCategory { get; set; }
        public string AlternateProductGrade { get; set; }
        public string AlternateProductSize { get; set; }
        public string AlternateProductCondition { get; set; }
        public decimal? AlternateProductTheoreticalWeight { get; set; }
        public int? AlternateProductLengthMultiplier { get; set; }
        public decimal? AlternateProductMinimumReorder { get; set; }
        public string AlternateProductMinimumReorderUom { get; set; }
        public string AlternateProductStratificationGrade { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string ProductCodePartOne { get; set; }
        public string ProductCodePartTwo { get; set; }
        public string ProductCodePartThree { get; set; }
        public string ProductAnalysisCode3 { get; set; }
        public string Status { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
    }
}
