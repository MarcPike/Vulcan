using Devart.Data.Linq.Monitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Helpers;

namespace Vulcan.IMetal.Models
{
    //public class SalesItemsModel: BaseModel<SalesItemsModelHelper>
    //{
    //    public string Coid { get; set; }
    //    public LinqMonitor LinqMonitor { get; } = new LinqMonitor();
    //    public int Id { get; set; }
    //    public int SalesHeaderId { get; set; }
    //    public DateTime FilterCreateDate { get; set; } = DateTime.UtcNow;
    //    public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;
    //    public string ProductCategory { get; set; }
    //    public string FilterProductCode { get; set; }
    //    public string ProductControlDescription { get; set; }
    //    public string ProductControlCode { get; set; }
    //    public string ProductSizeDescription { get; set; }
    //    public string FilterProductCondition { get; set; }
    //    public string FilterProductGrade { get; set; }

    //    public string CustomerPartSpecification { get; set; }

    //    public decimal RequiredLength { get; set; } 
    //    public int RequiredPieces { get; set; } 
    //    public decimal RequiredWeight { get; set; }

    //    public decimal MaterialCost { get; set; } = 0;
    //    public decimal MiscellaneousCost { get; set; } = 0;
    //    public decimal ProductionCost { get; set; } = 0;
    //    public decimal SurchargeCost { get; set; } = 0;
    //    public decimal TransportCost { get; set; } = 0;
    //    public decimal TotalCost { get; set; }
    //    public bool CostBasedOnStockItemsAverage { get; set; }
    //    public decimal CostPerLengthUnit => TotalCost/((RequiredLength == 0) ? 1 : RequiredLength);
    //    public decimal CostPerWeightUnit => TotalCost / ((RequiredWeight == 0) ? 1 : RequiredWeight);

    //    public decimal PricePerLengthUnit { get; set; }
    //    public decimal PricePerWeightUnit => (RequiredWeight == 0) ? 0 : TotalPrice/RequiredWeight;

    //    public decimal TotalPrice { get; set; }
    //    public decimal TheoWeight { get; set; }
    //    public decimal Margin => TotalPrice - TotalCost;

    //    public decimal TotalWeight => RequiredLength * TheoWeight;

    //    public static List<SalesItemsModel> Convert(int salesHeaderId, VulcanIMetalDataContext context, string coid)
    //    {
    //        var items = new List<SalesItemsModel>();

    //        var salesItems = context.SalesItem.Where(x => x.SalesHeaderId == salesHeaderId).OrderBy(x => x.Id).ToList();
    //        foreach (var salesItem in salesItems)
    //        {
    //            var newSalesItem = new SalesItemsModel()
    //            {
    //                Coid = coid,
    //                Id = salesItem.Id,
    //                FilterCreateDate = salesItem.Cdate ?? DateTime.UtcNow,
    //                ModifiedDate = salesItem.Mdate,
    //                SalesHeaderId = salesHeaderId,
    //                ProductCategory = Helper.GetProductCategory(salesItem),
    //                FilterProductCode = Helper.GetProductCode(salesItem),
    //                ProductControlDescription = Helper.GetProductControlDescription(salesItem),
    //                ProductControlCode = Helper.GetProductControlCode(salesItem),
    //                ProductSizeDescription = Helper.GetProductSizeDescription(salesItem),
    //                FilterProductCondition = Helper.GetProductCondition(salesItem),
    //                FilterProductGrade = Helper.GetProductGrade(salesItem),

    //                RequiredLength = Helper.GetRequiredLength(salesItem),
    //                RequiredPieces = salesItem.RequiredPiece ?? 0,
    //                RequiredWeight = salesItem.RequiredWeight ?? 0,
    //                MaterialCost = Helper.GetMaterialCost(salesItem),
    //                MiscellaneousCost = Helper.GetMiscellaneousCost(salesItem),
    //                ProductionCost = Helper.GetProductionCost(salesItem),
    //                SurchargeCost = Helper.GetSurchargeCost(salesItem),
    //                TransportCost = Helper.GetTransportCost(salesItem),
    //                TotalPrice = Helper.GetTotalPrice(salesItem),
    //                PricePerLengthUnit = Helper.GetPricePerLengthUnit(salesItem),
    //                TheoWeight = Helper.GetTheoWeight(salesItem, coid),
    //                CustomerPartSpecification = Helper.GetCustomerPartSpecification(salesItem)
    //            };
    //            items.Add(newSalesItem);

    //            newSalesItem.TotalCost = newSalesItem.MaterialCost + 
    //                                     newSalesItem.MiscellaneousCost +
    //                                     newSalesItem.ProductionCost +
    //                                     newSalesItem.SurchargeCost + 
    //                                     newSalesItem.TransportCost;

    //            if (newSalesItem.TotalCost == 0)
    //            {
    //                newSalesItem.TotalCost = Helper.GetStockItemAvgCost(newSalesItem.FilterProductCode, context);
    //                newSalesItem.CostBasedOnStockItemsAverage = true;
    //            }

    //        }

    //        return items;
    //    }

    //}
}