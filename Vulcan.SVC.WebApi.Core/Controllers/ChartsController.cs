using System;
using System.Dynamic;
using System.Threading.Tasks;
using DAL.Marketing.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]

    public class ChartsController: BaseController
    {
        private IHelperTeam _helperTeam;
        private IHelperCompany _helperCompany;
        private IHelperChart _helperChart;

        public ChartsController(IHelperCompany helperCompany, IHelperTeam helperTeam, IHelperChart helperChart, IHelperUser helperUser) : base(helperUser)
        {
            _helperCompany = helperCompany;
            _helperTeam = helperTeam;
            _helperChart = helperChart;
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("charts/GetQuoteHistoryForCompany/{application}/{userId}/{coid}/{companyId}")]
        public async Task<JsonResult> GetQuoteHistoryForCompany(string application, string userId, string coid, string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var quotes = _helperChart.GetAllQuotesHistoryForCompany(coid, companyId);
                    var model = _helperChart.GetChartHistoryModelForQuotes(quotes);
                    _helperChart.CalculateAndSortModel(model);

                    result.ChartQuoteHistoryModel = model;
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

        [HttpGet]
        [AllowAnonymous]
        [Route("charts/GetQuoteHistoryForAllCompanies/{application}/{userId}/{coid}")]
        public async Task<JsonResult> GetQuoteHistoryForAllCompanies(string application, string userId, string coid)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var quotes = _helperChart.GetQuoteHistoryForAllQuotes(coid);
                    var model = _helperChart.GetChartHistoryModelForQuotes(quotes);
                    _helperChart.CalculateAndSortModel(model);

                    result.ChartQuoteHistoryModel = model;
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

        [HttpGet]
        [AllowAnonymous]
        [Route("charts/GetQuoteHistoryForTeam/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> GetQuoteHistoryForTeam(string application, string userId, string teamId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var quotes = _helperChart.GetQuoteHistoryForTeam(teamId);

                    var model = _helperChart.GetChartHistoryModelForQuotes(quotes);
                    _helperChart.CalculateAndSortModel(model);

                    result.ChartQuoteHistoryModel = model;
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

        [HttpGet]
        [AllowAnonymous]
        [Route("charts/GetQuoteHistoryForTeamAndCompany/{application}/{userId}/{teamId}/{companyId}")]
        public async Task<JsonResult> GetQuoteHistoryForTeamAndCompany(string application, string userId, string teamId, string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var quotes = _helperChart.GetQuoteHistoryForTeamAndCompany(teamId, companyId);
                    var model = _helperChart.GetChartHistoryModelForQuotes(quotes);
                    _helperChart.CalculateAndSortModel(model);

                    result.ChartQuoteHistoryModel = model;
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

        [HttpGet]
        [AllowAnonymous]
        [Route("charts/GetQuoteHistoryForStrategicAccount/{application}/{userId}/{strategicAccountId}")]
        public async Task<JsonResult> GetQuoteHistoryForStrategicAccount(string application, string userId, string strategicAccountId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var quotes = _helperChart.GetAllQuoteHistoryForStrategicAccount(strategicAccountId);

                    var model = _helperChart.GetChartHistoryModelForQuotes(quotes);
                    _helperChart.CalculateAndSortModel(model);

                    result.ChartQuoteHistoryModel = model;
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
