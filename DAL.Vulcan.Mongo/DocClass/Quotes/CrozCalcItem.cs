using System;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.Quotes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Vulcan.IMetal.Helpers;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    [BsonIgnoreExtraElements]
    public class CrozCalcItem : BaseDocument
    {
        private readonly HelperCurrencyForIMetal _iHelperCurrencyForIMetal = new HelperCurrencyForIMetal();

        public static MongoRawQueryHelper<CrozCalcItem> Helper = new MongoRawQueryHelper<CrozCalcItem>();
        public string Coid { get; set; } = String.Empty;

        public string DisplayCurrency { get; set; }


        public string BaseCurrency
        {
            get
            {
                var currency = "USD";
                if (String.IsNullOrEmpty(Coid))
                {
                    return currency;
                }
                if (Coid == "EUR")
                    currency = "GBP";
                if (Coid == "CAN")
                    currency = "CAD";
                return currency;
            }
        }

        public OrderQuantity OrderQuantity { get; set; } = new OrderQuantity(1,0,"in");

        public string StartingProductLabel { get; set; } = string.Empty;
        public string FinishedProductLabel { get; set; } = string.Empty;

        public decimal UnitCost => ResultBaseUnitCost == 0 ? 0 : _iHelperCurrencyForIMetal.ConvertValueFromCurrencyToCurrency(ResultBaseUnitCost, BaseCurrency, DisplayCurrency);
        public decimal UnitPrice => ResultBaseUnitPrice == 0 ? 0 : _iHelperCurrencyForIMetal.ConvertValueFromCurrencyToCurrency(ResultBaseUnitPrice, BaseCurrency, DisplayCurrency);

        public decimal UnitWeight { get; set; } = 0;
        public string UnitWeightUom { get; set; } = string.Empty;
        public decimal TotalWeight => OrderQuantity.Pieces * UnitWeight;


        public decimal TotalCost 
        {
            get
            {
                if (OrderQuantity.QuantityType == "ea")
                {
                    return OrderQuantity.Pieces * UnitCost;
                }
                else
                {
                    return OrderQuantity.Pieces * OrderQuantity.Quantity * UnitCost;
                }
            }
        }

        public decimal TotalPrice
        {
            get
            {
                if (OrderQuantity.QuantityType == "ea")
                {
                    return OrderQuantity.Pieces * UnitPrice;
                }
                else
                {
                    return OrderQuantity.Pieces * OrderQuantity.Quantity * UnitPrice;
                }
            }
        }

        public decimal ResultBaseUnitCost { get; set; }
        public decimal ResultBaseUnitPrice { get; set; }

        public string InputData { get; set; } = string.Empty;
        public string OutputData { get; set; } = string.Empty;

        public bool Regret { get; set; } = false;

        public CrozCalcItem(string coid, string displayCurrency)
        {
            Coid = coid;
            DisplayCurrency = displayCurrency;
        }

        public CrozCalcItem()
        {
            
        }

        public static CrozCalcItemModel Save(CrozCalcItemModel model)
        {
            var item = CrozCalcItem.Helper.FindById(model.Id) ?? new CrozCalcItem() { Id = ObjectId.Parse(model.Id) };

            item.Coid = model.Coid;
            item.DisplayCurrency = model.DisplayCurrency;
            item.OrderQuantity = model.OrderQuantity;
            item.ResultBaseUnitCost = model.ResultBaseUnitCost;
            item.ResultBaseUnitPrice = model.ResultBaseUnitPrice;
            item.StartingProductLabel = model.StartingProductLabel;
            item.FinishedProductLabel = model.FinishedProductLabel;
            item.OutputData = model.OutputData;
            item.InputData = model.InputData;
            item.UnitWeightUom = model.UnitWeightUom;
            item.Regret = model.Regret;
            CrozCalcItem.Helper.Upsert(item);

            return new CrozCalcItemModel(model.Application, model.UserId, item);

        }


    }
}