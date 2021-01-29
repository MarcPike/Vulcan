using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.Helper;
using NUnit.Framework;

namespace DAL.Common.Tests
{
    [TestFixture]
    public class TestGetCountryList
    {
        [Test]
        public void GetCountryList()
        {
            var helper = new HelperCommon();
            var countries = helper.GetCountryList();
            foreach (var country in countries.Countries)
            {
                Console.WriteLine($"Country: {country.CountryName}");
                foreach (var state in country.States)
                {
                    Console.WriteLine($"     State:{state.StateName}");
                }
            }
        }
    }
}
