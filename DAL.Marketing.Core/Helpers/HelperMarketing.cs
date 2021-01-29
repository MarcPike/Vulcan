using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Marketing.Core.ChartBuilders;
using DAL.Marketing.Core.ChartModels;
using DAL.Marketing.Core.Docs;
using DAL.Marketing.Core.Models;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson;
using MarketingAccount = DAL.Marketing.Core.Docs.MarketingAccount;
using MarketingAccountFolder = DAL.Marketing.Core.Docs.MarketingAccountFolder;
using MarketingAccountModel = DAL.Marketing.Core.Models.MarketingAccountModel;
using MarketingSalesTeam = DAL.Marketing.Core.Docs.MarketingSalesTeam;
using MarketingSalesTeamModel = DAL.Marketing.Core.Models.MarketingSalesTeamModel;

namespace DAL.Marketing.Core.Helpers
{
    public class HelperMarketing : IHelperMarketing
    {
        public struct AccountView
        {
            public string Id;
            public string Name;
            public string AccountType;

            public AccountView(string id, string name, string accountType)
            {
                Id = id;
                Name = name;
                AccountType = accountType;
            }
        }


        public MarketingAccountModel AddNewAccount(string application, string userId, string name, string type)
        {
            if (!Enum.TryParse(type, out MarketingAccountType accountType))
            {
                accountType = MarketingAccountType.Strategic;
            }

            var account = new MarketingAccount()
            {
                Name = name,
                MarketingAccountFolder = new MarketingAccountFolder()
                {
                    Name = "(root)"
                },
                AccountType = accountType
            };
            new RepositoryBase<MarketingAccount>().Upsert(account);
            return new MarketingAccountModel(application, userId, account);
        }

        public void RemoveAccount(string accountId)
        {
            var rep = new RepositoryBase<MarketingAccount>();
            var remove = rep.Find(accountId);
            if (remove != null)
            {
                rep.RemoveOne(remove);
            }
        }

        public List<AccountView> GetAllAccounts()
        {
            var result = new List<AccountView>();
            var strategicAccounts = GetRepository().AsQueryable().ToList();
            foreach (var strategicAccount in strategicAccounts)
            {
                var accountView = new AccountView(strategicAccount.Id.ToString(), strategicAccount.Name, strategicAccount.AccountType.ToString());
                result.Add(accountView);
            }

            return result;
        }

        public MarketingAccountModel GetAccount(string application, string userId, string accountId)
        {
            var strategicAccount = GetRepository().Find(accountId);
            if (strategicAccount == null) throw new Exception("Account not found");

            return new MarketingAccountModel(application, userId, strategicAccount);
        }

        private RepositoryBase<MarketingAccount> GetRepository()
        {
            return new RepositoryBase<MarketingAccount>();
        }

        private MarketingAccount FindAccount(string id)
        {
            var result = GetRepository().Find(id);
            if (result == null) throw new Exception("Strategic Account was not found");
            return result;
        }



        public (MarketingAccountModel Model, MarketingAccountFolder Folder, string FolderPath) AddChildFolder(string application, string userId, string accountId, string folderId, string name)
        {
            var account = FindAccount(accountId);
            var newFolder = account.AddChildNode(Guid.Parse(folderId), name, account.Id.ToString() );

            account = FindAccount(accountId);
            return (new MarketingAccountModel(application, userId, account), newFolder, GetFolderPath(accountId, newFolder.Id.ToString()));
        }

        public MarketingAccountModel RemoveFolder(string application, string userId, string accountId, string folderId)
        {
            var account = FindAccount(accountId);
            account.RemoveNode(Guid.Parse(folderId));

            account = FindAccount(accountId);
            return new MarketingAccountModel(application, userId, account);
        }

        public MarketingAccountModel MoveFolder(string application, string userId, string accountId, string moveFolderId, string originalParentId,
            string newParentId)
        {
            var account = FindAccount(accountId);
            account.MoveNode(Guid.Parse(moveFolderId), Guid.Parse(originalParentId), Guid.Parse(newParentId));

            account = FindAccount(accountId);
            return new MarketingAccountModel(application, userId, account);
        }

