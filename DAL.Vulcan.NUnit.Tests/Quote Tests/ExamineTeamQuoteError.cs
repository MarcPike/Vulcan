using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Quote_Tests
{
    [TestFixture]
    class ExamineTeamQuoteError
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var quote = CrmQuote.Helper.Find(x => x.QuoteId == 131476).FirstOrDefault();
            for (int i = 0; i < 100; i++)
            {
                var team = quote.Team.AsTeam();
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }
    }
}
