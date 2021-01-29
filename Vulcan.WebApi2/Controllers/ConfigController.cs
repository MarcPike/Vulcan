using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Config;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]

    public class ConfigController: BaseController
    {
        private readonly IHelperLocation _helperLocation;
        private readonly IHelperTeam _helperTeam;
        private readonly IHelperCompany _helperCompany;

        public ConfigController(IHelperUser helperUser, IHelperLocation helperLocation, IHelperTeam helperTeam, IHelperCompany helperCompany) : base(helperUser)
        {
            _helperLocation = helperLocation;
            _helperTeam = helperTeam;
            _helperCompany = helperCompany;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("config/GetCoidConfiguration/{application}/{userId}/{coid}")]
        public async Task<JsonResult> GetCoidConfiguration(string application, string userId, string coid)
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
                    result.CoidConfiguration = CoidConfigurationModel.GetForCoid(application, crmUser.User.Id, coid);
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
        [Route("config/GetCompanyConfiguration/{application}/{userId}/{companyId}")]
        public async Task<JsonResult> GetCompanyConfiguration(string application, string userId, string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var companyRef = _helperCompany.GetCompanyRef(companyId);
                    var crmUser = GetCrmUser(application, userId);
                    result.CompanyConfiguration = CompanyConfigurationModel.GetForCompany(application, crmUser.User.Id, companyRef);
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
        [Route("config/GetLocationConfiguration/{application}/{userId}/{locationId}")]
        public async Task<JsonResult> GetLocationConfiguration(string application, string userId, string locationId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var locationRef = _helperLocation.GetLocation(locationId).AsLocationRef();
                    var crmUser = GetCrmUser(application, userId);
                    result.LocationConfiguration = LocationConfigurationModel.GetForLocation(application, crmUser.User.Id, locationRef);
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
        [Route("config/GetTeamConfiguration/{application}/{userId}/{teamId}/{name}")]
        public async Task<JsonResult> GetTeamConfiguration(string application, string userId, string teamId, string name)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var teamRef = _helperTeam.GetTeam(teamId).AsTeamRef();
                    var crmUser = GetCrmUser(application, userId);
                    result.LocationConfiguration = TeamConfigurationModel.GetForTeam(application, crmUser.User.Id, teamRef, name);
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
        [Route("config/GetUserConfiguration/{application}/{userId}/{name}")]
        public async Task<JsonResult> GetUserConfiguration(string application, string userId, string name)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUserRef = GetCrmUser(application, userId).AsCrmUserRef();
                    result.UserConfiguration = UserConfigurationModel.GetForUser(application, crmUserRef.UserId, crmUserRef, name);
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
        [Route("config/SaveCoidConfiguration")]
        public async Task<JsonResult> SaveCoidConfiguration([FromBody] CoidConfigurationModel model)
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

                    var rep = new RepositoryBase<CoidConfiguration>();
                    var coidConfiguration = rep.Find(model.Id);
                    coidConfiguration.Configuration = model.Configuration;
                    rep.Upsert(coidConfiguration);
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return Json(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("config/SaveCompanyConfiguration")]
        public async Task<JsonResult> SaveCompanyConfiguration([FromBody] CompanyConfigurationModel model)
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

                    var rep = new RepositoryBase<CompanyConfiguration>();
                    var companyConfiguration = rep.Find(model.Id);
                    companyConfiguration.Configuration = model.Configuration;
                    rep.Upsert(companyConfiguration);
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
        [Route("config/SaveLocationConfiguration")]
        public async Task<JsonResult> SaveLocationConfiguration([FromBody] LocationConfigurationModel model)
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

                    var rep = new RepositoryBase<LocationConfiguration>();
                    var locationConfiguration = rep.Find(model.Id);
                    locationConfiguration.Configuration = model.Configuration;
                    locationConfiguration.DefaultMargins = model.DefaultMargins;
                    rep.Upsert(locationConfiguration);
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
        [Route("config/SaveTeamConfiguration")]
        public async Task<JsonResult> SaveTeamConfiguration([FromBody] TeamConfigurationModel model)
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

                    var rep = new RepositoryBase<TeamConfiguration>();
                    var teamConfiguration = rep.Find(model.Id);
                    teamConfiguration.Configuration = model.Configuration;
                    teamConfiguration.Name = model.Name;
                    rep.Upsert(teamConfiguration);
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
        [Route("config/SaveUserConfiguration")]
        public async Task<JsonResult> SaveUserConfiguration([FromBody] UserConfigurationModel model)
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

                    var rep = new RepositoryBase<UserConfiguration>();
                    var userConfiguration = rep.Find(model.Id);
                    userConfiguration.Configuration = model.Configuration;
                    userConfiguration.Name = model.Name;
                    rep.Upsert(userConfiguration);
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
