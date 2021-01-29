using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Quotes;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class CreateNewMadeupCostModel
    {
        public string Coid { get; set; }
        public decimal OuterDiameter { get; set; }
        public decimal InsideDiameter { get; set; }
        public string MetalCategory { get; set; }
        public string ProductCondition { get; set; }
        public decimal CostPerPound { get; set; }
        public decimal CostPerKilogram { get; set; }
        public OrderQuantity OrderQuantity { get; set; } = new OrderQuantity(0,0,"in");
        public string Application { get; set; }
        public string UserId { get; set; }
        public string ProductCode { get; set; } = "<NEW PRODUCT>";
        public string ProductCategory { get; set; } = string.Empty;
        public string StockGrade { get; set; } = string.Empty;
        public QuoteSource QuoteSource { get; set; } = QuoteSource.MadeUpCost;

        public int ProductIdForNonStockItem { get; set; } = 0;

        public string DisplayCurrency { get; set; } 

        public string CompanyId { get; set; }

        public CreateNewMadeupCostModel()
        {

        }

        public MadeUpCost AsMadeUpCost()
        {
            MadeUpCost madeUpCost = null;

            if (QuoteSource == QuoteSource.NonStockItem)
            {
                madeUpCost = MadeUpCost.FromNonStockItem(Coid,ProductIdForNonStockItem,CostPerPound,CostPerKilogram,OrderQuantity, DisplayCurrency);
            }
            else
            {
                madeUpCost = MadeUpCost.CreateNew(
                    Coid, OuterDiameter, InsideDiameter,
                    MetalCategory, ProductCondition,
                    CostPerPound, CostPerKilogram, OrderQuantity,ProductCode, DisplayCurrency, ProductCategory, StockGrade);
            }

            return madeUpCost;
        }
    }
}
