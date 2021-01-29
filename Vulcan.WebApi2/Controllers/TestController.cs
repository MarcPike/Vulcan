using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.RequestLogging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Vulcan.IMetal.Queries.ProductCodes;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces(@"application/json", @"application/pdf")]
    public class TestController: BaseController
    {
        public TestController(IHelperUser helperUser) : base(helperUser)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("test/PerformTest")]
        public async Task<JsonResult> PerformTest()
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    var query = ProductMasterAdvancedQuery.AsQueryable("INC");
                    query = query.Where(x => x.ProductCode == "420M 5-2.75 HT");
                    result.ProductMaster = query.FirstOrDefault();
                    result.Success = true;
                    statusCode = HttpStatusCode.OK;
                });
            }
            catch (Exception e)
            {
                statusCode = HttpStatusCode.BadRequest;
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

    }
}
