using System;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Quotes;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class QuickQuoteItem: BaseDocument
    {
        public string Coid { get; set; } = String.Empty;

        public CompetitorRef LostTo { get; set; } = null;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? LostDate { get; set; } = null;

        public LostReason LostReason { get; set; } = null;
        public bool IsLost => (LostReason != null);

        public string BaseCurrency
        {
            get
            {
                var currency = "USD";
                if (String.IsNullOrEmpty(Coid))
                {
                    return currency;
                }
                if (Coid == "EUR")
                    currency = "GBP";
                if (Coid == "CAN")
                    currency = "CAD";
                return currency;
            }
        }

        public OrderQuantity OrderQuantity { get; set; }
        public ProductMaster FinishedProduct { get; set; }
        public ProductMaster StartingProduct { get; set; }

        public RequiredQuantity RequiredQuantity
        {
            get
            {
                if ((FinishedProduct == null) || (OrderQuantity == null))
                {
                    OrderQuantity = new OrderQuantity(0,0,"in");
                    return null;
                }
                else
                {
                    return OrderQuantity.GetRequiredQuantity(Coid, FinishedProduct.TheoWeight);
                }
            }
        }

        public string Label { get; set; }

        public decimal Cost { get; set; }
        public decimal UnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
        public bool Regret { get; set; } = false;

        public string PartSpecification { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string LeadTime { get; set; } = string.Empty;
        public string OemType { get; set; } = string.Empty;

        public string SalesPersonNotes { get; set; } = string.Empty;
        public string CustomerNotes { get; set; } = string.Empty;


        public QuickQuoteItemRef AsQuickQuoteItemRef()
        {
            return new QuickQuoteItemRef(this);
        }


    }
}