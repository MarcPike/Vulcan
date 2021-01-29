using System;
using Devart.Data.Linq;
using Devart.Data.Linq.Mapping;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using Vulcan.IMetal.Helpers;
using Vulcan.IMetal.Models;

namespace Vulcan.IMetal.Context.PurchaseOrders
{

    public partial class PurchaseOrderItem
    {
        public DimensionsModel PC_Dimensions = new DimensionsModel();
        public string Coid { get; set; }
        public string PC_CategoryCode => PurchaseOrderHeader_PurchaseHeaderId.PurchaseCategoryCode?.Code;
        public string PC_CategoryCodeDescription => PurchaseOrderHeader_PurchaseHeaderId.PurchaseCategoryCode?.Description;
        public string PC_ProductCode => Product.Code;
        public string PC_ProductCodeDescription => Product.Description;
        public string PC_MetalType => Product?.ProductCategory?.StockAnalysisCode_Analysis1Id?.Description;
        public string PC_MetalType2 => Product?.ProductCategory?.StockAnalysisCode_Analysis2Id?.Description;
        public string PC_StockType => Product?.ProductCategory?.ProductControl?.Description;
        public string PC_ProductCategory => Product?.ProductCategory?.Category;
        public string PC_ProductGrade => Product?.StockGrade_GradeId?.Code;
        public string PC_ProductCondition => Product?.SpecificationValue2;
        public string PC_ProductSize => Product?.SizeDescription;
        public DateTime PC_CreateDate => Cdate ?? DateTime.Parse("1/1/1980");
        public DateTime PC_DueDate => DueDate ?? DateTime.Parse("1/1/1980");
        public string PC_HeatNumber
        {
            get
            {
                if (Product.StockCast == null)
                    return "";

                return Product?.StockCast?.FirstOrDefault()?.CastNumber ?? "";
            }
        }
        public string PC_ProductSizeDescription => Product.SizeDescription ?? "";
        public string PC_MaterialTypeDescription
        {
            get
            {
                if (Product.ProductCategory.StockAnalysisCode_Analysis2Id == null)
                    return "";

                return Product.ProductCategory.StockAnalysisCode_Analysis2Id.Description ?? "";

            }
        }
        public string PC_MaterialType
        {
            get
            {
                if (Product.ProductCategory.StockAnalysisCode_Analysis1Id == null)
                    return "";

                return Product.ProductCategory.StockAnalysisCode_Analysis1Id.Description ?? "";
            }
        }


        public decimal PC_OutsideDiameter => PC_Dimensions.OuterDiameter;
        public decimal PC_InsideDiameter => PC_Dimensions.InsideDiameter;
        public decimal PC_Thick => PC_Dimensions.Thick;
        public decimal PC_Width => PC_Dimensions.Width;
        public decimal PC_Length => PC_Dimensions.Length;
        public decimal PC_Density => PC_Dimensions.Density;
        public string PC_DensityUnit => Product?.ProductCategory?.ProductControl?.DensityLabel;


        public int PC_OrderedPieces => OrderedPiece ?? 0;
        public int PC_BalancePieces => BalancePiece ?? 0;
        public int PC_AllocatedPieces => AllocatedPiece ?? 0;
        public int PC_AvailablePieces => PC_OrderedPieces - PC_AllocatedPieces;
        public string PC_PiecesUnit => Product?.ProductCategory?.ProductControl?.UnitsOfMeasure_PiecesUnitId?.Code;


        public decimal PC_OrderedQuantity => OrderedQuantity ?? 0;
        public decimal PC_BalanceQuantity => BalanceQuantity ?? 0;
        public decimal PC_AllocatedQuantity => AllocatedQuantity ?? 0;
        public decimal PC_AvailableQuantity => PC_OrderedQuantity - PC_AllocatedQuantity;
        public string PC_QuantityUnit => Product?.ProductCategory?.ProductControl?.UnitsOfMeasure_QuantityUnitId?.Code;


        public decimal PC_OrderedLengthBase => UomHelper.QuantityIsLength(PC_QuantityUnit) ? PC_OrderedQuantity : 0;
        public decimal PC_BalanceLengthBase => UomHelper.QuantityIsLength(PC_QuantityUnit) ? PC_BalanceQuantity : 0;
        public decimal PC_AllocatedLengthBase => UomHelper.QuantityIsLength(PC_QuantityUnit) ? PC_AllocatedQuantity : 0;
        public decimal PC_AvailableLengthBase => PC_OrderedLengthBase - PC_AllocatedLengthBase;
        public string PC_LengthUnit => UomHelper.QuantityIsLength(PC_QuantityUnit) ? PC_QuantityUnit : "";


        public decimal PC_OrderedWeightBase => OrderedWeight ?? 0;
        public decimal PC_OrderedWeightLbs => UomHelper.FactorToPounds(Coid, PC_OrderedWeightBase);
        public decimal PC_OrderedWeightKgs => UomHelper.FactorToKilograms(Coid, PC_OrderedWeightBase);

