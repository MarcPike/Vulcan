using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteRevision")]
    public class ExcelQuoteRevision : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteRevision()
        {
            FieldName = "Revision";
            Label = "Quote Rev #";
            Id = "965E77A7-97C4-47EB-9817-1E2A86775421";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.RevisionNumber.ToString();
        }
    }
}