using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuotePoNumber")]
    public class ExcelQuotePoNumber : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuotePoNumber()
        {
            FieldName = "PoNumber";
            Label = "PO#";
            Id = "1528B826-2CB9-4D42-8C67-DED95A3B69AF";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.PoNumber;
        }
    }
}