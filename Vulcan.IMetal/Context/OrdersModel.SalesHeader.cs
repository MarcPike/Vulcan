using System;
using Devart.Data.Linq;
using Devart.Data.Linq.Mapping;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using Vulcan.IMetal.Models;

namespace Vulcan.IMetal.Context.Orders
{

    public partial class SalesHeader
    {
        public ItemCostModel GetTotalCosts(string coid, OrdersDataContext context, IEnumerable<SalesItem> items, IEnumerable<CostItem> costItems)
        {
            context = context ?? ContextFactory.GetOrdersContextForCoid(coid);
            var result = new ItemCostModel();

            if (items == null)
            {
                foreach (var item in SalesItem)
                {
                    var costs = item.GetCosts(coid,context,costItems);
                    costs.AddTo(result);
                }
            }
            else
            {
                foreach (var item in items)
                {
                    var costs = item.GetCosts(coid,context,costItems);
                    costs.AddTo(result);
                }
                
            }

            return result;
        }
        public ItemChargeModel GetTotalCharges(string coid, OrdersDataContext context, IEnumerable<SalesItem> salesItems = null, IEnumerable<SalesCharge> salesCharges = null)
        {
            context = context ?? ContextFactory.GetOrdersContextForCoid(coid);
            var result = new ItemChargeModel();

            if (salesItems == null)
            {
                foreach (var item in SalesItem)
                {
                    var charges = item.GetCharges(coid, context, salesCharges);
                    charges.AddTo(result);
                }
            }
            else
            {
                foreach (var item in salesItems)
                {
                    var charges = item.GetCharges(coid, context, salesCharges);
                    charges.AddTo(result);
                }
            }

            return result;

        }

        public ItemMarginModel GetTotalMargin(
            string coid,
            OrdersDataContext context,
            IEnumerable<SalesItem> items,
            IEnumerable<CostItem> costItems, 
            IEnumerable<SalesCharge> salesCharges
            )
        {
            context = context ?? ContextFactory.GetOrdersContextForCoid(coid);
            var charges = GetTotalCharges(coid, context, items, salesCharges);
            var costs = GetTotalCosts(coid, context, items, costItems);

            return new ItemMarginModel(
                costs.Total, charges.Total,
                costs.Material, charges.Material,
                costs.Production, charges.Production,
                costs.Transport, charges.Transport,
                costs.Miscellaneous, charges.Miscellaneous,
                costs.Surcharge, charges.Surcharge
                );
        }
    }
}
