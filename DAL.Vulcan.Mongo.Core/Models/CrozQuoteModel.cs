using System;
using System.Collections.Generic;
using DAL.iMetal.Core.Helpers;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using Vulcan.IMetal.Models;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class CrozQuoteModel
    {
        public int QuoteId { get; set; }
        public string Coid { get; set; }
        public CompanyRef Company { get; set; }
        public ProspectRef Prospect { get; set; }
        public string DisplayCurrency { get; set; }
        public List<ExchangeRate> ExchangeRates { get; set; } 
        public ProductMaster ProductDetails { get; set; }

        public List<string> SupportedDisplayCurrencies { get; set; } = new List<string>();

        public CrozQuoteModel()
        {
        }

        public CrozQuoteModel(QuoteModel quote, ProductMaster productMaster)
        {
            if (quote == null) throw new Exception("Quote is null");

            QuoteId = quote.QuoteId;
            Coid = quote.Coid;
            Company = quote.Company;
            Prospect = quote.Prospect;
            DisplayCurrency = quote.DisplayCurrency;
            ExchangeRates = ExchangeRate.GetRateListForCoid(quote.Coid);
            ProductDetails = productMaster;

            var helperCurrency = new HelperCurrencyForIMetal();
            SupportedDisplayCurrencies = helperCurrency.GetSupportedDisplayCurrencyCodes();
        }

        public CrozQuoteModel(int quoteId, string coid, string companyId, string prospectId,
            string displayCurrency, string productCode)
        {
            QuoteId = quoteId;
            Coid = coid;
            if (companyId != null)
            {
                Company = DAL.Vulcan.Mongo.Core.DocClass.Companies.Company.Helper.FindById(companyId)?.AsCompanyRef();
            }

            if (prospectId != null)
            {
                Prospect = DAL.Vulcan.Mongo.Core.DocClass.Companies.Prospect.Helper.FindById(prospectId)?.AsProspectRef();
            }
            DisplayCurrency = displayCurrency;
            ExchangeRates = ExchangeRate.GetRateListForCoid(coid);
            ProductDetails = new ProductMaster(coid, productCode);

            var helperCurrency = new HelperCurrencyForIMetal();
            SupportedDisplayCurrencies = helperCurrency.GetSupportedDisplayCurrencyCodes();
        }

    }
}
