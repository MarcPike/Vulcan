using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]

    public class UserController : BaseController
    {
        private readonly IHelperApplication _helperApplication;
        private readonly IHelperTeam _helperTeam;

        private static readonly int MinSize = 1;
        private static readonly int MaxSize = 512;

        private int _size = 80;

        /// <summary>
        /// The default image to be shown if no Gravatar is found for an email address.
        /// </summary>
        public string DefaultImage { get; set; }

        /// <summary>
        /// The size, in pixels, of the Gravatar to render.
        /// </summary>
        public int Size
        {
            get => _size;
            set
            {
                if (value < MinSize || value > MaxSize)
                    throw new ArgumentOutOfRangeException("Size",
                        "The allowable range for 'Size' is '" + MinSize +
                        "' to '" + MaxSize + "', inclusive.");
                _size = value;
            }
        }

        ///// <summary>
        ///// The maximum Gravatar rating allowed to display.
        ///// </summary>
        //public GravatarRating MaxRating { get; set; } = GravatarRating.PG;

        public UserController(
            IHelperApplication helperApplication,
            IHelperUser helperUser,
            IHelperTeam helperTeam) : base(helperUser)
        {
            _helperApplication = helperApplication;
            _helperTeam = helperTeam;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user/ConnectionMade/{application}/{userId}")]
        public async Task<JsonResult> ConnectionMade(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperUser.UserConnectedCommand(application, userId);
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
        [Route("user/GetCrmUserLog/{application}/{userId}")]
        public async Task<JsonResult> GetCrmUserLog(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.UserLogSummaryModel =
                        new CrmUserLogSummaryModel(new RepositoryBase<CrmUserLog>().AsQueryable().ToList());
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
        [Route("user/GetUserToken/{application}/{userId}")]
        public async Task<JsonResult> GetUserToken(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var tokenData = _helperUser.GetUserToken(application, userId);
                    result.UserToken = tokenData.token;
                    result.Expired = tokenData.expired;
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                statusCode = HttpStatusCode.Unauthorized;
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user/GetCrmUserInfo/{application}/{userId}")]
        public new async Task<JsonResult> GetCrmUserInfo(string application, string userId)
        {

            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.CrmUserInfo = _helperUser.GetCrmUserInfo(application, userId);
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                statusCode = HttpStatusCode.Unauthorized;
                result.ErrorMessage = e.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("user/GetUserPersonModel/{application}/{userId}")]
        public async Task<JsonResult> GetUserPersonModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.UserPersonModel = _helperUser.GetUserPersonModel(userId);
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
        [Route("user/SaveUserPersonModel")]
        public async Task<JsonResult> SaveUserPersonModel([FromBody] UserPersonModel model)
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
                    result.UserPersonModel = _helperUser.SaveUserPersonModel(model);
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
        [Route("user/GetTeamUserCompanyViewSelections/{application}/{userId}")]
        public async Task<JsonResult> GetTeamUserCompanyViewSelections(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.TeamUserCompanyViewSelectionsModel = _helperUser.GetTeamUserCompanyViewSelectionsModel(application, userId);
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
        [Route("user/SaveTeamUserCompanyViewSelections")]
        public async Task<JsonResult> SaveTeamUserCompanyViewSelections([FromBody] TeamUserCompanyViewSelectionsModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application,model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    CheckForModelErrors();
                    result.TeamUserCompanyViewSelectionsModel = _helperUser.SaveTeamUserCompanyViewSelectionsModel(model);
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
        [Route("user/ChangeUserLocation/{application}/{userId}/{userToModify}/{moveToLocationId}")]
        public async Task<JsonResult> ChangeUserLocation(string application, string userId, string userToModify, string moveToLocationId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.CrmUserModel = _helperUser.ChangeUserLocation(application, userId, userToModify, moveToLocationId);
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
        [Route("user/SaveCrmUserModel")]
        public async Task<JsonResult> SaveCrmUserModel([FromBody] CrmUserModel model)
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
                    _helperUser.SaveCrmUserModel(model);
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
        [Route("user/GetCrmUserModel/{application}/{userId}/{userIdForModel}")]
        public async Task<JsonResult> GetCrmUserModel(string application, string userId, string userIdForModel)
        {

            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.CrmUserModel = _helperUser.GetCrmUserModel(application, userIdForModel);
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
        [Route("user/SetUserViewMyStuff/{application}/{userId}")]
        public async Task<JsonResult> SetUserViewMyStuff(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var tokenData = _helperUser.GetUserToken(application, userId);
                    result.UserToken = tokenData.token;
                    result.Expired = tokenData.expired;
                    if (tokenData.expired)
                    {
                        throw new Exception("Security token has expired");
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
        [HttpGet]
        [Route("user/SetUserViewTeam/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> SetUserViewTeam(string application, string userId, string teamId)
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
                    if (crmUser.Teams.All(x => x.Id != teamId))
                    {
                        throw new Exception($"{crmUser.User.GetFullName()} Teams List does not have this Team");
                    }
                    VerifyUserIsOnTeam(application, teamId, userId);
                    var team = _helperTeam.GetTeam(teamId).AsTeamRef();
                    crmUser.ViewConfig.Team = team;
                    crmUser.SaveToDatabase();
                    result.UserInfo = _helperUser.GetCrmUserInfo(application, userId);
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

        private void VerifyUserIsOnTeam(string application, string salesTeamId, string userId)
        {
            var crmUser = _helperUser.GetCrmUser(application, userId);
            var teams = _helperTeam.GetTeamsForUser(application, userId);
            var team = _helperTeam.GetTeam(salesTeamId);
            if (teams.All(x => x.Id != salesTeamId))
                throw new Exception($"{crmUser.User.GetFullName()} is not a member of Team {team.Name}");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user/GetTeamsForUser/{application}/{userId}")]
        public async Task<JsonResult> GetTeamsForUser(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var user = _helperUser.GetUser(userId);
                    result.Teams = _helperTeam.GetTeamsForUser(application, userId);
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
        [Route("user/GetAllSalesPersons/{application}/{userId}")]
        public async Task<JsonResult> GetAllSalesPersons(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.SalesPersons = _helperUser.GetAllSalesPersons(application);
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
        [Route("user/GetAllManagers/{application}/{userId}")]
        public async Task<JsonResult> GetAllManagers(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.Managers = _helperUser.GetAllManagers(application);
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
        [Route("user/GetAllDirectors/{application}/{userId}")]
        public async Task<JsonResult> GetAllDirectors(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.Directors = _helperUser.GetAllDirectors(application);
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
        [Route("user/GetAllSalesPersonsNotInTeam/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> GetAllSalesPersonsNotInTeam(string application, string userId, string teamId)
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
                    var allSalesPersons = _helperUser.GetAllSalesPersons(application);
                    var team = _helperTeam.GetTeam(teamId);

                    var availableSalesPersons = new List<CrmUserRef>();
                    foreach (var salesPersonRef in allSalesPersons.ToList())
                    {
                        var id = ObjectId.Parse(salesPersonRef.Id);
                        var existingSalesPerson = team.CrmUsers.FirstOrDefault(x => x.Id == id.ToString());
                        if (existingSalesPerson == null)
                        {
                            availableSalesPersons.Add(salesPersonRef);
                        }
                    }
                    result.AvailableSalesPersons = availableSalesPersons;

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
        [Route("user/GetAllManagersNotInTeam/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> GetAllManagersNotInTeam(string application, string userId, string teamId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var allManagers = _helperUser.GetAllManagers(application);
                    var team = _helperTeam.GetTeam(teamId);
                    var availableManagers = new List<CrmUserRef>();
                    foreach (var managerRef in allManagers.ToList())
                    {
                        var id = ObjectId.Parse(managerRef.Id);
                        var existingManager = team.CrmUsers.FirstOrDefault(x => x.Id == id.ToString());
                        if (existingManager == null)
                        {
                            availableManagers.Add(managerRef);
                        }
                    }
                    result.AvailableManagers = availableManagers;
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
        [Route("user/GetAllDirectorsNotInTeam/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> GetAllDirectorsNotInTeam(string application, string userId, string teamId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var allDirectors = _helperUser.GetAllDirectors(application);
                    var team = _helperTeam.GetTeam(teamId);

                    var availableDirectors = new List<CrmUserRef>();
                    foreach (var directorRef in allDirectors.ToList())
                    {
                        var id = ObjectId.Parse(directorRef.Id);
                        var existingManager = team.CrmUsers.FirstOrDefault(x => x.Id == id.ToString());
                        if (existingManager == null)
                        {
                            availableDirectors.Add(directorRef);
                        }
                    }
                    result.AvailableDirectors = availableDirectors;
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
        [Route("user/GetAllEmployees/{application}/{userId}")]
        public async Task<JsonResult> GetAllEmployees(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var allEmployees = _helperUser.GetAllEmployees(application);
                    result.AllEmployees = allEmployees;
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
        [Route("user/CreateAndOrSetUserAsUserType/{application}/{adminId}/{userId}/{userType}/{isAdmin}/{readOnly}/{isCalcAdmin}")]
        public async Task<JsonResult> CreateAndOrSetUserAsUserType(string application, string adminId, string userId, string userType, bool isAdmin, bool readOnly, bool isCalcAdmin)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, adminId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var admin = _helperUser.GetCrmUser(application, adminId);
                    if (!admin.IsAdmin) throw new Exception("You are not authorized");
                    var userTypeConverted = (CrmUserType)Enum.Parse(typeof(CrmUserType), userType);
                    var crmUserModel = _helperUser.CreateAndOrSetUserAsUserType(application, userId, userTypeConverted, isAdmin, readOnly, isCalcAdmin);
                    result.CrmUserModel = crmUserModel;
                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    result.UserModel = new UserModel(crmUser);
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
        [Route("user/GetExistingUserReferencesForApplication/{application}/{userId}")]
        public async Task<JsonResult> GetExistingUserReferencesForApplication(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ExistingUsers = _helperUser.GetExistingUserReferencesForApplication(application);
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
        [Route("user/GetAllAvailableUsers/{application}/{userId}")]
        public async Task<JsonResult> GetAllAvailableUsers(string application, string userId)
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
                    var existingUsers = _helperUser.GetExistingUsersForApplication(application);
                    result.AvailableUsers = _helperUser.GetAllAvailableNewUsersForApplication(application, "", existingUsers);
                    result.ExistingUsers = existingUsers;
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
        [Route("user/GetExistingUsers/{application}/{userId}")]
        public async Task<JsonResult> GetExistingUsers(string application, string userId)
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
                    var existingUsers = _helperUser.GetExistingUsersForApplication(application);
                    result.ExistingUsers = existingUsers;
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
        [Route("user/GetAllAvailableUsersWithLastName/{application}/{userId}/{lastName}")]
        public async Task<JsonResult> GetAllAvailableUsersWithLastName(string application, string userId, string lastName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);

            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    if (lastName == "(none)") lastName = "";
                    var crmUser = _helperUser.GetCrmUser(application, userId);

                    var existingUsers = _helperUser.GetExistingUsersForApplication(application);
                    var availableUsers = _helperUser.GetAllAvailableNewUsersForApplication(application, lastName, existingUsers);
                    result.AvailableUsers = availableUsers;
                    result.ExistingUsers = existingUsers;
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
        [Route("user/KillUser/{application}/{userId}/{existingUserId}")]
        public async Task<JsonResult> KillUser(string application, string userId, string existingUserId)
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
                    if (!crmUser.IsAdmin)
                    {
                        throw new Exception($"{crmUser.User.GetFullName()} is not a Manager or Admin");
                    }
                    var user = _helperUser.GetUser(existingUserId);
                    _helperUser.RemoveUser(application, existingUserId);
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