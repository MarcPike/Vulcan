using System;
using System.Linq;
using NUnit.Framework;
using Vulcan.IMetal.Queries.GeneralInfo;

namespace Vulcan.IMetal.Tests.GeneralInfoTests
{
    [TestFixture()]
    public class CountryCodeTest
    {
        [Test]
        public void GetCountryCodes()
        {
            var countryCodes = CountryCodeModel.GetForCoid("INC");
            var us = countryCodes.SingleOrDefault(x => x.Country == "United States");
            Console.WriteLine(ObjectDumper.Dump(us));

        }
    }
}
