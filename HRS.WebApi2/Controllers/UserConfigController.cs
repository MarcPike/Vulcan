using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Config;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class UserConfigController: BaseController
    {

        [AllowAnonymous]
        [HttpGet]
        [Route("config/GetUserConfiguration/{userId}/{name}")]
        public JsonResult GetUserConfiguration(string userId, string name)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(userId);

                result.UserConfiguration = UserConfigurationModel.GetForUser(hrsUser.AsHrsUserRef(), name);

                result.Success = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("config/SaveUserConfiguration")]
        public JsonResult SaveUserConfiguration([FromBody] UserConfigurationModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                CheckForModelErrors();

                var rep = new RepositoryBase<UserConfiguration>();
                var userConfiguration = rep.Find(model.Id);

                userConfiguration.Configuration = model.Configuration;
                userConfiguration.Name = model.Name;

                rep.Upsert(userConfiguration);

                result.UserConfiguration = new UserConfigurationModel(userConfiguration);
                result.Success = true;

            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

    }
}
