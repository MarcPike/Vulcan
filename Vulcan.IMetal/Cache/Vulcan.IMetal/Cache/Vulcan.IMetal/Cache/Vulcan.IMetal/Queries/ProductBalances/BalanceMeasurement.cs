namespace Vulcan.IMetal.Queries.ProductBalances
{
    public class BalanceMeasurement
    {
        public int Pieces { get; set; } = 0;
        public decimal Weight { get; set; } = 0;
        public decimal Quantity { get; set; } = 0;

        public BalanceMeasurement()
        {

        }

        public BalanceMeasurement(int? pieces, decimal? weight, decimal? quantity)
        {
            Pieces = pieces ?? 0;
            Weight = weight ?? 0;
            Quantity = quantity ?? 0;
        }
    }
}