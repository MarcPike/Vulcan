using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.StockItems;
using Vulcan.IMetal.Helpers;

namespace Vulcan.IMetal.Queries.ProductCodes
{

    public class ProductMasterAdvancedQuery
    {
        public int ProductId { get; set; }
        public string Coid { get; set; }
        public string ProductCode { get; set; }
        public string ProductCondition { get; set; }
        public string ProductCategory { get; set; }
        public string MetalCategory { get; set; }
        public string MetalType { get; set; }
        public string StockGrade { get; set; }
        public decimal InsideDiameter { get; set; }
        public decimal OuterDiameter { get; set; }
        public string StockType { get; set; }
        public decimal Density { get; set; }
        public string ProductControlCode { get; set; }
        public int ControlPieces { get; set; }
        public decimal VolumeDensity { get; set; }
        public decimal Dim1StaticDimension { get; set; }
        public decimal Dim2StaticDimension { get; set; }
        public decimal Dim3StaticDimension { get; set; }
        public decimal ProductDensity { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Thick { get; set; }

        public string SizeDescription { get; set; }
        public string Description { get; set; }

        public decimal FactorForKilograms { get; set; }
        public decimal FactorForLbs { get; set; }

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
                var coidFactor = new[] { "SIN", "MSA", "DUB", "EUR" }.Contains(Coid) ? (decimal)0.45359237 : (decimal)1;

                if (Dim1StaticDimension == 0) Dim1StaticDimension = 1;

                if (ProductControlCode == "ZWS") result = (decimal)0;
                else
                {
                    if (new[] { "ME", "MW" }.Contains(controlCode))
                    {
                        result = ControlPieces * Density;
                    }
                    else if (new[] { "FE", "FW" }.Contains(controlCode))
                    {
                        result = ControlPieces * Dim1StaticDimension * Dim2StaticDimension * Dim3StaticDimension *
                                 ProductDensity * VolumeDensity * coidFactor;
                    }
                    else if (new[] { "DE", "DW" }.Contains(controlCode))
                    {
                        result = ControlPieces * Dim1StaticDimension * Dim2StaticDimension * Dim2StaticDimension *
                                 ProductDensity * VolumeDensity *
                                 (decimal)0.785398 * coidFactor;
                    }
                    if (new[] { "TE", "TW" }.Contains(controlCode))
                    {
                        result = ControlPieces *
                                 Dim1StaticDimension *
                                 (
                                     (Dim2StaticDimension * Dim2StaticDimension)
                                     - (Dim3StaticDimension * Dim3StaticDimension)
                                 )
                                 * ProductDensity * VolumeDensity * (decimal)0.785398 * coidFactor;
                    }
                }

                //Trace.WriteLine($"CC: {controlCode} ProductId: {stockItem.ProductId} TW: {result}");
                return result;
            }
        }




        //public string StockGrade { get; set; }

        public static Dictionary<int, string> GetProductCodesForCoid(string coid)
        {
            var context = ContextFactory.GetStockItemsContextForCoid(coid);
            return context.Product.Select(x => new {x.Id, x.Code}).ToDictionary(t => t.Id, t => t.Code);
        }

