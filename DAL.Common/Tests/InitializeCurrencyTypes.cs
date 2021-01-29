using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using NUnit.Framework;

namespace DAL.Common.Tests
{
    [TestFixture]
    public class InitializeCurrencyTypes
    {
        [Test]
        public void Execute()
        {
            var currencyTypes = CurrencyType.GetDefaults();
            foreach (var currencyType in currencyTypes)
            {
                Console.WriteLine($"{currencyType.Code} - {currencyType.Description}");
            }
        }
    }
}
