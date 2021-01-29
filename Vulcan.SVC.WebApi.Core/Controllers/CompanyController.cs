using DAL.iMetal.Core.Queries;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Base.Core.ValueBucket;
using DAL.Vulcan.Mongo.Core.DocClass.Companies;
using DAL.Vulcan.Mongo.Core.DocClass.CompanyGroups;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Helpers;
using DAL.Vulcan.Mongo.Core.Importers;
using DAL.Vulcan.Mongo.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Vulcan.SVC.WebApi.Core.Models;
using Company = DAL.Vulcan.Mongo.Core.DocClass.Companies.Company;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class CompanyController : BaseController
    {
        private readonly IHelperApplication _helperApplication;
        private readonly IHelperCompany _helperCompany;
        private readonly IHelperProspect _helperProspect;
        private readonly IHelperLocation _helperLocation;

        public CompanyController(
            IHelperApplication helperApplication,
            IHelperCompany helperCompany,
            IHelperProspect helperProspect,
            IHelperLocation helperLocation,
            IHelperUser helperUser) : base(helperUser)
        {
            _helperApplication = helperApplication;
            _helperCompany = helperCompany;
            _helperProspect = helperProspect;
            _helperLocation = helperLocation;
        }

        #region IMetal Imports

        [HttpGet]
        [AllowAnonymous]
        [Route("company/ImportCompaniesFromSourceSystem/{application}/{userId}/{coid}")]
        public async Task<JsonResult> ImportCompaniesFromSourceSystem(string application, string userId, string coid)
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
                    if (!crmUser.IsAdmin) throw new Exception("You are not an Admin");

                    var importer = new CompanyImporter { DebugMode = false };
                    importer.Execute(coid);
                    result.ImportCompanyLog = new ImportCompanyLogModel(importer);

                    var teams = new RepositoryBase<Team>().AsQueryable().ToList();
                    foreach (var team in teams)
                    {
                        team.RefreshTeamCompaniesList();
                        team.RefreshAllianceNonAllianceLists();
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

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("company/GetCreditInfo/{application}/{userId}/{companyId}")]
        //public async Task<JsonResult> GetCreditInfo(string application, string userId, string companyId)
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var statusCode = CheckToken(application, userId);
        //    try
        //    {
        //        ThrowExceptionForBadToken(statusCode);

        //        var company = Company.Helper.FindById(companyId);

        //        result.iMetalCompanyCreditRuleModel =
        //            new QueryCompany(company.Location.GetCoid()).GetCreditRuleForCompany(company.SqlId);

        //        result.Success = true;
        //        _helperUser.ControllerMethodCalled(application, userId, "CompanyController", "ImportCompaniesFromSourceSystem");

        //    }
        //    catch (Exception e)
        //    {
        //        result.ErrorMessage = e.Message;
        //        result.Success = false;
        //    }
        //    return JsonResultWithStatusCode(result, statusCode);
        //}


        [HttpGet]
        [AllowAnonymous]
        [Route("company/ExecuteCompanyResolver/{application}/{userId}/{companyId}")]
        public async Task<JsonResult> ExecuteCompanyResolver(string application, string userId, string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var company = new RepositoryBase<Company>().Find(companyId);
                    CompanyResolver.Execute(company);

                    // Reload
                    company = new RepositoryBase<Company>().Find(companyId);
                    result.CompanyRef = company.AsCompanyRef();
                    result.PrimaryAddress = company.Addresses.FirstOrDefault(x => x.Type == AddressType.Primary) ??
                                            new Address();
                    result.ShipToAddress = company.Addresses.LastOrDefault(x => x.Type == AddressType.Shipping) ??
                                           company.Addresses.LastOrDefault(x => x.Type == AddressType.Other) ??
                                           company.Addresses.LastOrDefault(x => x.Type == AddressType.Primary) ??
                                           company.Addresses.LastOrDefault() ??
                                           new Address();
                    result.OtherAddresses = company.Addresses.ToList();

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
        [Route("company/GetImportCompanyLog/{application}/{userId}")]
        public async Task<JsonResult> GetImportCompanyLog(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var companyImports = new RepositoryBase<CompanyImporter>().AsQueryable().ToList();
                    result.ImportCompanyLog = companyImports.OrderByDescending(x => x.ExecutedOn).Select(x => new ImportCompanyLogModel(x)).ToList();
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
        [Route("company/GetCompanyAddresses/{application}/{userId}/{coid}/{companyId}")]
        public async Task<JsonResult> GetCompanyAddresses(string application, string userId, string coid, string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    coid = coid.ToUpper();
                    if (coid == "USA") coid = "INC";
                    var companyRef = _helperCompany.GetCompanyRef(companyId);

                    var companyValues = CompanyResolver.GetAllCompanyAddresses(companyRef);
                    result.CompanyRef = companyValues.CompanyRef;
                    result.Addresses = companyValues.Addresses;

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
        [Route("company/RemoveAddress/{application}/{userId}/{companyId}/{addressId}")]
        public async Task<JsonResult> RemoveAddress(string application, string userId, string companyId, string addressId)
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
                    var isNorway = crmUser.ViewConfig.Team.Name.Contains("Norway");

                    var company = _helperCompany.GetCompanyRef(companyId).AsCompany();
                    var otherAddressToRemove = company.Addresses.FirstOrDefault(x => x.Id.ToString() == addressId);
                    if (otherAddressToRemove != null)
                    {
                        company.Addresses.Remove(otherAddressToRemove);
                        company.SaveToDatabase();
                    }
                    var companyDefaults = CompanyDefaults.GetCompanyDefaults(company.Location.GetCoid(), company, isNorway);
                    if (companyDefaults.ShippingAddressId.ToString() == addressId)
                    {
                        companyDefaults.ShippingAddressId = null;
                        companyDefaults.SaveToDatabase();
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


        #endregion

        #region No longer Used

        [HttpGet]
        [AllowAnonymous]
        [Route("company/FindNonAllianceCompanies/{application}/{userId}/{coid}/{searchValue?}")]
        public async Task<JsonResult> FindNonAllianceCompanies(string application, string userId, string coid, string searchValue = null)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {

                    ThrowExceptionForBadToken(statusCode);
                    coid = coid.ToUpper();
                    if (coid == "INC") coid = "USA";

                    var rep = new RepositoryBase<Company>();
                    var results = rep.AsQueryable().Where(x => x.IsAlliance == false && x.Location.Branch == coid);
                    if (searchValue != null)
                    {
                        searchValue = searchValue.ToUpper();
                        results = results.Where(x =>
                            x.Name.ToUpper().Contains(searchValue) ||
                            x.SearchTags.Any(t => t.ToUpper().Contains(searchValue)));
                    }

                    var companiesFound = results.ToList();
                    result.NonAllianceCompaniesFound =
                        companiesFound.Select(x => x.AsCompanyRef()).OrderBy(x => x.Name).ToList();
                    //var companyBucket = new ValueBucket<Company, string>();
                    //result.NonAllianceCompanies = companyBucket.Refresh(allCompanies, (company) => company.Name.ToUpper().Substring(0, 1));
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

        [HttpGet]
        [AllowAnonymous]
        [Route("company/FindNonAllianceCompaniesIndexed/{application}/{userId}/{coid}")]
        public async Task<JsonResult> FindNonAllianceCompaniesIndexed(string application, string userId, string coid)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {

                    ThrowExceptionForBadToken(statusCode);
                    coid = coid.ToUpper();
                    if (coid == "INC") coid = "USA";

                    var rep = new RepositoryBase<Company>();
                    var nonAllianceCompanies = rep.AsQueryable()
                        .Where(x => x.IsAlliance == false && x.Location.Branch == coid && x.Name.Length > 0).ToList();

                    var companyBucket = new ValueBucket<Company, string>();
                    var resultByName = companyBucket.Execute(nonAllianceCompanies,
                        (company) => company.Name.ToUpper().Substring(0, 1));

                    var model = resultByName.Select(bucketValue => new IndexValueModelForCompanyRef()
                        {
                            Index = bucketValue.GroupBy,
                            Companies = bucketValue.Documents.Select(x => x.AsCompanyRef()).OrderBy(x => x.Name)
                                .ToList()
                        })
                        .ToList();
                    result.NonAllianceIndex = model;
                    //var companyBucket = new ValueBucket<Company, string>();
                    //result.NonAllianceCompanies = companyBucket.Refresh(allCompanies, (company) => company.Name.ToUpper().Substring(0, 1));
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

        #endregion

        #region Queries

        [HttpGet]
        [AllowAnonymous]
        [Route("company/GetCompanyDefaults/{application}/{userId}/{coid}/{companyId}")]
        public async Task<JsonResult> GetCompanyDefaults(string application, string userId, string coid, string companyId)
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

                    CompanyResolver.Execute(company);

                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    var isNorway = crmUser.ViewConfig.Team.Name.Contains("Norway");

                    var companyDefaults = CompanyDefaults.GetCompanyDefaults(company.Location.GetCoid(), company, isNorway);
                    result.CompanyDefaultsModel = new CompanyDefaultsModel(application, userId, companyDefaults);


                    result.PaymentTermList = PaymentTerms.GetPaymentTermsForCoid(companyDefaults.Coid);
                    result.FreightTermList = FreightTerms.GetFreightTermsForCoid(companyDefaults.Coid);
                    var salesGroups = new RepositoryBase<SalesGroup>().AsQueryable().Where(x => x.Coid == companyDefaults.Coid).ToList();
                    result.SalesGroupList = salesGroups.Select(x => new SalesGroupModel(x)).OrderBy(x => x.Code).ToList();
                    result.CustomerUomList = Enum.GetNames(typeof(CustomerUom)).ToList();
                    result.ContactList = company.Contacts;
                    result.CompanyAddressList = company.Addresses;
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
        [Route("company/ClearAndRefreshCompanyDefaults/{application}/{userId}/{coid}/{companyId}")]
        public async Task<JsonResult> ClearAndRefreshCompanyDefaults(string application, string userId, string coid, string companyId)
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

                    var crmUser = _helperUser.GetCrmUser(application, userId);
                    var isNorway = crmUser.ViewConfig.Team.Name.Contains("Norway");

                    var companyDefaults = CompanyDefaults.ClearAndRefreshCompanyDefaults(company.Location.GetCoid(), companyId, isNorway);
                    result.CompanyDefaultsModel = new CompanyDefaultsModel(application, userId, companyDefaults);


                    result.PaymentTermList = PaymentTerms.GetPaymentTermsForCoid(companyDefaults.Coid);
                    result.FreightTermList = FreightTerms.GetFreightTermsForCoid(companyDefaults.Coid);
                    var salesGroups = new RepositoryBase<SalesGroup>().AsQueryable().Where(x => x.Coid == companyDefaults.Coid).ToList();
                    result.SalesGroupList = salesGroups.Select(x => new SalesGroupModel(x)).OrderBy(x => x.Code).ToList();
                    result.CustomerUomList = Enum.GetNames(typeof(CustomerUom)).ToList();
                    result.ContactList = company.Contacts;
                    result.CompanyAddressList = company.Addresses;
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
        [Route("company/SaveCompanyDefaults")]
        public async Task<JsonResult> SaveCompanyDefaults([FromBody] CompanyDefaultsModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    CheckForModelErrors();
                    ThrowExceptionForBadToken(statusCode);
                    model.SaveToDatabase();
                    result.CompanyDefaultsModel = model;

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
        [Route("company/GetQuoteHistoryReport/{application}/{userId}/{companyId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> GetQuoteHistoryReport(string application, string userId, string companyId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var company = new RepositoryBase<Company>().Find(companyId);
                    if (company == null) throw new Exception("Company not found");

                    result.QuoteHistoryReport = _helperCompany.GetReportQuoteHistory(company, fromDate, toDate, displayCurrency);

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
        [Route("company/GetAllCompanies/{application}/{userId}")]
        public async Task<JsonResult> GetAllCompanies(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var companies = new RepositoryBase<Company>().AsQueryable().ToList().Select(x => x.AsCompanyRef()).OrderBy(x => x.Coid).ThenBy(x => x.Name).ToList();

                    result.Companies = companies;
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
        [Route("company/GetMyTeamCompanies/{application}/{userId}")]
        public async Task<JsonResult> GetMyTeamCompanies(string application, string userId)
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
                    var companies = team.Companies;
                    result.Companies = companies.Where(x => x.IsActive).OrderBy(x => x.Code).ToList();
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
        [Route("company/GetMyTeamCompaniesByCode/{application}/{userId}")]
        public async Task<JsonResult> GetMyTeamCompaniesByCode(string application, string userId)
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
                    var companies = team.Companies;
                    result.Companies = companies.Where(x => x.IsActive).OrderBy(x=>x.Code).ToList();
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
        [Route("company/GetMyTeamCompaniesByName/{application}/{userId}")]
        public async Task<JsonResult> GetMyTeamCompaniesByName(string application, string userId)
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
                    var companies = team.Companies;
                    result.Companies = companies.Where(x => x.IsActive).OrderBy(x => x.Name).ToList();
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
        [Route("company/GetCompaniesForTeam/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> GetCompaniesForTeam(string application, string userId, string teamId)
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

                    var team = Team.Helper.Find(x => x.Id == ObjectId.Parse(teamId)).SingleOrDefault();
                    if (team == null) throw new Exception("Team not found");
                    result.Companies = team.Companies;
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
        [Route("company/GetCompaniesForBranch/{application}/{userId}/{branch}")]
        public async Task<JsonResult> GetCompaniesForBranch(string application, string userId, string branch)
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

                    result.Companies = _helperCompany.GetCompaniesForBranch(branch);
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
        [Route("company/GetCompanyBranches/{application}/{userId}")]
        public async Task<JsonResult> GetCompanyBranches(string application, string userId)
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

                    result.Branches = _helperCompany.GetUniqueCompanyBranches();
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
        [Route("company/GetFullCompany/{application}/{userId}/{coid}/{companyId}")]
        public async Task<JsonResult> GetFullCompany(string application, string userId, string coid, string companyId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(async () =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    coid = coid.ToUpper();
                    if (coid == "USA") coid = "INC";
                    var companyRef = _helperCompany.GetCompanyRef(companyId);



                    var companyQuery = await CompanyQuery.GetForId(coid, companyRef.SqlId, true);
                    if (companyQuery == null) throw new Exception("Company no longer exists in iMetal");

                    result.CompanyRef = companyRef;
                    result.Company = new { companyQuery.CustomerGroupCode, companyQuery.CustomerGroupDescription, companyQuery.Branch, companyQuery.Code, companyQuery.Name, companyQuery.Addresses, companyQuery.Telephone, companyQuery.FastDial, companyQuery.Fax, companyQuery.Email };

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

        #region Company Groups

        [HttpGet]
        [AllowAnonymous]
        [Route("company/GetCompanyTree/{application}/{userId}")]
        public async Task<JsonResult> GetCompanyTree(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {

                    ThrowExceptionForBadToken(statusCode);
                    var rep = new RepositoryBase<CompanyGroup>();
                    var roots = rep.AsQueryable().Where(x => x.ParentObjectId == ObjectId.Empty).OrderBy(x => x.Name)
                        .ToList();
                    var root = new List<CompanyTreeNode>();

                    foreach (var companyGroup in roots)
                    {
                        root.Add(companyGroup.AsTreeNode);
                    }

                    result.Root = root;

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
        [Route("company/GetCompanyGroupCompanies/{applicationId}/{userId}/{companyGroupId}")]
        public async Task<JsonResult> GetCompanyGroupCompanies(string application, string userId, string companyGroupId)
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
                    var companyGroupRef = _helperCompany.GetCompanyGroupRef(companyGroupId);
                    result.Companies = companyGroupRef.AsCompanyGroup().GetAllCompanies();
                });
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, statusCode);

        }

        [HttpGet]
        [Route("company/AddCompanyToCompanyGroup/{application}/{userId}/{companyId}/{companyGroupId}")]
        public async Task<JsonResult> AddCompanyToCompanyGroup(string application, string userId, string companyId,
            string companyGroupId)
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

                    var admin = GetCrmUser(application, userId);
                    if (!admin.IsAdmin)
                    {
                        throw new Exception("Account is not an admin");
                    }

                    var companyGroupRef = _helperCompany.GetCompanyGroupRef(companyGroupId);
                    var companyRef = _helperCompany.GetCompanyRef(companyId);

                    var companyGroup = companyGroupRef.AsCompanyGroup();

                    companyGroup.Companies.Add(companyRef);
                    companyGroup.Save();

                    result.Companies = companyGroup.Companies;
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

        [HttpGet]
        [Route("company/RemoveCompanyFromCompanyGroup/{application}/{userId}/{companyId}/{companyGroupId}")]
        public async Task<JsonResult> RemoveCompanyFromCompanyGroup(string application, string userId, string companyId,
            string companyGroupId)
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

                    var companyGroupRef = _helperCompany.GetCompanyGroupRef(companyGroupId);
                    var companyRef = _helperCompany.GetCompanyRef(companyId);

                    var companyGroup = companyGroupRef.AsCompanyGroup();

                    companyGroup.Companies.Remove(companyRef);
                    companyGroup.Save();

                    result.Companies = companyGroup.Companies;
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

        #endregion

        #region Prospects

        [HttpGet]
        [AllowAnonymous]
        [Route("prospect/GetNewProspectModel/{application}/{userId}")]
        public async Task<JsonResult> GetNewProspectModel(string application, string userId)
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

                    var locationId = crmUser.User.AsUser().Location.Id;

                    result.ProspectModel = _helperProspect.GetNewProspectModel(application, crmUser.User.Id, locationId);
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
        [Route("prospect/SaveProspect")]
        public async Task<JsonResult> SaveProspect([FromBody] ProspectModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var saveProspect = _helperProspect.SaveProspect(model);
                    result.ProspectModel = saveProspect.ProspectModel;
                    result.ProspectRef = saveProspect.ProspectRef;
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
        [Route("prospect/RemoveProspect/{application}/{userId}/{prospectId}")]
        public async Task<JsonResult> RemoveProspect(string application, string userId, string prospectId)
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

                    var rep = new RepositoryBase<Prospect>();
                    var removeProspect = rep.Find(prospectId);

                    if (removeProspect == null) throw new Exception("Prospect not found");

                    var repQuote = new RepositoryBase<CrmQuote>();
                    if (repQuote.AsQueryable().Any(x => x.Prospect.Id == prospectId))
                    {
                        throw new Exception("Quotes are using this Prospect. Unable to Delete.");
                    }

                    var repTeam = new RepositoryBase<Team>();
                    foreach (var team in repTeam.AsQueryable().ToList())
                    {
                        if (team.Prospects.Any(x => x.Id == prospectId))
                        {
                            foreach (var removeTeamProspectRef in team.Prospects.Where(x => x.Id == prospectId).ToList())
                            {
                                team.Prospects.Remove(removeTeamProspectRef);
                            }

                            repTeam.Upsert(team);
                        }
                    }
                    rep.RemoveOne(removeProspect);
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
        [Route("prospect/FindProspects/{application}/{userId}")]
        public async Task<JsonResult> FindProspects(string application, string userId)
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
                    var teamRef = crmUser.ViewConfig.Team;
                    var team = teamRef.AsTeam();

                    var prospects = team.Prospects.Select(x => x.AsProspect()).Where(x => x.Company == null).AsQueryable();

                    result.ProspectsFound = prospects.Select(x => x.AsProspectRef()).OrderBy(x => x.Name).ToList();
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
        [Route("company/GetFullProspect/{application}/{userId}/{prospectId}")]
        public async Task<JsonResult> GetFullProspect(string application, string userId, string prospectId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var prospect = _helperProspect.GetProspect(application, userId, prospectId);

                    result.ProspectModel = prospect.ProspectModel;
                    result.ProspectRef = prospect.ProspectRef;

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
        [Route("company/ConvertProspectIntoCompany/{application}/{userId}/{prospectId}/{companyId}")]
        public async Task<JsonResult> ConvertProspectIntoCompany(string application, string userId, string prospectId, string companyId)
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
                    var teamId = crmUser.ViewConfig.Team.Id;
                    _helperProspect.ConvertProspectIntoCompany(prospectId, companyId, teamId);
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
        [Route("company/GetProspectAddresses/{application}/{userId}/{prospectId}")]
        public async Task<JsonResult> GetProspectAddresses(string application, string userId, string prospectId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var prospect = _helperProspect.GetProspect(application, userId, prospectId);

                    result.ProspectRef = prospect.ProspectRef;
                    result.ProspectModel = prospect.ProspectModel;
                    result.PrimaryAddress = prospect.ProspectModel.Addresses.FirstOrDefault();
                    result.OtherAddresses = prospect.ProspectModel.Addresses;

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