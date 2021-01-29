using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.SalesGroups
{
    [TestFixture()]
    public class SalesGroupUpdates
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void DisableHOH()
        {
            var rep = new RepositoryBase<SalesGroup>();
            var hoh = rep.AsQueryable().SingleOrDefault(x => x.Code == "HOH" && x.Coid == "INC");
            if (hoh != null) hoh.IgnoreInVulcan = true;
            rep.Upsert(hoh);
        }
    }
}
