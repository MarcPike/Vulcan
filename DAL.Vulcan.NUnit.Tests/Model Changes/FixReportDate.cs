using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Model_Changes
{
    [TestFixture]
    public class FixReportDate
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
        }

        [Test]
        public void FixNullReportDates()
        {
            var rep = new RepositoryBase<CrmQuote>();
            var quotesWithNullReportDates = rep.AsQueryable().Where(x => x.ReportDate == null).ToList();
            foreach (var crmQuote in quotesWithNullReportDates)
            {
                crmQuote.SetReportDate();
                rep.Upsert(crmQuote);
            }
        }
    }
}
