using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Helpers;
using NUnit.Framework.Internal;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.PersonTests
{
    [TestFixture]
    public class GeneralRoleTests
    {
        private const string Application = "vulcancrm";
        private IHelperPerson _helperPerson;
        private IHelperApplication _helperApplication;
        private IHelperUser _helperUser;

        [SetUp]
        public void SetUp()
        {
            _helperPerson = new HelperPerson();
            _helperApplication = new HelperApplication();
            _helperUser = new HelperUser(_helperPerson);
        }

        [Test]
        public void GetAllDirectors()
        {
            var directors = _helperUser.GetAllDirectors(Application);
        }

        [Test]
        public void UserLocationTest()
        {
            var mpike = _helperUser.LookupUserByNetworkId("mpike");
        }

        [Test]
        public void GetAllManagers()
        {
            var managers = _helperUser.GetAllManagers(Application);
        }

        [Test]
        public void GetAllSalesPersons()
        {
            var salesPersons = _helperUser.GetAllSalesPersons(Application);
        }

        [Test]
        public void GetIsidroNetworkId()
        {
            var isidro = _helperUser.LookupUserByNetworkId("igallego");
            Assert.IsNotNull(isidro);
        }

        [Test]
        public void GetUserToken()
        {
            var isidro = _helperUser.LookupUserByNetworkId("igallego");
            var userToken = _helperUser.GetUserToken(Application, isidro.Id.ToString());
            Assert.IsNotNull(userToken);
        }

        //[Test]
        //public void GetManagerModel()
        //{
        //    var isidro = _helperUser.LookupUserByNetworkId("igallego");
        //    var userToken = _helperUser.GetUserToken(Application, isidro);

        //    var managerModel = _helperUser.GetCrmUser(Application, userToken.Manager.Id);
        //    Assert.IsNotNull(managerModel);

        //}

        //[Test]
        //public void SaveManagerModel()
        //{
        //    var pike = _helperUser.LookupUserByNetworkId("mpike");
        //    var userToken = _helperUser.GetUserToken(Application, pike);

        //    var managerModel = _helperUser.GetManagerModel(Application, userToken.Manager.Id);
        //    managerModel.PersonalData.LastName = "Pikey";

        //    managerModel = _helperUser.SaveManager(managerModel);
        //    Assert.IsTrue(managerModel.PersonalData.LastName == "Pikey");

        //    managerModel.PersonalData.LastName = "Pike";
        //    managerModel = _helperUser.SaveManager(managerModel);
        //    Assert.IsTrue(managerModel.PersonalData.LastName == "Pike");

        //}
    }
}
