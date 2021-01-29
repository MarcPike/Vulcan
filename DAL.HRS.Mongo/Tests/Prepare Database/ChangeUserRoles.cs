using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Prepare_Database
{
    [TestFixture()]
    public class ChangeUserRoles
    {
        private Helpers.HelperUser _helperUser;
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
            _helperUser = new Helpers.HelperUser();
        }

        [Test]
        public void Execute()
        {
            //foreach (var hrsUser in new RepositoryBase<HrsUser>().AsQueryable().Where(x=>x.SecurityRole.Name == "SystemAdmin").ToList())
            //{
            //    _helperUser.ChangeUserRole(hrsUser.UserId, RoleType.SystemAdmin);
            //}
        }
    }
}
