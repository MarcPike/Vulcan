using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteOrderDate")]
    public class ExcelQuoteOrderDate : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteOrderDate()
        {
            FieldName = "ReportDate";
            Label = "Report Order Date";
            Id = "74CB8885-9091-40A3-957E-4554F1B13555";
        }

        public override string GetValueFor(QuoteModel model)
        {
            var reportDate = model.WonDate ?? model.LostDate ?? model.SubmitDate ?? model.CreateDateTime;
            return reportDate.ToString("D");

        }
    }
}