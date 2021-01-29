using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Driver.Core.Operations;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SVC.QNG.Exporter.Models;
using Vulcan.IMetal.Helpers;

namespace SVC.QNG.Exporter.UpdateDataScripts
{
    [TestFixture()]
    public class FixCurrency
    {

        private RepositoryBase<CrmQuote> _quoteRep;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
            _quoteRep = new RepositoryBase<CrmQuote>();
        }

        [Test]
        public void ChangeDisplayCurrencyToActualDisplayCurrency()
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            using (var context = new ODSContext())
            {
                var onRow = 0;
                foreach (var qngQuote in context.Vulcan_CrmQuote.ToList())
                {
                    var crmQuote = GetVulcanQuote(qngQuote.ObjectId);
                    if (crmQuote == null) continue;

                    foreach (var qngQuoteItem in qngQuote.Vulcan_CrmQuoteItem.ToList())
                    {

                        var crmQuoteItem = GetVulcanQuoteItem(crmQuote, qngQuoteItem.QuoteItemId);
                        if (crmQuoteItem == null)
                        {
                            Console.WriteLine($"QuoteId: {qngQuote.QuoteId} No quote item for index == {qngQuoteItem.QuoteItemId}");
                            continue;
                        }

                        var correctDisplayCurrency = GetDisplayCurrencyForQuote(crmQuote);
                        var correctBaseCurrency = GetBaseCurrencyForQuoteItem(crmQuote, crmQuoteItem);

                        var baseCurrencyIsWrong = (qngQuoteItem.BaseCurrency != correctBaseCurrency);
                        var displayCurrencyIsWrong = (qngQuoteItem.DisplayCurrency != correctDisplayCurrency);
                        var exchangeRateIsWrong =
                            (correctBaseCurrency != correctDisplayCurrency) &&
                            ((qngQuoteItem.ExchangeRate == 1)) || (qngQuoteItem.ExchangeRate == 0);

                        if ( baseCurrencyIsWrong || displayCurrencyIsWrong || exchangeRateIsWrong )
                        {
                            if (baseCurrencyIsWrong || displayCurrencyIsWrong)
                            {
                                qngQuoteItem.BaseCurrency = correctBaseCurrency;
                                qngQuoteItem.DisplayCurrency = correctDisplayCurrency;

                            }

                            if (exchangeRateIsWrong)
                            {
                                qngQuoteItem.ExchangeRate =
                                    helperCurrency.ConvertValueFromCurrencyToCurrency(1, correctBaseCurrency, correctDisplayCurrency);
                            }
                            context.SaveChanges();
                        } 

                        ++onRow;

                    }
                }

            }

        }

        [Test]
        public void FixNullBaseCurrency()
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            using (var context = new ODSContext())
            {

                foreach (var qngQuoteItem in context.Vulcan_CrmQuoteItem.Where(x => x.BaseCurrency == null)
                    .ToList())
                {
                    var crmQuote = GetVulcanQuote(qngQuoteItem.QuoteId);
                    if (crmQuote == null)
                    {
                        Console.WriteLine($"QuoteId: {qngQuoteItem.QuoteId} No quote found");
                        context.Vulcan_CrmQuoteItem.Remove(qngQuoteItem);
                        context.SaveChanges();
                        continue;
                    }


                    var crmQuoteItem = GetVulcanQuoteItem(crmQuote, qngQuoteItem.QuoteItemId);
                    if (crmQuoteItem == null)
                    {
                        Console.WriteLine($"QuoteId: {crmQuote.QuoteId} No quote item for index == {qngQuoteItem.QuoteItemId}");
                        context.Vulcan_CrmQuoteItem.Remove(qngQuoteItem);
                        context.SaveChanges();
                        continue;
                    }

                    var correctBaseCurrency = GetBaseCurrencyForQuoteItem(crmQuote, crmQuoteItem);
                    qngQuoteItem.BaseCurrency = correctBaseCurrency;
                    context.SaveChanges();

                }

            }



        }


        private CrmQuote GetVulcanQuote(int quoteId)
        {
            return new RepositoryBase<CrmQuote>().AsQueryable().SingleOrDefault(x=>x.QuoteId == quoteId);
        }

        private CrmQuote GetVulcanQuote(string objectId)
        {
            return _quoteRep.Find(objectId);
        }

        private CrmQuoteItem GetVulcanQuoteItem(CrmQuote quote, int quoteItemIndex)
        {
            

            var quoteItem = quote?.Items.FirstOrDefault(x => x.Index == quoteItemIndex);
            if (quoteItem == null) return null;

            try
            {
                return quoteItem.AsQuoteItem();
            }
            catch (Exception)
            {
                Console.WriteLine($"Unable to get value for {quote.QuoteId}-{quoteItemIndex}");
                return null;
            }
        }

        private static string GetBaseCurrencyForQuoteItem(CrmQuote quote, CrmQuoteItem quoteItem)
        {
            var coid = quoteItem.Coid;
            if (quoteItem.QuoteSource == QuoteSource.StockItem)
            {
                coid = quoteItem.QuotePrice.StartingProduct.Coid;
            }
            else if (quoteItem.QuoteSource == QuoteSource.MachinedPart)
            {
                coid = quoteItem.MachinedPartModel.Coid;
            }
            else if (quoteItem.QuoteSource == QuoteSource.MadeUpCost)
            {
                coid = quoteItem.QuotePrice.StartingProduct.Coid;
            }
            else if (quoteItem.QuoteSource == QuoteSource.QuickQuoteItem)
            {
                coid = quote.Coid;
            }
            else if (quoteItem.QuoteSource == QuoteSource.NonStockItem)
            {
                coid = quoteItem.QuotePrice.StartingProduct.Coid;
            }
            else if (quoteItem.QuoteSource == QuoteSource.PurchaseOrderItem)
            {
                coid = quoteItem.QuotePrice.StartingProduct.Coid;
            }

            if (coid == "EUR") return "GBP";
            if (coid == "CAN") return "CAD";
            return "USD";
        }

        private static string GetDisplayCurrencyForQuote(CrmQuote quote)
        {
            var displayCurrency = quote.DisplayCurrency;

            if (String.IsNullOrEmpty(displayCurrency))
            {
                var coid = quote.Coid;

                if (coid == "EUR") displayCurrency = "GBP";
                if (coid == "CAN") displayCurrency = "CAD";
                displayCurrency = "USD";
            }

            return displayCurrency;
        }

    }


}
