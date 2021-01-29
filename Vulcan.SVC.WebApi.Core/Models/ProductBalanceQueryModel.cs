namespace Vulcan.SVC.WebApi.Core.Models
{
    public class ProductBalanceQueryModel
    {
        public string Coid { get; set; } = string.Empty;
        public bool HasAvailableInventory { get; set; } = false;
        public string StockGrade { get; set; } = string.Empty;
        public string MetalCategory { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;
        public string ProductCondition { get; set; } = string.Empty;
        public decimal OutsideDiameterMin { get; set; } = 0;
        public decimal OutsideDiameterMax { get; set; } = 0;
        public decimal InsideDiameterMin { get; set; } = 0;
        public decimal InsideDiameterMax { get; set; } = 0;
        public string ProductCode { get; set; } = string.Empty;

        public ProductBalanceQueryModel(string coid)
        {
            Coid = coid;
        }

        public ProductBalanceQueryModel()
        {
        }
    }
}
