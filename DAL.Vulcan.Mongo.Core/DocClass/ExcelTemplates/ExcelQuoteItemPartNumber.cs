using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemPartNumber")]
    public class ExcelQuoteItemPartNumber : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemPartNumber()
        {
            FieldName = "PartNumber";
            Label = "Part Number";
            Id = "0F195242-8EE4-4932-967B-6DDA5D9AE150";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            return item.PartNumber;
        }
    }
}