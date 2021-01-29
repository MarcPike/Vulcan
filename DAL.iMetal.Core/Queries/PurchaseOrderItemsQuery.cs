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
    public class PurchaseOrderItemsQuery : BaseQuery<PurchaseOrderItemsQuery>
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
        public bool ControlPieces { get; set; }
        public decimal VolumeDensity { get; set; }
        public decimal Dim1StaticDimension { get; set; }
        public decimal Dim2StaticDimension { get; set; }
        public decimal Dim3StaticDimension { get; set; }

        public string OrderedPiecesUnit { get; set; } = string.Empty;
        public string OrderedLengthUnit { get; set; } = string.Empty;
        public string OrderedWeightUnit { get; set; } = string.Empty;
        public string OrderedQuantityUnit { get; set; } = string.Empty;
        public int OrderedPieces { get; set; } = 0;
        public decimal OrderedQuantity { get; set; } = 0;
        public decimal OrderedLength { get; set; } = 0;
        public decimal OrderedWeight { get; set; } = 0;
        public decimal OrderedWeightLbs => OrderedWeight * FactorForLbs;
        public decimal OrderedWeightKgs => OrderedWeight * FactorForKgs;

        public string AllocatedPiecesUnit { get; set; } = string.Empty;
        public string AllocatedLengthUnit { get; set; } = string.Empty;
        public string AllocatedWeightUnit { get; set; } = string.Empty;
        public string AllocatedQuantityUnit { get; set; } = string.Empty;
        public int AllocatedPieces { get; set; } = 0;
        public decimal AllocatedQuantity { get; set; } = 0;
        public decimal AllocatedLength { get; set; } = 0;
        public decimal AllocatedWeight { get; set; } = 0;
        public decimal AllocatedWeightLbs => AllocatedWeight * FactorForLbs;
        public decimal AllocatedWeightKgs => AllocatedWeight * FactorForKgs;

        public string DeliveredPiecesUnit { get; set; } = string.Empty;
        public string DeliveredLengthUnit { get; set; } = string.Empty;
        public string DeliveredWeightUnit { get; set; } = string.Empty;
        public string DeliveredQuantityUnit { get; set; } = string.Empty;
        public int DeliveredPieces { get; set; } = 0;
        public decimal DeliveredQuantity { get; set; } = 0;
        public decimal DeliveredLength { get; set; } = 0;
        public decimal DeliveredWeight { get; set; } = 0;
        public decimal DeliveredWeightLbs => DeliveredWeight * FactorForLbs;
        public decimal DeliveredWeightKgs => DeliveredWeight * FactorForKgs;

        public string BalancePiecesUnit { get; set; } = string.Empty;
        public string BalanceLengthUnit { get; set; } = string.Empty;
        public string BalanceWeightUnit { get; set; } = string.Empty;
        public string BalanceQuantityUnit { get; set; } = string.Empty;
        public int BalancePieces { get; set; } = 0;
        public decimal BalanceQuantity { get; set; } = 0;
        public decimal BalanceLength { get; set; } = 0;
        public decimal BalanceWeight { get; set; } = 0;
        public decimal BalanceWeightLbs => BalanceWeight * FactorForLbs;
        public decimal BalanceWeightKgs => BalanceWeight * FactorForKgs;

        public decimal MaterialCostTotal { get; set; }
        public decimal ProductionCostTotal { get; set; }
        public decimal TransportCostTotal { get; set; }
        public decimal SurchargeCostTotal { get; set; }
        public decimal MiscellaneousCostTotal { get; set; }
        public decimal TotalCost { get; set; }

        public decimal BaseMaterialValue { get; set; }
        public decimal BaseProductionValue { get; set; }
        public decimal BaseTransportValue { get; set; }
        public decimal BaseSurchargeValue { get; set; }
        public decimal BaseMiscellaneousValue { get; set; }

        public decimal TotalValue => BaseMaterialValue + 
                                     BaseProductionValue + 
                                     BaseTransportValue + 
                                     BaseSurchargeValue +
                                     BaseMiscellaneousValue;

        public decimal CostPerLb
        {
            get
            {
                if (BalanceWeight == 0) return 0;

                return (TotalValue / BalanceWeight) * FactorForLbs;

            }
        }

        public decimal CostPerKg
        {
            get
            {
                if (BalanceWeight == 0) return 0;

                return (TotalValue / BalanceWeight) * FactorForKgs;

            }
        }

        public decimal CostPerInch
        {
            get
            {
                if (BalanceQuantity == 0) return 0;

                return (TotalValue / BalanceQuantity);
            }
        }

        public decimal ProductDensity { get; set; }

        /// <summary>
        /// Not safe to use because PhysicalQuantity is not guaranteed to be inches.
        /// </summary>

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
        public decimal FactorForKgs { get; set; }
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

        public static List<PurchaseOrderItemsQuery> GetForId(string coid, int productId)
        {
            if (coid == null) return null;
            var parameters = new Dictionary<string, object>();
            parameters.Add("ProductId", productId);
            return ExecuteAsync(coid, parameters).Result.ToList();
        }

        public static List<PurchaseOrderItemsQuery> GetForCode(string coid, string productCode)
        {
            if (coid == null) return null;
            var parameters = new Dictionary<string, object>();
            parameters.Add("ProductCode", productCode);
            return ExecuteAsync(coid, parameters).Result.ToList();
        }

        public new static async Task<List<PurchaseOrderItemsQuery>> ExecuteAsync(string coid,
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
            var dimTypeIdForDensity = dimTypes.Single(x => x.name == "DENSITY").id;

            sqlQueryDynamic.Dispose();

            var sqlForWidth = $"COALESCE(dim.dim{dimTypeIdForWidth},0) as Width";
            var sqlForLength = $"COALESCE(dim.dim{dimTypeIdForWidth},0) as Length";
            var sqlForThick = $"COALESCE(dim.dim{dimTypeIdForThick},0) as Thick";
            var sqlForId = $"COALESCE(dim.dim{dimTypeIdForInsideDiameter},0) as InsideDiameter";
            var sqlForOd = $"COALESCE(dim.dim{dimTypeIdForOuterDiameter},0) as OuterDiameter";

            var factorForLbs = UomHelper.GetFactorForPounds(coid).ToString("0.00000");
            var factorForKilos = UomHelper.GetFactorForKilograms(coid).ToString("0.00000");

            var sqlFactorForLbs = $"{factorForLbs} as FactorForLbs";
            var sqlFactorForKilos = $"{factorForKilos} as FactorForKgs";

            var sql = @$"
                SELECT
                  '{coid}' as Coid,
                  pos.code as StatusCode,
                  pos.description as StatusDescription,
                  pos.internal_status_id as InternalStatusId,
                  poi.id as PurchaseOrderItemId ,
                  poi.cdate as CreateDate,
                  sac1.description as MetalCategory,
                  sac2.description as MetalType,
                  pc.description as StockType,
                  p.id as ProductId,
                  p.code as ProductCode,
                  sg.code as StockGrade,
                  c.name as Supplier,
                  b.name as Buyer,
                  p.specification_value2 as ProductCondition ,
                  pcat.category as ProductCategory,
                  poi.purchase_header_id as PurchaseOrderHeaderId,
                  poh.purchase_type as PurchaseType,
                  poh.transfer_type as TransferType,
                  poh.request_type as RequestType,
                  pcode.code as PurchaseCategoryCode,
                  pcode.description as PurchaseCategoryDescription,
                  wh.code as WarehouseCode,
                  wh.name as WarehouseName,
                  wh.short_name as WarehouseShortName,
                  poi.item_number as ItemNumber,
                  poh.number as PoNumber,
                  poh.due_date as DueDate,
                  poi.due_date as ItemDueDate,
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
                  COALESCE(p.dim1_static_dimension,0) as Dim1StaticDimension,
                  COALESCE(p.dim2_static_dimension,0) as Dim2StaticDimension,
                  COALESCE(p.dim3_static_dimension,0) as Dim3StaticDimension,
                  COALESCE(pot.base_material_value,0) as MaterialCostTotal,
                  COALESCE(pot.base_production_value,0) as ProductionCostTotal,
                  COALESCE(pot.base_transport_value,0) as TransportCostTotal,
                  COALESCE(pot.base_surcharge_value,0) as SurchargeCostTotal,
                  COALESCE(pot.base_miscellaneous_value,0) as MiscellaneousCostTotal,
                  COALESCE(pot.base_material_value,0) + 
                  COALESCE(pot.base_production_value,0) + 
                  COALESCE(pot.base_transport_value,0) + 
                  COALESCE(pot.base_surcharge_value,0) + 
                  COALESCE(pot.base_miscellaneous_value,0) AS TotalCost,
  
                  COALESCE(poi.allocated_pieces,0) as AllocatedPieces,
                  uomPieces.Code as AllocatedPiecesUnit,
                  CASE WHEN 
                      (uomQuantity.code = 'FT' OR 
                       uomQuantity.code = 'IN' OR 
                       uomQuantity.code = 'm' OR
                       uomQuantity.code = 'mm' OR
                       uomQuantity.code = 'cm') THEN COALESCE(poi.allocated_quantity,0) ELSE 0 
                  END AS AllocatedLength,
                  CASE WHEN 
                      (uomQuantity.code = 'FT' OR 
                       uomQuantity.code = 'IN' OR 
                       uomQuantity.code = 'm' OR
                       uomQuantity.code = 'mm' OR
                       uomQuantity.code = 'cm') THEN uomQuantity.Code ELSE ''
                  END AS AllocatedLengthUnit,
                  COALESCE(poi.allocated_quantity,0) as AllocatedQuantity ,
                  uomQuantity.code as AllocatedQuantityUnit,
                  COALESCE(poi.allocated_weight,0) as AllocatedWeight,
                  uomWeight.code as AllocatedWeightUnit,
  
                  COALESCE(poi.balance_pieces,0) as BalancePieces,
                  uomPieces.code as BalancePiecesUnit,
                  CASE WHEN 
                      (uomQuantity.code = 'FT' OR 
                       uomQuantity.code = 'IN' OR 
                       uomQuantity.code = 'm' OR
                       uomQuantity.code = 'mm' OR
                       uomQuantity.code = 'cm') THEN COALESCE(poi.balance_quantity,0) ELSE 0 
                  END AS BalanceLength,
                  CASE WHEN 
                      (uomQuantity.code = 'FT' OR 
                       uomQuantity.code = 'IN' OR 
                       uomQuantity.code = 'm' OR
                       uomQuantity.code = 'mm' OR
                       uomQuantity.code = 'cm') THEN uomQuantity.Code ELSE ''
                  END AS BalanceLengthUnit,
                  COALESCE(poi.balance_quantity,0) as BalanceQuantity ,
                  uomQuantity.code as BalanceQuantityUnit,
                  COALESCE(poi.balance_weight,0) as BalanceWeight,
                  uomWeight.code as BalanceWeightUnit,
  
                  COALESCE(poi.ordered_pieces,0) as OrderedPieces,
                  uomPieces.code as OrderedPiecesUnit,
                  CASE WHEN 
                      (uomQuantity.code = 'FT' OR 
                       uomQuantity.code = 'IN' OR 
                       uomQuantity.code = 'm' OR
                       uomQuantity.code = 'mm' OR
                       uomQuantity.code = 'cm') THEN COALESCE(poi.ordered_quantity,0) ELSE 0 
                  END AS OrderedLength,
                  CASE WHEN 
                      (uomQuantity.code = 'FT' OR 
                       uomQuantity.code = 'IN' OR 
                       uomQuantity.code = 'm' OR
                       uomQuantity.code = 'mm' OR
                       uomQuantity.code = 'cm') THEN uomQuantity.Code ELSE ''
                  END AS OrderedLengthUnit,
                  COALESCE(poi.ordered_quantity,0) as OrderedQuantity ,
                  uomQuantity.code as OrderedQuantityUnit,
                  COALESCE(poi.ordered_weight,0) as OrderedWeight,
                  uomWeight.code as OrderedWeightUnit,
  
  
                  COALESCE(poi.delivered_pieces,0) as DeliveredPieces,
                  uomPieces.code as DeliveredPiecesUnit,
                  CASE WHEN 
                      (uomQuantity.code = 'FT' OR 
                       uomQuantity.code = 'IN' OR 
                       uomQuantity.code = 'm' OR
                       uomQuantity.code = 'mm' OR
                       uomQuantity.code = 'cm') THEN COALESCE(poi.delivered_quantity,0) ELSE 0 
                  END AS DeliveredLength,
                  CASE WHEN 
                      (uomQuantity.code = 'FT' OR 
                       uomQuantity.code = 'IN' OR 
                       uomQuantity.code = 'm' OR
                       uomQuantity.code = 'mm' OR
                       uomQuantity.code = 'cm') THEN uomQuantity.Code ELSE ''
                  END AS DeliveredLengthUnit,
                  COALESCE(poi.delivered_quantity,0) as DeliveredQuantity,
                  uomQuantity.code as DeliveredQuantityUnit,
                  COALESCE(poi.delivered_weight,0) as DeliveredWeight,
                  uomWeight.code as DeliveredWeightUnit,
                  pot.base_material_value as BaseMaterialValue,
                  pot.base_production_value as BaseProductionValue,
                  pot.base_transport_value as BaseTransportValue,
                  pot.base_surcharge_value as BaseSurchargeValue,
                  pot.base_miscellaneous_value as BaseMiscellaneousValue

                FROM public.purchase_order_items poi
                  INNER JOIN public.products p ON poi.product_id = p.id
                  INNER JOIN public.product_categories pcat ON p.category_id = pcat.id
                  INNER JOIN public.dimension_values dim ON poi.dimensions_id = dim.id
                  INNER JOIN public.stock_grades sg ON p.grade_id = sg.id
                  INNER JOIN public.stock_analysis_codes sac1 ON pcat.analysis1_id = sac1.id
                  INNER JOIN public.stock_analysis_codes sac2 ON pcat.analysis2_id = sac2.id
                  INNER JOIN public.product_controls pc ON pcat.product_control_id = pc.id
                  INNER JOIN public.units_of_measure uomPieces ON pc.pieces_unit_id = uomPieces.id
                  INNER JOIN public.units_of_measure uomQuantity ON pc.quantity_unit_id = uomQuantity.id
                  INNER JOIN public.units_of_measure uomWeight ON pc.weight_unit_id = uomWeight.id
                  INNER JOIN public.purchase_order_totals pot ON poi.purchase_order_totals_id = pot.id
                  INNER JOIN public.purchase_order_headers poh ON poi.purchase_header_id = poh.id
                  INNER JOIN public.warehouses wh ON poh.delivery_warehouse_id = wh.id
                  INNER JOIN public.purchase_status_codes pos ON poi.status_id = pos.id
                  INNER JOIN public.companies c ON poh.supplier_id = c.id
                  INNER JOIN public.buyers b ON poh.buyer_id = b.id
                  LEFT OUTER JOIN public.purchase_category_codes pcode ON poh.category_id = pcode.id
                WHERE COALESCE(poi.balance_weight,0) > 0
            ";

            if (parameters != null)
            {
                var productId = parameters.FirstOrDefault(x => x.Key == "ProductId").Value;
                var productCode = parameters.FirstOrDefault(x => x.Key == "ProductCode").Value;
                var itemDueDate = parameters.FirstOrDefault(x => x.Key == "ItemDueDate").Value;
                if (productId != null)
                {
                    sql += $" AND p.id = {productId}";
                }
                if (productCode != null)
                {
                    sql += $" AND p.code = '{productCode}'";
                }
                if (itemDueDate != null)
                {
                    sql += $" AND poi.due_date > '{itemDueDate}'";
                }
            }

            var sqlQuery = new SqlQuery<PurchaseOrderItemsQuery>();
            return await sqlQuery.ExecuteQueryAsync(coid, sql);

        }
    }
}
