using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class ContactController: BaseController
    {
        private readonly IHelperContact _helperContact;
        private readonly IHelperApplication _helperApplication;

        public ContactController(
            IHelperContact helperContact,
            IHelperApplication helperApplication,
            IHelperUser helperUser) : base(helperUser)
        {
            _helperContact = helperContact;
            _helperApplication = helperApplication;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("contact/GetContact/{application}/{userId}/{contactId}")]
        public async Task<JsonResult> GetContact(string application, string userId, string contactId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var contact = _helperContact.GetContact(contactId);
                    result.ContactModel = new ContactModel(application, userId, contact);
                    result.ContactRef = contact.AsContactRef();
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
        [Route("contact/SetReportsTo/{application}/{userId}/{companyId}/{contactId}/{reportsToId}")]
        public async Task<JsonResult> SetReportsTo(string application, string userId, string companyId, string contactId,
            string reportsToId)
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
                    var contact = Contact.Helper.FindById(contactId);
                    if (reportsToId == "(root)")
                    {
                        contact.ReportsTo = null;
                        Contact.Helper.Upsert(contact);
                        result.ContactModel = new ContactModel(application, userId, contact);
                        result.CompanyContactTree = new CompanyContactTree(company.AsCompanyRef());
                    }
                    else
                    {
                        var reportsTo = Contact.Helper.FindById(reportsToId).AsContactRef();
                        var reportsToResults = _helperContact.SetReportsTo(application, userId, company, contact.AsContactRef(), reportsTo);
                        result.ContactModel = reportsToResults.contactModel;
                        result.CompanyContactTree = reportsToResults.companyContactTree;
                    }
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
        [Route("contact/GetCompanyContactTree/{application}/{userId}/{companyId}")]
        public async Task<JsonResult> GetCompanyContactTree(string application, string userId, string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var company = Company.Helper.FindById(companyId).AsCompanyRef();
                    result.CompanyContactTree = _helperContact.GetCompanyContactTree(company);
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
        [Route("contact/ResetCompanyContactTree/{application}/{userId}/{companyId}")]
        public async Task<JsonResult> ResetCompanyContactTree(string application, string userId, string companyId)
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
                    foreach (var companyContact in company.Contacts.Select(x=>x.AsContact()))
                    {
                        companyContact.ReportsTo = null;
                        Contact.Helper.Upsert(companyContact);
                    }

                    result.CompanyContactTree = _helperContact.GetCompanyContactTree(company.AsCompanyRef());
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
        [Route("contact/SaveContact")]
        public async Task<JsonResult> SaveContact([FromBody] ContactModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    CheckForModelErrors();
                    var contact = _helperContact.SaveContact(model);
                    result.ContactModel = new ContactModel(model.Application, model.UserId, contact);
                    result.ContactRef = contact.AsContactRef();
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
        [Route("contact/UseExistingContact/{application}/{userId}/{contactId}")]
        public async Task<JsonResult> UseExistingContact(string application, string userId, string contactId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ContactModel = _helperContact.UseExistingContact(application, userId, contactId);
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
        [Route("contact/GetAllAvailableContacts/{application}/{userId}")]
        public async Task<JsonResult> GetAllAvailableContacts(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ContactList = _helperContact.GetAllAvailableContacts(application, userId);
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
        [Route("contact/GetMyTeamContacts/{application}/{userId}")]
        public async Task<JsonResult> GetMyTeamContacts(string application, string userId)
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
                    var team = crmUser.ViewConfig.Team.AsTeam();
                    var contactList = new List<ContactModel>();
                    foreach (var company in team.Companies.Select(x=>x.AsCompany()).ToList())
                    {
                        foreach (var contact in company.Contacts.Select(x=>x.AsContact()).ToList())
                        {
                            contactList.Add(new ContactModel(application, userId, contact));
                        }
                    }

                    result.ContactList = contactList;
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
        [Route("contact/GetAllAvailableContactsForCompany/{application}/{userId}/{companyId}")]
        public async Task<JsonResult> GetAllAvailableContactsForCompany(string application, string userId, string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ContactList = _helperContact.GetAllAvailableContactsForCompany(application, userId, companyId);
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
        [Route("contact/GetAllAvailableContactsForProspect/{application}/{userId}/{prospectId}")]
        public async Task<JsonResult> GetAllAvailableContactsForProspect(string application, string userId, string prospectId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ContactList = _helperContact.GetAllAvailableContactsForProspect(application, userId, prospectId);
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
        [Route("contact/GetAllAvailableContactsForCompanyGroup/{application}/{userId}/{companyGroupId}")]
        public async Task<JsonResult> GetAllAvailableContactsForCompanyGroup(string application, string userId, string companyGroupId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ContactList =
                        _helperContact.GetAllAvailableContactsForCompanyGroup(
                            application, userId, companyGroupId);
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

        // AllContactsForLocation

        [AllowAnonymous]
        [HttpGet]
        [Route("contact/RemoveContact/{application}/{userId}/{contactId}")]
        public async Task<JsonResult> RemoveContact(string application, string userId, string contactId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperContact.RemoveContact(application, userId, contactId);
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
        [Route("contact/GetAllContacts/{application}/{userId}")]
        public async Task<JsonResult> GetAllContacts(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ContactList = _helperContact.GetAllContacts(application, userId);
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
        [Route("contact/GetContactsForUser/{application}/{userId}")]
        public async Task<JsonResult> GetContactsForUser(string application, string userId)
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
                    foreach (var contactRef in crmUser.Contacts.ToList())
                    {
                        if (contactRef.GetIsDeleted())
                        {
                            crmUser.Contacts.Remove(contactRef);
                            continue;
                        }
                        contactRef.RefreshPropertyValues();
                    }
                    crmUser.SaveToDatabase();
                    result.ContactList = crmUser.Contacts.Select(x => new ContactModel(application, userId, x.AsContact())).OrderBy(x => x.Person.FirstName).ThenBy(x => x.Person.LastName).ToList();
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
        [Route("contact/GetNewContact/{application}/{userId}")]
        public async Task<JsonResult> GetNewContact(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var user = GetCrmUser(application, userId);
                    result.ContactModel = _helperContact.GetNewContact(application, userId);
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
        [Route("contact/GetContactsForTeam/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> GetContactsForTeam(string application, string userId, string teamId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var repQuote = new RepositoryBase<CrmQuote>();

                    var contacts = repQuote.AsQueryable().Where(x => x.Team.Id == teamId && x.Contact != null).Select(x => x.Contact).Distinct().OrderBy(x => x.FirstName)
                        .ThenBy(x => x.LastName).ToList();
                    result.Contacts = contacts;
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
