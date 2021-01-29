using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteShipToName")]
    public class ExcelQuoteShipToName : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteShipToName()
        {
            FieldName = "ShipToName";
            Label = "Ship To Name";
            Id = "E808CA3F-10EC-4A2B-B7D2-7DB0F7AAEE94";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.ShipToAddress?.Name;
        }
    }
}