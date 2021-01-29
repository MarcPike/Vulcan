using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.RequestForQuoteExternal;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class RfqTeamConfigModel
    {
        public string Id { get; set; }
        public string Application { get; set; }
        public string UserId { get; set; }
        public TeamRef Team { get; set; }
        public List<string> Keywords { get; set; }
        public string AcceptText { get; set; }
        public string RejectText { get; set; }

        public List<string> MessageConstants = new List<string>() 
        {
            "{RequestForQuoteId}",
            "{ContactName}",
            "{CompanyName}",
            "{ContactEmailAddress}",
            "{ContactPhone}"
        };

        public RfqTeamConfigModel()
        {
        }

        public RfqTeamConfigModel(string application, string userId, RequestForQuoteExternal.RfqTeamConfig w)
        {
            Id = w.Id.ToString();
            Team = w.Team;
            Keywords = w.Keywords;
            Application = application;
            UserId = userId;
            AcceptText = w.AcceptText;
            RejectText = w.RejectText;

        }
    }
}
