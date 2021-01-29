using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = DAL.Vulcan.Mongo.DocClass.CRM.Action;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperAction : HelperBase, IHelperAction
    {
        private readonly IHelperNotifications _helperNotifications;
        private readonly IHelperApplication _helperApplication;
        private readonly IHelperUser _helperUser;
        private readonly IHelperReminder _helperReminder;

        public HelperAction(
            IHelperUser helperUser,
            IHelperNotifications helperNotifications,
            IHelperApplication helperApplication,
            IHelperReminder helperReminder)
        {
            _helperUser = helperUser;
            _helperNotifications = helperNotifications;
            _helperApplication = helperApplication;
            _helperReminder = helperReminder;
        }

        public ActionModel GetActionModel(string application, string userId, string actionId)
        {
            return ActionModel.GetActionModel(application,userId,actionId);
        }

        public Action GetAction(string actionId)
        {
            var action = new RepositoryBase<Action>().Find(actionId);
            if (action == null) throw new Exception("No action found");
            return action;
        }

        public Action SaveAction(ActionModel model)
        {
            var rep = new RepositoryBase<Action>();
            var user = _helperUser.GetUser(model.UserId);

            var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);
            var team = crmUser.ViewConfig.Team;

            var activityActions = new List<System.Action>();


            ValidatePercentComplete(model.PercentComplete);

            var action = rep.Find(model.Id);

            var addActionToUserRequired = false;

            if (action == null)
            {
                action = new Action
                {
                    Id = ObjectId.Parse(model.Id),
                    Label = model.Label,
                    DueDate = model.DueDate,
                    PercentComplete = model.PercentComplete,
                    CreatedByUserId = model.UserId,
                    CreateDateTime = DateTime.Now,
                    Team = model.Team ?? team,
                    ActionType = (ActionType)Enum.Parse(typeof(ActionType), model.ActionType, true)
                };

                addActionToUserRequired = true;
                CreateNotificationForActionAdded(activityActions, action);
            }
            else
            {
                CheckIfUserCanModify(user, action);

                SaveActionHistory(action, model);

                var taskWasCompleted = CreateNotificationForActionCompletedIfNeeded(model, action, activityActions);
                var taskWasStarted = CreateNotificationForActionStartedIfNeeded(model, action, activityActions);
                if (!taskWasStarted && !taskWasCompleted)
                    CreateNotificationForActionModifiedIfNeeded(model, action, activityActions);

                action.ActionType = (ActionType) Enum.Parse(typeof(ActionType), model.ActionType, true);
                action.Id = ObjectId.Parse(model.Id);
                action.Label = model.Label;

                CheckIfDueDateChangedWithReminderSet(action,model, crmUser);

                RemoveRemindersIfCompleted(action, model);

                action.DueDate = model.DueDate;
                action.PercentComplete = model.PercentComplete;

                action.ModifiedByUserId = model.UserId;
                action.ModifiedDateTime = DateTime.Now;
            }

            action.IsTeamAction = model.IsTeamAction;
            action.Addresses = model.Addresses;
            action.EmailAddresses = model.EmailAddresses;
            action.PhoneNumbers = model.PhoneNumbers;

            action.CrmUsers.ResyncWithList(model.CrmUsers);
            action.Contacts.ResyncWithList(model.Contacts);

            action.SearchTags = model.SearchTags;

            action.Notes.ResynchWithList(model.Notes);

            //var companyGroupsInModel = model.CompanyGroups.Select(x => x.AsCompanyGroup()).ToList();
            //LinkSynchronizer<CompanyGroup>.SynchronizeWithModelValues(task, companyGroupsInModel);

            //var companiesInModel = model.Companies.Select(x => x.AsCompany()).ToList();
            //LinkSynchronizer<Company>.SynchronizeWithModelValues(task, companiesInModel);

            //var contactsInModel = model.Contacts.Select(x => x.AsContact()).ToList();
            //LinkSynchronizer<Contact>.SynchronizeWithModelValues(task, contactsInModel);

            rep.Upsert(action);

            if (addActionToUserRequired)
            {
                crmUser.Actions.AddReferenceObject(action.AsActionRef());
                crmUser.SaveToDatabase();
            }
            else
            {
                var existingAction = crmUser.Actions.SingleOrDefault(x => x.Id == model.Id);
                if (existingAction != null)
                {
                    var indexOf = crmUser.Actions.IndexOf(existingAction);
                    crmUser.Actions[indexOf] = action.AsActionRef();
                    crmUser.SaveToDatabase();
                }

            }

            var otherCrmUsers = action.CrmUsers.Where(x => x.UserId != crmUser.User.Id).ToList();
            foreach (var otherCrmUser in otherCrmUsers.Select(x=>x.AsCrmUser()))
            {
                var thisActionRef = otherCrmUser.Actions.SingleOrDefault(x => x.Id == action.Id.ToString());
                if (thisActionRef == null)
                {
                    otherCrmUser.Actions.Add(action.AsActionRef());
                    otherCrmUser.SaveToDatabase();
                }
                else
                {
                    var indexOf = otherCrmUser.Actions.IndexOf(thisActionRef);
                    otherCrmUser.Actions[indexOf] = action.AsActionRef();
                    otherCrmUser.SaveToDatabase();
                }
            }

            // Run all our collected Actions
            foreach (var activityAction in activityActions)
            {
                activityAction.Invoke();
            }

            return action;
        }

        private void RemoveRemindersIfCompleted(Action action, ActionModel model)
        {
            if ((!action.IsCompleted) && (model.IsCompleted))
            {
                _helperReminder.RemoveAllRemindersForAction(action);
            }

        }

        private void CheckIfDueDateChangedWithReminderSet(Action action, ActionModel model, CrmUser user)
        {
            if ((action.DueDate != model.DueDate) && (model.ReminderMinutesPrior > 0))
            {
                _helperReminder.RemoveAllRemindersForAction(action);
                _helperReminder.AddReminderForAction("Reminder: " + action.Label,
                    model.DueDate.AddMinutes(-model.ReminderMinutesPrior), user, action, 1, new TimeSpan(0), false);
            }

        }


        public ActionScheduleModel GetActionScheduleModel(string application, string userId, string actionId)
        {
            var action = GetAction(actionId);
            return new ActionScheduleModel(application, userId, action);
        }

        public bool ApplyActionSchedule(ActionScheduleModel model)
        {
            try
            {
                var action = GetAction(model.ActionId);

                action.Schedule.ScheduleType =
                    (ActionScheduleType) Enum.Parse(typeof(ActionScheduleType), model.ScheduleType);
                action.Schedule.DaysFrom = model.DaysFrom;
                action.Schedule.EndDate = model.EndDate;
                action.SaveToDatabase();

                if (action.CrmUsers.All(x => x.UserId != model.UserId))
                {
                    throw new Exception("Not authorized to modify this Action");
                }

                action.ApplySchedule();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void RemoveActionFromCrmUsersLinkedToAction(Action action)
        {
            var rep = new RepositoryBase<CrmUser>();
            foreach (var crmUser in rep.AsQueryable().ToList())
            {
                var needToSave = false;
                var actionsToRemove = crmUser.Actions.Where(x => x.Id == action.Id.ToString()).ToList();
                foreach (var actionRefToRemove in actionsToRemove)
                {
                    needToSave = true;
                    crmUser.Actions.Remove(actionRefToRemove);
                }
                if (needToSave) crmUser.SaveToDatabase();
            }
        }

        public void RemoveAction(Action action)
        {
            var actionRef = action.AsActionRef();
            var activityActions =
                new List<System.Action> {() => _helperNotifications.ActionWasRemoved(actionRef)};

            RemoveActionFromCrmUsersLinkedToAction(action);
            new RepositoryBase<Action>().RemoveOne(action);

            // Run all our collected Actions
            foreach (var activityAction in activityActions)
            {
                activityAction.Invoke();
            }

        }

        public void RemoveAction(string application, string userId, string actionId)
        {
            _helperApplication.VerifyApplication(application);

            var action = GetAction(actionId);
            var user = _helperUser.GetUser(userId);

            CheckIfUserCanDelete(application, user, action);

            RemoveAction(action);
            
        }

        public List<ActionRef> GetOpenActionsForUser(string application, string userId, DateTime minDate, DateTime maxDate)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var actionsForMyStuff = crmUser.Actions.Where(x => x.DueDate >= minDate && x.DueDate <= maxDate && x.PercentComplete < 100).OrderBy(x => x.DueDate).ToList();
            if (crmUser.ViewConfig.ViewType == ViewType.MyStuff)
            {
                return actionsForMyStuff;
            }
            var rep = new RepositoryBase<Action>();
            var companyViewSelections = TeamUserCompanyViewSelections.GetTeamUserCompanyViewSelections(crmUser.User, crmUser.ViewConfig.Team);
            foreach (var companySelection in companyViewSelections.Alliances.Where(x=>x.Selected).ToList())
            {
                var actionsForCompany = rep.AsQueryable()
                    .Where(x => x.Companies.Any(c => c.Id == companySelection.Company.Id)).ToList();
                foreach (var action in actionsForCompany.Select(x=>x.AsActionRef()))
                {
                    if (actionsForMyStuff.All(x => x.Id != action.Id))
                    {
                        actionsForMyStuff.Add(action);
                    }
                }
            }

            foreach (var companySelection in companyViewSelections.NonAlliances.Where(x => x.Selected).ToList())
            {
                var actionsForCompany = rep.AsQueryable()
                    .Where(x => x.Companies.Any(c => c.Id == companySelection.Company.Id)).ToList();
                foreach (var action in actionsForCompany.Select(x => x.AsActionRef()))
                {
                    if (actionsForMyStuff.All(x => x.Id != action.Id))
                    {
                        actionsForMyStuff.Add(action);
                    }
                }
            }

            return actionsForMyStuff.OrderBy(x => x.DueDate).ToList();
        }

        public List<ActionRef> GetClosedActionsForUser(string application, string userId, DateTime minDate, DateTime maxDate)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var actionsForMyStuff = crmUser.Actions.Where(x => x.DueDate >= minDate && x.DueDate <= maxDate && x.PercentComplete == 100).OrderBy(x => x.DueDate).ToList();
            if (crmUser.ViewConfig.ViewType == ViewType.MyStuff)
            {
                return actionsForMyStuff;
            }
            var rep = new RepositoryBase<Action>();
            var companyViewSelections = TeamUserCompanyViewSelections.GetTeamUserCompanyViewSelections(crmUser.User, crmUser.ViewConfig.Team);
            foreach (var companySelection in companyViewSelections.Alliances.Where(x => x.Selected).ToList())
            {
                var actionsForCompany = rep.AsQueryable()
                    .Where(x => x.Companies.Any(c => c.Id == companySelection.Company.Id)).ToList();
                foreach (var action in actionsForCompany.Select(x => x.AsActionRef()))
                {
                    if (actionsForMyStuff.All(x => x.Id != action.Id))
                    {
                        actionsForMyStuff.Add(action);
                    }
                }
            }

            foreach (var companySelection in companyViewSelections.NonAlliances.Where(x => x.Selected).ToList())
            {
                var actionsForCompany = rep.AsQueryable()
                    .Where(x => x.Companies.Any(c => c.Id == companySelection.Company.Id)).ToList();
                foreach (var action in actionsForCompany.Select(x => x.AsActionRef()))
                {
                    if (actionsForMyStuff.All(x => x.Id != action.Id))
                    {
                        actionsForMyStuff.Add(action);
                    }
                }
            }

            return actionsForMyStuff.OrderBy(x => x.DueDate).ToList();
        }

        public List<ActionRef> GetActionsForUserTeam(string application, string userId, DateTime minDate, DateTime maxDate)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var team = crmUser.ViewConfig.Team.AsTeam();
            var result = new List<ActionRef>();
            foreach (var crmUserRef in team.CrmUsers)
            {
                result.AddRange(crmUserRef.AsCrmUser().Actions.Where(x => x.DueDate >= minDate && x.DueDate <= maxDate));
            }
            return result.Distinct().OrderBy(x => x.DueDate).ToList();
        }

        public ActionModel CreateNewAction(string application, LdapUser user)
        {
            return ActionModel.CreateNewActionModel(application, user.Id.ToString());
        }

        private void SaveActionHistory(Action action, ActionModel model)
        {
            if (action.PercentComplete != model.PercentComplete)
            {
                action.ActionHistory.Add(new ActionHistory()
                {
                    User = _helperUser.GetUser(model.UserId).AsUserRef(),
                    IsCompleted = model.PercentComplete == 100,
                    IsStarted = model.PercentComplete > 0 && model.PercentComplete < 100,
                    PercentComplete = model.PercentComplete,
                    ActionDate = DateTime.Now
                });
            }
        }

        private void CheckIfUserCanModify(LdapUser user, Action action)
        {
            var userId = user.Id.ToString();
            if (action.CreatedByUserId == user.Id.ToString()) return;

            if (action.CrmUsers.Any(x=>x.UserId == userId)) return;

            throw new Exception("This task is not assigned to you or created by you");
        }

        private void CheckIfUserCanDelete(string application, LdapUser user, Action action)
        {
            var isAdmin = user.GetApplicationRoles(application).Any(x => x == "Admin");

            if ((action.CreatedByUserId != user.Id.ToString()) && (!isAdmin))
            {
                throw new Exception("You must be an Admin to remove another User's task");
            }
        }

        private void ValidatePercentComplete(int percentComplete)
        {
            if ((percentComplete < 0) || (percentComplete > 100))
                throw new Exception("Percent Complete must be between 0 and 100");
        }

        private void CreateNotificationForActionAdded(List<System.Action> activityActions, Action action)
        {
            activityActions.Add(() => _helperNotifications.ActionWasAdded(action));
        }

        private bool CreateNotificationForActionStartedIfNeeded(ActionModel model, Action action, List<System.Action> activityActions)
        {
            if ((!action.IsCompleted) && (!action.IsStarted) && (model.PercentComplete > 0 && model.PercentComplete < 100))
            {
                activityActions.Add(() => _helperNotifications.ActionWasStarted(action));
                return true;
            }
            return false;
        }

        private void CreateNotificationForActionModifiedIfNeeded(ActionModel model, Action action, List<System.Action> activityActions)
        {
            if (model.PercentComplete != action.PercentComplete)
            {
                activityActions.Add(() => _helperNotifications.ActionWasModified(action));
            }

        }

        private bool CreateNotificationForActionCompletedIfNeeded(ActionModel model, Action action, List<System.Action> activityActions)
        {
            var result = false;
            if ((!action.IsCompleted && model.PercentComplete == 100))
            {
                activityActions.Add(() => _helperNotifications.ActionWasCompleted(action));
                result = true;
            }

            return result;
        }

    }
}