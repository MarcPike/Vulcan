using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.StockItems;
using Vulcan.IMetal.Helpers;

namespace Vulcan.IMetal.Queries.StockItems
{

    public class StockItemsAdvancedQuery
    {
        public int ProductId { get; set; }
        public int StockItemId { get; set; }
        public string Coid { get; set; }
        public DateTime CreateDate { get; set; }
        public string TagNumber { get; set; }
        public string HeatNumber { get; set; }
        public string MetalCategory { get; set; }
        public string MetalType { get; set; }
        public string StockType { get; set; }
        public string Notes { get; set; }
        public string Location { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseShortName { get; set; }
        public string ProductCode { get; set; }
        public string StockGrade { get; set; }
        public string ProductCondition { get; set; }
        public string ProductCategory { get; set; }

        public string BaseCurrency
        {
            get
            {
                if (Coid == "EUR")
                {
                    return "GBP";
                }

                if (Coid == "CAN")
                {
                    return "CAD";
                }

                return "USD";
            }
        }

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

        public string MillCode { get; set; }
        public string MillName { get; set; }
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

        public string StockHoldReason { get; set; }
        public string StockHoldUser { get; set; }

        public int PhysicalPieces { get; set; }
        public int AllocatedPieces { get; set; }
        public int AvailablePieces { get; set; }
        public string PiecesUnit { get; set; }


        public decimal PhysicalQuantity { get; set; }
        public decimal AllocatedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; } 
        public string QuantityUnit { get; set; }


        public decimal PhysicalLength { get; set; } 
        public decimal AllocatedLength { get; set;  } 
        public decimal AvailableLength { get; set; } 
        public string LengthUnit { get; set; }  


        public decimal PhysicalWeight { get; set; }
        public decimal PhysicalWeightLbs { get; set; }
        public decimal PhysicalWeightKgs { get; set; } 


        public decimal AllocatedWeight { get; set; }
        public decimal AllocatedWeightLbs { get; set; } 
        public decimal AllocatedWeightKgs { get; set; }  


        public decimal AvailableWeight { get; set; }
        public decimal AvailableWeightLbs { get; set; }
        public decimal AvailableWeightKgs { get; set; }
        public string WeightUnit { get; set; }

