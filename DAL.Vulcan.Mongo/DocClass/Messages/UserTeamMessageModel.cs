using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Messages
{
    public class UserTeamMessageModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public TeamRef Team { get; set; }
        public CrmUserRef CreatedBy { get; set; }
        public DateTime CreateDateTime { get; set; }
        public List<UserTeamMessageObject> Messages { get; set; } = new List<UserTeamMessageObject>();
        public List<CrmUserRef> Recipients { get; set; } = new List<CrmUserRef>();

        public UserTeamMessageModel()
        {
        }

        public UserTeamMessageModel(string application, string userId, UserTeamMessage message)
        {
            Application = application;
            UserId = userId;

            Id = message.Id.ToString();
            Team = message.Team;
            CreatedBy = message.CreatedBy;
            CreateDateTime = message.CreateDateTime;
            Messages = message.Messages;
            Recipients = (message.OnlyIncludedUsers.Any()) ? message.OnlyIncludedUsers : message.Team.AsTeam().CrmUsers;

        }
    }
}