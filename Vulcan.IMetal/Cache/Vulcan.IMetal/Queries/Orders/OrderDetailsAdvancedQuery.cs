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
    public class OrderDetailsAdvancedQuery
    {
        public string Coid { get; set; }
        public int SalesHeaderId { get; set; }

        public int OrderNumber { get; set; }
        public int OrderVersion { get; set; }
        public DateTime OrderDueDate { get; set; }
        public DateTime OrderSaleDate { get; set; }
        public DateTime OrderCreated { get; set; }
        public DateTime OrderModied { get; set; }
        public IEnumerable<SalesItem> Items { get; set; }
        public string OrderSaleTypeCode { get; set; }
        public string OrderSaleTypeDescription { get; set; }
        public string OrderSaleTypeStatus { get; set; }
        public int SalesItemId { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string SalesPerson { get; set; }

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

        public int ItemNumber { get; set; }
        public string ProductionCode { get; set; }

        public decimal RequiredQuantity { get; set; }
        public int RequiredPieces { get; set; }
        public decimal RequiredWeight { get; set; }

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


        public static OrderDetailsProjection GetProjectionForOrderNumber(string coid, int orderNumber, OrdersDataContext context = null)
        {
            context = context ?? ContextFactory.GetOrdersContextForCoid(coid, true);
            var query =
            (
                from h in context.SalesHeader
                join c in context.Company on h.CustomerId equals c.Id
                join dta in context.Address on h.DeliverToAddressId equals dta.Id
                join da in context.Address on h.DeliveryAddressId equals da.Id
                join st in context.SalesType on h.TypeId equals st.Id
                join s in context.Personnel on h.InsideSalespersonId equals s.Id

                join i in context.SalesItem on h.Id equals i.SalesHeaderId
                where h.Number == orderNumber
                select new OrderDetailsAdvancedQuery()
                {
                    Coid = coid,
                    SalesHeaderId = h.Id,
                    OrderNumber = h.Number ?? 0,
                    OrderVersion = h.Version ?? 0,
                    OrderDueDate = h.DueDate ?? new DateTime(1980, 1, 1),
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

                    SalesItemId = i.Id,
                    ItemNumber = i.ItemNumber ?? 0,
                    ProductionCode = i.Product.Code,

                    RequiredQuantity = i.RequiredQuantity ?? 0,
                    RequiredPieces = i.RequiredPiece ?? 0,
                    RequiredWeight = i.RequiredWeight ?? 0,

                    MaterialCosts = (context.CostItem.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode.Code == "MAT").Sum(ci => ci.BaseValue ?? 0)),
                    ProductionCosts = (context.CostItem.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode.Code == "PRD").Sum(ci => ci.BaseValue ?? 0)),
                    TransportCosts = (context.CostItem.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode.Code == "TRN").Sum(ci => ci.BaseValue ?? 0)),
                    MiscellaneousCosts = (context.CostItem.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode.Code == "MSC").Sum(ci => ci.BaseValue ?? 0)),
                    SurchargeCosts = (context.CostItem.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode.Code == "SUR").Sum(ci => ci.BaseValue ?? 0)),

                    MaterialCharges = (context.SalesCharge.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode == "MAT").Sum(ci => ci.BaseValue ?? 0)),
                    ProductionCharges = (context.SalesCharge.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode == "PRD").Sum(ci => ci.BaseValue ?? 0)),
                    TransportCharges = (context.SalesCharge.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode == "TRN").Sum(ci => ci.BaseValue ?? 0)),
                    MiscellaneousCharges = (context.SalesCharge.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode == "MSC").Sum(ci => ci.BaseValue ?? 0)),
                    SurchargeCharges = (context.SalesCharge.Where(ci => ci.ItemId == i.Id && ci.CostGroupCode == "SUR").Sum(ci => ci.BaseValue ?? 0)),
                }).AsQueryable();

            return OrderDetailsProjection.GenerateProjectionFromList(query.ToList());
        }
    }
}
