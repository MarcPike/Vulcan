using System;
using System.Linq;
using Vulcan.IMetal.Context;

namespace Vulcan.IMetal.Helpers
{
    //public class SalesItemsModelHelper: BaseHelper
    //{
    //    public  string GetProductGrade(SalesItem salesItem)
    //    {
    //        try
    //        {
    //            return salesItem.Product?.StockGrade_GradeId?.Code;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public  string GetProductCondition(SalesItem salesItem)
    //    {
    //        try
    //        {
    //            return salesItem.Product?.SpecificationValue2;
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //    public  string GetProductSizeDescription(SalesItem salesItem)
    //    {
    //        try
    //        {
    //            return salesItem.Product?.SizeDescription;
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //    public  string GetProductControlDescription(SalesItem salesItem)
    //    {
    //        try
    //        {
    //            return salesItem.Product?.ProductCategory?.ProductControl?.Description;
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //    public string GetProductControlCode(SalesItem salesItem)
    //    {
    //        return salesItem.Product?.ProductCategory?.ProductControl?.Code;
    //    }

    //    public string GetProductCode(SalesItem salesItem)
    //    {
    //        return salesItem.Product.Code;
    //    }

    //    public  string GetProductCategory(SalesItem salesItem)
    //    {
    //        try
    //        {
    //            return salesItem.Product?.ProductCategory?.Category;
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //    public  decimal GetRequiredLength(SalesItem salesItem)
    //    {
            
    //        var productControlCode = salesItem.Product?.ProductCategory?.ProductControl?.Code;
    //        if (new[] {"ZWS", "ZWT", "IE", "WE", "ME", "MW"}.Contains(productControlCode))
    //        {
    //            return 0;
    //        }

    //        try
    //        {
    //            return salesItem.RequiredQuantity ?? 0;

    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //    public decimal GetMaterialCost(SalesItem salesItem)
    //    {
    //        return salesItem.SalesTotal_SalesTotalId.ActualMaterialCost ?? 0;
    //    }

    //    public decimal GetMiscellaneousCost(SalesItem salesItem)
    //    {
    //        return salesItem.SalesTotal_SalesTotalId.ActualMiscellaneousCost ?? 0;
    //    }

    //    public decimal GetProductionCost(SalesItem salesItem)
    //    {
    //        return salesItem.SalesTotal_SalesTotalId.ActualProductionCost ?? 0;
    //    }

    //    public decimal GetSurchargeCost(SalesItem salesItem)
    //    {
    //        return salesItem.SalesTotal_SalesTotalId.ActualSurchargeCost ?? 0;
    //    }

    //    public decimal GetTransportCost(SalesItem salesItem)
    //    {
    //        return salesItem.SalesTotal_SalesTotalId.ActualTransportCost ?? 0;
    //    }

    //    public decimal GetPricePerLengthUnit(SalesItem salesItem)
    //    {
    //        return salesItem.SalesTotal_SalesTotalId.RequiredQuantity ?? 0;
    //    }

    //    public decimal GetPricePerWeightUnit(SalesItem salesItem)
    //    {
    //        return salesItem.SalesTotal_SalesTotalId.RequiredWeight ?? 0;
    //    }

    //    public decimal GetTheoWeight(SalesItem salesItem, string coid)
    //    {
    //        /*
    //         CASE WHEN CTRL.code = 'ZWS' THEN 0
    //             WHEN CTRL.code IN ( 'ME', 'MW' ) THEN CTRL.control_pieces * P.density
    //             WHEN CTRL.code IN ( 'FE', 'FW' )
    //             THEN CTRL.control_pieces * ISNULL(NULLIF(P.dim1_static_dimension, 0), 1) * ISNULL(P.dim2_static_dimension, 1) * ISNULL(P.dim3_static_dimension,1) * P.density
    //                  * PC.volume_density * CASE WHEN CTRL.COID IN ( 'SIN', 'MSA', 'DUB' ) THEN 0.45359237
    //                                             ELSE 1
    //                                        END
    //             WHEN CTRL.code IN ( 'DE', 'DW' )
    //             THEN CTRL.control_pieces * ISNULL(NULLIF(P.dim1_static_dimension, 0), 1) * ISNULL(P.dim2_static_dimension, 1) * ISNULL(P.dim2_static_dimension,1) * P.density
    //                  * PC.volume_density * 0.785398 * CASE	WHEN CTRL.COID IN ( 'SIN', 'MSA', 'DUB' ) THEN 0.45359237
    //                                                        ELSE 1
    //                                                   END
    //             WHEN CTRL.code IN ( 'TE', 'TW' )
    //             THEN CTRL.control_pieces * ISNULL(NULLIF(P.dim1_static_dimension, 0), 1) * ( ( ISNULL(P.dim2_static_dimension, 1)
    //                                                                                            * ISNULL(P.dim2_static_dimension, 1) )
    //                                                                                          - ( ISNULL(P.dim3_static_dimension, 1)
    //                                                                                              * ISNULL(P.dim3_static_dimension, 1) ) ) * P.density
    //                  * PC.volume_density * 0.785398 * CASE	WHEN CTRL.COID IN ( 'SIN', 'MSA', 'DUB' ) THEN 0.45359237
    //                                                        ELSE 1
    //                                                   END
    //        END AS TheoreticalWeight        
    //        */
    //        decimal result = 0;
    //        var controlCode = salesItem.Product.ProductCategory.ProductControl.Code;
    //        var controlPieces = salesItem.Product.ProductCategory.ProductControl.ControlPiece == true ? 1 : 0;
    //        var volumeDensity = salesItem.Product.ProductCategory.VolumeDensity ?? 1;
    //        var density = salesItem.Product.FilterDensity ?? 1;
    //        var dim1StaticDim = salesItem.Product.Dim1StaticDimension ?? 1;
    //        var dim2StaticDim = salesItem.Product.Dim2StaticDimension ?? 1;
    //        var dim3StaticDim = salesItem.Product.Dim3StaticDimension ?? 1;
    //        var coidFactor = (new[] { "SIN", "MSA", "DUB" }.Contains(coid)) ? (decimal)0.45359237 : (decimal)1;

