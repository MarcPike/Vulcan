using System;
using System.Globalization;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Vulcan.Mongo.MetalogicsCacheData
{
    public class ProductMastersCache: BaseDocument
    {
        public Guid CacheId { get; set; }
        public static MongoRawQueryHelper<ProductMastersCache> Helper = new MongoRawQueryHelper<ProductMastersCache>();

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
        public bool ControlPieces { get; set; }
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

        public decimal FactorForLbs { get; set; }
        public decimal FactorForKgs { get; set; }

        public string StratificationRank { get; set; }

        public string ProductType { get; set; }

        private string Normalize(decimal value)
        {
            return (value / 1.000000000000000000000000000000000m).ToString(CultureInfo.InvariantCulture);
        }

        public string ProductSize { get; set; }

        public decimal TheoWeight { get; set; }

    }
}
