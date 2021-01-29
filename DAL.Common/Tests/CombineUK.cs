using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Common.Tests
{
    [TestFixture]
    public class CombineUK
    {
        [Test]
        public void Execute()
        {
            var uk = CountryValue.Helper.Find(x => x.CountryName == "United Kingdom").First();
            var england = CountryValue.Helper.Find(x => x.CountryName == "England").First();
            var scotland = CountryValue.Helper.Find(x => x.CountryName == "Scotland").First();

            uk.States.AddRange(england.States);
            uk.States.AddRange(scotland.States);
            var southState = uk.States.FirstOrDefault(x => x.StateName == "South ");
            uk.States.Remove(southState);
            CountryValue.Helper.Upsert(uk);


        }
    }
}
