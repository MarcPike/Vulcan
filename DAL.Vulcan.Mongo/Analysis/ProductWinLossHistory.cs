using System;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.Quotes;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Analysis
{
    public class ProductWinLossHistory
    {
        public string Coid { get; set; }
        public int QuoteId { get; set; }
        public ObjectId QuoteObjectId { get; set; }
        public ObjectId QuoteItemObjectId { get; set; }
        public CompanyRef Company { get; set; }
        public ProspectRef Prospect { get; set; }
        public TeamRef Team { get; set; }
        public CrmUserRef SalesPerson { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime WonLossDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public RequiredQuantity RequiredQuantity { get; set; }
        public ProductMaster ProductInfo { get; set; }
        public bool Win { get; set; }
        public bool Loss { get; set; }
        public bool Expired { get; set; }
        public string DisplayCurrency { get; set; }
        public decimal PricePerInch { get; set; }
        public decimal PricePerPound { get; set; }
        public decimal PricePerEach { get; set; }
        public decimal PricePerKilogram { get; set; }
        public bool IsFinishedProduct { get; set; }
        public bool IsStartingProduct { get; set; }
        public decimal TotalPounds { get; set; }
        public decimal TotalInches { get; set; }
        public decimal TotalKilograms { get; set; }
        public PipelineStatus Status { get; set; }
        public string OemType { get; set; }
        public decimal TotalCost { get; set; } = 0;
        public decimal TotalPrice { get; set; } = 0;
        public decimal AdditionalServiceCost { get; set; } = 0;
        public decimal AdditionalServicePrice { get; set; } = 0;
        public decimal TotalKerfCost { get; set; } = 0;

    }
}