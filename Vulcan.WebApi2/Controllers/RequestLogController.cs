using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.RequestLogging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;


namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class RequestLogController : BaseController
    {
        public RequestLogController(IHelperUser helperUser) : base(helperUser)
        {

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("requestLog/GetAllServerRequestsLastHour")]
        public async Task<JsonResult> GetAllServerRequestsLastHour()
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            result.Success = false;
            try
            {

                await Task.Run(() =>
                {
                    result.RequestLog = RequestLog.LastHour();
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
        [Route("requestLog/GetAllServerRequestsLastDay")]
        public async Task<JsonResult> GetAllServerRequestsLastDay()
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    result.RequestLog = RequestLog.LastDay();
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
