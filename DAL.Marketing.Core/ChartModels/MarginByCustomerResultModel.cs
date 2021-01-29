namespace DAL.Marketing.Core.ChartModels
{
    public class MarginByCustomerResultModel
    {
        public MarginByCustomerChartData MaterialMargin { get; set; }
        public MarginByCustomerChartData SellingMargin { get; set; }

        public MarginByCustomerResultModel()
        {
        }

        public MarginByCustomerResultModel(MarginByCustomerChartData materialMargin, MarginByCustomerChartData sellingMargin)
        {
            MaterialMargin = materialMargin;
            SellingMargin = sellingMargin;
        }
    }
}