using System;
using System.Collections.Generic;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.StockItems;
using Vulcan.IMetal.Helpers;
using Vulcan.IMetal.Queries.StockItems;

namespace Vulcan.IMetal.Queries.ProductBalances
{
    public class ProductBalancesAdvancedQuery
    {
        public int ProductId { get; set; }
        public int StockItemId { get; set; }
        public string Coid { get; set; }
        public DateTime CreateDate { get; set; }
        public string MetalCategory { get; set; }
        public string MetalType { get; set; }
        public string StockType { get; set; } = string.Empty;
        public string Notes { get; set; }
        public string ProductCode { get; set; }
        public string StockGrade { get; set; } = string.Empty;
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
        public decimal Dim4StaticDimension { get; set; }
        public decimal Dim5StaticDimension { get; set; }

        public BalanceMeasurement Available { get; set; } = new BalanceMeasurement();

        // Physical
        public BalanceMeasurement Physical { get; set; } = new BalanceMeasurement();

        // Averaging
        public BalanceMeasurement Averaging { get; set; } = new BalanceMeasurement();
        // Quarantine
        public BalanceMeasurement Quarantine { get; set; } = new BalanceMeasurement();
        // SalesAllocated
        public BalanceMeasurement SalesAllocated { get; set; } = new BalanceMeasurement();
        // SalesOrder
        public BalanceMeasurement SalesOrder { get; set; } = new BalanceMeasurement();
        // Production Allocated
        public BalanceMeasurement ProductionAllocated { get; set; } = new BalanceMeasurement();
        // Transient
        public BalanceMeasurement Transient { get; set; } = new BalanceMeasurement();
        // TransientAllocated
        public BalanceMeasurement TransientAllocated { get; set; } = new BalanceMeasurement();
        // Incoming
        public BalanceMeasurement Incoming { get; set; } = new BalanceMeasurement();
        // Reserved
        public BalanceMeasurement Reserved { get; set; } = new BalanceMeasurement();
        // SalesReserved
        public BalanceMeasurement SalesReserved { get; set; } = new BalanceMeasurement();
        // Production Due
        public BalanceMeasurement ProductionDue { get; set; } = new BalanceMeasurement();
        // Stock Unavailable
        public BalanceMeasurement StockUnavailable { get; set; } = new BalanceMeasurement();
        // Production Due Allocated
        public BalanceMeasurement ProductionDueAllocated { get; set; } = new BalanceMeasurement();



        public decimal ProductDensity { get; set; }

        /// <summary>
        /// Not safe to use because PhysicalQuantity is not guaranteed to be inches.
        /// </summary>

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

