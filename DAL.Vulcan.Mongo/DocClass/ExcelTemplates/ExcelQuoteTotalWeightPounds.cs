using System.Linq;
using DAL.Vulcan.Mongo.Extensions;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteTotalWeightPounds")]
    public class ExcelQuoteTotalWeightPounds : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteTotalWeightPounds()
        {
            FieldName = "WeightTotalPounds";
            Label = "Weight Total Pounds";
            Id = "88D17974-E155-4182-B071-7A06BD43B9C2";
        }

        public override string GetValueFor(QuoteModel model)
        {
            decimal total = 0;
            foreach (var quoteItemModel in model.Items.Where(x => !x.IsLost).ToList())
            {
                if (quoteItemModel.IsQuickQuoteItem)
                {
                    total += quoteItemModel.QuickQuoteData?.RequiredQuantity?.TotalPounds() ?? 0;
                }
                else if (quoteItemModel.IsMachinedPart)
                {
                    total += quoteItemModel.MachinedPartModel.Pieces *
                             quoteItemModel.MachinedPartModel.PieceWeightLbs;
                }
                else
                {
                    total += quoteItemModel.QuotePriceModel?.RequiredQuantity?.TotalPounds() ?? 0;
                }
            }

            return $"{total.RoundAndNormalize(4)}lbs";
        }
    }
}