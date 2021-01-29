using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.iMetal.Core.Helpers;
using DAL.iMetal.Core.Models;
using DAL.iMetal.Core.Queries;
using Company = DAL.Vulcan.Mongo.Core.DocClass.Companies.Company;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class CompanyCreditAnalysisModel
    {
        private HelperCurrencyForIMetal _helperCurrency;

        public string Coid { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }

        public string CompanyCurrencyCode { get; set; }

        public string DisplayCurrency { get; set; }

        public decimal ExchangeRate { get; set; }

        public long OpenInvoices { get; set; }
        public long CompletedInvoices { get; set; }

        public decimal TotalOpen { get; set; }
        public decimal TotalDue { get; set; }

        public decimal TotalDue30 { get; set; }
        public decimal TotalDue60 { get; set; }
        public decimal TotalDue90 { get; set; }
        public decimal TotalDue120 { get; set; }
        public decimal TotalDueOver120 { get; set; }
        public decimal LargestInvoice { get; set; }

        public string CreditStatus { get; set; }
        public decimal? CreditLimit { get; set; }
        public decimal? CreditLimitPercentage { get; set; }
        public int? OverdueDaysAllowed { get; set; }
        public decimal? OverduePercentageAllowed { get; set; }
        public DateTime? CreditLimitDate { get; set; }
        public DateTime? CreditLimitExpiry { get; set; }
        public string PaymentTerm { get; set; }

        public decimal AvailableCredit => (CreditLimit ?? 0) - TotalOpen;

        public double AverageDaysToPay
        {
            get
            {
                if (!OpenInvoicesList.Any()) return 0;
                return OpenInvoicesList.Average(x => x.DaysToPay);
            }
        }
        public List<iMetalInvoice> OpenInvoicesList { get; set; } = new List<iMetalInvoice>();

        public decimal SubmittedQuotesTotal { get; set; }
        public decimal ExpiredQuotesTotal { get; set; }

        public decimal QuoteTotal => SubmittedQuotesTotal + ExpiredQuotesTotal;

        public CompanyCreditAnalysisModel()
        {
            
        }

        public CompanyCreditAnalysisModel(string companyId, string displayCurrency)
        {
            var company = Company.Helper.FindById(companyId);
            
            _helperCurrency = new HelperCurrencyForIMetal();


            Coid = company.Location.GetCoid();
            CompanyCode = company.Code;
            DisplayCurrency = displayCurrency;

            SetCompanyCurrencyCode();

            Task getExpiredQuotesTotal = new Task(() => GetExpiredQuotesTotal(companyId));
            getExpiredQuotesTotal.Start();

            Task getSubmittedQuotesTotal = new Task(() => GetSubmittedQuotesTotal(companyId));
            getSubmittedQuotesTotal.Start();

            Task getCompanyCreditTotalResults = new Task(GetCompanyCreditTotalResults);
            getCompanyCreditTotalResults.Start();

            Task getTotalOpenCompletedAndLargest = new Task(GetTotalOpenCompletedAndLargest);
            getTotalOpenCompletedAndLargest.Start();

            Task getCreditLimitData = new Task(GetCreditLimitDataAndPaymentTerm);
            getCreditLimitData.Start();

            Task getInvoices = new Task(GetInvoices);
            getInvoices.Start();

            Task.WaitAll(
                getSubmittedQuotesTotal,
                getExpiredQuotesTotal,
                getCompanyCreditTotalResults, 
                getInvoices, 
                getCreditLimitData, 
                getTotalOpenCompletedAndLargest);

        }

        private void GetSubmittedQuotesTotal(string companyId)
        {
            var filter = CrmQuote.Helper.FilterBuilder.Where(x =>
                x.Company.Id == companyId &&
                x.Status == PipelineStatus.Submitted);
            var project = CrmQuote.Helper.ProjectionBuilder.Expression(x => new { Items = x.Items, x.DisplayCurrency });
            var allQuotes = CrmQuote.Helper.FindWithProjection(filter, project);
            decimal result = 0;
            foreach (var quote in allQuotes)
            {
                //result += quote.Where(x => !x.ItemSummaryViewModel.Regret && !x.ItemSummaryViewModel.IsLost)
                //    .Sum(x => x.ItemSummaryViewModel.Total);
                var exchangeRate =
                    _helperCurrency.ConvertValueFromCurrencyToCurrency(1, quote.DisplayCurrency, DisplayCurrency);

                result += DAL.Vulcan.Mongo.Core.DocClass.Quotes.QuoteTotal.GetQuoteTotal(quote.Items).TotalPrice * exchangeRate;
            }

            SubmittedQuotesTotal = result;
        }

        private void GetExpiredQuotesTotal(string companyId)
        {
            var filter = CrmQuote.Helper.FilterBuilder.Where(x =>
                x.Company.Id == companyId &&
                x.Status == PipelineStatus.Expired);
            var project = CrmQuote.Helper.ProjectionBuilder.Expression(x => new {Items = x.Items, x.DisplayCurrency });
            var allQuotes = CrmQuote.Helper.FindWithProjection(filter, project);
            decimal result = 0;
            foreach (var quote in allQuotes)
            {
                //result += quote.Where(x => !x.ItemSummaryViewModel.Regret && !x.ItemSummaryViewModel.IsLost)
                //    .Sum(x => x.ItemSummaryViewModel.Total);

                var exchangeRate =
                    _helperCurrency.ConvertValueFromCurrencyToCurrency(1, quote.DisplayCurrency, DisplayCurrency);

                result += DAL.Vulcan.Mongo.Core.DocClass.Quotes.QuoteTotal.GetQuoteTotal(quote.Items).TotalPrice * exchangeRate;

            }

            ExpiredQuotesTotal = result;
        }

        private void SetCompanyCurrencyCode()
        {
            var result = CompanyQuery.GetCompanyCurrencyCode(Coid, CompanyCode).Result;

            CompanyCurrencyCode = result?.Currency ?? "USD";

            ExchangeRate =
                _helperCurrency.ConvertValueFromCurrencyToCurrency(1, CompanyCurrencyCode, DisplayCurrency);

        }

        private void GetCreditLimitDataAndPaymentTerm()
        {
            var result = CompanyQuery.GetCompanyCreditRuleResult(Coid, CompanyCode).Result;

            CompanyName = result.CompanyName;
            CreditStatus = result.CreditStatus;
            CreditLimit = result.CreditLimit;
            CreditLimitPercentage = result.CreditLimitPercentage;
            OverdueDaysAllowed = result.OverdueDaysAllowed;
            OverduePercentageAllowed = result.OverduePercentageAllowed;
            CreditLimitDate = result.CreditLimitDate;
            CreditLimitExpiry = result.CreditLimitExpiry;
            PaymentTerm = result.PaymentTerm;
        }



    private void GetInvoices()
        {
            var invoices = CompanyQuery.GetInvoices(Coid, CompanyCode).Result;


            OpenInvoicesList = invoices;
            foreach (var openInvoice in OpenInvoicesList)
            {
                openInvoice.Balance = openInvoice.Balance * ExchangeRate;
            }
        }



        private void GetCompanyCreditTotalResults()
        {
            var result = CompanyQuery.GetCompanyCreditTotalResults(Coid, CompanyCode).Result;


            TotalOpen = result.TotalOpen * ExchangeRate;
            TotalDue = result.TotalDue * ExchangeRate;
            TotalDue30 = result.TotalDue30 * ExchangeRate;
            TotalDue60 = result.TotalDue60 * ExchangeRate;
            TotalDue90 = result.TotalDue90 * ExchangeRate;
            TotalDue120 = result.TotalDue120 * ExchangeRate;
            TotalDueOver120 = result.TotalDueOver120 * ExchangeRate;
        }

        private void GetTotalOpenCompletedAndLargest()
        {
            var result = CompanyQuery.GetTotalOpenCompletedAndLargest(Coid, CompanyCode).Result;


            LargestInvoice = result.LargestInvoice * ExchangeRate;
            OpenInvoices = result.OpenInvoices;
            CompletedInvoices = result.CompletedInvoices;
        }

        private class CompanyCurrency
        {
            public string Currency { get; set; }
        }


        private class CompanyCreditRuleResult
        {
            public string CompanyName { get; set; }
            public string CreditStatus { get; set; }
            public decimal? CreditLimit { get; set; }
            public decimal? CreditLimitPercentage { get; set; }
            public int? OverdueDaysAllowed { get; set; }
            public decimal? OverduePercentageAllowed { get; set; }
            public DateTime? CreditLimitDate { get; set; }
            public DateTime? CreditLimitExpiry { get; set; }
            public string PaymentTerm { get; set; }


        }

        public class Invoice
        {
            public int Number { get; set; }
            public int ItemNumber { get; set; }
            public string CustomerOrderNumber { get; set; }
            public decimal Balance { get; set; }
            public DateTime DueDate { get; set; }
            public int DaysToPay { get; set; }
            public int? DueDays { get; set; }
        }

        private class CompanyCreditTotalResults
        {
            public decimal TotalOpen { get; set; }
            public decimal TotalDue { get; set; }
            public decimal TotalDue30 { get; set; }
            public decimal TotalDue60 { get; set; }
            public decimal TotalDue90 { get; set; }
            public decimal TotalDue120 { get; set; }
            public decimal TotalDueOver120 { get; set; }
        }

        private class TotalOpenCompletedAndLargestResult
        {
            public int OpenInvoices { get; set; }
            public int CompletedInvoices { get; set; }
            public decimal LargestInvoice { get; set; }

        }

    }
}