        public decimal PC_BalanceWeightBase => BalanceWeight ?? 0;
        public decimal PC_BalanceWeightLbs => UomHelper.FactorToPounds(Coid, PC_BalanceWeightBase);
        public decimal PC_BalanceWeightKgs => UomHelper.FactorToKilograms(Coid, PC_BalanceWeightBase);

        public decimal PC_AllocatedWeightBase => AllocatedWeight ?? 0;
        public decimal PC_AllocatedWeightLbs => UomHelper.FactorToPounds(Coid, PC_AllocatedWeightBase);
        public decimal PC_AllocatedWeightKgs => UomHelper.FactorToKilograms(Coid, PC_AllocatedWeightBase);


        public decimal PC_AvailableWeightBase => PC_OrderedWeightBase - PC_AllocatedWeightBase;
        public decimal PC_AvailableWeightKgs => UomHelper.FactorToKilograms(Coid, PC_AvailableWeightBase);
        public decimal PC_AvailableWeightLbs => UomHelper.FactorToPounds(Coid, PC_AvailableWeightBase);


        public string PC_WeightUnit => Product?.ProductCategory?.ProductControl?.UnitsOfMeasure_WeightUnitId?.Code;


        public decimal PC_MaterialCostTotal => PC_AvailableWeightBase * (PurchaseOrderTotal.MaterialValue ?? 0);
        public decimal PC_ProductionCostTotal => PC_AvailableWeightBase * (PurchaseOrderTotal.ProductionValue ?? 0);
        public decimal PC_TransportCostTotal => PC_AvailableWeightBase * (PurchaseOrderTotal.TransportValue ?? 0);
        public decimal PC_SurchargeCostTotal => PC_AvailableWeightBase * (PurchaseOrderTotal.SurchargeValue ?? 0);
        public decimal PC_MiscellaneousCostTotal => PC_AvailableWeightBase * (PurchaseOrderTotal.MiscellaneousValue ?? 0);
        public decimal PC_TotalCost
            =>
                PC_MaterialCostTotal +
                PC_ProductionCostTotal +
                PC_TransportCostTotal +
                PC_SurchargeCostTotal +
                PC_MiscellaneousCostTotal;

        public decimal PC_CostPerLb => PC_AvailableWeightLbs == 0 ? 0 : PC_TotalCost / PC_AvailableWeightLbs;
        public decimal PC_CostPerKg => PC_AvailableWeightKgs == 0 ? 0 : PC_TotalCost / PC_AvailableWeightKgs;

        /// <summary>
        /// This property is not safe to use since PC_AvailableLengthBase might not always be inches.
        /// </summary>
        public decimal PC_CostPerInch => PC_AvailableLengthBase == 0 ? 0 : PC_TotalCost / PC_AvailableLengthBase;



        public void InitializeDimensions(PurchaseOrdersContext context)
        {

            //this.Products.Dim1TypeId

            var productControls = this.Product.ProductCategory.ProductControl;

            var dimTypes = new Dictionary<int, decimal?>();

            if ((productControls.Dim1TypeId ?? 0) > 0)
                dimTypes.Add(productControls.Dim1TypeId ?? 0, DimensionValue.Dim1 ?? 0);
            if ((productControls.Dim2TypeId ?? 0) > 0)
                dimTypes.Add(productControls.Dim2TypeId ?? 0, DimensionValue.Dim2 ?? 0);
            if ((productControls.Dim3TypeId ?? 0) > 0)
                dimTypes.Add(productControls.Dim3TypeId ?? 0, DimensionValue.Dim3 ?? 0);
            if ((productControls.Dim4TypeId ?? 0) > 0)
                dimTypes.Add(productControls.Dim4TypeId ?? 0, DimensionValue.Dim4 ?? 0);
            if ((productControls.Dim5TypeId ?? 0) > 0)
                dimTypes.Add(productControls.Dim5TypeId ?? 0, DimensionValue.Dim5 ?? 0);

            //if (Dim1TypeId != null && Dim1TypeId != 0)
            //    dimTypes.Add(Dim1TypeId ?? 0, Dim1 ?? 0);
            //if (Dim2TypeId != null && Dim2TypeId != 0)
            //    dimTypes.Add(Dim2TypeId ?? 0, Dim2 ?? 0);
            //if (Dim3TypeId != null && Dim3TypeId != 0)
            //    dimTypes.Add(Dim3TypeId ?? 0, Dim3 ?? 0);
            //if (Dim4TypeId != null && Dim4TypeId != 0)
            //    dimTypes.Add(Dim4TypeId ?? 0, Dim4 ?? 0);
            //if (Dim5TypeId != null && Dim5TypeId != 0)
            //    dimTypes.Add(Dim5TypeId ?? 0, Dim5 ?? 0);

            foreach (var dimType in dimTypes)
            {
                //if (dimType.Key == null) continue;

                var stockDimType = context.StockDimensionType.FirstOrDefault(x => x.Id == dimType.Key);
                if (stockDimType == null) continue;

                if (stockDimType.Name == "WIDTH") PC_Dimensions.Width = dimType.Value ?? 0;
                if (stockDimType.Name == "LENGTH") PC_Dimensions.Length = dimType.Value ?? 0;
                if (stockDimType.Name == "THICK") PC_Dimensions.Thick = dimType.Value ?? 0;
                if (stockDimType.Name == "InsideDiameter") PC_Dimensions.InsideDiameter = dimType.Value ?? 0;
                if (stockDimType.Name == "OutsideDiameter") PC_Dimensions.OuterDiameter = dimType.Value ?? 0;
                if (stockDimType.Name == "DENSITY") PC_Dimensions.Density = dimType.Value ?? 0;

            }
        }

