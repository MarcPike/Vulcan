using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteValidity")]
    public class ExcelQuoteValidity : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteValidity()
        {
            FieldName = "Validity";
            Label = "Validity Days";
            Id = "4363A866-4422-4733-823D-66E0BFBAA148";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.ValidityDays.ToString();

        }
    }
}