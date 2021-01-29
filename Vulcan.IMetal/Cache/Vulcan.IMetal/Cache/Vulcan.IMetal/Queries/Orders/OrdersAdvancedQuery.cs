using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Orders;
using Vulcan.IMetal.Models;

namespace Vulcan.IMetal.Queries.Orders
{
    public class OrdersAdvancedQuery
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


        public static IQueryable<OrdersAdvancedQuery> AsQueryable(string coid, OrdersDataContext context = null)
        {
            //var costGroupCodes = new List<string> {"MAT", "PRD", "TRN", "MSC", "SUR"};
            context = context ?? ContextFactory.GetOrdersContextForCoid(coid, true);
            var q =
            (
                from h in context.SalesHeader
                join c in context.Company on h.CustomerId equals c.Id
                join st in context.SalesType on h.TypeId equals st.Id
                join s in context.Personnel on h.InsideSalespersonId equals s.Id

                join i in context.SalesItem on h.Id equals i.SalesHeaderId 
                join ci in context.CostItem on i.Id equals ci.ItemId into costItems
                join sc in context.SalesCharge on i.Id equals sc.ItemId into salesCharges
               
                select new OrdersAdvancedQuery()
                {
                    Coid = coid,
                    OrderNumber = h.Number ?? 0,
                    OrderVersion = h.Version ?? 0,
                    OrderDueDate = h.DueDate ?? new DateTime(1980,1,1),
                    OrderSaleDate = h.SaleDate ?? new DateTime(1980, 1, 1),
                    OrderCreated = h.Cdate ?? new DateTime(1980, 1, 1),
                    OrderModied = h.Mdate ?? new DateTime(1980, 1, 1),
                    OrderSaleTypeCode = st.Code,
                    OrderSaleTypeStatus = st.Status,
                    OrderSaleTypeDescription = st.Description,
                    SalesPerson = s.Name,
                    CompanyCode = c.Code,
                    CompanyName = c.Name,
                    CompanyShortName = c.ShortName,
                    MaterialCosts =  costItems.Where(x => x.CostGroupCode.Code == "MAT").Sum(x=>x.BaseValue ?? 0),
                    ProductionCosts = costItems.Where(x => x.CostGroupCode.Code == "PRD").Sum(x => x.BaseValue ?? 0),
                    TransportCosts = costItems.Where(x => x.CostGroupCode.Code == "TRN").Sum(x => x.BaseValue ?? 0),
                    MiscellaneousCosts = costItems.Where(x => x.CostGroupCode.Code == "MSC").Sum(x => x.BaseValue ?? 0),
                    SurchargeCosts = costItems.Where(x => x.CostGroupCode.Code == "SUR").Sum(x => x.BaseValue ?? 0),
                    MaterialCharges = salesCharges.Where(x => x.CostGroupCode == "MAT").Sum(x => x.BaseValue ?? 0),
                    ProductionCharges = salesCharges.Where(x => x.CostGroupCode == "PRD").Sum(x => x.BaseValue ?? 0),
                    TransportCharges = salesCharges.Where(x => x.CostGroupCode == "TRN").Sum(x => x.BaseValue ?? 0),
                    MiscellaneousCharges = salesCharges.Where(x => x.CostGroupCode == "MSC").Sum(x => x.BaseValue ?? 0),
                    SurchargeCharges = salesCharges.Where(x => x.CostGroupCode == "SUR").Sum(x => x.BaseValue ?? 0),
                }).AsQueryable();


            return q;

        }

    }
}
