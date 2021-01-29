using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using DAL.Vulcan.Mongo.DocClass.QueueSchedule;
using DAL.Vulcan.Mongo.Helpers;
using DocumentFormat.OpenXml.InkML;
using NUnit.Framework;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;
using Trace = System.Diagnostics.Trace;

namespace DAL.Vulcan.NUnit.Tests.QueueSchedule
{
    [TestFixture]
    public class QueueTesting
    {
        private const string TestQueueName = "ScheduledEvents";
        private Mongo.DocClass.QueueSchedule.QueueSchedule _schedule;

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
            var rep = new RepositoryBase<Mongo.DocClass.QueueSchedule.QueueSchedule>();
            _schedule = rep.AsQueryable().SingleOrDefault(x => x.Name == TestQueueName);
            if (_schedule == null)
            {
                _schedule = new Mongo.DocClass.QueueSchedule.QueueSchedule() { Name = TestQueueName };
            }
        }

        [Test]
        public void CountByStatus()
        {
            var active = _schedule.Events.Count(x => x.Status == ScheduledEventWorkStatus.Active);
            var pending = _schedule.Events.Count(x => x.Status == ScheduledEventWorkStatus.Pending);
            var failed = _schedule.Events.Count(x => x.Status == ScheduledEventWorkStatus.Failed);
            var completed = _schedule.Events.Count(x => x.Status == ScheduledEventWorkStatus.Completed);
            var cancelled = _schedule.Events.Count(x => x.Status == ScheduledEventWorkStatus.Cancelled);

            Console.WriteLine($"Active: {active} Pending: {pending} Failed: {failed} Completed: {completed} Cancelled: {cancelled}");
        }

        /*
        private readonly RepositoryBase<Location> _repLocation = new RepositoryBase<Location>();
        private readonly RepositoryBase<Team> _repTeam = new RepositoryBase<Team>();

        private IHelperUser _helperUser;
        private IHelperLocation _helperLocation;
        private IHelperTeam _helperTeam;
        private IHelperCompany _helperCompany;
        private IHelperGoal _helperGoal;
        private IHelperAction _helperAction;
        private IHelperUserViewConfig _helperUserViewConfig;
        private IHelperApplication _helperApplication;
        private IHelperNotifications _helperNotifications;
        private IHelperPerson _helperPerson;
        private IHelperContact _helperContact;
        private IHelperDomain _helperDomain;
        private const string AppName = "vulcancrm";

        [SetUp]
        public void Initialize()
        {
            var rep = new RepositoryBase<Mongo.DocClass.QueueSchedule.QueueSchedule>();
            _schedule = rep.AsQueryable().SingleOrDefault(x => x.Name == TestQueueName);
            if (_schedule == null)
            {
                _schedule = new Mongo.DocClass.QueueSchedule.QueueSchedule() { Name = TestQueueName };
            }
            _schedule.Events.Clear();
            _schedule.SaveAccount();

            _helperLocation = new HelperLocation();
            _helperApplication = new HelperApplication();
            _helperDomain = new HelperDomain();
            _helperPerson = new HelperPerson();
            _helperContact = new HelperContact(_helperPerson, _helperUser);
            _helperTeam = new HelperTeam(_helperUser);
            _helperCompany = new HelperCompany();
            _helperNotifications = new HelperNotifications(_helperUser);
            _helperAction = new HelperAction(_helperUser, _helperNotifications, _helperApplication);
            _helperGoal = new HelperGoal(_helperUser, _helperNotifications, _helperAction);
            _helperUserViewConfig = new HelperUserViewConfig(_helperUser, _helperTeam);


        }
        [Test]
        public void RunReminderTest()
        {
            var teams = _repTeam.AsQueryable().ToList();
            var activity = new Notification()
            {
                GroupBy = "Reminder for [Quarterly Finance Meeting]",
                PrimaryObjectType = NotificationObjectType.ScheduledEvent,
                SecondaryObjectType = NotificationObjectType.Meeting
            };
            activity.SaveToDatabase();

            var newScheduledEvent = new ScheduledEvent()
            {
                GroupBy = "Quarterly Finance Meeting",
                EventType = ScheduledEventType.Meeting,
                Notification = new NotificationRef(activity),
                ExecuteOn = DateTime.Now.AddMinutes(5),
                OccuranceLimit = 3,
                ReoccureSpan = new TimeSpan(0,0,5,0),
                QueueId = _schedule.Id
            };

            foreach (var team in teams.ToList())
            {
                foreach (var salesPerson in team.SalesPersons)
                {

                    newScheduledEvent.SalesPersons.Add(salesPerson);
                }

                foreach (var manager in team.Managers)
                {
                    newScheduledEvent.Managers.Add(manager);
                }
            }

            newScheduledEvent.SaveToDatabase();
            _schedule.Events.Add(newScheduledEvent);
            _schedule.SaveAccount();
        }
        */
    }
}
