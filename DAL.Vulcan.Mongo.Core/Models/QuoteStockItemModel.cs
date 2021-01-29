using DAL.iMetal.Core.Queries;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Quotes;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class QuoteStockItemModel
    {
        public string Application { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Coid { get; set; }
        public ProductMaster StartingProduct { get; set; }
        public ProductMaster FinishedProduct { get; set; }
        public OrderQuantity OrderQuantity { get; set; } = new OrderQuantity(1, 1, "in");
        public StockItemsQuery StockItem { get; set; }
        public string DisplayCurrency { get; set; }
        public string CompanyId { get; set; }

    }
}