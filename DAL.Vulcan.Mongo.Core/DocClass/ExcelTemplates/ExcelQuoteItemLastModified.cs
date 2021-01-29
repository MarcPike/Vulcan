using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemLastModified")]
    public class ExcelQuoteItemLastModified : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemLastModified()
        {
            FieldName = "LastModified";
            Label = "Last Modified";
            Id = "99D30A89-A254-42B6-9FFF-4B6840BFB0CB";

        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            return item.ModifiedDateTime.ToString("g");
        }
    }
}