        public decimal GetTheoWeight()
        {


            /*
             CASE WHEN CTRL.code = 'ZWS' THEN 0
				 WHEN CTRL.code IN ( 'ME', 'MW' ) THEN CTRL.control_pieces * P.density
				 WHEN CTRL.code IN ( 'FE', 'FW' )
				 THEN CTRL.control_pieces * ISNULL(NULLIF(P.dim1_static_dimension, 0), 1) * ISNULL(P.dim2_static_dimension, 1) * ISNULL(P.dim3_static_dimension,1) * P.density
					  * PC.volume_density * CASE WHEN CTRL.COID IN ( 'SIN', 'MSA', 'DUB' ) THEN 0.45359237
												 ELSE 1
											END
				 WHEN CTRL.code IN ( 'DE', 'DW' )
				 THEN CTRL.control_pieces * ISNULL(NULLIF(P.dim1_static_dimension, 0), 1) * ISNULL(P.dim2_static_dimension, 1) * ISNULL(P.dim2_static_dimension,1) * P.density
					  * PC.volume_density * 0.785398 * CASE	WHEN CTRL.COID IN ( 'SIN', 'MSA', 'DUB' ) THEN 0.45359237
															ELSE 1
													   END
				 WHEN CTRL.code IN ( 'TE', 'TW' )
				 THEN CTRL.control_pieces * ISNULL(NULLIF(P.dim1_static_dimension, 0), 1) * ( ( ISNULL(P.dim2_static_dimension, 1)
																								* ISNULL(P.dim2_static_dimension, 1) )
																							  - ( ISNULL(P.dim3_static_dimension, 1)
																								  * ISNULL(P.dim3_static_dimension, 1) ) ) * P.density
					  * PC.volume_density * 0.785398 * CASE	WHEN CTRL.COID IN ( 'SIN', 'MSA', 'DUB' ) THEN 0.45359237
															ELSE 1
													   END
			END AS TheoreticalWeight        
            */
            decimal result = 0;
            var controlCode = Product.ProductCategory.ProductControl.Code;
            var controlPieces = Product.ProductCategory.ProductControl.ControlPiece == true ? 1 : 0;
            var volumeDensity = Product.ProductCategory.VolumeDensity ?? 1;
            var density = Product.Density ?? 1;
            var dim1StaticDim = Product.Dim1StaticDimension ?? 1;
            var dim2StaticDim = Product.Dim2StaticDimension ?? 1;
            var dim3StaticDim = Product.Dim3StaticDimension ?? 1;
            var coidFactor = new[] { "SIN", "MSA", "DUB", "EUR" }.Contains(Coid) ? (decimal)0.45359237 : (decimal)1;

            if (dim1StaticDim == 0) dim1StaticDim = 1;

            if (controlCode == "ZWS") result = (decimal)0;
            else
            {
                if (new[] { "ME", "MW" }.Contains(controlCode))
                {
                    result = controlPieces * density;
                }
                else if (new[] { "FE", "FW" }.Contains(controlCode))
                {
                    result = controlPieces * dim1StaticDim * dim2StaticDim * dim3StaticDim * density * volumeDensity * coidFactor;
                }
                else if (new[] { "DE", "DW" }.Contains(controlCode))
                {
                    result = controlPieces * dim1StaticDim * dim2StaticDim * dim2StaticDim * density * volumeDensity * (decimal)0.785398 * coidFactor;
                }
                if (new[] { "TE", "TW" }.Contains(controlCode))
                {
                    result = controlPieces *
                             dim1StaticDim *
                             (
                                 dim2StaticDim * dim2StaticDim
                                 - dim3StaticDim * dim3StaticDim
                             )
                             * density * volumeDensity * (decimal)0.785398 * coidFactor;
                }
            }

            //Trace.WriteLine($"CC: {controlCode} ProductId: {stockItem.ProductId} TW: {result}");
            return result;
        }

    }
}
