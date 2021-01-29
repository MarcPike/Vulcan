using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Queries;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.Quote_Tests
{
    [TestFixture()]
    public class QuotePipelineTest
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            var sw = new Stopwatch();
            sw.Start();

            var pipeline = new 
                QuotesPipelineQuery(
                    "5a660b21b508d745a088a2bf", 
                    DateTime.Parse("8/14/2019"),
                    DateTime.Parse("9/14/2019").AddMinutes(-1), 
                    true, true);

            Console.WriteLine($"Drafts: {pipeline.Drafts.Count}");
            Console.WriteLine($"Pending: {pipeline.Pending.Count}");
            Console.WriteLine($"Won: {pipeline.Won.Count}");
            Console.WriteLine($"Lost: {pipeline.Lost.Count}");
            Console.WriteLine($"Expired: {pipeline.Expired.Count}");


            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.Elapsed}");
        }

    }
}
