using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.Quotes;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Vulcan.IMetal.Queries.Companies;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Web_Emulations
{
    /*
    [TestFixture]
    public class QuoteBuilder
    {
        private readonly string _dividerLine = new String('=', 60);
        private readonly string _dividerLineTwo = new String('-', 60);

        private const string Application = "vulcancrm";
        private const string UserId = "599b1575b508d62d0c75a932"; // TestPerson
        private const string Coid = "INC";

        private const string CompanyId = "593ee5cfb508d7372cf9d343"; // Baker Hughes 00113

        private HelperQuote _helperQuote;
        private HelperUser _helperUser;
        private HelperCompany _helperCompany;

        private CrmUser _crmUser;
        private CrmUserModel _crmUserModel;

        // Models 
        public QuoteModel QuoteModel;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;

            _helperUser = new HelperUser(new HelperPerson());
            _helperCompany = new HelperCompany();
            _helperQuote = new HelperQuote();


            QuoteModel = _helperQuote.CreateNewQuoteForCompany(Application, UserId, Coid, CompanyId);
            _crmUser = _helperUser.GetCrmUser(Application, UserId);
            _crmUserModel = new CrmUserModel(Application,UserId,_crmUser);

            var companyAddresses = GetAddressesForCompanyFromIMetal(Coid, CompanyId);

            QuoteModel.PrimaryAddress = companyAddresses.PrimaryAddress;
            QuoteModel.ShipToAddress = companyAddresses.OtherAddress.FirstOrDefault() ?? new Address();
            QuoteModel.OtherAddresses = companyAddresses.OtherAddress;
            QuoteModel.SalesPerson = _crmUser.AsCrmUserRef();

            //Console.WriteLine(ObjectDumper.Dump(QuoteModel));

        }

        [Test]
        public void CreateQuoteItem_Test()
        {
            var result = CreateQuoteItem();
            Assert.IsNotNull(result.QuoteItemModel);
            Assert.IsNotNull(result.QuotePriceModel);
            Assert.IsNotNull(result.CalculateQuotePriceModel);
            Assert.IsNotNull(result.QuotePriceModel.BaseCost);
        }

        [Test]
        public void CreateQuoteItem_AddMargin_Test()
        {
            var result = CreateQuoteItem();
            result = AddMarginToQuoteItem(
                result.QuoteItemModel, 
                result.QuotePriceModel,
                result.CalculateQuotePriceModel);

            Assert.AreEqual(result.QuotePriceModel.Margin * 100,20);

        }

        [Test]
        public void CreateQuoteItem_AddMargin_BlendedServiceCost_Test()
        {
            var result = CreateQuoteItem();
            result = AddMarginToQuoteItem(
                result.QuoteItemModel,
                result.QuotePriceModel,
                result.CalculateQuotePriceModel);

            var quotePriceModelPriorToServiceCost = result.QuotePriceModel;

            result = AddBlendedServiceCost(result.QuoteItemModel, result.QuotePriceModel,
                result.CalculateQuotePriceModel);

            Assert.AreNotEqual(quotePriceModelPriorToServiceCost.SalePrice, result.QuotePriceModel.SalePrice);
            Assert.AreEqual(result.QuotePriceModel.Margin * 100, 20);
        }

        [Test]
        public void CreateQuoteItem_AddMargin_BlendedServiceCost_OverridePerInchCost_Test()
        {
            var result = CreateQuoteItem();
            result = AddMarginToQuoteItem(
                result.QuoteItemModel,
                result.QuotePriceModel,
                result.CalculateQuotePriceModel);


            result = AddBlendedServiceCost(result.QuoteItemModel, result.QuotePriceModel,
                result.CalculateQuotePriceModel);

            var salePriceBefore = result.QuotePriceModel.SalePrice;
            var costPerInchBefore = result.QuotePriceModel.BaseCost.ActCostPerInch;

            result = OverridePerInchCost(result.QuoteItemModel, result.QuotePriceModel, result.CalculateQuotePriceModel,
                (decimal) 0.10);

            Assert.Greater(result.QuotePriceModel.SalePrice, salePriceBefore);
            Assert.AreEqual(result.QuotePriceModel.BaseCost.ActCostPerInch - (decimal)0.10, costPerInchBefore);

        }


        public (QuoteItemModel QuoteItemModel, QuotePriceModel QuotePriceModel, CalculateQuotePriceModel CalculateQuotePriceModel) 
            CreateQuoteItem()
        {
            var orderQuanity = new OrderQuantity(5, 10, "in");

            var quoteItem = _helperQuote.CreateNewItem(UserId, Coid);
            quoteItem.OrderQuantity = orderQuanity;

            var quoteItemModel = new QuoteItemModel(quoteItem)
            {
                Coid = Coid,
                ProductId = 10093
            };
            // 925 0.5 SAPH

            quoteItemModel = _helperQuote.CalculateQuoteItem(quoteItemModel);

            var calculateQuotePriceModel = new CalculateQuotePriceModel
            {
                StartingBaseCost = quoteItemModel.BaseCost,
                QuoteCalcOperation = QuoteCalcOperation.Initialize.ToString(),
                Value = 0
            };

            var quotePriceModel = _helperQuote.CalculateQuotePrice(calculateQuotePriceModel);

            calculateQuotePriceModel.LastQuotePriceOperationValue = quotePriceModel.LastQuotePriceOperationValue;
            calculateQuotePriceModel.LastQuotePriceOperation = quotePriceModel.LastQuotePriceOperation;

            DumpObject("CreateQuoteItem", quotePriceModel);

            return (quoteItemModel, quotePriceModel, calculateQuotePriceModel);

        }

        public (QuoteItemModel QuoteItemModel, QuotePriceModel QuotePriceModel, CalculateQuotePriceModel CalculateQuotePriceModel) 
            AddMarginToQuoteItem(
                QuoteItemModel quoteItemModel, QuotePriceModel quotePriceModel, CalculateQuotePriceModel calculateQuotePriceModel)
        {
            calculateQuotePriceModel.LastQuotePriceOperationValue = quotePriceModel.LastQuotePriceOperationValue;
            calculateQuotePriceModel.LastQuotePriceOperation = quotePriceModel.LastQuotePriceOperation;


            calculateQuotePriceModel.QuoteCalcOperation = QuoteCalcOperation.Margin.ToString();
            calculateQuotePriceModel.Value = 20;

            quotePriceModel = _helperQuote.CalculateQuotePrice(calculateQuotePriceModel);

            calculateQuotePriceModel.LastQuotePriceOperationValue = quotePriceModel.LastQuotePriceOperationValue;
            calculateQuotePriceModel.LastQuotePriceOperation = quotePriceModel.LastQuotePriceOperation;

            DumpObject("AddMarginToQuoteItem", quotePriceModel);

            return (quoteItemModel, quotePriceModel, calculateQuotePriceModel);

        }

        public (QuoteItemModel QuoteItemModel, QuotePriceModel QuotePriceModel, CalculateQuotePriceModel CalculateQuotePriceModel)
            AddBlendedServiceCost(QuoteItemModel quoteItemModel, QuotePriceModel quotePriceModel, CalculateQuotePriceModel calculateQuotePriceModel)
        {


            calculateQuotePriceModel.LastQuotePriceOperationValue = quotePriceModel.LastQuotePriceOperationValue;
            calculateQuotePriceModel.LastQuotePriceOperation = quotePriceModel.LastQuotePriceOperation;

            var previousQuotePriceModel = quotePriceModel;

            var serviceCost = new ItemResourceCostModel()
            {
                ResourceTypeId = (int)ResourceType.Inspection,
                PriceTypeId = (int)PerType.PerLot,
                ProductionCost = 25,
                InternalCost = (decimal)12.5,
                IsPriceBlended = true
            };

            calculateQuotePriceModel.QuoteCalcOperation = QuoteCalcOperation.ServiceCosts.ToString();
            calculateQuotePriceModel.ServiceCosts.Add(serviceCost);

            quotePriceModel = _helperQuote.CalculateQuotePrice(calculateQuotePriceModel);

            calculateQuotePriceModel.LastQuotePriceOperationValue = quotePriceModel.LastQuotePriceOperationValue;
            calculateQuotePriceModel.LastQuotePriceOperation = quotePriceModel.LastQuotePriceOperation;

            DumpObject("AddBlendedServiceCost", quotePriceModel);

            return (quoteItemModel, quotePriceModel, calculateQuotePriceModel);

        }

        public (QuoteItemModel QuoteItemModel, QuotePriceModel QuotePriceModel, CalculateQuotePriceModel CalculateQuotePriceModel)
            OverridePerInchCost(QuoteItemModel quoteItemModel, QuotePriceModel quotePriceModel, CalculateQuotePriceModel calculateQuotePriceModel, decimal addToCostPerInch)
        {

            calculateQuotePriceModel.LastQuotePriceOperationValue = quotePriceModel.LastQuotePriceOperationValue;
            calculateQuotePriceModel.LastQuotePriceOperation = quotePriceModel.LastQuotePriceOperation;

            var costOverride = new CostOverride()
            {
                OverrideType = OverrideType.MaterialCostPerInch,
                Value = addToCostPerInch + quotePriceModel.BaseCost.ActCostPerInch
            };

            calculateQuotePriceModel.QuoteCalcOperation = QuoteCalcOperation.CostOverrides.ToString();
            calculateQuotePriceModel.Value = costOverride.Value;
            calculateQuotePriceModel.CostOverrides.Add(new CostOverrideModel(costOverride));

            quotePriceModel = _helperQuote.CalculateQuotePrice(calculateQuotePriceModel);

            calculateQuotePriceModel.LastQuotePriceOperationValue = quotePriceModel.LastQuotePriceOperationValue;
            calculateQuotePriceModel.LastQuotePriceOperation = quotePriceModel.LastQuotePriceOperation;

            DumpObject("OverridePerInchCost", quotePriceModel);

            return (quoteItemModel, quotePriceModel, calculateQuotePriceModel);

        }

        #region Test Helper Methods

        private void DumpObject(string caption, object obj)
        {
            Console.WriteLine(_dividerLine);
            Console.WriteLine(caption);
            Console.WriteLine(_dividerLine);
            Console.WriteLine(ObjectDumper.Dump(obj));
            Console.WriteLine(_dividerLineTwo);

        }

        private (Address PrimaryAddress, List<Address> OtherAddress)
            GetAddressesForCompanyFromIMetal(string coid, string companyId)
        {
            var query = new QueryCompany(coid);
            var companyRef = _helperCompany.GetCompanyRef(companyId);
            query.Id = companyRef.SqlId;

            var companySearchResult = query.Refresh().FirstOrDefault();
            if (companySearchResult == null) throw new Exception("Company no longer exists in iMetal");

            var primaryAddress = new Address
            {
                Id = Guid.NewGuid(),
                AddressLine1 = companySearchResult.PrimaryAddress.Address,
                AddressLine2 = string.Empty,
                City = companySearchResult.PrimaryAddress.Town,
                StateProvince = companySearchResult.PrimaryAddress.County,
                PostalCode = companySearchResult.PrimaryAddress.PostCode,
                Country = companySearchResult.PrimaryAddress.CountryName,
                Type = AddressType.Primary
            };

            var otherAddresses = new List<Address>();
            otherAddresses.AddRange(companySearchResult.Addresses.Select(x => new Address()
            {
                Id = Guid.NewGuid(),
                AddressLine1 = x.Address,
                AddressLine2 = string.Empty,
                City = x.Town,
                StateProvince = x.County,
                PostalCode = x.PostCode,
                Country = x.CountryName,
                Type = AddressType.Other

            }));

            return (primaryAddress, otherAddresses);

        }


        #endregion

    }
    */
}
