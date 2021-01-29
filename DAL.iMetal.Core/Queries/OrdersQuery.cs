using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.iMetal.Core.DbUtilities;
using DAL.iMetal.Core.Models;
using Vulcan.IMetal.Queries.Companies;

namespace DAL.iMetal.Core.Queries
{
    public class OrdersQuery : BaseQuery<OrdersQuery>
    {
        public string Coid { get; set; }
        public int OrderNumber { get; set; }
        public int OrderVersion { get; set; }
        public DateTime OrderDueDate { get; set; }
        public DateTime OrderSaleDate { get; set; }
        public DateTime OrderCreated { get; set; }
        public DateTime OrderModied { get; set; }
        public string OrderSaleTypeCode { get; set; }
        public string OrderSaleTypeDescription { get; set; }
        public string OrderSaleTypeStatus { get; set; }
        public int SalesItemId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }

        public decimal MaterialCosts { get; set; }
        public decimal ProductionCosts { get; set; }
        public decimal TransportCosts { get; set; }
        public decimal MiscellaneousCosts { get; set; }
        public decimal SurchargeCosts { get; set; }

        public decimal TotalCosts =>
            MaterialCosts + ProductionCosts + TransportCosts + MiscellaneousCosts + SurchargeCosts;

        public decimal MaterialCharges { get; set; }
        public decimal ProductionCharges { get; set; }
        public decimal TransportCharges { get; set; }
        public decimal MiscellaneousCharges { get; set; }
        public decimal SurchargeCharges { get; set; }

        public decimal TotalCharges =>
            MaterialCharges + ProductionCharges + TransportCharges + MiscellaneousCharges + SurchargeCharges;

        public ItemMarginModel Margin
        {
            get
            {
                var result = new ItemMarginModel(TotalCosts, TotalCharges, MaterialCosts, MaterialCharges, ProductionCosts, ProductionCharges, TransportCosts, TransportCharges, MiscellaneousCosts, MiscellaneousCharges, SurchargeCosts, SurchargeCharges);
                return result;
            }
        }

        public ItemCostModel Costs => new ItemCostModel()
        {
            Material = MaterialCosts,
            Miscellaneous = MiscellaneousCosts,
            Surcharge = SurchargeCosts,
            Transport = TransportCosts,
            Production = ProductionCosts
        };

        public ItemChargeModel Charges => new ItemChargeModel()
        {
            Material = MaterialCharges,
            Miscellaneous = MiscellaneousCharges,
            Surcharge = SurchargeCharges,
            Transport = TransportCharges,
            Production = ProductionCharges
        };

        public string SalesPerson { get; set; }


        public static async Task<List<OrdersQuery>> ExecuteAsync(string coid)
        {
            var sqlQuery = new SqlQuery<OrdersQuery>();
            var result = await sqlQuery.ExecuteQueryAsync(coid,
                $@"
                SELECT 
                  h.number AS OrderNumber,
                  h.version AS OrderVersion,
                  h.due_date AS OrderDueDate,
                  h.sale_date AS OrderSaleDate,
                  h.cdate AS OrderCreated,
                  h.mdate AS OrderModified,
                  st.code AS OrderSaleTypeCode,
                  st.status AS OrderSaleTypeStatus,
                  st.Description AS OrderSaleTypeDescription,
                  s.name AS SalesPerson,
                  c.code AS CompanyCode,
                  c.name AS CompanyName,
                  c.short_name AS CompanyShortName,
                  SUM(CASE WHEN csgc.code = 'MAT' THEN ci.base_value END) AS MaterialCosts,
                  SUM(CASE WHEN csgc.code = 'PRD' THEN ci.base_value END) AS ProductionCosts,
                  SUM(CASE WHEN csgc.code = 'TRN' THEN ci.base_value END) AS TransportCosts,
                  SUM(CASE WHEN csgc.code = 'MSC' THEN ci.base_value END) AS MiscellaneousCosts,
                  SUM(CASE WHEN csgc.code = 'SUR' THEN ci.base_value END) AS SurchargeCosts,
                  SUM(CASE WHEN csgc2.code = 'MAT' THEN sc.base_value END) AS MaterialCharges,
                  SUM(CASE WHEN csgc2.code = 'PRD' THEN sc.base_value END) AS ProductionChargest,
                  SUM(CASE WHEN csgc2.code = 'TRN' THEN sc.base_value END) AS TransportCharges,
                  SUM(CASE WHEN csgc2.code = 'MSC' THEN sc.base_value END) AS MiscellaneousCharges,
                  SUM(CASE WHEN csgc2.code = 'SUR' THEN sc.base_value END) AS SurchargeCharges
                FROM sales_headers h 
                JOIN companies c ON h.customer_id = c.Id
                JOIN sales_types st ON h.type_id = st.Id
                JOIN personnel s ON h.inside_salesperson_id = s.Id

                LEFT OUTER JOIN sales_items i ON h.Id = i.sales_header_id
                LEFT OUTER JOIN cost_items ci ON i.Id = ci.item_id
                LEFT OUTER JOIN sales_charges sc ON i.Id = sc.item_id 
                LEFT OUTER JOIN cost_group_codes csgc ON ci.cost_group_id = csgc.id
                LEFT OUTER JOIN cost_group_codes csgc2 ON sc.sales_charge_type_id = csgc2.id
                GROUP BY   h.number,
                  h.version,
                  h.due_date,
                  h.sale_date,
                  h.cdate,
                  h.mdate,
                  st.code,
                  st.status,
                  st.Description,
                  s.name,
                  c.code,
                  c.name,
                  c.short_name 
                ");

            return result.ToList();
        }
    }
}
