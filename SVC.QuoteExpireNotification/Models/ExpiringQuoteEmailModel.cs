using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Extensions;
using DAL.Vulcan.Mongo.Models;
using Vulcan.IMetal.Helpers;

namespace SVC.QuoteExpireNotification.Models
{
    public class ExpiringQuoteEmailModel
    {
        private readonly string _server;
        private readonly string _id;
        public CrmUserRef SalesPerson { get; set; }
        public int QuoteId { get; set; }
        public CompanyRef Company { get; set; }
        public ProspectRef Prospect { get; set; }
        public decimal QuoteTotal { get; set; }
        public string Currency { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Link => $"<a href='{_server}/home/dashboard/quotes/quote?id={_id}' target='_blank'>Click here to View Quote</a>)";

        public ExpiringQuoteEmailModel(CrmQuote quote, string server = "http://s-us-web02:88")
        {
            if (quote == null) throw new Exception("No Quote passed to ExpiringQuoteEmailModel constructor");
            if (quote.Status != PipelineStatus.Submitted) throw new Exception("Quote is not Active");
            if (quote.SubmitDate == null) throw new Exception("Quote is missing Submitted date");

            var helperCurrency = new HelperCurrencyForIMetal();

            var quoteTotal = new QuoteTotal(quote.Items.Select(x => x.AsQuoteItem()).ToList(), false).TotalPrice;
            var displayCurrency = quote.DisplayCurrency;
            if (displayCurrency == String.Empty) displayCurrency = helperCurrency.GetDefaultCurrencyForCoid(quote.Coid);
            quoteTotal = helperCurrency.ConvertValueFromCurrencyToCurrency(quoteTotal, displayCurrency, displayCurrency);


            _server = server;
            SalesPerson = quote.SalesPerson;
            QuoteTotal = quoteTotal;
            Currency = quote.DisplayCurrency;
            _id = quote.Id.ToString();
            QuoteId = quote.QuoteId;
            Company = quote.Company;
            Prospect = quote.Prospect;
            ExpireDate = quote.SubmitDate.Value.AddDays(quote.ValidityDays + 1).Date;
        }
    }

}
