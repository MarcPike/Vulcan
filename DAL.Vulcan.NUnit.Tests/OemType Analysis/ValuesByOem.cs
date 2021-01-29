using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Linq;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.OemType_Analysis
{
    [TestFixture()]
    public class ValuesByOem
    {
        [Test]
        public void CountByOemType()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<CrmQuoteItem>();
            var groups = rep.AsQueryable()
                .GroupBy(n => n.OemType)
                .Select(n => new
                    {
                        MetricName = n.Key,
                        MetricCount = n.Count()
                    }
                )
                .OrderBy(n => n.MetricName);
            foreach (var @group in groups)
            {
                Console.WriteLine(ObjectDumper.Dump($"{@group.MetricName ?? "(null)"} - {@group.MetricCount}"));
            }

        }
    }
}
