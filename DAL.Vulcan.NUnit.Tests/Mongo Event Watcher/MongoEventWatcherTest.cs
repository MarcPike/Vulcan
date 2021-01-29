using System.Diagnostics;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.EventWatcher;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using DAL.Vulcan.Test;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Mongo_Event_Watcher
{
    [TestFixture]
    public class MongoEventWatcherTest
    {
        [Test]
        public void TestActivityWatcher()
        {
            MongoEventWatcher.AddWatcher(typeof(Notification), WatcherDataOperation.Create, OnActivityUpdated);
            MongoEventWatcher.AddWatcher(typeof(Notification), WatcherDataOperation.Delete, OnActivityDeleted);

            var activity = new Notification()
            {
                ActionType = NotificationActionType.Create,
                PrimaryObjectType = NotificationObjectType.Other,
                Label = "This is a test activity that will evaluate if the MongoEventWatcher works"
            };

            var rep = new RepositoryBase<Notification>();

            rep.Upsert(activity);


            rep.RemoveOne(activity);

        }

        private void OnActivityDeleted(BaseDocument activity)
        {
            var thisActivity = activity as Notification;
            if (thisActivity == null)
            {
                Trace.WriteLine($"Wrong type of object {activity.GetType().FullName}");

            }
            Trace.WriteLine($"New Notification was deleted {ObjectDumper.Dump(thisActivity)}");
        }

        private void OnActivityUpdated(BaseDocument activity)
        {
            var thisActivity = activity as Notification;
            if (thisActivity == null)
            {
                Trace.WriteLine($"Wrong type of object {activity.GetType().FullName}");

            }
            Trace.WriteLine($"New Notification was created {ObjectDumper.Dump(thisActivity)}");
        }
    }
}
