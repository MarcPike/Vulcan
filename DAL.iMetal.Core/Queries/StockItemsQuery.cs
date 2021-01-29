using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DAL.iMetal.Core.Context;
using DAL.iMetal.Core.DbUtilities;
using DAL.iMetal.Core.Helpers;

namespace DAL.iMetal.Core.Queries
{
    public class StockItemsQuery : BaseQuery<StockItemsQuery>
    {

        public string Coid { get; set; }
        public int ProductId { get; set; }
        public int StockItemId { get; set; }
        public DateTime? CreateDate { get; set; }
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
                if (Coid == "EUR") return "GBP";

                if (Coid == "CAN") return "CAD";

                return "USD";
            }
        }

        public string ProductSize
        {
            get
            {
                if (InsideDiameter > 0) return Normalize(OuterDiameter) + "-" + Normalize(InsideDiameter);

                if (OuterDiameter > 0) return Normalize(OuterDiameter);

                var productCodeTokens = ProductCode.Split(' ');
                if (productCodeTokens.Count() > 2)
                    return productCodeTokens[1];
                return "<Unknown>";
            }
        }

        public string MillCode { get; set; }
        public string MillName { get; set; }
        public string ProductControlCode { get; set; }
        public bool ControlPieces { get; set; }
        public decimal VolumeDensity { get; set; }
        public decimal Dim1StaticDimension { get; set; }
        public decimal Dim2StaticDimension { get; set; }
        public decimal Dim3StaticDimension { get; set; }


        public decimal Width { get; set; }

        public decimal Length { get; set; }

        public decimal InsideDiameter { get; set; }

        public decimal OuterDiameter { get; set; }

        public decimal Density { get; set; }

        public string StockHoldReason { get; set; }
        public string StockHoldUser { get; set; }

        public int PhysicalPieces { get; set; }
        public int AllocatedPieces { get; set; }
        public int AvailablePieces { get; set; }
        public string PiecesUnit { get; set; }


        public decimal PhysicalQuantity { get; set; }
        public decimal AllocatedQuantity { get; set; }
        public decimal AvailableQuantity => PhysicalQuantity - AllocatedQuantity;
        public string QuantityUnit { get; set; }


        public decimal PhysicalLength { get; set; }
        public decimal AllocatedLength { get; set; }

        public decimal AvailableLength => PhysicalLength - AllocatedLength;

        public string LengthUnit { get; set; }


        public decimal PhysicalWeight { get; set; }
        public decimal PhysicalWeightLbs { get; set; }
        public decimal PhysicalWeightKgs { get; set; }


        public decimal AllocatedWeight { get; set; }
        public decimal AllocatedWeightLbs { get; set; }
        public decimal AllocatedWeightKgs { get; set; }


        public decimal AvailableWeight { get; set; }
        public decimal AvailableWeightLbs => FactorForLbs * AvailableWeight;
        public decimal AvailableWeightKgs => FactorForKgs * AvailableWeight;
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
                        return 0;
                    return (PhysicalWeight - AllocatedWeight) * factorForLbs / theoWeight;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Found the bug: {e.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        ///     Not safe to use because PhysicalQuantity is not guaranteed to be inches.
        /// </summary>
        public decimal CostPerInch
        {
            get
            {
                try
                {
                    if (AvailableInches == 0) return 0;

                    return TotalCost / AvailableInches;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Found the bug: {e.Message}");
                    throw;
                }
            }
        }

        public decimal CostPerWeight  
        {
            get
            {
                if ((PhysicalWeight != 0) && (TotalCost > 0))
                {
                    return TotalCost / PhysicalWeight;
                }
                //CostPerWeight = (si.PhysicalWeight == 0) ? 0 :
                //    (
                //        (((si.MaterialValue ?? 0) + (si.ProductionValue ?? 0) + (si.TransportValue ?? 0) + (si.SurchargeValue ?? 0) + (si.MiscellaneousValue ?? 0))
                //         / (si.PhysicalWeight ?? 0))),
                //CostPerQty = (si.PhysicalQuantity == 0) ? 0 :
                //    ((si.MaterialValue ?? 0) + (si.ProductionValue ?? 0) + (si.TransportValue ?? 0) + (si.SurchargeValue ?? 0) + (si.MiscellaneousValue ?? 0))
                //    / (si.PhysicalQuantity ?? 0),

                return 0;

            }

        }

        public decimal CostPerQty
        {
            get
            {
                if ((PhysicalQuantity != 0) && (TotalCost > 0))
                {
                    return TotalCost / PhysicalQuantity;
                }
                //CostPerWeight = (si.PhysicalWeight == 0) ? 0 :
                //    (
                //        (((si.MaterialValue ?? 0) + (si.ProductionValue ?? 0) + (si.TransportValue ?? 0) + (si.SurchargeValue ?? 0) + (si.MiscellaneousValue ?? 0))
                //         / (si.PhysicalWeight ?? 0))),
                //CostPerQty = (si.PhysicalQuantity == 0) ? 0 :
                //    ((si.MaterialValue ?? 0) + (si.ProductionValue ?? 0) + (si.TransportValue ?? 0) + (si.SurchargeValue ?? 0) + (si.MiscellaneousValue ?? 0))
                //    / (si.PhysicalQuantity ?? 0),

                return 0;

            }
        }


        public string StratificationRank { get; set; }

        public decimal CostPerLb
        {
            get
            {
                if (new[] {"SIN", "MSA", "DUB", "EUR"}.Contains(Coid))
                    return CostPerWeight * (decimal) 0.453592;
                return CostPerWeight;
            }
        } //CostPerWeight * FactorForLbs;

        public decimal CostPerKg
        {
            get
            {
                if (new[] {"SIN", "MSA", "DUB", "EUR"}.Contains(Coid))
                    return CostPerWeight;
                return CostPerWeight * (decimal) 2.20462;
            }
        } //=> CostPerWeight * FactorForKilograms;

        public decimal ProductDensity { get; set; }


        public DateTime ReceivedDate { get; set; }

        public bool IsMachinedPart
        {
            get
            {
                var result = ProductControlCode == "ME" || ProductControlCode == "MW";

                return result;
            }
        }

        public bool IsZeroWeightService
        {
            get
            {
                var result = ProductControlCode == "ZWS" || ProductControlCode == "ZWT";

                return result;
            }
        }

        public decimal PieceCost
        {
            get
            {
                var result = (decimal) 0;
                if (AvailablePieces > 0) result = TotalCost / AvailablePieces;

                return result;
            }
        }

        public decimal TheoWeight
        {
            get
            {
                decimal result = 0;
                var controlCode = ProductControlCode;
                var coidFactor = new[] {"SIN", "MSA", "DUB", "EUR"}.Contains(Coid)
                    ? (decimal) 0.45359237
                    : 1;

                if (Dim1StaticDimension == 0) Dim1StaticDimension = 1;

                if (ProductControlCode == "ZWS")
                {
                    result = 0;
                }
                else
                {
                    if (new[] {"ME", "MW"}.Contains(controlCode))
                        result = ((ControlPieces)? 1: 0) * Density;
                    else if (new[] {"FE", "FW"}.Contains(controlCode))
                        result = ((ControlPieces) ? 1 : 0) * Dim1StaticDimension * Dim2StaticDimension * Dim3StaticDimension *
                                 ProductDensity * VolumeDensity * coidFactor;
                    else if (new[] {"DE", "DW"}.Contains(controlCode))
                        result = ((ControlPieces) ? 1 : 0) * Dim1StaticDimension * Dim2StaticDimension * Dim2StaticDimension *
                                 ProductDensity * VolumeDensity *
                                 (decimal) 0.785398 * coidFactor;

                    if (new[] {"TE", "TW"}.Contains(controlCode))
                        result = ((ControlPieces) ? 1 : 0) *
                                 Dim1StaticDimension *
                                 (
                                     Dim2StaticDimension * Dim2StaticDimension
                                     - Dim3StaticDimension * Dim3StaticDimension
                                 )
                                 * ProductDensity * VolumeDensity * (decimal) 0.785398 * coidFactor;
                }

                //Trace.WriteLine($"CC: {controlCode} ProductId: {stockItem.ProductId} TW: {result}");
                return result;
            }
        }

        public string ProductType
        {
            get
            {
                if (StockType.ToUpper().Contains("BAR")) return "Bar";

                return StockType.ToUpper().Contains("TUBE") ? "Tube" : "Other";
            }
        }

        public decimal FactorForLbs { get; set; }
        public decimal FactorForKgs { get; set; }

        private string Normalize(decimal value)
        {
            return (value / 1.000000000000000000000000000000000m).ToString(CultureInfo.InvariantCulture);
        }


        public static List<StockItemsQuery> GetForId(string coid, int productId)
        {
            if (coid == null) return null;
            var parameters = new Dictionary<string, object>();
            parameters.Add("ProductId", productId);
            return ExecuteAsync(coid, parameters).Result.ToList();
        }

        public static List<StockItemsQuery> GetForCode(string coid, string productCode)
        {
            if (coid == null) return null;
            var parameters = new Dictionary<string, object>();
            parameters.Add("ProductCode", productCode);
            return ExecuteAsync(coid, parameters).Result.ToList();
        }

        public static List<StockItemsQuery> FromStockId(string coid, in int stockItemId)
        {
            if (coid == null) return null;
            var parameters = new Dictionary<string, object>();
            parameters.Add("StockItemId", stockItemId);
            return ExecuteAsync(coid, parameters).Result.ToList();
        }

        public static StockItemsQuery GetForTag(string coid, string tagNumber)
        {
            if (coid == null) return null;
            var parameters = new Dictionary<string, object>();
            parameters.Add("TagNumber", tagNumber);
            return ExecuteAsync(coid, parameters).Result.FirstOrDefault();

        }

        public static List<StockItemsQuery> ForHeatNumber(string coid, string heatNumber)
        {
            if (coid == null) return null;
            var parameters = new Dictionary<string, object>();
            parameters.Add("HeatNumber", heatNumber);
            return ExecuteAsync(coid, parameters).Result.ToList();
        }

        public new static async Task<List<StockItemsQuery>> ExecuteAsync(string coid,
            Dictionary<string, object> parameters = null)
        {
            var conn = ConnectionFactory.GetConnection(coid);

            var sqlQueryDynamic = new SqlQueryDynamic(coid, conn);


            //var gbp = sqlQueryDynamic.ExecuteQueryAsync(
            //        "select exchange_rate from currency_codes where symbol ='GBP'").Result.FirstOrDefault()
            //    ?.exchange_rate ?? 1;
            //var cad = sqlQueryDynamic.ExecuteQueryAsync(
            //        "select exchange_rate from currency_codes where symbol ='CAD'").Result.FirstOrDefault()
            //    ?.exchange_rate ?? 1;
            //var usd = sqlQueryDynamic.ExecuteQueryAsync(
            //        "select exchange_rate from currency_codes where symbol ='USD'").Result.FirstOrDefault()
            //    ?.exchange_rate ?? 1;


            var dimTypes = sqlQueryDynamic.ExecuteQueryAsync(
                "select id, name from stock_dimension_types").Result;
            var dimTypeIdForWidth = dimTypes.Single(x => x.name == "WIDTH").id;
            var dimTypeIdForLength = dimTypes.Single(x => x.name == "LENGTH").id;
            var dimTypeIdForThick = dimTypes.Single(x => x.name == "THICK").id;
            var dimTypeIdForInsideDiameter = dimTypes.Single(x => x.name == "ID").id;
            var dimTypeIdForOuterDiameter = dimTypes.Single(x => x.name == "OD").id;
            var dimTypeIdForDensity = dimTypes.Single(x => x.name == "DENSITY").id;

            sqlQueryDynamic.Dispose();

            var sqlForWidth = $@"
                CASE 
                    WHEN pc.dim1_type_id = {dimTypeIdForWidth} THEN COALESCE(si.dim1,0)
                    WHEN pc.dim2_type_id = {dimTypeIdForWidth} THEN COALESCE(si.dim2,0)
                    WHEN pc.dim3_type_id = {dimTypeIdForWidth} THEN COALESCE(si.dim3,0)
                    WHEN pc.dim4_type_id = {dimTypeIdForWidth} THEN COALESCE(si.dim4,0)
                    WHEN pc.dim5_type_id = {dimTypeIdForWidth} THEN COALESCE(si.dim5,0)
                END AS Width";
            var sqlForLength = $@"
                CASE 
                    WHEN pc.dim1_type_id = {dimTypeIdForLength} THEN COALESCE(si.dim1,0)
                    WHEN pc.dim2_type_id = {dimTypeIdForLength} THEN COALESCE(si.dim2,0)
                    WHEN pc.dim3_type_id = {dimTypeIdForLength} THEN COALESCE(si.dim3,0)
                    WHEN pc.dim4_type_id = {dimTypeIdForLength} THEN COALESCE(si.dim4,0)
                    WHEN pc.dim5_type_id = {dimTypeIdForLength} THEN COALESCE(si.dim5,0)
                END AS Length";
            var sqlForThick = $@"
                CASE 
                    WHEN pc.dim1_type_id = {dimTypeIdForThick} THEN COALESCE(si.dim1,0)
                    WHEN pc.dim2_type_id = {dimTypeIdForThick} THEN COALESCE(si.dim2,0)
                    WHEN pc.dim3_type_id = {dimTypeIdForThick} THEN COALESCE(si.dim3,0)
                    WHEN pc.dim4_type_id = {dimTypeIdForThick} THEN COALESCE(si.dim4,0)
                    WHEN pc.dim5_type_id = {dimTypeIdForThick} THEN COALESCE(si.dim5,0)
                END AS Thick";
            var sqlForId = $@"
                CASE 
                    WHEN pc.dim1_type_id = {dimTypeIdForInsideDiameter} THEN COALESCE(si.dim1,0)
                    WHEN pc.dim2_type_id = {dimTypeIdForInsideDiameter} THEN COALESCE(si.dim2,0)
                    WHEN pc.dim3_type_id = {dimTypeIdForInsideDiameter} THEN COALESCE(si.dim3,0)
                    WHEN pc.dim4_type_id = {dimTypeIdForInsideDiameter} THEN COALESCE(si.dim4,0)
                    WHEN pc.dim5_type_id = {dimTypeIdForInsideDiameter} THEN COALESCE(si.dim5,0)
                END AS InsideDiameter";
            var sqlForOd = $@"
                CASE 
                    WHEN pc.dim1_type_id = {dimTypeIdForOuterDiameter} THEN COALESCE(si.dim1,0)
                    WHEN pc.dim2_type_id = {dimTypeIdForOuterDiameter} THEN COALESCE(si.dim2,0)
                    WHEN pc.dim3_type_id = {dimTypeIdForOuterDiameter} THEN COALESCE(si.dim3,0)
                    WHEN pc.dim4_type_id = {dimTypeIdForOuterDiameter} THEN COALESCE(si.dim4,0)
                    WHEN pc.dim5_type_id = {dimTypeIdForOuterDiameter} THEN COALESCE(si.dim5,0)
                END AS OuterDiameter";
            var sqlForDensity = $@"
                CASE 
                    WHEN pc.dim1_type_id = {dimTypeIdForDensity} THEN COALESCE(si.dim1,0)
                    WHEN pc.dim2_type_id = {dimTypeIdForDensity} THEN COALESCE(si.dim2,0)
                    WHEN pc.dim3_type_id = {dimTypeIdForDensity} THEN COALESCE(si.dim3,0)
                    WHEN pc.dim4_type_id = {dimTypeIdForDensity} THEN COALESCE(si.dim4,0)
                    WHEN pc.dim5_type_id = {dimTypeIdForDensity} THEN COALESCE(si.dim5,0)
                END AS Density";

            var factorForLbs = UomHelper.GetFactorForPounds(coid).ToString("0.00000");
            var factorForKilos = UomHelper.GetFactorForKilograms(coid).ToString("0.00000");

            var sqlFactorForLbs = $"{factorForLbs} as FactorForLbs";
            var sqlFactorForKilos = $"{factorForKilos} as FactorForKgs";

            var sqlForAllocatedLength = $@"
            CASE
               WHEN(uomQuantity.code = 'FT') THEN COALESCE(si.allocated_quantity,0)
               WHEN(uomQuantity.code = 'IN') THEN COALESCE(si.allocated_quantity,0)
               WHEN(uomQuantity.code = 'cm') THEN COALESCE(si.allocated_quantity,0)
               WHEN(uomQuantity.code = 'm') THEN COALESCE(si.allocated_quantity,0)
               WHEN(uomQuantity.code = 'mm') THEN COALESCE(si.allocated_quantity,0)
               ELSE 0
            END AS AllocatedLength
            ";
            var sqlForPhysicalLength = $@"
            CASE
               WHEN(uomQuantity.code = 'FT') THEN COALESCE(si.physical_quantity,0)
               WHEN(uomQuantity.code = 'IN') THEN COALESCE(si.physical_quantity,0)
               WHEN(uomQuantity.code = 'cm') THEN COALESCE(si.physical_quantity,0)
               WHEN(uomQuantity.code = 'm') THEN COALESCE(si.physical_quantity,0)
               WHEN(uomQuantity.code = 'mm') THEN COALESCE(si.physical_quantity,0)
               ELSE 0
            END AS PhysicalLength
            ";

            var sql = @$"
            SELECT 
                '{coid}' as Coid,
                p.id as ProductId,
                si.Id as StockItemId,
                si.Cdate as CreateDate,
                si.received_date as ReceivedDate,
                sac1.Description as MetalCategory,
                sac2.Description as MetalType,
                pc.Description as StockType,
                hr.Description as StockHoldReason,
                hu.Name as StockHoldUser,
                si.notes as Notes,
                si.Location,
                w.Code as WarehouseCode,
                w.Name as WarehouseName,
                w.short_name as WarehouseShortName,
                p.Code as ProductCode,
                sg.Code as StockGrade,
                p.specification_value2 as ProductCondition,
                pcat.Category as ProductCategory,
                m.Code as MillCode,
                m.Name as MillName,
                p.specification_value9 as StratificationRank,
                {sqlForWidth},
				{sqlForLength},
				{sqlForThick},
				{sqlForId},
				{sqlForOd},
                {sqlForDensity},
                {sqlFactorForLbs},
                {sqlFactorForKilos},
                {sqlForAllocatedLength},
                {sqlForPhysicalLength},
                pc.code as ProductControlCode,
                pc.control_pieces as ControlPieces,
                pcat.volume_density as VolumeDensity,
                p.density as ProductDensity, 
                COALESCE(p.dim1_static_dimension,0) as Dim1StaticDimension,
                COALESCE(p.dim2_static_dimension,0) as Dim2StaticDimension,
                COALESCE(p.dim3_static_dimension,0) as Dim3StaticDimension,

                COALESCE(si.physical_pieces,0) as PhysicalPieces,
                COALESCE(si.allocated_pieces,0) as AllocatedPieces,
                COALESCE(si.physical_pieces,0) - COALESCE(si.allocated_pieces,0) as AvailablePieces,
				
				uomPieces.Code as PiecesUom,
                uomQuantity.code as QuantityUom,
				uomWeight.code as WeightUom,
				
				COALESCE(si.physical_quantity,0) as PhysicalQuantity,
				COALESCE(si.allocated_quantity,0) as AllocatedQuantity,
				
                COALESCE(si.physical_weight,0) as PhysicalWeight,
                COALESCE(si.allocated_Weight,0) as AllocatedWeight,
                COALESCE(si.physical_weight,0) - COALESCE(si.allocated_weight,0) as AvailableWeight,

                COALESCE(si.material_value,0) as MaterialValue,
                COALESCE(si.production_value,0) as ProductionValue,
                COALESCE(si.transport_value,0) as TransportValue,
                COALESCE(si.surcharge_value,0) as SurchargeValue,
                COALESCE(si.miscellaneous_value,0) as MiscellaneousValue,
                COALESCE(si.material_value,0) + COALESCE(si.production_value,0) + COALESCE(si.transport_value,0) + COALESCE(si.surcharge_value,0) + COALESCE(si.miscellaneous_value,0) as TotalCost,
                si.number as TagNumber,
                sc.cast_number as HeatNumber

            FROM
	            stock_items si
            LEFT OUTER JOIN stock_hold_reasons hr ON hr.id = si.hold_reason_id
            LEFT OUTER JOIN personnel hu ON hu.id = si.hold_user_id
            INNER JOIN products p ON p.id = si.product_id
            INNER JOIN product_categories pcat ON pcat.id = p.category_id
            LEFT OUTER JOIN stock_casts sc ON sc.id = si.stock_cast_id
            LEFT OUTER JOIN mills m ON m.id = sc.mill_id
            LEFT OUTER JOIN stock_grades sg ON sg.id = p.grade_id
            LEFT OUTER JOIN warehouses w ON w.id = si.warehouse_id
            LEFT OUTER JOIN stock_analysis_codes sac1 ON sac1.id = pcat.analysis1_id
            LEFT OUTER JOIN stock_analysis_codes sac2 ON sac2.id = pcat.analysis2_id
            INNER JOIN product_controls pc ON pc.id = pcat.product_control_id
            LEFT OUTER JOIN units_of_measure uomPieces ON uomPieces.id = pc.pieces_unit_id
            LEFT OUTER JOIN units_of_measure uomQuantity ON uomQuantity.id = pc.quantity_unit_id
            LEFT OUTER JOIN units_of_measure uomWeight ON uomWeight.id = pc.weight_unit_id   
            WHERE (si.physical_pieces - si.allocated_pieces) > 0
            ";
            if (parameters != null)
            {
                var productId = parameters.FirstOrDefault(x => x.Key == "ProductId").Value;
                var productCode = parameters.FirstOrDefault(x => x.Key == "ProductCode").Value;
                var stockItemId = parameters.FirstOrDefault(x => x.Key == "StockItemId").Value;
                var tagNumber = parameters.FirstOrDefault(x => x.Key == "TagNumber").Value;
                var heatNumber = parameters.FirstOrDefault(x => x.Key == "HeatNumber").Value;
                if (productId != null)
                {
                    sql += $" AND p.id = {productId}";
                }
                else if (productCode != null)
                {
                    sql += $" AND p.code = '{productCode}'";
                }
                else if (stockItemId != null)
                {
                    sql += $" AND si.id = {stockItemId}";
                }
                else if (tagNumber != null)
                {
                    sql += $" AND si.number = '{tagNumber}'";
                }
                else if (heatNumber != null)
                {
                    sql += $" AND sc.cast_number = '{heatNumber}'";
                }

            }


            var sqlQuery = new SqlQuery<StockItemsQuery>();
            return await sqlQuery.ExecuteQueryAsync(coid, sql);
        }

    }
}