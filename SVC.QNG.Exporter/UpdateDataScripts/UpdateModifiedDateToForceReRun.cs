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
using SVC.QNG.Exporter.Models;

namespace SVC.QNG.Exporter.UpdateDataScripts
{
    [TestFixture()]
    public class UpdateModifiedDateToForceReRun
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            var modifiedDateTime = DateTime.Now;
            var rep = new RepositoryBase<CrmQuote>();
            var quotesToUpdate = rep.AsQueryable().Where(x => x.ReportDate >= DateTime.Parse("10/30/2019"))
                .ToList();
            foreach (var crmQuote in quotesToUpdate)
            {

                crmQuote.ModifiedDateTime = modifiedDateTime;
                rep.Upsert(crmQuote);
            }
        }

    }
}
