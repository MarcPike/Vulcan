using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.DocClass.Config
{
    public class UserConfiguration : BaseDocument
    {
        public HrsUserRef User { get; set; }
        public string Name { get; set; }
        public Configuration Configuration { get; set; } = new Configuration();

        public static MongoRawQueryHelper<UserConfiguration> Helper = new MongoRawQueryHelper<UserConfiguration>();

        public UserConfiguration()
        {

        }

        public UserConfiguration(HrsUserRef user, string name)
        {
            User = user;
            Name = name;
        }

    }
}