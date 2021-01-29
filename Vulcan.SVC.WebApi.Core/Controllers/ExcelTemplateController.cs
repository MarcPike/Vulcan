using System;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;
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
    public class ExcelTemplateController : BaseController
    {
        private readonly IHelperQuote _helperQuote;
        private readonly IHelperExcelTemplate _helperExcelTemplate;

        public ExcelTemplateController(IHelperQuote helperQuote, IHelperUser helperUser, IHelperExcelTemplate helperExcelTemplate) : base(helperUser)
        {
            _helperQuote = helperQuote;
            _helperExcelTemplate = helperExcelTemplate;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("excelTemplate/GetNewExcelTemplateModel/{application}/{userId}")]
        public async Task<JsonResult> GetNewExcelTemplateModel(string application, string userId)
        {

            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = _helperUser.GetCrmUser(application, userId);

                    var model = _helperExcelTemplate.GetNewExcelTemplateModel(crmUser);
                    model.Application = application;
                    model.UserId = userId;

                    result.ExcelTemplateModel = model;
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
        [Route("excelTemplate/GetExcelTemplateModel/{application}/{userId}/{templateId}")]
        public async Task<JsonResult> GetExcelTemplateModel(string application, string userId, string templateId)
        {

            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var model = _helperExcelTemplate.GetExcelTemplateModel(templateId);
                    model.Application = application;
                    model.UserId = userId;

                    result.ExcelTemplateModel = model;
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
        [HttpPost]
        [Route("excelTemplate/SaveExcelTemplateModel/{application}/{userId}")]
        public async Task<JsonResult> SaveExcelTemplateModel([FromBody] ExcelTemplateModel model)
        {

            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ExcelTemplateModel = _helperExcelTemplate.SaveExcelTemplate(model);
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
        [Route("excelTemplate/GetTemplatesForMyTeam/{application}/{userId}")]
        public async Task<JsonResult> GetTemplatesForMyTeam(string application, string userId)
        {

            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    var models = _helperExcelTemplate.GetTemplatesForTeam(crmUser.ViewConfig.Team);
                    foreach (var model in models)
                    {
                        model.Application = application;
                        model.UserId = userId;
                    }

                    result.ExcelTemplates = models;
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
        [Route("excelTemplate/RemoveExcelTemplate/{application}/{userId}/{templateId}")]
        public async Task<JsonResult> GetTemplatesForMyTeam(string application, string userId, string templateId)
        {

            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperExcelTemplate.RemoveExcelTemplate(templateId);
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
        [Route("excelTemplate/ExportQuoteUsingTemplate/{application}/{userId}/{quoteId}/{templateId}")]
        public IActionResult ExportQuoteUsingTemplate(string application, string userId, string quoteId, string templateId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                result.Success = true;
                var quote = _helperQuote.GetQuote(quoteId);

                var fileName = $"{quote.QuoteId}-{quote.RevisionNumber}.xlsx";
                var workbook = _helperExcelTemplate.GenerateExcelStream(application, userId, quoteId, templateId);

                using (var memory = new MemoryStream())
                {
                    workbook.Write(memory);
                    return File(memory.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}");
                }
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);
        }


        //[AllowAnonymous]
        //[HttpGet]
        //[Route("excelTemplate/ExportDummyQuoteUsingTemplate/{application}/{userId}/{templateId}")]
        //public IActionResult ExportDummyQuoteUsingTemplate(string application, string userId, string templateId)
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var statusCode = CheckToken(application, userId);
        //    try
        //    {
        //        ThrowExceptionForBadToken(statusCode);
        //        result.Success = true;



        //        var fileName = $"00000-1.xlsx";
        //        var workbook = _helperExcelTemplate.GenerateExcelStreamDummy(application, userId, templateId);

        //        using (var memory = new MemoryStream())
        //        {
        //            workbook.Write(memory);
        //            return File(memory.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
        //        result.ErrorMessage = e.Message;
        //        result.Success = false;
        //    }
        //    return JsonResultWithStatusCode(result, statusCode);
        //}


    }
}
