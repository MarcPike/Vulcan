using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteValidityDate")]
    public class ExcelQuoteValidityDate : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteValidityDate()
        {
            FieldName = "ValidityDate";
            Label = "Validity End Date";
            Id = "0944202D-C638-49DF-B399-966F93446A5C";
        }

        public override string GetValueFor(QuoteModel model)
        {
            if (model.LostDate != null) return string.Empty;

            if (model.SubmitDate == null) return string.Empty;

            var validityDate = model.SubmitDate.Value.AddDays(model.ValidityDays);
            return validityDate.ToString("D");

        }
    }
}