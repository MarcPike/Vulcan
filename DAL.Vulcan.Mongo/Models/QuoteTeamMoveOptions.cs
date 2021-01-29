using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Models
{
    public class QuoteTeamMoveOptionsModel
    {
        public List<QuoteTeamUsers> Teams { get; set; } = new List<QuoteTeamUsers>();
        public QuoteTeamMoveOptionsModel()
        {
            foreach (var team in new RepositoryBase<Team>().AsQueryable().ToList().OrderBy(x=>x.Name).ToList())
            {
                Teams.Add(new QuoteTeamUsers(team));
            }
        }
    }

    public class QuoteTeamUsers
    {
        public string TeamId { get; set; }
        public string TeamName { get; set; }
        public List<CrmUserRef> SalesPersons { get; set; }

        public QuoteTeamUsers(Team team)
        {
            TeamId = team.Id.ToString();
            TeamName = team.Name;
            SalesPersons = team.CrmUsers.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
        }
    }
}
