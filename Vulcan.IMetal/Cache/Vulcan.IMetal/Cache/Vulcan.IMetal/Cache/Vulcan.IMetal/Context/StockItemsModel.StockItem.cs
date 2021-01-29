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

namespace Vulcan.IMetal.Context.StockItems
{

    public partial class StockItem
    {
        public DimensionsModel PC_Dimensions = new DimensionsModel();



        public string Coid { get; set; }
        public string PC_MetalType
        {
            get
            {
                if (Product.ProductCategory.StockAnalysisCode_Analysis1Id == null) return "";

                return Product.ProductCategory.StockAnalysisCode_Analysis1Id.Description;

            }
        }
        public string PC_MetalType2
        {
            get
            {
                if (Product.ProductCategory.StockAnalysisCode_Analysis2Id == null) return "";

                return Product.ProductCategory.StockAnalysisCode_Analysis2Id.Description;
            }
        }
        public string PC_StockType => Product.ProductCategory.ProductControl.Description;
        public string PC_StockRemarks => Note;
        public string PC_StockLocation => Location;
        public string PC_WarehouseName => Warehouse.Name;
        public string PC_WarehouseCode => Warehouse.Code;
        public string PC_ProductCode => Product.Code;
        public string PC_ProductSizeDescription => Product.SizeDescription ?? "";
        public string PC_TagNumber => Number;
        public string PC_HeatNumber
        {
            get
            {
                if (StockCast == null)
                    return "";

                return StockCast.CastNumber ?? "";
            }
        }
        public string PC_ProductGrade
        {
            get
            {
                if (Product.StockGrade_GradeId == null)
                    return "";

                return Product.StockGrade_GradeId.Code ?? "";
            }
        }
        public string PC_ProductCondition => Product.SpecificationValue2 ?? "";
        public string PC_ProductCategory => Product.ProductCategory.Category ?? "";
        public string PC_Mill
        {
            get
            {
                if (StockCast == null)
                    return "";

                return StockCast.Mill.Code ?? "";
            }
        }
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
        public string PC_ProductSize
        {
            get
            {
                var result = "";
                /*
                 CASE 
                    WHEN NULLIF(P.dim3__dimension, 0) IS NULL THEN CAST(NULLIF(CAST(P.dim2__dimension AS FLOAT), 0) AS NVARCHAR(9))
                    ELSE 
                        CASE 
                            WHEN NULLIF(P.dim2__dimension, 0) IS NULL THEN CAST(NULLIF(CAST(P.dim3__dimension AS FLOAT), 0) AS NVARCHAR(9))
                            ELSE CAST(NULLIF(CAST(P.dim2__dimension AS FLOAT), 0) AS NVARCHAR(9)) + N'-'
                                    + CAST(NULLIF(CAST(P.dim3__dimension AS FLOAT), 0) AS NVARCHAR(9))
                          END
                END
                 */
                if ((Product.Dim3StaticDimension ?? 0) == 0)
                {
                    result = Product.Dim2StaticDimension.ToString();
                }
                else
                {
                    if ((Product.Dim2StaticDimension ?? 0) == 0)
                        result = Product.Dim3StaticDimension.ToString();
                    else
                    {
                        result =
                            Product.Dim2StaticDimension + "-" +
                            Product.Dim3StaticDimension;
                    }
                }
                return result;

            }
        }


        public decimal PC_OutsideDiameter => PC_Dimensions.OuterDiameter;
        public decimal PC_InsideDiameter => PC_Dimensions.InsideDiameter;
        public decimal PC_Thick => PC_Dimensions.Thick;
        public decimal PC_Width => PC_Dimensions.Width;
        public decimal PC_Length => PC_Dimensions.Length;
        public decimal PC_Density => PC_Dimensions.Density;
        public string PC_DensityUnit => Product?.ProductCategory?.ProductControl?.DensityLabel;


        public int PC_PhysicalPieces => PhysicalPiece ?? 0;
        public int PC_AllocatedPieces => AllocatedPiece ?? 0;
        public int PC_AvailablePieces => PC_PhysicalPieces - PC_AllocatedPieces;
        public string PC_PiecesUnit => Product?.ProductCategory?.ProductControl?.UnitsOfMeasure_PiecesUnitId?.Code;


        public decimal PC_PhysicalQuantity => PhysicalQuantity ?? 0;
        public decimal PC_AllocatedQuantity => AllocatedQuantity ?? 0;
        public decimal PC_AvailableQuantity => PC_PhysicalQuantity - PC_AllocatedQuantity;
        public string PC_QuantityUnit => Product?.ProductCategory?.ProductControl?.UnitsOfMeasure_QuantityUnitId?.Code;


