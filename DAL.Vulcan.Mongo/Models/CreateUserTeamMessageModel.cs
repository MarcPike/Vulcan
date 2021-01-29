using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Messages;

namespace Vulcan.WebApi2.Models
{
    public class CreateUserTeamMessageModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public CrmUserRef CreatedBy { get; set; }
        public TeamRef Team { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Mood { get; set; } = MessageMood.Normal.ToString();
        public List<CrmUserRef> Recipients { get; set; } = new List<CrmUserRef>();

        public CreateUserTeamMessageModel()
        {

        }

        public CreateUserTeamMessageModel(string application, string userId, CrmUser user)
        {
            Team = user.ViewConfig.Team;
            Recipients = user.ViewConfig.Team.AsTeam().CrmUsers;
            CreatedBy = user.AsCrmUserRef();

            Application = application;
            UserId = userId;

        }
    }
}