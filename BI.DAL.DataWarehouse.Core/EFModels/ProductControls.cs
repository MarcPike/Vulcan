using System;
using System.Collections.Generic;

namespace BI.DAL.DataWarehouse.Core.Models
{
    public partial class ProductControls
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int? Version { get; set; }
        public DateTime? Cdate { get; set; }
        public int? CuserId { get; set; }
        public DateTime? Mdate { get; set; }
        public int? MuserId { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? WeightUnitId { get; set; }
        public int? QuantityUnitId { get; set; }
        public int? WeightDecimals { get; set; }
        public string ControlQuantity { get; set; }
        public bool? ControlPieces { get; set; }
        public int? Specification1Id { get; set; }
        public int? Specification2Id { get; set; }
        public int? Specification3Id { get; set; }
        public int? Specification4Id { get; set; }
        public int? Specification5Id { get; set; }
        public string Pcs2qtyCalc { get; set; }
        public string Wgt2qtyCalc { get; set; }
        public string Qty2pcsCalc { get; set; }
        public string Wgt2pcsCalc { get; set; }
        public string Qty2wgtCalc { get; set; }
        public string Pcs2wgtCalc { get; set; }
        public string WidthCalc { get; set; }
        public string LengthCalc { get; set; }
        public string HeightCalc { get; set; }
        public string DiameterCalc { get; set; }
        public string ZeroStockTrigger { get; set; }
        public int? Dim1TypeId { get; set; }
        public bool? Dim1Locked { get; set; }
        public int? Dim1UnitsId { get; set; }
        public int? Dim2TypeId { get; set; }
        public bool? Dim2Locked { get; set; }
        public int? Dim2UnitsId { get; set; }
        public int? Dim3TypeId { get; set; }
        public bool? Dim3Locked { get; set; }
        public int? Dim3UnitsId { get; set; }
        public int? Dim4TypeId { get; set; }
        public bool? Dim4Locked { get; set; }
        public int? Dim4UnitsId { get; set; }
        public int? Dim5TypeId { get; set; }
        public bool? Dim5Locked { get; set; }
        public int? Dim5UnitsId { get; set; }
        public string DensityLabel { get; set; }
        public int? PiecesUnitId { get; set; }
        public string DensityCalc { get; set; }
        public int? Dim1ToleranceUnitsId { get; set; }
        public int? Dim2ToleranceUnitsId { get; set; }
        public int? Dim3ToleranceUnitsId { get; set; }
        public int? Dim4ToleranceUnitsId { get; set; }
        public int? Dim5ToleranceUnitsId { get; set; }
        public int? Specification6Id { get; set; }
        public int? Specification7Id { get; set; }
        public int? Specification8Id { get; set; }
        public int? Specification9Id { get; set; }
        public int? Specification10Id { get; set; }
        public int? FirstOptimisableDimension { get; set; }
        public int? SecondOptimisableDimension { get; set; }
        public int? QuantityDecimals { get; set; }
        public decimal? DensityFactor { get; set; }
        public string Length2weightCalc { get; set; }
        public string Area2weightCalc { get; set; }
        public DateTime? EtlcreateDate { get; set; }
        public DateTime? EtlupdateDate { get; set; }
        public string TotalLengthCalc { get; set; }
        public int? TotalLengthUnitId { get; set; }
        public string TotalAreaCalc { get; set; }
        public int? TotalAreaUnitId { get; set; }
        public string WeightWidthCalc { get; set; }
        public string VolumeCalc { get; set; }
        public int? OutsideDiameterUnitId { get; set; }
    }
}
