using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using DAL.Vulcan.Mongo.Core.Models;
using Action = DAL.Vulcan.Mongo.Core.DocClass.CRM.Action;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperAction
    {
        ActionModel CreateNewAction(string application, LdapUser user);
        Action GetAction(string actionId);
        Action SaveAction(ActionModel model);
        void RemoveAction(Action action);
        List<ActionRef> GetOpenActionsForUser(string application, string userId, DateTime minDate, DateTime maxDate);
        List<ActionRef> GetClosedActionsForUser(string application, string userId, DateTime minDate, DateTime maxDate);
        void RemoveAction(string application, string userId, string actionId);
        ActionModel GetActionModel(string application, string userId, string actionId);
        ActionScheduleModel GetActionScheduleModel(string application, string userId, string actionId);
        bool ApplyActionSchedule(ActionScheduleModel model);
    }
}