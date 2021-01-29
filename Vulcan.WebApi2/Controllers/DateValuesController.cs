using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DateValues;
using DAL.Vulcan.Mongo.Helpers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class DateValuesController : BaseController
    {
        public DateValuesController(IHelperUser helperUser) : base(helperUser)
        {

        }

        [HttpGet]
        [Route("dateValues/Get/{application}/{userId}")]
        public async Task<JsonResult> GetDateValues(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.DateValues = DateValueItem.GetDateValues();
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