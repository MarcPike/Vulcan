using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.DocClass.Config
{
    public class UserConfiguration : BaseDocument
    {
        public CrmUserRef User { get; set; }
        public string Name { get; set; }
        public Configuration Configuration { get; set; } = new Configuration();

        public UserConfiguration()
        {

        }

        public UserConfiguration(CrmUserRef user, string name)
        {
            User = user;
            Name = name;
        }
    }
}