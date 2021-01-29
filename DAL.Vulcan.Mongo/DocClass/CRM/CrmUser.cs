using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Email;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Messages;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class CrmUser: BaseDocument
    {
        public static MongoRawQueryHelper<CrmUser> Helper = new MongoRawQueryHelper<CrmUser>();
        public string Application { get; set; } = "vulcancrm";
        public CrmUserType UserType { get; set; } 
        public UserRef User { get; set; }
        public bool IsAdmin { get; set; } = false;
        public ReferenceList<Strategy, StrategyRef> Strategies { get; set; } = new ReferenceList<Strategy, StrategyRef>();
        public ReferenceList<Goal,GoalRef> Goals { get; set; } = new ReferenceList<Goal, GoalRef>();
        public GuidList<Note> Notes { get; set; } = new GuidList<Note>();

        public ReferenceList<Notification, NotificationRef> Notifications { get; set; } = new ReferenceList<Notification, NotificationRef>();

        public ViewConfig ViewConfig { get; set; } = new ViewConfig();

        public ReferenceList<Action, ActionRef> Actions { get; set; } = new ReferenceList<Action, ActionRef>();
        public ReferenceList<Team, TeamRef> Teams { get; set; } = new ReferenceList<Team, TeamRef>();
        public ReferenceList<Contact, ContactRef> Contacts { get; set; } = new ReferenceList<Contact, ContactRef>();

        public List<MessageObject> Messages { get; set; } = new List<MessageObject>();
        public ReferenceList<Email.Email, EmailRef> Emails { get; set; } = new ReferenceList<Email.Email, EmailRef>();

        public bool ReadOnly { get; set; } = false;

        public bool UseMyLocationForPdf { get; set; } = true;

        public string UserId => User.Id;

        public bool IsCalcAdmin { get; set; } = false;

        public string Coid
        {
            get
            {
                var team = ViewConfig.Team?.AsTeam();
                if (team == null) return string.Empty;
                return team.Coid;
            }
        }

        public CrmUser(LdapUser user, CrmUserType userType)
        {
            if (user.Person == null)
            {
                user.Person = new Person(user);
            }
            user.Save();
            User = user.AsUserRef();
            UserType = userType;
            //ViewConfig.GetSelectedCompanies(User);
        }

        public CrmUser()
        {
        }

        public void AddToTeam(TeamRef teamRef)
        {
            Teams.AddReferenceObject(teamRef);
            ViewConfig.ViewType = ViewType.Team;
            ViewConfig.Team = teamRef;
            //ViewConfig.GetSelectedCompanies(User);
            SaveToDatabase();
        }

        public void RemoveFromTeam(TeamRef teamRef)
        {
            var removeTeam = Teams.SingleOrDefault(x => x.Id == teamRef.Id);
            Teams.RemoveDocumentRef(removeTeam);

            var lastTeam = Teams.LastOrDefault();
            if (lastTeam == null)
            {
                ViewConfig.Team = null;
                ViewConfig.SetViewMyStuff();
            }
            else
            {
                ViewConfig.SetViewTeam(teamRef);
            }
            SaveToDatabase();

        }

        public void SecurityCheckCanCreateTeam()
        {
            if ((UserType != CrmUserType.Director) && (UserType != CrmUserType.Manager))
            {
                throw new Exception("You must be either a Director or a Manager to Create a team");
            }
        }

        //public static CrmUser Find(string application, string userId)
        //{
        //    var tokenUser = new RepositoryBase<CrmUserToken>().Find(userId);
        //    if (tokenUser != null) return tokenUser.CrmUserRef.AsCrmUser();

        //    return new RepositoryBase<CrmUser>().AsQueryable()
        //        .SingleOrDefault(x => x.Application == application && x.User.Id == userId);
        //}

        //public static CrmUser Find(string application, ObjectId userId)
        //{
        //    var tokenUser = new RepositoryBase<CrmUserToken>().Find(userId);
        //    if (tokenUser != null) return tokenUser.CrmUserRef.AsCrmUser();

        //    return new RepositoryBase<CrmUser>().AsQueryable()
        //        .SingleOrDefault(x => x.Application == application && x.User.Id == userId.ToString());
        //}

        public CrmUserRef AsCrmUserRef()
        {
            return new CrmUserRef(this);
        }

        public override string ToString()
        {
            return User.GetFullName();
        }
    }

    

}