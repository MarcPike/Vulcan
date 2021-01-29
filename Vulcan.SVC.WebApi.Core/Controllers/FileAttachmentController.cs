using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.EMail;
using DAL.Osiris;
using DAL.Quotes.Mongo.Core.DocClass.FileAttachment;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.FileAttachment;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using Action = DAL.Vulcan.Mongo.Core.DocClass.CRM.Action;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    //[Produces(@"application/json", @"application/pdf")]
    //[Consumes("application/json")]
    public class FileAttachmentController: BaseController
    {
        private readonly IHelperApplication _helperApplication;
        private readonly IHelperAction _helperAction;
        private readonly IHelperQuote _helperQuote;

        public FileAttachmentController(IHelperApplication helperApplication, IHelperUser helperUser, IHelperAction helperAction, IHelperQuote helperQuote) : base(helperUser)
        {
            _helperApplication = helperApplication;
            _helperAction = helperAction;
            _helperQuote = helperQuote;
        }

        #region Action Attachments


        [AllowAnonymous]
        [HttpPost]
        [Route("fileAttachment/UploadAttachmentForAction/{application}/{userId}/{actionId}")]
        public ActionResult UploadAttachmentForAction(string application, string userId, string actionId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                _helperApplication.VerifyApplication(application);

                var crmUser = GetCrmUser(application, userId);

                var action = _helperAction.GetAction(actionId);
                if (action == null) throw new Exception("Action not found");

                if (!crmUser.IsAdmin)
                {
                    throw new Exception($"{crmUser.User.GetFullName()} is not a Manager or Admin");
                }

                if (action.CrmUsers.All(x => x.UserId != crmUser.User.Id))
                {
                    throw new Exception("You are not associated as a User for this Action");
                }

                var file = HttpContext.Request.Form.Files[0];
                if (file == null)
                {
                    throw new Exception("No file sent to Upload");
                }

                var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var fileAttachmentId = FileAttachmentsVulcan.SaveFileAttachment(
                    fileBytes, file.FileName, action, FileAttachmentType.Action, crmUser.User.Id);

                var attachedFiles = FileAttachmentsVulcan.GetAllAttachmentsForDocument(action) ?? new List<GridFSFileInfo>();
                result.AttachedFiles = attachedFiles.Select(x => new FileAttachmentModel(x)).ToList();

                result.Success = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("fileAttachment/RemoveAttachmentForAction/{application}/{userId}/{actionId}/{attachmentId}")]
        public async Task<JsonResult> RemoveAttachmentForAction(string application, string userId, string actionId, string attachmentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {

                    ThrowExceptionForBadToken(statusCode);
                    _helperApplication.VerifyApplication(application);

                    var crmUser = GetCrmUser(application, userId);

                    var action = _helperAction.GetAction(actionId);
                    if (action == null) throw new Exception("Action not found");

                    if (!crmUser.IsAdmin)
                    {
                        throw new Exception($"{crmUser.User.GetFullName()} is not a Manager or Admin");
                    }

                    if (action.CrmUsers.All(x => x.UserId != crmUser.User.Id))
                    {
                        throw new Exception("You are not associated as a User for this Action");
                    }

                    var attachedFiles = FileAttachmentsVulcan.GetAllAttachmentsForDocument(action) ??
                                        new List<GridFSFileInfo>();

                    var fileToRemove = attachedFiles.FirstOrDefault(x => x.Id == ObjectId.Parse(actionId));
                    if (fileToRemove == null)
                    {
                        throw new Exception("No matching attachment found");
                    }

                    FileAttachmentsVulcan.Remove(fileToRemove.Id);
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("fileAttachment/DownloadAttachmentForAction/{application}/{userId}/{actionId}/{attachmentId}")]
        public IActionResult DownloadAttachmentForAction(string application, string userId, string actionId, string attachmentId)
        {
            try
            {
                var statusCode = CheckToken(application, userId);
                ThrowExceptionForBadToken(statusCode);
                var rep = new RepositoryBase<Action>();
                var action = rep.AsQueryable().FirstOrDefault(x => x.Id == ObjectId.Parse(actionId));
                if (action == null) throw new Exception("Action could not be found");

                var attachment = FileAttachmentsVulcan.GetAllAttachmentsForDocument(action)
                    .SingleOrDefault(x => x.Id == ObjectId.Parse(attachmentId));

                if (attachment == null) throw new Exception("Attachment not found");
                var fileName = Path.GetFileName(attachment.Filename);
                var memory = FileAttachmentsVulcan.DownloadAsStream(attachment);
                memory.Position = 0;
                return File(memory, GetContentType(fileName), fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        #endregion

        #region Quote Attachments


        [AllowAnonymous]
        [HttpPost]
        [Route("fileAttachment/UploadAttachmentForQuote/{application}/{userId}/{quoteId}")]
        public ActionResult UploadAttachmentForQuote(string application, string userId, string quoteId)
        {
            CheckForModelErrors();

            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                _helperApplication.VerifyApplication(application);

                var crmUser = GetCrmUser(application, userId);
                var quote = _helperQuote.GetQuote(quoteId);
                if (quote == null) throw new Exception("Quote not found");

                var file = HttpContext.Request.Form.Files[0];
                if (file == null)
                {
                    throw new Exception("No file sent to Upload");
                }

                var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var fileAttachmentId = FileAttachmentsVulcan.SaveFileAttachment(
                    fileBytes, file.FileName, quote, FileAttachmentType.Quote, crmUser.User.Id);

                var attachedFiles = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quote) ?? new List<GridFSFileInfo>();
                result.AttachedFiles = attachedFiles.Select(x => new FileAttachmentModel(x)).ToList();

                result.Success = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("fileAttachment/RemoveAttachmentForQuote/{application}/{userId}/{quoteId}/{attachmentId}")]
        public async Task<JsonResult> RemoveAttachmentForQuote(string application, string userId, string quoteId, string attachmentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperApplication.VerifyApplication(application);

                    var crmUser = GetCrmUser(application, userId);

                    var quote = _helperQuote.GetQuote(quoteId);
                    if (quote == null) throw new Exception("Quote not found");

                    var attachedFiles = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quote) ?? new List<GridFSFileInfo>();

                    var fileToRemove = attachedFiles.FirstOrDefault(x => x.Id == ObjectId.Parse(attachmentId));
                    if (fileToRemove == null)
                    {
                        throw new Exception("No matching attachment found");
                    }

                    FileAttachmentsVulcan.Remove(fileToRemove.Id);
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
        [Route("fileAttachment/DownloadAttachmentForQuote/{application}/{userId}/{quoteId}/{attachmentId}")]
        public IActionResult DownloadAttachmentForQuote(string application, string userId, string quoteId, string attachmentId)
        {
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                var rep = new RepositoryBase<CrmQuote>();
                var quote = rep.AsQueryable().FirstOrDefault(x => x.Id == ObjectId.Parse(quoteId));

                if (quote == null) throw new Exception("Quote could not be found");

                var attachment = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quote)
                    .SingleOrDefault(x => x.Id == ObjectId.Parse(attachmentId));

                if (attachment == null) throw new Exception("Attachment not found");

                var fileName = Path.GetFileName(attachment.Filename);
                var memory = FileAttachmentsVulcan.DownloadAsStream(attachment);
                memory.Position = 0;
                return File(memory, GetContentType(fileName), fileName);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("fileAttachment/DownloadAttachmentBytesForQuote/{application}/{userId}/{quoteId}/{attachmentId}")]
        public async Task<JsonResult> DownloadAttachmentBytesForQuote(string application, string userId, string quoteId, string attachmentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var rep = new RepositoryBase<CrmQuote>();

                    var quote = rep.AsQueryable().FirstOrDefault(x => x.Id == ObjectId.Parse(quoteId));

                    if (quote == null) throw new Exception("Quote could not be found");

                    var attachment = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quote)
                        .SingleOrDefault(x => x.Id == ObjectId.Parse(attachmentId));

                    if (attachment == null) throw new Exception("Attachment not found");

                    var fileName = Path.GetFileName(attachment.Filename);
                    var memory = FileAttachmentsVulcan.DownloadAsStream(attachment);
                    memory.Position = 0;

                    byte[] fileData = ReadFully(memory);

                    result.FileName = fileName;
                    result.MimeType = GetContentType(fileName);
                    result.FileInfo = new FileAttachmentModel(attachment);
                    result.FileData = fileData;
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
        [Route("fileAttachment/GetAttachmentsForCompany/{application}/{userId}/{companyId}")]
        public async Task<JsonResult> GetAttachmentsForCompany(string application, string userId, string companyId)
        {
            CheckForModelErrors();

            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperApplication.VerifyApplication(application);

                    var crmUser = GetCrmUser(application, userId);
                    var company = new RepositoryBase<Company>().Find(companyId);
                    if (company == null) throw new Exception("Company not found");

                    var attachedFiles = FileAttachmentsVulcan.GetAllAttachmentsForDocument(company) ?? new List<GridFSFileInfo>();

                    result.AttachedFiles = attachedFiles.Select(x => new FileAttachmentModel(x)).ToList();

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
        [Route("fileAttachment/UploadAttachmentForCompany/{application}/{userId}/{companyId}")]
        public async Task<ActionResult> UploadAttachmentForCompany(string application, string userId, string companyId)
        {
            CheckForModelErrors();

            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperApplication.VerifyApplication(application);

                    var crmUser = GetCrmUser(application, userId);
                    var company = new RepositoryBase<Company>().Find(companyId);
                    if (company == null) throw new Exception("Company not found");

                    var file = HttpContext.Request.Form.Files[0];
                    if (file == null)
                    {
                        throw new Exception("No file sent to Upload");
                    }

                    var ms = new MemoryStream();
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    var attachedFiles = FileAttachmentsVulcan.GetAllAttachmentsForDocument(company) ?? new List<GridFSFileInfo>();

                    var existingFile = attachedFiles.FirstOrDefault(x => x.Filename == file.FileName);
                    if (existingFile != null)
                    {
                        RemoveFile(company, existingFile.Id.ToString());
                    }

                    var fileAttachmentId = FileAttachmentsVulcan.SaveFileAttachment(
                        fileBytes, file.FileName, company, FileAttachmentType.Company, crmUser.User.Id);

                    result.AttachedFiles = attachedFiles.Select(x => new FileAttachmentModel(x)).ToList();

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
        [Route("fileAttachment/RemoveAttachmentForCompany/{application}/{userId}/{companyId}/{attachmentId}")]
        public async Task<JsonResult> RemoveAttachmentForCompany(string application, string userId, string companyId, string attachmentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperApplication.VerifyApplication(application);

                    var company = new RepositoryBase<Company>().Find(companyId);
                    if (company == null) throw new Exception("Company not found");

                    RemoveFile(company, attachmentId);
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
        [Route("fileAttachment/DownloadAttachmentForCompany/{application}/{userId}/{companyId}/{attachmentId}")]
        public IActionResult DownloadAttachmentForCompany(string application, string userId, string companyId, string attachmentId)
        {
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                var rep = new RepositoryBase<Company>();
                var company = rep.AsQueryable().FirstOrDefault(x => x.Id == ObjectId.Parse(companyId));

                if (company == null) throw new Exception("Company could not be found");

                var attachment = FileAttachmentsVulcan.GetAllAttachmentsForDocument(company)
                    .SingleOrDefault(x => x.Id == ObjectId.Parse(attachmentId));

                if (attachment == null) throw new Exception("Attachment not found");

                var fileName = Path.GetFileName(attachment.Filename);
                var memory = FileAttachmentsVulcan.DownloadAsStream(attachment);
                memory.Position = 0;
                return File(memory, GetContentType(fileName), fileName);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("fileAttachment/DownloadAttachmentBytesForCompany/{application}/{userId}/{companyId}/{attachmentId}")]
        public async Task<JsonResult> DownloadAttachmentBytesForCompany(string application, string userId, string companyId, string attachmentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var rep = new RepositoryBase<Company>();

                    var company = rep.AsQueryable().FirstOrDefault(x => x.Id == ObjectId.Parse(companyId));

                    if (company == null) throw new Exception("Quote could not be found");

                    var attachment = FileAttachmentsVulcan.GetAllAttachmentsForDocument(company)
                        .SingleOrDefault(x => x.Id == ObjectId.Parse(attachmentId));

                    if (attachment == null) throw new Exception("Attachment not found");

                    var fileName = Path.GetFileName(attachment.Filename);
                    var memory = FileAttachmentsVulcan.DownloadAsStream(attachment);
                    memory.Position = 0;

                    byte[] fileData = ReadFully(memory);

                    result.FileName = fileName;
                    result.MimeType = GetContentType(fileName);
                    result.FileInfo = new FileAttachmentModel(attachment);
                    result.FileData = fileData;
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



        private void RemoveFile(BaseDocument document, string attachmentId)
        {
            var attachedFiles = FileAttachmentsVulcan.GetAllAttachmentsForDocument(document) ?? new List<GridFSFileInfo>();

            var fileToRemove = attachedFiles.FirstOrDefault(x => x.Id == ObjectId.Parse(attachmentId));
            if (fileToRemove == null)
            {
                throw new Exception("No matching attachment found");
            }

            FileAttachmentsVulcan.Remove(fileToRemove.Id);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("fileAttachment/EmailQuoteDocument/{application}/{userId}/{quoteId}/{attachmentId}")]
        public async Task<JsonResult> EmailQuoteDocument(string application, string userId, string quoteId, string attachmentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var rep = new RepositoryBase<CrmQuote>();

                    var quote = rep.AsQueryable().FirstOrDefault(x => x.Id == ObjectId.Parse(quoteId));

                    if (quote == null) throw new Exception("Quote could not be found");

                    var attachment = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quote)
                        .SingleOrDefault(x => x.Id == ObjectId.Parse(attachmentId));

                    if (attachment == null) throw new Exception("Attachment not found");

                    var fileName = Path.GetFileName(attachment.Filename);
                    var memory = FileAttachmentsVulcan.DownloadAsStream(attachment);
                    memory.Position = 0;

                    var crmUser = GetCrmUser(application, userId);
                    var user = crmUser.User.AsUser();
                    var userEmail = user.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business);

                    if (userEmail == null) throw new Exception("SalesPerson (Business) Email is not configured");

                    var emailRecipients = new List<string> { userEmail.Address };


                    var body = "";
                    if (quote.CurrentRevision != null)
                    {
                        body = quote.CurrentRevision.RevisionNotesForPdf;
                    }
                    Stream fileStream = memory;

                    SendEMail.Execute($"VulcanCRM Quote: {quote.QuoteId} Attached Document: {fileName}", body, emailRecipients, "VulcanCRM@howcogroup.com", fileName, fileStream);

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

        #endregion

        #region Quote Item Attachments

        [AllowAnonymous]
        [HttpGet]
        [Route("fileAttachment/SaveOsirisDocumentsToQuoteItem/{application}/{userId}/{quoteItemId}")]
        public async Task<JsonResult> SaveOsirisDocumentsToQuoteItem(string application, string userId, string quoteItemId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var rep = new RepositoryBase<CrmQuoteItem>();

                    var quoteItem = rep.AsQueryable().FirstOrDefault(x => x.Id == ObjectId.Parse(quoteItemId));

                    if (quoteItem == null) throw new Exception("Quote Item could not be found");

                    var osirisDocs = GetOsirisDocumentListForQuoteItem(quoteItem);
                    foreach (var osirisDocInfo in osirisDocs)
                    {
                        AttachOsirisDocumentToQuoteItem(application, userId, osirisDocInfo, quoteItem);
                    }

                    var attachedFiles = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quoteItem) ?? new List<GridFSFileInfo>();
                    result.AttachedFiles = attachedFiles.Select(x => new FileAttachmentModel(x)).ToList();
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
        [Route("fileAttachment/DownloadAttachmentBytesForQuoteItem/{application}/{userId}/{quoteItemId}/{attachmentId}")]
        public async Task<JsonResult> DownloadAttachmentBytesForQuoteItem(string application, string userId, string quoteItemId, string attachmentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var rep = new RepositoryBase<CrmQuoteItem>();

                    var quoteItem = rep.AsQueryable().FirstOrDefault(x => x.Id == ObjectId.Parse(quoteItemId));

                    if (quoteItem == null) throw new Exception("Quote could not be found");

                    var attachment = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quoteItem)
                        .SingleOrDefault(x => x.Id == ObjectId.Parse(attachmentId));

                    if (attachment == null) throw new Exception("Attachment not found");

                    var fileName = Path.GetFileName(attachment.Filename);
                    var memory = FileAttachmentsVulcan.DownloadAsStream(attachment);
                    memory.Position = 0;

                    byte[] fileData = ReadFully(memory);
                    result.FileName = fileName;
                    result.MimeType = GetContentType(fileName);
                    result.FileInfo = new FileAttachmentModel(attachment);
                    result.FileData = fileData;

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
        [Route("fileAttachment/DownloadAttachmentForQuoteItem/{application}/{userId}/{quoteItemId}/{attachmentId}")]
        public IActionResult DownloadAttachmentForQuoteItem(string application, string userId, string quoteItemId, string attachmentId)
        {
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                var quoteItem = _helperQuote.GetQuoteItem(quoteItemId);
                if (quoteItem == null) throw new Exception("Action could not be found");

                var attachment = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quoteItem)
                    .SingleOrDefault(x => x.Id == ObjectId.Parse(attachmentId));

                if (attachment == null) throw new Exception("Attachment not found");

                var fileName = Path.GetFileName(attachment.Filename);
                var memory = FileAttachmentsVulcan.DownloadAsStream(attachment);
                memory.Position = 0;
                return File(memory, GetContentType(fileName), fileName);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("fileAttachment/RemoveAttachmentForQuoteItem/{application}/{userId}/{quoteItemId}/{attachmentId}")]
        public async Task<JsonResult> RemoveAttachmentForQuoteItem(string application, string userId, string quoteItemId, string attachmentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperApplication.VerifyApplication(application);

                    var quoteItem = _helperQuote.GetQuoteItem(quoteItemId);
                    if (quoteItem == null) throw new Exception("QuoteItem not found");

                    var attachedFiles = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quoteItem) ?? new List<GridFSFileInfo>();

                    var fileToRemove = attachedFiles.FirstOrDefault(x => x.Id == ObjectId.Parse(attachmentId));
                    if (fileToRemove == null)
                    {
                        throw new Exception("No matching attachment found");
                    }

                    FileAttachmentsVulcan.Remove(fileToRemove.Id);
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
        [Route("fileAttachment/EmailQuoteItemDocument/{application}/{userId}/{quoteItemId}/{attachmentId}")]
        public async Task<JsonResult> EmailQuoteItemDocument(string application, string userId, string quoteItemId, string attachmentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var rep = new RepositoryBase<CrmQuoteItem>();

                    var quoteItem = rep.AsQueryable().FirstOrDefault(x => x.Id == ObjectId.Parse(quoteItemId));

                    if (quoteItem == null) throw new Exception("Quote could not be found");

                    var attachment = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quoteItem)
                        .SingleOrDefault(x => x.Id == ObjectId.Parse(attachmentId));

                    if (attachment == null) throw new Exception("Attachment not found");

                    var fileName = Path.GetFileName(attachment.Filename);
                    var memory = FileAttachmentsVulcan.DownloadAsStream(attachment);
                    memory.Position = 0;

                    var crmUser = GetCrmUser(application, userId);
                    var user = crmUser.User.AsUser();
                    var userEmail = user.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business);

                    if (userEmail == null) throw new Exception("SalesPerson (Business) Email is not configured");

                    var emailRecipients = new List<string> { userEmail.Address };

                    var quote = quoteItem.GetQuote();

                    var body = "";
                    if (quote.CurrentRevision != null)
                    {
                        body = quote.CurrentRevision.RevisionNotesForPdf;
                    }
                    Stream fileStream = memory;

                    SendEMail.Execute($"VulcanCRM Quote: {quote.QuoteId} Item#: {quoteItem.Index} Attached Document: {fileName}", body, emailRecipients, "VulcanCRM@howcogroup.com", fileName, fileStream);

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
        [Route("fileAttachment/UploadAttachmentToQuoteItem/{application}/{userId}/{quoteItemId}")]
        public ActionResult UploadAttachmentToQuoteItem(string application, string userId, string quoteItemId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                _helperApplication.VerifyApplication(application);

                var crmUser = GetCrmUser(application, userId);
                var quoteItem = _helperQuote.GetQuoteItem(quoteItemId);
                if (quoteItem == null) throw new Exception("QuoteItem not found");

                var file = HttpContext.Request.Form.Files[0];
                if (file == null)
                {
                    throw new Exception("No file sent to Upload");
                }

                var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var fileAttachmentId = FileAttachmentsVulcan.SaveFileAttachment(
                    fileBytes, file.FileName, quoteItem, FileAttachmentType.QuoteItem, crmUser.User.Id);

                var attachedFiles = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quoteItem) ?? new List<GridFSFileInfo>();
                result.AttachedFiles = attachedFiles.Select(x => new FileAttachmentModel(x)).ToList();

                result.Success = true;
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, statusCode);

        }

        #endregion

        #region Private Methods


        private List<OsirisDocInfo> GetOsirisDocumentListForQuoteItem(CrmQuoteItem quoteItem)
        {
            var result = new List<OsirisDocInfo>();
            try
            {

                var coid = quoteItem.Coid;
                var tagNumber = quoteItem.CalculateQuotePriceModel.BaseCostStart.TagNumber;

                var repository = new OsirisRepository();
                result.AddRange(repository.GetOsirisDocumentList(coid, tagNumber, ""));

            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        private void AttachOsirisDocumentToQuoteItem(string application, string userId, OsirisDocInfo doc, CrmQuoteItem quoteItem)
        {
            try
            {
                var crmUser = GetCrmUser(application, userId);

                var attachedFiles = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quoteItem) ?? new List<GridFSFileInfo>();
                var existingFile = attachedFiles.SingleOrDefault(x => x.Filename == doc.FileName);

                var repository = new OsirisRepository();
                var coid = quoteItem.Coid;
                var tagNumber = quoteItem.CalculateQuotePriceModel.BaseCostStart.TagNumber;

                var document = repository.GetOsirisDocumentAsStream(coid, doc.ID);

                var fileBytes = document.ToArray();

                var fileAttachmentId = FileAttachmentsVulcan.SaveFileAttachmentForOsiris(
                    fileBytes, doc.FileName, quoteItem, FileAttachmentType.Osiris, crmUser.User.Id, doc.TypeName, doc.Description);

                if (existingFile != null)
                {
                    FileAttachmentsVulcan.Remove(existingFile.Id);
                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }


        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }



        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            var result = types.FirstOrDefault(x=>x.Key == ext).Value ?? "???";
            //if (result == "application/pdf")
            //{
            //    result = "application/octet-stream";
            //}

            return result;
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };

        }

        #endregion
    }
}
