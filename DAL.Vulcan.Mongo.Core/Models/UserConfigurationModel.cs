using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Config;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class UserConfigurationModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public CrmUserRef User { get; set; }
        public Configuration Configuration { get; set; }

        public UserConfigurationModel()
        {

        }

        public UserConfigurationModel(string application, string userId, UserConfiguration config)
        {
            Id = config.Id.ToString();
            User = config.User;
            Configuration = config.Configuration;
            Name = config.Name;
            Application = application;
            UserId = userId;
        }

        public static UserConfigurationModel GetForUser(string application, string userId, CrmUserRef crmUserRef, string name)
        {
            var rep = new RepositoryBase<UserConfiguration>();
            var result = rep.AsQueryable().SingleOrDefault(x => x.User.Id == crmUserRef.Id && x.Name == name);
            if (result == null)
            {
                result = new UserConfiguration(crmUserRef, name);
                rep.Upsert(result);
            }

            return new UserConfigurationModel(application, userId, result);
        }

    }
}
