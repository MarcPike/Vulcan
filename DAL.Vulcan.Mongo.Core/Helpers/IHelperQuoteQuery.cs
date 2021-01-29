using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperQuoteQuery
    {
        List<QuoteQueryModel> GetAllQueriesForUser(string application, string userId);
        List<QuoteQueryModel> GetAllQueriesForUserTeam(string application, string userId);
        QuoteQueryModel GetNewQuoteQuery(string application, string userId);

        QuoteQueryModel SaveQuoteQueryToSalesPerson(string application, string userId, string quoteQueryId);

        QuoteQueryProductOptionsHelper GetQuoteQueryProductOptionsHelper(string teamId);
        QuoteQueryCompanyOptionsHelper GetQuoteQueryCompanyOptionsHelper(string teamId);
        QuoteQueryTeamMemberOptionsHelper GetQuoteQueryTeamMemberOptionsHelper(string teamId);

        QuoteQueryModel SaveQuoteQuery(QuoteQueryModel model);
        QuotePipelineModel ExecuteQuoteQuery(string application, string userId, string quoteQueryId);
    }
}