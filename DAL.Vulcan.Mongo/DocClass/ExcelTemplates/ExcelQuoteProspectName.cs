using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteProspectName")]
    public class ExcelQuoteProspectName: ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteProspectName()
        {
            FieldName = "ProspectName";
            Label = "Prospect Company Name";
            Id = "7841E257-2A53-40D6-9067-B1677AC938D5";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.Prospect?.Name ?? string.Empty;
        }
    }
}