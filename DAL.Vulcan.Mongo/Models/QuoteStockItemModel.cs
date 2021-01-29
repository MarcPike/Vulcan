using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Quotes;
using Vulcan.IMetal.Queries.StockItems;

namespace DAL.Vulcan.Mongo.Models
{
    public class QuoteStockItemModel
    {
        public string Application { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Coid { get; set; }
        public ProductMaster StartingProduct { get; set; }
        public ProductMaster FinishedProduct { get; set; }
        public OrderQuantity OrderQuantity { get; set; } = new OrderQuantity(1, 1, "in");
        public StockItemsAdvancedQuery StockItem { get; set; }
        public string DisplayCurrency { get; set; }
        public string CompanyId { get; set; }

    }
}