        public static IQueryable<ProductBalancesAdvancedQuery> AsQueryable(string coid, StockItemsContext context = null)
        {
            context = context ?? ContextFactory.GetStockItemsContextForCoid(coid);

            var dimTypes = context.StockDimensionType.ToList();
            var dimTypeIdForWidth = dimTypes.Single(x => x.Name == "WIDTH").Id;
            var dimTypeIdForLength = dimTypes.Single(x => x.Name == "LENGTH").Id;
            var dimTypeIdForThick = dimTypes.Single(x => x.Name == "THICK").Id;
            var dimTypeIdForInsideDiameter = dimTypes.Single(x => x.Name == "InsideDiameter").Id;
            var dimTypeIdForOuterDiameter = dimTypes.Single(x => x.Name == "OutsideDiameter").Id;
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

            var productBalanceQuery = (
                from pb in context.ProductBalance
                where pb.BranchId != null

                //join tp in context.Product on si.ProductId equals tp.Id into tp
                //from p in tp.DefaultIfEmpty()
                join p in context.Product on pb.ProductId equals p.Id

                join pcat in context.ProductCategory on p.CategoryId equals pcat.Id

                join tsg in context.StockGrade on p.GradeId equals tsg.Id into tsg
                from sg in tsg.DefaultIfEmpty()

                join tsac1 in context.StockAnalysisCode on pcat.Analysis1Id equals tsac1.Id into tsac1
                from sac1 in tsac1.DefaultIfEmpty()

                join tsac2 in context.StockAnalysisCode on pcat.Analysis2Id equals tsac2.Id into tsac2
                from sac2 in tsac2.DefaultIfEmpty()

                join pc in context.ProductControl on pcat.ProductControlId equals pc.Id 

                join tuomPieces in context.UnitsOfMeasure on pc.PiecesUnitId equals tuomPieces.Id into tuomPieces
                from uomPieces in tuomPieces.DefaultIfEmpty()

                join tuomQuantity in context.UnitsOfMeasure on pc.QuantityUnitId equals tuomQuantity.Id into tuomQuantity
                from uomQuantity in tuomQuantity.DefaultIfEmpty()

                //join tuomWeight in context.UnitsOfMeasure on pc.WeightUnitId equals tuomWeight.Id into tuomWeight
                //from uomWeight in tuomWeight.DefaultIfEmpty()

                select new ProductBalancesAdvancedQuery()
                {
                    ProductId = p.Id,
                    StockItemId = pb.Id,
                    Coid = coid,
                    CreateDate = pb.Cdate ?? new DateTime(1980, 1, 1),
                    MetalCategory = sac1.Description,
                    MetalType = sac2.Description,
                    StockType = pc.Description,
                    ProductCode = p.Code,
                    StockGrade = sg.Code,
                    ProductCondition = p.SpecificationValue2,
                    ProductCategory = pcat.Category,
                    Width = (
                        ((pc.Dim1TypeId == dimTypeIdForWidth) ? (p.Dim1StaticDimension ?? (decimal) 0) : (decimal) 0)) +
                        ((pc.Dim2TypeId == dimTypeIdForWidth) ? (p.Dim2StaticDimension ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim3TypeId == dimTypeIdForWidth) ? (p.Dim3StaticDimension ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim4TypeId == dimTypeIdForWidth) ? (p.Dim4StaticDimension ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim5TypeId == dimTypeIdForWidth) ? (p.Dim5StaticDimension ?? (decimal) 0) : (decimal) 0),
                    Length = (
                        ((pc.Dim1TypeId == dimTypeIdForLength) ? (p.Dim1StaticDimension ?? (decimal) 0) : (decimal) 0)) +
                        ((pc.Dim2TypeId == dimTypeIdForLength) ? (p.Dim2StaticDimension ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim3TypeId == dimTypeIdForLength) ? (p.Dim3StaticDimension ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim4TypeId == dimTypeIdForLength) ? (p.Dim4StaticDimension ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim5TypeId == dimTypeIdForLength) ? (p.Dim5StaticDimension ?? (decimal) 0) : (decimal) 0),
                    Thick = (
                        ((pc.Dim1TypeId == dimTypeIdForThick) ? (p.Dim1StaticDimension ?? (decimal) 0) : (decimal) 0)) +
                        ((pc.Dim2TypeId == dimTypeIdForThick) ? (p.Dim2StaticDimension ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim3TypeId == dimTypeIdForThick) ? (p.Dim3StaticDimension ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim4TypeId == dimTypeIdForThick) ? (p.Dim4StaticDimension ?? (decimal) 0) : (decimal) 0) +
                        ((pc.Dim5TypeId == dimTypeIdForThick) ? (p.Dim5StaticDimension ?? (decimal) 0) : (decimal) 0),
                    InsideDiameter = (
                         ((pc.Dim1TypeId == dimTypeIdForInsideDiameter) ? (p.Dim1StaticDimension ?? (decimal) 0) : (decimal) 0)) +
                         ((pc.Dim2TypeId == dimTypeIdForInsideDiameter) ? (p.Dim2StaticDimension ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim3TypeId == dimTypeIdForInsideDiameter) ? (p.Dim3StaticDimension ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim4TypeId == dimTypeIdForInsideDiameter) ? (p.Dim4StaticDimension ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim5TypeId == dimTypeIdForInsideDiameter) ? (p.Dim5StaticDimension ?? (decimal) 0) : (decimal) 0),
                    OuterDiameter = (
                         ((pc.Dim1TypeId == dimTypeIdForOuterDiameter) ? (p.Dim1StaticDimension ?? (decimal) 0) : (decimal) 0)) +
                         ((pc.Dim2TypeId == dimTypeIdForOuterDiameter) ? (p.Dim2StaticDimension ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim3TypeId == dimTypeIdForOuterDiameter) ? (p.Dim3StaticDimension ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim4TypeId == dimTypeIdForOuterDiameter) ? (p.Dim4StaticDimension ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim5TypeId == dimTypeIdForOuterDiameter) ? (p.Dim5StaticDimension ?? (decimal) 0) : (decimal) 0),
                    Density = (
                         ((pc.Dim1TypeId == dimTypeIdForDensity) ? (p.Dim1StaticDimension ?? (decimal) 0) : (decimal) 0)) +
                         ((pc.Dim2TypeId == dimTypeIdForDensity) ? (p.Dim2StaticDimension ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim3TypeId == dimTypeIdForDensity) ? (p.Dim3StaticDimension ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim4TypeId == dimTypeIdForDensity) ? (p.Dim4StaticDimension ?? (decimal) 0) : (decimal) 0) +
                         ((pc.Dim5TypeId == dimTypeIdForDensity) ? (p.Dim5StaticDimension ?? (decimal) 0) : (decimal) 0),
                    ProductControlCode = pc.Code,
                    ControlPieces = (pc.ControlPiece == true) ? 1 : 0,
                    VolumeDensity = pcat.VolumeDensity ?? 1,
                    ProductDensity = p.Density ?? 1,
                    Dim1StaticDimension = p.Dim1StaticDimension ?? 1,
                    Dim2StaticDimension = p.Dim2StaticDimension ?? 1,
                    Dim3StaticDimension = p.Dim3StaticDimension ?? 1,
                    Dim4StaticDimension = p.Dim4StaticDimension ?? 1,
                    Dim5StaticDimension = p.Dim5StaticDimension ?? 1,
                    Physical = new BalanceMeasurement(pb.PhysicalPiece, pb.PhysicalWeight, pb.PhysicalQuantity),
                    Averaging = new BalanceMeasurement(pb.AveragingPiece, pb.AveragingWeight, pb.AveragingQuantity),
                    Quarantine = new BalanceMeasurement(pb.QuarantinePiece, pb.QuarantineWeight, pb.QuarantineQuantity),
                    SalesAllocated = new BalanceMeasurement(pb.SalesAllocatedPiece, pb.SalesAllocatedWeight, pb.SalesAllocatedQuantity),
                    SalesOrder = new BalanceMeasurement(pb.SalesOrderPiece, pb.SalesOrderWeight, pb.SalesOrderQuantity),
                    ProductionAllocated = new BalanceMeasurement(pb.ProductionAllocatedPiece, pb.ProductionAllocatedWeight, pb.ProductionAllocatedQuantity),
                    Transient = new BalanceMeasurement(pb.TransientPiece, pb.TransientWeight, pb.TransientQuantity),
                    TransientAllocated = new BalanceMeasurement(pb.TransientAllocPiece, pb.TransientAllocWeight, pb.TransientAllocQuantity),
                    Incoming = new BalanceMeasurement(pb.IncomingPiece, pb.IncomingWeight, pb.IncomingQuantity),
                    Reserved = new BalanceMeasurement(pb.ReservedPiece, pb.ReservedWeight, pb.ReservedQuantity),
                    SalesReserved = new BalanceMeasurement(pb.SalesReservedPiece, pb.SalesReservedWeight, pb.SalesReservedQuantity),
                    ProductionDue = new BalanceMeasurement(pb.ProductionDuePiece, pb.ProductionDueWeight, pb.ProductionDueQuantity),
                    StockUnavailable = new BalanceMeasurement(pb.StockUnavailablePiece, pb.StockUnavailableWeight, pb.StockUnavailableQuantity),
                    ProductionDueAllocated = new BalanceMeasurement(pb.ProductionDueAllocatedPiece, pb.ProductionDueAllocatedWeight, pb.ProductionDueAllocatedQuantity),

                    Available = new BalanceMeasurement(
                        (pb.PhysicalPiece ?? 0) - (pb.SalesAllocatedPiece ?? 0 + pb.ProductionAllocatedPiece ?? 0),
                        (pb.PhysicalWeight ?? 0) - (pb.SalesAllocatedWeight ?? 0 + pb.ProductionAllocatedWeight ?? 0),
                        (pb.PhysicalQuantity ?? 0) - (pb.SalesAllocatedQuantity ?? 0 + pb.ProductionAllocatedQuantity ?? 0))


                }).AsQueryable();

            return productBalanceQuery;

        }

        public decimal FactorForKilograms { get; set; }

        public decimal FactorForLbs { get; set; }

    }
}