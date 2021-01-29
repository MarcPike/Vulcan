using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.HrsUserList
{
    [TestFixture]
    class GetHrsUserList
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void Execute()
        {
            foreach (var hrsUser in HrsUser.Helper.GetAll().OrderBy(x=>x.LastName).ThenBy(x=>x.FirstName).ToList())
            {
                var emailAddress = hrsUser.Employee.AsEmployee().EmailAddresses.FirstOrDefault()?.EmailAddress ?? "<Unknown Email Address>";
                Console.WriteLine($"{hrsUser.LastName},{hrsUser.FirstName},{hrsUser.Employee.PreferredName??String.Empty},{emailAddress}");                
            }
        }
    }
}
