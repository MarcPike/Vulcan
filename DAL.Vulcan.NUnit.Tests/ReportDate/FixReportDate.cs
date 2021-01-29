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

namespace DAL.Vulcan.NUnit.Tests.ReportDate
{
    [TestFixture]
    public class FixReportDate
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = DAL.Vulcan.Mongo.Base.Context.Environment.QualityControl;
        }

        [Test]
        public void FixNullReportDates()
        {
            var rep = new RepositoryBase<CrmQuote>();
            var quotesToFix = rep.AsQueryable().Where(x => x.ReportDate == null).ToList();
            foreach (var crmQuote in quotesToFix)
            {
                crmQuote.SetReportDate();
                rep.Upsert(crmQuote);
            }
        }
    }
}
