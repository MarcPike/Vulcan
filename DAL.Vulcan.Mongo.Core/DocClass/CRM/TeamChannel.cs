using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class TeamChannel: BaseDocument
    {
        public TeamRef Team { get; set; }

        public Stack<TeamMessage> Messages { get; set; } = new Stack<TeamMessage>();

        public void AddMessage(UserRef user, string message)
        {
            Messages.Push(new TeamMessage(user, message));
            SaveToDatabase();
        }

        public TeamChannel Save()
        {
            SaveToDatabase();
            return this;
        }

        public static TeamChannel GetTeamChannel(TeamRef team)
        {
            var id = ObjectId.Parse(team.Id);
            var rep = new RepositoryBase<TeamChannel>();
            var result = rep.AsQueryable().SingleOrDefault(x => x.Id == id);
            if (result == null)
            {
                result = new TeamChannel() {Team =  team};
                rep.Upsert(result);
            }
            return result;
        }

    }
}