        public MarketingAccountFolderModel GetFolder(string application, string userId, string accountId, string folderId)
        {
            var account = FindAccount(accountId);
            var folder = account.MarketingAccountFolder.FindFolder(Guid.Parse(folderId));

            return new MarketingAccountFolderModel(application, userId, account, folder);
        }

        public MarketingAccountFolderModel SaveFolder(MarketingAccountFolderModel model)
        {
            var account = FindAccount(model.MarketingAccountId);
            var folder = account.MarketingAccountFolder.FindFolder(Guid.Parse(model.Id));

            folder.Name = model.Name;
            folder.Companies = model.Companies;
            folder.ParentObjectId = Guid.Parse(model.ParentObjectId);
            account.SaveToDatabase();

            return new MarketingAccountFolderModel(model.Application, model.UserId, account, folder);
        }

        public MarketingAccountFolder GetMarketingAccountFolder(string accountId, string folderId)
        {
            var account = FindAccount(accountId);
            var folder = account.MarketingAccountFolder.FindFolder(Guid.Parse(folderId));
            return folder;
        }

        public List<CompanyRef> GetAllCompanies(string accountId)
        {
            var account = FindAccount(accountId);
            return account.GetAllCompanies();
        }

        public List<CompanyRef> GetAllCompaniesForFolder(string accountId, string folderId)
        {
            var account = FindAccount(accountId);
            return account.GetAllCompaniesForFolder(Guid.Parse(folderId));
        }

        public MarketingAccountModel SaveAccount(MarketingAccountModel model)
        {
            if (!Enum.TryParse(model.AccountType, out MarketingAccountType accountType))
            {
                accountType = MarketingAccountType.Strategic;
            }


            var account = FindAccount(model.Id);
            var strategicAccountFolder = model.FolderNodes.AsStrategicAccountFolder();
            account.Name = model.Name;
            account.MarketingAccountFolder = strategicAccountFolder;
            account.AccountType = accountType;
            GetRepository().Upsert(account);
            account = FindAccount(model.Id);
            return new MarketingAccountModel(model.Application, model.UserId, account);
        }

        public MarketingSalesTeamModel GetNewMarketingSalesTeamModel(string application, string userId)
        {
            return new MarketingSalesTeamModel(application, userId, new MarketingSalesTeam());
        }

        public MarketingSalesTeamModel SaveMarketingSalesTeam(MarketingSalesTeamModel model)
        {
            var rep = new RepositoryBase<MarketingSalesTeam>();
            var account = rep.Find(model.Id);
            if (account == null)
            {
                account = new MarketingSalesTeam()
                {
                    Id = ObjectId.Parse(model.Id),
                };
            }

            account.Name = model.Name;
            account.SalesPersons = model.SalesPersons;
            rep.Upsert(account);
            return new MarketingSalesTeamModel(model.Application, model.UserId, account);
        }

        public List<MarketingSalesTeamRef> GetAllMarketingSalesTeams()
        {
            return new RepositoryBase<MarketingSalesTeam>().AsQueryable().OrderBy(x=>x.Name).ToList().Select(x=>x.AsMarketingSalesTeamRef()).ToList();
        }


        public List<string> GetAccountTypes()
        {
            return new List<string>() {MarketingAccountType.Strategic.ToString(), MarketingAccountType.Corporate.ToString(), MarketingAccountType.SalesAnalysis.ToString()};
        }

        public string GetFolderPath(string accountId, string folderId)
        {
            var account = new RepositoryBase<MarketingAccount>().Find(accountId);
            if (account == null) throw new Exception("Account not found");

            var folderPath = account.GetFolderPath(folderId);
            return folderPath;
        }

