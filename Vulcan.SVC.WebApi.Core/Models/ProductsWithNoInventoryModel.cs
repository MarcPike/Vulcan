namespace Vulcan.SVC.WebApi.Core.Models
{
    public class ProductsWithNoInventoryModel
    {
        public string Coid { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSize { get; set; }
        public string ProductCondition { get; set; }
        public decimal OuterDiameter { get; set; }
        public decimal InsideDiameter { get; set; }
        public string MetalType { get; set; }
        public string StockGrade { get; set; }
        public string StratificationRank { get; set; }
    }
}
