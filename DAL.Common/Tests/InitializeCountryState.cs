using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.Common.Helper;
using NUnit.Framework;

namespace DAL.Common.Tests
{
    [TestFixture]
    public class InitializeCountryState
    {

        [Test]
        public void Execute()
        {
            CountryState.Initialize();
        }

        [Test]
        public void TestHelperGetCountryStateList()
        {
            var helperCommon = new HelperCommon();
            var results = helperCommon.GetCountryStateList();
            //foreach (var countryStateModel in results)
            //{
            //    Console.WriteLine($"Id: {countryStateModel.Id} Country: {countryStateModel.Country} State: {countryStateModel.StateProvince}");
            //}
        }

    }
}
