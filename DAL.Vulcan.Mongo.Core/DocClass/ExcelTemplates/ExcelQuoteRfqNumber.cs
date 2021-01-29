using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteRfqNumber")]
    public class ExcelQuoteRfqNumber : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteRfqNumber()
        {
            FieldName = "RfqNumber";
            Label = "Customer RFQ#";
            Id = "E84D5272-B5A5-4DE6-A284-932CC9F8484B";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.RfqNumber;
        }
    }
}