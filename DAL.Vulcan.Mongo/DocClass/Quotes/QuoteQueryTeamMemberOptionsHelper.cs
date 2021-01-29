using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Quotes
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