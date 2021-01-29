using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
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