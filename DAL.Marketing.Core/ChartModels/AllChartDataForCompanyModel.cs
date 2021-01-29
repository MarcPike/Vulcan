using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Marketing.Core.ChartModels
{
    public class AllChartDataForCompanyModel
    {
        public ChartQuoteHistoryModel TimelineData { get; set; }
        public TotalDollarsBySalesPersonChartData TotalDollarsBySalesPerson { get; set; }
        public TotalDollarsByMetalCategoryChartData TotalDollarsByMetalCategory { get; set; }

        public AllChartDataForCompanyModel()
        {
        }

        public AllChartDataForCompanyModel(ChartQuoteHistoryModel timelineData,
            TotalDollarsBySalesPersonChartData totalDollarsBySalesPerson,
            TotalDollarsByMetalCategoryChartData totalDollarsByMetalCategory)
        {
            TimelineData = timelineData;
            TotalDollarsBySalesPerson = totalDollarsBySalesPerson;
            TotalDollarsByMetalCategory = totalDollarsByMetalCategory;
        }

    }
}
