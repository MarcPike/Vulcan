using System;
using System.Dynamic;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class ProductAnalysisController : BaseController
    {
        public ProductAnalysisController(IHelperUser helperUser) : base(helperUser)
        {
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("productAnalysis/GetProductAnalysisQueryModel/{application}/{userId}")]
        public async Task<JsonResult> GetProductAnalysisQueryModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var model = new ProductAnalysisQueryModel();

                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    var team = crmUser.ViewConfig.Team;

                    if (team != null)
                    {
                        model.Teams.Add(team);
                    }

                    model.Application = application;
                    model.UserId = userId;

                    result.ProductAnalysisQueryModel = model;
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("productAnalysis/GetProductAnalysisResultModel")]
        public async Task<JsonResult> GetProductAnalysisResultModel([FromBody] ProductAnalysisQueryModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    result.ProductAnalysisResultModel = new ProductAnalysisResultModel(model);
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("productAnalysis/GetIncomingAnalysisQueryModel/{application}/{userId}")]
        public async Task<JsonResult> GetIncomingAnalysisQueryModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var model = new IncomingAnalysisQueryModel();

                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    var team = crmUser.ViewConfig.Team?.AsTeam();
                    if (team == null) throw new Exception("User does not have a Team");

                    model.CoidList.Add(team.Location.AsLocation().GetCoid());

                    result.IncomingAnalysisQueryModel = model;
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("productAnalysis/GetIncomingAnalysisResultModel")]
        public async Task<JsonResult> GetIncomingAnalysisResultModel([FromBody] IncomingAnalysisQueryModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    result.IncomingAnalysisResultModel = new IncomingAnalysisResultModel(model);
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("productAnalysis/GetCreditAnalysis/{application}/{userId}/{companyId}")]
        public async Task<JsonResult> GetCreditAnalysis(string application, string userId, string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var company = Company.Helper.FindById(companyId);
                    if (company == null) throw new Exception("Company not found");

                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    var team = crmUser.ViewConfig.Team.AsTeam();
                    var defaultCurrency = team.DefaultCurrency;

                    var model = new CompanyCreditAnalysisModel(companyId, defaultCurrency);
                    result.CompanyCreditAnalysis = model;
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

    }
}
