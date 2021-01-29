using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteSubmitTime")]
    public class ExcelQuoteSubmitTime : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteSubmitTime()
        {
            FieldName = "SubmitTime";
            Label = "Quote Submitted Time";
            Id = "C8A6A352-A204-436C-8727-DAE176A94C2A";
        }

        public override string GetValueFor(QuoteModel model)
        {
            if (model.SubmitDate == null) return string.Empty;

            var submitTime = model.SubmitDate.Value.TimeOfDay;
            return submitTime.ToString("g");

        }
    }
}