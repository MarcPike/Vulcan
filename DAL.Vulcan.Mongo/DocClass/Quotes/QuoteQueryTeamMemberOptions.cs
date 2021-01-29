using System.Collections.Generic;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
{
    public class QuoteQueryTeamMemberOptions
    {
        public List<CrmUserRef> TeamMembers { get; set; } = new List<CrmUserRef>();

        public bool IsUsed => TeamMembers.Count > 0;
    }
}