        public List<TestGetQuoteModel> TestGetQuotes(string accountId, string folderId, DateTime fromDate, DateTime toDate,
            List<PipelineStatus> statusFilter = null)
        {
            var result = new List<TestGetQuoteModel>();

            var account = new RepositoryBase<MarketingAccount>().Find(accountId);
            if (account == null) throw new Exception("Account not found");

            var folder = account.MarketingAccountFolder.FindFolder(Guid.Parse(folderId));
            
            if (folder == null) throw new Exception("Folder not found");

            var quoteQuery = new QuoteFetcher(account, folder, fromDate, toDate, statusFilter);
            var quotes = quoteQuery.Quotes;
            if (quotes.Any())
            {
                result.AddRange(quoteQuery.Quotes.Select(x => new TestGetQuoteModel(x)).ToList());
            }
            return result;
        }

        public List<CompanyRef> TestGetCompaniesForFolder(string accountId, string folderId, DateTime fromDate, DateTime toDate)
        {
            var account = new RepositoryBase<MarketingAccount>().Find(accountId);
            if (account == null) throw new Exception("Account not found");

            var folder = account.MarketingAccountFolder.FindFolder(Guid.Parse(folderId));
            if (folder == null) throw new Exception("Folder not found");

            var quoteQuery = new QuoteFetcher(account, folder, fromDate, toDate);
            return quoteQuery.StrategiCompaniesForThisFolder;
        }

        public (HitRateBySalesPersonChartData HitRateBySalesPersonChartData, 
                TotalDollarsBySalesPersonChartData TotalDollarsBySalesPersonChartData,
                MarginBySalesPersonChartData MaterialMargin, MarginBySalesPersonChartData SellingMargin,
                ChartQuoteHistoryModel ChartQuoteHistoryModel) 
            GetTimelineAndSalesPersonModels(string accountId, string folderId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);

            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            var margins = GetMarginBySalesPersonChartDataForQuotes(quotes);

            return (GetHitRateBySalesPersonChartDataForQuotes(quotes),
                GetTotalDollarsBySalesPersonChartDataForQuotes(displayCurrency, quotes),
                margins.MaterialMargins, margins.SellingMargins,
                GetChartQuoteHistoryModel(quotes));
        }

        public (
            HitRateByMetalCategoryChartData HitRateByMetalCategoryChartData, 
            TotalDollarsByMetalCategoryChartData TotalDollarsByMetalCategoryChartData, 
            MarginByMetalCategoryChartData MaterialMargin, MarginByMetalCategoryChartData SellingMargin,
            ChartQuoteHistoryModel ChartQuoteHistoryModel)
            GetTimelineAndMaterialModels(string accountId, string folderId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);

            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            var margins = GetMarginByMetalCategoryChartDataForQuotes(quotes);

