using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteQuoteId")]
    public class ExcelQuoteQuoteId : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteQuoteId()
        {
            FieldName = "QuoteId";
            Label = "Quote #";
            Id = "FBA8A85C-96A8-4C8D-8402-9976AB24FEDF";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.QuoteId.ToString();
        }
    }
}