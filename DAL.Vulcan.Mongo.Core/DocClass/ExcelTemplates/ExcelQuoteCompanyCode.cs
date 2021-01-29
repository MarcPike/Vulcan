using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteCompanyCode")]
    public class ExcelQuoteCompanyCode : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteCompanyCode()
        {
            FieldName = "CompanyCode";
            Label = "Customer Account Number";
            Id = "FCFC4D7F-F6A0-4E9C-B394-EAE06D4C41A0";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.Company?.Code;
        }
    }
}