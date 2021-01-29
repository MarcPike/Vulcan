using System;
using System.Collections.Generic;
using System.Linq;
using BLL.EMail;
using BLL.EMail.Core;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.Core.Analysis
{
    public static class ProductWinLossAnalysisBuilder
    {

        public static void AddQuote(CrmQuote quote)
        {
            try
            {
                var allItems = quote.Items.Where(x => !x.IsQuickQuoteItem).ToList();

                foreach (var crmQuoteItemRef in allItems)
                {
                    var quoteItem = crmQuoteItemRef.AsQuoteItem();
                    if (quoteItem.IsCrozCalc) continue;

                    if (quoteItem?.QuotePrice?.StartingProduct?.ProductCode == null) continue;

                    var pa = GetProductAnalysisObject(quoteItem.QuotePrice.StartingProduct.ProductCode);

                    pa.AddNewWinLossHistory(quote, quoteItem);

                    if (quoteItem.QuotePrice.StartingProduct.ProductCode ==
                        quoteItem.QuotePrice.FinishedProduct.ProductCode) continue;

                    pa = GetProductAnalysisObject(quoteItem.QuotePrice.FinishedProduct.ProductCode);
                    pa.AddNewWinLossHistory(quote, quoteItem);

                }

            }
            catch (Exception ex)
            {
                var emailSupport = new List<string>() {"isidro.gallegos@howcogroup.com","marc.pike@howcogroup.com"};
                EMailSupport.SendEMailToSupportException("ProductWinLossAnalysisBuilder.AddQuote Exception",emailSupport,ex);
            }

        }

        public static void AddTeamQuotes(Team team)
        {
            var teamQuotesWonOrLost = CrmQuote.Helper.Find(x =>
                    x.Team.Id == team.Id.ToString() &&
                    (x.Status != PipelineStatus.Draft ))
                .ToList();

            foreach (var crmQuote in teamQuotesWonOrLost)
            {
                AddQuote(crmQuote);
            }
        }

        public static void AddAllQuotes()
        {
            var allQuotes = CrmQuote.Helper.Find(x =>
                    x.Status != PipelineStatus.Draft)
                .ToList();

            foreach (var crmQuote in allQuotes)
            {
                AddQuote(crmQuote);
            }
        }


        public static ProductWinLossData GetProductAnalysisObject(string productCode)
        {

            var result =  ProductWinLossData.Helper.Find(x=>x.ProductCode == productCode).FirstOrDefault();
            if (result == null)
            {
                result = new ProductWinLossData() {ProductCode = productCode};
                ProductWinLossData.Helper.Upsert(result);
            }

            return result;
        }

    }
}