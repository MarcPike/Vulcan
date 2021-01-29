using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.iMetal.Core.Context;
using DAL.iMetal.Core.DbUtilities;
using DAL.iMetal.Core.Helpers;

namespace DAL.iMetal.Core.Queries
{
    public class ProductMastersQuery : BaseQuery<ProductMastersQuery>
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

        public bool IsMachineComponent => MetalType == "MACHINE COMPONENT";

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
                var coidFactor = new[] {"SIN", "MSA", "DUB", "EUR"}.Contains(Coid) ? (decimal) 0.45359237 : (decimal) 1;

                if (Dim1StaticDimension == 0) Dim1StaticDimension = 1;

                if (ProductControlCode == "ZWS") result = (decimal) 0;
                else
                {
                    if (new[] {"ME", "MW"}.Contains(controlCode))
                    {
                        result = ((ControlPieces) ? 1 : 0) * Density;
                    }
                    else if (new[] {"FE", "FW"}.Contains(controlCode))
                    {
                        result = ((ControlPieces) ? 1 : 0) * Dim1StaticDimension * Dim2StaticDimension * Dim3StaticDimension *
                                 ProductDensity * VolumeDensity * coidFactor;
                    }
                    else if (new[] {"DE", "DW"}.Contains(controlCode))
                    {
                        result = ((ControlPieces) ? 1 : 0) * Dim1StaticDimension * Dim2StaticDimension * Dim2StaticDimension *
                                 ProductDensity * VolumeDensity *
                                 (decimal) 0.785398 * coidFactor;
                    }

                    if (new[] {"TE", "TW"}.Contains(controlCode))
                    {
                        result = ((ControlPieces) ? 1 : 0) *
                                 Dim1StaticDimension *
                                 (
                                     (Dim2StaticDimension * Dim2StaticDimension)
                                     - (Dim3StaticDimension * Dim3StaticDimension)
                                 )
                                 * ProductDensity * VolumeDensity * (decimal) 0.785398 * coidFactor;
                    }
                }

