using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BLL.EMail;
using DAL.Osiris;
using DAL.Quotes.Mongo.DocClass.FileAttachment;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.FileAttachment;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DateValues;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.MetalogicsCacheData;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.Queries;
using DAL.Vulcan.Mongo.Quotes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using RPT.HtmlTemplateLibrary;
using Vulcan.IMetal.Helpers;
using Vulcan.IMetal.Queries.Companies;
using Vulcan.IMetal.Queries.StockItems;
using Vulcan.iMetal.Quote.Export.Model;
using Vulcan.WebApi2.Hubs;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces(@"application/json", @"application/pdf")]
    //[Consumes("application/json")]
    public class QuoteController : BaseController
    {
        private readonly List<ResourceType> _heatTreatTypes = new List<ResourceType>
        {
            ResourceType.HeatTreat
        };

        private readonly IHelperCompany _helperCompany;
        private readonly IHelperCompanyPaymentTerms _helperCompanyPaymentTerms;

        private readonly IHelperContact _helperContact;

        //private readonly IProductMasterCache _productMasterCache;
        private readonly IHelperFile _helperFile;
        private readonly IHelperQuote _helperQuote;
        private readonly IHelperTeam _helperTeam;


        private readonly List<ResourceType> _machineResourceTypes = new List<ResourceType>
        {
            ResourceType.Assembly,
            ResourceType.Bore,
            ResourceType.CncMill,
            ResourceType.CncProgram,
            ResourceType.CncLathe,
            ResourceType.Hone,
            ResourceType.Lathe,
            ResourceType.Trepan
        };

        private readonly ITeamHub _teamHub;

        public QuoteController(
            IHelperQuote helperQuote,
            IHelperCompany helperCompany,
            IHelperUser helperUser,
            IHelperContact helperContact,
            //IProductMasterCache productMasterCache, 
            IHelperFile helperFile,
            ITeamHub teamHub,
            IHelperCompanyPaymentTerms helperCompanyPaymentTerms,
            IHelperTeam helperTeam) : base(helperUser)
        {
            _helperQuote = helperQuote;
            _helperCompany = helperCompany;
            _helperContact = helperContact;
            //_productMasterCache = productMasterCache;
            _helperFile = helperFile;
            _teamHub = teamHub;
            _helperCompanyPaymentTerms = helperCompanyPaymentTerms;
            _helperTeam = helperTeam;
        }

        #region Material Price History

        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/GetMaterialPriceHistory/{application}/{userId}/{coid}/{productId}/{displayCurrency}")]
        public JsonResult GetMaterialPriceHistory(string application, string userId, string coid, int productId,
            string displayCurrency)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                result.MaterialPriceHistory =
                    MaterialPriceHistory.GetMaterialPriceHistory(coid, productId, displayCurrency);
                result.Success = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        #endregion

        #region Status

        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/WonQuote/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> WonQuote(string application, string userId, string quoteId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                var crmUser = GetCrmUser(application, userId);

                CheckForReadonly(crmUser);

                var rep = new RepositoryBase<CrmQuote>();

                var crmQuote = rep.Find(quoteId);

                if (crmQuote.IsProspect)
                    throw new Exception("You must convert this Prospect into a iMetal Company first.");

                await Task.Run(async () =>
                {
                    crmQuote.Status = PipelineStatus.Won;
                    crmQuote.WonDate = DateTime.Now;

                    rep.Upsert(crmQuote);

                    result.QuoteModel = new QuoteModel(application, crmUser.UserId, crmQuote);
                    result.QuoteMiniModel = new QuoteMiniModel(crmQuote, PipelineStatus.Won);

                    await _teamHub.SendWonQuoteMessageFromUser(application, crmUser.UserId, quoteId);
                });


                result.Success = true;
            }
            catch (Exception ex)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/GetQuoteMiniModel/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> GetQuoteMiniModel(string application, string userId, string quoteId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                await Task.Run(() =>
                {
                    var quote = _helperQuote.GetQuote(quoteId);
                    result.QuoteMiniModel = new QuoteMiniModel(quote, quote.Status);
                    result.Success = true;
                });
            }
            catch (Exception ex)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/PrepareToSendQuoteToIMetal/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> PrepareToSendQuoteToIMetal(string application, string userId, string quoteId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                var crmUser = GetCrmUser(application, userId);

                CheckForReadonly(crmUser);

                var rep = new RepositoryBase<CrmQuote>();

                var crmQuote = rep.Find(quoteId);
                var company = crmQuote.Company.AsCompany();
                CompanyResolver.SaveNewAddressIfNecessary(company, crmQuote.ShipToAddress);

                Validate();

                await Task.Run(() =>
                {
                    //rep.Upsert(crmQuote);

                    result.AllCompanyAddresses = company.Addresses;

                    result.QuoteModel = new QuoteModel(application, crmUser.UserId, crmQuote);
                    result.QuoteMiniModel = new QuoteMiniModel(crmQuote, PipelineStatus.Won);

                    result.Success = true;
                });

                void Validate()
                {
                    var quoteItems = crmQuote.Items.Select(x => x.AsQuoteItem()).ToList();

                    var ignoredItems = quoteItems.Count(x => x.IsCrozCalc || x.IsQuickQuoteItem || x.IsMachinedPart);
                    var totalItems = quoteItems.Count();

                    if (totalItems - ignoredItems == 0)
                        throw new Exception(
                            "No Quote Items were found that can be exported. Quick Quotes and Machined items cannot be exported");


                    if (string.IsNullOrEmpty(crmQuote.PoNumber)) throw new Exception("Missing PO#");

                    if (string.IsNullOrEmpty(crmQuote.SalesGroupCode))
                        throw new Exception("Missing SalesGroupQuery Code");

                    if (crmQuote.IsProspect)
                        throw new Exception("You must convert this Prospect into a iMetal Company first.");

                    if (crmQuote.Status != PipelineStatus.Won)
                        throw new Exception("You must first recognize this Quote as Won");

                    if (crmQuote.ShipToAddress == null /* || !crmQuote.ShipToAddress.IsValid()*/)
                        throw new Exception("You must specify a Ship To Address");


                    if (crmQuote.ShipToAddress == null || string.IsNullOrEmpty(crmQuote.ShipToAddress.ExternalCode))
                        throw new Exception(
                            "Invalid Shipping Address specified. You must choose a valid iMetal Address before Exporting.");

                    var iMetalSubAddresses = company.GetAllAddressesFromIMetal();
                    if (iMetalSubAddresses.All(x => x.ExternalCode != crmQuote.ShipToAddress.ExternalCode))
                        throw new Exception(
                            "Invalid Shipping Address specified. You must choose a valid iMetal Address before Exporting.");
                }
            }
            catch (Exception ex)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/SendQuoteToIMetal/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> SendQuoteToIMetal(string application, string userId, string quoteId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                var crmUser = GetCrmUser(application, userId);
                CheckForReadonly(crmUser);

                var rep = new RepositoryBase<CrmQuote>();

                var crmQuote = rep.Find(quoteId);

                Validate();

                await Task.Run(() =>
                {
                    crmQuote.ExportStatus = crmQuote.ExportStatus == ExportStatus.Failed
                        ? ExportStatus.Retry
                        : ExportStatus.Pending;
                    crmQuote.ExportRequestedBy = crmUser.AsCrmUserRef();
                    rep.Upsert(crmQuote);

                    result.QuoteModel = new QuoteModel(application, crmUser.UserId, crmQuote);
                    result.QuoteMiniModel = new QuoteMiniModel(crmQuote, PipelineStatus.Won);

                    result.Success = true;
                });

                void Validate()
                {
                    if (string.IsNullOrEmpty(crmQuote.PoNumber)) throw new Exception("Missing PO#");

                    if (string.IsNullOrEmpty(crmQuote.SalesGroupCode))
                        throw new Exception("Missing SalesGroupQuery Code");

                    if (crmQuote.IsProspect)
                        throw new Exception("You must convert this Prospect into a iMetal Company first.");

                    if (crmQuote.Status != PipelineStatus.Won)
                        throw new Exception("You must first recognize this Quote as Won");

                    if (crmQuote.ShipToAddress == null /* || !crmQuote.ShipToAddress.IsValid()*/)
                        throw new Exception("You must specify a Ship To Address");


                    var quoteItems = crmQuote.Items.Select(x => x.AsQuoteItem()).ToList();
                    var hasMadeUpCost = quoteItems.Any(x => x.QuoteSource == QuoteSource.MadeUpCost);

                    if (hasMadeUpCost)
                        throw new Exception(
                            "This quote has [New Product(s)] and cannot be exported because iMetal requires Products to be created before importing.");
                }
            }
            catch (Exception ex)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/GetIMetalExportErrorInfo/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> GetIMetalExportErrorInfo(string application, string userId, string quoteId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);

                await Task.Run(() =>
                {
                    var rep = new RepositoryBase<CrmQuote>();
                    var crmQuote = rep.Find(quoteId);
                    result.QuoteExportAttemptResultsModel = new QuoteExportAttempResultsModel(crmQuote);

                    result.Success = true;
                });
            }
            catch (Exception ex)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/GetNewLostQuoteModel/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> GetNewLostQuoteModel(string application, string userId, string quoteId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                var crmUser = GetCrmUser(application, userId);
                CheckForReadonly(crmUser);

                ThrowExceptionForBadToken(statusCode);

                await Task.Run(() =>
                {
                    var lostQuoteModelValues = _helperQuote.GetNewLostQuoteModel(application, userId, quoteId);
                    result.LostQuoteModel = lostQuoteModelValues.LostQuoteModel;
                    result.Competitors = lostQuoteModelValues.Competitors.ToList();
                    result.LostReasons = lostQuoteModelValues.LostReasons.ToList();

                    result.Success = true;
                });
            }
            catch (Exception ex)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("quotes/LostQuote")]
        public async Task<JsonResult> LostQuote([FromBody] LostQuoteModel model)
        {
            dynamic result = new ExpandoObject();

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    var crmUser = GetCrmUser(model.Application, model.UserId);
                    CheckForReadonly(crmUser);

                    result.QuoteModel = _helperQuote.LostQuote(model);
                    ThrowExceptionForBadToken(statusCode);

                    result.Success = true;
                });
            }
            catch (Exception ex)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/BackToDraft/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> BackToDraft(string application, string userId, string quoteId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                var rep = new RepositoryBase<CrmQuote>();

                var crmQuote = rep.Find(quoteId);
                var crmUser = GetCrmUser(application, userId);

                CheckForReadonly(crmUser);

                await Task.Run(() =>
                {
                    crmQuote.Status = PipelineStatus.Draft;
                    crmQuote.LostDate = null;
                    crmQuote.LostTo = null;
                    crmQuote.LostReasonId = string.Empty;
                    crmQuote.WonDate = null;
                    crmQuote.SubmitDate = null;

                    foreach (var crmQuoteItemRef in crmQuote.Items.ToList())
                    {
                        var quoteItem = crmQuoteItemRef.AsQuoteItem();

                        quoteItem.LostReasonId = string.Empty;
                        quoteItem.LostDate = null;
                        quoteItem.LostTo = null;
                        quoteItem.LostProductCode = null;
                        quoteItem.SaveToDatabase();

                        var onQuoteItem = crmQuote.Items.IndexOf(crmQuoteItemRef);
                        crmQuote.Items[onQuoteItem] = quoteItem.AsQuoteItemRef();
                    }

                    rep.Upsert(crmQuote);

                    result.QuoteModel = new QuoteModel(application, crmUser.UserId, crmQuote);
                    result.QuoteMiniModel = new QuoteMiniModel(crmQuote, PipelineStatus.Loss);

                    result.Success = true;
                });
            }
            catch (Exception ex)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/SubmitQuote/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> SubmitQuote(string application, string userId, string quoteId)
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
                    CheckForReadonly(crmUser);
                    var quote = new RepositoryBase<CrmQuote>().Find(quoteId);

                    if (quote == null) throw new Exception("Quote was not found");

                    if (quote.Contact == null) throw new Exception("This Quote is missing a Contact");

                    string contactName;
                    string contactEmail;
                    string contactPhone;
                    var contact = _helperContact.GetContact(quote.Contact.Id);
                    if (contact != null)
                    {
                        contactName = contact.Person.FirstName + " " + contact.Person.LastName;
                        contactEmail = contact.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business)
                            ?.Address ?? "";
                        contactPhone = contact.Person.PhoneNumbers.FirstOrDefault(x => x.Type == PhoneType.Office)
                            ?.Number ?? "";
                    }
                    else
                    {
                        contactName = "<None>";
                        contactEmail = "<None>";
                        contactPhone = "<None>";
                    }

                    //if (String.IsNullOrEmpty(contactEmail)) throw new Exception("Contact does not have a (Business) Email address");

                    quote.Status = PipelineStatus.Submitted;
                    quote.SubmitDate = DateTime.Now;
                    quote.SaveToDatabase();

                    var fileName = GenerateQuotePdf(quote, contactName, contactEmail, contactPhone, out var fileStream);

                    var salesPerson = quote.SalesPerson.AsCrmUser();
                    var ldapUser = salesPerson.User.AsUser();
                    var salesPersonEmail =
                        ldapUser.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business);

                    if (salesPersonEmail == null) throw new Exception("SalesPerson (Business) Email is not configured");

                    var emailRecipients = new List<string> {salesPersonEmail.Address};
                    //emailRecipients.Add("isidro.gallego@howcogroup.com");
                    //emailRecipients.Add("marc.pike@howcogroup.com");
                    //emailRecipients.Add(contactEmail);

                    var body = "";
                    if (quote.CurrentRevision != null) body = quote.CurrentRevision.RevisionNotesForPdf;


                    SendEMail.Execute($"VulcanCRM Quote: {quote.QuoteId} for {contactName}", body, emailRecipients,
                        "VulcanCRM@howcogroup.com", fileName, fileStream);

                    fileStream.Dispose();

                    result.QuoteModel = new QuoteModel(application, salesPerson.UserId, quote);

                    result.Success = true;
                });
            }
            catch (Exception ex)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                var critical = ex.Message.ToUpper().Contains("MEMORY");
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, ex, critical, critical);

                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        #endregion

        #region Revisions

        [HttpGet]
        [Route("quotes/GetNewRevisionModel/{application}/{userId}/{quoteId}")]
        [AllowAnonymous]
        public async Task<JsonResult> GetNewRevisionModel(string application, string userId, string quoteId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(application, userId);
                    var quote = _helperQuote.GetQuote(quoteId);
                    result.QuoteRevisionModel = new QuoteRevisionModel(application, crmUser.UserId, quote);
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
        [Route("quotes/GetCrozCalcModel/{quoteId}/{coid}/{companyId}/{prospectId}/{displayCurrency}/{productCode}")]
        [AllowAnonymous]
        public async Task<JsonResult> GetCrozCalcModel(int quoteId, string coid, string companyId, string prospectId,
            string displayCurrency, string productCode)
        {
            if (string.IsNullOrEmpty(companyId) || companyId == "none") companyId = ObjectId.Empty.ToString();
            if (string.IsNullOrEmpty(prospectId) || prospectId == "none") prospectId = ObjectId.Empty.ToString();

            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            try
            {
                await Task.Run(() =>
                {
                    result.CrozCalcModel =
                        new CrozQuoteModel(quoteId, coid, companyId, prospectId, displayCurrency, productCode);
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
        [Route("quotes/CreateQuoteRevision")]
        [AllowAnonymous]
        public async Task<JsonResult> CreateQuoteRevision([FromBody] QuoteRevisionModel model)
        {
            dynamic result = new ExpandoObject();

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var quote = _helperQuote.GetQuote(model.QuoteId);
                    var nextRevisionId = 1;
                    if (quote.Revisions.Any()) nextRevisionId = quote.Revisions.Max(x => x.Id) + 1;
                    var salesPerson = GetCrmUser(model.Application, model.UserId).AsCrmUserRef();
                    var revision = new Revision
                    {
                        Id = nextRevisionId,
                        SalesPerson = salesPerson,
                        RevisionDate = DateTime.Now,
                        RevisionNotesForPdf = model.RevisionNotesForPdf
                    };
                    quote.Revisions.Add(revision);
                    quote.SetReportDate();
                    quote.SaveToDatabase();
                    result.QuoteModel = new QuoteModel(model.Application, model.UserId, quote);
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
        [Route("quotes/EmailLatestQuoteRevision/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> EmailLatestQuoteRevision(string application, string userId, string quoteId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var quote = new RepositoryBase<CrmQuote>().Find(quoteId);

                    if (quote == null) throw new Exception("Quote was not found");

                    if (quote.Contact == null) throw new Exception("This Quote is missing a Contact");

                    var requestByCrmUser = GetCrmUser(application, userId);
                    var requestByUser = requestByCrmUser.User.AsUser();
                    var requestByEmail =
                        requestByUser.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business);

                    if (requestByEmail == null)
                        throw new Exception(
                            $"No (Business) Email has been configured for {requestByUser.FirstName} {requestByUser.LastName}");


                    string contactName;
                    string contactEmail;
                    string contactPhone;
                    var contact = _helperContact.GetContact(quote.Contact.Id);
                    if (contact != null)
                    {
                        contactName = contact.Person.FirstName + " " + contact.Person.LastName;
                        contactEmail = contact.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business)
                            ?.Address ?? "";
                        contactPhone = contact.Person.PhoneNumbers.FirstOrDefault(x => x.Type == PhoneType.Office)
                            ?.Number ?? "";
                    }
                    else
                    {
                        contactName = "<None>";
                        contactEmail = "<None>";
                        contactPhone = "<None>";
                    }

                    var fileName = GenerateQuotePdf(quote, contactName, contactEmail, contactPhone, out var fileStream);


                    //var salesPerson = quote.SalesPerson.AsCrmUser();
                    //var user = salesPerson.User.AsUser();
                    //var salesPersonEmail = user.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business);

                    //if (salesPersonEmail == null) throw new Exception("SalesPerson (Business) Email is not configured");

                    var emailRecipients = new List<string>();
                    emailRecipients.Add(requestByEmail.Address);

                    var body = "";
                    if (quote.CurrentRevision != null) body = quote.CurrentRevision.RevisionNotesForPdf;


                    SendEMail.Execute(
                        $"VulcanCRM Quote: {quote.QuoteId} for {contact.Person.FirstName} {contact.Person.LastName}",
                        body, emailRecipients, "VulcanCRM@howcogroup.com", fileName, fileStream);

                    result.Success = true;
                });
            }
            catch (Exception ex)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                var critical = ex.Message.ToUpper().Contains("MEMORY");

                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, ex, critical, critical);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        private static string GenerateQuotePdf(CrmQuote quote, string contactName, string contactEmail,
            string contactPhone,
            out Stream fileStream)
        {
            var fileName = $"{quote.QuoteId}-Rev-{quote.RevisionNumber}.pdf";
            using (var htmlGenerator =
                CrmQuoteHtmlGenerator.GetQuoteHtmlGenerator(quote, contactName, contactEmail, contactPhone))
            {
                try
                {
                    var memory = htmlGenerator.GetAsPdfDocument();
                    memory.Position = 0;
                    //var file = File(memory, _helperFile.GetContentType(fileName), fileName);
                    fileStream = memory;
                    return fileName;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        #endregion

        #region Copies

        [HttpGet]
        [Route("quotes/GetNewQuoteCopyModel/{application}/{userId}/{quoteId}")]
        [AllowAnonymous]
        public async Task<JsonResult> GetNewQuoteCopyModel(string application, string userId, string quoteId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(application, userId);
                    CheckForReadonly(crmUser);

                    var quote = _helperQuote.GetQuote(quoteId);

                    result.CopyQuoteModel = new QuoteCopyModel
                    {
                        Application = application,
                        UserId = crmUser.UserId,
                        QuoteId = quoteId
                    };

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
        [Route("quotes/CopyQuoteItemToDifferentQuote/{application}/{userId}/{destinationQuoteId}/{sourceQuoteItemId}")]
        public async Task<JsonResult> CopyQuoteItemToDifferentQuote(string application, string userId,
            string destinationQuoteId,
            string sourceQuoteItemId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(application, userId);
                    CheckForReadonly(crmUser);
                    var quote = _helperQuote.CopyQuoteItemToDifferentQuote(application, crmUser.UserId,
                        destinationQuoteId, sourceQuoteItemId);
                    result.QuoteModel = new QuoteModel(application, crmUser.UserId, quote);
                    result.QuoteMiniModel = new QuoteMiniModel(quote, quote.Status);
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
        [Route("quotes/CopyQuote/{application}/{userId}/{quoteId}")]
        [AllowAnonymous]
        public async Task<JsonResult> CopyQuote(string application, string userId, string quoteId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(application, userId);
                    CheckForReadonly(crmUser);
                    var model = new QuoteCopyModel(application, crmUser.UserId, quoteId);
                    var quote = _helperQuote.CopyQuote(model, false);
                    result.QuoteModel = new QuoteModel(model.Application, model.UserId, quote);
                    result.QuoteMiniModel = new QuoteMiniModel(quote, PipelineStatus.Draft);
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
        [Route("quotes/CreateLinkedQuote/{application}/{userId}/{sourceQuoteId}/{forCompanyId}")]
        [AllowAnonymous]
        public async Task<JsonResult> CreateLinkedQuote(string application, string userId, string sourceQuoteId,
            string forCompanyId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(application, userId);
                    CheckForReadonly(crmUser);
                    var quote = _helperQuote.CreateLinkedQuote(application, userId, sourceQuoteId, forCompanyId);
                    result.QuoteModel = new QuoteModel(application, userId, quote);
                    result.QuoteMiniModel = new QuoteMiniModel(quote, PipelineStatus.Draft);
                    result.PaymentTermList = PaymentTerms.GetPaymentTermsForCoid(quote.Coid);
                    result.FreightTermList = FreightTerms.GetFreightTermsForCoid(quote.Coid);

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
        [Route("quotes/CopyQuoteItems")]
        [AllowAnonymous]
        public async Task<JsonResult> CopyQuoteItems([FromBody] QuoteCopyModel model)
        {
            dynamic result = new ExpandoObject();

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(model.Application, model.UserId);
                    CheckForReadonly(crmUser);
                    var quote = _helperQuote.CopyQuote(model, false);
                    result.QuoteModel = new QuoteModel(model.Application, crmUser.UserId, quote);
                    result.QuoteMiniModel = new QuoteMiniModel(quote, PipelineStatus.Draft);
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
        [Route("quotes/MoveQuoteItemsToNewQuote")]
        [AllowAnonymous]
        public async Task<JsonResult> MoveQuoteItemsToNewQuote([FromBody] QuoteCopyModel model)
        {
            dynamic result = new ExpandoObject();

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }


            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                var crmUser = GetCrmUser(model.Application, model.UserId);
                CheckForReadonly(crmUser);
                ThrowExceptionForBadToken(statusCode);
                await Task.Run(() =>
                {
                    var quote = _helperQuote.MoveQuoteItemsToNewQuote(model);
                    result.QuoteModel = new QuoteModel(model.Application, crmUser.UserId, quote);
                    result.QuoteMiniModel = new QuoteMiniModel(quote, PipelineStatus.Draft);
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
        [Route("quotes/CopyQuoteItemsToSameQuote")]
        [AllowAnonymous]
        public async Task<JsonResult> CopyQuoteItemsToSameQuote([FromBody] QuoteCopyModel model)
        {
            dynamic result = new ExpandoObject();

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                var crmUser = GetCrmUser(model.Application, model.UserId);
                CheckForReadonly(crmUser);
                ThrowExceptionForBadToken(statusCode);

                await Task.Run(() =>
                {
                    var quote = _helperQuote.CopyQuote(model, true);
                    result.QuoteModel = new QuoteModel(model.Application, crmUser.UserId, quote);
                    result.QuoteMiniModel = new QuoteMiniModel(quote, quote.Status);
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

        #region Move Quote

        [HttpGet]
        [Route("quotes/GetTeamMoveOptions/{application}/{userId}")]
        [AllowAnonymous]
        public async Task<JsonResult> GetTeamMoveOptions(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.TeamMoveOptions = new QuoteTeamMoveOptionsModel();
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
        [Route(
            "quotes/MoveQuoteToNewTeam/{application}/{userId}/{quoteId}/{destinationTeamId}/{salesPersonId}/{companyId}")]
        [AllowAnonymous]
        public async Task<JsonResult> MoveQuoteToNewTeam(string application, string userId, string quoteId,
            string destinationTeamId, string salesPersonId, string companyId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(application, userId);
                    CheckForReadonly(crmUser);
                    var destinationTeam = _helperTeam.GetTeam(destinationTeamId);
                    var salesPerson = GetCrmUser(application, salesPersonId);

                    if (destinationTeam.CrmUsers.All(x => x.UserId != salesPerson.UserId))
                        throw new Exception(
                            $"{salesPerson.User.GetFullName()} is not a member of Team: {destinationTeam.Name}");

                    var quote = _helperQuote.GetQuote(quoteId);
                    var originalTeam = quote.Team;

                    quote = _helperQuote.MoveQuote(quote, destinationTeam, salesPerson, companyId);

                    result.QuoteModel = new QuoteModel(application, crmUser.UserId, quote);
                    result.QuoteMiniModel = new QuoteMiniModel(quote, quote.Status);

                    var subject =
                        $"{crmUser.User.GetFullName()} moved a Quote#: {quote.QuoteId} for Company: {quote.Company.Code}-{quote.Company.Name} and assigned it to you!";
                    var server = EnvironmentSettings.GetWebAddress();
                    var body = new StringBuilder();

                    body.AppendLine($"Originally this Quote was in Team: {originalTeam.Name}");
                    body.AppendLine();
                    body.AppendLine(
                        $"<a href='{server}/home/dashboard/quotes/quote?id={quote.Id}' target='_blank'>Click here to View Quote</a>");

                    var emailAddress = salesPerson.User.AsUser().Person.EmailAddresses
                        .First(x => x.Type == EmailType.Business);
                    var emailRecipients = new List<string> {emailAddress.Address};

                    SendEMail.Execute(subject, body.ToString(), emailRecipients, "VulcanCRM@howcogroup.com", true);
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

        #region Pdf Publish

        [HttpGet]
        [Route("quotes/GetHtmlForQuote/{application}/{userId}/{quoteId}/{contactId}")]
        public ActionResult PrintQuote(string application, string userId, string quoteId, string contactId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);

                var rep = new RepositoryBase<CrmQuote>();
                //var id = ObjectId.Parse(quoteId);
                var crmQuote = rep.Find(quoteId);

                var contact = _helperContact.GetContact(contactId);

                var contactName = contact.Person.FirstName + contact.Person.LastName;

                var contactEmail = contact.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business)
                    ?.Address ?? "";
                var contactPhone =
                    contact.Person.PhoneNumbers.FirstOrDefault(x => x.Type == PhoneType.Office)?.Number ?? "";

                //var response = new HttpResponseMessage();
                var htmlGenerator =
                    CrmQuoteHtmlGenerator.GetQuoteHtmlGenerator(crmQuote, contactName, contactEmail, contactPhone);
                return Content(htmlGenerator.Html);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        [HttpGet]
        [Produces("application/pdf")]
        [Route("quotes/GetPdfForQuote/{application}/{userId}/{quoteId}/{contactId}")]
        public IActionResult GetPdfForQuote(string application, string userId, string quoteId, string contactId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                var rep = new RepositoryBase<CrmQuote>();

                var crmQuote = rep.Find(quoteId);

                string contactName;
                string contactEmail;
                string contactPhone;
                var contact = _helperContact.GetContact(contactId);
                if (contact != null)
                {
                    contactName = contact.Person.FirstName + " " + contact.Person.LastName;
                    contactEmail = contact.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business)
                        ?.Address ?? "";
                    contactPhone =
                        contact.Person.PhoneNumbers.FirstOrDefault(x => x.Type == PhoneType.Office)?.Number ?? "";
                }
                else
                {
                    contactName = "<None>";
                    contactEmail = "<None>";
                    contactPhone = "<None>";
                }

                var htmlGenerator =
                    CrmQuoteHtmlGenerator.GetQuoteHtmlGenerator(crmQuote, contactName, contactEmail, contactPhone);

                var fileName = $"{crmQuote.QuoteId}-Rev-{crmQuote.CurrentRevision?.Id ?? 0}.pdf";
                var memory = htmlGenerator.GetAsPdfDocument();
                memory.Position = 0;

                return File(memory, _helperFile.GetContentType(fileName), fileName);
            }
            catch (Exception ex)
            {
                var critical = ex.Message.ToUpper().Contains("MEMORY");
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, ex, critical, critical);

                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        [HttpGet]
        [Route("quotes/GetPdfForQuoteAsBytes/{application}/{userId}/{quoteId}/{contactId}")]
        public async Task<JsonResult> GetPdfForQuoteAsBytes(string application, string userId, string quoteId,
            string contactId)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var rep = new RepositoryBase<CrmQuote>();
                    var crmQuote = rep.Find(quoteId);

                    string contactName;
                    string contactEmail;
                    string contactPhone;
                    var contact = _helperContact.GetContact(contactId);
                    if (contact != null)
                    {
                        contactName = contact.Person.FirstName + " " + contact.Person.LastName;
                        contactEmail = contact.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business)
                            ?.Address ?? "";
                        contactPhone = contact.Person.PhoneNumbers.FirstOrDefault(x => x.Type == PhoneType.Office)
                            ?.Number ?? "";
                    }
                    else
                    {
                        contactName = "<None>";
                        contactEmail = "<None>";
                        contactPhone = "<None>";
                    }

                    var htmlGenerator =
                        CrmQuoteHtmlGenerator.GetQuoteHtmlGenerator(crmQuote, contactName, contactEmail, contactPhone);

                    var fileName = $"{crmQuote.QuoteId}-Rev-{crmQuote.CurrentRevision?.Id ?? 0}.pdf";
                    var memory = htmlGenerator.GetAsPdfDocument();
                    memory.Position = 0;


                    var fileData = ReadFully(memory);

                    result.FileName = fileName;
                    result.MimeType = GetContentType(fileName);
                    result.FileData = fileData;
                    result.Success = true;
                });
            }
            catch (Exception ex)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                var critical = ex.Message.ToUpper().Contains("MEMORY");
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, ex, critical, critical);

                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        #endregion

        #region Functions

        private static void CheckForReadonly(CrmUser crmUser)
        {
            if (crmUser.ReadOnly)
                throw new Exception("You only have ReadOnly access. You cannot perform this operation.");
        }

        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0) ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            var result = types[ext];
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

        #region View Quotes

        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/GetQuotePipeline/{application}/{userId}/{dateRange}/{forTeam}")]
        public async Task<JsonResult> GetQuotePipeline(string application, string userId, string dateRange,
            bool forTeam)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var allDateValues = DateValueItem.GetDateValues();
                    var dateValues = allDateValues.SingleOrDefault(x => x.Name == dateRange);
                    if (dateValues == null) throw new Exception("Date range not supported");

                    var crmUser = GetCrmUser(application, userId);

                    result.QuotePipelineModel = _helperQuote.GetQuotesPipeline(application, crmUser.UserId,
                        dateValues.DateRange.BegDate, dateValues.DateRange.EndDate, forTeam);
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
        [Route("quotes/GetQuotePipelineUpgraded/{userId}/{beginDate}/{endDate}/{forTeam}/{showExpired}")]
        public async Task<JsonResult> GetQuotePipelineUpgraded(string userId, DateTime beginDate, DateTime endDate,
            bool forTeam, bool showExpired)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    endDate = endDate.AddDays(1).AddSeconds(-1);

                    var crmUser = GetCrmUser("vulcancrm", userId);

                    result.QuotePipelineModel =
                        new QuotesPipelineQuery(crmUser.UserId, beginDate, endDate, forTeam, showExpired);
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
        [Route("quotes/QuoteSearchByQuoteId/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> QuoteSearchByQuoteId(string application, string userId, int quoteId)
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
                    var quote = rep.AsQueryable().SingleOrDefault(x => x.QuoteId == quoteId);
                    if (quote == null) throw new Exception("Quote was not found");

                    result.Id = quote.Id.ToString();
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
        [Route("quotes/GetQuotePipelineForDates/{application}/{userId}/{beginDate}/{endDate}/{forTeam}")]
        public async Task<JsonResult> GetQuotePipelineForDates(string application, string userId, DateTime beginDate,
            DateTime endDate, bool forTeam)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    beginDate = beginDate.Date;
                    endDate = endDate.Date.AddDays(1).AddSeconds(-1);

                    if (endDate < beginDate) throw new Exception("Invalid date range specified");


                    var crmUser = GetCrmUser(application, userId);

                    result.QuotePipelineModel =
                        _helperQuote.GetQuotesPipeline(application, crmUser.UserId, beginDate, endDate, forTeam);
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
        [Route("quotes/GetQuotePipelineForContact/{application}/{userId}/{contactId}/{teamId}")]
        public async Task<JsonResult> GetQuotePipelineForContact(string application, string userId, string contactId,
            string teamId)
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

                    result.QuotePipelineModel =
                        _helperQuote.GetQuotesPipelineForContact(application, crmUser.UserId, contactId, teamId);
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
        [Route("quotes/GetQuotePipelineForCompany/{application}/{userId}/{companyId}")]
        public async Task<JsonResult> GetQuotePipelineForCompany(string application, string userId, string companyId)
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

                    result.QuotePipelineModel =
                        _helperQuote.GetQuotesPipelineForCompany(application, crmUser.UserId, companyId);
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

        #region Quote CRUD

        [HttpPost]
        [AllowAnonymous]
        [Route("quotes/GetPriceModelsForMadeUpCost")]
        public async Task<JsonResult> GetPriceModelsForMadeUpCost([FromBody] MadeUpCostModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var crmUser = GetCrmUser(model.Application, model.UserId);
                    CheckForReadonly(crmUser);

                    if (model.NonStockItemProduct != null)
                    {
                        model.InsideDiameter = model.NonStockItemProduct.InsideDiameter;
                        model.OuterDiameter = model.NonStockItemProduct.OuterDiameter;
                        model.MetalCategory = model.NonStockItemProduct.MetalCategory;
                        model.ProductCode = model.NonStockItemProduct.ProductCode;
                        model.ProductCondition = model.NonStockItemProduct.ProductCondition;
                        model.ProductType = model.NonStockItemProduct.ProductType;
                        model.TheoWeight = model.NonStockItemProduct.TheoWeight;
                        model.QuoteSource = QuoteSource.NonStockItem;
                    }
                    //if (!model.IsValid) throw new Exception("Madeup InternalCost model is not valid");

                    var models = _helperQuote.GetNewCalculateQuotePriceModelForMadeUpCost(model);
                    result.QuoteCalcOverridesSupported = CalculateQuotePriceModel.GetQuoteCalcOverridesSupported();
                    result.QuotePriceModel = models.QuotePriceModel;
                    result.CalculateQuotePriceModel = models.CalculateQuotePriceModel;
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
        [Route("quotes/GetNewPriceModelsForPurchaseOrderItem")]
        public async Task<JsonResult> GetNewPriceModelsForPurchaseOrderItem(
            [FromBody] QuotePurchaseOrderItemModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    if (model.CostPerPound == 0) throw new Exception("Missing CostPerPound in model");

                    var crmUser = GetCrmUser(model.Application, model.UserId);
                    CheckForReadonly(crmUser);

                    var models = _helperQuote.GetNewCalculateQuotePriceModelForPurchaseOrderItem(model.Coid,
                        model.CostPerPound, model.ProductId, model.OrderQuantity, model.Application, crmUser.UserId,
                        model.DisplayCurrency, model.CompanyId);

                    result.QuotePriceModel = models.QuotePriceModel;

                    result.CalculateQuotePriceModel = models.CalculateQuotePriceModel;
                    if (model.CompanyId != string.Empty)
                        result.QuoteCalcOverridesSupported = CalculateQuotePriceModel.GetQuoteCalcOverridesSupported();
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
        [Route("quotes/GetNewPriceModelsForStockItem")]
        public async Task<JsonResult> GetNewPriceModelsForStockItem([FromBody] QuoteStockItemModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    var crmUser = GetCrmUser(model.Application, model.UserId);
                    CheckForReadonly(crmUser);
                    if (model.StockItem.MetalType == "MACHINED COMPONENT")
                        throw new Exception("Machined Component not supported");

                    ThrowExceptionForBadToken(statusCode);
                    var models = _helperQuote.GetNewCalculateQuotePriceModelForStockItem(model);
                    result.CalculateQuotePriceModel = models.CalculateQuotePriceModel;
                    result.QuotePriceModel = models.QuotePriceModel;
                    result.QuoteCalcOverridesSupported = CalculateQuotePriceModel.GetQuoteCalcOverridesSupported();
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

        #region Machined Parts

        [HttpGet]
        [AllowAnonymous]
        [Route(
            "quotes/GetNewQuoteItemModelForMachinedPart/{application}/{userId}/{coid}/{stockItemId}/{displayCurrency}")]
        public async Task<JsonResult> GetNewQuoteGetNewQuoteItemModelForMachinedPart(string application, string userId,
            string coid, int stockItemId, string displayCurrency)
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

                    var quoteItem = new CrmQuoteItem
                    {
                        Coid = coid,
                        SalesPerson = crmUser.AsCrmUserRef(),
                        MachinedPartModel = new QuoteMachinedPartModel(application, crmUser.UserId, coid, stockItemId,
                            displayCurrency),
                        QuoteSource = QuoteSource.MachinedPart
                    };


                    var model = new QuoteItemModel(application, crmUser.UserId, quoteItem)
                    {
                        Application = application,
                        UserId = crmUser.UserId,
                        IsDirty = true
                    };

                    result.QuoteItemModel = model;
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
        [Route("quotes/CalculateMachinedPartModel")]
        public async Task<JsonResult> CalculateMachinedPartModel([FromBody] QuoteMachinedPartModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.MachinedPartModel = QuoteMachinedPartModel.Calculate(model);
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
        [Route("quotes/RecalculateQuoteItemModelForMachinedPart")]
        public async Task<JsonResult> RecalculateQuoteItemModelForMachinedPart([FromBody] QuoteItemModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    model.MachinedPartModel = QuoteMachinedPartModel.Calculate(model.MachinedPartModel);
                    model.ItemSummaryViewModel = new ItemSummaryViewModel(model);

                    result.QuoteItemModel = model;
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


        [HttpPost]
        [AllowAnonymous]
        [Route("quotes/CreateNewMadeupCostModels")]
        public async Task<JsonResult> CreateNewMadeupCostModels([FromBody] CreateNewMadeupCostModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var madeUpCost = model.AsMadeUpCost();
                    var otherModels = madeUpCost.ConvertToCostValues(model.OrderQuantity);
                    var crmUser = GetCrmUser(model.Application, model.UserId);
                    CheckForReadonly(crmUser);

                    result.ProductMaster = otherModels.ProductMaster;
                    result.BaseCost = otherModels.BaseCost;
                    var madeupCostModel = new MadeUpCostModel(model.AsMadeUpCost())
                    {
                        OrderQuantity = model.OrderQuantity,
                        Application = model.Application,
                        UserId = crmUser.UserId,
                        CompanyId = model.CompanyId,
                        QuoteSource = model.QuoteSource
                    };
                    result.MadeUpCostModel = madeupCostModel;

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
        [Route("quotes/RefreshCompanyAddressesForQuote/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> RefreshCompanyAddressesForQuote(string application, string userId, string quoteId)
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
                    CheckForReadonly(crmUser);
                    var quote = new RepositoryBase<CrmQuote>().Find(quoteId);
                    FixQuoteCompanyDuplicateIssue.Execute(quote);
                    result.QuoteModel = _helperQuote.GetQuoteModel(application, crmUser.UserId, quoteId);
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
        [Route("quotes/CreateNewQuoteForCompany/{application}/{userId}/{coid}/{companyId}")]
        public async Task<JsonResult> CreateNewQuoteForCompany(string application, string userId, string coid,
            string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var company = _helperCompany.GetCompanyRef(companyId).AsCompany();
                    var crmUser = GetCrmUser(application, userId);
                    var team = crmUser.ViewConfig.Team.AsTeam();
                    CheckForReadonly(crmUser);

                    var quoteModel =
                        _helperQuote.CreateNewQuoteForCompany(application, crmUser.UserId, coid, companyId);
                    quoteModel.SalesPerson = crmUser.AsCrmUserRef();
                    quoteModel.Contacts = company.Contacts;
                    if (quoteModel.DisplayCurrency == string.Empty) quoteModel.DisplayCurrency = team.DefaultCurrency;

                    var paymentTerms = _helperCompanyPaymentTerms.GetPaymentTermsForCompany(coid, company.SqlId);

                    if (paymentTerms != null) quoteModel.PaymentTerm = paymentTerms.Description;

                    result.QuoteModel = quoteModel;
                    result.PaymentTermList = PaymentTerms.GetPaymentTermsForCoid(coid);
                    result.FreightTermList = FreightTerms.GetFreightTermsForCoid(coid);

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
        [Route("quotes/CreateNewQuoteForProspect/{application}/{userId}/{coid}/{prospectId}")]
        public async Task<JsonResult> CreateNewQuoteForProspect(string application, string userId, string coid,
            string prospectId)
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
                    CheckForReadonly(crmUser);
                    var team = crmUser.ViewConfig.Team.AsTeam();
                    var quoteModel =
                        _helperQuote.CreateNewQuoteForProspect(application, crmUser.UserId, coid, prospectId);
                    quoteModel.DisplayCurrency = team.DefaultCurrency;

                    result.QuoteModel = quoteModel;

                    result.PaymentTermList = PaymentTerms.GetPaymentTermsForCoid(coid);
                    result.FreightTermList = FreightTerms.GetFreightTermsForCoid(coid);

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
        [Route("quotes/GetMadeUpCostModel/{application}/{userId}/{companyId}")]
        public async Task<JsonResult> GetMadeUpCostModel(string application, string userId, string companyId)
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
                    CheckForReadonly(crmUser);
                    result.MadeUpCostModel = new MadeUpCostModel
                    {
                        Application = application,
                        UserId = crmUser.UserId,
                        CompanyId = companyId
                    };
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
        [Route("quotes/GetCreateNewMadeupCostModel/{application}/{userId}/{companyId}")]
        public async Task<JsonResult> GetCreateNewMadeupCostModel(string application, string userId, string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    if (companyId == "prospect") companyId = string.Empty;
                    var crmUser = GetCrmUser(application, userId);
                    CheckForReadonly(crmUser);
                    var model = new CreateNewMadeupCostModel
                    {
                        Application = application,
                        UserId = crmUser.UserId,
                        CompanyId = companyId
                    };
                    result.CreateNewMadeupCostModel = model;
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
        [Route(
            "quotes/GetCreateNewMadeupCostModelForNonStockItem/{application}/{userId}/{companyId}/{coid}/{nonStockItemId}")]
        public async Task<JsonResult> GetCreateNewMadeupCostModelForNonStockItem(string application, string userId,
            string companyId, string coid, int nonStockItemId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    if (companyId == "prospect") companyId = string.Empty;
                    var crmUser = GetCrmUser(application, userId);
                    CheckForReadonly(crmUser);
                    var productMaster = new ProductMaster(coid, nonStockItemId);

                    var model = new CreateNewMadeupCostModel
                    {
                        Application = application,
                        UserId = crmUser.UserId,
                        CompanyId = companyId,
                        Coid = coid,
                        InsideDiameter = productMaster.InsideDiameter,
                        MetalCategory = productMaster.MetalCategory,
                        OuterDiameter = productMaster.OuterDiameter,
                        ProductCode = productMaster.ProductCode,
                        ProductCondition = productMaster.ProductCondition,
                        ProductIdForNonStockItem = nonStockItemId,
                        QuoteSource = QuoteSource.NonStockItem
                    };
                    result.CreateNewMadeupCostModel = model;

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
        [Route("quotes/GetQuotePurchaseOrderItemModel/{application}/{userId}/{companyId}")]
        public async Task<JsonResult> GetQuotePurchaseOrderItemModel(string application, string userId,
            string companyId)
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

                    var team = crmUser.ViewConfig.Team.AsTeam();

                    if (companyId == "prospect") companyId = string.Empty;

                    result.QuotePurchaseOrderItemModel = new QuotePurchaseOrderItemModel
                    {
                        Application = application,
                        UserId = crmUser.UserId,
                        CompanyId = companyId,
                        DisplayCurrency = team.DefaultCurrency
                    };
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
        [Route("quotes/GetQuoteStockItemModel/{application}/{userId}/{coid}/{stockItemId}/{companyId}")]
        public async Task<JsonResult> GetQuoteStockItemModel(string application, string userId, string coid,
            int stockItemId, string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var stockInfo = ProductMaster.FromStockId(coid, stockItemId);

                    if (companyId == "prospect") companyId = string.Empty;

                    var crmUser = GetCrmUser(application, userId);


                    result.QuoteStockItemModel = new QuoteStockItemModel
                    {
                        Coid = coid,
                        Application = application,
                        UserId = crmUser.UserId,
                        StartingProduct = stockInfo.ProductMaster,
                        FinishedProduct = stockInfo.ProductMaster,
                        StockItem = stockInfo.StockItem,
                        CompanyId = companyId
                    };
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
        [Route("quotes/GetNewQuoteItemModel/{application}/{userId}/{companyId}")]
        public async Task<JsonResult> GetNewQuoteItemModel(string application, string userId, string companyId)
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

                    CheckForReadonly(crmUser);

                    var team = crmUser.ViewConfig.Team.AsTeam();
                    var location = team.Location.AsLocation();

                    var coid = location.GetCoid();

                    var quoteItem = new CrmQuoteItem
                    {
                        Coid = coid,
                        SalesPerson = crmUser.AsCrmUserRef()
                    };

                    if (companyId != "prospect")
                    {
                        var company = _helperCompany.GetCompanyRef(companyId).AsCompany();
                        var companyDefaults =
                            CompanyDefaults.GetCompanyDefaults(company.Location.GetCoid(), company, false);
                        companyDefaults.ApplyCompanyDefaultsToQuoteItem(quoteItem);
                    }

                    var model = new QuoteItemModel(application, crmUser.UserId, quoteItem)
                    {
                        IsDirty = true
                    };

                    result.QuoteItemModel = model;
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
        [Route("quotes/GetNewQuoteItemModelForQuickQuoteItem/{application}/{userId}")]
        public async Task<JsonResult> GetNewQuoteItemModelForQuickQuoteItem(string application, string userId)
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
                    CheckForReadonly(crmUser);
                    var team = crmUser.ViewConfig.Team.AsTeam();

                    var coid = team.Location.AsLocation().GetCoid();

                    var exchangeRatesFromBaseCurrency = new Dictionary<string, decimal>();
                    var helperCurrency = new HelperCurrencyForIMetal();

                    var baseCurrency = helperCurrency.GetDefaultCurrencyForCoid(coid);

                    foreach (var currency in helperCurrency.GetSupportedDisplayCurrencyCodes().ToList())
                        exchangeRatesFromBaseCurrency.Add(currency,
                            helperCurrency.ConvertValueFromCurrencyToCurrency(1, baseCurrency, currency));

                    result.ExchangeRatesFromBaseCurrency = exchangeRatesFromBaseCurrency;
                    var quoteItem = new CrmQuoteItem
                    {
                        Coid = coid,
                        SalesPerson = crmUser.AsCrmUserRef(),
                        QuickQuoteData = new QuickQuoteData(coid),
                        QuoteSource = QuoteSource.QuickQuoteItem
                    };


                    var model = new QuoteItemModel(application, crmUser.UserId, quoteItem)
                    {
                        Application = application,
                        UserId = userId,
                        IsDirty = true
                    };

                    result.QuoteItemModel = model;
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
        [Route("quotes/GetNewQuoteItemModelForCrozCalcItem/{application}/{userId}/{coid}/{displayCurrency}")]
        public async Task<JsonResult> GetNewQuoteItemModelForCrozCalcItem(string application, string userId,
            string coid, string displayCurrency)
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
                    CheckForReadonly(crmUser);
                    var team = crmUser.ViewConfig.Team.AsTeam();


                    var quoteItem = new CrmQuoteItem
                    {
                        Coid = coid,
                        SalesPerson = crmUser.AsCrmUserRef(),
                        CrozCalcItem = new CrozCalcItem(coid, displayCurrency),
                        QuoteSource = QuoteSource.CrozCalcItem
                    };


                    var model = new QuoteItemModel(application, crmUser.UserId, quoteItem)
                    {
                        Application = application,
                        UserId = userId,
                        IsDirty = true
                    };

                    result.QuoteItemModel = model;
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
        [Route("quotes/GetOrderQuantityModel/{application}/{userId}")]
        public async Task<JsonResult> GetOrderQuantityModel(string application, string userId)
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
                    CheckForReadonly(crmUser);

                    var orderQuantity = new OrderQuantity
                    {
                        Pieces = 1,
                        QuantityType = "in",
                        Quantity = 0
                    };
                    result.OrderQuantityModel = orderQuantity;

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
        [Route("quotes/GetQuoteItemModel/{application}/{userId}/{quoteItemId}")]
        public async Task<JsonResult> GetQuoteItemModel(string application, string userId, string quoteItemId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var item = _helperQuote.GetQuoteItem(quoteItemId);
                    var crmUser = GetCrmUser(application, userId);
                    CheckForReadonly(crmUser);

                    result.QuoteItemModel = new QuoteItemModel(application, crmUser.UserId, item);
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
        [Route("quotes/SaveQuote")]
        public async Task<JsonResult> SaveQuote([FromBody] QuoteModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                _logger.Log(GetClassName(), $"SaveQuote:ModelError Occurred:{ex.Message}", ex, true, true);

                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = HttpStatusCode.OK; //CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    model.Coid = model.Coid.ToUpper();
                    if (model.Coid == "USA") model.Coid = "INC";

                    var index = 0;
                    foreach (var quoteItemModel in model.Items) quoteItemModel.Index = ++index;

                    var crmUser = GetCrmUser(model.Application, model.UserId);
                    CheckForReadonly(crmUser);
                    var quote = _helperQuote.SaveQuote(model);
                    result.QuoteModel = new QuoteModel(model.Application, crmUser.UserId, quote);
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                _logger.Log(GetClassName(), $"SaveQuote:{model?.QuoteId}:{model?.SalesPerson?.FullName}", e, true,
                    true);
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/RemoveQuote/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> RemoveQuote(string application, string userId, string quoteId)
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
                    var repItems = new RepositoryBase<CrmQuoteItem>();
                    var quote = rep.Find(quoteId);

                    if (quote == null) throw new Exception("Quote was not found");

                    var crmUser = GetCrmUser(application, userId);
                    CheckForReadonly(crmUser);

                    if (quote.SalesPerson.Id != crmUser.Id.ToString())
                        if (!crmUser.IsAdmin)
                            throw new Exception(
                                "You must be an Admin or the SalesPerson for the Quote in order to Remove it.");

                    foreach (var itemRef in quote.Items.ToList())
                    {
                        var quoteItem = itemRef.AsQuoteItem();
                        repItems.RemoveOne(quoteItem);
                    }

                    rep.RemoveOne(quote);

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
        [Route("quotes/GetQuote/{application}/{userId}/{quoteId}")]
        public async Task<JsonResult> GetQuote(string application, string userId, string quoteId)
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
                    var team = crmUser.ViewConfig.Team.AsTeam();


                    var quote = _helperQuote.GetQuote(quoteId);

                    var quoteModel = new QuoteModel(application, crmUser.UserId, quote);
                    if (quoteModel.DisplayCurrency == string.Empty) quoteModel.DisplayCurrency = team.DefaultCurrency;

                    result.QuoteModel = quoteModel;

                    result.PaymentTermList = PaymentTerms.GetPaymentTermsForCoid(quote.Coid);
                    result.FreightTermList = FreightTerms.GetFreightTermsForCoid(quote.Coid);

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
        [Route("quotes/GetCostOverrideModel/{application}/{userId}")]
        public async Task<JsonResult> GetCostOverrideModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.CostOverrideModel = new CostOverrideModel();
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
        [Route("quotes/FindQuoteItemsMatchingPartNumber/{application}/{userId}/{partNumber}")]
        public async Task<JsonResult> FindQuoteItemsMatchingPartNumber(string application, string userId,
            string partNumber)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var quoteItems = new RepositoryBase<CrmQuoteItem>().AsQueryable()
                        .Where(x => x.PartNumber == partNumber).OrderByDescending(x => x.CreateDateTime);
                    var quoteItemsResult = new List<QuoteItemModel>();
                    var crmUser = GetCrmUser(application, userId);

                    foreach (var crmQuoteItem in quoteItems)
                        quoteItemsResult.Add(new QuoteItemModel(application, crmUser.UserId, crmQuoteItem));

                    result.QuoteItemModels = quoteItemsResult;
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


        //Calculate
        [HttpPost]
        [AllowAnonymous]
        [Route("quotes/GetQuotePrice")]
        public async Task<JsonResult> GetQuotePrice([FromBody] CalculateQuotePriceModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                _logger.Log(GetClassName(), $"GetQuotePrice:ModelError Occurred:{ex.Message}", ex, true, true);
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(model.Application, model.UserId);
                    CheckForReadonly(crmUser);

                    var models = _helperQuote.CalculateQuotePrice(model);
                    result.QuotePriceModel = models.QuotePriceModel;
                    result.CalculateQuotePriceModel = models.CalculateQuotePriceModel;
                    result.QuoteCalcOverridesSupported = CalculateQuotePriceModel.GetQuoteCalcOverridesSupported();
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

        // NewStep
        [HttpGet]
        [AllowAnonymous]
        [Route(
            "quotes/GetNewProductionStep/{application}/{userId}/{coid}/{resourceTypeName}/{totalPieces}/{totalPounds}/{totalInches}/{displayCurrency}")]
        public async Task<JsonResult> GetNewProductionStepModel(string application, string userId, string coid,
            string resourceTypeName, decimal totalPieces, decimal totalPounds, decimal totalInches,
            string displayCurrency)
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
                    CheckForReadonly(crmUser);
                    var team = crmUser.ViewConfig.Team.AsTeam();

                    var teamCoidLocation = GetUserTeamCoidLocation(crmUser);
                    var resourceType = (ResourceType) Enum.Parse(typeof(ResourceType), resourceTypeName);

                    var productionCostListModel = new ProductionCostListModel(application, crmUser.UserId,
                        ProductionCostList.GetFor(teamCoidLocation.Coid, resourceType,
                            teamCoidLocation.Location.Office), displayCurrency);
                    //var productionTests = ProductionTestList.GetFor(userCoid, resourceTypeName);
                    var productionStepBase = new ProductionStepCostBase
                    {
                        ResourceType = resourceType,
                        Coid = coid,
                        CostValues = productionCostListModel.CostValues
                        //ProductionStepTests = productionTests.ProductionStepTests
                    };

                    var helperCurrency = new HelperCurrencyForIMetal();

                    foreach (var costValue in productionStepBase.CostValues)
                    {
                        costValue.InternalCost = helperCurrency.ConvertValueFromCurrencyToCurrency(
                            costValue.InternalCost, costValue.Currency,
                            displayCurrency);

                        costValue.MinimumCost = helperCurrency.ConvertValueFromCurrencyToCurrency(costValue.MinimumCost,
                            costValue.Currency,
                            displayCurrency);

                        costValue.ProductionCost = helperCurrency.ConvertValueFromCurrencyToCurrency(
                            costValue.ProductionCost, costValue.Currency,
                            displayCurrency);

                        costValue.TotalPieces = totalPieces;
                        costValue.TotalPounds = totalPounds;
                        costValue.TotalInches = totalInches;
                        costValue.Currency = displayCurrency;

                        costValue.IsActive = false;
                        //if (costValue.Currency == string.Empty)
                        //{
                        //    costValue.Currency = displayCurrency;
                        //}
                    }

                    result.ProductionStepCostModel = new ProductionStepCostModel(productionStepBase, totalInches);
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
        [Route("quotes/GetProductionCostListModel/{application}/{userId}/{resourceTypeName}")]
        public async Task<JsonResult> GetProductionCostListModel(string application, string userId,
            string resourceTypeName)
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
                    CheckForReadonly(crmUser);
                    var team = crmUser.ViewConfig.Team.AsTeam();
                    var teamCoidLocation = GetUserTeamCoidLocation(crmUser);
                    var resourceType = (ResourceType) Enum.Parse(typeof(ResourceType), resourceTypeName);

                    result.ProductionCostListModel = new ProductionCostListModel(application, crmUser.UserId,
                        ProductionCostList.GetFor(teamCoidLocation.Coid, resourceType,
                            teamCoidLocation.Location.Office), team.DefaultCurrency);
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
        [Route("quotes/GetNewQuoteTestPieceModel/{application}/{userId}")]
        public async Task<JsonResult> GetNewQuoteTestPieceModel(string application, string userId)
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
                    CheckForReadonly(crmUser);

                    result.CreateNewQuoteTestPieceModel = new CreateNewQuoteTestPieceModel(application, crmUser.UserId);
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
        [Route("quotes/GetNewQuoteTestPiece")]
        public async Task<JsonResult> GetNewQuoteTestPiece([FromBody] CreateNewQuoteTestPieceModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(model.Application, model.UserId);
                    CheckForReadonly(crmUser);
                    result.QuoteTestPiece = model.GetQuoteTestPiece();
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
        [Route("quotes/GetNewProductionCostValue/{application}/{userId}/{totalPieces}/{totalPounds}/{totalInches}")]
        public async Task<JsonResult> GetNewProductionCostValue(string application, string userId, decimal totalPieces,
            decimal totalPounds, decimal totalInches)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var perType = (PerType) Enum.Parse(typeof(PerType), "PerPound");

                    var newId = Guid.NewGuid();

                    var crmUser = GetCrmUser(application, userId);
                    CheckForReadonly(crmUser);
                    var costValue = new CostValue
                    {
                        IsActive = false,
                        TotalPieces = totalPieces,
                        TotalInches = totalInches,
                        TotalPounds = totalPounds,
                        PerType = perType,
                        InternalCost = 0,
                        ProductionCost = 0,
                        Id = newId,
                        MinimumCost = 0,
                        TypeName = string.Empty,
                        PerTypeName = perType.ToString()
                    };
                    result.CostValueModel = new CostValueModel(costValue);
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

        [HttpPost]
        [AllowAnonymous]
        [Route("quotes/ModifyProductionCostValue")]
        public async Task<JsonResult> ModifyProductionCostValue([FromBody] ModifyProductionCostValueModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = GetCrmUser(model.Application, model.UserId);
                    CheckForReadonly(crmUser);

                    var perType = (PerType) Enum.Parse(typeof(PerType), model.PerTypeName);

                    var newId = Guid.Parse(model.Id);

                    var costValue = new CostValue
                    {
                        IsActive = model.IsActive,
                        TotalPieces = model.TotalPieces,
                        TotalInches = model.TotalInches,
                        TotalPounds = model.TotalPounds,
                        PerType = perType,
                        InternalCost = model.InternalCost,
                        ProductionCost = model.ProductionCost,
                        Id = newId,
                        MinimumCost = model.MinimumCost,
                        TypeName = model.TypeName,
                        PerTypeName = model.PerTypeName
                    };
                    result.CostValueModel = new CostValueModel(costValue);
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

        #region IMetal Calls

        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/GetCompanyAddressesFromIMetal/{application}/{userId}/{coid}/{companyId}")]
        public async Task<JsonResult> GetCompanyAddressesFromIMetal(string application, string userId, string coid,
            string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var companyAddresses = GetAddressesForCompanyFromIMetal(coid, companyId);

                    result.PrimaryAddress = companyAddresses.PrimaryAddress;
                    result.OtherAddresses = companyAddresses.OtherAddress;

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
        [Route("quotes/GetOsirisDocumentListForQuoteItem/{application}/{userId}/{quoteItemId}")]
        public async Task<JsonResult> GetOsirisDocumentListForQuoteItem(string application, string userId,
            string quoteItemId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var quoteItem = _helperQuote.GetQuoteItem(quoteItemId);
                    if (quoteItem == null) throw new Exception("Unable to find Quote Item");

                    //var coid = quoteItem.Coid;
                    var coid = quoteItem.QuotePrice.StartingProduct.Coid;
                    var tagNumber = quoteItem.QuotePrice?.MaterialCostValue?.BaseCost?.TagNumber;

                    var repository = new OsirisRepository();
                    var docs = repository.GetOsirisDocumentList(coid, tagNumber, "");

                    result.OsirisDocs = docs;

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
        [Route("quotes/AttachOsirisDocumentToQuoteItem/{application}/{userId}/{documentId}/{quoteItemId}")]
        public async Task<JsonResult> AttachOsirisDocumentToQuoteItem(string application, string userId,
            long documentId, string quoteItemId)
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
                    var quoteItem = _helperQuote.GetQuoteItem(quoteItemId);
                    if (quoteItem == null) throw new Exception("Unable to find Quote Item");


                    var repository = new OsirisRepository();
                    var coid = quoteItem.Coid;
                    var tagNumber = quoteItem.QuotePrice.BaseCost.TagNumber;

                    var docs = repository.GetOsirisDocumentList(coid, tagNumber, "");
                    var docToFetch = docs.SingleOrDefault(x => x.ID == documentId);

                    if (docToFetch == null) throw new Exception("Document could not be found in Osiris");

                    var document = repository.GetOsirisDocumentAsStream(coid, documentId);

                    var fileBytes = document.ToArray();
                    var fileAttachmentId = FileAttachmentsVulcan.SaveFileAttachment(
                        fileBytes, docToFetch.FileName, quoteItem, FileAttachmentType.Action, crmUser.User.Id);

                    var attachedFiles = FileAttachmentsVulcan.GetAllAttachmentsForDocument(quoteItem) ??
                                        new List<GridFSFileInfo>();
                    result.AttachedFiles = attachedFiles.Select(x => new FileAttachmentModel(x)).ToList();


                    result.OsirisDocs = docs;

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

        private (Address PrimaryAddress, List<Address> OtherAddress)
            GetAddressesForCompanyFromIMetal(string coid, string companyId)
        {
            var query = new QueryCompany(coid);
            var companyRef = _helperCompany.GetCompanyRef(companyId);
            query.Id = companyRef.SqlId;

            var companySearchResult = query.Execute().FirstOrDefault();
            if (companySearchResult == null) throw new Exception("Company no longer exists in iMetal");

            var primaryAddress = new Address
            {
                Id = Guid.NewGuid(),
                AddressLine1 = companySearchResult.PrimaryAddress.Address,
                AddressLine2 = string.Empty,
                City = companySearchResult.PrimaryAddress.Town,
                StateProvince = companySearchResult.PrimaryAddress.County,
                PostalCode = companySearchResult.PrimaryAddress.PostCode,
                Country = companySearchResult.PrimaryAddress.CountryName,
                Type = AddressType.Primary
            };

            var otherAddresses = new List<Address>();
            otherAddresses.AddRange(companySearchResult.Addresses.Select(x => new Address
            {
                Id = Guid.NewGuid(),
                AddressLine1 = x.Address,
                AddressLine2 = string.Empty,
                City = x.Town,
                StateProvince = x.County,
                PostalCode = x.PostCode,
                Country = x.CountryName,
                Type = AddressType.Other
            }));

            return (primaryAddress, otherAddresses);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/GetProductMaster/{application}/{userId}/{coid}/{productId}")]
        public async Task<JsonResult> GetProductMaster(string application, string userId, string coid, int productId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ProductMaster = new ProductMaster(coid, productId);
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
        [Route(
            "quotes/GetFinishedProductChoices/{application}/{userId}/{coid}/{metalCategory}/{stockGrade}/{outerDiameterMax}/{insideDiameterMin}/{barOrTube}/{productCondition}/{resourceTypeName}")]
        public async Task<JsonResult> GetFinishedProductChoices(string application, string userId, string coid,
            string metalCategory, string stockGrade, decimal outerDiameterMax, decimal insideDiameterMin,
            string barOrTube, string productCondition, string resourceTypeName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    if (barOrTube != "Bar" && barOrTube != "Tube")
                        throw new Exception("Unable to determine Bar or Tube");
                    var resourceType = (ResourceType) Enum.Parse(typeof(ResourceType), resourceTypeName);

                    var cacheId = CacheSettings.GetCurrentProductMastersCacheId();
                    var productMasters = ProductMastersCache.Helper.Find(x => x.CacheId == cacheId && x.Coid == coid)
                        .ToList();
                    var productMasterResults = new List<ProductMaster>();

                    var matchingProducts = productMasters.Where(x =>
                            x.StockType.Contains(barOrTube) && x.StockGrade == stockGrade &&
                            x.OuterDiameter <= outerDiameterMax && x.InsideDiameter >= insideDiameterMin &&
                            x.MetalCategory == metalCategory.ToUpper()).OrderByDescending(x => x.OuterDiameter)
                        .ThenByDescending(x => x.InsideDiameter).ThenBy(x => x.ProductCode).ToList();

                    if (_heatTreatTypes.Contains(resourceType))
                        matchingProducts = matchingProducts.Where(x =>
                            x.OuterDiameter == outerDiameterMax && x.InsideDiameter == insideDiameterMin &&
                            x.ProductCondition != productCondition).ToList();

                    if (_machineResourceTypes.Contains(resourceType))
                        matchingProducts = matchingProducts.Where(x => x.ProductCondition == productCondition &&
                                                                       !(x.OuterDiameter == outerDiameterMax &&
                                                                         x.InsideDiameter == insideDiameterMin))
                            .ToList();

                    foreach (var p in matchingProducts)
                        productMasterResults.Add(new ProductMaster
                        {
                            TheoWeight = p.TheoWeight,
                            Coid = coid,
                            OuterDiameter = p.OuterDiameter,
                            InsideDiameter = p.InsideDiameter,
                            ProductCode = p.ProductCode,
                            ProductId = p.ProductId,
                            MetalCategory = p.MetalCategory,
                            ProductType = p.ProductType,
                            ProductCondition = p.ProductCondition,
                            StockGrade = p.StockGrade,
                            FactorForLbs = UomHelper.GetFactorForPounds(coid),
                            FactorForKilograms = UomHelper.GetFactorForKilograms(coid)
                        });

                    result.ProductMasterList = productMasterResults;
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
        [Route(
            "quotes/GetFinishedProductChoicesNoResource/{application}/{userId}/{coid}/{metalCategory}/{outerDiameterMax}/{insideDiameterMin}/{barOrTube}/{productCondition}")]
        public async Task<JsonResult> GetFinishedProductChoicesNoResource(string application, string userId,
            string coid, string metalCategory, decimal outerDiameterMax, decimal insideDiameterMin, string barOrTube,
            string productCondition)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    if (barOrTube != "Bar" && barOrTube != "Tube")
                        throw new Exception("Unable to determine Bar or Tube");

                    var cacheId = CacheSettings.GetCurrentProductMastersCacheId();
                    var productMasters = ProductMastersCache.Helper.Find(x => x.CacheId == cacheId && x.Coid == coid)
                        .ToList();

                    var productMasterResults = new List<ProductMaster>();

                    var matchingProducts = productMasters.Where(x =>
                            x.StockType.Contains(barOrTube) && x.OuterDiameter <= outerDiameterMax &&
                            x.InsideDiameter >= insideDiameterMin && x.MetalCategory == metalCategory.ToUpper())
                        .OrderByDescending(x => x.OuterDiameter).ThenByDescending(x => x.InsideDiameter)
                        .ThenBy(x => x.ProductCode).ToList();

                    foreach (var p in matchingProducts)
                        productMasterResults.Add(new ProductMaster
                        {
                            TheoWeight = p.TheoWeight,
                            Coid = coid,
                            OuterDiameter = p.OuterDiameter,
                            InsideDiameter = p.InsideDiameter,
                            ProductCode = p.ProductCode,
                            ProductId = p.ProductId,
                            MetalCategory = p.MetalCategory,
                            ProductType = p.ProductType,
                            ProductCondition = p.ProductCondition,
                            FactorForLbs = UomHelper.GetFactorForPounds(coid),
                            FactorForKilograms = UomHelper.GetFactorForKilograms(coid)
                        });

                    result.ProductMasterList = productMasterResults;
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
        [Route("quotes/GetAvailableStockItemsForQuoteItem/{application}/{userId}/{coid}/{tagNumber}")]
        [AllowAnonymous]
        public async Task<JsonResult> GetAvailableStockItemsForQuoteItem(string application, string userId, string coid,
            string tagNumber)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var currentStockItem = StockItemsAdvancedQuery.AsQueryable(coid)
                        .SingleOrDefault(x => x.TagNumber == tagNumber);

                    if (currentStockItem == null) throw new Exception("MachinedPartFromCacheValue not found");


                    var allStockItems = StockItemsAdvancedQuery.AsQueryable(coid)
                        .Where(x => x.ProductId == currentStockItem.ProductId && x.AvailableLength > 0 &&
                                    x.StockItemId != currentStockItem.StockItemId)
                        .ToList();
                    result.StockItems = allStockItems
                        .Select(x => new
                        {
                            x.StockItemId, x.TagNumber, x.AvailableLength, x.LengthUnit, x.CostPerLb, x.CostPerKg,
                            x.WarehouseCode, x.WarehouseName
                        })
                        .OrderByDescending(x => x.AvailableLength);
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

        #region Lookup Lists

        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/GetSalesGroups/{application}/{userId}/{coid}")]
        public async Task<JsonResult> GetSalesGroups(string application, string userId, string coid)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    SalesGroup.UpdateListForCoid(coid);
                    var salesGroups = new RepositoryBase<SalesGroup>().AsQueryable()
                        .Where(x => x.Coid == coid && x.IsActive && x.IgnoreInVulcan == false).ToList();
                    result.SalesGroups = salesGroups.Select(x => new SalesGroupModel(x)).OrderBy(x => x.Code).ToList();
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
        [Route("quotes/GetOemList/{application}/{userId}")]
        public async Task<JsonResult> GetOemList(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var oemList = new RepositoryBase<OemType>().AsQueryable().Select(x => x.Name).ToList();
                    if (!oemList.Any())
                        oemList = new List<string>
                        {
                            "Baker Hughes",
                            "Baker Oil Tools",
                            "Halliburton",
                            "SCHLUMBERGER",
                            "TFMC",
                            "Weatherford"
                        };

                    result.OemList = oemList.OrderBy(x => x);
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
        [Route("quotes/SaveOem/{application}/{userId}/{oemId}/{name}")]
        public async Task<JsonResult> SaveOem(string application, string userId, string oemId, string name)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var rep = new RepositoryBase<OemType>();
                    var oemType = rep.Find(oemId);
                    if (oemType == null) throw new Exception("Oem not found");

                    oemType.Name = name;
                    rep.Upsert(oemType);
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
        [Route("quotes/GetResourceTypes/{application}/{userId}")]
        public async Task<JsonResult> GetResourceType(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var resourceTypeList = new Dictionary<int, string>();
                    foreach (ResourceType perType in Enum.GetValues(typeof(ResourceType)))
                        resourceTypeList.Add((int) perType, perType.ToString());

                    result.ResourceTypes = resourceTypeList;
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
        [Route("quotes/PublishProductionCostList")]
        public async Task<JsonResult> PublishProductionCostList([FromBody] ProductionCostListModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var crmUser = _helperUser.GetCrmUser(model.Application, model.UserId);
                    var team = crmUser.ViewConfig.Team.AsTeam();
                    var teamCoidLocation = GetUserTeamCoidLocation(crmUser);

                    var user = GetUser(crmUser.User.Id);
                    var rep = new RepositoryBase<ProductionCostList>();
                    var costValueList = rep.AsQueryable()
                                            .SingleOrDefault(x =>
                                                x.Coid == teamCoidLocation.Coid &&
                                                x.ResourceType == model.ResourceType &&
                                                x.Location.Office == teamCoidLocation.Location.Office) ??
                                        new ProductionCostList
                                        {
                                            Coid = teamCoidLocation.Coid,
                                            ResourceType = model.ResourceType,
                                            Location = teamCoidLocation.Location.AsLocationRef()
                                        };
                    costValueList.CostValues = model.CostValues.OrderBy(x => x.TypeName).ToList();
                    costValueList.ModifiedByUserId = user.Id.ToString();
                    rep.Upsert(costValueList);
                    result.CostValueListModel = new ProductionCostListModel(model.Application, model.UserId,
                        costValueList, team.DefaultCurrency);
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

        private static (Location Location, string Coid) GetUserTeamCoidLocation(CrmUser crmUser)
        {
            var team = crmUser.ViewConfig.Team.AsTeam();
            var userTeamLocation = team.Location.AsLocation();
            var teamCoid = userTeamLocation.GetCoid();
            return (userTeamLocation, teamCoid);
        }

        #endregion

        #region Product Buckets

        [HttpGet]
        [AllowAnonymous]
        [Route("quotes/GetNewProductBucketModel/{application}/{userId}/{coid}")]
        public async Task<JsonResult> GetNewProductBucketModel(string application, string userId, string coid)
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
                    CheckForReadonly(crmUser);

                    result.ProductBucketModel =
                        new ProductBucketModel(application, crmUser.UserId, new ProductBucket {Coid = coid});
                    result.ProductBucketHelper = new ProductBucketHelper(coid);
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
        [Route("quotes/GetProductBucketModel/{application}/{userId}/{productBucketId}")]
        public async Task<JsonResult> GetProductBucketModel(string application, string userId, string productBucketId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var bucket = new RepositoryBase<ProductBucket>().Find(productBucketId);
                    if (bucket == null) throw new Exception("Product Bucket not found");

                    result.ProductBucketModel = new ProductBucketModel(application, userId, bucket);
                    result.ProductBucketHelper = new ProductBucketHelper(bucket.Coid);
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
        [Route("quotes/SaveProductBucket")]
        public async Task<JsonResult> SaveProductBucket([FromBody] ProductBucketModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;

            try
            {
                CheckForModelErrors();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var rep = new RepositoryBase<ProductBucket>();
                    var bucket = rep.Find(model.Id);
                    var crmUser = GetCrmUser(model.Application, model.UserId);
                    CheckForReadonly(crmUser);

                    if (!Enum.TryParse(model.CategoryConditionAndOrValue, out AndOrValue categoryConditionAndOr))
                        throw new Exception($"{model.CategoryConditionAndOrValue} is not a valid And/Or value");


                    if (bucket == null)
                        bucket = new ProductBucket
                        {
                            CreatedByUserId = crmUser.UserId,
                            Id = ObjectId.Parse(model.Id)
                        };
                    bucket.Coid = model.Coid;
                    bucket.IgnoreWarehouseCodes = model.IgnoreWarehouseCodes;
                    bucket.Name = model.Name;
                    bucket.ProductCategories = model.ProductCategories;
                    bucket.ProductConditions = model.ProductConditions;
                    bucket.ShowOnlyWarehouseCodes = model.ShowOnlyWarehouseCodes;
                    bucket.CategoryConditionAndOrValue = categoryConditionAndOr;
                    bucket = rep.Upsert(bucket);
                    result.ProductBucketModel = new ProductBucketModel(model.Application, model.UserId, bucket);
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
        [Route("quotes/GetProductBucketListForCoid/{application}/{userId}/{coid}")]
        public async Task<JsonResult> GetProductBucketListForCoid(string application, string userId, string coid)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var rep = new RepositoryBase<ProductBucket>();
                    var buckets = rep.AsQueryable().Where(x => x.Coid == coid).ToList();
                    if (buckets.Any())
                        result.ProductBuckets = buckets.Select(x => new {x.Id, x.Name}).OrderBy(x => x.Name);
                    else
                        result.ProductBuckets = buckets;
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
    }
}