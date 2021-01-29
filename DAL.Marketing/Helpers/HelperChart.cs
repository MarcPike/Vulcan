using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Marketing.Docs;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Marketing.Helpers
{
    public class HelperChart : IHelperChart
    {
        public List<QuoteDataForQuoteHistory> GetQuoteHistoryForTeamAndCompany(string teamId, string companyId)
        {
            var rep = new RepositoryBase<CrmQuote>();
            var quotes = rep.AsQueryable().Where(x => x.Team.Id == teamId && x.Company.Id == companyId)
                .Select(x => new QuoteDataForQuoteHistory(x.QuoteId, x.ReportDate, x.Status, x.Items, x.QuickQuoteItems, x.SalesPerson, x.Company))
                .ToList();
            return quotes;
        }

        public List<QuoteDataForQuoteHistory> GetQuoteHistoryForTeam(string teamId)
        {
            var rep = new RepositoryBase<CrmQuote>();

            var quotes = rep.AsQueryable().Where(x => x.Team.Id == teamId)
                .Select(x => new QuoteDataForQuoteHistory(x.QuoteId, x.ReportDate, x.Status, x.Items, x.QuickQuoteItems, x.SalesPerson, x.Company))
                .ToList();
            return quotes;
        }

        public List<QuoteDataForQuoteHistory> GetQuoteHistoryForAllQuotes(string coid)
        {
            var rep = new RepositoryBase<CrmQuote>();
            var quotes = rep.AsQueryable().Where(x => x.Coid == coid)
                .Select(x => new QuoteDataForQuoteHistory(x.QuoteId, x.ReportDate, x.Status, x.Items, x.QuickQuoteItems, x.SalesPerson, x.Company))
                .ToList();
            return quotes;
        }

        public List<QuoteDataForQuoteHistory> GetAllQuotesHistoryForCompany(string coid, string companyId)
        {
            var rep = new RepositoryBase<CrmQuote>();
            var quotes = rep.AsQueryable().Where(x => x.Coid == coid && x.Company.Id == companyId)
                .Select(x => new QuoteDataForQuoteHistory(x.QuoteId, x.ReportDate, x.Status, x.Items, x.QuickQuoteItems, x.SalesPerson, x.Company))
                .ToList();
            return quotes;
        }

        public List<QuoteDataForQuoteHistory> GetAllQuoteHistoryForStrategicAccount(string strategicAccountId)
        {
            var strategicAccount = new RepositoryBase<MarketingAccount>().Find(strategicAccountId);
            var quotes = new List<QuoteDataForQuoteHistory>();
            var companies = strategicAccount.MarketingAccountFolder.GetAllCompanies();
            var rep = new RepositoryBase<CrmQuote>();
            foreach (var companyRef in companies)
            {
                quotes.AddRange(rep.AsQueryable().Where(x => x.Company.Id == companyRef.Id)
                    .Select(x => new QuoteDataForQuoteHistory(x.QuoteId, x.ReportDate, x.Status, x.Items, x.QuickQuoteItems, x.SalesPerson, x.Company))
                    .ToList());
            }

            return quotes;
        }

        public void CalculateAndSortModel(ChartQuoteHistoryModel model)
        {
            model.CalculateStatusTotals();

            model.Draft = model.Draft.OrderBy(x => x.X).ToList();
            model.Active = model.Active.OrderBy(x => x.X).ToList();
            model.Won = model.Won.OrderBy(x => x.X).ToList();
            model.Lost = model.Lost.OrderBy(x => x.X).ToList();
            model.Expired = model.Expired.OrderBy(x => x.X).ToList();
        }


        public ChartQuoteHistoryModel GetChartHistoryModelForQuotes(List<QuoteDataForQuoteHistory> quotes)
        {
            var model = new ChartQuoteHistoryModel();

            foreach (var quote in quotes)
            {
                var salesPersonFullName = quote.SalesPerson.FullName;
                var quoteTotal = QuoteTotal.GetQuoteTotal(quote.Items).TotalPrice;
                model.AddSalesPersonTotal(salesPersonFullName, quoteTotal);

                // Only do this if we have more than one Company
                if (quotes.Select(x => x.Company.Id).Distinct().Count() > 1)
                {
                    AddCompanyTotals(model, quotes);
                }

                AddStatusTotals(model, quote, quoteTotal);
            }
            AddMaterialTotals(model, quotes);
            return model;
        }

        public ChartQuoteHistoryModel GetChartHistoryModelForQuotes(List<CrmQuote> quotes)
        {
            quotes = quotes.Where(x => x.Company != null).ToList();

            var model = new ChartQuoteHistoryModel();

            var quoteDetailList = quotes.Select(x=> new QuoteDataForQuoteHistory(x.QuoteId, x.ReportDate, x.Status, x.Items, x.QuickQuoteItems, x.SalesPerson, x.Company))
                .ToList();

            foreach (var quote in quoteDetailList)
            {
                //var salesPersonFullName = quote.SalesPerson.FullName;
                var quoteTotal = QuoteTotal.GetQuoteTotal(quote.Items).TotalPrice;
                //model.AddSalesPersonTotal(salesPersonFullName, quoteTotal);

                // Only do this if we have more than one Company
                //if (quotes.Select(x => x.Company.Id).Distinct().Count() > 1)
                //{
                //    AddCompanyTotals(model, quoteDetailList);
                //}

                AddStatusTotals(model, quote, quoteTotal);
            }
            //AddMaterialTotals(model, quoteDetailList);
            return model;
        }


        public void AddCompanyTotals(ChartQuoteHistoryModel model, List<QuoteDataForQuoteHistory> quotes)
        {
            model.CustomerSummary.Clear();

            foreach (var quote in quotes.Where(x=> x.Status == PipelineStatus.Won).ToList())
            {
                var quoteTotal = QuoteTotal.GetQuoteTotal(quote.Items).TotalPrice;

                AddCompanyTotal(model, quote, quoteTotal);
            }
        }

        public void AddCompanyTotal(ChartQuoteHistoryModel model, QuoteDataForQuoteHistory quote, decimal quoteTotal)
        {
            var customerRow = model.CustomerSummary.FirstOrDefault(
                x => x.CompanyId.ToString() == quote.Company.Id);
            if (customerRow != null)
            {
                customerRow.Y += quoteTotal;
            }
            else
            {
                model.CustomerSummary.Add(new CustomerChartSummary()
                {
                    CompanyId = quote.Company.Id,
                    Name = quote.Company.CodePlusName,
                    Y = quoteTotal
                });
            }
        }

        public void AddMaterialTotals(ChartQuoteHistoryModel model, List<QuoteDataForQuoteHistory> quotes)
        {
            model.MaterialHistoryTotals.Clear();
            foreach (var quoteDataForQuoteHistory in quotes.Where(x => x.Status == PipelineStatus.Won).ToList())
            {
                var items = quoteDataForQuoteHistory.Items.ToList();
                foreach (var itemRef in items)
                {
                    var item = itemRef.AsQuoteItem();
                    if (item.IsLost) continue;
                    if (item.IsQuickQuoteItem) continue;

                    

                    var finishProduct = item.QuotePrice?.FinishedProduct;

                    if (finishProduct == null)
                    {
                        continue;
                    }

                    var key = $"{finishProduct.MetalCategory } {finishProduct.ProductType.ToUpper()}";
                    var subKey = finishProduct.ProductCode;
                    var materialFound = model.MaterialHistoryTotals.FirstOrDefault(x => x.Name == key);

                    materialFound = MetalCategoryAndProductTypeSummary(key, materialFound, item);
                    ProductCodeSummary(materialFound, subKey, item);

                }
            }

            MaterialChartSummary MetalCategoryAndProductTypeSummary(string key, MaterialChartSummary materialFound, CrmQuoteItem crmQuoteItem)
            {
                if (materialFound == null)
                {
                    materialFound = new MaterialChartSummary() { Name = key, Y = crmQuoteItem.QuotePrice.FinalPrice };
                    model.MaterialHistoryTotals.Add(materialFound);
                }
                else
                {
                    materialFound.Y += crmQuoteItem.QuotePrice.FinalPrice;
                }

                return materialFound;
            }

            void ProductCodeSummary(MaterialChartSummary material, string subKey, CrmQuoteItem crmQuoteItem)
            {
                var child = model.MaterialHistoryTotalsDrilldown.FirstOrDefault(x => x.Id == material.Drilldown);
                var materialValue = new MaterialDrilldownValue() { Name = subKey, Y = material.Y };
                if (child == null)
                {
                    child = new MaterialChartSummaryDrilldown() { Id = material.Name };
                    child.Data.Add(materialValue);
                    model.MaterialHistoryTotalsDrilldown.Add(child);
                }
                else
                {
                    var materialValueFound = child.Data.SingleOrDefault(x => x.Name == subKey);
                    if (materialValueFound == null)
                    {
                        child.Data.Add(materialValue);
                    }
                    else
                    {
                        materialValueFound.Y += material.Y;
                    }
                }
            }
        }


        private void AddStatusTotals(ChartQuoteHistoryModel model, QuoteDataForQuoteHistory quote, decimal quoteTotal)
        {
            switch (quote.Status)
            {
                case PipelineStatus.Draft:
                    model.Draft.Add(new ChartQuoteHistoryItem()
                    {
                        X = ChartQuoteHistoryItem.GetUnixDate(quote.ReportDate ?? DateTime.Parse("1/1/1980")),
                        Name = quote.QuoteId.ToString(),
                        Y = quoteTotal
                    }
                    );
                    break;
                case PipelineStatus.Submitted:
                    model.Active.Add(new ChartQuoteHistoryItem()
                    {
                        X = ChartQuoteHistoryItem.GetUnixDate(quote.ReportDate ?? DateTime.Parse("1/1/1980")),
                        Name = quote.QuoteId.ToString(),
                        Y = quoteTotal
                    }
                    );
                    break;
                case PipelineStatus.Won:
                    model.Won.Add(new ChartQuoteHistoryItem()
                    {
                        X = ChartQuoteHistoryItem.GetUnixDate(quote.ReportDate ?? DateTime.Parse("1/1/1980")),
                        Name = quote.QuoteId.ToString(),
                        Y = quoteTotal
                    }
                    );
                    break;
                case PipelineStatus.Loss:
                    model.Lost.Add(new ChartQuoteHistoryItem()
                    {
                        X = ChartQuoteHistoryItem.GetUnixDate(quote.ReportDate ?? DateTime.Parse("1/1/1980")),
                        Name = quote.QuoteId.ToString(),
                        Y = quoteTotal
                    }
                    );
                    break;
                case PipelineStatus.Expired:
                    model.Expired.Add(new ChartQuoteHistoryItem()
                    {
                        X = ChartQuoteHistoryItem.GetUnixDate(quote.ReportDate ?? DateTime.Parse("1/1/1980")),
                        Name = quote.QuoteId.ToString(),
                        Y = quoteTotal
                    }
                    );
                    break;
            }
        }

    }
}
