using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Notifications;
using DAL.Vulcan.Mongo.Core.DocClass.QueueSchedule;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Driver;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class ReminderController: BaseController
    {
        private const string TestQueueName = "ScheduledEvents";
        private readonly QueueSchedule _schedule;

        private readonly IHelperTeam _helperTeam;
        private readonly IHelperAction _helperAction;
        private readonly IHelperApplication _helperApplication;
        private readonly IHelperNotifications _helperNotifications;
        private readonly IHelperReminder _helperReminder;

        public ReminderController(
            IHelperUser helperUser, 
            IHelperTeam helperTeam, 
            IHelperAction helperAction, 
            IHelperApplication helperApplication, 
            IHelperNotifications helperNotifications,
            IHelperReminder helperReminder) : base(helperUser)
        {
            _helperTeam = helperTeam;
            _helperAction = helperAction;
            _helperApplication = helperApplication;
            _helperNotifications = helperNotifications;
            _helperReminder = helperReminder;

            _schedule = QueueSchedule.Helper.Find(x => x.Name == TestQueueName).FirstOrDefault();
        }

        [HttpGet]
        [Route("reminder/GetQueueSchedulePending/{application}/{userId}/{queueName?}")]
        public async Task<JsonResult> GetQueueSchedulePending(string application, string userId, string queueName = TestQueueName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperApplication.VerifyApplication(application);
                    var crmUser = GetCrmUser(application, userId);

                    if (crmUser.UserType != CrmUserType.Director &&
                        crmUser.UserType != CrmUserType.Manager &&
                        crmUser.UserType != CrmUserType.SalesPerson)
                    {
                        throw new Exception("You are not authorized to execute");
                    }

                    var schedule = QueueSchedule.Helper.Find(x => x.Name == queueName).FirstOrDefault();
                    result.Schedule = schedule?.Events.Where(x => x.Status != ScheduledEventWorkStatus.Completed).ToList();

                    result.Success = true;
                });

            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);


        }

        [HttpGet]
        [Route("reminder/GetQueueScheduleCompleted/{application}/{userId}/{queueName?}")]
        public async Task<JsonResult> GetQueueScheduleCompleted(string application, string userId, string queueName = TestQueueName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperApplication.VerifyApplication(application);
                    var crmUser = GetCrmUser(application, userId);

                    if (crmUser.UserType != CrmUserType.Director &&
                        crmUser.UserType != CrmUserType.Manager &&
                        crmUser.UserType != CrmUserType.SalesPerson)
                    {
                        throw new Exception("You are not authorized to execute");
                    }

                    var schedule = QueueSchedule.Helper.Find(x => x.Name == queueName).FirstOrDefault();
                    result.Schedule = schedule?.Events.Where(x => x.Status == ScheduledEventWorkStatus.Completed).ToList();

                    result.Success = true;
                });

            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("reminder/CancelPendingRemindersForNotification/{application}/{userId}/{notificationId}")]
        public async Task<JsonResult> CancelPendingRemindersForNotification(string application, string userId, string notificationId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperApplication.VerifyApplication(application);

                    var id = ObjectId.Parse(notificationId);
                    var activity = Notification.Helper.Find(x => x.Id == id).SingleOrDefault();

                    if (activity == null)
                    {
                        throw new Exception("Notification could not be found");
                    }

                    var scheduledEvents = new LinkResolver<ScheduledEvent>(activity).GetAllLinkedDocuments();


                    foreach (var thisEvent in scheduledEvents.Where(x => x.Status == ScheduledEventWorkStatus.Pending).ToList())
                    {
                        thisEvent.Status = ScheduledEventWorkStatus.Cancelled;
                    }
                    _schedule.Save();

                    _helperNotifications.RemoveNotification(activity);

                    var crmUser = GetCrmUser(application, userId);

                    result.Activities = _helperNotifications.GetMyNotifications(application, crmUser.UserId);

                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);


        }

        [AllowAnonymous]
        [HttpGet]
        [Route("reminder/GetReminderActionModel/{application}/{userId}/{actionId}")]
        public async Task<JsonResult> GetReminderActionModel(string application, string userId, string actionId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperApplication.VerifyApplication(application);

                    var crmUser = GetCrmUser(application, userId);
                    var action = _helperAction.GetAction(actionId);

                    var model = new ReminderActionModel(application, crmUser.UserId, actionId) { ExecuteOn = action.DueDate };
                    result.ReminderActionModel = model;
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);


        }

        [AllowAnonymous]
        [HttpPost]
        [Route("reminder/AddReminderToAction")]
        public async Task<JsonResult> AddReminderToAction([FromBody] ReminderActionModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    CheckForModelErrors();
                    _helperApplication.VerifyApplication(model.Application);
                    var crmUser = GetCrmUser(model.Application, model.UserId);
                    var action = _helperAction.GetAction(model.ActionId);
                    var remindersAdded = new List<ScheduledEvent>();


                    var scheduledEvent = _helperReminder.AddReminderForAction(
                        model.Label,
                        model.ExecuteOn,
                        crmUser,
                        action,
                        1,
                        new TimeSpan(0),
                        model.RemindAllTeamMembers);

                    remindersAdded.Add(scheduledEvent);

                    var onDate = model.ExecuteOn;
                    for (int i = 1; i < model.Repeat; i++)
                    {
                        if (model.RepeatMonths > 0)
                        {
                            onDate = onDate.AddMonths(model.RepeatMonths);
                        }
                        if (model.RepeatDays > 0)
                        {
                            onDate = onDate.AddDays(model.RepeatDays);
                        }
                        if (model.RepeatHours > 0)
                        {
                            onDate = onDate.AddHours(model.RepeatHours);
                        }
                        if (model.RepeatMinutes > 0)
                        {
                            onDate = onDate.AddMinutes(model.RepeatMinutes);
                        }

                        scheduledEvent = _helperReminder.AddReminderForAction(
                            model.Label,
                            onDate,
                            crmUser,
                            action,
                            1,
                            new TimeSpan(0),
                            model.RemindAllTeamMembers);

                        remindersAdded.Add(scheduledEvent);

                    }

                    result.RemindersAdded = remindersAdded;

                    result.Success = true;

                });

            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

    }
}
