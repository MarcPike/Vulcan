using System;
using System.Collections.Generic;
using System.Linq;
using BLL.EMail;
using DAL.Vulcan.Mongo.Analysis;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.Queries;
using DAL.Vulcan.Mongo.Quotes;
using DAL.Vulcan.Mongo.TeamSettings;
using MongoDB.Bson;
using MongoDB.Driver;
using Vulcan.IMetal.Helpers;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperQuote : HelperBase, IHelperQuote
    {
        private readonly HelperCompany _helperCompany = new HelperCompany();
        private readonly HelperUser _helperUser = new HelperUser(new HelperPerson());

        #region Calculate Quote

        public (QuotePriceModel QuotePriceModel, CalculateQuotePriceModel CalculateQuotePriceModel) CalculateQuotePrice(
            CalculateQuotePriceModel model)
        {
            var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);
            var teamPriceTier = TeamPriceTier.GetForTeam(crmUser.ViewConfig.Team.AsTeam());

            var quotePrice = model.GenerateQuotePrice(model.DisplayCurrency, model.OldCurrency, model.QuoteSource,
                teamPriceTier);

            var quotePriceModel = new QuotePriceModel(model.Application, model.UserId, quotePrice)
            {
                Application = model.Application,
                UserId = model.UserId
            };
            //model = new CalculateQuotePriceModel()
            //{
            //    BaseCostStart = model.BaseCostStart,
            //    StartingProduct = model.StartingProduct,
            //    FinishedProduct = model.FinishedProduct,
            //    KurfInchesPerCut = model.KurfInchesPerCut,
            //    CostOverrides = model.CostOverrides,
            //    RequiredQuantity = model.RequiredQuantity,
            //    ProductionCosts = quotePriceModel.ProductionCosts,
            //    Status = "Ok",
            //    Application = model.Application,
            //    UserId = model.UserId,
            //    MaterialCostValue = model.MaterialCostValue,
            //    MaterialPriceValue = model.MaterialPriceValue,
            //    OrderQuantity = model.OrderQuantity,
            //};

            model.OldCurrency = model.DisplayCurrency;
            return (quotePriceModel, model);
        }

        #endregion

        #region Quote Queries

        public QuotePipelineModel GetQuotesPipelineForUser(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);

            var teamRef = crmUser.ViewConfig.Team;
            if (teamRef == null) throw new Exception("User is not currently part of a Team");

            var team = teamRef.AsTeam();
            var companies = team.Alliances.Select(x => x.Id).ToList();
            companies.AddRange(team.NonAlliances.Select(x => x.Id));

            var quoteRep = new RepositoryBase<CrmQuote>();
            var quotes = quoteRep.AsQueryable()
                .Where(x => companies.Contains(x.Company.Id) && x.CreatedByUserId == userId).ToList();

            var result = new QuotePipelineModel(application, userId, quotes);

            return result;
        }

        public QuotesPipelineQuery GetQuotesPipelineForCompany(DateTime begDate, DateTime endDate, string companyId,
            bool showExpired)
        {
            //var queryHelper = new MongoRawQueryHelper<CrmQuote>();

            //var quotes = queryHelper.Find(
            //    queryHelper.FilterBuilder.Where(x => x.Company.Id == companyId)).ToList();

            //var quoteRep = new RepositoryBase<CrmQuote>();
            //var quotes = quoteRep.AsQueryable().Where(x => x.Company.Id == companyId).ToList();

            var result = QuotesPipelineQuery.QuotesPipelineQueryForCompany(begDate, endDate, companyId, showExpired);

            return result;
        }

        public QuotePipelineModel GetQuotesPipelineForCompany(string application, string userId, string companyId)
        {
            var quoteRep = new RepositoryBase<CrmQuote>();
            var quotes = quoteRep.AsQueryable().Where(x => x.Company.Id == companyId).ToList();


            return new QuotePipelineModel(application, userId, quotes);
        }


        public CompanyActivityView GetCustomerActivityView(
            string application,
            string userId,
            DateTime beginDate,
            DateTime endDate,
            string salesPersonId)
        {
            //var crmUser = _helperUser.GetCrmUser(application, userId);
            var repQuotes = new RepositoryBase<CrmQuote>();
            var salesPerson = _helperUser.GetCrmUser(application, salesPersonId);
            var result = new CompanyActivityView(salesPerson.AsCrmUserRef());
            salesPersonId = salesPerson.Id.ToString();

            var companyActivityList = new List<CompanyActivity>();
            var repCompany = new RepositoryBase<Company>();
            //var companyActivityList = repQuotes.AsQueryable(new AggregateOptions() {AllowDiskUse = true, MaxTime = new TimeSpan(0,0,3,0)})
            //    .OrderBy(x => x.Company.Name)
            //    .Where(x => x.Company != null && 
            //                x.SalesPerson.Id == salesPersonId && 
            //                x.ReportDate != null &&
            //                x.ReportDate >= beginDate && 
            //                x.ReportDate <= endDate)
            //    .OrderBy(x=>x.Company.Name)
            //    .GroupBy(c => c.Company.Id).Select(c =>
            //            new CompanyActivity(repCompany, c.Key, c.Count() 
            //        ));

            var companyIds = repQuotes.AsQueryable().Where(x => x.Company != null &&
                                                                x.SalesPerson.Id == salesPersonId &&
                                                                x.ReportDate != null &&
                                                                x.ReportDate >= beginDate &&
                                                                x.ReportDate <= endDate)
                .Select(x => x.Company.Id)
                .ToList();
            companyIds = companyIds.Distinct().ToList();

            foreach (var companyId in companyIds)
            {
                var company = repCompany.Find(companyId);
                if (company == null) continue;
                var companyRef = company.AsCompanyRef();
                var count = repQuotes.AsQueryable().Count(x => x.Company != null &&
                                                               x.SalesPerson.Id == salesPersonId &&
                                                               x.ReportDate != null &&
                                                               x.ReportDate >= beginDate &&
                                                               x.ReportDate <= endDate &&
                                                               x.Company.Id == companyRef.Id);
                if (count == 0) continue;

                var wins = repQuotes.AsQueryable().Count(x => x.Company != null &&
                                                              x.SalesPerson.Id == salesPersonId &&
                                                              x.ReportDate != null &&
                                                              x.ReportDate >= beginDate &&
                                                              x.ReportDate <= endDate &&
                                                              x.Company.Id == companyRef.Id &&
                                                              x.Status == PipelineStatus.Won);


                companyActivityList.Add(new CompanyActivity(companyRef, count, wins));
            }

            companyActivityList = companyActivityList.OrderBy(x => x.Company.Name).ToList();

            result.CompanyActivities.AddRange(companyActivityList);
            return result;
        }

        public ProspectActivityView GetProspectActivityView(
            string application,
            string userId,
            DateTime beginDate,
            DateTime endDate,
            string salesPersonId)
        {
            var repQuotes = new RepositoryBase<CrmQuote>();
            var salesPerson = _helperUser.GetCrmUser(application, salesPersonId);
            var result = new ProspectActivityView(salesPerson.AsCrmUserRef());
            salesPersonId = salesPerson.Id.ToString();

            var prospectActivityList = new List<ProspectActivity>();
            var repProspect = new RepositoryBase<Prospect>();

            var prospectIds = repQuotes.AsQueryable().Where(x => x.Prospect != null &&
                                                                 x.SalesPerson.Id == salesPersonId &&
                                                                 x.ReportDate != null &&
                                                                 x.ReportDate >= beginDate &&
                                                                 x.ReportDate <= endDate)
                .Select(x => x.Prospect.Id)
                .ToList();
            prospectIds = prospectIds.Distinct().ToList();

            foreach (var prospectId in prospectIds)
            {
                var prospect = repProspect.Find(prospectId);
                if (prospect == null) continue;
                var prospectRef = prospect.AsProspectRef();
                var count = repQuotes.AsQueryable().Count(x => x.Prospect != null &&
                                                               x.SalesPerson.Id == salesPersonId &&
                                                               x.ReportDate != null &&
                                                               x.ReportDate >= beginDate &&
                                                               x.ReportDate <= endDate &&
                                                               x.Prospect.Id == prospectRef.Id);
                if (count == 0) continue;

                var wins = repQuotes.AsQueryable().Count(x => x.Prospect != null &&
                                                              x.SalesPerson.Id == salesPersonId &&
                                                              x.ReportDate != null &&
                                                              x.ReportDate >= beginDate &&
                                                              x.ReportDate <= endDate &&
                                                              x.Prospect.Id == prospectRef.Id &&
                                                              x.Status == PipelineStatus.Won);


                prospectActivityList.Add(new ProspectActivity
                {
                    Prospect = prospectRef,
                    TotalQuotes = count,
                    Wins = wins
                });
            }

            prospectActivityList = prospectActivityList.OrderBy(x => x.Prospect.Name).ToList();

            result.ProspectActivities.AddRange(prospectActivityList);
            return result;
        }

        public QuotePipelineModel GetQuotePipelineForCustomerActivity(string application, string userId,
            DateTime beginDate,
            DateTime endDate, string companyOrProspectId, string salesPersonId, bool prospectsInsteadOfCompanies)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var salesPerson = _helperUser.GetCrmUser(application, salesPersonId);
            salesPersonId = salesPerson.Id.ToString();

            var repQuotes = new RepositoryBase<CrmQuote>();

            var quotes = new List<CrmQuote>();

            GetAllCompanyQuotes();
            GetAllProspectQuotes();

            return new QuotePipelineModel(application, userId, quotes);

            void GetAllCompanyQuotes()
            {
                if (!prospectsInsteadOfCompanies)
                    quotes.AddRange(repQuotes.AsQueryable().Where(x =>
                        x.Company != null &&
                        x.Company.Id == companyOrProspectId &&
                        x.SalesPerson.Id == salesPersonId &&
                        x.ReportDate != null &&
                        x.ReportDate >= beginDate &&
                        x.ReportDate <= endDate).ToList());
            }

            void GetAllProspectQuotes()
            {
                if (prospectsInsteadOfCompanies)
                    quotes.AddRange(repQuotes.AsQueryable().Where(x =>
                        x.Company == null &&
                        x.Prospect != null &&
                        x.Prospect.Id == companyOrProspectId &&
                        x.SalesPerson.Id == salesPersonId &&
                        x.ReportDate != null &&
                        x.ReportDate >= beginDate &&
                        x.ReportDate <= endDate).ToList());
            }
        }

        public CrmQuote MoveQuote(CrmQuote quote, Team newTeam, CrmUser newSalesPerson, string newCompanyId)
        {
            var newCompany = Company.Helper.FindById(newCompanyId);
            if (newCompany == null) throw new Exception("Company not found");


            if (newTeam.Companies.All(x => x.Id != newCompanyId)) throw new Exception("Invalid Company");

            quote.Team = newTeam.AsTeamRef();
            quote.SalesPerson = newSalesPerson.AsCrmUserRef();
            quote.Company = newCompany.AsCompanyRef();
            quote.Coid = newTeam.Coid;
            CompanyDefaults.ApplyCompanyDefaultsToQuote(quote);
            quote.SaveToDatabase();
            return quote;
        }


        public QuotePipelineModel GetQuotesPipeline(string application, string userId, DateTime begDate,
            DateTime endDate, bool forTeam)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);

            var quoteRep = new RepositoryBase<CrmQuote>();
            var teamRef = crmUser.ViewConfig.Team;

            var collection = quoteRep.Collection;
            var builder = Builders<CrmQuote>.Filter;
            var filter = builder.Gte(x => x.ReportDate, begDate) & builder.Lte(x => x.ReportDate, endDate);

            if (forTeam)
                filter = filter & builder.Eq(x => x.Team.Id, teamRef.Id);
            else
                filter = filter & builder.Eq(x => x.SalesPerson.Id, crmUser.Id.ToString());

            var quotes = collection.Find(filter).ToList();

            return new QuotePipelineModel(application, userId, quotes);
        }

        public QuotePipelineModel GetQuotesPipelineForContact(string application, string userId, string contactId,
            string teamId)
        {
            var repContact = new RepositoryBase<Contact>();
            var contact = repContact.Find(contactId);
            if (contact == null) throw new Exception("Contact not found");


            var repQuotes = new RepositoryBase<CrmQuote>();
            var quotes = repQuotes.AsQueryable()
                .Where(x => x.Contact.Id == contact.Id.ToString() && x.Team.Id == teamId);

            return new QuotePipelineModel(application, userId, quotes.ToList());
        }


        public QuotePipelineModel GetQuotesPipelineForTeam(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var teamRef = crmUser.ViewConfig.Team;
            if (teamRef == null) throw new Exception("User is not currently part of a Team");

            var quoteRep = new RepositoryBase<CrmQuote>();
            var quotes = quoteRep.AsQueryable().Where(x => x.Team.Id == teamRef.Id).ToList();

            //var teamMembers = teamRef.AsTeam().CrmUsers.Select(x=>x.Id).ToList();
            //var quoteRep = new RepositoryBase<CrmQuote>();
            //var quotes = quoteRep.AsQueryable().Where(x => teamMembers.Contains(x.SalesPerson.Id)).ToList();

            return new QuotePipelineModel(application, userId, quotes);
        }

        public List<QuoteModel> GetCompanyQuotes(string application, string userId, string companyId, DateTime minDate,
            DateTime maxDate)
        {
            var repQuote = new RepositoryBase<CrmQuote>();

            var quotes = repQuote.AsQueryable().Where(x =>
                x.Company.Id == companyId && x.CreateDateTime >= minDate && x.CreateDateTime <= maxDate).ToList();

            return quotes.Select(x => new QuoteModel(application, userId, x)).ToList();
        }

        public QuoteModel GetQuoteModel(string application, string userId, string quoteId)
        {
            var quote = GetQuote(quoteId);

            return new QuoteModel(application, userId, quote);
        }


        public CrmQuoteItem GetQuoteItem(string quoteItemId)
        {
            var item = new RepositoryBase<CrmQuoteItem>().AsQueryable()
                .SingleOrDefault(x => x.Id == ObjectId.Parse(quoteItemId));
            if (item == null) throw new Exception("CrmQuoteItem not found");
            return item;
        }

        public QuoteModel GetQuote(string application, string userId, string quoteId)
        {
            var repQuote = new RepositoryBase<CrmQuote>();
            var quote = repQuote.Find(quoteId);
            if (quote == null) throw new Exception("CrmQuote not found");

            return new QuoteModel(application, userId, quote);
        }

        public CrmQuote GetQuote(string quoteId)
        {
            var quote = new RepositoryBase<CrmQuote>().AsQueryable()
                .SingleOrDefault(x => x.Id == ObjectId.Parse(quoteId));
            if (quote == null) throw new Exception("CrmQuote not found");
            return quote;
        }

        #endregion

        #region Create Quote

        public (CalculateQuotePriceModel CalculateQuotePriceModel, QuotePriceModel QuotePriceModel)
            GetNewCalculateQuotePriceModelForStockItem(QuoteStockItemModel model)
        {
            var startingProduct = new ProductMaster(model.Coid, model.StartingProduct.ProductId);

            if (startingProduct.StockGrade == null)
                startingProduct.StockGrade = string.Empty;
            //throw new Exception("This stock item has no assigned Stock Grade which is required in order to calculate Cutting cost");


            var finishedProduct = startingProduct;

            var customerUom = CustomerUom.PerPiece;
            if (model.CompanyId != string.Empty)
            {
                var company = _helperCompany.GetCompanyRef(model.CompanyId).AsCompany();
                var companyDefaults = CompanyDefaults.GetCompanyDefaults(company.Location.GetCoid(), company, false);
                customerUom = companyDefaults.CustomerUom;
            }

            var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);
            var teamPriceTier = TeamPriceTier.GetForTeam(crmUser.ViewConfig.Team.AsTeam());

            var baseCostValues = BaseCost.FromStockItems(model.Coid, model.StockItem.CostPerLb,
                model.StockItem.TheoWeight, model.OrderQuantity, model.StockItem.TagNumber, model.DisplayCurrency);

            var requiredQuantity = baseCostValues.RequiredQuantity;
            var baseCost = baseCostValues.BaseCost;

            var kurfInchesPerCut = model.Coid == "EUR" ? (decimal) 0.197 : (decimal) 0.25;

            var materialCostValue = new MaterialCostValue(
                startingProduct,
                model.OrderQuantity,
                baseCost,
                new List<QuoteTestPiece>(),
                kurfInchesPerCut,
                new List<CostOverride>(),
                0,
                model.DisplayCurrency);
            var materialPriceValue = new MaterialPriceValue(materialCostValue, teamPriceTier);

            var calcModel = new CalculateQuotePriceModel
            {
                BaseCostStart = baseCost,
                StartingProduct = startingProduct,
                FinishedProduct = finishedProduct,
                KurfInchesPerCut = kurfInchesPerCut,
                CostOverrides = new List<CostOverrideModel>(),
                RequiredQuantity = requiredQuantity,
                ProductionCosts = new List<ProductionStepCostBase>(),
                Status = "Ok",
                Application = model.Application,
                UserId = model.UserId,
                MaterialCostValue = materialCostValue,
                MaterialPriceValue = materialPriceValue,
                OrderQuantity = model.OrderQuantity,
                DisplayCurrency = model.DisplayCurrency,
                CustomerUom = customerUom,
                QuoteSource = QuoteSource.StockItem
            };
            calcModel.CalculateCutCost();

            var quotePrice = calcModel.GenerateQuotePrice(model.DisplayCurrency, model.DisplayCurrency,
                QuoteSource.StockItem, teamPriceTier);

            var quotePriceModel = new QuotePriceModel(model.Application, model.UserId, quotePrice)
            {
                Application = model.Application,
                UserId = model.UserId
            };
            return (calcModel, quotePriceModel);
        }

        public QuoteMachinedPartModel GetNewMachinedPartModel(string application, string userId, string coid,
            int stockItemId,
            string displayCurrency)
        {
            return new QuoteMachinedPartModel(application, userId, coid, stockItemId, displayCurrency);
        }


        public (CalculateQuotePriceModel CalculateQuotePriceModel, QuotePriceModel QuotePriceModel)
            GetNewCalculateQuotePriceModelForMachinedPart(QuoteMachinedPartModel model)
        {
            var machinePartCost = new MachinedPartCostPriceValue(model.Coid, model.MachinedPart.ProductId, model.Pieces,
                model.DisplayCurrency);

            //CustomerUom customerUom;

            //var baseCostValues = BaseCost.FromStockItems(model.Coid, model.MachinedPartFromCacheValue.CostPerLb, model.MachinedPartFromCacheValue.TheoWeight, model.OrderQuantity, model.MachinedPartFromCacheValue.TagNumber, model.DisplayCurrency);

            //var requiredQuantity = baseCostValues.RequiredQuantity;
            //var baseCost = baseCostValues.BaseCost;
            var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);
            var teamPriceTier = TeamPriceTier.GetForTeam(crmUser.ViewConfig.Team.AsTeam());

            var calcModel = new CalculateQuotePriceModel();

            var quotePrice = calcModel.GenerateQuotePrice(model.DisplayCurrency, model.DisplayCurrency,
                QuoteSource.MachinedPart, teamPriceTier);

            var quotePriceModel = new QuotePriceModel(model.Application, model.UserId, quotePrice)
            {
                Application = model.Application,
                UserId = model.UserId
            };
            return (calcModel, quotePriceModel);
        }

        public (CalculateQuotePriceModel CalculateQuotePriceModel, QuotePriceModel QuotePriceModel)
            GetNewCalculateQuotePriceModelForPartNumberAndStockItem(QuotePartNumberAndStockItemModel model)
        {
            var startingProduct = new ProductMaster(model.Coid, model.StartingProduct.ProductId);
            var finishedProduct = startingProduct;

            var baseCostValues = BaseCost.FromStockItems(model.Coid, model.StockItem.CostPerLb,
                model.StockItem.TheoWeight, model.OrderQuantity, model.StockItem.TagNumber, model.DisplayCurrency);

            var company = _helperCompany.GetCompanyRef(model.CompanyId).AsCompany();
            var companyDefaults = CompanyDefaults.GetCompanyDefaults(company.Location.GetCoid(), company, false);

            var requiredQuantity = baseCostValues.RequiredQuantity;
            var baseCost = baseCostValues.BaseCost;

            var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);
            var teamPriceTier = TeamPriceTier.GetForTeam(crmUser.ViewConfig.Team.AsTeam());

            var kurfInchesPerCut = model.Coid == "EUR" ? (decimal) 0.197 : (decimal) 0.25;

            var materialCostValue = new MaterialCostValue(
                startingProduct,
                model.OrderQuantity,
                baseCost,
                new List<QuoteTestPiece>(),
                kurfInchesPerCut,
                new List<CostOverride>(),
                0,
                model.DisplayCurrency);
            var materialPriceValue = new MaterialPriceValue(materialCostValue, teamPriceTier);

            var calcModel = new CalculateQuotePriceModel
            {
                BaseCostStart = baseCost,
                StartingProduct = startingProduct,
                FinishedProduct = finishedProduct,
                KurfInchesPerCut = kurfInchesPerCut,
                CostOverrides = new List<CostOverrideModel>(),
                RequiredQuantity = requiredQuantity,
                ProductionCosts = new List<ProductionStepCostBase>(),
                Status = "Ok",
                Application = model.Application,
                UserId = model.UserId,
                MaterialCostValue = materialCostValue,
                MaterialPriceValue = materialPriceValue,
                OrderQuantity = model.OrderQuantity,
                DisplayCurrency = model.DisplayCurrency,
                CustomerUom = companyDefaults.CustomerUom,
                QuoteSource = QuoteSource.StockItem
            };
            calcModel.CalculateCutCost();

            var quotePrice = calcModel.GenerateQuotePrice(model.DisplayCurrency, model.DisplayCurrency,
                QuoteSource.StockItem, teamPriceTier);
            var quotePriceModel = new QuotePriceModel(model.Application, model.UserId, quotePrice)
            {
                Application = model.Application,
                UserId = model.UserId
            };
            return (calcModel, quotePriceModel);
        }


        public QuickQuoteItemModel GetNewQuickQuoteItemModel(string coid, string application, string userId)
        {
            var newQuickQuoteItem = new QuickQuoteItem {Coid = coid, CreatedByUserId = userId};
            return new QuickQuoteItemModel(application, userId, newQuickQuoteItem);
        }

        public (CalculateQuotePriceModel CalculateQuotePriceModel, QuotePriceModel QuotePriceModel)
            GetNewCalculateQuotePriceModelForPurchaseOrderItem(string coid, decimal costPerPound, int productId,
                OrderQuantity orderedQuantity, string application, string userId, string displayCurrency,
                string companyId)
        {
            var startingProduct = new ProductMaster(coid, productId);
            var finishedProduct = startingProduct;
            var kurfInchesPerCut = coid == "EUR" ? (decimal) 0.197 : (decimal) 0.25;

            var calculatedCost = startingProduct.GetPurchaseOrderItemCalculatedCost();
            var baseCost = BaseCost.FromPurchaseOrderItem(coid, costPerPound, calculatedCost.TheoWeight,
                orderedQuantity, displayCurrency);
            var requiredQuantity =
                orderedQuantity.GetRequiredQuantityBasedOnPoundsPerInch(coid, startingProduct.TheoWeight);

            Company company = null;
            CompanyDefaults companyDefaults = null;
            if (companyId != string.Empty)
            {
                company = _helperCompany.GetCompanyRef(companyId).AsCompany();
                companyDefaults = CompanyDefaults.GetCompanyDefaults(company.Location.GetCoid(), company, false);
            }

            var crmUser = _helperUser.GetCrmUser(application, userId);
            var teamPriceTier = TeamPriceTier.GetForTeam(crmUser.ViewConfig.Team.AsTeam());

            var materialCostValue = new MaterialCostValue(
                startingProduct,
                orderedQuantity,
                baseCost,
                new List<QuoteTestPiece>(),
                kurfInchesPerCut,
                new List<CostOverride>(),
                0,
                displayCurrency);
            var materialPriceValue = new MaterialPriceValue(materialCostValue, teamPriceTier);

            var calcModel = new CalculateQuotePriceModel
            {
                BaseCostStart = baseCost,
                StartingProduct = startingProduct,
                FinishedProduct = finishedProduct,
                KurfInchesPerCut = kurfInchesPerCut,
                CostOverrides = new List<CostOverrideModel>(),
                RequiredQuantity = requiredQuantity,
                ProductionCosts = new List<ProductionStepCostBase>(),
                Status = "Ok",
                Application = application,
                UserId = userId,
                MaterialCostValue = materialCostValue,
                MaterialPriceValue = materialPriceValue,
                OrderQuantity = orderedQuantity,
                DisplayCurrency = displayCurrency,
                QuoteSource = QuoteSource.PurchaseOrderItem,
                CustomerUom = companyDefaults?.CustomerUom ?? CustomerUom.Inches
            };

            calcModel.CalculateCutCost();

            var quotePrice = calcModel.GenerateQuotePrice(displayCurrency, displayCurrency,
                QuoteSource.PurchaseOrderItem, teamPriceTier);
            var quotePriceModel = new QuotePriceModel(application, userId, quotePrice)
            {
                Application = application,
                UserId = userId
            };
            return (calcModel, quotePriceModel);
        }

        //public (CalculateQuotePriceModel CalculateQuotePriceModel, QuotePriceModel QuotePriceModel) GetNewCalculateQuotePriceModelForPurchaseOrderItem(string coid, int productId, decimal costPerPound, OrderQuantity orderedQuantity, string application, string userId, string displayCurrency)
        //{
        //    var startingProduct = new ProductMaster(coid, productId);
        //    var finishedProduct = startingProduct;

        //    var baseCost = BaseCost.FromPurchaseOrderItem(coid, costPerPound, startingProduct.TheoWeight, orderedQuantity, displayCurrency);
        //    var requiredQuantity =
        //        orderedQuantity.GetRequiredQuantityBasedOnPoundsPerInch(coid, startingProduct.TheoWeight);

        //    var materialCostValue = new MaterialCostValue(
        //        startingProduct: startingProduct,
        //        orderQuantity: orderedQuantity,
        //        baseCost: baseCost,
        //        testPieces: new List<QuoteTestPiece>(),
        //        kurfInchesPerCut: (decimal)0.25,
        //        costOverrides: new List<CostOverride>(),
        //        cutCostPerPiece: 0,
        //        displayCurrency: displayCurrency);
        //    var materialPriceValue = new MaterialPriceValue(materialCostValue);

        //    var calcModel = new CalculateQuotePriceModel()
        //    {
        //        BaseCostStart = baseCost,
        //        StartingProduct = startingProduct,
        //        FinishedProduct = finishedProduct,
        //        KurfInchesPerCut = (decimal)0.25,
        //        CostOverrides = new List<CostOverrideModel>(),
        //        RequiredQuantity = requiredQuantity,
        //        ProductionCosts = new List<ProductionStepCostBase>(),
        //        Status = "Ok",
        //        Application = application,
        //        UserId = userId,
        //        MaterialCostValue = materialCostValue,
        //        MaterialPriceValue = materialPriceValue,
        //        OrderQuantity = orderedQuantity,
        //        DisplayCurrency = displayCurrency
        //    };
        //    calcModel.CalculateCutCost();

        //    var quotePrice = calcModel.GenerateQuotePrice(displayCurrency, displayCurrency);
        //    var quotePriceModel = new QuotePriceModel(quotePrice)
        //    {
        //        Application = application,
        //        UserId = userId
        //    };
        //    return (calcModel, quotePriceModel);
        //}


        public (CalculateQuotePriceModel CalculateQuotePriceModel, QuotePriceModel QuotePriceModel)
            GetNewCalculateQuotePriceModelForMadeUpCost(MadeUpCostModel model)
        {
            var startingProduct = new ProductMaster
            {
                TheoWeight = model.TheoWeight,
                Coid = model.Coid,
                OuterDiameter = model.OuterDiameter,
                InsideDiameter = model.InsideDiameter,
                ProductCode = model.ProductCode,
                ProductId = 0,
                MetalCategory = model.MetalCategory,
                ProductType = model.ProductType,
                ProductCondition = model.ProductCondition,
                IsNewProduct = true,
                ProductCategory = model.ProductCategory,
                FactorForLbs = UomHelper.GetFactorForPounds(model.Coid),
                FactorForKilograms = UomHelper.GetFactorForKilograms(model.Coid)
            };

            var finishedProduct = startingProduct;

            var madeUpCost = model.AsMadeUpCost();

            if (madeUpCost == null)
            {
                if (madeUpCost.TheoWeight < 0) throw new Exception("MadeupCost theoretical weight is less than zero");
                throw new Exception("Invalid MadeUp Product definition");
            }

            var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);
            var teamPriceTier = TeamPriceTier.GetForTeam(crmUser.ViewConfig.Team.AsTeam());

            var baseCost = BaseCost.FromMadeUpCost(model.Coid, madeUpCost, model.OrderQuantity, model.DisplayCurrency);
            var requiredQuantity =
                model.OrderQuantity.GetRequiredQuantityBasedOnPoundsPerInch(model.Coid, model.TheoWeight);

            var company = _helperCompany.GetCompanyRef(model.CompanyId)?.AsCompany();
            CompanyDefaults companyDefaults = null;
            if (company != null)
                companyDefaults = CompanyDefaults.GetCompanyDefaults(company.Location.GetCoid(), company, false);

            var kurfInchesPerCut = model.Coid == "EUR" ? (decimal) 0.197 : (decimal) 0.25;


            var materialCostValue = new MaterialCostValue(
                startingProduct,
                model.OrderQuantity,
                baseCost,
                new List<QuoteTestPiece>(),
                kurfInchesPerCut,
                new List<CostOverride>(),
                0,
                model.DisplayCurrency);
            var materialPriceValue = new MaterialPriceValue(materialCostValue, teamPriceTier);

            var calcModel = new CalculateQuotePriceModel
            {
                BaseCostStart = baseCost,
                StartingProduct = startingProduct,
                FinishedProduct = finishedProduct,
                KurfInchesPerCut = kurfInchesPerCut,
                CostOverrides = new List<CostOverrideModel>(),
                RequiredQuantity = requiredQuantity,
                ProductionCosts = new List<ProductionStepCostBase>(),
                Status = "Ok",
                Application = model.Application,
                UserId = model.UserId,
                MaterialCostValue = materialCostValue,
                MaterialPriceValue = materialPriceValue,
                OrderQuantity = model.OrderQuantity,
                CutCostPerPiece = (decimal) 0.25,
                DisplayCurrency = model.DisplayCurrency,
                CustomerUom = companyDefaults?.CustomerUom ?? CustomerUom.Inches,
                QuoteSource = model.QuoteSource
            };
            calcModel.CalculateCutCost();


            var quotePrice = calcModel.GenerateQuotePrice(model.DisplayCurrency, model.DisplayCurrency,
                model.QuoteSource, teamPriceTier);

            var quotePriceModel = new QuotePriceModel(model.Application, model.UserId, quotePrice)
            {
                StartingProduct = startingProduct,
                FinishedProduct = finishedProduct
            };

            return (calcModel, quotePriceModel);
        }

        public QuoteModel CreateNewQuoteForCompany(string application, string userId, string coid, string companyId)
        {
            var companyRef = _helperCompany.GetCompanyRef(companyId);
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var quoteCreator = new QuoteCreatorCompany(coid, crmUser, companyRef);
            var quote = quoteCreator.GetQuote();
            quote.Coid = coid;
            var model = new QuoteModel(application, userId, quote);
            return model;
        }

        public QuoteModel CreateNewQuoteForProspect(string application, string userId, string coid, string prospectId)
        {
            var prospect = new RepositoryBase<Prospect>().Find(prospectId);
            var prospectRef = prospect.AsProspectRef();
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var quoteCreator = new QuoteCreatorProspect(crmUser, prospectRef);
            var quote = quoteCreator.GetQuote();
            quote.ShipToAddress = prospect.Addresses.FirstOrDefault(x => x.Type == AddressType.Shipping) ??
                                  prospect.Addresses.FirstOrDefault();
            quote.Addresses = prospect.Addresses;
            quote.Coid = coid;
            var model = new QuoteModel(application, userId, quote);
            return model;
        }

        #endregion

        #region Copy Quote

        public CrmQuote CopyQuoteItemToDifferentQuote(string application, string userId, string destinationQuoteId,
            string sourceQuoteItemId)
        {
            var rep = new RepositoryBase<CrmQuote>();
            var repItem = new RepositoryBase<CrmQuoteItem>();

            var salesPerson = _helperUser.GetCrmUser(application, userId);

            var destinationQuote = rep.Find(destinationQuoteId);
            if (destinationQuote == null) throw new Exception("Destination Quote not found");

            var sourceQuoteItem = repItem.Find(sourceQuoteItemId);
            if (sourceQuoteItem == null) throw new Exception("Source QuoteItem not found");

            var thisIndex = destinationQuote.Items.Count + 1;

            var newItem = new CrmQuoteItem
            {
                Coid = sourceQuoteItem.Coid,
                Index = thisIndex,
                CustomerNotes = sourceQuoteItem.CustomerNotes,
                QuotePrice = sourceQuoteItem.QuotePrice,
                CalculateQuotePriceModel = sourceQuoteItem.CalculateQuotePriceModel,
                CreateDateTime = DateTime.Now,
                CreatedByUserId = salesPerson.User.Id,
                LeadTime = sourceQuoteItem.LeadTime,
                PartNumber = sourceQuoteItem.PartNumber,
                PartSpecification = sourceQuoteItem.PartSpecification,
                PoNumber = sourceQuoteItem.PoNumber,
                QuoteSource = sourceQuoteItem.QuoteSource,
                SalesPerson = salesPerson.AsCrmUserRef(),
                SalesPersonNotes = sourceQuoteItem.SalesPersonNotes,
                SearchTags = sourceQuoteItem.SearchTags,
                OemType = sourceQuoteItem.OemType,
                RequestedProductCode = sourceQuoteItem.RequestedProductCode,
                QuickQuoteData = sourceQuoteItem.QuickQuoteData,
                CrozCalcItem = sourceQuoteItem.CrozCalcItem
            };
            newItem.SaveToDatabase();

            destinationQuote.Items.Add(newItem.AsQuoteItemRef());
            destinationQuote.SaveToDatabase();
            return destinationQuote;
        }

        public (CrmQuote SourceQuote, CrmQuote DestinationQuote) MoveQuoteItemToDifferentQuote(string application,
            string userId, string destinationQuoteId,
            string sourceQuoteItemId)
        {
            // Copy the item to a different Quote
            var destinationQuote =
                CopyQuoteItemToDifferentQuote(application, userId, destinationQuoteId, sourceQuoteItemId);

            var rep = new RepositoryBase<CrmQuote>();
            var repItem = new RepositoryBase<CrmQuoteItem>();

            // Get Quote Item we are moving
            var sourceQuoteItem = repItem.Find(sourceQuoteItemId);
            if (sourceQuoteItem == null) throw new Exception("Source QuoteItem not found");

            // Get Quote we are moving from
            var sourceQuote = sourceQuoteItem.GetQuote();

            // Remove the Item from the Source Quote Items
            var sourceQuoteItemRef = sourceQuote.Items.FirstOrDefault(x => x.Id == sourceQuoteItemId);
            if (sourceQuoteItemRef != null)
            {
                sourceQuote.Items.Remove(sourceQuoteItemRef);
                rep.Upsert(sourceQuote);
            }

            // Remove the CrmQuoteItem that was just moved
            var repItems = new RepositoryBase<CrmQuoteItem>();
            repItems.RemoveOne(sourceQuoteItem);

            return (sourceQuote, destinationQuote);
        }

        public CrmQuote CreateLinkedQuote(string application, string userId, string sourceQuoteId, string forCompanyId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);

            var destCompany = new RepositoryBase<Company>().Find(forCompanyId);
            if (destCompany == null) throw new Exception("Company not found");


            var sourceQuote = CrmQuote.Helper.FindById(sourceQuoteId);

            if (sourceQuote == null) throw new Exception("Source Quote not found");
            if (sourceQuote.QuoteLinkId == Guid.Empty)
            {
                sourceQuote.QuoteLinkId = Guid.NewGuid();
                sourceQuote.QuoteLinkType = QuoteLinkType.Original;
                CrmQuote.Helper.Upsert(sourceQuote);
            }


            var quoteCopyModel = new QuoteCopyModel(application, userId, sourceQuoteId);

            var result = CopyQuote(quoteCopyModel, false);
            result.Company = destCompany.AsCompanyRef();

            var helperCompanyPaymentTerms = new HelperCompanyPaymentTerms();
            var paymentTerms =
                helperCompanyPaymentTerms.GetPaymentTermsForCompany(destCompany.Location.GetCoid(), destCompany.SqlId);

            result.PaymentTerm = paymentTerms.Description;

            result.Contact = destCompany.Contacts.FirstOrDefault();
            result.QuoteLinkType = QuoteLinkType.Repeat;

            result = ApplyDisplayCurrencyChangeIfNeeded(application, userId, sourceQuote, destCompany, result);

            CompanyDefaults.ApplyCompanyDefaultsToQuote(result);
            if (result.ReportDate == null) result.SetReportDate();
            CrmQuote.Helper.Upsert(result);
            return result;
        }

        private CrmQuote ApplyDisplayCurrencyChangeIfNeeded(string application, string userId, CrmQuote sourceQuote,
            Company destCompany, CrmQuote result)
        {
            var isNorway = sourceQuote.Team.Name.Contains("Norway");

            var companyDefaults = CompanyDefaults.GetCompanyDefaults(sourceQuote.Coid, destCompany, isNorway);
            if (companyDefaults.DisplayCurrency != result.DisplayCurrency)
            {
                var quoteModel = new QuoteModel(application, userId, result);
                quoteModel.DisplayCurrency = companyDefaults.DisplayCurrency;
                SaveQuote(quoteModel);
                result = CrmQuote.Helper.FindById(quoteModel.Id);
            }

            return result;
        }

        public CrmQuote CopyQuote(QuoteCopyModel model, bool sameQuote)
        {
            var rep = new RepositoryBase<CrmQuote>();
            var repItem = new RepositoryBase<CrmQuoteItem>();

            var salesPerson = _helperUser.GetCrmUser(model.Application, model.UserId);

            var original = rep.Find(model.QuoteId);
            if (original == null) throw new Exception("Source Quote not found");

            var result = sameQuote
                ? original
                : new CrmQuote
                {
                    CreatedByUserId = salesPerson.User.Id,
                    CreateDateTime = DateTime.Now,
                    Coid = original.Coid,
                    Contact = original.Contact,
                    ExpireDate = DateTime.Now.Date.AddDays(30),
                    CustomerNotes = original.CustomerNotes,
                    Company = original.Company,
                    FreightTerm = original.FreightTerm,
                    Addresses = original.Addresses,
                    PaymentTerm = original.PaymentTerm,
                    ShipToAddress = original.ShipToAddress,
                    QuickQuoteItems = original.QuickQuoteItems,
                    QuoteId = KeyValues.GetNextQuoteId(),
                    Prospect = original.Prospect,
                    SalesPerson = salesPerson.AsCrmUserRef(),
                    Status = PipelineStatus.Draft,
                    Validity = original.Validity,
                    SalesPersonNotes = original.SalesPersonNotes,
                    SearchTags = original.SearchTags,
                    Team = salesPerson.ViewConfig.Team,
                    PdfRowsPerPage = original.PdfRowsPerPage,
                    DisplayCurrency = original.DisplayCurrency,
                    QuoteLinkId = original.QuoteLinkId
                };
            CopyQuoteItems();
            //CopyQuickQuoteItems();
            result.SetReportDate();

            result.SaveToDatabase();
            return result;

            void CopyQuoteItems()
            {
                var itemList = original.Items.Select(x => x.Id).ToList();
                if (model.QuoteItemIdList.Any()) itemList = model.QuoteItemIdList;

                foreach (var quoteItemId in itemList)
                {
                    var originalQuoteItemRef = original.Items.SingleOrDefault(x => x.Id == quoteItemId);
                    if (originalQuoteItemRef == null) throw new Exception("No QuoteItem with Id == " + quoteItemId);

                    var originalItem = originalQuoteItemRef.AsQuoteItem();
                    var thisIndex = result.Items.Count + 1;

                    var newItem = new CrmQuoteItem
                    {
                        Coid = originalItem.Coid,
                        Index = thisIndex,
                        CustomerNotes = originalItem.CustomerNotes,
                        QuotePrice = originalItem.QuotePrice,
                        CalculateQuotePriceModel = originalItem.CalculateQuotePriceModel,
                        CreateDateTime = DateTime.Now,
                        CreatedByUserId = salesPerson.User.Id,
                        LeadTime = originalItem.LeadTime,
                        PartNumber = originalItem.PartNumber,
                        PartSpecification = originalItem.PartSpecification,
                        PoNumber = originalItem.PoNumber,
                        QuoteSource = originalItem.QuoteSource,
                        SalesPerson = salesPerson.AsCrmUserRef(),
                        SalesPersonNotes = originalItem.SalesPersonNotes,
                        SearchTags = originalItem.SearchTags,
                        OemType = originalItem.OemType,
                        RequestedProductCode = originalItem.RequestedProductCode,
                        QuickQuoteData = originalItem.QuickQuoteData,
                        CrozCalcItem = originalItem.CrozCalcItem
                    };
                    newItem.SaveToDatabase();

                    result.Items.Add(newItem.AsQuoteItemRef());
                }
            }
        }

        public CrmQuote MoveQuoteItemsToNewQuote(QuoteCopyModel model)
        {
            var rep = new RepositoryBase<CrmQuote>();
            var repItem = new RepositoryBase<CrmQuoteItem>();

            var salesPerson = _helperUser.GetCrmUser(model.Application, model.UserId);

            var original = rep.Find(model.QuoteId);
            if (original == null) throw new Exception("Source Quote not found");

            var result = new CrmQuote
            {
                CreatedByUserId = salesPerson.User.Id,
                CreateDateTime = DateTime.Now,
                Coid = original.Coid,
                Contact = original.Contact,
                ExpireDate = DateTime.Now.Date.AddDays(30),
                CustomerNotes = original.CustomerNotes,
                Company = original.Company,
                FreightTerm = original.FreightTerm,
                Addresses = original.Addresses,
                PaymentTerm = original.PaymentTerm,
                ShipToAddress = original.ShipToAddress,
                QuickQuoteItems = original.QuickQuoteItems,
                QuoteId = KeyValues.GetNextQuoteId(),
                Prospect = original.Prospect,
                SalesPerson = salesPerson.AsCrmUserRef(),
                Status = PipelineStatus.Draft,
                Validity = original.Validity,
                SalesPersonNotes = original.SalesPersonNotes,
                SearchTags = original.SearchTags,
                Team = original.Team,
                PdfRowsPerPage = original.PdfRowsPerPage,
                DisplayCurrency = original.DisplayCurrency,
                QuoteLinkId = original.QuoteLinkId
            };
            result.SetReportDate();
            result.SaveToDatabase();
            MoveQuoteItems();
            //CopyQuickQuoteItems();

            return result;

            void MoveQuoteItems()
            {
                var itemList = original.Items.Select(x => x.Id).ToList();
                if (model.QuoteItemIdList.Any()) itemList = model.QuoteItemIdList;

                foreach (var quoteItemId in itemList)
                    result = MoveQuoteItemToDifferentQuote(model.Application, model.UserId, result.Id.ToString(),
                        quoteItemId).DestinationQuote;
            }
        }

        #endregion

        #region Save Quote

        public CrmQuote SaveQuote(QuoteModel model)
        {
            var repQuote = new RepositoryBase<CrmQuote>();

            var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);

            var team = crmUser.ViewConfig.Team.AsTeam();

            var quoteCalculationRequiredDueToCurrencyChange = false;

            var quote = repQuote.Find(model.Id);
            if (quote == null)
                quote = new CrmQuote
                {
                    Id = ObjectId.Parse(model.Id),
                    CreatedByUserId = model.UserId,
                    SalesPerson = crmUser.AsCrmUserRef(),
                    Team = crmUser.ViewConfig.Team
                };
            else
                quote.ModifiedByUserId = model.UserId;

            try
            {
                quote.Team = crmUser.ViewConfig.Team;
                var teamCoid = team.Location.AsLocation().GetCoid();
                if (quote.Coid != teamCoid) quote.Coid = teamCoid;
            }
            catch (Exception e)
            {
                throw new Exception("SaveQuote:Setting Coid based on Team: " + e.Message);
            }

            TeamPriceTier teamPriceTier = null;
            try
            {
                teamPriceTier = TeamPriceTier.GetForTeam(team);
            }
            catch (Exception e)
            {
                throw new Exception("SaveQuote:Getting TeamPriceTier: " + e.Message);
            }

            try
            {
                quote.Revisions = model.Revisions;
                //quote.Coid = model.Coid;
                quote.Company = model.Company;
                quote.Contact = model.Contact;
                quote.LostDate = model.LostDate;
                quote.LostTo = model.LostTo;
                quote.Prospect = model.Prospect;
                quote.SalesPerson = model.SalesPerson;
                quote.Addresses = model.Addresses;
                quote.Bid = model.Bid;
                quote.Star = model.Star;
                quote.LostComments = model.LostComments;
                quote.ShipToAddress = model.ShipToAddress;
                quote.SearchTags = model.SearchTags;
                quote.QuoteId = model.QuoteId;

                quote.Validity = model.Validity;
                quote.ValidityDays = model.ValidityDays;
                quote.RfqNumber = model.RfqNumber;
                quote.ReceivedRFQ = model.ReceivedRFQ;

                quote.PdfRowsPerPage = model.PdfRowsPerPage;

                quote.SalesPersonNotes = model.SalesPersonNotes;
                quote.CustomerNotes = model.CustomerNotes;

                quote.PoNumber = model.PoNumber;
                //quote.DeliveryDate = model.DeliveryDate;
                quote.PaymentTerm = model.PaymentTerm;
                quote.FreightTerm = model.FreightTerm;

                quote.Status = (PipelineStatus) Enum.Parse(typeof(PipelineStatus), model.Status);
                quote.SalesGroupCode = model.SalesGroupCode;
            }
            catch (Exception e)
            {
                throw new Exception("SaveQuote:Saving Basic Data: " + e.Message);
            }

            try
            {
                if (quote.Company != null)
                    if (quote.ShipToAddress != null && quote.ShipToAddress.HashCode != model.ShipToAddress.HashCode)
                    {
                        var company = quote.Company.AsCompany();
                        quote.ShipToAddress = CompanyResolver.SaveNewAddressIfNecessary(company, quote.ShipToAddress);
                    }
            }
            catch (Exception e)
            {
                throw new Exception("SaveQuote:Saving new Shipping Address: " + e.Message);
            }


            if (string.IsNullOrEmpty(quote.DisplayCurrency) || quote.DisplayCurrency != model.DisplayCurrency)
            {
                quote.DisplayCurrency = model.DisplayCurrency;
                quoteCalculationRequiredDueToCurrencyChange = true;
            }

            try
            {
                quote.CheckIfExpired();
            }
            catch (Exception e)
            {
                throw new Exception("SaveQuote:Checking if Quote is Expired: " + e.Message);
            }


            AddRemoveQuoteItems();

            try
            {
                quote.SetReportDate();
            }
            catch (Exception e)
            {
                throw new Exception("SaveQuote:Setting Report Date: " + e.Message);
            }

            quote.SaveToDatabase();

            try
            {
                AddPrimaryAddressToCompanyProspect();
            }
            catch (Exception e)
            {
                throw new Exception("SaveQuote:Adding Primary Address to Prospect: " + e.Message);
            }

            try
            {
                CalculateQuotePricesDueToCurrencyChange();
            }
            catch (Exception e)
            {
                throw new Exception("SaveQuote:Recalculate due to DisplayCurrency change: " + e.Message);
            }

            try
            {
                SaveQuoteProductAnalysisInfo();
            }
            catch (Exception e)
            {
                throw new Exception("SaveQuote:Saving Quote Product Analysis Info: " + e.Message);
            }

            return quote;

            void AddPrimaryAddressToCompanyProspect()
            {
                if (quote.Company != null)
                {
                    var changeMade = false;
                    var company = quote.Company.AsCompany();

                    if (quote.ShipToAddress != null)
                        if (company.Addresses.All(x => x.Id != quote.ShipToAddress.Id))
                        {
                            company.Addresses.Add(quote.ShipToAddress);
                            changeMade = true;
                        }

                    if (changeMade) company.SaveToDatabase();
                }

                if (quote.Prospect != null)
                {
                    var changeMade = false;
                    var prospect = quote.Prospect.AsProspect();

                    if (quote.ShipToAddress != null)
                        if (prospect.Addresses.All(x => x.Id != quote.ShipToAddress.Id))
                        {
                            prospect.Addresses.Add(quote.ShipToAddress);
                            changeMade = true;
                        }

                    if (changeMade) prospect.SaveToDatabase();
                }
            }

            void SaveQuoteProductAnalysisInfo()
            {
                try
                {
                    if (quote.Status != PipelineStatus.Draft) ProductWinLossAnalysisBuilder.AddQuote(quote);
                }
                catch (Exception e)
                {
                    EMailSupport.SendEmailToSupport(
                        $"ProductWinLossAnalysisBuilder.AddQuote(quote) Exception:{EnvironmentSettings.CurrentEnvironment} QuoteId: {quote.QuoteId}",
                        new List<string>
                        {
                            "marc.pike@howcogroup.com"
                        }, e.Message);
                }
            }
            //void AddUpdateRemoveQuickQuoteItems()
            //{
            //    // Add and Update
            //    foreach (var quickQuoteItemModel in model.QuickQuoteItems)
            //    {
            //        var quickQuoteItem = SaveQuickQuoteItem(quickQuoteItemModel);
            //        var existingItem = quote.QuickQuoteItems.SingleOrDefault(x => x.Id == quickQuoteItemModel.Id);
            //        if (existingItem == null)
            //        {
            //            quote.QuickQuoteItems.Add(quickQuoteItem.AsQuickQuoteItemRef());
            //        }
            //    }

            //    // Remove
            //    var repQuickQuoteItem = new RepositoryBase<QuickQuoteItem>();
            //    foreach (var quickQuoteItemRef in quote.QuickQuoteItems.Where(x => model.QuickQuoteItems.All(i => i.Id != x.Id)).ToList())
            //    {
            //        var quickQuoteItem = repQuickQuoteItem.Find(quickQuoteItemRef.Id);
            //        if (quickQuoteItem != null)
            //        {
            //            repQuickQuoteItem.RemoveOne(quickQuoteItem);
            //        }

            //        quote.QuickQuoteItems.Remove(quickQuoteItemRef);
            //    }

            //}

            void AddRemoveQuoteItems()
            {
                // Add and Update
                foreach (var quoteItemModel in model.Items.OrderBy(x => x.Index))
                {
                    var quoteItem = SaveQuoteItem(quoteItemModel, model.DisplayCurrency);
                    var existingItem = quote.Items.SingleOrDefault(x => x.Id == quoteItemModel.Id);
                    if (existingItem == null)
                        quote.Items.Add(quoteItem.AsQuoteItemRef());
                    else
                        //existingItem = new CrmQuoteItemRef(quoteItem);
                        existingItem = quoteItem.AsQuoteItemRef();
                }

                // Remove
                try
                {
                    var repQuoteItem = new RepositoryBase<CrmQuoteItem>();
                    foreach (var quoteItemRef in quote.Items.Where(x => model.Items.All(i => i.Id != x.Id)).ToList())
                    {
                        var quoteItem = repQuoteItem.Find(quoteItemRef.Id);
                        if (quoteItem != null) repQuoteItem.RemoveOne(quoteItem);

                        quote.Items.Remove(quoteItemRef);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("SaveQuote:Removing Quote Item: " + e.Message);
                }
            }

            void CalculateQuotePricesDueToCurrencyChange()
            {
                if (!quoteCalculationRequiredDueToCurrencyChange) return;
                foreach (var crmQuoteItemRef in quote.Items)
                {
                    var quoteItem = crmQuoteItemRef.AsQuoteItem();
                    if (quoteItem.IsQuickQuoteItem) continue;
                    if (quoteItem.IsCrozCalc)
                    {
                        quoteItem.CrozCalcItem.DisplayCurrency = quote.DisplayCurrency;
                        var crozCalcItemModel =
                            new CrozCalcItemModel(model.Application, model.UserId, quoteItem.CrozCalcItem);
                        var helperCroz = new HelperCroz();
                        helperCroz.SaveCalcItemModel(crozCalcItemModel);
                        continue;
                    }

                    if (quoteItem.IsMachinedPart) continue;

                    quoteItem.CalculateQuotePriceModel.DisplayCurrency = quote.DisplayCurrency;
                    quoteItem.QuotePrice = quoteItem.CalculateQuotePriceModel.GenerateQuotePrice(quote.DisplayCurrency,
                        model.OldCurrency, quoteItem.QuoteSource, teamPriceTier);
                    quoteItem.SaveToDatabase();
                }
            }
        }


        //private QuickQuoteItem SaveQuickQuoteItem(QuickQuoteItemModel model)
        //{
        //    var rep = new RepositoryBase<QuickQuoteItem>();

        //    if (model.Id == null) throw new Exception("QuickQuoteItem.Id is null");

        //    var quickQuoteItem = rep.Find(model.Id) ?? new QuickQuoteItem()
        //    {
        //        Id = ObjectId.Parse(model.Id),
        //        CreatedByUserId = model.UserId
        //    };

        //    quickQuoteItem.Coid = model.Coid;
        //    quickQuoteItem.Label = model.Label;
        //    quickQuoteItem.LostReason = model.LostReason;
        //    quickQuoteItem.LostTo = model.LostTo;
        //    quickQuoteItem.LostDate = model.LostDate;
        //    quickQuoteItem.OrderQuantity = model.OrderQuantity;
        //    quickQuoteItem.Cost = model.Cost;
        //    quickQuoteItem.Price = model.Price;
        //    quickQuoteItem.FinishedProduct = model.FinishedProduct;
        //    quickQuoteItem.PartSpecification = model.PartSpecification;
        //    quickQuoteItem.PartNumber = model.PartNumber;
        //    quickQuoteItem.LeadTime = model.LeadTime;
        //    quickQuoteItem.OemType = model.OemType;

        //    quickQuoteItem.SalesPersonNotes = model.SalesPersonNotes;
        //    quickQuoteItem.CustomerNotes = model.CustomerNotes;
        //    quickQuoteItem.Regret = model.Regret;

        //    return rep.Upsert(quickQuoteItem);
        //}

        private CrmQuoteItem SaveQuoteItem(QuoteItemModel model, string displayCurrency)
        {
            CrmQuoteItem quoteItem = null;
            try
            {
                quoteItem = CrmQuoteItem.Helper.FindById(model.Id) ??
                            new CrmQuoteItem {Id = ObjectId.Parse(model.Id)};
            }
            catch (Exception e)
            {
                throw new Exception("SaveQuoteItem: Error getting the QuoteItem: " + e.Message);
            }

            quoteItem.Index = model.Index;
            quoteItem.Coid = model.Coid;
            quoteItem.SearchTags = model.SearchTags;

            if (model.IsQuickQuoteItem)
            {
                try
                {
                    quoteItem.QuoteSource = QuoteSource.QuickQuoteItem;
                    quoteItem.QuickQuoteData = model.QuickQuoteData;
                }
                catch (Exception e)
                {
                    throw new Exception("SaveQuoteItem: Error getting QuickQuote Data: " + e.Message);
                }
            }
            else if (model.IsCrozCalc)
            {
                var crozModel = model.CrozCalcItem;
                quoteItem.QuoteSource = QuoteSource.CrozCalcItem;
                crozModel.DisplayCurrency = displayCurrency;
                crozModel = CrozCalcItem.Save(crozModel);
                quoteItem.CrozCalcItem = CrozCalcItem.Helper.FindById(crozModel.Id);
                quoteItem.LostReasonId = model.LostReason;
                quoteItem.LostDate = model.LostDate;
                quoteItem.LostTo = model.LostTo;
                quoteItem.PartSpecification = model.PartSpecification;
                quoteItem.PartNumber = model.PartNumber;
                quoteItem.LeadTime = model.LeadTime;
                quoteItem.OemType = model.OemType;
                quoteItem.SalesPersonNotes = model.SalesPersonNotes;
                quoteItem.CustomerNotes = model.CustomerNotes;
            }
            else if (model.MachinedPartModel != null)
            {
                try
                {
                    quoteItem.QuoteSource = QuoteSource.MachinedPart;
                    quoteItem.MachinedPartModel = model.MachinedPartModel;
                }
                catch (Exception e)
                {
                    throw new Exception("SaveQuoteItem: Error getting MachinedPart Data: " + e.Message);
                }
            }
            else
            {
                try
                {
                    quoteItem.QuotePrice = model.QuotePriceModel.AsQuotePrice();
                    quoteItem.CalculateQuotePriceModel = model.CalculateQuotePriceModel;
                }
                catch (Exception e)
                {
                    throw new Exception("SaveQuoteItem: Error Normal Quote Calc Data: " + e.Message);
                }
            }

            try
            {
                quoteItem.PartNumber = model.PartNumber;
                quoteItem.PartSpecification = model.PartSpecification;
                quoteItem.PoNumber = model.PoNumber;
                quoteItem.LostComments = model.LostComments;

                quoteItem.SalesPersonNotes = model.SalesPersonNotes;
                quoteItem.CustomerNotes = model.CustomerNotes;

                quoteItem.LeadTime = model.LeadTime;

                quoteItem.RequestedProductCode = model.RequestedProductCode;

                quoteItem.ShowProductCodeOnQuote = model.ShowProductCodeOnQuote;

                quoteItem.LostDate = model.LostDate;
                quoteItem.LostTo = model.LostTo;
                quoteItem.LostReasonId = model.LostReasonId;
                quoteItem.LostProductCode = model.LostProductCode;
                quoteItem.OemType = model.OemType;
            }
            catch (Exception e)
            {
                throw new Exception("SaveQuoteItem: Saving Basic Data: " + e.Message);
            }

            quoteItem.SaveToDatabase();
            return quoteItem;
        }

        #endregion

        #region Lose Quote

        public (List<CompetitorRef> Competitors, List<LostReasonModel> LostReasons, LostQuoteModel LostQuoteModel)
            GetNewLostQuoteModel(string application,
                string userId, string quoteId)
        {
            //var crmQuote = new RepositoryBase<CrmQuote>().Find(quoteId);
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var lostQuoteModel = new LostQuoteModel
            {
                Application = application,
                UserId = crmUser.UserId,
                QuoteId = quoteId,
                Competitor = "Unknown"
            };

            var compFilter = Competitor.Helper.FilterBuilder.Empty;
            var compProject = Competitor.Helper.ProjectionBuilder.Expression(x => x.AsCompetitorRef());
            var competitors = Competitor.Helper.FindWithProjection(compFilter, compProject).OrderBy(x => x.Name)
                .ToList();

            return (competitors, LostReasonModel.GetList(), lostQuoteModel);
        }

        public QuoteModel LostQuote(LostQuoteModel model)
        {
            var crmQuote = CrmQuote.Helper.FindById(model.QuoteId);
            var lostDate = DateTime.Now;

            CompetitorRef lostToCompetitor = null;

            if (!string.IsNullOrEmpty(model.Competitor))
            {
                var competitor = Competitor.Helper.Find(x => x.Name == model.Competitor).FirstOrDefault();
                if (competitor == null)
                {
                    competitor = new Competitor {Name = model.Competitor};
                    Competitor.Helper.Upsert(competitor);
                }

                lostToCompetitor = competitor.AsCompetitorRef();
            }

            var lostReason = LostReason.Helper.FindById(model.LostReasonId);
            if (lostReason == null)
                throw new Exception($"Missing Lost Reason: No Loss Reason for Id = {model.LostReasonId}");


            // Only losing one or more Items
            if (model.QuoteItemIdList.Any())
            {
                foreach (var quoteItemId in model.QuoteItemIdList)
                {
                    var crmQuoteItem = crmQuote.Items.SingleOrDefault(x => x.Id == quoteItemId)?.AsQuoteItem();
                    if (crmQuoteItem == null) throw new Exception("Quote Item could not be found");

                    LoseItem(crmQuoteItem, quoteItemId);
                }

                if (crmQuote.Items.All(x => x.AsQuoteItem().IsLost)) LoseQuote();
            }
            // Lost entire Quote
            else
            {
                if (lostToCompetitor != null) crmQuote.LostTo = lostToCompetitor;

                LoseQuote();
            }


            void LoseItem(CrmQuoteItem crmQuoteItem, string quoteItemId)
            {
                crmQuoteItem.LostDate = lostDate;
                crmQuoteItem.LostReasonId = lostReason.Id.ToString();
                crmQuoteItem.LostTo = lostToCompetitor;
                crmQuoteItem.LostProductCode = model.LostProductCode;
                crmQuoteItem.LostComments = model.LostComments;
                crmQuoteItem.SaveToDatabase();

                var quoteItem = crmQuote.Items.SingleOrDefault(x => x.Id == quoteItemId);
                var index = crmQuote.Items.IndexOf(quoteItem);
                crmQuote.Items[index] = crmQuoteItem.AsQuoteItemRef();
                crmQuote.SaveToDatabase();
            }

            void LoseQuote()
            {
                crmQuote.Status = PipelineStatus.Loss;
                crmQuote.LostDate = DateTime.Now;
                crmQuote.LostReasonId = lostReason.Id.ToString();
                crmQuote.LostTo = lostToCompetitor;
                crmQuote.LostComments = model.LostComments;

                foreach (var quoteItemRef in crmQuote.Items.ToList())
                {
                    var quoteItem = quoteItemRef.AsQuoteItem();

                    if (quoteItem.IsLost) continue;

                    quoteItem.LostReasonId = lostReason.Id.ToString();
                    quoteItem.LostDate = lostDate;
                    quoteItem.LostTo = lostToCompetitor;
                    quoteItem.LostComments = model.LostComments;
                    quoteItem.SaveToDatabase();

                    var onQuoteItem = crmQuote.Items.IndexOf(quoteItemRef);
                    crmQuote.Items[onQuoteItem] = quoteItem.AsQuoteItemRef();
                }

                CrmQuote.Helper.Upsert(crmQuote);
            }

            return new QuoteModel(model.Application, model.UserId, crmQuote);
        }

        #endregion
    }
}