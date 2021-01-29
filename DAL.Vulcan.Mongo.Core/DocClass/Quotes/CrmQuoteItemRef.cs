using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    [BsonIgnoreExtraElements]
    public class CrmQuoteItemRef : ReferenceObject<CrmQuoteItem>
    {
        public int Index { get; set; }
        public string ProductCode { get; set; }
        public bool IsQuickQuoteItem { get; set; } = false;
        public ItemSummaryViewModel ItemSummaryViewModel { get; set; } = null;

        public CrmQuoteItemRef()
        {
        }

        public CrmQuoteItemRef(CrmQuoteItem doc) : base(doc)
        {
            IsQuickQuoteItem = doc.IsQuickQuoteItem;
            ItemSummaryViewModel = new ItemSummaryViewModel(doc);
        }

        public CrmQuoteItem AsQuoteItem()
        {
            return ToBaseDocument();
        }

    }
}
