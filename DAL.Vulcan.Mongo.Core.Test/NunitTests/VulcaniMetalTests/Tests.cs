using System;
using System.Collections.Generic;
using System.Text;
using DAL.Vulcan.Mongo.Base.Core.Context;
using NUnit.Framework;
using Vulcan.IMetal.Queries.Companies;

namespace DAL.Vulcan.Mongo.Core.Test.NunitTests.VulcaniMetalTests
{
    [TestFixture]
    class Tests
    {

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void GetPaymentTerms()
        {
            var query = new QueryCompany("INC");
            var paymentTerms = query.GetPaymentTerms("INC");
        }
    }
}
