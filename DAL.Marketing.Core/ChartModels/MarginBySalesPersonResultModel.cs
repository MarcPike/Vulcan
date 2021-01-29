namespace DAL.Marketing.Core.ChartModels
{
    public class MarginBySalesPersonResultModel
    {
        public MarginBySalesPersonChartData MaterialMargin { get; set; }
        public MarginBySalesPersonChartData SellingMargin { get; set; }

        public MarginBySalesPersonResultModel()
        {
        }

        public MarginBySalesPersonResultModel(MarginBySalesPersonChartData materialMargin, MarginBySalesPersonChartData sellingMargin)
        {
            MaterialMargin = materialMargin;
            SellingMargin = sellingMargin;
        }
    }
}