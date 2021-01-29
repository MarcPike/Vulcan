using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class TeamController : BaseController
    {
        private readonly IHelperApplication _helperApplication;
        private readonly IHelperTeam _helperTeam;

        public TeamController(
            IHelperUser helperUser,
            IHelperApplication helperApplication,
            IHelperTeam helperTeam) : base(helperUser)

        {
            _helperApplication = helperApplication;
            _helperTeam = helperTeam;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("team/GetMyTeams/{application}/{userId}")]
        public async Task<JsonResult> GetMyTeams(string application, string userId)
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
                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    crmUser.SaveToDatabase();
                    result.Teams = _helperTeam.GetMyTeams(crmUser);
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
        [Route("team/GetAllTeams/{application}/{userId}")]
        public async Task<JsonResult> GetAllTeams(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.Teams = _helperTeam.GetAllTeams();
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
        [Route("team/RefreshTeamCompanyNames/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> RefreshTeamCompanyNames(string application, string userId, string teamId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    var team = _helperTeam.GetTeam(teamId);
                    if (crmUser.IsAdmin || crmUser.UserType == CrmUserType.Manager &&
                        crmUser.Teams.Any(x => x.Id == userId))
                        team.RefreshCompanyNames();
                    else
                        throw new Exception("Security level not adequate.");
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
        [Route("team/RemoveTeam/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> RemoveTeam(string application, string userId, string teamId)
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
                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    var team = _helperTeam.GetTeam(teamId);
                    if (crmUser.IsAdmin || crmUser.UserType == CrmUserType.Manager &&
                        crmUser.Teams.Any(x => x.Id == userId))
                        _helperTeam.RemoveTeam(team);
                    else
                        throw new Exception("Security level not adequate.");
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
        [Route("team/GetNewTeamModel/{application}/{userId}")]
        public async Task<JsonResult> GetNewTeamModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    if (crmUser.UserType != CrmUserType.Director && crmUser.UserType != CrmUserType.Manager)
                        throw new Exception("You must be a Director or a Manager in order to Create a Team");

                    var team = _helperTeam.CreateNewTeam(application, userId);
                    team.CrmUsers.Add(crmUser.AsCrmUserRef());
                    result.TeamModel = new TeamModel(team, application, userId);

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
        [Route("team/GetTeamModel/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> GetTeamModel(string application, string userId, string teamId)
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

                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    result.ReadOnly = false;
                    if (crmUser.UserType != CrmUserType.Director && crmUser.UserType != CrmUserType.Manager)
                        result.ReadOnly = true;

                    var team = _helperTeam.GetTeam(teamId);
                    result.TeamModel = new TeamModel(team, application, userId);
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
        [Route("team/SaveTeam")]
        public async Task<JsonResult> SaveTeam([FromBody] TeamModel model)
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

                    var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);
                    result.ReadOnly = false;

                    var team = _helperTeam.SaveTeam(model);
                    result.TeamModel = new TeamModel(team, model.Application, model.UserId);
                    result.TeamRef = team.AsTeamRef();
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
        [Route("team/GetTeamActivities/{application}/{userId}/{teamId}/{fromDateString}/{toDateString}")]
        public async Task<JsonResult> GetTeamActivities(string application, string userId, string teamId,
            string fromDateString, string toDateString)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var fromDate = DateTime.Parse(fromDateString);
                    var toDate = DateTime.Parse(toDateString);

                    _helperApplication.VerifyApplication(application);
                    var team = _helperTeam.GetTeam(teamId);

                    var notifications = team.Notifications.Select(x => x.AsNotification())
                        .Where(x => x.NotificationDate >= fromDate && x.NotificationDate <= toDate).ToList();
                    result.Notifications = notifications.Select(x => x.AsNotificationRef()).ToList();
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
        [Route("team/GetTeamMessages/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> GetTeamMessages(string application, string userId, string teamId)
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
                    var team = _helperTeam.GetTeam(teamId).AsTeamRef();
                    result.Messages = _helperTeam.GetTeamMessages(team);
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
        [Route("team/RefreshTeamCompaniesFromCompanyGroups/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> RefreshTeamCompaniesFromCompanyGroups(string application, string userId,
            string teamId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var team = _helperTeam.GetTeam(teamId).AsTeamRef().AsTeam();
                    team.RefreshTeamCompaniesList();
                    team.RefreshAllianceNonAllianceLists();
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

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("team/GetTeamPriceTier/{application}/{userId}")]
        //public async Task<JsonResult> GetTeamPriceTier(string application, string userId)
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var statusCode = CheckToken(application, userId);
        //    try
        //    {
        //        ThrowExceptionForBadToken(statusCode);

        //        var crmUser = _helperUser.GetCrmUser(application, userId);
        //        var team = crmUser.ViewConfig.Team.AsTeam();

        //        result.TeamTierPrices = _helperTeam.GetTeamPriceTier(team, application, userId);

        //        result.Success = true;
        //    }
        //    catch (Exception e)
        //    {
        //        result.ErrorMessage = e.Message;
        //    }
        //    return JsonResultWithStatusCode(result, statusCode);

        //}

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("team/TeamPriceTierAddProductCode/{application}/{userId}/{productCode}")]
        //public async Task<JsonResult> TeamPriceTierAddProductCode(string application, string userId, string productCode)
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var statusCode = CheckToken(application, userId);
        //    try
        //    {
        //        ThrowExceptionForBadToken(statusCode);

        //        var crmUser = _helperUser.GetCrmUser(application, userId);
        //        var team = crmUser.ViewConfig.Team.AsTeam();

        //        result.TeamTierPrices = _helperTeam.TeamPriceTierAddProductCode(team, productCode, application, userId);

        //        result.Success = true;
        //    }
        //    catch (Exception e)
        //    {
        //        result.ErrorMessage = e.Message;
        //    }
        //    return JsonResultWithStatusCode(result, statusCode);

        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("team/SaveTeamPriceTier")]
        //public async Task<JsonResult> SaveTeamPriceTier([FromBody] TeamPriceTierModel model)
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var statusCode = CheckToken(model.Application, model.UserId);
        //    try
        //    {
        //        ThrowExceptionForBadToken(statusCode);

        //        var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);
        //        var team = crmUser.ViewConfig.Team.AsTeam();

        //        result.TeamTierPrices = _helperTeam.SaveTeamPriceTierModel(model);

        //        result.Success = true;
        //    }
        //    catch (Exception e)
        //    {
        //        result.ErrorMessage = e.Message;
        //    }
        //    return JsonResultWithStatusCode(result, statusCode);

        //}
    }
}