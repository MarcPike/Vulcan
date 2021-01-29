using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemFinishedProduct")]
    public class ExcelQuoteItemFinishedProduct : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemFinishedProduct()
        {
            FieldName = "FinishedProductCode";
            Label = "Finished Product";
            Id = "A44DA7ED-0305-42C2-AD09-B2CCA40D3DE1";
        }

        public override string GetValueFor(QuoteModel model, int index)
        {
            var item = GetQuoteItemModel(model, index);
            if (item.IsQuickQuoteItem)
            {
                return item.QuickQuoteData.FinishedProduct?.ProductCode ?? string.Empty;
            }
            else if (item.IsMachinedPart)
            {
                return item.MachinedPartModel.MachinedPart?.ProductCode ?? string.Empty;
            }

            return item.QuotePriceModel.FinishedProduct?.ProductCode ?? string.Empty;
        }
    }
}