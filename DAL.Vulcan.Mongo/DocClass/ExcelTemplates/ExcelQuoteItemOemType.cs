using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemOemType")]
    public class ExcelQuoteItemOemType : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemOemType()
        {
            FieldName = "OemType";
            Label = "OEM";
            Id = "49B0B74C-4BF2-4716-8D26-C6DFF2DA2551";

        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            return item.OemType;
        }
    }
}