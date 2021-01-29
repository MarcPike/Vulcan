using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Authentication;
using DAL.Vulcan.Mongo.Base.Logger;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.Helpers;

namespace Vulcan.WebApi2.Controllers
{
    public class BaseController: Controller
    {
        public BaseController(IHelperUser helperUser)
        {
            _logger = new VulcanLogger();
            _helperUser = helperUser;
        }
        protected VulcanLogger _logger;
        protected readonly IHelperUser _helperUser;

        public Dictionary<string, object> GetParametersDictionary()
        {
            return new Dictionary<string, object>();
        }

        protected string  GetClassName()
        {
            return this.GetType().Name;
        }

        protected void CheckForModelErrors()
        {
            if (!ModelState.IsValid)
            {
                var errors = this.ModelState.Values.First().Errors.ToList();
                var errorMessage = string.Empty;
                foreach (var modelError in this.ModelState.Values.First().Errors)
                {
                    errorMessage += modelError.ErrorMessage + "\n";
                }

                throw new Exception($"Model Errors: {errorMessage}");
            }
        }

        protected string GetHeaderValue(string name)
        {
            string headerValue = string.Empty;
            var index = 0;
            foreach (var key in HttpContext.Request.Headers.Keys)
            {

                if (key == name)
                {
                    break;
                }

                index++;
            }

            var onIndex = 0;
            foreach (var value in HttpContext.Request.Headers.Values)
            {

                if (onIndex != index)
                {
                    ++onIndex;
                }
                else
                {
                    headerValue = value;
                    break;
                }
            }
            return headerValue;
        }

        protected (string token, string application) GetTokenFromHeaders()
        {
            var token = GetHeaderValue("Authorization");
            var application = GetHeaderValue("Application");

            return (token, application);
        }

        protected HttpStatusCode CheckToken(string application, string userId)
        {
            try
            {
                var tokenData = _helperUser.GetUserToken(application, userId);
                if ((tokenData.expired) || (tokenData.token == null))
                {
                    return HttpStatusCode.Unauthorized;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return HttpStatusCode.Unauthorized;
            }

            return HttpStatusCode.OK;
        }

        protected void SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref HttpStatusCode statusCode)
        {
            if ((statusCode != HttpStatusCode.Unauthorized) && (statusCode != HttpStatusCode.Forbidden))
            {
                statusCode = HttpStatusCode.BadRequest;
            }
        }

        protected bool CheckCoid(string coid)
        {
            var validCoids = new List<string>()
            {
                "INC",
                "CAN",
                "EUR",
                "SIN",
                "MSA",
                "DUB"
            };

            if (validCoids.All(x => x != coid))
            {
                return false;
            }

            return true;
        }

        protected (CrmUserToken token, CrmUserInfo userInfo, CrmUser crmUser) GetAllCrmUserData(string application, string userId)
        {
            try
            {
                var tokenData = _helperUser.GetUserToken(application, userId);
                var userInfo = _helperUser.GetCrmUserInfo(application, userId);
                if (userInfo.CrmUserRef == null) throw new Exception("User not found");
                return (tokenData.token, userInfo, userInfo.CrmUserRef.AsCrmUser());
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        protected CrmUser GetCrmUser(string application, string userId)
        {
            try
            {
                var userInfo = _helperUser.GetCrmUserInfo(application, userId);
                if (userInfo.CrmUserRef == null) throw new Exception("User not found");
                return (userInfo.CrmUserRef.AsCrmUser());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected LdapUser GetUser(string userId)
        {
            try
            {
                return _helperUser.GetUser(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        protected CrmUserInfo GetCrmUserInfo(string application, string userId)
        {
            try
            {
                var userInfo = _helperUser.GetCrmUserInfo(application, userId);
                if (userInfo.CrmUserRef == null) throw new Exception("User not found");
                return userInfo;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        protected CrmUserToken GetToken(string application, string userId)
        {
            try
            {
                var tokenData = _helperUser.GetUserToken(application, userId);
                return tokenData.token;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        protected void ThrowExceptionForBadToken(HttpStatusCode code)
        {
            if (code  == HttpStatusCode.Unauthorized)
                throw new AuthenticationException("User token has expired");
        }

        protected JsonResult JsonResultWithStatusCode(dynamic value, HttpStatusCode statusCode)
        {
            return new JsonResult(value)
            {
                StatusCode = (int)statusCode
            };
        }

    }
}