                //Trace.WriteLine($"CC: {controlCode} ProductId: {stockItem.ProductId} TW: {result}");
                return result;
            }
        }

        public static ProductMastersQuery GetForId(string coid, int productId)
        {
            if (coid == null) return null;
            var parameters = new Dictionary<string, object>();
            parameters.Add("ProductId", productId);
            return ExecuteAsync(coid, parameters).Result.FirstOrDefault();
        }

        public static ProductMastersQuery GetForCode(string coid, string productCode)
        {
            if (coid == null) return null;
            var parameters = new Dictionary<string, object>();
            parameters.Add("ProductCode", productCode);
            return ExecuteAsync(coid, parameters).Result.FirstOrDefault();
        }

        public static List<string> GetAllProductCategories(string coid)
        {
            var conn = ConnectionFactory.GetConnection(coid);

            var sqlQueryDynamic = new SqlQueryDynamic(coid, conn);
            var categories = sqlQueryDynamic.ExecuteQueryAsync(
                "select distinct category from product_categories").Result.Select(x=>x.category).ToList();
            var result = new List<string>();
            foreach (var category in categories)
            {
                result.Add(category);
            }
            return result.OrderBy(x=>x).ToList();
        }

        public static List<string> GetAllProductConditions(string coid)
        {
            var conn = ConnectionFactory.GetConnection(coid);

            var sqlQueryDynamic = new SqlQueryDynamic(coid, conn);
            var conditions = sqlQueryDynamic.ExecuteQueryAsync(
                "select distinct specification_value2 AS condition from products").Result.Select(x=>x.condition).ToList();
            var result = new List<string>();
            foreach (var condition in conditions)
            {
                result.Add(condition);
            }
            return result.OrderBy(x => x).ToList();
        }


        public new static async Task<List<ProductMastersQuery>> ExecuteAsync(string coid,
            Dictionary<string, object> parameters = null)
        {
            var conn = ConnectionFactory.GetConnection(coid);
            var sqlQueryDynamic = new SqlQueryDynamic(coid, conn);

            var dimTypes = sqlQueryDynamic.ExecuteQueryAsync(
                "select id, name from stock_dimension_types").Result;
            var dimTypeIdForWidth = dimTypes.Single(x => x.name == "WIDTH").id;
            var dimTypeIdForLength = dimTypes.Single(x => x.name == "LENGTH").id;
            var dimTypeIdForThick = dimTypes.Single(x => x.name == "THICK").id;
            var dimTypeIdForInsideDiameter = dimTypes.Single(x => x.name == "ID").id;
            var dimTypeIdForOuterDiameter = dimTypes.Single(x => x.name == "OD").id;

            sqlQueryDynamic.Dispose();

            var sqlForWidth = $@"
                CASE 
                    WHEN p.dim1_type_id = {dimTypeIdForWidth} THEN COALESCE(p.dim1_static_dimension,0)
                    WHEN p.dim2_type_id = {dimTypeIdForWidth} THEN COALESCE(p.dim2_static_dimension,0)
                    WHEN p.dim3_type_id = {dimTypeIdForWidth} THEN COALESCE(p.dim3_static_dimension,0)
                    WHEN p.dim4_type_id = {dimTypeIdForWidth} THEN COALESCE(p.dim4_static_dimension,0)
                    WHEN p.dim5_type_id = {dimTypeIdForWidth} THEN COALESCE(p.dim5_static_dimension,0)
                END AS Width";
            var sqlForLength = $@"
                CASE 
                    WHEN p.dim1_type_id = {dimTypeIdForLength} THEN COALESCE(p.dim1_static_dimension,0)
                    WHEN p.dim2_type_id = {dimTypeIdForLength} THEN COALESCE(p.dim2_static_dimension,0)
                    WHEN p.dim3_type_id = {dimTypeIdForLength} THEN COALESCE(p.dim3_static_dimension,0)
                    WHEN p.dim4_type_id = {dimTypeIdForLength} THEN COALESCE(p.dim4_static_dimension,0)
                    WHEN p.dim5_type_id = {dimTypeIdForLength} THEN COALESCE(p.dim5_static_dimension,0)
                END AS Length";
            var sqlForThick = $@"
                CASE 
                    WHEN p.dim1_type_id = {dimTypeIdForThick} THEN COALESCE(p.dim1_static_dimension,0)
                    WHEN p.dim2_type_id = {dimTypeIdForThick} THEN COALESCE(p.dim2_static_dimension,0)
                    WHEN p.dim3_type_id = {dimTypeIdForThick} THEN COALESCE(p.dim3_static_dimension,0)
                    WHEN p.dim4_type_id = {dimTypeIdForThick} THEN COALESCE(p.dim4_static_dimension,0)
                    WHEN p.dim5_type_id = {dimTypeIdForThick} THEN COALESCE(p.dim5_static_dimension,0)
                END AS Thick";
            var sqlForId = $@"
                CASE 
                    WHEN p.dim1_type_id = {dimTypeIdForInsideDiameter} THEN COALESCE(p.dim1_static_dimension,0)
                    WHEN p.dim2_type_id = {dimTypeIdForInsideDiameter} THEN COALESCE(p.dim2_static_dimension,0)
                    WHEN p.dim3_type_id = {dimTypeIdForInsideDiameter} THEN COALESCE(p.dim3_static_dimension,0)
                    WHEN p.dim4_type_id = {dimTypeIdForInsideDiameter} THEN COALESCE(p.dim4_static_dimension,0)
                    WHEN p.dim5_type_id = {dimTypeIdForInsideDiameter} THEN COALESCE(p.dim5_static_dimension,0)
                END AS InsideDiameter";
            var sqlForOd = $@"
                CASE 
                    WHEN p.dim1_type_id = {dimTypeIdForOuterDiameter} THEN COALESCE(p.dim1_static_dimension,0)
                    WHEN p.dim2_type_id = {dimTypeIdForOuterDiameter} THEN COALESCE(p.dim2_static_dimension,0)
                    WHEN p.dim3_type_id = {dimTypeIdForOuterDiameter} THEN COALESCE(p.dim3_static_dimension,0)
                    WHEN p.dim4_type_id = {dimTypeIdForOuterDiameter} THEN COALESCE(p.dim4_static_dimension,0)
                    WHEN p.dim5_type_id = {dimTypeIdForOuterDiameter} THEN COALESCE(p.dim5_static_dimension,0)
                END AS OuterDiameter";

            var factorForLbs = UomHelper.GetFactorForPounds(coid).ToString("0.00000");
            var factorForKilos = UomHelper.GetFactorForKilograms(coid).ToString("0.00000");

            var sqlFactorForLbs = $"{factorForLbs} as FactorForLbs";
            var sqlFactorForKilos = $"{factorForKilos} as FactorForKgs";

            var sql = @$"
            SELECT 
                '{coid}' as Coid,
                p.id as ProductId,
                sac1.Description as MetalCategory,
                sac2.Description as MetalType,
                pc.Description as StockType,
                p.Code as ProductCode,
                sg.Code as StockGrade,
                p.specification_value2 as ProductCondition,
                pcat.Category as ProductCategory,
                p.specification_value9 as StratificationRank,
                p.density as Density,
				{sqlForWidth},
				{sqlForLength},
				{sqlForThick},
				{sqlForId},
				{sqlForOd},
                {sqlFactorForLbs},
                {sqlFactorForKilos},
                pc.code as ProductControlCode,
                pc.control_pieces as ControlPieces,
                pcat.volume_density as VolumeDensity,
                p.density as ProductDensity, 
                COALESCE(p.dim1_static_dimension,0) as Dim1StaticDimension,
                COALESCE(p.dim2_static_dimension,0) as Dim2StaticDimension,
                COALESCE(p.dim3_static_dimension,0) as Dim3StaticDimension,
				uomPieces.Code as PiecesUom,
                uomQuantity.code as QuantityUom,
				uomWeight.code as WeightUom
            FROM products p 
            INNER JOIN product_categories pcat ON pcat.id = p.category_id
            LEFT OUTER JOIN stock_grades sg ON sg.id = p.grade_id
            LEFT OUTER JOIN stock_analysis_codes sac1 ON sac1.id = pcat.analysis1_id
            LEFT OUTER JOIN stock_analysis_codes sac2 ON sac2.id = pcat.analysis2_id
            JOIN product_controls pc ON pc.id = pcat.product_control_id
            LEFT OUTER JOIN units_of_measure uomPieces ON uomPieces.id = pc.pieces_unit_id
            LEFT OUTER JOIN units_of_measure uomQuantity ON uomQuantity.id = pc.quantity_unit_id
            LEFT OUTER JOIN units_of_measure uomWeight ON uomWeight.id = pc.weight_unit_id   
            ";

            if (parameters != null)
            {
                var productId = parameters.FirstOrDefault(x => x.Key == "ProductId").Value;
                var productCode = parameters.FirstOrDefault(x => x.Key == "ProductCode").Value;
                if (productId != null)
                {
                    sql += $" WHERE p.id = {productId}";
                } else if (productCode != null)
                {
                    sql += $" WHERE p.code = '{productCode}'";
                }
            }

            var sqlQuery = new SqlQuery<ProductMastersQuery>();
            return await sqlQuery.ExecuteQueryAsync(coid, sql);
        }
    }
}
