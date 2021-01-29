using System;
using System.Collections.Generic;
using System.Linq;

namespace Vulcan.IMetal.Queries.Orders
{
    public class OrderDetailsProjection {
        public string Coid { get; set; }
        public int SalesHeaderId { get; set; }
        public int OrderNumber { get; set; }
        public int OrderVersion { get; set; }
        public DateTime OrderDueDate { get; set; }
        public DateTime OrderSaleDate { get; set; }
        public DateTime OrderCreated { get; set; }
        public DateTime OrderModied { get; set; }
        public string OrderSaleTypeCode { get; set; }
        public string OrderSaleTypeDescription { get; set; }
        public string OrderSaleTypeStatus { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyShortName { get; set; }
        public string SalesPerson { get; set; }
        public List<OrderItemsProjection> SalesItems { get; set; } = new List<OrderItemsProjection>();

        public static OrderDetailsProjection GenerateProjectionFromList(List<OrderDetailsAdvancedQuery> order)
        {
            var firstItem = order.First();
            var result = new OrderDetailsProjection()
            {
                Coid = firstItem.Coid,
                OrderDueDate = firstItem.OrderDueDate,
                SalesHeaderId = firstItem.SalesHeaderId,
                CompanyCode = firstItem.CompanyCode,
                CompanyName = firstItem.CompanyName,
                CompanyShortName = firstItem.CompanyShortName,
                OrderCreated = firstItem.OrderCreated,
                OrderModied = firstItem.OrderModied,
                OrderNumber = firstItem.OrderNumber,
                OrderSaleDate = firstItem.OrderSaleDate,
                OrderSaleTypeCode = firstItem.OrderSaleTypeCode,
                OrderSaleTypeDescription = firstItem.OrderSaleTypeDescription,
                OrderSaleTypeStatus = firstItem.OrderSaleTypeStatus,
                OrderVersion = firstItem.OrderVersion,
                SalesPerson = firstItem.SalesPerson
            };
            foreach (var orderItem in order.OrderBy(x=>x.ItemNumber).ToList())
            {
                result.SalesItems.Add(new OrderItemsProjection()
                {
                    SalesItemId = orderItem.SalesItemId,
                    MiscellaneousCosts = orderItem.MiscellaneousCosts,
                    TransportCosts = orderItem.TransportCosts,
                    ProductionCosts = orderItem.ProductionCosts,
                    SurchargeCosts = orderItem.SurchargeCosts,
                    MaterialCosts = orderItem.MaterialCosts,
                    ProductionCharges = orderItem.ProductionCharges,
                    TransportCharges = orderItem.TransportCharges,
                    MaterialCharges = orderItem.MaterialCharges,
                    SurchargeCharges = orderItem.SurchargeCharges,
                    MiscellaneousCharges = orderItem.MiscellaneousCharges,
                    ItemNumber = orderItem.ItemNumber,
                    ProductionCode = orderItem.ProductionCode,
                    RequiredPieces = orderItem.RequiredPieces,
                    RequiredQuantity = orderItem.RequiredQuantity,
                    RequiredWeight = orderItem.RequiredWeight,
                    Margin = orderItem.Margin,
                    Charges = orderItem.Charges,
                    Costs = orderItem.Costs
                });
            }
            return result;
        }

    }
}