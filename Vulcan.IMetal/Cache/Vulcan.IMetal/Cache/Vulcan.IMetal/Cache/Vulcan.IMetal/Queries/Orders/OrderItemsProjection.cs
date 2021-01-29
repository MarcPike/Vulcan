using Vulcan.IMetal.Models;

namespace Vulcan.IMetal.Queries.Orders
{
    public class OrderItemsProjection
    {
        public int SalesItemId { get; set; }
        public int ItemNumber { get; set; }
        public string ProductionCode { get; set; }

        public decimal RequiredQuantity { get; set; }
        public int RequiredPieces { get; set; }
        public decimal RequiredWeight { get; set; }

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

        public ItemMarginModel Margin { get; set; }

        public ItemCostModel Costs { get; set; }

        public ItemChargeModel Charges { get; set; }



    }
}