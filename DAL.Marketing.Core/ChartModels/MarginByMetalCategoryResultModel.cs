namespace DAL.Marketing.Core.ChartModels
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