            return (GetHitRateByMetalCategoryChartDataForQuotes(quotes),
                GetTotalDollarsByMetalCategoryChartDataForQuotes(displayCurrency, quotes),
                margins.MaterialMargins, margins.SellingMargins,
                GetChartQuoteHistoryModel(quotes));
        }

        public (
            HitRateByMetalCategoryChartData HitRateByMetalCategoryChartData,
            TotalDollarsByMetalCategoryChartData TotalDollarsByMetalCategoryChartData,
            MarginByMetalCategoryChartData MaterialMargin, MarginByMetalCategoryChartData SellingMargin,
            ChartQuoteHistoryModel ChartQuoteHistoryModel)
            GetTimelineAndMaterialModels(string displayCurrency, List<CrmQuote> quotes)
        {


            var margins = GetMarginByMetalCategoryChartDataForQuotes(quotes);

            return (GetHitRateByMetalCategoryChartDataForQuotes(quotes),
                GetTotalDollarsByMetalCategoryChartDataForQuotes(displayCurrency, quotes),
                margins.MaterialMargins, margins.SellingMargins,
                GetChartQuoteHistoryModel(quotes));
        }
        public (HitRateByCustomerChartData HitRateByCustomerChartData, 
                TotalDollarsByCustomerChartData TotalDollarsByCustomerChartData, 
                MarginByCustomerChartData MaterialMargins, MarginByCustomerChartData SellingMargins,
                ChartQuoteHistoryModel ChartQuoteHistoryModel)
            GetTimelineAndCustomerModels(string accountId, string folderId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);

            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            var customerMargins = GetMarginByCustomerChartDataForQuotes(quotes);

            return (GetHitRateByCustomerChartDataForQuotes(quotes),
                GetTotalDollarsByCustomerChartDataForQuotes(displayCurrency, quotes),
                customerMargins.MaterialMargins, customerMargins.SellingMargins,
                GetChartQuoteHistoryModel(quotes));
        }

        public (HitRateByCustomerChartData HitRateByCustomerChartData,
            TotalDollarsByCustomerChartData TotalDollarsByCustomerChartData,
            MarginByCustomerChartData MaterialMargins, MarginByCustomerChartData SellingMargins,
            ChartQuoteHistoryModel ChartQuoteHistoryModel)
            GetTimelineAndCustomerModelsForCompany(string companyId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {

            var quotes = QuoteFetcher.GetAllQuotesForCompany(companyId, fromDate, toDate);

            var customerMargins = GetMarginByCustomerChartDataForQuotes(quotes);

            return (GetHitRateByCustomerChartDataForQuotes(quotes),
                GetTotalDollarsByCustomerChartDataForQuotes(displayCurrency, quotes),
                customerMargins.MaterialMargins, customerMargins.SellingMargins,
                GetChartQuoteHistoryModel(quotes));
        }


        public (HitRateBySalesPersonChartData HitRateBySalesPersonChartData, 
                TotalDollarsBySalesPersonChartData TotalDollarsBySalesPersonChartData,
                MarginBySalesPersonChartData MaterialMargin, MarginBySalesPersonChartData SellingMargin)
            GetSalesPersonModels(string accountId, string folderId, DateTime fromDate, DateTime toDate,
                string displayCurrency)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);

            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            var margins = GetMarginBySalesPersonChartDataForQuotes(quotes);

            return (GetHitRateBySalesPersonChartDataForQuotes(quotes),
                GetTotalDollarsBySalesPersonChartDataForQuotes(displayCurrency, quotes),
                margins.MaterialMargins, margins.SellingMargins);
        }

        public (HitRateByMetalCategoryChartData HitRateByMetalCategoryChartData, 
               TotalDollarsByMetalCategoryChartData TotalDollarsByMetalCategoryChartData, 
               MarginByMetalCategoryChartData MaterialMargin, MarginByMetalCategoryChartData SellingMargin)
            GetMaterialModels(string accountId, string folderId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);

            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            var margins = GetMarginByMetalCategoryChartDataForQuotes(quotes);

            return (GetHitRateByMetalCategoryChartDataForQuotes(quotes),
                    GetTotalDollarsByMetalCategoryChartDataForQuotes(displayCurrency, quotes),
                    margins.MaterialMargins, margins.SellingMargins);
        }


        public (HitRateByCustomerChartData HitRateByCustomerChartData, 
                TotalDollarsByCustomerChartData TotalDollarsByCustomerChartData,
                MarginByCustomerChartData MaterialMargins, MarginByCustomerChartData SellingMargins)
            GetCustomerModels(string accountId, string folderId, DateTime fromDate, DateTime toDate,
                string displayCurrency)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);


            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            var customerMargins = GetMarginByCustomerChartDataForQuotes(quotes);

            return (GetHitRateByCustomerChartDataForQuotes(quotes),
                    GetTotalDollarsByCustomerChartDataForQuotes(displayCurrency, quotes),
                    customerMargins.MaterialMargins, customerMargins.SellingMargins);
        }

        public (ChartQuoteHistoryModel TimelineData,
            TotalDollarsBySalesPersonChartData TotalDollarsBySalesPerson,
            TotalDollarsByMetalCategoryChartData TotalDollarsByMetalCategory)
            GetAllChartDataForCompany(string companyId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {

            var quotes = QuoteFetcher.GetAllQuotesForCompany(companyId, fromDate, toDate);

            var timeLineChartData = GetChartQuoteHistoryModel(quotes);
            var totalDollarsByMetalCategoryChartData = GetTotalDollarsByMetalCategoryChartDataForQuotes(displayCurrency, quotes);
            var totalDollarsBySalesPersonChartData = GetTotalDollarsBySalesPersonChartDataForQuotes(displayCurrency, quotes);

            return (timeLineChartData, totalDollarsBySalesPersonChartData,totalDollarsByMetalCategoryChartData);
        }

        public AllChartDataForCompanyModel GetAllChartDataForCompanyAsync(string companyId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {

            var quotes = QuoteFetcher.GetAllQuotesForCompany(companyId, fromDate, toDate);

            ChartQuoteHistoryModel chartQuoteHistoryModel = new ChartQuoteHistoryModel();
            TotalDollarsByMetalCategoryChartData totalDollarsByMetalCategoryChartData = new TotalDollarsByMetalCategoryChartData();
            TotalDollarsBySalesPersonChartData totalDollarsBySalesPersonChartData = new TotalDollarsBySalesPersonChartData();

            var timeLineChartDataTask = new Task(() => { chartQuoteHistoryModel = GetChartQuoteHistoryModel(quotes); }); 
            var totalDollarsByMetalCategoryChartDataTask = new Task(() => { totalDollarsByMetalCategoryChartData = GetTotalDollarsByMetalCategoryChartDataForQuotes(displayCurrency, quotes); }); 
            var totalDollarsBySalesPersonChartDataTask = new Task(() => { totalDollarsBySalesPersonChartData = GetTotalDollarsBySalesPersonChartDataForQuotes(displayCurrency, quotes); });

            var tasks = new List<Task>();
            tasks.Add(timeLineChartDataTask);
            tasks.Add(totalDollarsByMetalCategoryChartDataTask);
            tasks.Add(totalDollarsBySalesPersonChartDataTask);

            Parallel.ForEach(tasks, task => task.Start());

            AllChartDataForCompanyModel result = new AllChartDataForCompanyModel();

            Task.WhenAll(tasks).ContinueWith(done =>
            {
                result = new AllChartDataForCompanyModel(chartQuoteHistoryModel, totalDollarsBySalesPersonChartData, totalDollarsByMetalCategoryChartData);
            });

            return result;

        }


        public (MarginBySalesPersonChartData MaterialMargin, MarginBySalesPersonChartData SellingMargin) GetMarginBySalesPersonChartData(string accountId, string folderId, DateTime fromDate,
            DateTime toDate)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);
            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            var marginBuilder = new AverageMarginBySalesPersonChartBuilder();
            marginBuilder.Calculate(quotes);
            return marginBuilder.GetChartData();
        }

        public (MarginByMetalCategoryChartData MaterialMargin, MarginByMetalCategoryChartData SellingMargin) GetMarginByMetalCategoryChartData(string accountId, string folderId, DateTime fromDate,
            DateTime toDate)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);
            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            var marginBuilder = new AverageMarginByMetalCategoryChartBuilder();
            marginBuilder.Calculate(quotes);
            return marginBuilder.GetChartData();
        }

        public (MarginByCustomerChartData MaterialMargins, MarginByCustomerChartData SellingMargins) GetMarginByCustomerChartData(string accountId, string folderId, DateTime fromDate,
            DateTime toDate)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);
            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            var marginBuilder = new AverageMarginByCustomerChartBuilder();
            marginBuilder.Calculate(quotes);
            return marginBuilder.GetChartData();
        }

        public HitRateBySalesPersonChartData HitRateBySalesPerson(string accountId, string folderId, DateTime fromDate, DateTime toDate)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);

            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            return GetHitRateBySalesPersonChartDataForQuotes(quotes);
        }


        public TotalDollarsBySalesPersonChartData TotalDollarsBySalesPerson(string accountId, string folderId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);

            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            return GetTotalDollarsBySalesPersonChartDataForQuotes(displayCurrency, quotes);
        }

        public TotalDollarsByMetalCategoryChartData TotalDollarsByMetalCategory(string accountId, string folderId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);

            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            return GetTotalDollarsByMetalCategoryChartDataForQuotes(displayCurrency, quotes);
        }

        public HitRateByMetalCategoryChartData HitRateByMetalCategory(string accountId, string folderId, DateTime fromDate, DateTime toDate)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);

            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            return GetHitRateByMetalCategoryChartDataForQuotes(quotes);
        }

        public TotalDollarsByCustomerChartData TotalDollarsByCustomer(string accountId, string folderId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);

            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            return GetTotalDollarsByCustomerChartDataForQuotes(displayCurrency, quotes);
        }

        public HitRateByCustomerChartData HitRateByCustomer(string accountId, string folderId, DateTime fromDate, DateTime toDate)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);

            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            return GetHitRateByCustomerChartDataForQuotes(quotes);
        }

        public ChartQuoteHistoryModel GetQuotesTimeline(string accountId, string folderId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {
            var accountAndFolder = GetAccountAndFolder(accountId, folderId);

            var quotes = new QuoteFetcher(accountAndFolder.Account, accountAndFolder.Folder, fromDate, toDate).Quotes;

            var result = GetChartQuoteHistoryModel(quotes);

            return result;
        }

        private static (MarketingAccount Account, MarketingAccountFolder Folder) GetAccountAndFolder(string accountId, string folderId)
        {
            var account = new RepositoryBase<MarketingAccount>().Find(accountId);
            if (account == null) throw new Exception("Account not found");

            var folder = account.MarketingAccountFolder.FindFolder(Guid.Parse(folderId));
            if (folder == null) throw new Exception("Folder not found");
            return (account, folder);
        }

        private (MarginBySalesPersonChartData MaterialMargins, MarginBySalesPersonChartData SellingMargins) GetMarginBySalesPersonChartDataForQuotes(List<CrmQuote> quotes)
        {
            var result = new AverageMarginBySalesPersonChartBuilder();
            result.Calculate(quotes);
            return result.GetChartData();
        }

        private async Task<MarginBySalesPersonResultModel> GetMarginBySalesPersonChartDataForQuotesAsync(List<CrmQuote> quotes)
        {
            return await new Task<MarginBySalesPersonResultModel>(() =>
            {
                var result = GetMarginBySalesPersonChartDataForQuotes(quotes);
                return new MarginBySalesPersonResultModel(result.MaterialMargins, result.SellingMargins);
            });
        }


        private (MarginByCustomerChartData MaterialMargins, MarginByCustomerChartData SellingMargins) GetMarginByCustomerChartDataForQuotes(List<CrmQuote> quotes)
        {
            var result = new AverageMarginByCustomerChartBuilder();
            result.Calculate(quotes);
            return result.GetChartData();
        }

        //private async Task<MarginByCustomerResultModel> GetMarginByCustomerChartDataForQuotesAsync(List<CrmQuote> quotes)
        //{
        //    return await new Task<MarginByCustomerResultModel>(() =>
        //    {
        //        var result = GetMarginByCustomerChartDataForQuotes(quotes);
        //        return new MarginByCustomerResultModel(result.MaterialMargins, result.SellingMargins);
        //    });
        //}


        private (MarginByMetalCategoryChartData MaterialMargins, MarginByMetalCategoryChartData SellingMargins) GetMarginByMetalCategoryChartDataForQuotes(List<CrmQuote> quotes)
        {
            var result = new AverageMarginByMetalCategoryChartBuilder();
            result.Calculate(quotes);
            return result.GetChartData();
        }

        //private async Task<MarginByMetalCategoryResultModel> GetMarginByMetalCategoryChartDataForQuotesAsync(List<CrmQuote> quotes)
        //{
        //    return await new Task<MarginByMetalCategoryResultModel>(() =>
        //    {
        //        var result = GetMarginByMetalCategoryChartDataForQuotes(quotes);
        //        return new MarginByMetalCategoryResultModel(result.MaterialMargins, result.SellingMargins);
        //    });
        //}


        private HitRateBySalesPersonChartData GetHitRateBySalesPersonChartDataForQuotes(List<CrmQuote> quotes)
        {
            var result = new HitRateBySalesPersonChartBuilder();
            result.Calculate(quotes);
            return result.GetChartData();
        }

        //private async Task<HitRateBySalesPersonChartData> GetHitRateBySalesPersonChartDataForQuotesAsync(List<CrmQuote> quotes)
        //{
        //    return await new Task<HitRateBySalesPersonChartData>(() => GetHitRateBySalesPersonChartDataForQuotes(quotes));
        //}


        private TotalDollarsBySalesPersonChartData GetTotalDollarsBySalesPersonChartDataForQuotes(string displayCurrency,
            List<CrmQuote> quotes)
        {
            var result = new TotalDollarsBySalesPersonChartBuilder();
            result.Calculate(quotes, displayCurrency);
            return result.GetChartData();
        }

        //private async Task<TotalDollarsBySalesPersonChartData> GetTotalDollarsBySalesPersonChartDataForQuotesAsync(string displayCurrency, List<CrmQuote> quotes)
        //{
        //    var myTask = new Task<TotalDollarsBySalesPersonChartData>(() => GetTotalDollarsBySalesPersonChartDataForQuotes(displayCurrency, quotes));
        //    return await myTask;
        //}

        private TotalDollarsByMetalCategoryChartData GetTotalDollarsByMetalCategoryChartDataForQuotes(string displayCurrency,
            List<CrmQuote> quotes)
        {
            var result = new TotalDollarsByMetalCategoryChartBuilder();
            result.Calculate(quotes, displayCurrency);
            return result.GetChartData();
        }

        //private async Task<TotalDollarsByMetalCategoryChartData> GetTotalDollarsByMetalCategoryChartDataForQuotesAsync(string displayCurrency, List<CrmQuote> quotes)
        //{
        //    var myTask = new Task<TotalDollarsByMetalCategoryChartData>(() => GetTotalDollarsByMetalCategoryChartDataForQuotes(displayCurrency, quotes));
        //    return await myTask;
        //}


        private HitRateByMetalCategoryChartData GetHitRateByMetalCategoryChartDataForQuotes(List<CrmQuote> quotes)
        {
            var result = new HitRateByMetalCategoryChartBuilder();
            result.Calculate(quotes);
            return result.GetChartData();
        }

        //private async Task<HitRateByMetalCategoryChartData> GetHitRateByMetalCategoryChartDataForQuotesAsync(List<CrmQuote> quotes)
        //{
        //    return await new Task<HitRateByMetalCategoryChartData>(() => GetHitRateByMetalCategoryChartDataForQuotes(quotes));
        //}


        private static TotalDollarsByCustomerChartData GetTotalDollarsByCustomerChartDataForQuotes(string displayCurrency,
            List<CrmQuote> quotes)
        {
            var result = new TotalDollarsByCustomerChartBuilder();
            result.Calculate(quotes, displayCurrency);
            return result.GetChartData();
        }

        //private async Task<TotalDollarsByCustomerChartData> GetTotalDollarsByCustomerChartDataForQuotesAsync(string displayCurrency, List<CrmQuote> quotes)
        //{
        //    return await new Task<TotalDollarsByCustomerChartData>(() => GetTotalDollarsByCustomerChartDataForQuotes(displayCurrency,quotes));
        //}


        private HitRateByCustomerChartData GetHitRateByCustomerChartDataForQuotes(List<CrmQuote> quotes)
        {
            var result = new HitRateByCustomerChartBuilder();
            result.Calculate(quotes);
            return result.GetChartData();
        }

        //private async Task<HitRateByCustomerChartData> GetHitRateByCustomerChartDataForQuotesAsync(List<CrmQuote> quotes)
        //{
        //    return await new Task<HitRateByCustomerChartData>(() => GetHitRateByCustomerChartDataForQuotes(quotes));
        //}


        private ChartQuoteHistoryModel GetChartQuoteHistoryModel(List<CrmQuote> quotes)
        {
            var helperChart = new HelperChart();
            var result = helperChart.GetChartHistoryModelForQuotes(quotes);
            helperChart.CalculateAndSortModel(result);
            return result;
        }

        //private async Task<ChartQuoteHistoryModel> GetChartQuoteHistoryModelAsync(List<CrmQuote> quotes)
        //{
        //    ChartQuoteHistoryModel result;
        //    var task = new Task<ChartQuoteHistoryModel>(() => result = GetChartQuoteHistoryModel(quotes));
        //    return await task;
        //}
    }
}
