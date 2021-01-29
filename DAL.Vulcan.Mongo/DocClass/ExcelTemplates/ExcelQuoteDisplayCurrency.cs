using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteDisplayCurrency")]
    public class ExcelQuoteDisplayCurrency : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteDisplayCurrency()
        {
            FieldName = "DisplayCurrency";
            Label = "Order Total Currency";
            Id = "DF1DFFB6-7D0A-48C1-B8CA-0F7244328C34";
        }

        public override string GetValueFor(QuoteModel model)
        {
            return model.DisplayCurrency;
        }
    }
}