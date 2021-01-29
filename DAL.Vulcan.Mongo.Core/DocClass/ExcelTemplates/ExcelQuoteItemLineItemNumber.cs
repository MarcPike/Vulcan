using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemLineItemNumber")]
    public class ExcelQuoteItemLineItemNumber: ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemLineItemNumber()
        {
            FieldName = "ItemNumber";
            Label = "Item #";
            Id = "74096A7A-23A7-451F-837E-89C7C434255E";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            return item.Index.ToString();
        }
    }
}