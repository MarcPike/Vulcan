using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Config;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.Models
{
    public class UserConfigurationModel: BaseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public HrsUserRef User { get; set; }
        public Configuration Configuration { get; set; }

        public UserConfigurationModel()
        {

        }

        public UserConfigurationModel(UserConfiguration config)
        {


            Id = config.Id.ToString();
            User = config.User;
            Configuration = config.Configuration;
            Name = config.Name;
        }

        public static UserConfigurationModel GetForUser(HrsUserRef hrsUserRef, string name)
        {
            var rep = new RepositoryBase<UserConfiguration>();
            var userConfig = rep.AsQueryable().SingleOrDefault(x => x.User.Id == hrsUserRef.Id && x.Name == name);
            if (userConfig == null)
            {
                userConfig = new UserConfiguration(hrsUserRef, name);
                rep.Upsert(userConfig);
            }

            return new UserConfigurationModel(userConfig);
        }

    }
}
