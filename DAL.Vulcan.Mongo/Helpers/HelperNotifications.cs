using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Notifications;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = DAL.Vulcan.Mongo.DocClass.CRM.Action;
using CrmUser = DAL.Vulcan.Mongo.DocClass.CRM.CrmUser;

namespace DAL.Vulcan.Mongo.Helpers
{
    public class HelperNotifications : HelperBase, IHelperNotifications
    {
        private readonly IHelperUser _helperUser;

        public HelperNotifications(IHelperUser helperUser)
        {
            _helperUser = helperUser;
        }

        public Notification GetNotification(string notificationId)
        {
            return new RepositoryBase<Notification>().Find(notificationId);
        }

        public void RemoveNotification(Notification notification)
        {
            new RepositoryBase<Notification>().RemoveOne(notification);
        }

        public void ActionWasAdded(Action action)
        {
            NotificationRouter.ActionAdded(action);
        }

        public void ActionWasStarted(Action action)
        {
            NotificationRouter.ActionStarted(action);
        }

        public void ActionWasModified(Action action)
        {
            NotificationRouter.ActionModified(action);
        }

        public void ActionWasCompleted(Action action)
        {
            NotificationRouter.ActionCompleted(action);
        }

        public void CompanyGroupAddedToTeam(TeamRef team, CompanyGroupRef group)
        {
            NotificationRouter.CompanyGroupAddedToTeam(team.AsTeam(), group.AsCompanyGroup());
        }

        public void CompanyGroupRemovedFromTeam(TeamRef team, CompanyGroupRef group)
        {
            NotificationRouter.CompanyGroupRemovedFromTeam(team.AsTeam(), group.AsCompanyGroup());

        }

        public void CompanyAddedToTeam(TeamRef team, CompanyRef company)
        {
            NotificationRouter.CompanyAddedToTeam(team.AsTeam(), company.AsCompany());
        }

        public void CompanyRemovedFromTeam(TeamRef team, CompanyRef company)
        {
            NotificationRouter.CompanyRemovedFromTeam(team.AsTeam(), company.AsCompany());
        }

        public void UserAddedToTeam(CrmUser crmUser, TeamRef team)
        {
            NotificationRouter.UserAddedToTeam(crmUser.AsCrmUserRef(), team.AsTeam());
        }

        public void UserRemovedFromTeam(CrmUser crmUser, TeamRef team)
        {
            NotificationRouter.UserRemovedFromTeam(crmUser.AsCrmUserRef(), team.AsTeam());
        }

        public void SendRefreshNotificationsToUser(CrmUserRef crmUser, Notification notification)
        {
            NotificationRouter.Publisher.SendRefreshNotificationsToUser(crmUser, notification);
        }


        public void ActionWasRemoved(ActionRef actionRef)
        {
            RemoveNotificationsLinkedToAction(actionRef);
        }

        public void TeamCreated(TeamRef team)
        {
            NotificationRouter.TeamCreated(team.AsTeam());
        }


        public void GoalWasAdded(Goal goal)
        {
            NotificationRouter.GoalWasAdded(goal);
        }

        public void MilestoneAddedToGoal(Goal goal, Milestone milestone)
        {
            NotificationRouter.MilestoneAddedToGoal(goal, milestone);
        }

        public void ActionWasAddedToGoal(Action action, Goal goal)
        {
            NotificationRouter.ActionAddedToGoal(action);
        }

        public void RemovedActionFromGoal(Action action, Goal goal)
        {
            NotificationRouter.ActionRemovedFromGoal(action, goal);
        }

        public void ActionAssignedToMilestone(Goal goal, Action action, Milestone milestone)
        {
            NotificationRouter.ActionAssignedToMilestone(goal, action, milestone);
        }
        public void RemoveNotificationsLinkedToGoal(Goal goal)
        {
            var rep = new RepositoryBase<Notification>();

            foreach (var crmUser in goal.CrmUsers.Select(x => x.AsCrmUser()))
            {
                var refreshRequired = false;
                foreach (var notification in crmUser.Notifications.Select(x => x.AsNotification()).ToList())
                {
                    var goals = new LinkResolver<Goal>(notification).GetAllLinkedDocuments();
                    if (goals.Any(x => x.Id == goal.Id))
                    {
                        crmUser.RemoveLink(notification);
                        crmUser.SaveToDatabase();
                        refreshRequired = true;
                    }
                }
                if (refreshRequired)
                {
                    NotificationRouter.Publisher.SendRefreshNotificationsToUser(crmUser.AsCrmUserRef());
                }
            }

            var notificationsForThisGoal = new LinkResolver<Notification>(goal).GetAllLinkedDocuments();
            foreach (var activity in notificationsForThisGoal.ToList())
            {
                rep.RemoveOne(activity);
            }
        }

        public List<NotificationModel> GetMyNotifications(string application, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            if (crmUser == null) throw new Exception("User not found");

            //if (team == null) throw new Exception("No team found for user");
            //CleanAndRemoveDeadActivitiesFromTeam(team);
            var notifications = new List<Notification>();
            foreach (var notificationRef in crmUser.Notifications.ToList())
            {
                try
                {
                    var notification = notificationRef.AsNotification();
                    if (notification.Team == null)
                    {
                        notifications.Add(notification);
                    }
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    crmUser.Notifications.Remove(notificationRef);
                    crmUser.SaveToDatabase();
                }
            }

            return notifications.OrderByDescending(x => x.NotificationDate).Select(x => new NotificationModel(application, userId, x)).ToList();
        }

