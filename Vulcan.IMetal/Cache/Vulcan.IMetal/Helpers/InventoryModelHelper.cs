using Devart.Data.Linq;
using System;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.StockItems;

namespace Vulcan.IMetal.Helpers
{
    public class InventoryModelHelper: BaseHelper
    {
        public string GetMetalType(StockItem stockItem)
        {
            if (stockItem.Product.ProductCategory.StockAnalysisCode_Analysis1Id == null) return "";

            return stockItem.Product.ProductCategory.StockAnalysisCode_Analysis1Id.Description;
        }

        public string GetMetalType2(StockItem stockItem)
        {
            if (stockItem.Product.ProductCategory.StockAnalysisCode_Analysis2Id == null) return "";

            return stockItem.Product.ProductCategory.StockAnalysisCode_Analysis2Id.Description;
        }

        public decimal GetTheoWeight(StockItem stockItem, StockItemsContext context, string coid)
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
            var controlCode = stockItem.Product.ProductCategory.ProductControl.Code;
            var controlPieces = stockItem.Product.ProductCategory.ProductControl.ControlPiece == true ? 1 : 0;
            var volumeDensity = stockItem.Product.ProductCategory.VolumeDensity ?? 1;
            var density = stockItem.Product.Density ?? 1;
            var dim1StaticDim = stockItem.Product.Dim1StaticDimension ?? 1;
            var dim2StaticDim = stockItem.Product.Dim2StaticDimension ?? 1;
            var dim3StaticDim = stockItem.Product.Dim3StaticDimension ?? 1;
            var coidFactor = (new[] { "SIN", "MSA", "DUB" }.Contains(coid)) ? (decimal)0.45359237 : (decimal)1;

            if (dim1StaticDim == 0) dim1StaticDim = 1;

            if (controlCode == "ZWS") result = (decimal)0;
            else
            {
                if (new[] { "ME", "MW" }.Contains(controlCode))
                {
                    result = controlPieces * (density);
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
                            (dim2StaticDim * dim2StaticDim)
                          - (dim3StaticDim * dim3StaticDim)
                        )
                        * density * volumeDensity * (decimal)0.785398 * coidFactor;
                }
            }

            //Trace.WriteLine($"CC: {controlCode} ProductId: {stockItem.ProductId} TW: {result}");
            return result;
        }

        public string GetStockType(StockItem stockItem)
        {
            return stockItem.Product.ProductCategory.ProductControl.Description;
        }

        public string GetStockRemarks(StockItem stockItem)
        {
            return stockItem.Note;
        }

        public string GetStockLocation(StockItem stockItem)
        {
            return stockItem.Location;
        }

        public string GetWarehouseName(StockItem stockItem)
        {
            return stockItem.Warehouse.Name;
        }

        public string GetWarehouse(StockItem stockItem)
        {
            return stockItem.Warehouse.Code;
        }

        public string GetProductCode(StockItem stockItem)
        {
            try
            {
                return stockItem.Product.Code ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetProductSizeDescription(StockItem stockItem)
        {
            try
            {
                return stockItem.Product.SizeDescription ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  string GetTagNumber(StockItem stockItem)
        {
            try
            {
                return stockItem.Number ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetHeatNumber(StockItem stockItem)
        {
            if (stockItem.StockCast == null)
                return "";

            try
            {
                return stockItem.StockCast.CastNumber ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  decimal GetDensity(StockItem stockItem)
        {
            try
            {
                return stockItem.Density ?? 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  decimal GetGauge(StockItem stockItem)
        {
            try
            {
                return stockItem.Dim3 ?? 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  decimal GetOutsideDiameter(StockItem stockItem)
        {
            try
            {
                return stockItem.Dim5 ?? 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  decimal GetInsideDiameter(StockItem stockItem)
        {
            return stockItem.Dim4 ?? 0;
        }

        public  string GetProductGrade(StockItem stockItem)
        {

            if (stockItem.Product.StockGrade_GradeId == null)
                return "";

            try
            {
                return stockItem.Product.StockGrade_GradeId.Code ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  string GetProductCondition(StockItem stockItem)
        {
            try
            {
                return stockItem.Product.SpecificationValue2 ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  string GetProductCategory(StockItem stockItem)
        {
            try
            {
                return stockItem.Product.ProductCategory.Category ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  string GetMill(StockItem stockItem)
        {
            if (stockItem.StockCast == null)
                return "";

            try
            {
                return stockItem.StockCast.Mill.Code ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  string GetMaterialTypeDescription(StockItem stockItem)
        {
            if (stockItem.Product.ProductCategory.StockAnalysisCode_Analysis2Id == null)
                return "";

            try
            {
                return stockItem.Product.ProductCategory.StockAnalysisCode_Analysis2Id.Description ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  string GetMaterialType(StockItem stockItem)
        {
            if (stockItem.Product.ProductCategory.StockAnalysisCode_Analysis1Id == null)
                return "";

            try
            {
                return stockItem.Product.ProductCategory.StockAnalysisCode_Analysis1Id.Description ?? "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        public  string GetProductSize(StockItem stockItem, DataContext context)
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
            if ((stockItem.Product.Dim3StaticDimension ?? 0) == 0)
            {
                result = stockItem.Product.Dim2StaticDimension.ToString();
            }
            else
            {
                if ((stockItem.Product.Dim2StaticDimension ?? 0) == 0)
                    result = stockItem.Product.Dim3StaticDimension.ToString();
                else
                {
                    result =
                        stockItem.Product.Dim2StaticDimension + "-" +
                        stockItem.Product.Dim3StaticDimension;
                }
            }
            return result;
        }

        //public  string GetPartNumber(StockItem stockItem, StockItemsDataContext context)
        //{
        //    var result = "";
        //    if (stockItem.PartSpecificationId != null)
        //    {
        //        var partNumber =
        //            context.PartNumberSpecification.FirstOrDefault(
        //                x => x.Id == stockItem.PartSpecificationId);
        //        if (partNumber != null)
        //            result = partNumber.Number ?? "";
        //    }
        //    return result;
        //}

    }
}
