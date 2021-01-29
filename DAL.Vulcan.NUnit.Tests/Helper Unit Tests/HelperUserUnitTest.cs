using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.Helper_Unit_Tests
{
    [TestFixture]
    public class HelperUserUnitTest
    {
        private const string _application = "vulcancrm";
        private IHelperUser _helperUser = new HelperUser(new HelperPerson());

        private string _userId { get; set; }

        [Test]
        public void BasicTest()
        {
            // Test LookupUserByNetworkId
            var ldapUser = _helperUser.LookupUserByNetworkId("mpike");
            Assert.IsNotNull(ldapUser);

            _userId = ldapUser.Id.ToString();

            // Test GetUser
            ldapUser = _helperUser.GetUser(_userId);
            Assert.AreEqual(_userId,ldapUser.AsUserRef().Id);

            var crmUser = _helperUser.GetCrmUser(_application, _userId);
            Assert.IsNotNull(crmUser);

            var crmUserModel = _helperUser.GetCrmUserModel(_application, _userId);
            Assert.IsNotNull(crmUserModel);

            if (crmUserModel.PersonalInfo.Addresses.Count == 0)
            {
                var address = new AddressModel()
                {
                    AddressLine1 = "10626 Archmont Drive",
                    City = "Houston",
                    StateProvince = "Texas",
                    PostalCode = "77070",
                    Country = "USA",
                    Type = AddressType.Home.ToString()
                };
                crmUserModel.PersonalInfo.Addresses.Add(address);

                var crmUserModelAfterUpdate = _helperUser.SaveCrmUserModel(crmUserModel);
                Assert.IsTrue(crmUserModel.PersonalInfo.Addresses.Count == crmUserModelAfterUpdate.PersonalInfo.Addresses.Count);



            }

            var userPersonModel = _helperUser.GetUserPersonModel(_userId);
            var mobilePhone =
                userPersonModel.PhoneNumbers.FirstOrDefault(x => x.Type == PhoneType.Mobile.ToString());
            if (mobilePhone == null)
            {
                mobilePhone = new PhoneNumberModel()
                {
                    Number = "(713) 292-4278",
                    Type = PhoneType.Mobile.ToString()
                };
                Assert.IsNotNull(mobilePhone.Id);
                userPersonModel.PhoneNumbers.Add(mobilePhone);
            }
            var userPersonModelAfterUpdate = _helperUser.SaveUserPersonModel(userPersonModel);

            Assert.IsTrue(userPersonModelAfterUpdate.PhoneNumbers.Any(x => x.Id == mobilePhone.Id));
        }


    }
}