        public List<NotificationModel> GetMyTeamNotifications(string application, string userId)
        {
           
            var crmUser = _helperUser.GetCrmUser(application, userId);
            if (crmUser == null) throw new Exception("User not found");

            if (crmUser.ViewConfig.Team == null) throw new Exception("Not a member of a Team");

            //if (team == null) throw new Exception("No team found for user");
            //CleanAndRemoveDeadActivitiesFromTeam(team);
            var notifications = new List<Notification>();
            foreach (var notificationRef in crmUser.Notifications.ToList())
            {
                try
                {
                    
                    var notification = notificationRef.AsNotification();
                    if ((notification.Team != null) && (notification.Team.Id != crmUser.ViewConfig.Team.Id)) continue;

                    notifications.Add(notification);
                    
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    crmUser.Notifications.Remove(notificationRef);
                    crmUser.SaveToDatabase();
                }
            }
            /*
            var team = crmUser.ViewConfig.Team.AsTeam();
            foreach (var user in team.CrmUsers.Where(x=>x.UserId != crmUser.User.Id).Select(x=>x.AsCrmUser()).ToList())
            {
                foreach (var notification in user.Notifications.Select(x=>x.AsNotification()).Where(x=>x.Team != null).ToList())
                {
                    if (notifications.All(x => x.Id != notification.Id))
                    {
                        notifications.Add(notification);
                    }
                }
            }
            */
            return notifications.OrderByDescending(x=>x.NotificationDate).Select(x => new NotificationModel(application, userId, x)).ToList();
        }

        private void CleanAndRemoveDeadActivitiesFromTeam(Team team)
        {
            var rep = new RepositoryBase<Notification>();
            foreach (var activity in team.Notifications.ToList())
            {
                if (rep.Find(activity.Id) == null)
                {
                    team.Notifications.Remove(activity);
                    team.Save();
                }
            }

        }

        public Notification GetMostRecentNotificationForUser(CrmUser crmUser)
        {
            CleanAndRemoveDeadNotificationsFromUser(crmUser);
            return crmUser.Notifications.Select(x => x.AsNotification()).OrderByDescending(x => x.NotificationDate)
                .FirstOrDefault();
        }

        private static List<NotificationModel> GetNotificationsAsListOfNotificationModel(string application, string userId, List<Notification> notifications)
        {
            return notifications.Select(x => new NotificationModel(application, userId, x))
                .OrderByDescending(x => x.NotificationDate).ToList();
        }

        private static List<Notification> GetNotificationsWithAndWithoutTeamForUser(CrmUser crmUser, TeamRef team)
        {
            var activities = crmUser.Notifications.Select(x => x.AsNotification())
                .Where(x => x.Team == null || x.Team.Id == team.Id)
                .ToList();
            return activities;
        }

        private static List<NotificationModel> GetNotificationsWithoutTeam(string application, string userId, CrmUser crmUser)
        {
            var activities = crmUser.Notifications.Select(x => x.AsNotification()).Where(x => x.Team == null).ToList();
            return activities.Select(x => new NotificationModel(application, userId, x))
                .OrderByDescending(x => x.NotificationDate).ToList();
        }

        private static void CleanAndRemoveDeadNotificationsFromUser(CrmUser crmUser)
        {
            var rep = new RepositoryBase<Notification>();
            foreach (var notificationRef in crmUser.Notifications.ToList())
            {
                if (notificationRef == null)
                {
                    continue;
                }
                if (rep.Find(notificationRef.Id) == null)
                {
                    crmUser.Notifications.Remove(notificationRef);
                    crmUser.SaveToDatabase();
                }
            }
        }


        private void RemoveNotificationsLinkedToAction(ActionRef actionRef)
        {
            var repNotifs = new RepositoryBase<Notification>();
            var repCrmUsers = new RepositoryBase<CrmUser>();
            var crmUsersNeedingDeleteNotification = new List<CrmUser>();
            foreach (var crmUser in repCrmUsers.AsQueryable().ToList())
            {
                var refreshRequired = false;
                foreach (var notificationRef in crmUser.Notifications.ToList())
                {
                    var removeNotification = false;
                    var notification = notificationRef.AsNotification();

                    if ((notification == null) || ((notification.Action != null) && (notification.Action.Id == actionRef.Id)))
                    {
                        crmUser.Notifications.Remove(notificationRef);
                        crmUser.SaveToDatabase();
                        refreshRequired = true;
                        removeNotification = true;

                        if (crmUsersNeedingDeleteNotification.All(x => x.Id != crmUser.Id))
                        {
                            crmUsersNeedingDeleteNotification.Add(crmUser);
                            var deletedNotification = new Notification
                            {
                                ActionType = NotificationActionType.Removed,
                                PrimaryObjectType = NotificationObjectType.Information,
                                SecondaryObjectType = NotificationObjectType.None,
                                Label = $"Action for [{actionRef.Label}] was removed",
                                CreatedByUserId = crmUser.User.Id,
                                CrmUser = crmUser.AsCrmUserRef(),
                                Team = crmUser.ViewConfig.Team
                            };
                            deletedNotification.SaveToDatabase();
                            crmUser.Notifications.Add(deletedNotification.AsNotificationRef());
                            crmUser.SaveToDatabase();
                        }
                    }

                    if (removeNotification)
                    {
                        var notificationFound = repNotifs.Find(notificationRef.Id);
                        if (notificationFound != null)
                        {
                            repNotifs.RemoveOne(notificationFound);
                        }
                    }
                }
                if (refreshRequired)
                {
                    NotificationRouter.Publisher.SendRefreshNotificationsToUser(crmUser.AsCrmUserRef());
                }
            }
        }

    }
}
