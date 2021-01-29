using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelTemplateQuoteColumnBase")]
    public class ExcelTemplateQuoteColumnBase
    {
        public string Id { get; set; }
        public string FieldName { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;

        public virtual string GetValueFor(QuoteModel model)
        {
            return string.Empty;
        }
    }
}