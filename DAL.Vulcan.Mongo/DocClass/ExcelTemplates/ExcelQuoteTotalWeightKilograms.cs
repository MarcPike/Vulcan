using System.Linq;
using DAL.Vulcan.Mongo.Extensions;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteTotalWeightKilograms")]
    public class ExcelQuoteTotalWeightKilograms : ExcelTemplateQuoteColumnBase
    {
        public ExcelQuoteTotalWeightKilograms()
        {
            FieldName = "WeightTotalKilos";
            Label = "Weight Total Kilograms";
            Id = "9ED2085A-53CC-46A5-8FB5-90BAD7CB1E45";
        }

        public override string GetValueFor(QuoteModel model)
        {
            decimal total = 0;
            foreach (var quoteItemModel in model.Items.Where(x=>!x.IsLost).ToList())
            {
                if (quoteItemModel.IsQuickQuoteItem)
                {
                    total  += quoteItemModel.QuickQuoteData?.RequiredQuantity?.TotalKilograms() ?? 0;
                } else if (quoteItemModel.IsMachinedPart)
                {
                    total += quoteItemModel.MachinedPartModel.Pieces *
                             quoteItemModel.MachinedPartModel.PieceWeightKilos;
                }
                else
                {
                    total += quoteItemModel.QuotePriceModel?.RequiredQuantity?.TotalKilograms() ?? 0;
                }
            }

            return $"{total.RoundAndNormalize(4)}kg";
        }
    }
}