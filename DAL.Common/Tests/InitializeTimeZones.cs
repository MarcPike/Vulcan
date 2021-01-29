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
    public class InitializeLocationTimeZones 
    {

        [Test]
        public void Execute()
        {
            var timeZones = LocationTimeZone.Initialize();
            foreach (var timeZone in timeZones)
            {
                Console.WriteLine(timeZone.Name);
            }

        }
    }
}
