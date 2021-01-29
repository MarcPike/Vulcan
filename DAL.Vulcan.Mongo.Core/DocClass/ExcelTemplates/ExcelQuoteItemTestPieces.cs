using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.Extensions;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemTestPieces")]
    public class ExcelQuoteItemTestPieces : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemTestPieces()
        {
            FieldName = "TestPieces";
            Label = "TestPieces";
            Id = "BED5D8B3-8695-4E29-B9A4-0A1C4FAD29D5";
            MultipleColumns = true;
        }

        private int MaxPieces = 0;
        private int Columns = 3;

        public override List<string> GetLabelsFor(QuoteModel model)
        {
            var result = new List<string>();
            SetMaxPieces(model);
            foreach (var quoteItemModel in model.Items)
            {
                if ((quoteItemModel.IsQuickQuoteItem) || (quoteItemModel.IsMachinedPart) || (quoteItemModel.IsCrozCalc) ||
                    (!quoteItemModel.CalculateQuotePriceModel.TestPieces.Any()))
                {
                    continue;
                }

            }

            for (int i = 1; i <= MaxPieces; i++)
            {
                result.Add($"Test Piece {i} Count");
                result.Add($"Test Piece {i} Quantity");
                result.Add($"Test Piece {i} Price");
            }

            return result;
        }

        private void SetMaxPieces(QuoteModel model)
        {
            foreach (var item in model.Items)
            {
                if (item.CalculateQuotePriceModel != null && item.CalculateQuotePriceModel.TestPieces.Any())
                {
                    if (item.CalculateQuotePriceModel.TestPieces.Count > MaxPieces)
                    {
                        MaxPieces = item.CalculateQuotePriceModel.TestPieces.Count;
                    }
                }
            }
        }

        private void GetDefaultValue(List<string> result)
        {
            if (result.Count < (MaxPieces * Columns))
            {
                for (int i = 0; i < MaxPieces; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        result.Add(string.Empty);
                    }
                }
            }
        }

        public override List<string> GetValuesFor(QuoteModel model, int index)
        {
            var result = new List<string>();

            if (MaxPieces == 0) return result;
            var quoteItemModel = GetQuoteItemModel(model, index);

            if ((quoteItemModel.IsQuickQuoteItem) || (quoteItemModel.IsMachinedPart) ||
                (!quoteItemModel.CalculateQuotePriceModel.TestPieces.Any())) 
            {
                GetDefaultValue(result);
                return result;
            }

            foreach (var testPiece in quoteItemModel.CalculateQuotePriceModel.TestPieces)
            {
                var quantity = $"{testPiece.RequiredQuantity.TotalInches()} inches";
                var count = testPiece.RequiredQuantity.Pieces.ToString();
                var price = quoteItemModel.CalculateQuotePriceModel.MaterialPriceValue.TestPiecesTotalPrice
                    .RoundAndNormalize(2).ToString("F");

                result.Add(count);
                result.Add(quantity);
                result.Add(price);
            }

            return result;
        }
    }

}
