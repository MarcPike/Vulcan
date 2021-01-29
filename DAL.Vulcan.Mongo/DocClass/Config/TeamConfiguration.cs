using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Config
{
    public class TeamConfiguration : BaseDocument
    {
        public TeamRef Team { get; set; }
        public string Name { get; set; } 
        public Configuration Configuration { get; set; } = new Configuration();

        public TeamConfiguration()
        {

        }

        public TeamConfiguration(TeamRef team, string name)
        {
            Team = team;
            Name = name;
        }
    }
}