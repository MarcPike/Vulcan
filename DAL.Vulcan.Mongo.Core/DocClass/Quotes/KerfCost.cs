namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class KerfCost
    {
        public int Cuts { get; set; }
        public decimal CostPerInch { get; set; }
        public decimal TheoWeight { get; set; }
        public decimal InchesPerCut { get; set; } = (decimal)0.25;
        public decimal TotalInches => Cuts * InchesPerCut;
        public decimal PoundsPerPiece => (InchesPerCut * TheoWeight);
        public decimal TotalPounds => (TotalInches * TheoWeight);
        public decimal TotalCost => (TotalInches * CostPerInch);

        public KerfCost(int cuts, decimal theoWeight)
        {
            Cuts = cuts;
            TheoWeight = theoWeight;
        }

        public KerfCost(int cuts, decimal theoWeight, decimal costPerInch)
        {
            Cuts = cuts;
            TheoWeight = theoWeight;
            CostPerInch = costPerInch;
        }

        public void CalculatedCost(decimal actCostPerInch)
        {
            CostPerInch = actCostPerInch;
        }
    }
}