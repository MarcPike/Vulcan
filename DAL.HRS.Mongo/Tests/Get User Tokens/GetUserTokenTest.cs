using System;
using DAL.HRS.Mongo.Helpers;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.Get_User_Tokens
{
    [TestFixture]
    public class GetUserTokenTest
    {
        private IHelperSecurity _helperSecurity; 
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
            _helperSecurity = new HelperSecurity();
        }

        [Test]
        public void AuthenticateTest()
        {
            var networkId = "mpike";
            var password = "Reed1014";

            var networkIdEncoded = _helperSecurity.EncodeToBase64(networkId);
            var passwordEncoded = _helperSecurity.EncodeToBase64(password);

            var token = _helperSecurity.Authenticate(networkIdEncoded, passwordEncoded);
            Console.WriteLine(ObjectDumper.Dump(token));
        }

        [Test]
        public void Impersonate()
        {
            var networkId = "mpike";
            var password = "Reed1014";

            var networkIdEncoded = _helperSecurity.EncodeToBase64(networkId);
            var passwordEncoded = _helperSecurity.EncodeToBase64(password);

            var token = _helperSecurity.Authenticate(networkIdEncoded, passwordEncoded);

            var adminUserIdEncoded = _helperSecurity.EncodeToBase64(token.User.Id);
            networkIdEncoded = _helperSecurity.EncodeToBase64("sreese");
            token = _helperSecurity.Impersonate(adminUserIdEncoded, networkIdEncoded);
            Console.WriteLine(ObjectDumper.Dump(token));
        }
    }
}
