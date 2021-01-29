using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class QuoteTotal
    {
        public decimal TotalCost { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0;
        public decimal TotalMargin { get; set; } = 0;
        public decimal TotalKerf { get; set; } = 0;
        public decimal AdditionalCosts { get; set; } = 0;
        public decimal AdditionalPrice { get; set; } = 0;

        public QuoteTotal()
        {
        }

        public static QuoteTotal GetQuoteTotal(CrmQuote quote)
        {
            var items = quote.Items.Select(x => x.AsQuoteItem()).ToList();
            //var quickQuoteItems = quote.QuickQuoteItems.Select(x => x.AsQuickQuoteItem()).ToList();
            return new QuoteTotal(items, false);
        }

        public static QuoteTotal GetQuoteTotal(List<CrmQuoteItemRef> itemRefs)
        {
            var items = itemRefs.Select(x => x.AsQuoteItem()).ToList();
            return new QuoteTotal(items, false);
        }

        public QuoteTotal(List<CrmQuoteItem> items, bool includeLost)
        {
            if (items == null) return;
            if (items.Count == 0) return;

            try
            {
                CalculateNormalItems();

                CalculateQuickQuoteItems();

                CalculateMachinedParts();

                CalculateCrozCalcItems();

                if (TotalPrice > 0)
                {
                    TotalMargin = QuoteCalculations.GetMargin(TotalCost, TotalPrice);
                }

            }
            catch (Exception e)
            {
                var itemNotNull = items.FirstOrDefault(x => x != null);
                if (itemNotNull != null)
                {
                    var quoteId = itemNotNull.GetQuote().QuoteId;
                    Console.WriteLine($"QuoteID: {quoteId} has this problem:");
                    Console.WriteLine(e);

                }
                else
                {
                    Console.WriteLine(e);
                }
                throw;
            }

            
            void CalculateNormalItems()
            {
                var normalItems = items.Where(x => !x.IsQuickQuoteItem).ToList();

                if (includeLost)
                {
                    TotalCost = normalItems.Where(x=> x.QuotePrice != null).Sum(x => x.QuotePrice.TotalCost);
                    TotalPrice = normalItems.Where(x => x.QuotePrice != null).Sum(x => x.QuotePrice.FinalPrice);
                    TotalKerf = normalItems.Where(x => x.QuotePrice != null).Sum(x => x.QuotePrice.MaterialCostValue.KerfTotalCost);
                }
                else
                {
                    TotalCost = normalItems.Where(x => x.QuotePrice != null && !x.IsLost).Sum(x => x.QuotePrice.TotalCost);
                    TotalPrice = normalItems.Where(x => x.QuotePrice != null && !x.IsLost).Sum(x => x.QuotePrice.FinalPrice);
                    TotalKerf = normalItems.Where(x => x.QuotePrice != null && !x.IsLost).Sum(x => x.QuotePrice.MaterialCostValue.KerfTotalCost);
                }

                var additionalCostTotal = (decimal) 0;
                var additionalPriceTotal = (decimal) 0;
                foreach (var quoteItem in normalItems.Where(x => x.QuotePrice != null && !x.IsLost).ToList())
                {
                    foreach (var additionalCost in quoteItem.QuotePrice.ProductionCosts.Where(x => x.IsPriceBlended == false)
                        .ToList())
                    {
                        additionalCostTotal += additionalCost.CostValues.Sum(x => x.InternalCost);
                        additionalPriceTotal += additionalCost.CostValues.Sum(x => x.ProductionCost);
                    }
                }

                AdditionalCosts = additionalCostTotal;
                AdditionalPrice = additionalPriceTotal;
            }

            void CalculateQuickQuoteItems()
            {
                var quickQuoteItems = items.Where(x => x.IsQuickQuoteItem).ToList();
                foreach (var quickQuoteItem in quickQuoteItems)
                {
                    if (quickQuoteItem.QuickQuoteData == null) continue;


                    if ((!quickQuoteItem.QuickQuoteData.Regret) && (!quickQuoteItem.IsLost))
                    //if (!quickQuoteItem.QuickQuoteData.Regret)
                    {
                        TotalCost += quickQuoteItem.QuickQuoteData.Cost;
                        TotalPrice += quickQuoteItem.QuickQuoteData.Price;
                    }
                }
            }

            void  CalculateMachinedParts()
            {
                var machinedParts = items.Where(x => x.IsMachinedPart).ToList();
                foreach (var machinedPart in machinedParts)
                {
                    if (machinedPart.MachinedPartModel == null) continue;


                    TotalCost += machinedPart.MachinedPartModel.TotalCost;
                    TotalPrice += machinedPart.MachinedPartModel.TotalPrice;
                }
            }
            
            void CalculateCrozCalcItems()
            {
                var crozCalcs = items.Where(x => x.IsCrozCalc).ToList();
                foreach (var crmQuoteItem in crozCalcs)
                {
                    TotalCost += crmQuoteItem.CrozCalcItem?.TotalCost ?? 0;
                    TotalPrice += crmQuoteItem.CrozCalcItem?.TotalPrice ?? 0;
                }
            }

        }



        //public QuoteTotal(List<QuoteItemModel> items, List<QuickQuoteItem> quickQuoteItems, bool includeLost)
        //{
        //    if (items.Count == 0) return;

        //    if (includeLost)
        //    {
        //        TotalCost = items.Sum(x => x.QuotePriceModel.TotalCost);
        //        TotalPrice = items.Sum(x => x.QuotePriceModel.FinalPrice);
        //    }
        //    else
        //    {
        //        TotalCost = items.Where(x => !x.IsLost).Sum(x => x.QuotePriceModel.TotalCost);
        //        TotalPrice = items.Where(x => !x.IsLost).Sum(x => x.QuotePriceModel.FinalPrice);
        //    }
        //    var additionalCostTotal = (decimal) 0;
        //    var additionalPriceTotal = (decimal)0;
        //    foreach (var quoteItem in items.Where(x => !x.IsLost).ToList())
        //    {
        //        foreach (var additionalCost in quoteItem.QuotePriceModel.ProductionCosts.Where(x=>x.IsPriceBlended == false).ToList())
        //        {
        //            additionalCostTotal += additionalCost.CostValues.Sum(x => x.InternalCost);
        //            additionalPriceTotal += additionalCost.CostValues.Sum(x => x.ProductionCost);
        //        }
        //    }

        //    foreach (var quickQuoteItem in quickQuoteItems.Where(x => !x.IsLost).ToList())
        //    {
        //        additionalCostTotal += (quickQuoteItem.Cost > 0) ? quickQuoteItem.Cost : quickQuoteItem.Price;
        //        additionalPriceTotal += quickQuoteItem.Price;
        //    }

        //    AdditionalCosts = additionalCostTotal;
        //    AdditionalPrice = additionalPriceTotal;


        //    if (TotalPrice > 0)
        //    {
        //        TotalMargin = QuoteCalculations.GetMargin(TotalCost, TotalPrice);
        //    }
        //}
    }
}