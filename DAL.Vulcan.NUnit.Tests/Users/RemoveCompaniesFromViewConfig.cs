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
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.Users
{
    [TestFixture]
    public class RemoveCompaniesFromViewConfig
    {
        [Test]
        public void ReSaveAllCrmUsers()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<Mongo.DocClass.CRM.CrmUser>();
            var users = rep.AsQueryable().ToList();
            foreach (var crmUser in users)
            {
                crmUser.ViewConfig.SelectedAlliances = null;
                crmUser.ViewConfig.SelectedNonAlliances = null;
                rep.Upsert(crmUser);
            }
        }
    }
}
