using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Extensions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.Number_Format_Tests
{
    [TestFixture]
    public class NumericFormatTesting
    {
        [Test]
        public void DecimalToStringUsingG29()
        {
            Decimal testValue = 10.00000000M;
            Console.WriteLine(testValue.ToString("G29"));
        }

        [Test]
        public void DecimalToStringUsingNormalize()
        {
            Decimal testValue = 10.00000000M;
            Console.WriteLine(testValue.Normalize());

        }

        private string Normalize(decimal value)
        {
            return (value / 1.000000000000000000000000000000000m).ToString(CultureInfo.InvariantCulture);
        }

        [Test]
        public void RoundAndNormalizeFourPlaces()
        {
            var value = (decimal)2851.9058;

            Console.WriteLine(value.RoundAndNormalize(2));
            Console.WriteLine(DateTime.Now.Date);
        }

    }
}
