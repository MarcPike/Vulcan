using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteOrderTotal")]
    public class ExcelQuoteOrderTotal : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteOrderTotal()
        {
            FieldName = "OrderTotal";
            Label = "Order Total";
            Id = "4CA690CA-032D-4543-921E-CD23B7C16C70";
        }

        public override string GetValueFor(QuoteModel model)
        {
            var quoteTotal = model.QuoteTotal?.TotalPrice ?? 0;

            return quoteTotal.ToString("N");
        }
    }

}