using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;
using DAL.Vulcan.Mongo.Core.Queries;
using DAL.Vulcan.Mongo.Core.Quotes;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperQuote
    {
        QuoteModel CreateNewQuoteForCompany(string application, string userId, string coid, string companyId);

        QuoteModel CreateNewQuoteForProspect(string application, string userId, string coid, string prospectId);
        //List<QuoteModel> GetMyQuotes(string application, string userId, DateTime minDate, DateTime maxDate);

        List<QuoteModel> GetCompanyQuotes(string application, string userId, string companyId, DateTime minDate,
            DateTime maxDate);

        QuoteModel GetQuoteModel(string application, string userId, string quoteId);
        CrmQuote SaveQuote(QuoteModel model);
        CrmQuote GetQuote(string quoteId);
        CrmQuote CreateLinkedQuote(string application, string userId, string sourceQuoteId, string forCompanyId);
        CrmQuote CopyQuote(QuoteCopyModel model, bool sameQuote);
        CrmQuoteItem GetQuoteItem(string quoteItemId);

        CrmQuote CopyQuoteItemToDifferentQuote(string application, string userId, string destinationQuoteId,
            string sourceQuoteItemId);

        CrmQuote MoveQuoteItemsToNewQuote(QuoteCopyModel model);

        (CrmQuote SourceQuote, CrmQuote DestinationQuote) MoveQuoteItemToDifferentQuote(string application,
            string userId, string destinationQuoteId,
            string sourceQuoteItemId);

        (QuotePriceModel QuotePriceModel, CalculateQuotePriceModel CalculateQuotePriceModel) CalculateQuotePrice(
            CalculateQuotePriceModel model);

        QuotePipelineModel GetQuotesPipeline(string application, string userId, DateTime begDate, DateTime endDate,
            bool forTeam);

        //QuotePipelineModel GetQuotesPipelineForUser(string application, string userId);
        //QuotePipelineModel GetQuotesPipelineForCompany(string application, string userId, string companyOrProspectId);
        //QuotePipelineModel GetQuotesPipelineForTeam(string application, string userId);

        (CalculateQuotePriceModel CalculateQuotePriceModel, QuotePriceModel QuotePriceModel)
            GetNewCalculateQuotePriceModelForMadeUpCost(MadeUpCostModel model);

        (CalculateQuotePriceModel CalculateQuotePriceModel, QuotePriceModel QuotePriceModel)
            GetNewCalculateQuotePriceModelForPurchaseOrderItem(string coid, decimal costPerPound, int productId,
                OrderQuantity orderedQuantity, string application, string userId, string displayCurrency,
                string companyId);

        (CalculateQuotePriceModel CalculateQuotePriceModel, QuotePriceModel QuotePriceModel)
            GetNewCalculateQuotePriceModelForStockItem(QuoteStockItemModel model);

        QuoteMachinedPartModel GetNewMachinedPartModel(string application, string userId, string coid, int stockItemId,
            string displayCurrency);

        (CalculateQuotePriceModel CalculateQuotePriceModel, QuotePriceModel QuotePriceModel)
            GetNewCalculateQuotePriceModelForMachinedPart(QuoteMachinedPartModel model);

        (CalculateQuotePriceModel CalculateQuotePriceModel, QuotePriceModel QuotePriceModel)
            GetNewCalculateQuotePriceModelForPartNumberAndStockItem(QuotePartNumberAndStockItemModel model);

        //QuickQuoteItemModel GetNewQuickQuoteItemModel(string coid, string application, string userId);
        QuotePipelineModel GetQuotesPipelineForContact(string application, string userId, string contactId,
            string teamId);

        QuotePipelineModel GetQuotesPipelineForCompany(string application, string userId, string companyId);

        QuotesPipelineQuery GetQuotesPipelineForCompany(DateTime begDate, DateTime endDate, string companyId,
            bool showExpired);

        CompanyActivityView GetCustomerActivityView(
            string application,
            string userId,
            DateTime beginDate,
            DateTime endDate,
            string salesPersonId);

        ProspectActivityView GetProspectActivityView(
            string application,
            string userId,
            DateTime beginDate,
            DateTime endDate,
            string salesPersonId);

        QuotePipelineModel GetQuotePipelineForCustomerActivity(
            string application,
            string userId,
            DateTime beginDate,
            DateTime endDate,
            string companyOrProspectId,
            string salesPersonId,
            bool prospectsInsteadOfCompanies);

        CrmQuote MoveQuote(CrmQuote quote, Team newTeam, CrmUser newSalesPerson, string newCompanyId);

        QuoteModel LostQuote(LostQuoteModel model);

        (List<CompetitorRef> Competitors, List<LostReasonModel> LostReasons, LostQuoteModel LostQuoteModel)
            GetNewLostQuoteModel(string application, string userId, string quoteId);
    }
}