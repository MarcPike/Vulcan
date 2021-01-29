using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAL.Vulcan.Mongo.RequestForQuoteExternal
{
    public class RfqExternal : BaseDocument
    {
        public int RequestForQuoteId { get; set; } = 0;
        public string ContactEmailAddress { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string RfqText { get; set; } = string.Empty;

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public RfqExternalStatus Status { get; set; } = RfqExternalStatus.Initial;
        public int KeywordMatches { get; set; } = 0;
        public TeamRef Team { get; set; }
        public CrmUserRef SalesPerson { get; set; }

        public DateTime Requested { get; set; }
        public DateTime? Reviewed { get; set; }

        public string AcceptText { get; set; } = string.Empty;
        public string RejectText { get; set; } = string.Empty;

        public void CountMatchingWords()
        {
            var rfqText = RfqText.ToUpper();
            KeywordMatches = 0;
            var keywords = GetKeywordsForTeam();

            foreach (var keyword in keywords.Select(x=>x.ToUpper()))
            {
                KeywordMatches += new Regex(Regex.Escape(keyword)).Matches(rfqText).Count;
            }
        }

        private List<string> GetKeywordsForTeam()
        {
            var queryHelper = new MongoRawQueryHelper<RfqTeamConfig>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Team.Id == Team.Id);
            var project = queryHelper.ProjectionBuilder.Expression(x => x.Keywords);

            var keywords = queryHelper.FindWithProjection(filter, project).FirstOrDefault();

            if (keywords == null) return new List<string>();

            return keywords.Select(x => x.ToUpper()).ToList();

        }

        public void BuildRejectedText()
        {
            var queryHelperTeamConfig = new MongoRawQueryHelper<RfqTeamConfig>();
            var filter = queryHelperTeamConfig.FilterBuilder.Where(x => x.Team.Id == Team.Id);
            var teamConfig = queryHelperTeamConfig.Find(filter).SingleOrDefault();

            if (teamConfig == null || teamConfig.RejectText == string.Empty) return;

            RejectText = teamConfig.RejectText.
                Replace("{ContactName}", ContactName).
                Replace("{RequestForQuoteId}", RequestForQuoteId.ToString().
                Replace("{ContactEmailAddress}", ContactEmailAddress)).
                Replace("{CompanyName}", CompanyName).
                Replace("{ContactPhone}",ContactPhone);
        }

        public void BuildAcceptedText()
        {
            var queryHelperTeamConfig = new MongoRawQueryHelper<RfqTeamConfig>();
            var filter = queryHelperTeamConfig.FilterBuilder.Where(x => x.Team.Id == Team.Id);
            var teamConfig = queryHelperTeamConfig.Find(filter).SingleOrDefault();

            if (teamConfig == null || teamConfig.AcceptText == string.Empty) return;

            AcceptText = teamConfig.AcceptText.
                Replace("{ContactName}", ContactName).
                Replace("{RequestForQuoteId}", RequestForQuoteId.ToString().
                Replace("{ContactEmailAddress}", ContactEmailAddress)).
                Replace("{CompanyName}", CompanyName).
                Replace("{ContactPhone}", ContactPhone);
        }


    }
}
