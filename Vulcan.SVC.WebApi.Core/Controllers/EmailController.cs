using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.FileAttachment;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class EmailController : BaseController
    {
        private readonly IHelperFile _helperFile;

        public EmailController(IHelperFile helperFile, IHelperUser helperUser) : base(helperUser)
        {
            _helperFile = helperFile;
        }

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("email/GetMyEmails/{application}/{userId}")]
        //public async Task<JsonResult> GetMyEmails(string application, string userId)
        //{

        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var statusCode = CheckToken(application, userId);
        //    try
        //    {
        //        await Task.Run(() =>
        //        {
        //            ThrowExceptionForBadToken(statusCode);
        //            result.Emails = _helperEmail.GetMyEmails(application, userId).Select(x =>
        //                new EmailModel(application, userId, x)).ToList();

        //            result.Success = true;
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
        //        result.ErrorMessage = e.Message;
        //        result.Success = false;
        //    }
        //    return JsonResultWithStatusCode(result, statusCode);


        //}
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("email/GetContactEmails/{application}/{userId}/{contactId}")]
        //public async Task<JsonResult> GetContactEmails(string application, string userId, string contactId)
        //{

        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var statusCode = CheckToken(application, userId);
        //    try
        //    {
        //        await Task.Run(() =>
        //        {
        //            ThrowExceptionForBadToken(statusCode);
        //            result.Emails = _helperEmail.GetContactEmails(contactId).Select(x =>
        //                new EmailModel(application, userId, x)).ToList();

        //            result.Success = true;
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
        //        result.ErrorMessage = e.Message;
        //        result.Success = false;
        //    }
        //    return JsonResultWithStatusCode(result, statusCode);


        //}

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("email/GetMyTeamEmails/{application}/{userId}")]
        //public async Task<JsonResult> GetMyTeamEmails(string application, string userId)
        //{

        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var statusCode = CheckToken(application, userId);
        //    try
        //    {
        //        await Task.Run(() =>
        //        {
        //            ThrowExceptionForBadToken(statusCode);
        //            result.Emails = _helperEmail.GetMyTeamEmails(application, userId).Select(x =>
        //                new EmailModel(application, userId, x)).ToList();

        //            result.Success = true;
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
        //        result.ErrorMessage = e.Message;
        //        result.Success = false;
        //    }
        //    return JsonResultWithStatusCode(result, statusCode);


        //}

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("email/DownloadEmailAttachment/{application}/{userId}/{emailId}/{attachmentId}")]
        //public IActionResult DownloadEmailAttachment(string application, string userId, string emailId, string attachmentId)
        //{

        //    var statusCode = CheckToken(application, userId);
        //    try
        //    {
        //        ThrowExceptionForBadToken(statusCode);
        //        var emailRep = new RepositoryBase<Email>();
        //        var email = emailRep.AsQueryable().FirstOrDefault(x => x.Id == ObjectId.Parse(emailId));

        //        if (email == null) throw new Exception("Email could not be found");

        //        var attachment = FileAttachmentsVulcan.GetAllAttachmentsForDocument(email)
        //            .SingleOrDefault(x => x.Id == ObjectId.Parse(attachmentId));

        //        if (attachment == null) throw new Exception("Attachment not found");

        //        var fileName = Path.GetFileName(attachment.Filename);
        //        var memory = FileAttachmentsVulcan.DownloadAsStream(attachment);
        //        memory.Position = 0;
        //        return File(memory, _helperFile.GetContentType(fileName), fileName);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        throw;
        //    }

        //}
    }
}