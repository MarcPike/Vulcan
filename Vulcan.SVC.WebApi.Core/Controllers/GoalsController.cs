using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Action = DAL.Vulcan.Mongo.Core.DocClass.CRM.Action;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class GoalsController : BaseController
    {
        private readonly IHelperApplication _helperApplication;
        private readonly IHelperGoal _helperGoal;
        private readonly IHelperAction _helperAction;
        private readonly IHelperNotifications _helperNotifications;

        public GoalsController(
            IHelperUser helperUser, 
            IHelperApplication helperApplication,
            IHelperGoal helperGoal,
            IHelperAction helperAction,
            IHelperNotifications helperNotifications) : base(helperUser)
        {
            _helperApplication = helperApplication;
            _helperGoal = helperGoal;
            _helperAction = helperAction;
            _helperNotifications = helperNotifications;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("goals/GetGoalsForUser/{application}/{userId}")]
        public async Task<JsonResult> GetGoalsForUser(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(application, userId);
                    result.Goals = _helperGoal.GetGoalsForUser(application, crmUser.User.Id);
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
        [Route("goals/GetGoalModel/{application}/{userId}/{goalId}")]
        public async Task<JsonResult> GetGoalModel(string application, string userId, string goalId)
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

                    var goal = _helperGoal.GetGoal(goalId);

                    var crmUser = GetCrmUser(application, userId);

                    result.ReadOnly = true;
                    if (crmUser.UserType == CrmUserType.Director)
                    {
                        if (goal.CrmUsers.Any(x => x.Id == crmUser.Id.ToString()))
                        {
                            result.ReadOnly = false;
                        }
                    }

                    if (crmUser.UserType == CrmUserType.Manager)
                    {
                        if (goal.CrmUsers.Any(x => x.Id == crmUser.Id.ToString()))
                        {
                            result.ReadOnly = false;
                        }
                    }
                    result.GoalModel = new GoalModel(goal, application, userId);
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
        [Route("goals/GetNewGoalModel/{application}/{userId}")]
        public async Task<JsonResult> GetNewGoalModel(string application, string userId)
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

                    result.ReadOnly = false;

                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    if ((crmUser.UserType != CrmUserType.Director) && (crmUser.UserType != CrmUserType.Manager))
                    {
                        throw new Exception("You must be a Director or a Manager in order to Create a new Goal");
                    }

                    GoalModel model = _helperGoal.CreateNewGoal(application, userId);
                    result.GoalModel = model;
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
        [Route("goals/RemoveGoal/{application}/{userId}/{goalId}")]
        public async Task<JsonResult> RemoveGoal(string application, string userId, string goalId)
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
                    var goal = _helperGoal.GetGoal(goalId);

                    if (!crmUser.IsAdmin)
                    {
                        if (goal.CreatedByUserId != userId)
                        {
                            throw new Exception("Cannot remove Goal due to the fact that this Goal was not created by this user and user is not Admin");
                        }
                    }

                    var actions = goal.Actions;
                    new LinkResolver<Action>(goal).RemoveAllLinksForThisType();
                    new LinkResolver<Team>(goal).RemoveAllLinksForThisType();

                    var repActions = new RepositoryBase<Action>();
                    foreach (var action in actions)
                    {
                        repActions.RemoveOne(action.AsAction());
                    }

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
        [Route("goals/SaveGoal")]
        public async Task<JsonResult> SaveGoal([FromBody] GoalModel model)
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

                    GoalModel resultModel = _helperGoal.SaveGoal(model);
                    result.GoalModel = resultModel;
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
        [Route("goals/AddMilestoneToGoal/{application}/{userId}/{goalId}/{milestoneLabel}")]
        public async Task<JsonResult> AddMilestoneToGoal(string application, string userId, string goalId, string milestoneLabel )
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
                    var goal = _helperGoal.GetGoal(goalId);

                    if (goal == null)
                    {
                        throw new Exception("Missing goal for SalesPerson");
                    }


                    if (crmUser.UserType == CrmUserType.Manager)
                    {
                        if (goal.CrmUsers.All(x => x.Id != crmUser.Id.ToString()))
                        {
                            throw new Exception("You are not a manager for this goal");
                        }
                    }
                    else if (crmUser.UserType != CrmUserType.SalesPerson)
                    {
                        if (goal.CrmUsers.All(x => x.Id != crmUser.Id.ToString()))
                        {
                            throw new Exception("You are not a SalesPerson for this goal");
                        }
                    }

                    var milestone = _helperGoal.AddMileStone(goal, milestoneLabel);
                    goal = _helperGoal.GetGoal(goalId);

                    result.Milestone = milestone;
                    result.Goal = goal;

                    _helperNotifications.MilestoneAddedToGoal(goal, milestone);

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
        [Route("goals/AddActionToGoal/{application}/{userId}/{goalId}/{actionId}")]
        public async Task<JsonResult> AddActionToGoal(string application, string userId, string goalId, string actionId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var goal = _helperGoal.GetGoal(goalId);
                    var action = _helperAction.GetAction(actionId);

                    if (action.Goal != null)
                    {
                        _helperNotifications.RemovedActionFromGoal(action, goal);
                    }

                    _helperGoal.AddAction(goal, action);

                    _helperNotifications.ActionWasAddedToGoal(action, goal);

                    result.Goal = goal;

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
        [Route("goals/AssignActionToMilestone/{application}/{userId}/{goalId}/{actionId}/{milestoneId}")]
        public async Task<JsonResult> AssignActionToMilestone(string application, string userId, string goalId, string actionId, string milestoneId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var goal = _helperGoal.GetGoal(goalId);
                    var action = _helperAction.GetAction(actionId);

                    var milestone = goal.Milestones.SingleOrDefault(x => x.Id.ToString() == milestoneId);
                    if (milestone == null)
                    {
                        throw new Exception("Milestone does not exists for this goal.");
                    }

                    _helperGoal.AssignActionToMilestone(goal, action, milestone);
                    _helperNotifications.ActionAssignedToMilestone(goal, action, milestone);
                    result.Goal = goal;
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
        [Route("goals/AddActionAndAssignToMilestone/{application}/{userId}/{goalId}/{actionId}/{milestoneId}")]
        public async Task<JsonResult> AddActionAndAssignToMilestone(string application, string userId, string goalId, string actionId, string milestoneId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var goal = _helperGoal.GetGoal(goalId);
                    var action = _helperAction.GetAction(actionId);

                    var milestone = goal.Milestones.SingleOrDefault(x => x.Id.ToString() == milestoneId);
                    if (milestone == null)
                    {
                        throw new Exception("Milestone does not exists for this goal.");
                    }

                    _helperGoal.AddActionAndAssignToMilestone(goal, action, milestone);

                    _helperNotifications.ActionWasAddedToGoal(action, goal);
                    _helperNotifications.ActionAssignedToMilestone(goal, action, milestone);

                    result.Goal = goal;

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