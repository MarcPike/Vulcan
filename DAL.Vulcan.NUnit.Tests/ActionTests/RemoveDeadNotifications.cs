using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Action = DAL.Vulcan.Mongo.DocClass.CRM.Action;

namespace DAL.Vulcan.NUnit.Tests.ActionTests
{
    [TestFixture]
    public class RemoveDeadNotifications
    {
        [Test]
        public void Execute()
        {
            var rep = new RepositoryBase<Notification>();
            var notificationsForActions = rep.AsQueryable()
                .Where(x => x.Action != null).ToList();
            foreach (var notification in notificationsForActions.Where(x=>x.Action.AsAction() == null))
            {
                rep.RemoveOne(notification);
            }
        }
    }
}
