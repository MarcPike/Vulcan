using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Quotes;
using DocumentFormat.OpenXml.Office2010.Excel;
using Vulcan.IMetal.Helpers;

namespace DAL.Vulcan.Mongo.Models
{
    public class QuickQuoteItemModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; } 
        public string Coid { get; set; }
        public string BaseCurrency { get; set; }
        public OrderQuantity OrderQuantity { get; set; } 
        public RequiredQuantity RequiredQuantity { get; set; }
        public ProductMaster StartingProduct { get; set; } = null;
        public ProductMaster FinishedProduct { get; set; } = null;
        public decimal Cost { get; set; }
        public decimal UnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Price { get; set; }
        public string Label { get; set; }
        public bool IsLost { get; set; }
        public CompetitorRef LostTo { get; set; }
        public LostReason LostReason { get; set; }
        public DateTime? LostDate { get; set; }
        public string PartSpecification { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string LeadTime { get; set; } = string.Empty;
        public string OemType { get; set; } = string.Empty;

        public string SalesPersonNotes { get; set; } = string.Empty;
        public string CustomerNotes { get; set; } = string.Empty;
        public bool Regret { get; set; } = false;

        public Dictionary<string,decimal> ExchangeRatesFromBaseCurrency { get;  } = new Dictionary<string, decimal>();

        public QuickQuoteItemModel()
        {
        }

        public QuickQuoteItemModel(string application, string userId, QuickQuoteItem item)
        {

            Coid = item.Coid;
            BaseCurrency = item.BaseCurrency;
            Cost = item.Cost;
            FinishedProduct = item.FinishedProduct;
            StartingProduct = item.StartingProduct;
            Id = item.Id.ToString();
            IsLost = item.IsLost;
            Label = item.Label;
            LostDate = item.LostDate;
            LostReason = item.LostReason;
            LostTo = item.LostTo;
            OrderQuantity = item.OrderQuantity;
            UnitPrice = item.UnitPrice;
            UnitCost = item.UnitCost;
            Price = item.Price;
            RequiredQuantity = item.RequiredQuantity;
            PartSpecification = item.PartSpecification;
            PartNumber = item.PartNumber;
            LeadTime = item.LeadTime;
            OemType = item.OemType;
            SalesPersonNotes = item.SalesPersonNotes;
            CustomerNotes = item.CustomerNotes;
            Regret = item.Regret;

            Application = application;
            UserId = userId;

            var helperCurrency = new HelperCurrencyForIMetal();

            foreach (var currency in helperCurrency.GetSupportedDisplayCurrencyCodes().ToList())
            {
                ExchangeRatesFromBaseCurrency.Add(currency, helperCurrency.ConvertValueFromCurrencyToCurrency(1, BaseCurrency, currency));
            }


        }



    }
}
