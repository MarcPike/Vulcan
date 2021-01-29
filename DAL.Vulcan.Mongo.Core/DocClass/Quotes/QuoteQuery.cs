using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DateValues;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class QuoteQuery: BaseDocument
    {
        public CrmUserRef SalesPerson { get; set; }
        public TeamRef Team { get; set; }
        public QuoteQueryCompanyOptions CompanyOptions { get; set; } = new QuoteQueryCompanyOptions();
        public QuoteQueryDateOptions DateOptions { get; set; } = new QuoteQueryDateOptions();
        public QuoteQueryProductOptions ProductOptions { get; set; } = new QuoteQueryProductOptions();
        public QuoteQueryQuoteOptions QuoteOptions { get; set; } = new QuoteQueryQuoteOptions();
        public QuoteQueryTeamMemberOptions TeamMemberOptions { get; set; } = new QuoteQueryTeamMemberOptions();

        [JsonConverter(typeof(JsonStringEnumConverter))] // JSON.Net
        [BsonRepresentation(BsonType.String)] // Mongo
        public QuoteQueryScope ScopeOption { get; set; } = QuoteQueryScope.OnlyMyQuotes;



        public string Name { get; set; } = string.Empty;
        public List<QuoteQueryExecutionHistory> RunHistory { get; set; } = new List<QuoteQueryExecutionHistory>();

        public QuoteQuery CreateCopyFor(CrmUserRef salesPerson)
        {
            var rep = new RepositoryBase<QuoteQuery>();
            return rep.Upsert(new QuoteQuery()
            {
                SalesPerson = salesPerson,
                Team = Team,
                DateOptions = DateOptions,
                ProductOptions = ProductOptions,
                ScopeOption = ScopeOption,
                Name = Name,
            });
        }

        public List<QuoteQuery> GetAllForTeam(TeamRef teamRef)
        {
            var rep = new RepositoryBase<QuoteQuery>();
            return Queryable.Where(rep.AsQueryable(), x => x.Team.Id == teamRef.Id).ToList();
        }

        public List<QuoteQuery> GetAllForSalesPerson(CrmUserRef salesPerson)
        {
            var rep = new RepositoryBase<QuoteQuery>();
            return Queryable.Where(rep.AsQueryable(), x => x.SalesPerson.Id == salesPerson.Id).ToList();
        }

        public List<CrmQuote> Execute()
        {
            var execHistory = new QuoteQueryExecutionHistory()
            {
                ExecutedOn = DateTime.Now
            };
            try
            {
                var repQuotes = new RepositoryBase<CrmQuote>();
                //var repTeam = new RepositoryBase<Team>();
                var query = repQuotes.AsQueryable();

                query = EvaluateTeamMemberOptions(query);
                query = EvaluateScopeOptions(query);
                query = EvaluateCompanyOptions(query);
                query = EvalulateQuoteOptions(query);
                query = EvaluateDateOptions(query);

                var quotes = query.ToList();

                quotes = EvaluateProductOptions(quotes);

                execHistory.CompletedOn = DateTime.Now;
                execHistory.RowCount = quotes.Count;
                RunHistory.Add(execHistory);
                SaveToDatabase();

                return quotes;
            }
            catch (Exception e)
            {
                execHistory.Error = e;
                RunHistory.Add(execHistory);
                SaveToDatabase();
                //Console.WriteLine(e);
                throw;
            }
        }

        private IMongoQueryable<CrmQuote> EvaluateTeamMemberOptions(IMongoQueryable<CrmQuote> query)
        {
            if (TeamMemberOptions.IsUsed)
            {
                var teamMemberIds = TeamMemberOptions.TeamMembers.Select(x => x.Id).ToList();
                query = query.Where(x => teamMemberIds.Contains(x.SalesPerson.Id));

                //var predicate = PredicateBuilder.False<CrmQuote>();
                //foreach (var salesPerson in TeamMemberOptions.TeamMembers)
                //{
                //    query = query.Where(x => x.SalesPerson.Id == salesPerson.Id);
                //    //predicate.Or(x => x.SalesPerson.Id == salesPerson.Id);
                //}


            }
            return query;
        }

        private List<CrmQuote> EvaluateProductOptions(List<CrmQuote> quotes)
        {
            if (ProductOptions.IsUsed)
            {

                foreach (var crmQuote in quotes.ToList())
                {
                    var quoteValid = false;
                    foreach (var crmQuoteItem in crmQuote.Items.Select(x=>x.AsQuoteItem()))
                    {
                        if (ProductOptions.QuoteItemPassed(crmQuoteItem))
                        {
                            quoteValid = true;
                        }
                    }

                    if (!quoteValid)
                    {
                        quotes.Remove(crmQuote);
                    }
                }
            }

            return quotes;
        }

        private IMongoQueryable<CrmQuote> EvaluateDateOptions(IMongoQueryable<CrmQuote> query)
        {
            if (DateOptions.DateRange != string.Empty)
            {
                var dateValue = new DateValue();
                var dateRange = dateValue.GetDateRangeFor(DateOptions.DateRange);
                query = query.Where(x =>
                    x.ReportDate >= dateRange.BegDate && x.ReportDate <= dateRange.EndDate);
            }
            return query;
        }

        private IMongoQueryable<CrmQuote> EvalulateQuoteOptions(IMongoQueryable<CrmQuote> query)
        {
            if (QuoteOptions.IsUsed)
            {
                if (QuoteOptions.RfqNumber != string.Empty)
                {
                    query = query.Where(x => x.RfqNumber.ToUpper().Contains(QuoteOptions.RfqNumber.ToUpper()));
                }

                if (QuoteOptions.CustomerNotesContain != string.Empty)
                {
                    query = query.Where(x => x.CustomerNotes.ToUpper().Contains(QuoteOptions.CustomerNotesContain.ToUpper()));
                }

                if (QuoteOptions.SalesPersonNotesContain != string.Empty)
                {
                    query = query.Where(x => x.SalesPersonNotes.ToUpper().Contains(QuoteOptions.SalesPersonNotesContain.ToUpper()));
                }
            }

            return query;
        }

        private IMongoQueryable<CrmQuote> EvaluateCompanyOptions(IMongoQueryable<CrmQuote> query)
        {
            if (CompanyOptions.IsUsed)
            {
                if (CompanyOptions.Companies.Count > 0)
                {
                    var companyIds = CompanyOptions.Companies.Select(x => x.Id).ToList();
                    query = query.Where(x => companyIds.Contains(x.Company.Id));


                    ////var predicate = PredicateBuilder.True<CrmQuote>();
                    //foreach (var companyRef in CompanyOptions.Companies)
                    //{
                    //    //predicate.Or(x => x.Company.Id == companyRef.Id);
                    //    query = query.Where(x=>x.Company.Id == companyRef.Id);
                    //}

                    
                }

                if (CompanyOptions.Contacts.Count > 0)
                {

                    var contactIds = CompanyOptions.Contacts.Select(x => x.Id).ToList();
                    query = query.Where(x => contactIds.Contains(x.Contact.Id));

                    //var predicate = PredicateBuilder.True<CrmQuote>();
                    //foreach (var contactRef in CompanyOptions.Contacts)
                    //{
                    //    query = query.Where(x => x.Contact.Id == contactRef.Id);
                    //    //predicate.Or(x => x.Contact.Id == contactRef.Id);
                    //}

                    //query = query.Where(predicate);
                }
            }

            return query;
        }

        private IMongoQueryable<CrmQuote> EvaluateScopeOptions(IMongoQueryable<CrmQuote> query)
        {
            switch (ScopeOption)
            {
                case QuoteQueryScope.OnlyMyQuotes:
                {
                    query = (IMongoQueryable<CrmQuote>) Queryable.Where(query, x => x.SalesPerson.Id == SalesPerson.Id);
                }
                break;
                case QuoteQueryScope.OnlyMyTeam:
                {
                    var crmUser = SalesPerson.AsCrmUser();
                    var team = crmUser.ViewConfig.Team;

                query = (IMongoQueryable<CrmQuote>) Queryable.Where(query, x => x.Team.Id == team.Id);
                }
                break;
                //case QuoteQueryScope.AllTeams:
                //break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return query;
        }
    }
}
