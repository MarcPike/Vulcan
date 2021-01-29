using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.ActionTests
{
    [TestFixture]
    public class CleanupActions
    {
        [Test]
        public void Execute()
        {
            var crmUsers = new RepositoryBase<Mongo.DocClass.CRM.CrmUser>().AsQueryable().ToList();
            foreach (var crmUser in crmUsers)
            {
                var actionRefs = crmUser.Actions.ToList();
                foreach (var actionRef in actionRefs)
                {
                    try
                    {
                        var action = actionRef.AsAction();
                        if (action == null) throw new Exception($"{actionRef.Label} no longer exists for {crmUser.User.GetFullName()}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        crmUser.Actions.Remove(actionRef);
                        crmUser.SaveToDatabase();
                    }
                }
            }
        }
    }
}
