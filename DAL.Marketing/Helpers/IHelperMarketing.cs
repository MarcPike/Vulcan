using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Marketing.ChartModels;
using DAL.Marketing.Docs;
using DAL.Marketing.Models;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using MarketingAccountFolder = DAL.Marketing.Docs.MarketingAccountFolder;
using MarketingAccountModel = DAL.Marketing.Models.MarketingAccountModel;
using MarketingSalesTeamModel = DAL.Marketing.Models.MarketingSalesTeamModel;

namespace DAL.Marketing.Helpers
{
    public interface IHelperMarketing
    {
        MarketingAccountModel AddNewAccount(string application, string userId, string name, string type);
        void RemoveAccount(string accountId);

        List<HelperMarketing.AccountView> GetAllAccounts();

        MarketingAccountModel GetAccount(string application, string userId, string accountId);

        (MarketingAccountModel Model, MarketingAccountFolder Folder, string FolderPath) AddChildFolder(string application, string userId, string accountId, string folderId, string name);
        MarketingAccountModel RemoveFolder(string application, string userId, string accountId, string folderId);
        MarketingAccountModel MoveFolder(string application, string userId, string accountId, string moveFolderId, string originalParentId, string newParentId);

        MarketingAccountFolderModel GetFolder(string application, string userId, string accountId, string folderId);
        MarketingAccountFolderModel SaveFolder(MarketingAccountFolderModel model);

        string GetFolderPath(string accountId, string folderId);
        MarketingAccountFolder GetMarketingAccountFolder(string accountId, string folderId);

        List<CompanyRef> GetAllCompanies(string accountId);
        List<CompanyRef> GetAllCompaniesForFolder(string accountId, string folderId);

        MarketingAccountModel SaveAccount(MarketingAccountModel model);

        MarketingSalesTeamModel GetNewMarketingSalesTeamModel(string application, string userId);

        MarketingSalesTeamModel SaveMarketingSalesTeam(MarketingSalesTeamModel model);

        List<MarketingSalesTeamRef> GetAllMarketingSalesTeams();

        List<string> GetAccountTypes();

        List<TestGetQuoteModel> TestGetQuotes(string accountId, string folderId, DateTime fromDate,
            DateTime toDate, List<PipelineStatus> statusFilter = null);

        List<CompanyRef> TestGetCompaniesForFolder(string accountId, string folderId, DateTime fromDate, DateTime toDate);

        HitRateBySalesPersonChartData HitRateBySalesPerson(string accountId, string folderId, DateTime fromDate,
            DateTime toDate);

        TotalDollarsBySalesPersonChartData TotalDollarsBySalesPerson(string accountId, string folderId, DateTime fromDate,
            DateTime toDate, string displayCurrency);

        TotalDollarsByMetalCategoryChartData TotalDollarsByMetalCategory(string accountId, string folderId,
            DateTime fromDate, DateTime toDate, string displayCurrency);

        HitRateByMetalCategoryChartData HitRateByMetalCategory(string accountId, string folderId,
            DateTime fromDate, DateTime toDate);

        TotalDollarsByCustomerChartData TotalDollarsByCustomer(string accountId, string folderId,
            DateTime fromDate, DateTime toDate, string displayCurrency);
        HitRateByCustomerChartData HitRateByCustomer(string accountId, string folderId,
            DateTime fromDate, DateTime toDate);

        ChartQuoteHistoryModel GetQuotesTimeline(string accountId, string folderId,
            DateTime fromDate, DateTime toDate, string displayCurrency);

        (HitRateBySalesPersonChartData HitRateBySalesPersonChartData,
            TotalDollarsBySalesPersonChartData TotalDollarsBySalesPersonChartData,
            MarginBySalesPersonChartData MaterialMargin, MarginBySalesPersonChartData SellingMargin,
            ChartQuoteHistoryModel ChartQuoteHistoryModel)
            GetTimelineAndSalesPersonModels(string accountId, string folderId, DateTime fromDate, DateTime toDate,
                string displayCurrency);

