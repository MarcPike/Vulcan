using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Messages
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
