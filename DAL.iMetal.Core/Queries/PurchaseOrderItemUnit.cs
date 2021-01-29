namespace DAL.iMetal.Core.Queries
{
    public class PurchaseOrderItemUnit
    {

        public string PiecesUnit { get; set; } = string.Empty;
        public string LengthUnit { get; set; } = string.Empty;
        public string WeightUnit { get; set; } = string.Empty;
        public string QuantityUnit { get; set; } = string.Empty;
        public int Pieces { get; set; } = 0;
        public decimal Quantity { get; set; } = 0;
        public decimal Length { get; set; } = 0;
        public decimal Weight { get; set; } = 0;
        public decimal WeightLbs { get; set; } = 0;
        public decimal WeightKgs { get; set; } = 0;

    }
}