        (HitRateByMetalCategoryChartData HitRateByMetalCategoryChartData,
         TotalDollarsByMetalCategoryChartData TotalDollarsByMetalCategoryChartData,
         MarginByMetalCategoryChartData MaterialMargin, MarginByMetalCategoryChartData SellingMargin,
         ChartQuoteHistoryModel ChartQuoteHistoryModel)
            GetTimelineAndMaterialModels(string accountId, string folderId, DateTime fromDate, DateTime toDate,
                string displayCurrency);

        (HitRateByCustomerChartData HitRateByCustomerChartData, 
         TotalDollarsByCustomerChartData TotalDollarsByCustomerChartData, 
         MarginByCustomerChartData MaterialMargins, MarginByCustomerChartData SellingMargins,
         ChartQuoteHistoryModel ChartQuoteHistoryModel)
            GetTimelineAndCustomerModels(string accountId, string folderId, DateTime fromDate, DateTime toDate,
                string displayCurrency);

        (HitRateByCustomerChartData HitRateByCustomerChartData,
            TotalDollarsByCustomerChartData TotalDollarsByCustomerChartData,
            MarginByCustomerChartData MaterialMargins, MarginByCustomerChartData SellingMargins,
            ChartQuoteHistoryModel ChartQuoteHistoryModel)
            GetTimelineAndCustomerModelsForCompany(string companyId, DateTime fromDate, DateTime toDate, string displayCurrency);

        (HitRateByMetalCategoryChartData HitRateByMetalCategoryChartData,
            TotalDollarsByMetalCategoryChartData TotalDollarsByMetalCategoryChartData,
            MarginByMetalCategoryChartData MaterialMargin, MarginByMetalCategoryChartData SellingMargin)
            GetMaterialModels(string accountId, string folderId, DateTime fromDate, DateTime toDate,
                string displayCurrency);


        (HitRateBySalesPersonChartData HitRateBySalesPersonChartData,
            TotalDollarsBySalesPersonChartData TotalDollarsBySalesPersonChartData,
            MarginBySalesPersonChartData MaterialMargin, MarginBySalesPersonChartData SellingMargin)
            GetSalesPersonModels(string accountId, string folderId, DateTime fromDate, DateTime toDate,
                string displayCurrency);

        (HitRateByCustomerChartData HitRateByCustomerChartData, 
         TotalDollarsByCustomerChartData TotalDollarsByCustomerChartData,
         MarginByCustomerChartData MaterialMargins, MarginByCustomerChartData SellingMargins)
            GetCustomerModels(string accountId, string folderId, DateTime fromDate, DateTime toDate,
                string displayCurrency);

        (ChartQuoteHistoryModel TimelineData,
            TotalDollarsBySalesPersonChartData TotalDollarsBySalesPerson,
            TotalDollarsByMetalCategoryChartData TotalDollarsByMetalCategory)
            GetAllChartDataForCompany(string companyId, DateTime fromDate, DateTime toDate, string displayCurrency);

        (MarginBySalesPersonChartData MaterialMargin, MarginBySalesPersonChartData SellingMargin) GetMarginBySalesPersonChartData(
                string accountId, string folderId, DateTime fromDate, DateTime toDate);

        (MarginByMetalCategoryChartData MaterialMargin, MarginByMetalCategoryChartData SellingMargin) GetMarginByMetalCategoryChartData(
            string accountId, string folderId, DateTime fromDate, DateTime toDate);



        (MarginByCustomerChartData MaterialMargins, MarginByCustomerChartData SellingMargins) GetMarginByCustomerChartData(string accountId, string folderId, DateTime fromDate,
            DateTime toDate);

        AllChartDataForCompanyModel GetAllChartDataForCompanyAsync(string companyId, DateTime fromDate, DateTime toDate, string displayCurrency);

        (HitRateByMetalCategoryChartData HitRateByMetalCategoryChartData,
            TotalDollarsByMetalCategoryChartData TotalDollarsByMetalCategoryChartData,
        MarginByMetalCategoryChartData MaterialMargin, MarginByMetalCategoryChartData SellingMargin,
        ChartQuoteHistoryModel ChartQuoteHistoryModel)
        GetTimelineAndMaterialModels(string displayCurrency, List<CrmQuote> quotes);

    }
}