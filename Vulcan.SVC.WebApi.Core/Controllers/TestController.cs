using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAL.iMetal.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class TestController: BaseController
    {
        private readonly IHelperCurrencyForIMetal _helperCurrencyForIMetal;


        public TestController(IHelperUser helperUser, IHelperCurrencyForIMetal helperCurrencyForIMetal) : base(helperUser)
        {
            _helperUser = helperUser;
            _helperCurrencyForIMetal = helperCurrencyForIMetal;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(
            "test/GetExchangeRateForCOIDtoDisplayCurrency/{coid}/{displayCurrency}")]
        public async Task<JsonResult> GetExchangeRateForCoidToDisplayCurrency(string coid, string displayCurrency)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = HttpStatusCode.OK;
            try
            {
                
                await Task.Run(() =>
                {
                    var fromCurrency = _helperCurrencyForIMetal.GetDefaultCurrencyForCoid(coid);
                    result.ExchangeRate =
                        _helperCurrencyForIMetal.ConvertValueFromCurrencyToCurrency(1, fromCurrency, displayCurrency);

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
