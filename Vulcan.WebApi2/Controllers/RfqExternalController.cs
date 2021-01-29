using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.RequestForQuoteExternal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class RfqExternalController : BaseController
    {
        private readonly IHelperRfq _helperRfq;

        public RfqExternalController(IHelperUser helperUser, IHelperRfq helperRfq) : base(helperUser)
        {
            _helperRfq = helperRfq;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("rfq/GetNewRfqExternalModelForCustomer")]
        public async Task<JsonResult> GetNewRfqExternalModelForCustomer()
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;

            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    result.RfqCustomerModel = _helperRfq.GetNewRfqExternalModelForCustomer();
                    result.LocationsList = _helperRfq.GetLocationNamesForRfq();
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
        [Route("rfq/SaveRfqExternal")]
        public async Task<JsonResult> SaveRfqExternal([FromBody] RfqCustomerModel model)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;

            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    _helperRfq.SaveRfqExternal(model);
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
        [Route("rfq/GetAllForTeam/{application}/{userId}/{fromDate}/{toDate}")]
        public async Task<JsonResult> GetAllForTeam(string application, string userId, DateTime fromDate, DateTime toDate)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    var team = crmUser.ViewConfig.Team;
                    result.ExternalRfqList = _helperRfq.GetAllForTeam(application, userId, team, fromDate, toDate);
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
        [Route("rfq/Accept")]
        public async Task<JsonResult> Accept([FromBody] RfqExternalModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);
                    model.SalesPerson = crmUser.AsCrmUserRef();
                    result.RfqExternalModel = _helperRfq.Accept(model);
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
        [Route("rfq/Reject")]
        public async Task<JsonResult> Reject([FromBody] RfqExternalModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);
                    model.SalesPerson = crmUser.AsCrmUserRef();
                    result.RfqExternalModel = _helperRfq.Reject(model);
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
        [Route("rfq/GetTeamConfig/{application}/{userId}")]
        public async Task<JsonResult> GetTeamConfig(string application, string userId)
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
                    var team = crmUser.ViewConfig.Team;
                    result.RfqTeamConfigModel = _helperRfq.GetTeamConfig(application, userId, team);
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
        [Route("rfq/SaveTeamConfig")]
        public async Task<JsonResult> SaveTeamConfig([FromBody] RfqTeamConfigModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    result.RfqTeamConfigModel = _helperRfq.SaveTeamConfig(model);
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