        public decimal PC_PhysicalLengthBase => UomHelper.QuantityIsLength(PC_QuantityUnit) ? (PhysicalQuantity ?? 0) : 0;
        public decimal PC_AllocatedLengthBase => UomHelper.QuantityIsLength(PC_QuantityUnit) ? (AllocatedQuantity ?? 0) : 0;
        public decimal PC_AvailableLengthBase => PC_PhysicalLengthBase - PC_AllocatedLengthBase;
        public string PC_LengthUnit => UomHelper.QuantityIsLength(PC_QuantityUnit) ? PC_QuantityUnit : "";


        public decimal PC_PhysicalWeightBase => PhysicalWeight ?? 0;
        public decimal PC_PhysicalWeightLbs => UomHelper.FactorToPounds(Coid, PhysicalWeight ?? 0);
        public decimal PC_PhysicalWeightKgs => UomHelper.FactorToKilograms(Coid, PhysicalWeight ?? 0);


        public decimal PC_AllocatedWeightBase => AllocatedWeight ?? 0;
        public decimal PC_AllocatedWeightLbs => UomHelper.FactorToPounds(Coid, PC_AllocatedWeightBase);
        public decimal PC_AllocatedWeightKgs => UomHelper.FactorToKilograms(Coid, PC_AllocatedWeightBase);


        public decimal PC_AvailableWeightBase => (PhysicalWeight ?? 0) - (AllocatedWeight ?? 0);
        public decimal PC_AvailableWeightKgs => UomHelper.FactorToKilograms(Coid, PC_AvailableWeightBase);
        public decimal PC_AvailableWeightLbs => UomHelper.FactorToPounds(Coid, PC_AvailableWeightBase);
        public string PC_WeightUnit => Product?.ProductCategory?.ProductControl?.UnitsOfMeasure_WeightUnitId?.Code;


        public decimal PC_MaterialCostTotal => MaterialValue ?? 0;
        public decimal PC_ProductionCostTotal => ProductionValue ?? 0;
        public decimal PC_TransportCostTotal => TransportValue ?? 0;
        public decimal PC_SurchargeCostTotal => SurchargeValue ?? 0;
        public decimal PC_MiscellaneousCostTotal => MiscellaneousValue ?? 0;
        public decimal PC_TotalCost
            =>
                PC_MaterialCostTotal +
                PC_ProductionCostTotal +
                PC_TransportCostTotal +
                PC_SurchargeCostTotal +
                PC_MiscellaneousCostTotal;

        public decimal PC_CostPerLb => PhysicalWeight == 0 ? 0 : PC_TotalCost / UomHelper.FactorToPounds(Coid, PhysicalWeight ?? 0);

        public decimal PC_CostPerKg => PhysicalWeight == 0 ? 0 : PC_TotalCost / UomHelper.FactorToKilograms(Coid, PhysicalWeight ?? 0);

        /// <summary>
        /// Not safe to use because PhysicalQuantity is not guaranteed to be inches.
        /// </summary>
        public decimal PC_CostPerInch => PhysicalQuantity == 0 ? 0 : PC_TotalCost / (PhysicalQuantity ?? 0);

        public void InitializeDimensions(StockItemsContext context)
        {
            //this.Products.Dim1TypeId

            var productControls = this.Product.ProductCategory.ProductControl;

            var dimTypes = new Dictionary<int, decimal?>();

            if ((productControls.Dim1TypeId ?? 0) > 0)
                dimTypes.Add(productControls.Dim1TypeId ?? 0, Dim1 ?? 0);
            if ((productControls.Dim2TypeId ?? 0) > 0)
                dimTypes.Add(productControls.Dim2TypeId ?? 0, Dim2 ?? 0);
            if ((productControls.Dim3TypeId ?? 0) > 0)
                dimTypes.Add(productControls.Dim3TypeId ?? 0, Dim3 ?? 0);
            if ((productControls.Dim4TypeId ?? 0) > 0)
                dimTypes.Add(productControls.Dim4TypeId ?? 0, Dim4 ?? 0);
            if ((productControls.Dim5TypeId ?? 0) > 0)
                dimTypes.Add(productControls.Dim5TypeId ?? 0, Dim5 ?? 0);

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
                    result = controlPieces * dim1StaticDim * dim2StaticDim * dim2StaticDim * density * volumeDensity *
                             (decimal)0.785398 * coidFactor;
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
