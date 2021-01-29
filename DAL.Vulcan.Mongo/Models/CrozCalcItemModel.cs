﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Quotes;
using MongoDB.Bson;
using Vulcan.IMetal.Helpers;

namespace DAL.Vulcan.Mongo.Models
{
    public class CrozCalcItemModel
    {
        private readonly HelperCurrencyForIMetal _iHelperCurrencyForIMetal = new HelperCurrencyForIMetal();
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public string Coid { get; set; }
        public string BaseCurrency { get; set; }
       
        public string DisplayCurrency { get; set; }
        public OrderQuantity OrderQuantity { get; set; } = new OrderQuantity(0,1,"in");
        public string StartingProductLabel { get; set; } = string.Empty;
        public string FinishedProductLabel { get; set; } = string.Empty;
        public decimal UnitCost { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitWeight { get; set; }
        public string UnitWeightUom { get; set; }

        public decimal TotalCost { get; set; }

        public decimal TotalPrice { get; set; }
        public decimal ResultBaseUnitCost { get; set; }
        public decimal ResultBaseUnitPrice { get; set; }

        public decimal ResultBaseTotalCost
        {
            get
            {
                if (OrderQuantity.QuantityType == "ea")
                {
                    return OrderQuantity.Pieces * ResultBaseUnitCost;
                }
                else
                {
                    return OrderQuantity.Pieces * OrderQuantity.Quantity * ResultBaseUnitCost;
                }
            }
        }

        public decimal ResultBaseTotalPrice
        {
            get
            {
                if (OrderQuantity.QuantityType == "ea")
                {
                    return OrderQuantity.Pieces * ResultBaseUnitPrice;
                }
                else
                {
                    return OrderQuantity.Pieces * OrderQuantity.Quantity * ResultBaseUnitPrice;
                }
            }
        }
        public string InputData { get; set; }
        public string OutputData { get; set; }
        public bool Regret { get; set; }

        public bool IsSaved
        {
            get
            {
                return CrozCalcItem.Helper.FindById(Id) != null;
            }
        }


        public CrozCalcItemModel()
        {
            
        }
        public CrozCalcItemModel(string application, string userId, CrozCalcItem item)
        {
            Application = application;
            UserId = userId;
            Id = item.Id.ToString();
            Coid = item.Coid;
            BaseCurrency = item.BaseCurrency;
            DisplayCurrency = item.DisplayCurrency;
            OrderQuantity = item.OrderQuantity;
            UnitCost = item.UnitCost;
            UnitPrice = item.UnitPrice;
            UnitWeight = item.UnitWeight;
            UnitWeightUom = item.UnitWeightUom;
            TotalCost = item.TotalCost;
            TotalPrice = item.TotalPrice;
            ResultBaseUnitCost = item.ResultBaseUnitCost;
            ResultBaseUnitPrice = item.ResultBaseUnitPrice;
            InputData = item.InputData;
            OutputData = item.OutputData;
            StartingProductLabel = item.StartingProductLabel;
            FinishedProductLabel = item.FinishedProductLabel;
            Regret = item.Regret;

        }

        public static CrozCalcItemModel CreateNew(string application, string userId, string coid, string displayCurrency)
        {
            var item = new CrozCalcItem(coid, displayCurrency);
            return new CrozCalcItemModel(application, userId, item);
        }

    }
}
