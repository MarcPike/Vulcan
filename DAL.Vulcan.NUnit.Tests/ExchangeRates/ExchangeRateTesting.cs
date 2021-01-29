using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Vulcan.IMetal.Models;

namespace DAL.Vulcan.NUnit.Tests.ExchangeRates
{
    [TestFixture]
    public class ExchangeRateTesting
    {
        [Test]
        public void GetList()
        {
            var exchangeRates = ExchangeRate.GetRateList();
            foreach (var exchangeRate in exchangeRates)
            {
                Console.WriteLine(ObjectDumper.Dump(exchangeRate));
            }
        }
    }
}
