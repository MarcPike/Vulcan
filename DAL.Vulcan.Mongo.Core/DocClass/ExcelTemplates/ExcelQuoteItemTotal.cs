using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Extensions;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemTotal")]
    public class ExcelQuoteItemTotal : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemTotal()
        {
            FieldName = "TotalPrice";
            Label = "Line Item Total";
            Id = "34767B17-5F22-4E34-858A-91C7377AB627";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            var crmQuoteItem = CrmQuoteItem.Helper.FindById(item.Id);
            return crmQuoteItem.TotalPrice.RoundAndNormalize(3).ToString("0.000");
        }
    }
}