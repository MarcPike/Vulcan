using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Company;
using Vulcan.IMetal.Helpers;
using Company = DAL.Vulcan.Mongo.DocClass.Companies.Company;

namespace DAL.Vulcan.Mongo.Models
{
    public class CompanyCreditAnalysisModel
    {
        private HelperCurrencyForIMetal _helperCurrency;

        private CompanyContext _context;
        public string Coid { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }

        public string CompanyCurrencyCode { get; set; }

        public string DisplayCurrency { get; set; }

        public decimal ExchangeRate { get; set; }

        public int OpenInvoices { get; set; }
        public int CompletedInvoices { get; set; }

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
        public List<Invoice> OpenInvoicesList { get; set; } = new List<Invoice>();

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

            _context = ContextFactory.GetCompanyContextForCoid(Coid);

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

                result += DAL.Vulcan.Mongo.DocClass.Quotes.QuoteTotal.GetQuoteTotal(quote.Items).TotalPrice * exchangeRate;
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

                result += DAL.Vulcan.Mongo.DocClass.Quotes.QuoteTotal.GetQuoteTotal(quote.Items).TotalPrice * exchangeRate;

            }

            ExpiredQuotesTotal = result;
        }

        private void SetCompanyCurrencyCode()
        {
            var context = ContextFactory.GetCompanyContextForCoid(Coid);
            var result = context.ExecuteQuery<CompanyCurrency>($@"
            SELECT
                cc.code AS Currency
            FROM companies c 
                INNER JOIN currency_codes cc ON c.currency_id = cc.id
            WHERE c.Code = '{CompanyCode}' AND c.type_id = 1 AND c.Status = 'A'").FirstOrDefault();

            CompanyCurrencyCode = result?.Currency ?? "USD";

            ExchangeRate =
                _helperCurrency.ConvertValueFromCurrencyToCurrency(1, CompanyCurrencyCode, DisplayCurrency);

        }

        private void GetCreditLimitDataAndPaymentTerm()
        {
            var context = ContextFactory.GetCompanyContextForCoid(Coid);
            var result = context.ExecuteQuery<CompanyCreditRuleResult>($@"
            SELECT
                c.name as CompanyName, 
                cc.code AS Currency,
                ccr.status AS CreditStatus,
                ccr.credit_limit AS CreditLimit,
                ccr.credit_limit_percentage AS CreditLimitPercentage,
                ccr.overdue_days_allowed AS OverdueDaysAllowed,
                ccr.overdue_percentage_allowed AS OverduePercentageAllowed,
                ccr.credit_limit_date AS CreditLimitDate,
                ccr.credit_limit_expiry AS CreditLimitExpiry,
                t.description AS PaymentTerm
            FROM companies c 
                INNER JOIN currency_codes cc ON c.currency_id = cc.id
                INNER JOIN company_credit_rules ccr ON c.company_credit_rules_id = ccr.id
                INNER JOIN terms t ON c.terms_id = t.id
    
            WHERE c.Code = '{CompanyCode}' AND c.type_id = 1 AND ccr.Status = 'A'").FirstOrDefault();

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
            var context = ContextFactory.GetCompanyContextForCoid(Coid);

            var result = context.ExecuteQuery<Invoice>($@"
                SELECT 
                    cc.code AS Currency,
                    sh.number AS Number, 
                    si.item_number AS ItemNumber, 
                    sh.customer_order_number AS CustomerOrderNumber, 
                    st.balance_value AS Balance, 
                    si.due_date AS DueDate,
                    DATE_PART('day', si.due_date - NOW()) AS DaysToPay,
                    t.due_days AS DueDays
                FROM sales_headers sh INNER JOIN sales_items si ON sh.id=si.sales_header_id
                    INNER JOIN companies c ON sh.customer_id=c.id
                    INNER JOIN currency_codes cc ON c.currency_id = cc.id
                    INNER JOIN terms t ON sh.terms_id=t.id
                    INNER JOIN sales_types ON sh.type_id=sales_types.id
                    INNER JOIN sales_totals st ON si.sales_total_id=st.id
                    INNER JOIN sales_status_codes ssc ON sh.status_id=ssc.id
                WHERE c.code='{CompanyCode}' AND c.type_id = 1 AND sales_types.code IN ('DIV', 'MIV', 'INV', 'COD') AND (ssc.code<>'CMP' AND ssc.code<>'DEL')
             ").ToList();


            OpenInvoicesList = result;
            foreach (var openInvoice in OpenInvoicesList)
            {
                openInvoice.Balance = openInvoice.Balance * ExchangeRate;
            }
        }



        private void GetCompanyCreditTotalResults()
        {
            var context = ContextFactory.GetCompanyContextForCoid(Coid);

            var result = context.ExecuteQuery<CompanyCreditTotalResults>($@"
            SELECT
                            SUM(CASE WHEN si.due_date>NOW() THEN st.balance_value ELSE 0 END) AS TotalOpen,
                            SUM(CASE WHEN si.due_date<=NOW() THEN st.balance_value ELSE 0 END) AS TotalDue,
                            SUM(CASE WHEN DATE_PART('day', si.due_date - NOW())<31 THEN st.balance_value ELSE 0 END) AS TotalDue30,
                            SUM(CASE WHEN DATE_PART('day', si.due_date - NOW()) BETWEEN 31 AND 60 THEN st.balance_value ELSE 0 END) AS TotalDue60,
                            SUM(CASE WHEN DATE_PART('day', si.due_date - NOW()) BETWEEN 61 AND 90 THEN st.balance_value ELSE 0 END) AS TotalDue90,
                            SUM(CASE WHEN DATE_PART('day', si.due_date - NOW()) BETWEEN 91 AND 120 THEN st.balance_value ELSE 0 END) AS TotalDue120,
                            SUM(CASE WHEN DATE_PART('day', si.due_date - NOW())>120 THEN st.balance_value ELSE 0 END) AS TotalDueOver120,
                SUM(CASE WHEN ssc.code<>'CMP' AND ssc.code<>'DEL' THEN 1 ELSE 0 END) AS OpenInvoices,
                SUM(CASE ssc.code WHEN 'CMP' THEN 1 ELSE 0 END) AS CompletedInvoices,
                MAX(COALESCE(st.balance_value, 0)) AS LargestInvoice
            FROM sales_headers sh INNER JOIN sales_items si ON sh.id=si.sales_header_id
            INNER JOIN companies c ON sh.customer_id=c.id
            INNER JOIN terms t ON sh.terms_id=t.id
            INNER JOIN sales_types ON sh.type_id=sales_types.id
            INNER JOIN sales_totals st ON si.sales_total_id=st.id
            INNER JOIN sales_status_codes ssc ON sh.status_id=ssc.id
            WHERE c.code='{CompanyCode}' AND c.type_id = 1 AND sales_types.code IN ('DIV', 'MIV', 'INV', 'COD') AND (ssc.code<>'CMP' AND ssc.code<>'DEL')").Single();

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
            var context = ContextFactory.GetCompanyContextForCoid(Coid);

            var result = context.ExecuteQuery<TotalOpenCompletedAndLargestResult>($@"
            SELECT 
                MAX(COALESCE(st.balance_value, 0)) AS LargestInvoice,
                SUM(CASE WHEN ssc.code <> 'CMP' AND ssc.code<>'DEL' THEN 1 ELSE 0 END) AS OpenInvoices,
                SUM(CASE WHEN ssc.code  = 'CMP' THEN 1 ELSE 0 END) AS CompletedInvoices
            FROM sales_headers sh INNER JOIN sales_items si ON sh.id=si.sales_header_id
                INNER JOIN companies c ON sh.customer_id=c.id
                INNER JOIN sales_types ON sh.type_id=sales_types.id
                INNER JOIN sales_totals st ON si.sales_total_id=st.id
                INNER JOIN sales_status_codes ssc ON sh.status_id=ssc.id
            WHERE c.code='{CompanyCode}' AND c.type_id = 1 AND sales_types.code IN ('DIV', 'MIV', 'INV', 'COD')").Single();

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
