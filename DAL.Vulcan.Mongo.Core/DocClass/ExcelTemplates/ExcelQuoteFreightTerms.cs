using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteFreightTerms")]
    public class ExcelQuoteFreightTerms : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteFreightTerms()
        {
            FieldName = "FreightTerm";
            Label = "Freight Terms";
            Id = "80FAC97C-D1E1-4C8C-A6DD-B3697177B3E1";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.FreightTerm;
        }
    }
}