using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.HtmlTemplates;
using DAL.Vulcan.Mongo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class HtmlTemplateController: BaseController
    {

        public HtmlTemplateController(IHelperUser helperUser) : base(helperUser)
        {
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("htmlTemplates/GetAllTemplateNames/{application}/{adminId}")]
        public async Task<JsonResult> GetAllTemplateNames(string application, string adminId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, adminId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    CheckSecurity(application, adminId);


                    var rep = new RepositoryBase<HtmlTemplate>();
                    result.HtmlTemplateNames = rep.AsQueryable().Select(x => x.Name).ToList();
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
        [Route("htmlTemplates/GetTemplateForName/{application}/{adminId}/{templateName}")]
        public async Task<JsonResult> GetTemplateForName(string application, string adminId, string templateName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, adminId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    CheckSecurity(application, adminId);

                    var rep = new RepositoryBase<HtmlTemplate>();
                    var template = rep.AsQueryable().SingleOrDefault(x => x.Name == templateName);
                    if (template == null) throw new Exception("Template does not exist");
                    result.HtmlTemplate = new HtmlTemplateModel(template, application, adminId);
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
        [Route("htmlTemplates/GetTemplateForId/{application}/{adminId}/{templateId}")]
        public async Task<JsonResult> GetTemplateForId(string application, string adminId, string templateId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, adminId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    CheckSecurity(application, adminId);

                    var rep = new RepositoryBase<HtmlTemplate>();
                    var template = rep.Find(templateId);
                    if (template == null) throw new Exception("Template does not exist");
                    result.HtmlTemplate = new HtmlTemplateModel(template, application, adminId);
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

        [HttpPost]
        [AllowAnonymous]
        [Route("htmlTemplates/SaveTemplate")]
        public async Task<JsonResult> SaveTemplate([FromBody] HtmlTemplateModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    CheckSecurity(model.Application, model.UserId);

                    var rep = new RepositoryBase<HtmlTemplate>();
                    var template = rep.Find(model.Id);
                    if (template == null) throw new Exception("Template does not exist");

                    template.Html = model.Html;
                    rep.Upsert(template);

                    result.HtmlTemplate = new HtmlTemplateModel(template, model.Application, model.UserId);
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

        private void CheckSecurity(string application, string adminId)
        {
            var crmUser = GetCrmUser(application, adminId);
            if (!crmUser.IsAdmin)
            {
                throw new Exception("You are not authorized for this action");
            }
        }
    }
}
