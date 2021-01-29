using System.Linq;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Config;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class TeamConfigurationModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }
        public TeamRef Team { get; set; }
        public Configuration Configuration { get; set; }

        public TeamConfigurationModel()
        {

        }

        public TeamConfigurationModel(string application, string userId, TeamConfiguration config)
        {
            Id = config.Id.ToString();
            Team = config.Team;
            Configuration = config.Configuration;
            Name = config.Name;
            Application = application;
            UserId = userId;

        }

        public static TeamConfigurationModel GetForTeam(string application, string userId, TeamRef team, string name)
        {
            var rep = new RepositoryBase<TeamConfiguration>();
            var result = rep.AsQueryable().SingleOrDefault(x => x.Team.Id == team.Id);
            if (result == null)
            {
                result = new TeamConfiguration(team, name);
                rep.Upsert(result);
            }

            return new TeamConfigurationModel(application, userId, result);
        }
    }
}