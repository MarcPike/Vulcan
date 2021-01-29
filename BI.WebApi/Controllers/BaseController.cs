using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using BI.DAL.Mongo.Security;
using BI.DAL.Mongo.Helpers;
using BI.DAL.Mongo.Logging;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Logger;
using Microsoft.AspNetCore.Mvc;

namespace BI.WebApi.Controllers
{
    public class BaseController: Controller
    {
        public BaseController()
        {
            _logger = new Logger();
        }

        protected Logger _logger;
        protected IHelperUser _helperUser = new HelperUser();

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
                StringBuilder errorList = new StringBuilder();
                errorList.AppendLine("Model has the following errors:");
                foreach (var modelError in errors)
                {
                    errorList.AppendLine(modelError.Exception.Message);
                }
                throw new Exception(errorList.ToString());
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

        protected string GetTokenFromHeaders()
        {
            if (DeveloperToken.Token != null)
            {
                return DeveloperToken.Token.TokenId;
            }

            

            var token = GetHeaderValue("Authorization");
            //var application = GetHeaderValue("Application");

            return token;
        }

        protected (HttpStatusCode StatusCodeResult, string TokenId, string UserId) GetTokenDataFromHeaders()
        {
            var resultTokenId = string.Empty;
            var resultUserId = string.Empty;
            try
            {
                var token = GetTokenFromHeaders();

                var tokenData = _helperUser.GetUserToken(token);
                if ((tokenData.expired) || (tokenData.token == null))
                {
                    return (HttpStatusCode.Unauthorized, string.Empty, string.Empty);
                }

                resultTokenId = tokenData.token.Id.ToString();
                resultUserId = tokenData.user.Id.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (HttpStatusCode.Unauthorized, string.Empty, string.Empty);
            }

            return (HttpStatusCode.OK, resultTokenId, resultUserId);
        }


        protected LdapUser GetUser(string userId)
        {
            try
            {
                return _helperUser.GetLdapUser(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        protected BiUserToken GetToken(string userId)
        {
            try
            {
                var tokenData = _helperUser.GetUserToken(userId);
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

        protected void SetStatusToBadRequestIfNeeded(ref HttpStatusCode code)
        {
            if (code != HttpStatusCode.Unauthorized) code = HttpStatusCode.BadRequest;
        }

        protected void ThrowExceptionForError(string errorText)
        {
        }

        protected JsonResult JsonResultWithStatusCode(dynamic value, HttpStatusCode statusCode)
        {
            return new JsonResult(value)
            {
                StatusCode = (int)statusCode
            };
        }

        //protected JsonResult JsonNoCamelCase(dynamic value)
        //{
        //    var camelSettings = new JsonSerializerSettings { ContractResolver = new DefaultNamingStrategy() };
        //    return JsonConvert.SerializeObject(value, camelSettings);
        //}

    }
}
