using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.DocClass.Config
{
    public sealed class UserConfigurationManager
    {
        private static UserConfigurationManager _instance = null;
        private static readonly HelperUser HelperUser = new HelperUser();

        private static List<UserConfiguration> UserConfigurations { get; set; } = new List<UserConfiguration>();

        private UserConfigurationManager()
        {
        }

        public static UserConfigurationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserConfigurationManager();
                }
                return _instance;
            }
        }

        public UserConfiguration GetUserConfiguration(string userId, string configName, string defaultValue)
        {
            var userConfig = (UserConfigurations.FirstOrDefault(x => x.User.Id == userId && x.Name == configName));

            if (userConfig != null) return userConfig;

            var crmUser = HelperUser.GetCrmUser("vulcancrm", userId);

            userConfig = new RepositoryBase<UserConfiguration>().AsQueryable()
                .SingleOrDefault(x => x.User.Id == userId && x.Name == configName);

            if (userConfig == null)
            {
                userConfig = new UserConfiguration(crmUser.AsCrmUserRef(), configName)
                {
                    Configuration = new Configuration()
                    {
                        JsonData = defaultValue
                    }
                };
            }

            UserConfigurations.Add(userConfig);

            return userConfig;
        }

        public void SaveUserConfiguration(UserConfiguration userConfiguration)
        {
            userConfiguration.SaveToDatabase();
            var oldUserConfiguration = UserConfigurations.FirstOrDefault(x => x.Id == userConfiguration.Id);

            if (oldUserConfiguration != null) UserConfigurations.Remove(oldUserConfiguration);
            UserConfigurations.Add(userConfiguration);
        }
    }
}
