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
using Vulcan.IMetal.Queries.Orders;

namespace Vulcan.IMetal.Context.Orders
{

    public partial class SalesItem
    {

        public ItemCostModel GetCosts(string coid, OrdersDataContext context, IEnumerable<CostItem> costItems)
        {
            ItemCostModel result;
            context = context ?? ContextFactory.GetOrdersContextForCoid(coid);

            if (costItems == null)
            {
                var costs = context.CostItem
                    .Where(x => x.ItemId == Id &&
                                new[] {"MAT", "PRD", "TRN", "MSC", "SUR"}.Contains(x.CostGroupCode.Code) &&
                                x.Visibility == "S")
                    .Select(x => new {x.CostGroupCode.Code, x.BaseValue}).ToList();


                result = new ItemCostModel()
                {
                    Material = costs.Where(x => x.Code == "MAT").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Transport = costs.Where(x => x.Code == "TRN").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Miscellaneous = costs.Where(x => x.Code == "MSC").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Production = costs.Where(x => x.Code == "PRD").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Surcharge = costs.Where(x => x.Code == "SUR").Select(x => x.BaseValue).FirstOrDefault() ?? 0
                };

            }
            else
            {
                var costs = costItems
                    .Where(x => x.ItemId == Id &&
                                new[] { "MAT", "PRD", "TRN", "MSC", "SUR" }.Contains(x.CostGroupCode.Code) &&
                                x.Visibility == "S")
                    .Select(x => new { x.CostGroupCode.Code, x.BaseValue }).ToList();


                result = new ItemCostModel()
                {
                    Material = costs.Where(x => x.Code == "MAT").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Transport = costs.Where(x => x.Code == "TRN").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Miscellaneous = costs.Where(x => x.Code == "MSC").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Production = costs.Where(x => x.Code == "PRD").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Surcharge = costs.Where(x => x.Code == "SUR").Select(x => x.BaseValue).FirstOrDefault() ?? 0
                };

            }

            return result;
        }

        public ItemChargeModel GetCharges(string coid, OrdersDataContext context, IEnumerable<SalesCharge> salesCharges)
        {
            ItemChargeModel result;
            if (salesCharges == null)
            {
                context = context ?? ContextFactory.GetOrdersContextForCoid(coid);
                var charges  = context.SalesCharge
                    .Where(x => x.ItemId == Id && new[] { "MAT", "PRD", "TRN", "MSC", "SUR" }.Contains(x.CostGroupCode) && x.ChargeVisibility == "S")
                    .Select(x => new { x.CostGroupCode, x.BaseValue }).ToList();
                result = new ItemChargeModel()
                {
                    Material = charges.Where(x => x.CostGroupCode == "MAT").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Transport = charges.Where(x => x.CostGroupCode == "TRN").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Miscellaneous = charges.Where(x => x.CostGroupCode == "MSC").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Production = charges.Where(x => x.CostGroupCode == "PRD").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Surcharge = charges.Where(x => x.CostGroupCode == "SUR").Select(x => x.BaseValue).FirstOrDefault() ?? 0
                };

            }
            else
            {
                var charges = salesCharges
                    .Where(x => x.ItemId == Id && new[] { "MAT", "PRD", "TRN", "MSC", "SUR" }.Contains(x.CostGroupCode) && x.ChargeVisibility == "S")
                    .Select(x => new { x.CostGroupCode, x.BaseValue }).ToList();
                result = new ItemChargeModel()
                {
                    Material = charges.Where(x => x.CostGroupCode == "MAT").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Transport = charges.Where(x => x.CostGroupCode == "TRN").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Miscellaneous = charges.Where(x => x.CostGroupCode == "MSC").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Production = charges.Where(x => x.CostGroupCode == "PRD").Select(x => x.BaseValue).FirstOrDefault() ?? 0,
                    Surcharge = charges.Where(x => x.CostGroupCode == "SUR").Select(x => x.BaseValue).FirstOrDefault() ?? 0
                };
            }
            return result;
        }


    }


}
