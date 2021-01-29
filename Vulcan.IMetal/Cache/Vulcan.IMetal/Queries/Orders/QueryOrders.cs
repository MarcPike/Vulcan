using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devart.Data.Linq;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Orders;
using Vulcan.IMetal.Models;
using Vulcan.IMetal.Results;
using Vulcan.IMetal.ViewFilterObjects;

namespace Vulcan.IMetal.Queries.Orders
{
    public class QueryOrderCapture
    {
        public SalesHeader SalesHeader { get; set; }
        public IEnumerable<SalesItem> SalesItems { get; set; }
        public Company Company { get; set; }
        public SalesType SalesType { get; set; }
        public IEnumerable<CostItem> CostItems { get; set; }
        public IEnumerable<SalesCharge> SalesCharges { get; set; }
        public string SalesPerson { get; set; }
    }


    public class QueryOrders : QueryBase<QueryOrderCapture>
    {
        public string QueryCoid { get; set; }
        public OrdersDataContext Context;

        public FilterOrderTypeCode OrderTypeCode { get; set; } = new FilterOrderTypeCode();
        public FilterDueDate DueDate { get; set; } = new FilterDueDate();
        public FilterSaleDate SaleDate { get; set; } = new FilterSaleDate();
        public FilterOrderNumber OrderNumber { get; set; } = new FilterOrderNumber();
        public int CompanyId { get; set; } = Int32.MinValue;

        public FilterCompanyCode CompanyCode { get; set; } = new FilterCompanyCode();

        public QueryOrders(string coid, OrdersDataContext context = null) : base(coid)
        {
            Context = context ?? ContextFactory.GetOrdersContextForCoid(coid);

        }

        public IQueryable<QueryOrderCapture> GetAsQueryable()
        {
            var orders = BuildQueryable();

            //var orders = Context.SalesHeader
            //    .LoadWith(x => x.SalesType)
            //    //.LoadWith(x => x.SalesItem)
            //    //.LoadWith(x => x.SalesTotal_BalanceTotalId)
            //    //.LoadWith(x => x.Branch_BranchId)
            //    //.LoadWith(x => x.Branch_DeliveryBranchId)
            //    //.LoadWith(x => x.DeliveryWarehouseId)
            //    //.LoadWith(x => x.UnitsOfMeasure_TransportCostRateUnitId)
            //    //.LoadWith(x => x.UnitsOfMeasure_TransportChargeRateUnitId)
            //    //.LoadWith(x => x.Company_CustomerId)
            //    //.LoadWith(x => x.CompanySubAddress)
            //    //.LoadWith(x => x.Address_CustomerAddressId)
            //    //.LoadWith(x => x.Address_DeliverToAddressId)
            //    //.LoadWith(x => x.CurrencyCode)
            //    //.LoadWith(x => x.Contact)
            //    //.LoadWith(x => x.SalesGroupQuery)
            //    .Where(x => x.Discriminator == "O").AsQueryable();

            orders = OrderTypeCode.ApplyFilter(orders);
            orders = CompanyCode.ApplyFilter(orders);
            orders = DueDate.ApplyFilter(orders);
            orders = SaleDate.ApplyFilter(orders);
            orders = OrderNumber.ApplyFilter(orders);

            if (CompanyId != Int32.MinValue)
            {
                orders = orders.Where(x => x.Company.Id == CompanyId).AsQueryable();
            }

            return orders;
        }

        private IQueryable<QueryOrderCapture> BuildQueryable()
        {
            var q =
                from h in Context.SalesHeader 
                join c in Context.Company on h.CustomerId equals c.Id
                join st in Context.SalesType on h.TypeId equals st.Id
                join s in Context.Personnel on h.InsideSalespersonId equals s.Id
               
                join i in Context.SalesItem on h.Id equals i.SalesHeaderId 
                join ci in Context.CostItem on i.Id equals ci.ItemId into costItems
                join sc in Context.SalesCharge on i.Id equals sc.ItemId into salesCharges
                select new QueryOrderCapture()
                {
                    SalesHeader = h,
                    SalesPerson = s.Name,
                    Company = c,
                    SalesType = st,
                    SalesItems = h.SalesItem,
                    CostItems = costItems,
                    SalesCharges = salesCharges
                };
               

            return q;
        }

        //public override List<SalesHeader> Execute()
        //{
        //    return GetAsQueryable().ToList();
        //}

        
    }

}
