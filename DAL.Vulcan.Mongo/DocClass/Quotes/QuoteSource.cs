using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public enum QuoteSource
    {
        StockItem,
        MadeUpCost,
        PurchaseOrderItem,
        QuickQuoteItem,
        NonStockItem,
        MachinedPart,
        CrozCalcItem
    }
}
