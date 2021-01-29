using DAL.Vulcan.Mongo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Marketing.ChartModels
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
