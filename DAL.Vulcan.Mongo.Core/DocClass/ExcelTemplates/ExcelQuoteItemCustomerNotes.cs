using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemCustomerNotes")]
    public class ExcelQuoteItemCustomerNotes : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemCustomerNotes()
        {
            FieldName = "CustomerNotes";
            Label = "Notes";
            Id = "8BFB77C6-76D7-4D78-9E8F-970BC9648415";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            return item.CustomerNotes;
        }
    }
}