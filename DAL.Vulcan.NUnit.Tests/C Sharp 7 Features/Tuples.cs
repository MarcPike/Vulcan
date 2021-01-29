using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using static System.ValueTuple;

namespace DAL.Vulcan.NUnit.Tests.C_Sharp_7_Features
{
    [TestFixture]
    class Tuples
    {
        
        public static (string Office, string Mobile, string Fax) GetPhoneNumbers()
        {
            return ("(281) 649-8954", "(713) 292-4278", "");
        }

        public static (int Min, int Max, int Sum, int Avg, int Count) GetNumericStats(IEnumerable<int> values)
        {
            var min = int.MaxValue;
            var max = int.MinValue;
            var count = 0;
            var sum = 0;
            foreach (var value in values)
            {
                min = (value < min) ? value : min;
                max = (value > max) ? value : max;
                sum += value;
                count++;
            }
            if (count == 0) return (0, 0, 0, 0, 0);

            return (min, max, sum, sum / count, count);
        }

        [Test]
        public void TestTuple()
        {
            var phoneNumbers = GetPhoneNumbers();
            Console.WriteLine($"Office: {phoneNumbers.Office} Mobile: {phoneNumbers.Office}");
        }

        [Test]
        public void TestTupleFunctionReturn()
        {
            IEnumerable<int> values = new List<int>() {1,2,3,4,5,6,7,8,9,10};
            var stats = GetNumericStats(values);
            Console.WriteLine($"Min: {stats.Min} Max: {stats.Max} Sum: {stats.Sum} Count: {stats.Count} Avg: {stats.Avg}");
        }


    }
}
