using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteContactName")]
    public class ExcelQuoteContactName : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteContactName()
        {
            FieldName = "Contact";
            Label = "Contact";
            Id = "EA631F31-17D6-4096-B17B-D35BFBB7C721";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.Contact?.FullName;
        }
    }
}