using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.Extensions;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.ExcelTemplates
{
    [BsonDiscriminator("ExcelQuoteItemProductionSteps")]
    public class ExcelQuoteItemProductionSteps : ExcelTemplateQuoteItemColumnBase
    {
        public ExcelQuoteItemProductionSteps()
        {
            FieldName = "ProductionSteps";
            Label = "Production Steps";
            Id = "166E7678-53DA-474B-B81F-C129E11CC50C";
            MultipleColumns = true;
        }

        private int MaxSteps = 0;
        private int Columns = 2;

        public override List<string> GetLabelsFor(QuoteModel model)
        {
            var result = new List<string>();
            SetMaxSteps(model);
            foreach (var quoteItemModel in model.Items)
            {
                if ((quoteItemModel.IsQuickQuoteItem) || (quoteItemModel.IsMachinedPart) || (quoteItemModel.IsCrozCalc) ||
                    (!quoteItemModel.CalculateQuotePriceModel.ProductionCosts.Any())) continue;

            }

            for (int i = 1; i <= MaxSteps; i++)
            {
                result.Add($"Step {i}");
                result.Add($"Step {i} Price");
            }

            return result;
        }

        private void SetMaxSteps(QuoteModel model)
        {
            foreach (var item in model.Items)
            {
                if (item.CalculateQuotePriceModel != null && item.CalculateQuotePriceModel.ProductionCosts.Any())
                {
                    if (MaxSteps < item.CalculateQuotePriceModel.ProductionCosts.Count)
                    {
                        MaxSteps = item.CalculateQuotePriceModel.ProductionCosts.Count;
                    }
                }
            }
        }

        public override List<string> GetValuesFor(QuoteModel model, int index)
        {
            var result = new List<string>();
            if (MaxSteps == 0) return result;
            var quoteItemModel = GetQuoteItemModel(model, index);

            if ((quoteItemModel.IsQuickQuoteItem) || (quoteItemModel.IsMachinedPart) || (quoteItemModel.IsCrozCalc) ||
                (!quoteItemModel.CalculateQuotePriceModel.ProductionCosts.Any()))
            {
                GetDefaultValue(result);
                return result;
            }

            foreach (var productionStep in quoteItemModel.CalculateQuotePriceModel.ProductionCosts)
            {
                var step = productionStep.ResourceType.ToString();
                var value = productionStep.ProductionPrice + productionStep.ProductionPriceMargin;
                result.Add($"{step}");
                result.Add($"{value.RoundAndNormalize(2):F}");
            }

            return result;
        }

        private void GetDefaultValue(List<string> result)
        {
            if (result.Count < (MaxSteps * Columns))
            {
                for (int i = 0; i < MaxSteps; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        result.Add(string.Empty);
                    }
                }
            }
        }
    }
}