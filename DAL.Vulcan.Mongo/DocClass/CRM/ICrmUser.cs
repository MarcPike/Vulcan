﻿using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Messages;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.DocClass.CRM
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