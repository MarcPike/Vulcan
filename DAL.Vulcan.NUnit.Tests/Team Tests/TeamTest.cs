using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.Helpers;
using NUnit.Framework;


namespace DAL.Vulcan.NUnit.Tests.Team_Tests
{
    [TestFixture]
    public class TeamTest //: ISendNotificationRefreshToSignalR
    {
        private readonly RepositoryBase<Location> _repLocation = new RepositoryBase<Location>();
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
        private IHelperReminder _helperReminder;
        private const string AppName = "vulcancrm";
        private const string TestTeamName = "Pike's Crew";
        //private Team _team;

        [SetUp]
        public void Setup()
        {
            _helperLocation = new HelperLocation();
            _helperApplication = new HelperApplication();
            _helperPerson = new HelperPerson();
            _helperContact = new HelperContact(_helperPerson, _helperUser);
            _helperUser = new HelperUser(_helperPerson);
            _helperTeam = new HelperTeam(_helperUser);
            _helperCompany = new HelperCompany();
            _helperNotifications = new HelperNotifications(_helperUser);
            _helperReminder = new HelperReminder();
            _helperAction = new HelperAction(_helperUser, _helperNotifications,_helperApplication, _helperReminder);
            _helperGoal = new HelperGoal(_helperUser,_helperNotifications, _helperAction);
            _helperUserViewConfig = new HelperUserViewConfig(_helperUser,_helperTeam);

            var rep = new RepositoryBase<Team>();
            foreach (var team in rep.AsQueryable().Where(x=>x.Name == TestTeamName).ToList())
            {
                _helperTeam.RemoveTeam(team);
            }

            //NotificationRouter.RegisterSignalR(this);
        }

        //[Test]
        //public void TestIsidroBug()
        //{
        //    Stopwatch s = new Stopwatch();
        //    s.Start();
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        var manager = _helperUser.GetManagerForUserId(AppName, "5936c22ab508d716f8da040a");
        //        foreach (var team in manager.Teams.Select(x => x.AsTeam()).ToList())
        //        {
        //            //Console.WriteLine(team.Name);
        //            foreach (var mgr in team.Managers.Select(x => x.AsManager()).ToList())
        //            {
        //                foreach (var activity in manager.Notifications)
        //                {
        //                //    Console.WriteLine(activity.GroupBy);
        //                }
        //            }
        //        }
        //        if ((i > 0) && ((i % 100) == 0))
        //        {
        //            Console.WriteLine($"{i} in {s.Elapsed} => {i / s.Elapsed.Seconds } per second");
        //        }
        //    }
        //    s.Stop();
        //    Console.WriteLine(s.Elapsed);
        //}

        //[Test]
        //public void LinksIssueWithCircularReferences()
        //{
        //    var manager = _helperUser.GetManagerForUserId(AppName, "5936c22ab508d716f8da040a");
        //    foreach (var team in manager.Teams.Select(x=>x.AsTeam()))
        //    {
        //        Console.WriteLine(team.Name);
        //        foreach (var mgr in team.Managers.Select(x=>x.AsManager()))
        //        {
        //            Console.WriteLine($"  Manager => {mgr.User.GetFullName()}");
        //        }
        //    }
        //}

        //[Test]
        //public void BuildTeam()
        //{
        //    TearDown();
        //    var manager = GetManager();
        //    var location = GetLocation();
            
        //    _team = new Team()
        //    {
        //        Name = TestTeamName,
        //        Location = location.AsLocationRef(),
        //        CreatedByUserId = manager.User.Id;
        //    };

        //    dAllSalesPersonsToTeam(_team);
        //}

        //[Test]
        //public void BuildTeamAndCreateGoal()
        //{
        //    TearDown();

        //    BuildTeam();
        //    var manager = GetManager();
        //    var newGoalModel = _helperGoal.CreateNewGoal(AppName, manager.User.Id);
        //    newGoalModel.Audience = "AllTeamMembers";
        //    newGoalModel.RevenueGoal = 100;
        //    newGoalModel.GroupBy = "Make $100";
        //    newGoalModel.Team = _team.AsTeamRef();
            
        //    _helperGoal.SaveGoal(newGoalModel);
        //}

