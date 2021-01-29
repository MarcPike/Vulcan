using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson;
using Vulcan.IMetal.Queries.StockItems;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperQuoteQuery: HelperBase, IHelperQuoteQuery
    {
        private readonly IHelperPerson _helperPerson;
        private readonly IHelperUser _helperUser;
        private IHelperQuote _helperQuote;
        private IHelperCompany _helperCompany;
        private readonly RepositoryBase<QuoteQuery> _repository = new RepositoryBase<QuoteQuery>();
        private readonly HelperTeam _helperTeam;

        public HelperQuoteQuery()
        {
            _helperPerson = new HelperPerson();
            _helperUser = new HelperUser(_helperPerson);
            _helperQuote = new HelperQuote();
            _helperCompany = new HelperCompany();
            _helperTeam = new HelperTeam(_helperUser);
        }

        public List<QuoteQueryModel> GetAllQueriesForUser(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var crmUserRef = crmUser.AsCrmUserRef();
            var queries = _repository.AsQueryable().Where(x=>x.SalesPerson.Id == crmUserRef.Id).ToList();
            return ConvertQuoteQueriesToModel(application, userId, queries);
        }

        public List<QuoteQueryModel> GetAllQueriesForUserTeam(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var teamRef = crmUser.ViewConfig.Team;
            var queries = _repository.AsQueryable().Where(x => x.Team.Id == teamRef.Id && x.SalesPerson.Id != userId).ToList();
            return ConvertQuoteQueriesToModel(application, userId, queries);
        }

        public QuoteQueryModel GetNewQuoteQuery(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var teamRef = crmUser.ViewConfig.Team;
            var quoteQuery = new QuoteQuery()
            {
                SalesPerson = crmUser.AsCrmUserRef(),
                Team = teamRef
            };
            return new QuoteQueryModel(application, userId, quoteQuery);
        }

        public QuoteQueryModel SaveQuoteQueryToSalesPerson(string application, string userId, string quoteQueryId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var quoteQuery = _repository.Find(quoteQueryId);
            if (quoteQuery == null) throw new Exception("QuoteQuery could not be found.");

            if (quoteQuery.SalesPerson.Id == crmUser.Id.ToString())
            {
                throw new Exception($"QuoteQuery already belongs to {crmUser.User.GetFullName()}.");
            }

            var newQuoteQuery = new QuoteQuery()
            {
                SalesPerson = crmUser.AsCrmUserRef(),
                Team = crmUser.ViewConfig.Team,
                CompanyOptions = quoteQuery.CompanyOptions,
                DateOptions = quoteQuery.DateOptions,
                QuoteOptions = quoteQuery.QuoteOptions,
                ScopeOption = quoteQuery.ScopeOption
            };

            _repository.Upsert(newQuoteQuery);
            return new QuoteQueryModel(application, userId, newQuoteQuery);
        }

        //public QuoteQueryProductOptionsHelper GetQuoteQueryProductOptionsHelper(string quoteQueryId)
        //{
        //    var quoteQuery = _repository.Find(quoteQueryId);
        //    if (quoteQuery == null) throw new Exception("You must first Save the Query!");

        //    var teamRef = quoteQuery.Team;

        //    var quoteQueryProductOptionsHelper = new QuoteQueryProductOptionsHelper(quoteQueryId);

        //    return quoteQueryProductOptionsHelper;
        //}

        public QuoteQueryProductOptionsHelper GetQuoteQueryProductOptionsHelper(string teamId)
        {
            var teamRef = _helperTeam.GetTeam(teamId).AsTeamRef();
            var quoteQueryProductOptionsHelper = new QuoteQueryProductOptionsHelper(teamRef);
            return quoteQueryProductOptionsHelper;
        }


        public QuoteQueryCompanyOptionsHelper GetQuoteQueryCompanyOptionsHelper(string teamId)
        {
            var teamRef = _helperTeam.GetTeam(teamId).AsTeamRef();
            var quoteQueryCompanyOptionsHelper = new QuoteQueryCompanyOptionsHelper(teamRef);

            return quoteQueryCompanyOptionsHelper;
        }

        public QuoteQueryTeamMemberOptionsHelper GetQuoteQueryTeamMemberOptionsHelper(string teamId)
        {
            var teamRef = _helperTeam.GetTeam(teamId).AsTeamRef();
            var quoteQueryTeamMemberOptionsHelper = new QuoteQueryTeamMemberOptionsHelper(teamRef);

            return quoteQueryTeamMemberOptionsHelper;
        }

        public QuoteQueryModel SaveQuoteQuery(QuoteQueryModel model)
        {
            if (model.Name == string.Empty) throw new Exception("Name cannot be blank");
            var quoteQuery = _repository.Find(model.Id) ?? new QuoteQuery()
            {
                Id = ObjectId.Parse(model.Id)
            };

            quoteQuery.SalesPerson = model.SalesPerson;
            quoteQuery.Team = model.Team;
            quoteQuery.CompanyOptions = model.CompanyOptions;
            quoteQuery.DateOptions = model.DateOptions;
            quoteQuery.QuoteOptions = model.QuoteOptions;
            quoteQuery.ScopeOption = model.ScopeOption;
            quoteQuery.ProductOptions = model.ProductOptions;
            quoteQuery.TeamMemberOptions = model.TeamMemberOptions;
            quoteQuery.RunHistory = model.RunHistory;
            quoteQuery.Name = model.Name;
            _repository.Upsert(quoteQuery);

            return new QuoteQueryModel(model.Application, model.UserId, quoteQuery);
        }

        public QuotePipelineModel ExecuteQuoteQuery(string application, string userId, string quoteQueryId)
        {
            var quoteQuery = _repository.Find(quoteQueryId);
            if (quoteQuery == null) throw new Exception("QuoteQuery could not be found");

            var quotes = quoteQuery.Execute();

            return new QuotePipelineModel(application, userId, quotes);
        }

        private List<QuoteQueryModel> ConvertQuoteQueriesToModel(string application, string userId, List<QuoteQuery> queries)
        {
            return (queries.Any()) ? queries.Select(x => new QuoteQueryModel(application, userId, x)).ToList() : new List<QuoteQueryModel>();
        }

    }
}
