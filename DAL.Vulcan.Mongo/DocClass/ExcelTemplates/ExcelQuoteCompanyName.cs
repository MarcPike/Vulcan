using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteCompanyName")]
    public class ExcelQuoteCompanyName: ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteCompanyName()
        {
            FieldName = "CompanyName";
            Label = "Company Name";
            Id = "C58EB714-9B84-4CA8-BD7C-F4183DDB37BA";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.Company?.Name ?? string.Empty;
        }
    }
}