    //        if (dim1StaticDim == 0) dim1StaticDim = 1;

    //        if (controlCode == "ZWS") result = (decimal)0;
    //        else
    //        {
    //            if (new[] { "ME", "MW" }.Contains(controlCode))
    //            {
    //                result = controlPieces * (density);
    //            }
    //            else if (new[] { "FE", "FW" }.Contains(controlCode))
    //            {
    //                result = controlPieces * dim1StaticDim * dim2StaticDim * dim3StaticDim * density * volumeDensity * coidFactor;
    //            }
    //            else if (new[] { "DE", "DW" }.Contains(controlCode))
    //            {
    //                result = controlPieces * dim1StaticDim * dim2StaticDim * dim2StaticDim * density * volumeDensity * (decimal)0.785398 * coidFactor;
    //            }
    //            if (new[] { "TE", "TW" }.Contains(controlCode))
    //            {
    //                result = controlPieces *
    //                    dim1StaticDim *
    //                    (
    //                        (dim2StaticDim * dim2StaticDim)
    //                      - (dim3StaticDim * dim3StaticDim)
    //                    )
    //                    * density * volumeDensity * (decimal)0.785398 * coidFactor;
    //            }
    //        }

    //        //Trace.WriteLine($"CC: {controlCode} ProductId: {stockItem.ProductId} TW: {result}");
    //        return result;

    //    }


    //    public decimal GetStockItemAvgCost(string productCode, VulcanIMetalDataContext context)
    //    {
    //        decimal total = 0;
    //        var count = 0;

    //        var stockItems =  context.StockItem.Where(x => x.Product.Code == productCode).ToList();
    //        foreach (var item in stockItems)
    //        {
    //            var value = item.MiscellaneousValue + item.MaterialValue + item.ProductionValue + item.SurchargeValue +
    //                        item.TransportValue;
    //            if ((value == 0) || (value == null)) continue;

    //            var weight = item.PhysicalWeight ?? 0;

    //            if (weight == 0) continue;


    //            total += (value ?? 0)/weight;
    //            count++;

    //        }

    //        if (count == 0) return 0;

    //        return total/count;
    //    }

    //    public string GetCustomerPartSpecification(SalesItem salesItem)
    //    {
    //        if (salesItem.PartNumberSpecification == null) return "";

    //        return salesItem.PartNumberSpecification.Number;
    //    }

    //    public decimal GetTotalPrice(SalesItem salesItem)
    //    {
    //        var salesTotal = salesItem.SalesTotal_SalesTotalId;
    //        if (salesTotal == null) return 0;

    //        if (salesItem.RequiredWeight == 0) return 0;

    //        var totalPrice = (decimal)0;
    //        totalPrice += salesTotal.BaseCustomerMaterialValue ?? 0 +
    //                      salesTotal.BaseInternalMaterialValue ?? 0;
    //        totalPrice += salesTotal.BaseCustomerProductionValue ?? 0 +
    //                      salesTotal.BaseInternalProductionValue ?? 0;
    //        totalPrice += salesTotal.BaseCustomerMiscellaneousValue ?? 0 +
    //                      salesTotal.BaseInternalMiscellaneousValue ?? 0;
    //        totalPrice += salesTotal.BaseCustomerSurchargeValue ?? 0 +
    //                      salesTotal.BaseInternalSurchargeValue ?? 0;
    //        totalPrice += salesTotal.BaseCustomerTransportValue ?? 0 +
    //                      salesTotal.BaseInternalTransportValue ?? 0;
    //        return totalPrice;

    //    }
    //}
}
