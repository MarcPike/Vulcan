using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Marketing.ChartModels
{
    public class MarginByMetalCategoryResultModel
    {
        public MarginByMetalCategoryChartData MaterialMargin { get; set; }
        public MarginByMetalCategoryChartData SellingMargin { get; set; }

        public MarginByMetalCategoryResultModel()
        {
        }

        public MarginByMetalCategoryResultModel(MarginByMetalCategoryChartData materialMargin, MarginByMetalCategoryChartData sellingMargin)
        {
            MaterialMargin = materialMargin;
            SellingMargin = sellingMargin;
        }
    }
}
