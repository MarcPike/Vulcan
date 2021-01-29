namespace Vulcan.IMetal.Models
{
    public class ChargeCostModel
    {
        public decimal Charge { get; set; }
        public decimal Cost { get; set; }
        public decimal Margin => Cost - Charge;
        public decimal MarginPercent => (Charge == 0) ? 0 : 100 - (Cost / Charge) * 100;

        public ChargeCostModel(decimal charge, decimal cost)
        {
            Charge = charge;
            Cost = cost;
        }
    }
}