        //[Test]
        //public void BuildTeamAndCreateGoalWithTask()
        //{
        //    TearDown();

        //    BuildTeam();
        //    var manager = GetManager();
        //    var newGoalModel = _helperGoal.CreateNewGoal(AppName, manager.User.Id);
        //    newGoalModel.Audience = "AllTeamMembers";
        //    newGoalModel.RevenueGoal = 100;
        //    newGoalModel.GroupBy = "Make $100";
        //    newGoalModel.Team = _team.AsTeamRef();

        //    _helperGoal.SaveGoal(newGoalModel);
        //    var user = _helperUser.GetUser(manager.User.Id);

        //    var taskModel = _helperAction.CreateNewTask(AppName,user);
        //    taskModel.GroupBy = "Make Calls";
        //    taskModel.DueDate = DateTime.Now.AddDays(7);
        //    taskModel = _helperAction.SaveTask(taskModel);

        //    var goal = _helperGoal.GetGoal(newGoalModel.Id);
        //    var task = _helperAction.GetTask(taskModel.Id);

        //    _helperGoal.AddAction(goal, task);

        //    Assert.IsFalse(taskModel.IsStarted);
        //    Assert.IsFalse(taskModel.IsCompleted);

        //    goal = _helperGoal.GetGoal(newGoalModel.Id);
        //    Assert.IsTrue(goal.PendingActions.Count == 1);

        //    taskModel.PercentComplete = 50;
        //    taskModel = _helperAction.SaveTask(taskModel);

        //    Assert.IsTrue(taskModel.IsStarted);
        //    Assert.IsFalse(taskModel.IsCompleted);

        //    taskModel.PercentComplete = 100;
        //    taskModel = _helperAction.SaveTask(taskModel);

        //    Assert.IsFalse(taskModel.IsStarted);
        //    Assert.IsTrue(taskModel.IsCompleted);

        //    goal = _helperGoal.GetGoal(newGoalModel.Id);
        //    Assert.IsTrue(goal.PendingActions.Count == 0);
        //    Assert.IsTrue(goal.CompletedActions.Count == 1);

        //    _helperGoal.RemoveGoal(goal);

        //}
        //private void AddAllSalesPersonsToTeam(Team team)
        //{
        //    var allSalesPersons = _repSalesPerson.AsQueryable().ToList();
        //    foreach (var salesPerson in allSalesPersons)
        //    {
        //        _helperTeam.AddSalesPersonToTeam(salesPerson, team);
        //    }
        //}

        //private Location GetLocation()
        //{
        //    var allLocations = _helperLocation.GetAllLocations();
        //    var location = allLocations.Single(x => x.Office == "Telge");
        //    location = _helperLocation.GetLocation(location.Id.ToString());
        //    return location;
        //}

        //private Manager GetManager()
        //{
        //    var user = _helperUser.LookupUserByNetworkId("mpike");
        //    Assert.IsTrue(_helperUser.IsInRole(AppName, user, "Manager"));
        //    var manager = _helperUser.GetManagerForUserId(AppName, user.Id.ToString());
        //    return manager;
        //}

        //[TearDown]
        //public void TearDown()
        //{
        //    if (_team != null)
        //    {
        //        _team = _helperTeam.GetTeam(_team.Id.ToString());
        //        if (_team != null)
        //            _helperTeam.RemoveTeam(_team);
        //    }
        //}

        //public void RefreshNotificationsForManager(string managerId, bool noLabel = false)
        //{
        //    Console.WriteLine($"Notification Refresh Message would be called for this Manager: {_helperUser.GetManager(AppName,managerId).User.GetFullName()}");
        //}

        //public void RefreshNotificationsForSalesPerson(string salespersonId, bool noLabel = false)
        //{
        //    Console.WriteLine($"Notification Refresh Message would be called for this SalesPerson: {_helperUser.GetSalesPerson(AppName, salespersonId).User.GetFullName()}");
        //}

        //public void RefreshNotificationsForDirector(string directorId, bool noLabel = false)
        //{
        //    Console.WriteLine($"Notification Refresh Message would be called for this Director: {_helperUser.GetDirector(AppName, directorId).User.GetFullName()}");
        //}
    }
}
