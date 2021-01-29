using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class QuoteQueryTeamMemberOptionsHelper
    {
        public List<CrmUserRef> AllTeamMembers { get; set; }

        public QuoteQueryTeamMemberOptionsHelper(TeamRef teamRef)
        {
            AllTeamMembers = teamRef.AsTeam().CrmUsers;
        }
    }
}