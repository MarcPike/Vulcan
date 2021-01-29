using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteSubmitDate")]
    public class ExcelQuoteSubmitDate : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteSubmitDate()
        {
            FieldName = "SubmitDate";
            Label = "Quote Submitted Date";
            Id = "7AE738C8-DCF3-4D8D-9C8F-83E46908E9E3";
        }

        public override string GetValueFor(QuoteModel model)
        {
            if (model.SubmitDate == null) return string.Empty;

            var submitDate = model.SubmitDate.Value.Date;
            return submitDate.ToString("D");

        }
    }
}