using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using DAL.Vulcan.Mongo.Core.DocClass.Messages;
using DAL.Vulcan.Mongo.Core.DocClass.Notifications;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public interface ICrmUser
    {
        CrmUserType UserType { get; }
        ReferenceList<Notification,NotificationRef> Notifications { get; set; }
        ReferenceList<Contact, ContactRef> Contacts { get; }
        ReferenceList<Team, TeamRef> Teams { get; }
        ReferenceList<Action, ActionRef> Actions { get; }

        List<MessageObject> Messages { get; set; }

        UserRef User { get; set; }
        ViewConfig ViewConfig { get; set; }
        void SaveToDatabase();

    }
}