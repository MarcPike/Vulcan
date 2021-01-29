using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DocClass.Messages
{
    public class UserTeamMessage : BaseDocument
    {
        public CrmUserRef CreatedBy { get; set; }
        public TeamRef Team { get; set; }
        public List<CrmUserRef> OnlyIncludedUsers { get; set; } = new List<CrmUserRef>();
        public List<UserTeamMessageObject> Messages { get; set; } = new List<UserTeamMessageObject>();

        public UserTeamMessage()
        {

        }

        public UserTeamMessage(CrmUserRef createdBy, TeamRef team, List<CrmUserRef> onlyIncludedUsers,
            UserTeamMessageObject initialMessage)
        {
            CreatedBy = createdBy;
            Team = team;
            OnlyIncludedUsers = onlyIncludedUsers;
            Messages.Add(initialMessage);
        }

    }
}
