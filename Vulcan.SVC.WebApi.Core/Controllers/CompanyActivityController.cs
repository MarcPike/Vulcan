using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Marketing.Core.Helpers;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class CompanyActivityController : BaseController
    {
        private readonly IHelperQuote _helperQuote;
        private readonly IHelperMarketing _helperMarketing;

        public CompanyActivityController(
            IHelperUser helperUser, 
            IHelperQuote helperQuote, 
            IHelperMarketing helperMarketing) : base(helperUser)
        {
            _helperQuote = helperQuote;
            _helperMarketing = helperMarketing;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("companyActivity/GetCustomerActivity/{application}/{userId}/{beginDate}/{endDate}/{salesPersonId}")]
        public async Task<JsonResult> GetCustomerActivity(string application, string userId, DateTime beginDate, DateTime endDate, string salesPersonId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var crmUser = GetCrmUser(application, userId);

                    result.CustomerActivityView = _helperQuote.GetCustomerActivityView(application, crmUser.UserId, beginDate, endDate, salesPersonId);
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
        [Route("companyActivity/GetProspectActivity/{application}/{userId}/{beginDate}/{endDate}/{salesPersonId}")]
        public async Task<JsonResult> GetProspectActivity(string application, string userId, DateTime beginDate, DateTime endDate, string salesPersonId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var crmUser = GetCrmUser(application, userId);

                    result.CustomerActivityView = _helperQuote.GetProspectActivityView(application, crmUser.UserId, beginDate, endDate, salesPersonId);
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
        [Route("companyActivity/GetQuotePipelineForCustomerActivity/{application}/{userId}/{beginDate}/{endDate}/{companyOrProspectId}/{salesPersonId}/{prospectsInsteadOfCompanies}")]
        public async Task<JsonResult> GetQuotePipelineForCustomerActivity(string application, string userId, DateTime beginDate,
            DateTime endDate, string companyOrProspectId, string salesPersonId, bool prospectsInsteadOfCompanies)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var crmUser = GetCrmUser(application, userId);

                    result.QuotePipelineModel = _helperQuote.GetQuotePipelineForCustomerActivity(application, crmUser.UserId, beginDate, endDate, companyOrProspectId, salesPersonId, prospectsInsteadOfCompanies);
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
        [Route("companyActivity/GetTimelineAndMaterialChartModelsForCompany/{application}/{userId}/{displayCurrency}/{companyId}/{beginDate}/{endDate}/{salesPersonId}")]
        public async Task<JsonResult> GetTimelineAndMaterialChartModelsForCompany(string application, string userId, string displayCurrency, string companyId, DateTime beginDate, DateTime endDate, string salesPersonId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var repQuotes = new RepositoryBase<CrmQuote>();
                    var quotes = repQuotes.AsQueryable().Where(x => x.Company != null &&
                                                                    x.SalesPerson.Id == salesPersonId &&
                                                                    x.ReportDate != null &&
                                                                    x.ReportDate >= beginDate &&
                                                                    x.ReportDate <= endDate &&
                                                                    x.Company.Id == companyId).ToList();

                    var charts = _helperMarketing.GetTimelineAndMaterialModels(displayCurrency, quotes);
                    result.ChartQuoteHistoryModel = charts.ChartQuoteHistoryModel;
                    result.HitRateByMetalCategoryChartData = charts.HitRateByMetalCategoryChartData;
                    result.MaterialMargin = charts.MaterialMargin;
                    result.SellingMargin = charts.SellingMargin;
                    result.TotalDollarsByMetalCategoryChartData = charts.TotalDollarsByMetalCategoryChartData;

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
        [Route("companyActivity/GetTimelineAndMaterialChartModelsForProspect/{application}/{userId}/{displayCurrency}/{prospectId}/{beginDate}/{endDate}/{salesPersonId}")]
        public async Task<JsonResult> GetTimelineAndMaterialChartModelsForProspect(string application, string userId, string displayCurrency, string prospectId, DateTime beginDate, DateTime endDate, string salesPersonId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var crmUser = GetCrmUser(application, userId);

                    var repQuotes = new RepositoryBase<CrmQuote>();
                    var quotes = repQuotes.AsQueryable().Where(x => x.Prospect != null &&
                                                                    x.SalesPerson.Id == salesPersonId &&
                                                                    x.ReportDate != null &&
                                                                    x.ReportDate >= beginDate &&
                                                                    x.ReportDate <= endDate &&
                                                                    x.Prospect.Id == prospectId).ToList();

                    var charts = _helperMarketing.GetTimelineAndMaterialModels(displayCurrency, quotes);
                    result.ChartQuoteHistoryModel = charts.ChartQuoteHistoryModel;
                    result.HitRateByMetalCategoryChartData = charts.HitRateByMetalCategoryChartData;
                    result.MaterialMargin = charts.MaterialMargin;
                    result.SellingMargin = charts.SellingMargin;
                    result.TotalDollarsByMetalCategoryChartData = charts.TotalDollarsByMetalCategoryChartData;

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
