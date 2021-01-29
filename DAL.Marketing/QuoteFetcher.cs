using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.TimeKeeper;
using DAL.Vulcan.Mongo.DocClass.Companies;
using MongoDB.Bson;
using MarketingAccount = DAL.Marketing.Docs.MarketingAccount;
using MarketingAccountFolder = DAL.Marketing.Docs.MarketingAccountFolder;
using MarketingAccountType = DAL.Marketing.Docs.MarketingAccountType;

namespace DAL.Marketing
{
    public class QuoteFetcher
    {
        public MarketingAccount MarketingAccount { get; set; }
        public MarketingAccountFolder MarketingAccountFolder { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public List<PipelineStatus> StatusFilter = new List<PipelineStatus>() {PipelineStatus.Expired, PipelineStatus.Loss, PipelineStatus.Submitted, PipelineStatus.Won};

        public List<CompanyRef> StrategiCompaniesForThisFolder
        {
            get
            {
                var result = new List<CompanyRef>();
                if ((MarketingAccount.AccountType == MarketingAccountType.Strategic) || (MarketingAccount.AccountType == MarketingAccountType.SalesAnalysis))
                {
                    result.AddRange(MarketingAccountFolder.GetAllCompanies());
                }
                return result;
            }
        }


        private readonly RepositoryBase<MarketingAccount> _repAccount = new RepositoryBase<MarketingAccount>();

        public List<CrmQuote> Quotes
        {
            get
            {
                var quoteQuery = GetQueryResults();

                return quoteQuery;
            }
        }

        public QuoteFetcher(MarketingAccount account, MarketingAccountFolder folder, DateTime fromDate, DateTime toDate, List<PipelineStatus> statusFilter = null)
        {
            MarketingAccount = account;
            MarketingAccountFolder = folder;
            FromDate = fromDate;
            ToDate = toDate;
            if (statusFilter != null)
            {
                StatusFilter = statusFilter;
            }
        }

        private List<CrmQuote> GetQueryResults()
        {
            var quoteQuery = new RepositoryBase<CrmQuote>().AsQueryable();

            quoteQuery = quoteQuery.Where(x => x.Company != null && x.CreateDateTime >= FromDate && x.CreateDateTime <= ToDate);

            quoteQuery = quoteQuery.Where(x => x.Status != PipelineStatus.Draft);

            var results = quoteQuery.ToList();

            var removeExtraLosses = new List<ObjectId>();
            var linkedQuotes = results.Where(x => x.QuoteLinkId != Guid.Empty).ToList();
            foreach (var quoteLinkId in linkedQuotes.Select(x=>x.QuoteLinkId).Distinct())
            {
                var linkedQuoteSet = linkedQuotes.Where(x => x.QuoteLinkId == quoteLinkId).ToList();

                IfAllLosersKeepTheOriginalOnly(linkedQuoteSet, removeExtraLosses);

                IfAnyWinnersIgnoreTheLosers(linkedQuoteSet, removeExtraLosses);
            }

            RemoveDuplicateLinkLosses(removeExtraLosses, results);

            if (MarketingAccount.AccountType == MarketingAccountType.Strategic)
            {
                var strategicCompanyIds = StrategiCompaniesForThisFolder;
                results = results.Where(x => strategicCompanyIds.Any(c => c.Id == x.Company.Id)).ToList();
            }
            else if (MarketingAccount.AccountType == MarketingAccountType.Corporate)
            {
                var strategicCompanyIds = GetAllStrategicCompanyIds();
                results = results.Where(x => strategicCompanyIds.All(c => c != x.Company.Id)).ToList();
            }
            else if (MarketingAccount.AccountType == MarketingAccountType.SalesAnalysis)
            {
                var folderCompanyIds = MarketingAccountFolder.GetAllCompanies().Select(x=>x.Id).ToList();
                results = results.Where(x => folderCompanyIds.All(c => c == x.Company.Id)).ToList();
            }


            if (MarketingAccountFolder.SalesTeams.Any())
            {
                var salesTeamIds = MarketingAccountFolder.SalesTeams.Select(x => x.Id).ToList();

                results = results.Where(x => salesTeamIds.Any(s => s == x.Team.Id)).ToList();
            }

            if (MarketingAccountFolder.MarketingSalesTeams.Any())
            {
                var salesPersonIds = new List<string>();
                foreach (var marketingSalesTeamRef in MarketingAccountFolder.MarketingSalesTeams)
                {
                    var marketingTeam = marketingSalesTeamRef.AsStrategicSalesTeam();

                    salesPersonIds.AddRange(marketingTeam.SalesPersons.Select(x => x.Id));
                }

                if (salesPersonIds.Any())
                {
                    results = results.Where(x => salesPersonIds.Any(s => s == x.SalesPerson.Id)).ToList();
                }
            }



            return results;
        }

        private static void RemoveDuplicateLinkLosses(List<ObjectId> removeExtraLosses, List<CrmQuote> results)
        {
            foreach (var removeExtraLoss in removeExtraLosses)
            {
                var duplicateLoss = results.First(x => x.Id == removeExtraLoss);
                results.Remove(duplicateLoss);
            }
        }

        private static void IfAnyWinnersIgnoreTheLosers(List<CrmQuote> linkedQuoteSet, List<ObjectId> removeExtraLosses)
        {
            if (linkedQuoteSet.Any(x => x.Status == PipelineStatus.Won))
            {
                removeExtraLosses.AddRange(linkedQuoteSet.Where(x => x.Status != PipelineStatus.Won).Select(x => x.Id)
                    .ToList());
            }
        }

        private static void IfAllLosersKeepTheOriginalOnly(List<CrmQuote> linkedQuoteSet, List<ObjectId> removeExtraLosses)
        {
            if (linkedQuoteSet.All(x => x.Status == PipelineStatus.Loss || x.Status == PipelineStatus.Expired))
            {
                var keepOriginal = linkedQuoteSet.OrderBy(x => x.CreateDateTime).First();
                removeExtraLosses.AddRange(linkedQuoteSet.Where(x => x.Id != keepOriginal.Id).Select(x => x.Id).ToList());
            }
        }


        private List<string> GetAllStrategicCompanyIds()
        {
            
            var result = new List<string>();

            var strategicAccounts =
                _repAccount.AsQueryable().Where(x => x.AccountType == MarketingAccountType.Strategic);

            foreach (var strategicAccount in strategicAccounts)
            {
                    result.AddRange(strategicAccount.GetAllCompanies().Select(x => x.Id));
            }



            return result;
        }

        public static List<Company> GetAllStrategicCompanies()
        {

            var result = new List<Company>();

            var strategicAccounts =
                new RepositoryBase<MarketingAccount>().AsQueryable().Where(x => x.AccountType == MarketingAccountType.Strategic);

            foreach (var strategicAccount in strategicAccounts)
            {
                result.AddRange(strategicAccount.GetAllCompanies().ToList().Select(x => x.AsCompany()).ToList());
            }

            return result;
        }

        public static List<CrmQuote> GetAllQuotesForCompany(string companyId, DateTime fromDate, DateTime toDate)
        {
            return new RepositoryBase<CrmQuote>().AsQueryable().Where(x=>x.ReportDate >= fromDate && x.ReportDate <= toDate && x.Company != null && x.Company.Id == companyId && x.Status != PipelineStatus.Draft).ToList();
        }

    }

}