        public decimal MaterialCostTotal { get; set; }
        public decimal ProductionCostTotal { get; set; }
        public decimal TransportCostTotal { get; set; }
        public decimal SurchargeCostTotal { get; set; }
        public decimal MiscellaneousCostTotal { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AvailableInches
        {
            get
            {
                try
                {
                    var factorForLbs = UomHelper.GetFactorForPounds(Coid);
                    var theoWeight = TheoWeight;

                    if (theoWeight == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return ((PhysicalWeight - AllocatedWeight) * factorForLbs) / theoWeight;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Found the bug: {e.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Not safe to use because PhysicalQuantity is not guaranteed to be inches.
        /// </summary>
        public decimal CostPerInch
        {
            get
            {
                try
                {
                    if (AvailableInches == 0) return 0;

                    return (decimal)TotalCost / (decimal)AvailableInches;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Found the bug: {e.Message}");
                    throw;
                }

            }

        }

        public decimal CostPerWeight { get; set; }
        public decimal CostPerQty { get; set; }

        public decimal CostPerLb
        {
            get
            {
                if (new[] {"SIN", "MSA", "DUB", "EUR"}.Contains(Coid))
                {
                    return CostPerWeight * (decimal)0.453592;
                }
                else
                {
                    return CostPerWeight;
                }
            }
        } //CostPerWeight * FactorForLbs;

        public decimal CostPerKg
        {
            get
            {
                if (new[] { "SIN", "MSA", "DUB", "EUR" }.Contains(Coid))
                {
                    return CostPerWeight;
                }
                else
                {
                    return CostPerWeight * (decimal)2.20462;
                }

            }
        }//=> CostPerWeight * FactorForKilograms;

        public decimal ProductDensity { get; set; }


        public DateTime ReceivedDate { get; set; }

        public bool IsMachinedPart
        {
            get
            {
                bool result = (ProductControlCode == "ME") || (ProductControlCode == "MW");

                return result;
            }
        }

        public bool IsZeroWeightService
        {
            get
            {
                bool result = (ProductControlCode == "ZWS") || (ProductControlCode == "ZWT");

                return result;

            }
        }

        public decimal PieceCost
        {
            get
            {
                var result = (decimal) 0;
                if (AvailablePieces > 0)
                {
                    result = TotalCost / AvailablePieces;
                }

                return result;
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

        public static (decimal USD, decimal GBP, decimal CAD) GetExchangeRatesFromCoid(string coid)
        {
            var context = ContextFactory.GetStockItemsContextForCoid(coid);

            var gbp = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "GBP")?.ExchangeRate ?? (decimal)1;
            var cad = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "CAD")?.ExchangeRate ?? (decimal)1;
            var usd = context.CurrencyCode.FirstOrDefault(x => x.Symbol == "USD")?.ExchangeRate ?? (decimal)1;

            return (usd, gbp, cad);

        }

        public static IQueryable<StockItemsAdvancedQuery> AsQueryable(string coid, StockItemsContext context = null)
        {
            context = context ?? ContextFactory.GetStockItemsContextForCoid(coid);

            var gbp = context.CurrencyCode.FirstOrDefault(x => x.Code == "GBP")?.ExchangeRate ?? (decimal)1;
            var cad = context.CurrencyCode.FirstOrDefault(x => x.Code == "CAD")?.ExchangeRate ?? (decimal)1;
            var usd = context.CurrencyCode.FirstOrDefault(x => x.Code == "USD")?.ExchangeRate ?? (decimal)1;

            var dimTypes = context.StockDimensionType.ToList();
            var dimTypeIdForWidth = dimTypes.Single(x => x.Name == "WIDTH").Id;
            var dimTypeIdForLength = dimTypes.Single(x => x.Name == "LENGTH").Id;
            var dimTypeIdForThick = dimTypes.Single(x => x.Name == "THICK").Id;
            var dimTypeIdForInsideDiameter = dimTypes.Single(x => x.Name == "ID").Id;
            var dimTypeIdForOuterDiameter = dimTypes.Single(x => x.Name == "OD").Id;
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

            var stockItemQuery = (
                from si in context.StockItem

                join thr in context.StockHoldReason on si.StockHoldReason.Id equals thr.Id into thr
                from hr in thr.DefaultIfEmpty()

                join shu in context.Personnel on si.HoldUserId equals shu.Id into shu
                from hu in shu.DefaultIfEmpty()

                //join tp in context.Product on si.ProductId equals tp.Id into tp
                //from p in tp.DefaultIfEmpty()
                join p in context.Product on si.ProductId equals p.Id

                join pcat in context.ProductCategory on p.CategoryId equals pcat.Id

                join tsc in context.StockCast on si.StockCastId equals tsc.Id into tsc
                from sc in tsc.DefaultIfEmpty()

                join tm in context.Mill on sc.MillId equals tm.Id into tm
                from m in tm.DefaultIfEmpty()

                join tsg in context.StockGrade on p.GradeId equals tsg.Id into tsg
                from sg in tsg.DefaultIfEmpty()

                join tw in context.Warehouse on si.WarehouseId equals tw.Id into tw
                from w in tw.DefaultIfEmpty()

                join tsac1 in context.StockAnalysisCode on pcat.Analysis1Id equals tsac1.Id into tsac1
                from sac1 in tsac1.DefaultIfEmpty()

                join tsac2 in context.StockAnalysisCode on pcat.Analysis2Id equals tsac2.Id into tsac2
                from sac2 in tsac2.DefaultIfEmpty()

                join pc in context.ProductControl on pcat.ProductControlId equals pc.Id 

                join tuomPieces in context.UnitsOfMeasure on pc.PiecesUnitId equals tuomPieces.Id into tuomPieces
                from uomPieces in tuomPieces.DefaultIfEmpty()

                join tuomQuantity in context.UnitsOfMeasure on pc.QuantityUnitId equals tuomQuantity.Id into tuomQuantity
                from uomQuantity in tuomQuantity.DefaultIfEmpty()

                join tuomWeight in context.UnitsOfMeasure on pc.WeightUnitId equals tuomWeight.Id into tuomWeight
                from uomWeight in tuomWeight.DefaultIfEmpty()

                select new StockItemsAdvancedQuery()
                {
                    ProductId = p.Id,
                    StockItemId = si.Id,
                    Coid = coid,
                    CreateDate = si.Cdate ?? new DateTime(1980, 1, 1),
                    ReceivedDate = si.ReceivedDate ?? new DateTime(1980,1,1),
                    MetalCategory = sac1.Description,
                    MetalType = sac2.Description,
                    StockType = pc.Description,
                    StockHoldReason = hr.Description,
                    StockHoldUser = hu.Name,
                    Notes = si.Note,
                    Location = si.Location,
                    WarehouseCode = w.Code,
                    WarehouseName = w.Name,
                    WarehouseShortName = w.ShortName,
                    ProductCode = p.Code,
                    StockGrade = sg.Code,
                    ProductCondition = p.SpecificationValue2,
                    ProductCategory = pcat.Category,
                    MillCode = m.Code,
                    MillName = m.Name,
                    Width = (
                        ((pc.Dim1TypeId == dimTypeIdForWidth) ? (si.Dim1 ?? (decimal) 0) : (decimal) 0)) +
                        ((pc.Dim2TypeId == dimTypeIdForWidth) ? (si.Dim2 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim3TypeId == dimTypeIdForWidth) ? (si.Dim3 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim4TypeId == dimTypeIdForWidth) ? (si.Dim4 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim5TypeId == dimTypeIdForWidth) ? (si.Dim5 ?? (decimal) 0) : (decimal) 0),
                    Length = (
                        ((pc.Dim1TypeId == dimTypeIdForLength) ? (si.Dim1 ?? (decimal) 0) : (decimal) 0)) +
                        ((pc.Dim2TypeId == dimTypeIdForLength) ? (si.Dim2 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim3TypeId == dimTypeIdForLength) ? (si.Dim3 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim4TypeId == dimTypeIdForLength) ? (si.Dim4 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim5TypeId == dimTypeIdForLength) ? (si.Dim5 ?? (decimal) 0) : (decimal) 0),
                    Thick = (
                        ((pc.Dim1TypeId == dimTypeIdForThick) ? (si.Dim1 ?? (decimal) 0) : (decimal) 0)) +
                        ((pc.Dim2TypeId == dimTypeIdForThick) ? (si.Dim2 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim3TypeId == dimTypeIdForThick) ? (si.Dim3 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim4TypeId == dimTypeIdForThick) ? (si.Dim4 ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim5TypeId == dimTypeIdForThick) ? (si.Dim5 ?? (decimal) 0) : (decimal) 0),
                    InsideDiameter = (
                         ((pc.Dim1TypeId == dimTypeIdForInsideDiameter) ? (si.Dim1 ?? (decimal) 0) : (decimal) 0)) +
                         ((pc.Dim2TypeId == dimTypeIdForInsideDiameter) ? (si.Dim2 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim3TypeId == dimTypeIdForInsideDiameter) ? (si.Dim3 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim4TypeId == dimTypeIdForInsideDiameter) ? (si.Dim4 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim5TypeId == dimTypeIdForInsideDiameter) ? (si.Dim5 ?? (decimal) 0) : (decimal) 0),
                    OuterDiameter = (
                         ((pc.Dim1TypeId == dimTypeIdForOuterDiameter) ? (si.Dim1 ?? (decimal) 0) : (decimal) 0)) +
                         ((pc.Dim2TypeId == dimTypeIdForOuterDiameter) ? (si.Dim2 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim3TypeId == dimTypeIdForOuterDiameter) ? (si.Dim3 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim4TypeId == dimTypeIdForOuterDiameter) ? (si.Dim4 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim5TypeId == dimTypeIdForOuterDiameter) ? (si.Dim5 ?? (decimal) 0) : (decimal) 0),
                    Density = (
                         ((pc.Dim1TypeId == dimTypeIdForDensity) ? (si.Dim1 ?? (decimal) 0) : (decimal) 0)) +
                         ((pc.Dim2TypeId == dimTypeIdForDensity) ? (si.Dim2 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim3TypeId == dimTypeIdForDensity) ? (si.Dim3 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim4TypeId == dimTypeIdForDensity) ? (si.Dim4 ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim5TypeId == dimTypeIdForDensity) ? (si.Dim5 ?? (decimal) 0) : (decimal) 0),
                    ProductControlCode = pc.Code,
                    ControlPieces = (pc.ControlPiece == true) ? 1 : 0,
                    VolumeDensity = pcat.VolumeDensity ?? 1,
                    ProductDensity = p.Density ?? 1,
                    Dim1StaticDimension = p.Dim1StaticDimension ?? 1,
                    Dim2StaticDimension = p.Dim2StaticDimension ?? 1,
                    Dim3StaticDimension = p.Dim3StaticDimension ?? 1,

                    PhysicalPieces = si.PhysicalPiece ?? 0,
                    AllocatedPieces = si.AllocatedPiece ?? 0,
                    PiecesUnit = uomPieces.Code,
                    AvailablePieces = si.PhysicalPiece ?? 0 - si.AllocatedPiece ?? 0,

                    PhysicalLength = iMetalLengthUoMs.Contains(uomQuantity.Code) ? (si.PhysicalQuantity ?? 0) : 0,
                    AllocatedLength = iMetalLengthUoMs.Contains(uomQuantity.Code) ? (si.AllocatedQuantity ?? 0) : 0,
                    AvailableLength = (iMetalLengthUoMs.Contains(uomQuantity.Code) ? (si.PhysicalQuantity ?? 0) : 0) -
                                      (iMetalLengthUoMs.Contains(uomQuantity.Code) ? (si.AllocatedQuantity ?? 0) : 0),
                    LengthUnit = iMetalLengthUoMs.Contains(uomQuantity.Code) ? uomQuantity.Code : "",

                    PhysicalQuantity = si.PhysicalQuantity ?? 0,
                    AllocatedQuantity = si.AllocatedQuantity ?? 0,
                    AvailableQuantity = si.PhysicalQuantity ?? 0 - si.AllocatedQuantity ?? 0,
                    QuantityUnit = uomQuantity.Code,

                    PhysicalWeight = si.PhysicalWeight ?? 0,
                    AllocatedWeight = si.AllocatedWeight ?? 0,
                    PhysicalWeightLbs = factorForLbs * si.PhysicalWeight ?? 0,
                    PhysicalWeightKgs = factorForKilos * si.PhysicalWeight ?? 0,
                    AllocatedWeightLbs = factorForLbs * si.AllocatedWeight ?? 0,
                    AllocatedWeightKgs= factorForKilos * si.AllocatedWeight ?? 0,
                    AvailableWeight = (si.PhysicalWeight ?? 0) - (si.AllocatedWeight ?? 0),
                    AvailableWeightLbs = (factorForLbs * si.PhysicalWeight ?? 0) - (factorForLbs * si.AllocatedWeight ?? 0),
                    AvailableWeightKgs = (factorForKilos * si.PhysicalWeight ?? 0) - (factorForKilos * si.AllocatedWeight ?? 0),
                    WeightUnit = uomWeight.Code,

                    MaterialCostTotal = si.MaterialValue ?? 0,
                    ProductionCostTotal = si.ProductionValue ?? 0,
                    TransportCostTotal = si.TransportValue ?? 0,
                    SurchargeCostTotal = si.SurchargeValue ?? 0,
                    MiscellaneousCostTotal = si.MiscellaneousValue ?? 0,
                    TotalCost = (si.MaterialValue ?? 0) + (si.ProductionValue ?? 0) + (si.TransportValue ?? 0) + (si.SurchargeValue ?? 0) + (si.MiscellaneousValue ?? 0),
                    TagNumber = si.Number,
                    HeatNumber = sc.CastNumber,
                    FactorForLbs = factorForLbs,
                    FactorForKilograms = factorForKilos,
                    CostPerWeight = (si.PhysicalWeight == 0) ? 0 :
                        (
                            (((si.MaterialValue ?? 0) + (si.ProductionValue ?? 0) + (si.TransportValue ?? 0) + (si.SurchargeValue ?? 0) + (si.MiscellaneousValue ?? 0))
                             / (si.PhysicalWeight ?? 0))),
                    CostPerQty = (si.PhysicalQuantity == 0) ? 0 : 
                        ((si.MaterialValue ?? 0) + (si.ProductionValue ?? 0) + (si.TransportValue ?? 0) + (si.SurchargeValue ?? 0) + (si.MiscellaneousValue ?? 0)) 
                         / (si.PhysicalQuantity ?? 0),
                    ExchangeRateCAD = cad,
                    ExchangeRateGBP = gbp,
                    ExchangeRateUSD = usd

                }).AsQueryable();

            return stockItemQuery;

        }

        //private static string CalculateProductSize(StockItem si)
        //{
        //    var od = (si.Dim3 ?? si.Dim2).ToString();
        //    var id = si.Dim3?.ToString();

        //    if ((si.Dim3 == null) || (si.Dim2 == null))
        //    {
        //        return od;
        //    }
        //    else
        //    {
        //        return od + "-" + id;
        //    }

        //}

        public decimal FactorForKilograms { get; set; }

        public decimal FactorForLbs { get; set; }

        public decimal ExchangeRateUSD { get; set; } = 1;
        public decimal ExchangeRateCAD { get; set; } = 1;
        public decimal ExchangeRateGBP { get; set; } = 1;
    }

}