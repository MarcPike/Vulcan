using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemLeadTime")]
    public class ExcelQuoteItemLeadTime : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemLeadTime()
        {
            FieldName = "LeadTime";
            Label = "Delivery/Lead Time";
            Id = "2AAC9577-14D7-41B9-A4C0-279534FEF428";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            return item.LeadTime;
        }
    }
}