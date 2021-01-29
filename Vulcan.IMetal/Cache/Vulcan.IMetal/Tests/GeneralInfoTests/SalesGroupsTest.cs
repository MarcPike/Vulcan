using System;
using NUnit.Framework;
using Vulcan.IMetal.Queries.GeneralInfo;

namespace Vulcan.IMetal.Tests.GeneralInfoTests
{
    [TestFixture()]
    public class SalesGroupsTest
    {
        [Test]
        public void TestSalesGroupQuery()
        {
            var salesGroups = SalesGroupQuery.GetForCoid("INC");
            foreach (var salesGroup in salesGroups)
            {
                Console.WriteLine(salesGroup.Code);
            }

        }
    }
}
