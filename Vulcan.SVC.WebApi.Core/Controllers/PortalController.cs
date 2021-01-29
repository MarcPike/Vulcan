using System;
using System.Dynamic;
using System.Threading.Tasks;
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
    public class PortalController : BaseController
    {
        private readonly IHelperPortal _helperPortal;

        public PortalController(IHelperUser helperUser, IHelperPortal helperPortal) : base(helperUser)
        {
            _helperPortal = helperPortal;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("portal/CreateNewPortalInvitationModel/{companyId}/{contactId}/{salesPersonId}")]
        public async Task<JsonResult> CreateNewPortalInvitationModel(string companyId, string contactId, string salesPersonId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", salesPersonId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.NewPortalInvitationModel =
                        _helperPortal.CreateNewPortalInvitationModel(companyId, contactId, salesPersonId);
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
        [Route("portal/SendPortalInvitation")]
        public async Task<JsonResult> SendPortalInvitation([FromBody] NewPortalInvitationModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", model.SalesPerson.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    result.PortalInvitationModel = _helperPortal.SendPortalInvitation(model);
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
        [Route("portal/GetInvitationsForTeam/{application}/{UserId}/{teamId}")]
        public async Task<JsonResult> GetInvitationsForTeam(string application, string userId, string teamId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    result.Invitations = _helperPortal.GetInvitationsForTeam(teamId);
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
        [Route("portal/GetInvitationsSentBySalesPerson/{application}/{UserId}/{salesPersonId}")]
        public async Task<JsonResult> GetInvitationsSentBySalesPerson(string application, string userId, string salesPersonId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    result.Invitations = _helperPortal.GetInvitationsSentBySalesPerson(salesPersonId);
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
        [Route("portal/GetInvitationsSentToCompany/{application}/{UserId}/{companyId}")]
        public async Task<JsonResult> GetInvitationsSentToCompany(string application, string userId, string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    result.Invitations = _helperPortal.GetInvitationsSentToCompany(companyId);
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
        [Route("portal/GetInvitationsSentToContact/{application}/{UserId}/{contactId}")]
        public async Task<JsonResult> GetInvitationsSentToContact(string application, string userId, string contactId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    result.Invitations = _helperPortal.GetInvitationsSentToContact(contactId);
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
