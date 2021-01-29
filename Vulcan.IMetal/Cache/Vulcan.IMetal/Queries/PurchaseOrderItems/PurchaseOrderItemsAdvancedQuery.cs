using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.Helpers;

namespace Vulcan.IMetal.Queries.PurchaseOrderItems
{
    public class PurchaseOrderItemUnitValues
    {
        public PurchaseOrderItemUnit Ordered { get; set; } = new PurchaseOrderItemUnit();
        public PurchaseOrderItemUnit Allocated { get; set; } = new PurchaseOrderItemUnit();
        public PurchaseOrderItemUnit Delivered { get; set; } = new PurchaseOrderItemUnit();
        public PurchaseOrderItemUnit Balance { get; set; } = new PurchaseOrderItemUnit();
    }
    public class PurchaseOrderItemsAdvancedQuery
    {
        public string Coid { get; set; }
        public string Status { get; set; }

        public int PurchaseOrderHeaderId { get; set; }
        public int? PoNumber { get; set; }

        public int? ItemNumber { get; set; }

        public int PurchaseOrderItemId { get; set; }
        public DateTime CreateDate { get; set; }
        public string MetalCategory { get; set; }
        public string MetalType { get; set; }
        public string StockType { get; set; }
        public string Location { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string StockGrade { get; set; }
        public string ProductCondition { get; set; }
        public string ProductCategory { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Thick { get; set; }

        public decimal InsideDiameter { get; set; }
        public decimal OuterDiameter { get; set; }
        public decimal Density { get; set; }
        public string ProductControlCode { get; set; }
        public int ControlPieces { get; set; }
        public decimal VolumeDensity { get; set; }
        public decimal Dim1StaticDimension { get; set; }
        public decimal Dim2StaticDimension { get; set; }
        public decimal Dim3StaticDimension { get; set; }

        public PurchaseOrderItemUnitValues UnitValues { get; set; } = new PurchaseOrderItemUnitValues();
        
        public decimal MaterialCostTotal { get; set; }
        public decimal ProductionCostTotal { get; set; }
        public decimal TransportCostTotal { get; set; }
        public decimal SurchargeCostTotal { get; set; }
        public decimal MiscellaneousCostTotal { get; set; }
        public decimal TotalCost { get; set; }

        public decimal CostPerLb { get; set; } 

        public decimal CostPerKg { get; set; }

        public decimal ProductDensity { get; set; }

        /// <summary>
        /// Not safe to use because PhysicalQuantity is not guaranteed to be inches.
        /// </summary>
        public decimal CostPerInch { get; set; } 

        public string PurchaseType { get; set; }
        public string TransferType { get; set; }
        public string RequestType { get; set; }
        public string PurchaseCategoryCode { get; set; }
        public string PurchaseCategoryDescription { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseShortName { get; set; }
        public string WarehouseName { get; set; }

        public int? InternalStatusId { get; set; }

        public string StatusDescription { get; set; }

        public string StatusCode { get; set; }

        public string Supplier { get; set; }
        public string Buyer { get; set; }

        public string StratificationRank { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ItemDueDate { get; set; }
        public decimal FactorForKilograms { get; set; }
        public decimal FactorForLbs { get; set; }
        private string Normalize(decimal value)
        {
            return (value / 1.000000000000000000000000000000000m).ToString(CultureInfo.InvariantCulture);
        }

        public string ProductSize
        {
            get
            {
                if (InsideDiameter > 0)
                    return Normalize(OuterDiameter) + "-" + Normalize(InsideDiameter);
                else if (OuterDiameter > 0)
                {
                    return Normalize(OuterDiameter);
                }
                else
                {
                    var productCodeTokens = ProductCode.Split(' ');
                    if (productCodeTokens.Count() > 2)
                    {
                        return productCodeTokens[1];
                    }
                    else
                    {
                        return "<Unknown>";
                    }
                }

            }
        }

        public decimal TheoWeight
        {
            get
            {
                decimal result = 0;
                var controlCode = ProductControlCode;
                var coidFactor = new[] {"SIN", "MSA", "DUB", "EUR"}.Contains(Coid) ? (decimal) 0.45359237 : (decimal) 1;

                if (Dim1StaticDimension == 0) Dim1StaticDimension = 1;

                if (ProductControlCode == "ZWS") result = (decimal) 0;
                else
                {
                    if (new[] {"ME", "MW"}.Contains(controlCode))
                    {
                        result = ControlPieces * Density;
                    }
                    else if (new[] {"FE", "FW"}.Contains(controlCode))
                    {
                        result = ControlPieces * Dim1StaticDimension * Dim2StaticDimension * Dim3StaticDimension *
                                 ProductDensity * VolumeDensity * coidFactor;
                    }
                    else if (new[] {"DE", "DW"}.Contains(controlCode))
                    {
                        result = ControlPieces * Dim1StaticDimension * Dim2StaticDimension * Dim2StaticDimension *
                                 ProductDensity * VolumeDensity *
                                 (decimal) 0.785398 * coidFactor;
                    }
                    if (new[] {"TE", "TW"}.Contains(controlCode))
                    {
                        result = ControlPieces *
                                 Dim1StaticDimension *
                                 (
                                     Dim2StaticDimension * Dim2StaticDimension
                                     - Dim3StaticDimension * Dim3StaticDimension
                                 )
                                 * ProductDensity * VolumeDensity * (decimal) 0.785398 * coidFactor;
                    }
                }

                //Trace.WriteLine($"CC: {controlCode} ProductId: {stockItem.ProductId} TW: {result}");
                return result;
            }
        }

        public string ProductType
        {
            get
            {
                if (StockType.ToUpper().Contains("BAR"))
                {
                    return "Bar";
                }
                return StockType.ToUpper().Contains("TUBE") ? "Tube" : "Other";
            }
        }

        public static IQueryable<PurchaseOrderItemsAdvancedQuery> AsQueryable(string coid, PurchaseOrdersContext context = null)
        {
            context ??= ContextFactory.GetPurchaseOrdersContextForCoid(coid);
            
            var dimTypes = context.StockDimensionType.ToList();
            var dimTypeIdForWidth = dimTypes.Single(x => x.Name == "WIDTH").Id;
            var dimTypeIdForLength = dimTypes.Single(x => x.Name == "LENGTH").Id;
            var dimTypeIdForThick = dimTypes.Single(x => x.Name == "THICK").Id;
            var dimTypeIdForId = dimTypes.Single(x => x.Name == "ID").Id;
            var dimTypeIdForOd = dimTypes.Single(x => x.Name == "OD").Id;
            var dimTypeIdForDensity = dimTypes.Single(x => x.Name == "DENSITY").Id;
            var factorForLbs = UomHelper.GetFactorForPounds(coid);
            var factorForKilos = UomHelper.GetFactorForKilograms(coid);

            var iMetalLengthUoMs = new List<string>
            {
                "FT",
                "IN",
                "cm",
                "m",
                "mm",
            };

            var purchaseOrderQuery = (
                from poi in context.PurchaseOrderItem
                join p in context.Product on poi.ProductId equals p.Id
                join pcat in context.ProductCategory on p.CategoryId equals pcat.Id
                join dim in context.DimensionValue on poi.DimensionsId equals dim.Id
                join sg in context.StockGrade on p.GradeId equals sg.Id
                join sac1 in context.StockAnalysisCode on pcat.Analysis1Id equals sac1.Id
                join sac2 in context.StockAnalysisCode on pcat.Analysis2Id equals sac2.Id
                join pc in context.ProductControl on pcat.ProductControlId equals pc.Id
                join uomPieces in context.UnitsOfMeasure on pc.PiecesUnitId equals uomPieces.Id
                join uomQuantity in context.UnitsOfMeasure on pc.QuantityUnitId equals uomQuantity.Id
                join uomWeight in context.UnitsOfMeasure on pc.WeightUnitId equals uomWeight.Id
                join pot in context.PurchaseOrderTotal on poi.PurchaseOrderTotalsId equals pot.Id
                join poh in context.PurchaseOrderHeader on poi.PurchaseHeaderId equals poh.Id
                join wh in context.Warehouse on poh.DeliveryWarehouseId equals wh.Id
                join pos in context.PurchaseStatusCode on poi.StatusId equals pos.Id
                join c in context.Company on poh.SupplierId equals c.Id
                join b in context.Buyer on poh.BuyerId equals b.Id
                select new PurchaseOrderItemsAdvancedQuery()
                {
                    Coid = coid,
                    StatusCode = pos.Code,
                    StatusDescription = pos.Description,
                    InternalStatusId = pos.InternalStatusId,
                    PurchaseOrderItemId = poi.Id,
                    CreateDate = poi.Cdate ?? new DateTime(1980, 1, 1),
                    MetalCategory = sac1.Description,
                    MetalType = sac2.Description,
                    StockType = pc.Description,
                    ProductId = p.Id,
                    ProductCode = p.Code,
                    StockGrade = sg.Code,
                    Supplier = c.Name,
                    Buyer = b.Name,
                    ProductCondition = p.SpecificationValue2,
                    ProductCategory = pcat.Category,
                    PurchaseOrderHeaderId = poi.PurchaseHeaderId ?? 0,
                    PurchaseType =  poh.PurchaseType,
                    TransferType = poh.TransferType,
                    RequestType = poh.RequestType,
                    PurchaseCategoryCode = poh.PurchaseCategoryCode.Code,
                    PurchaseCategoryDescription = poh.PurchaseCategoryCode.Description,
                    WarehouseCode = wh.Code,
                    WarehouseName = wh.Name,
                    WarehouseShortName = wh.ShortName,
                    ItemNumber = poi.ItemNumber,
                    PoNumber = poh.Number,
                    DueDate = poh.DueDate,
                    ItemDueDate = poi.DueDate,
                    StratificationRank = p.SpecificationValue9,
                    Width = (
                        ((pc.Dim1TypeId == dimTypeIdForWidth) ? (dim.Dim1 ?? (decimal) 0) : (decimal) 0)) +
                        ((pc.Dim2TypeId == dimTypeIdForWidth) ? (dim.Dim2 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim3TypeId == dimTypeIdForWidth) ? (dim.Dim3 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim4TypeId == dimTypeIdForWidth) ? (dim.Dim4 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim5TypeId == dimTypeIdForWidth) ? (dim.Dim5 ?? (decimal) 0) : (decimal) 0),
                    Length = (
                        ((pc.Dim1TypeId == dimTypeIdForLength) ? (dim.Dim1 ?? (decimal) 0) : (decimal) 0)) +
                        ((pc.Dim2TypeId == dimTypeIdForLength) ? (dim.Dim2 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim3TypeId == dimTypeIdForLength) ? (dim.Dim3 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim4TypeId == dimTypeIdForLength) ? (dim.Dim4 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim5TypeId == dimTypeIdForLength) ? (dim.Dim5 ?? (decimal) 0) : (decimal) 0),
                    Thick = (
                        ((pc.Dim1TypeId == dimTypeIdForThick) ? (dim.Dim1 ?? (decimal) 0) : (decimal) 0)) +
                        ((pc.Dim2TypeId == dimTypeIdForThick) ? (dim.Dim2 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim3TypeId == dimTypeIdForThick) ? (dim.Dim3 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim4TypeId == dimTypeIdForThick) ? (dim.Dim4 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim5TypeId == dimTypeIdForThick) ? (dim.Dim5 ?? (decimal) 0) : (decimal) 0),
                    InsideDiameter = (
                         ((pc.Dim1TypeId == dimTypeIdForId) ? (dim.Dim1 ?? (decimal) 0) : (decimal) 0)) +
                         ((pc.Dim2TypeId == dimTypeIdForId) ? (dim.Dim2 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim3TypeId == dimTypeIdForId) ? (dim.Dim3 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim4TypeId == dimTypeIdForId) ? (dim.Dim4 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim5TypeId == dimTypeIdForId) ? (dim.Dim5 ?? (decimal) 0) : (decimal) 0),
                    OuterDiameter = (
                         ((pc.Dim1TypeId == dimTypeIdForOd) ? (dim.Dim1 ?? (decimal) 0) : (decimal) 0)) +
                         ((pc.Dim2TypeId == dimTypeIdForOd) ? (dim.Dim2 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim3TypeId == dimTypeIdForOd) ? (dim.Dim3 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim4TypeId == dimTypeIdForOd) ? (dim.Dim4 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim5TypeId == dimTypeIdForOd) ? (dim.Dim5 ?? (decimal) 0) : (decimal) 0),
                    Density = p.Density ?? 0,
                    ProductControlCode = pc.Code,
                    ControlPieces = (pc.ControlPiece == true) ? 1 : 0,
                    VolumeDensity = pcat.VolumeDensity ?? 1,
                    ProductDensity = p.Density ?? 1,
                    Dim1StaticDimension = p.Dim1StaticDimension ?? 1,
                    Dim2StaticDimension = p.Dim2StaticDimension ?? 1,
                    Dim3StaticDimension = p.Dim3StaticDimension ?? 1,

                    UnitValues = new PurchaseOrderItemUnitValues()
                    {
                        Allocated = new PurchaseOrderItemUnit()
                        {
                            Pieces = poi.AllocatedPiece ?? 0,
                            PiecesUnit = uomPieces.Code,
                            Length = iMetalLengthUoMs.Contains(uomQuantity.Code) ? (poi.AllocatedQuantity ?? 0) : 0,
                            LengthUnit = iMetalLengthUoMs.Contains(uomQuantity.Code) ? uomQuantity.Code : "",
                            Quantity = poi.AllocatedQuantity ?? 0,
                            QuantityUnit = uomQuantity.Code,
                            Weight = poi.AllocatedWeight ?? 0,
                            WeightLbs = factorForLbs * poi.AllocatedWeight ?? 0,
                            WeightKgs = factorForKilos * poi.AllocatedWeight ?? 0,
                            WeightUnit = uomWeight.Code,
                        },
                        Balance = new PurchaseOrderItemUnit()
                        {
                            Pieces = poi.BalancePiece ?? 0,
                            PiecesUnit = uomPieces.Code,
                            Length = iMetalLengthUoMs.Contains(uomQuantity.Code) ? (poi.BalanceQuantity ?? 0) : 0,
                            LengthUnit = iMetalLengthUoMs.Contains(uomQuantity.Code) ? uomQuantity.Code : "",
                            Quantity = poi.BalanceQuantity ?? 0,
                            QuantityUnit = uomQuantity.Code,
                            Weight = poi.BalanceWeight ?? 0,
                            WeightLbs = factorForLbs * poi.BalanceWeight ?? 0,
                            WeightKgs = factorForKilos * poi.BalanceWeight ?? 0,
                            WeightUnit = uomWeight.Code,
                        },
                        Ordered =  new PurchaseOrderItemUnit()
                        {
                            Pieces = poi.OrderedPiece ?? 0,
                            PiecesUnit = uomPieces.Code,
                            Length = iMetalLengthUoMs.Contains(uomQuantity.Code) ? (poi.OrderedQuantity ?? 0) : 0,
                            LengthUnit = iMetalLengthUoMs.Contains(uomQuantity.Code) ? uomQuantity.Code : "",
                            Quantity = poi.OrderedQuantity ?? 0,
                            QuantityUnit = uomQuantity.Code,
                            Weight = poi.OrderedWeight ?? 0,
                            WeightLbs = factorForLbs * poi.OrderedWeight ?? 0,
                            WeightKgs = factorForKilos * poi.OrderedWeight ?? 0,
                            WeightUnit = uomWeight.Code,
                        },
                        Delivered  = new PurchaseOrderItemUnit()
                        {
                            Pieces = poi.DeliveredPiece ?? 0,
                            PiecesUnit = uomPieces.Code,
                            Length = iMetalLengthUoMs.Contains(uomQuantity.Code) ? (poi.DeliveredQuantity ?? 0) : 0,
                            LengthUnit = iMetalLengthUoMs.Contains(uomQuantity.Code) ? uomQuantity.Code : "",
                            Quantity = poi.DeliveredQuantity ?? 0,
                            QuantityUnit = uomQuantity.Code,
                            Weight = poi.DeliveredWeight ?? 0,
                            WeightLbs = factorForLbs * poi.DeliveredWeight ?? 0,
                            WeightKgs = factorForKilos * poi.DeliveredWeight ?? 0,
                            WeightUnit = uomWeight.Code,
                        }
                    },

                    MaterialCostTotal = pot.BaseMaterialValue ?? 0,
                    ProductionCostTotal = pot.BaseProductionValue ?? 0,
                    TransportCostTotal = pot.BaseTransportValue ?? 0,
                    SurchargeCostTotal = pot.BaseSurchargeValue ?? 0,
                    MiscellaneousCostTotal = pot.BaseMiscellaneousValue ?? 0,
                    TotalCost = (pot.BaseMaterialValue ?? 0) + (pot.BaseProductionValue ?? 0) + (pot.BaseTransportValue ?? 0) + (pot.BaseSurchargeValue ?? 0) + (pot.BaseMiscellaneousValue ?? 0),
                    FactorForLbs = factorForLbs,
                    FactorForKilograms = factorForKilos,
                    CostPerLb = (poi.BalanceWeight == 0) ? 0 : 
                    (
                        ((pot.BaseMaterialValue ?? 0) + (pot.BaseProductionValue ?? 0) + (pot.BaseTransportValue ?? 0) + (pot.BaseSurchargeValue ?? 0) + (pot.BaseMiscellaneousValue ?? 0)) 
                        / (poi.BalanceWeight ?? 0 ) / factorForLbs
                    ),
                    CostPerKg = (poi.BalanceWeight == 0) ? 0 :
                        (
                            ((pot.BaseMaterialValue ?? 0) + (pot.BaseProductionValue ?? 0) + (pot.BaseTransportValue ?? 0) + (pot.BaseSurchargeValue ?? 0) + (pot.BaseMiscellaneousValue ?? 0))
                            / (poi.BalanceWeight ?? 0 ) / factorForKilos
                        ),
                    CostPerInch = (poi.BalanceQuantity == 0) ? 0 : 
                        ((pot.BaseMaterialValue ?? 0) + (pot.BaseProductionValue ?? 0) + (pot.BaseTransportValue ?? 0) + (pot.BaseSurchargeValue ?? 0) + (pot.BaseMiscellaneousValue ?? 0)) 
                        / (poi.BalanceQuantity ?? 0)

            }).AsQueryable();


            return purchaseOrderQuery;

        }

    }
}