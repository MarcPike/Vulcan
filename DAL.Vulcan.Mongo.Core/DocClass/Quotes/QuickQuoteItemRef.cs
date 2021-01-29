using DAL.Vulcan.Mongo.Base.Core.DocClass;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class QuickQuoteItemRef : ReferenceObject<QuickQuoteItem>
    {
        public string Label { get; set; }
        public QuickQuoteItemRef()
        {

        }

        public QuickQuoteItemRef(QuickQuoteItem doc) : base(doc)
        {
            Label = doc.Label;
        }


        public QuickQuoteItem AsQuickQuoteItem()
        {
            return ToBaseDocument();
        }

    }
}