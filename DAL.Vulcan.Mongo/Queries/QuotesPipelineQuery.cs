using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.Queries
{
    public class QuotesPipelineQuery
    {
        public List<QuotePipelineProjection> Drafts { get; set; } = new List<QuotePipelineProjection>();
        public List<QuotePipelineProjection> Pending { get; set; } = new List<QuotePipelineProjection>();
        public List<QuotePipelineProjection> Won { get; set; } = new List<QuotePipelineProjection>();
        public List<QuotePipelineProjection> Lost { get; set; } = new List<QuotePipelineProjection>();
        public List<QuotePipelineProjection> Expired { get; set; } = new List<QuotePipelineProjection>();

        public QuotesPipelineQuery()
        {
        }

        public QuotesPipelineQuery(string userId, DateTime begDate, DateTime endDate, bool forTeam, bool showExpired)
        {
            var helperUser = new HelperUser(new HelperPerson());

            var crmUser = helperUser.GetCrmUser("vulcancrm", userId);
            var teamRef = crmUser.ViewConfig.Team;

            var queryHelper = new MongoRawQueryHelper<CrmQuote>();
            var baseFilter = queryHelper.FilterBuilder.Gte(x => x.ReportDate, begDate) & queryHelper.FilterBuilder.Lte(x => x.ReportDate, endDate);

            //if (forTeam)
            {
                baseFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.Team.Id, teamRef.Id);
            }
            
            
            if (!forTeam)
            {
                baseFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.SalesPerson.Id, crmUser.Id.ToString());
            }

            var projection = queryHelper.ProjectionBuilder.Expression(x =>
                new QuotePipelineProjection(
                    x.Id,
                    x.QuoteId,
                    x.Status,
                    x.Company,
                    x.Prospect,
                    x.Contact,
                    x.ShipToAddress,
                    x.SalesPerson.Id,
                    x.PoNumber,
                    x.SalesPerson.FullName,
                    x.CreateDateTime,
                    x.SubmitDate,
                    x.WonDate,
                    x.LostDate,
                    x.ExpireDate,
                    x.ReportDate,
                    x.Items,
                    x.ExportStatus.ToString(),
                    x.ExportAttempts,
                    x.ExternalOrderId,
                    x.DisplayCurrency,
                    x.Bid,
                    x.RfqNumber,
                    x.ValidityDays,
                    x.Star
                  ));

            var draftFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.Status, PipelineStatus.Draft);
            var draftSort = queryHelper.SortBuilder.Descending(x => x.CreateDateTime);
            Drafts.AddRange(queryHelper.FindWithProjection(draftFilter,projection, draftSort));

            var submitFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.Status, PipelineStatus.Submitted);
            var submitSort = queryHelper.SortBuilder.Descending(x => x.SubmitDate);
            var submittedQuotes = queryHelper.FindWithProjection(submitFilter, projection, submitSort);

            submittedQuotes = ExpireQuotes(submittedQuotes, queryHelper, submitFilter, projection, submitSort);
            Pending.AddRange(submittedQuotes); 

            var wonFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.Status, PipelineStatus.Won);
            var wonSort = queryHelper.SortBuilder.Descending(x => x.WonDate);
            Won.AddRange(queryHelper.FindWithProjection(wonFilter, projection, wonSort));

            var lostFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.Status, PipelineStatus.Loss);
            var lostSort = queryHelper.SortBuilder.Descending(x => x.LostDate);
            Lost.AddRange(queryHelper.FindWithProjection(lostFilter, projection, lostSort));

            if (showExpired)
            {
                var expiredFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.Status, PipelineStatus.Expired);
                var expiredSort = queryHelper.SortBuilder.Descending(x => x.ExpireDate);
                Expired.AddRange(queryHelper.FindWithProjection(expiredFilter, projection, expiredSort)); //, expiredSort));
                //Expired.AddRange(queryHelper.FindWithProjection(expiredFilter, projection, expiredSort).OrderByDescending(x=>x.ExpireDate)); //, expiredSort));
            }

        }

        public static QuotesPipelineQuery QuotesPipelineQueryForCompany(DateTime begDate, DateTime endDate, string companyId, bool showExpired)
        {
            var result = new QuotesPipelineQuery();
            var helperUser = new HelperUser(new HelperPerson());

            var queryHelper = new MongoRawQueryHelper<CrmQuote>();
            var baseFilter = queryHelper.FilterBuilder.Gte(x => x.ReportDate, begDate) & queryHelper.FilterBuilder.Lte(x => x.ReportDate, endDate);

                baseFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.Company.Id, companyId);

            var projection = queryHelper.ProjectionBuilder.Expression(x =>
                new QuotePipelineProjection(
                    x.Id,
                    x.QuoteId,
                    x.Status,
                    x.Company,
                    x.Prospect,
                    x.Contact,
                    x.ShipToAddress,
                    x.SalesPerson.Id,
                    x.PoNumber,
                    x.SalesPerson.FullName,
                    x.CreateDateTime,
                    x.SubmitDate,
                    x.WonDate,
                    x.LostDate,
                    x.ExpireDate,
                    x.ReportDate,
                    x.Items,
                    x.ExportStatus.ToString(),
                    x.ExportAttempts,
                    x.ExternalOrderId,
                    x.DisplayCurrency,
                    x.Bid,
                    x.RfqNumber,
                    x.ValidityDays,
                    x.Star
                  ));

            var draftFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.Status, PipelineStatus.Draft);
            var draftSort = queryHelper.SortBuilder.Descending(x => x.CreateDateTime);
            result.Drafts.AddRange(queryHelper.FindWithProjection(draftFilter, projection, draftSort));

            var submitFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.Status, PipelineStatus.Submitted);
            var submitSort = queryHelper.SortBuilder.Descending(x => x.SubmitDate);
            var submittedQuotes = queryHelper.FindWithProjection(submitFilter, projection, submitSort);

            submittedQuotes = ExpireQuotes(submittedQuotes, queryHelper, submitFilter, projection, submitSort);
            result.Pending.AddRange(submittedQuotes);

            var wonFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.Status, PipelineStatus.Won);
            var wonSort = queryHelper.SortBuilder.Descending(x => x.WonDate);
            result.Won.AddRange(queryHelper.FindWithProjection(wonFilter, projection, wonSort));

            var lostFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.Status, PipelineStatus.Loss);
            var lostSort = queryHelper.SortBuilder.Descending(x => x.LostDate);
            result.Lost.AddRange(queryHelper.FindWithProjection(lostFilter, projection, lostSort));

            if (showExpired)
            {
                var expiredFilter = baseFilter & queryHelper.FilterBuilder.Eq(x => x.Status, PipelineStatus.Expired);
                var expiredSort = queryHelper.SortBuilder.Descending(x => x.ExpireDate);
                result.Expired.AddRange(queryHelper.FindWithProjection(expiredFilter, projection, expiredSort)); //, expiredSort));
                //Expired.AddRange(queryHelper.FindWithProjection(expiredFilter, projection, expiredSort).OrderByDescending(x=>x.ExpireDate)); //, expiredSort));
            }

            return result;
        }

        private static List<QuotePipelineProjection> ExpireQuotes(List<QuotePipelineProjection> submittedQuotes, MongoRawQueryHelper<CrmQuote> queryHelper, FilterDefinition<CrmQuote> submitFilter,
            ProjectionDefinition<CrmQuote, QuotePipelineProjection> projection, SortDefinition<CrmQuote> submitSort)
        {
            var expiredQuotes = submittedQuotes
                .Where(x => x.SubmitDate != null && x.SubmitDate.Value.AddDays(x.ValidityDays + 1).Date < DateTime.Now.Date)
                .ToList();
            if (expiredQuotes.Any())
            {
                foreach (var expiredQuote in expiredQuotes)
                {
                    var findQuoteFilter = queryHelper.FilterBuilder.Eq(x => x.Id, ObjectId.Parse(expiredQuote.Id));
                    var quote = queryHelper.Find(findQuoteFilter).First();
                    quote.Status = PipelineStatus.Expired;
                    quote.ExpireDate = DateTime.Now.Date;
                    quote.SaveToDatabase();
                }

                submittedQuotes = queryHelper.FindWithProjection(submitFilter, projection, submitSort);
            }

            return submittedQuotes;
        }
    }
}
