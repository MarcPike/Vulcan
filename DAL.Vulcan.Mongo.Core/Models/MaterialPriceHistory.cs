using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.iMetal.Core.Helpers;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class MaterialPriceHistory
    {
        public string DisplayCurrency { get; set; }
        public MaterialPriceHistoryStatus History7Days { get; set; } = new MaterialPriceHistoryStatus();

        public MaterialPriceHistoryStatus History30Days { get; set; } = new MaterialPriceHistoryStatus();

        public MaterialPriceHistoryStatus History60Days { get; set; } = new MaterialPriceHistoryStatus();

        public MaterialPriceHistoryStatus History90Days { get; set; } = new MaterialPriceHistoryStatus();

        public MaterialPriceHistoryStatus HistoryGreaterThan90Days { get; set; } = new MaterialPriceHistoryStatus();

        public List<MaterialPriceHistoryValue> AllHistoryValues { get; set; } = new List<MaterialPriceHistoryValue>();

        public List<CompanyRef> Companies { get; set; } = new List<CompanyRef>();

        public List<ProspectRef> Prospects { get; set; } = new List<ProspectRef>();

        public static MaterialPriceHistory GetMaterialPriceHistory(string coid, int productId, string displayCurrency)
        {


            var productMaster = new ProductMaster(coid,productId);
            return new MaterialPriceHistory(productMaster, displayCurrency);
        }

        public MaterialPriceHistory(ProductMaster product, string displayCurrency)
        {
            if (product == null)
            {
                return;
            }

            DisplayCurrency = displayCurrency;

            if (product.IsNewProduct) return;
            
            var rep = new RepositoryBase<CrmQuoteItem>();
            var allQuoteItemsStarting = rep.AsQueryable().Where(x =>
                x.QuotePrice.StartingProduct.ProductId == product.ProductId).ToList();

            var days7 = DateTime.Now.AddDays(-7);
            var days30 = DateTime.Now.AddDays(-30);
            var days60 = DateTime.Now.AddDays(-60);
            var days90 = DateTime.Now.AddDays(-90);
            var days90Plus = DateTime.Now.AddDays(-91);

            foreach (var crmQuoteItem in allQuoteItemsStarting.ToList())
            {
                var helperCurrency = new HelperCurrencyForIMetal();
                var exchangeRate =
                    helperCurrency.GetExchangeRateForCurrencyFromCoid(displayCurrency, crmQuoteItem.Coid);

                var quote = crmQuoteItem.GetQuote();
                if (quote == null) 
                {
                    allQuoteItemsStarting.Remove(crmQuoteItem);
                    continue;
                }

                var reportDateAndStatus = GetReportDateAndStatus(quote, crmQuoteItem);
                var newValue = new MaterialPriceHistoryValue(quote, crmQuoteItem, reportDateAndStatus.Status, reportDateAndStatus.ReportDate, exchangeRate);

                if (reportDateAndStatus.ReportDate >= days7)
                {
                    History7Days.AddItem(newValue);
                }
                else if (reportDateAndStatus.ReportDate >= days30)
                {
                    History30Days.AddItem(newValue);
                }
                else if (reportDateAndStatus.ReportDate >= days60)
                {
                    History60Days.AddItem(newValue);
                }
                else if (reportDateAndStatus.ReportDate >= days90)
                {
                    History90Days.AddItem(newValue);
                }
                else if (reportDateAndStatus.ReportDate >= days90Plus)
                {
                    HistoryGreaterThan90Days.AddItem(newValue);
                } 

                AllHistoryValues.Add(newValue);
            }

            var companyIdList = AllHistoryValues.Where(x=>x.Company != null).Select(x => x.Company.Id).Distinct().ToList();
            foreach (var companyId in companyIdList)
            {
                Companies.Add(AllHistoryValues.Where(x=>x.Company != null).Select(x=>x.Company).First(x => x.Id == companyId));
            }

            var prospectIdList = AllHistoryValues.Where(x => x.Prospect != null).Select(x => x.Prospect.Id).Distinct().ToList();
            foreach (var prospectId in prospectIdList)
            {
                Prospects.Add(AllHistoryValues.Where(x => x.Prospect != null).Select(x => x.Prospect).First(x => x.Id == prospectId));
            }

        }

        private (DateTime ReportDate,PipelineStatus Status)  GetReportDateAndStatus(CrmQuote quote, CrmQuoteItem crmQuoteItem)
        {
            PipelineStatus status;
            var dateOf = new DateTime(1980,1,1);
            if (quote.Status == PipelineStatus.Draft)
            {
                dateOf = quote.CreateDateTime;
                status = PipelineStatus.Draft;
            }
            else if (quote.Status == PipelineStatus.Loss) 
            {
                dateOf = quote.LostDate ?? DateTime.Now;
                status = PipelineStatus.Loss;
            } else if (crmQuoteItem.IsLost)
            {
                dateOf = crmQuoteItem.LostDate ?? DateTime.Now;
                status = PipelineStatus.Loss;
            }
            else if (quote.Status == PipelineStatus.Won)
            {
                if (crmQuoteItem.IsLost)
                {
                    dateOf = crmQuoteItem.LostDate ?? DateTime.Now;
                    status = PipelineStatus.Loss;
                }
                else
                {
                    dateOf = quote.WonDate ?? DateTime.Now;
                    status = PipelineStatus.Won;
                }
            }
            else
            {
                if (crmQuoteItem.IsLost)
                {
                    dateOf = crmQuoteItem.LostDate ?? DateTime.Now;
                    status = PipelineStatus.Loss;
                }
                else
                {
                    dateOf = quote.SubmitDate ?? DateTime.Now;
                    status = PipelineStatus.Submitted;
                }

            }


            return (dateOf, status);

        }
    }
}