using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Marketing.Core.Helpers
{
    public interface IHelperChart
    {
        ChartQuoteHistoryModel GetChartHistoryModelForQuotes(List<QuoteDataForQuoteHistory> quotes);
        void AddMaterialTotals(ChartQuoteHistoryModel model, List<QuoteDataForQuoteHistory> quotes);
        void AddCompanyTotals(ChartQuoteHistoryModel model, List<QuoteDataForQuoteHistory> quotes);
        void CalculateAndSortModel(ChartQuoteHistoryModel model);
        List<QuoteDataForQuoteHistory> GetQuoteHistoryForTeam(string teamId);
        List<QuoteDataForQuoteHistory> GetQuoteHistoryForAllQuotes(string coid);
        List<QuoteDataForQuoteHistory> GetAllQuotesHistoryForCompany(string coid, string companyId);
        List<QuoteDataForQuoteHistory> GetQuoteHistoryForTeamAndCompany(string teamId, string companyId);
        List<QuoteDataForQuoteHistory> GetAllQuoteHistoryForStrategicAccount(string strategicAccountId);
        ChartQuoteHistoryModel GetChartHistoryModelForQuotes(List<CrmQuote> quotes);
        //void AddStatusTotals(ChartQuoteHistoryModel model, QuoteDataForQuoteHistory quote, decimal quoteTotal);

    }
}