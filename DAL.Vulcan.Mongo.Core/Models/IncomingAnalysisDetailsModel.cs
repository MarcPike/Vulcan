using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.Models
{



    public class IncomingAnalysisDetailsModel
    {
        public List<IncomingAnalysisDataModel> Orders { get; set; } = new List<IncomingAnalysisDataModel>();

        public int Count => Orders.Count;

        public decimal MaxCostPerPound;
        public decimal MinCostPerPound;
        public decimal AvgCostPerPound;

        public decimal MaxCostPerInch;
        public decimal MinCostPerInch;
        public decimal AvgCostPerInch;

        public decimal MaxCostPerKilogram;
        public decimal MinCostPerKilogram;
        public decimal AvgCostPerKilogram;


        public IncomingAnalysisDetailsModel(List<IncomingAnalysisDataModel> orders)
        {
            Orders = orders ?? new List<IncomingAnalysisDataModel>();

            if (Orders.Any())
            {
                Calculate();
            }

        }

        private void Calculate()
        {
            MinCostPerPound = Orders?.Min(x => x.CostPerLb) ?? 0;
            MaxCostPerPound = Orders?.Max(x => x.CostPerLb) ?? 0;
            AvgCostPerPound = Orders?.Average(x => x.CostPerLb) ?? 0;

            MinCostPerInch = Orders?.Min(x => x.CostPerInch) ?? 0;
            MaxCostPerInch = Orders?.Max(x => x.CostPerInch) ?? 0;
            AvgCostPerInch = Orders?.Average(x => x.CostPerInch) ?? 0;

            MinCostPerKilogram = Orders?.Min(x => x.CostPerKg) ?? 0;
            MaxCostPerKilogram = Orders?.Max(x => x.CostPerKg) ?? 0;
            AvgCostPerKilogram = Orders?.Average(x => x.CostPerKg) ?? 0;

        }
    }
}