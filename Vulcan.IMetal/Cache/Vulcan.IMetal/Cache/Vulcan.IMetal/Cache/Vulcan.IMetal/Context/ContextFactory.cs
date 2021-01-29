using System;
using System.Collections.Generic;
using System.Linq;
using Devart.Data.Linq;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.Context.Orders;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.Context.StockItems;

namespace Vulcan.IMetal.Context
{
    public static class ContextFactory
    {
        private class OrderDataContextForCoidItem
        {
            public string Coid { get; set; }
            public OrdersDataContext Context { get; set; } 
        }

        private static List<OrderDataContextForCoidItem> OrderContextList { get; } = new List<OrderDataContextForCoidItem>();

        private static readonly Dictionary<string, string> ConnectionStrings = new Dictionary<string, string>()
        {
            {"INC","Host=s-us-imetalus;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Schema=public;Keepalive=10000;" },
            //{"INC","Host=10.100.20.23;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Schema=public;Keepalive=1000;" },
            {"DUB","Host=s-us-imetaldu;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Schema=public;Keepalive=10000;" },
            {"MSA","Host=s-us-imetalma;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Schema=public;Keepalive=10000;" },
            {"SIN","Host=10.5.20.20;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Schema=public;Keepalive=10000;" },
            {"EUR","Host=s-us-imetaleu;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Schema=public;Keepalive=10000;" },
            {"CAN","Host=172.30.48.48;Port=11061;User Id=reporter;Password=ooTai1ph;Database=live_emetal;Schema=public;Keepalive=10000;" },
        };

        public static StockItemsContext GetStockItemsContextForCoid(string coid)
        {
            var connectionString = ConnectionStrings.FirstOrDefault(x => x.Key == coid).Value;
            if (connectionString == null)
            {
                throw new Exception("No connection string defined for " + coid);
            }
            var context = new StockItemsContext(connectionString) {ObjectTrackingEnabled = false};


            return context;
        }

        public static CompanyContext GetCompanyContextForCoid(string coid)
        {
            var connectionString = ConnectionStrings.FirstOrDefault(x => x.Key == coid).Value;
            if (connectionString == null)
            {
                throw new Exception("No connection string defined for " + coid);
            }
            var context = new CompanyContext(connectionString) {ObjectTrackingEnabled = false};
            return context;
        }

        public static PurchaseOrdersContext GetPurchaseOrdersContextForCoid(string coid)
        {
            var connectionString = ConnectionStrings.FirstOrDefault(x => x.Key == coid).Value;
            if (connectionString == null)
            {
                throw new Exception("No connection string defined for " + coid);
            }
            var context = new PurchaseOrdersContext(connectionString) {ObjectTrackingEnabled = false};

            return context;
        }

        public static OrdersDataContext GetOrdersContextForCoid(string coid, bool forceNew = false)
        {
            
            if ((OrderContextList.All(x=>x.Coid != coid)) || forceNew)
            {
                var connectionString = ConnectionStrings.FirstOrDefault(x => x.Key == coid).Value;
                if (connectionString == null)
                {
                    throw new Exception("No connection string defined for " + coid);
                }
                var context = new OrdersDataContext(connectionString) {ObjectTrackingEnabled = false};
                var orderContext = new OrderDataContextForCoidItem()
                {
                    Context = context,
                    Coid = coid
                };
                OrderContextList.Add(orderContext);

            }

            return OrderContextList.Last(x=>x.Coid == coid).Context;
        }

        public static GeneralInfoContext GetGeneralInfoContextForCoid(string coid, bool forceNew = false)
        {
            var connectionString = ConnectionStrings.FirstOrDefault(x => x.Key == coid).Value;
            if (connectionString == null)
            {
                throw new Exception("No connection string defined for " + coid);
            }
            var context = new GeneralInfoContext(connectionString) { ObjectTrackingEnabled = false };
            return context;
        }

    }
}