        public static IQueryable<ProductMasterAdvancedQuery> AsQueryable(string coid, StockItemsContext context = null)
        {
            context = context ?? ContextFactory.GetStockItemsContextForCoid(coid);
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


            var stockItemQuery = (
                from p in context.Product 
                join pcat in context.ProductCategory on p.CategoryId equals pcat.Id

                //join sg in context.StockGrade on p.GradeId equals sg.Id
                join tsg in context.StockGrade on p.GradeId equals tsg.Id into tsg
                from sg in tsg.DefaultIfEmpty()

                join sac1 in context.StockAnalysisCode on pcat.Analysis1Id equals sac1.Id
                join sac2 in context.StockAnalysisCode on pcat.Analysis2Id equals sac2.Id
                join pc in context.ProductControl on pcat.ProductControlId equals pc.Id
                join uomPieces in context.UnitsOfMeasure on pc.PiecesUnitId equals uomPieces.Id
                join uomQuantity in context.UnitsOfMeasure on pc.QuantityUnitId equals uomQuantity.Id
                join uomWeight in context.UnitsOfMeasure on pc.WeightUnitId equals uomWeight.Id

                select new ProductMasterAdvancedQuery()
                {
                    Coid = coid,
                    StockType = pc.Description,
                    ProductId = p.Id,
                    ProductCode = p.Code,
                    StockGrade = sg.Code,
                    Description = p.Description,
                    SizeDescription = p.SizeDescription,
                    Width = (
                        ((pc.Dim1TypeId == dimTypeIdForWidth) ? (p.Dim1StaticDimension ?? (decimal)0) : (decimal)0)) +
                        ((pc.Dim2TypeId == dimTypeIdForWidth) ? (p.Dim2StaticDimension ?? (decimal)0) : (decimal)0) +
                        ((pc.Dim3TypeId == dimTypeIdForWidth) ? (p.Dim3StaticDimension ?? (decimal)0) : (decimal)0) +
                        ((pc.Dim4TypeId == dimTypeIdForWidth) ? (p.Dim4StaticDimension ?? (decimal)0) : (decimal)0) +
                        ((pc.Dim5TypeId == dimTypeIdForWidth) ? (p.Dim5StaticDimension ?? (decimal)0) : (decimal)0),
                    Length = (
                        ((pc.Dim1TypeId == dimTypeIdForLength) ? (p.Dim1StaticDimension ?? (decimal)0) : (decimal)0)) +
                        ((pc.Dim2TypeId == dimTypeIdForLength) ? (p.Dim2StaticDimension ?? (decimal)0) : (decimal)0) +
                        ((pc.Dim3TypeId == dimTypeIdForLength) ? (p.Dim3StaticDimension ?? (decimal)0) : (decimal)0) +
                        ((pc.Dim4TypeId == dimTypeIdForLength) ? (p.Dim4StaticDimension ?? (decimal)0) : (decimal)0) +
                        ((pc.Dim5TypeId == dimTypeIdForLength) ? (p.Dim5StaticDimension ?? (decimal)0) : (decimal)0),
                    Thick = (
                        ((pc.Dim1TypeId == dimTypeIdForThick) ? (p.Dim1StaticDimension ?? (decimal)0) : (decimal)0)) +
                        ((pc.Dim2TypeId == dimTypeIdForThick) ? (p.Dim2StaticDimension ?? (decimal)0) : (decimal)0) +
                        ((pc.Dim3TypeId == dimTypeIdForThick) ? (p.Dim3StaticDimension ?? (decimal)0) : (decimal)0) +
                        ((pc.Dim4TypeId == dimTypeIdForThick) ? (p.Dim4StaticDimension ?? (decimal)0) : (decimal)0) +
                        ((pc.Dim5TypeId == dimTypeIdForThick) ? (p.Dim5StaticDimension ?? (decimal)0) : (decimal)0),
                    InsideDiameter = (
                         ((pc.Dim1TypeId == dimTypeIdForId) ? (p.Dim1StaticDimension ?? (decimal)0) : (decimal)0)) +
                         ((pc.Dim2TypeId == dimTypeIdForId) ? (p.Dim2StaticDimension ?? (decimal)0) : (decimal)0) +
                         ((pc.Dim3TypeId == dimTypeIdForId) ? (p.Dim3StaticDimension ?? (decimal)0) : (decimal)0) +
                         ((pc.Dim4TypeId == dimTypeIdForId) ? (p.Dim4StaticDimension ?? (decimal)0) : (decimal)0) +
                         ((pc.Dim5TypeId == dimTypeIdForId) ? (p.Dim5StaticDimension ?? (decimal)0) : (decimal)0),
                    OuterDiameter = (
                         ((pc.Dim1TypeId == dimTypeIdForOd) ? (p.Dim1StaticDimension ?? (decimal)0) : (decimal)0)) +
                         ((pc.Dim2TypeId == dimTypeIdForOd) ? (p.Dim2StaticDimension ?? (decimal)0) : (decimal)0) +
                         ((pc.Dim3TypeId == dimTypeIdForOd) ? (p.Dim3StaticDimension ?? (decimal)0) : (decimal)0) +
                         ((pc.Dim4TypeId == dimTypeIdForOd) ? (p.Dim4StaticDimension ?? (decimal)0) : (decimal)0) +
                         ((pc.Dim5TypeId == dimTypeIdForOd) ? (p.Dim5StaticDimension ?? (decimal)0) : (decimal)0),
                    Density = (
                         ((pc.Dim1TypeId == dimTypeIdForDensity) ? (p.Dim1StaticDimension ?? (decimal)0) : (decimal)0)) +
                         ((pc.Dim2TypeId == dimTypeIdForDensity) ? (p.Dim2StaticDimension ?? (decimal)0) : (decimal)0) +
                         ((pc.Dim3TypeId == dimTypeIdForDensity) ? (p.Dim3StaticDimension ?? (decimal)0) : (decimal)0) +
                         ((pc.Dim4TypeId == dimTypeIdForDensity) ? (p.Dim4StaticDimension ?? (decimal)0) : (decimal)0) +
                         ((pc.Dim5TypeId == dimTypeIdForDensity) ? (p.Dim5StaticDimension ?? (decimal)0) : (decimal)0),
                    ProductControlCode = pc.Code,
                    ControlPieces = (pc.ControlPiece == true) ? 1 : 0,
                    VolumeDensity = pcat.VolumeDensity ?? 1,
                    ProductDensity = p.Density ?? 1,
                    Dim1StaticDimension = p.Dim1StaticDimension ?? 1,
                    Dim2StaticDimension = p.Dim2StaticDimension ?? 1,
                    Dim3StaticDimension = p.Dim3StaticDimension ?? 1,
                    ProductCondition = p.SpecificationValue2,
                    ProductCategory = pcat.Category,
                    MetalCategory = sac1.Description,
                    MetalType = sac2.Description,
                    FactorForLbs = factorForLbs,
                    FactorForKilograms = factorForKilos
                }).AsQueryable();

            return stockItemQuery;

        }

    }
}