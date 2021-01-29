using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using DAL.Vulcan.Mongo.Core.DocClass.Notifications;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Models;
using DAL.Vulcan.Mongo.Core.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Vulcan.SVC.WebApi.Core.Models;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class DomainController : BaseController
    {
        private readonly IHelperDomain _helperDomain;
        
        public DomainController(
            IHelperDomain helperDomain,
            IHelperUser helperUser) : base(helperUser)
        {
            _helperDomain = helperDomain;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("domain/GetOemListForUpdates/{application}/{userId}")]
        public async Task<JsonResult> GetOemListForUpdates(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    var appPerm = new RepositoryBase<Permission>().AsQueryable().FirstOrDefault(x => x.Name == "CAN_UPDATE_OEM");
                    if (appPerm == null) throw new Exception("Missing App permissions, looking for CAN_UPDATE_OEM");

                    if (appPerm.Users.All(x => x.UserId != crmUser.UserId))
                    {
                        throw new Exception("You are not authorized to update Oem list");
                    }

                    ThrowExceptionForBadToken(statusCode);

                    result.OemUpdateList = OemUpdateModel.GetAll(application, userId);

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
        [Route("domain/OemUpdatePreCheck")]
        public async Task<JsonResult> OemUpdatePreCheck([FromBody] OemUpdateModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var preCheck = OemUpdateModel.PreCheck(model);

                    result.RowsAffected = preCheck.RowsAffected;
                    result.OemTypeWillBeRemoved = preCheck.OemTypeWillBeRemoved;

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
        [Route("domain/OemUpdateExecute")]
        public async Task<JsonResult> OemUpdateExecute([FromBody] OemUpdateModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.OemUpdateList = OemUpdateModel.Execute(model);
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
        [Route("domain/AddNewOemType/{application}/{userId}/{newOemType}")]
        public async Task<JsonResult> AddNewOemType(string application, string userId, string newOemType)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    if (string.IsNullOrEmpty(newOemType))
                    {
                        throw new Exception("OemType cannot be empty");
                    }

                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    var appPerm = new RepositoryBase<Permission>().AsQueryable().FirstOrDefault(x => x.Name == "CAN_UPDATE_OEM");
                    if (appPerm == null) throw new Exception("Missing App permissions, looking for CAN_UPDATE_OEM");

                    if (appPerm.Users.All(x => x.UserId != crmUser.UserId))
                    {
                        throw new Exception("You are not authorized to update Oem list");
                    }

                    var rep = new RepositoryBase<OemType>();
                    var existing = rep.AsQueryable().FirstOrDefault(x => x.Name == newOemType);
                    if (existing != null)
                    {
                        throw new Exception("OemType already exists");
                    }

                    rep.Upsert(new OemType()
                    {
                        Name = newOemType
                    });

                    result.OemUpdateList = OemUpdateModel.GetAll(application, userId);

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
        [Route("domain/RemoveOemType/{application}/{userId}/{removeOemType}")]
        public async Task<JsonResult> RemoveOemType(string application, string userId, string removeOemType)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    if (string.IsNullOrEmpty(removeOemType))
                    {
                        throw new Exception("OemType cannot be empty");
                    }

                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    var appPerm = new RepositoryBase<Permission>().AsQueryable().FirstOrDefault(x => x.Name == "CAN_UPDATE_OEM");
                    if (appPerm == null) throw new Exception("Missing App permissions, looking for CAN_UPDATE_OEM");

                    if (appPerm.Users.All(x => x.UserId != crmUser.UserId))
                    {
                        throw new Exception("You are not authorized to update Oem list");
                    }

                    ThrowExceptionForBadToken(statusCode);

                    var rep = new RepositoryBase<OemType>();
                    var existing = rep.AsQueryable().FirstOrDefault(x => x.Name == removeOemType);
                    if (existing == null)
                    {
                        throw new Exception("OemType does not exists");
                    }

                    var rowsUsing = new RepositoryBase<CrmQuoteItem>().AsQueryable().Count(x => x.OemType == removeOemType);

                    if (rowsUsing > 0)
                    {
                        throw new Exception($"{rowsUsing} Quote Items are currently using this OemType.  Cannot remove.");
                    }

                    rep.RemoveOne(existing);

                    result.OemUpdateList = OemUpdateModel.GetAll(application, userId);

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
        [Route("domain/GetAllDomainTypes/{application}/{userId}")]
        public async Task<JsonResult> GetAllDomainTypes(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ActionTypes = Enum.GetNames(typeof(NotificationActionType)).ToList();
                    result.ObjectTypes = Enum.GetNames(typeof(NotificationObjectType)).ToList();
                    result.PhoneTypes = Enum.GetNames(typeof(PhoneType)).ToList();
                    result.AddressTypes = Enum.GetNames(typeof(AddressType)).ToList();
                    result.EmailTypes = Enum.GetNames(typeof(EmailType)).ToList();

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
        [Route("domain/GetUserUploadImageModel/{application}/{userId}")]
        public async Task<JsonResult> GetUserUploadImageModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.UserUploadImageModel = new UploadUserImageModel(application, userId);
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
        [Route("domain/GetContactUploadImageModel/{application}/{userId}")]
        public async Task<JsonResult> GetContactUploadImageModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ContactUploadImageModel = new UploadContactImageModel(application, userId);
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
        [Route("domain/GetAllActivityTypes/{application}/{userId}")]
        public async Task<JsonResult> GetAllActivityTypes(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ActionTypes = Enum.GetNames(typeof(NotificationActionType)).ToList();
                    result.ObjectTypes = Enum.GetNames(typeof(NotificationObjectType)).ToList();

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
        [Route("domain/GetNewPhoneNumber/{application}/{userId}")]
        public async Task<JsonResult> GetNewPhoneNumber(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.PhoneNumber = _helperDomain.GetNewPhoneNumber();
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
        [Route("domain/GetNewAddress/{application}/{userId}")]
        public async Task<JsonResult> GetNewAddress(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.Address = _helperDomain.GetNewAddressModel();
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
        [Route("domain/GetNewEmailAddress/{application}/{userId}")]
        public async Task<JsonResult> GetNewEmailAddress(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.EmailAddress = _helperDomain.GetNewEmailAddress();
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
        [Route("domain/GetNewNote/{application}/{userId}")]
        public async Task<JsonResult> GetNewNote(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.Note = new Note();
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