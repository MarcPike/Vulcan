using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.ValueBucket;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Models
{
    public struct QuotePipelineModel
    {
        public List<QuoteMiniModel> Drafts { get; set; }
        public List<QuoteMiniModel> Pending { get; set; }
        public List<QuoteMiniModel> Won { get; set; }
        public List<QuoteMiniModel> Lost { get; set; }
        public List<QuoteMiniModel> Expired { get; set; }

        public QuotePipelineModel(string application, string userId, List<CrmQuote> quotes)
        {
            Drafts = new List<QuoteMiniModel>();
            Pending = new List<QuoteMiniModel>();
            Won = new List<QuoteMiniModel>();
            Lost = new List<QuoteMiniModel>();
            Expired = new List<QuoteMiniModel>();

            quotes = CrmQuote.CheckForExpiredQuotes(quotes);
            using (var quoteBucket = new ValueBucket<CrmQuote, PipelineStatus>())
            {
                var results = quoteBucket.Execute(quotes, quote => quote.Status);
                foreach (var bucketValue in results)
                    try
                    {
                        switch (bucketValue.GroupBy)
                        {
                            case PipelineStatus.Draft:
                                Drafts.AddRange(bucketValue.Documents
                                    .Select(x => new QuoteMiniModel(x, PipelineStatus.Draft)).ToList());
                                break;
                            case PipelineStatus.Submitted:
                                Pending.AddRange(bucketValue.Documents
                                    .Select(x => new QuoteMiniModel(x, PipelineStatus.Submitted)).ToList());
                                break;
                            case PipelineStatus.Won:
                                Won.AddRange(bucketValue.Documents
                                    .Select(x => new QuoteMiniModel(x, PipelineStatus.Won)).ToList());
                                break;
                            case PipelineStatus.Loss:
                                Lost.AddRange(bucketValue.Documents
                                    .Select(x => new QuoteMiniModel(x, PipelineStatus.Loss)).ToList());
                                break;
                            case PipelineStatus.Expired:
                                Expired.AddRange(bucketValue.Documents
                                    .Select(x => new QuoteMiniModel(x, PipelineStatus.Expired)).ToList());
                                break;
                            //case PipelineStatus.Sent:
                            //    Lost.AddRange(bucketValue.Documents.Select(x => new QuoteMiniModel(x, PipelineStatus.Sent)).ToList());
                            //    break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"QuoteId: {bucketValue.Documents} has error: {e.Message}");
                        //throw;
                    }
            }

            Drafts = Drafts.OrderByDescending(x => x.CreateDateTime).ToList();
            Pending = Pending.OrderByDescending(x => x.SubmitDate).ToList();
            Won = Won.OrderByDescending(x => x.WonDate).ToList();
            Lost = Lost.OrderByDescending(x => x.LostDate).ToList();
            Expired = Expired.OrderByDescending(x => x.LostDate).ToList();
        }
    }
}