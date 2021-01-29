using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Queries.GeneralInfo;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Model_Changes
{
    [TestFixture()]
    public class LoadSalesGroups
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
        }

        [Test]
        public void Execute()
        {
            var coidList = new List<string>() {"INC", "CAN", "MSA", "SIN", "EUR", "DUB"};
            var rep = new RepositoryBase<DAL.Vulcan.Mongo.DocClass.CRM.SalesGroup>();

            //rep.RemoveAllFromCollection();

            foreach (var coid in coidList)
            {
                var salesGroups = SalesGroupQuery.GetForCoid(coid);

                foreach (var salesGroup in salesGroups)
                {
                    if (!rep.AsQueryable().Any(x => x.Coid == coid && x.Code == salesGroup.Code))
                    {
                        var newSalesGroup = new DAL.Vulcan.Mongo.DocClass.CRM.SalesGroup()
                        {
                            Coid = coid,
                            Code = salesGroup.Code,
                            Description = salesGroup.Description
                        };
                        rep.Upsert(newSalesGroup);
                    }


                }
            }

        }
    }
}
