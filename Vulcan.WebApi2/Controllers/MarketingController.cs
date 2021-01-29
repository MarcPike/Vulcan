using DAL.Marketing.Helpers;
using DAL.Marketing.Models;
using DAL.Vulcan.Mongo.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.TimeKeeper;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]

    public class MarketingController : BaseController
    {
        private IHelperCompany _helperCompany;
        private IHelperMarketing _helperMarketing;

        public MarketingController(IHelperCompany helperCompany, IHelperMarketing helperMarketing, IHelperUser helperUser) : base(helperUser)
        {
            _helperCompany = helperCompany;
            _helperMarketing = helperMarketing;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("marketing/ModuleAccessedByUser/{application}/{userId}")]
        public async Task<JsonResult> ModuleAccessedByUser(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
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
        [Route("marketing/GetAccountTypes/{application}/{userId}")]

        public async Task<JsonResult> GetAccountTypes(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.AccountTypes = _helperMarketing.GetAccountTypes();
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
        [Route("marketing/AddNewAccount/{application}/{userId}/{name}/{type}")]

        public async Task<JsonResult> AddNewAccount(string application, string userId, string name, string type)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var newAccountModel = _helperMarketing.AddNewAccount(application, userId, name, type);
                    result.AccountModel = newAccountModel;
                    result.MarketingAccountId = newAccountModel.Id;
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
        [Route("marketing/RemoveAccount/{application}/{userId}/{accountId}")]
        public async Task<JsonResult> RemoveAccount(string application, string userId, string accountId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperMarketing.RemoveAccount(accountId);
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
        [Route("marketing/GetAllAccounts/{application}/{userId}")]
        public async Task<JsonResult> GetAllAccounts(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var marketingAccounts = _helperMarketing.GetAllAccounts();
                    result.MarketingAccounts = marketingAccounts;
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
        [Route("marketing/GetAccount/{application}/{userId}/{accountId}")]
        public async Task<JsonResult> GetAccount(string application, string userId, string accountId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.AccountModel = _helperMarketing.GetAccount(application, userId, accountId);
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
        [Route("marketing/GetFolderInfo/{application}/{userId}/{accountId}/{folderId}")]
        public async Task<JsonResult> GetFolderInfo(string application, string userId, string accountId, string folderId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.AccountFolder =
                        _helperMarketing.GetMarketingAccountFolder(accountId, folderId);
                    result.FolderPath = _helperMarketing.GetFolderPath(accountId, folderId);
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
        [Route("marketing/GetFolder/{application}/{userId}/{accountId}/{folderId}")]
        public async Task<JsonResult> GetFolder(string application, string userId, string accountId, string folderId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.FolderModel =
                        _helperMarketing.GetFolder(application, userId, accountId, folderId);
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
        [Route("marketing/SaveFolder")]
        public async Task<JsonResult> SaveFolder([FromBody] MarketingAccountFolderModel model)
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

                    result.FolderModel =
                        _helperMarketing.SaveFolder(model);
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
        [Route("marketing/AddChildFolder/{application}/{userId}/{accountId}/{folderId}/{name}")]
        public async Task<JsonResult> AddChildFolder(string application, string userId, string accountId, string folderId, string name)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var newFolderInfo = _helperMarketing.AddChildFolder(application, userId, accountId, folderId, name);
                    result.AccountModel = newFolderInfo.Model;
                    result.NewFolder = newFolderInfo.Folder;
                    result.NewFolderPath = newFolderInfo.FolderPath;
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
        [Route("marketing/RemoveFolder/{application}/{userId}/{accountId}/{folderId}")]
        public async Task<JsonResult> RemoveFolder(string application, string userId, string accountId, string folderId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.AccountModel = _helperMarketing.RemoveFolder(application, userId, accountId, folderId);
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
        [Route("marketing/MoveFolder/{application}/{userId}/{accountId}/{moveFolderId}/{originalParentId}/{newParentId}")]
        public async Task<JsonResult> MoveFolder(string application, string userId, string accountId, string moveFolderId, string originalParentId,
            string newParentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.AccountModel =
                        _helperMarketing.MoveFolder(application, userId, accountId, moveFolderId, originalParentId, newParentId);
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
        [Route("marketing/GetAllCompanies/{application}/{userId}/{accountId}")]
        public async Task<JsonResult> GetAllCompanies(string application, string userId, string accountId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.Companies = _helperMarketing.GetAllCompanies(accountId);
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
        [Route("marketing/GetAllCompaniesForFolder/{application}/{userId}/{accountId}/{folderId}")]
        public async Task<JsonResult> GetAllCompaniesForFolder(string application, string userId, string accountId, string folderId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.Companies = _helperMarketing.GetAllCompaniesForFolder(accountId, folderId);
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
        [Route("marketing/SaveAccount")]
        public async Task<JsonResult> SaveAccount([FromBody] MarketingAccountModel model)
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
                    result.AccountModel = _helperMarketing.SaveAccount(model);
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
        [Route("marketing/GetNewMarketingSalesTeamModel/{application}/{userId}")]
        public async Task<JsonResult> GetNewMarketingSalesTeamModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.MarketingSalesTeamModel = _helperMarketing.GetNewMarketingSalesTeamModel(application, userId);
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
        [Route("marketing/SaveMarketingSalesTeam")]
        public async Task<JsonResult> SaveMarketingSalesTeam([FromBody] MarketingSalesTeamModel model)
        {
            dynamic result = new ExpandoObject();
            var statusCode = CheckToken(model.Application, model.UserId);

            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    result.MarketingSalesTeamModel = _helperMarketing.SaveMarketingSalesTeam(model);
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
        [Route("marketing/GetFolderPath/{application}/{userId}/{accountId}/{folderId}")]
        public async Task<JsonResult> GetFolderPath(string application, string userId, string accountId, string folderId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "Get folder path";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);
                        result.FolderPath = _helperMarketing.GetFolderPath(accountId, folderId);
                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetAllMarketingSalesTeams/{application}/{userId}")]
        public async Task<JsonResult> GetAllMarketingSalesTeams(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.MarketingSalesTeams = _helperMarketing.GetAllMarketingSalesTeams();
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
        [Route("marketing/TestGetQuotes/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}")]
        public async Task<JsonResult> TestGetQuotes(string application, string userId, string accountId, string folderId, DateTime fromDate, DateTime toDate)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "QuoteQuery for Marketing";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        result.TestQuoteModels = _helperMarketing.TestGetQuotes(accountId, folderId, fromDate, toDate);

                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/TestGetCompaniesForFolder/{application}/{userId}/{accountId}/{folderId}")]
        public async Task<JsonResult> TestGetCompaniesForFolder(string application, string userId, string accountId, string folderId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "QuoteQuery for Marketing";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        result.Companies = _helperMarketing.TestGetCompaniesForFolder(accountId, folderId, DateTime.Parse("1/1/2018"), DateTime.Parse("1/1/2018"));
                        result.Count = result.Companies.Count;
                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/HitRateBySalesPerson/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}")]
        public async Task<JsonResult> HitRateBySalesPerson(string application, string userId, string accountId, string folderId, DateTime fromDate, DateTime toDate)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "ChartData for HitRateBySalesPerson";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        result.ChartData = _helperMarketing.HitRateBySalesPerson(accountId, folderId, fromDate, toDate);
                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/HitRateByMetalCategory/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}")]
        public async Task<JsonResult> HitRateByMetalCategory(string application, string userId, string accountId, string folderId, DateTime fromDate,
            DateTime toDate)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "ChartData for HitRateByMetalCategory";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        result.ChartData =
                            _helperMarketing.HitRateByMetalCategory(accountId, folderId, fromDate, toDate);
                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/HitRateByCustomer/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}")]
        public async Task<JsonResult> HitRateByCustomer(string application, string userId, string accountId, string folderId, DateTime fromDate, DateTime toDate)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "ChartData for HitRateByCustomer";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        result.ChartData =
                            _helperMarketing.HitRateByCustomer(accountId, folderId, fromDate, toDate);
                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/TotalDollarsBySalesPerson/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> TotalDollarsBySalesPerson(string application, string userId, string accountId, string folderId, DateTime fromDate,
            DateTime toDate, string displayCurrency)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "ChartData for TotalDollarsBySalesPerson";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        result.ChartData =
                            _helperMarketing.TotalDollarsBySalesPerson(accountId, folderId, fromDate, toDate,
                                displayCurrency);
                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/TotalDollarsByMetalCategory/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> TotalDollarsByMetalCategory(string application, string userId, string accountId, string folderId, DateTime fromDate,
            DateTime toDate, string displayCurrency)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "ChartData for TotalDollarsByMetalCategory";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        result.ChartData =
                            _helperMarketing.TotalDollarsByMetalCategory(accountId, folderId, fromDate, toDate,
                                displayCurrency);
                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/TotalDollarsByCustomer/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> TotalDollarsByCustomer(string application, string userId, string accountId, string folderId, DateTime fromDate, DateTime toDate,
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
                    var timedAction = "ChartData for TotalDollarsByCustomer";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        result.ChartData =
                            _helperMarketing.TotalDollarsByCustomer(accountId, folderId, fromDate, toDate,
                                displayCurrency);
                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetQuotesTimeline/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> GetQuotesTimeline(string application, string userId, string accountId, string folderId, DateTime fromDate, DateTime toDate,
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
                    var timedAction = "ChartData for TotalDollarsByCustomer";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        result.ChartQuoteHistoryModel = _helperMarketing.GetQuotesTimeline(accountId, folderId, fromDate, toDate, displayCurrency);

                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetSalesPersonModels/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> GetSalesPersonModels(string application, string userId, string accountId, string folderId, DateTime fromDate,
            DateTime toDate,
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
                    var timedAction = "ChartData for GetSalesPersonModels";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        var models = _helperMarketing.GetSalesPersonModels(accountId, folderId, fromDate, toDate, displayCurrency);

                        result.HitRateBySalesPersonChartData = models.HitRateBySalesPersonChartData;
                        result.TotalDollarsBySalesPersonChartData = models.TotalDollarsBySalesPersonChartData;
                        result.MaterialMarginBySalesPersonChartData = models.MaterialMargin;
                        result.SellingMarginBySalesPersonChartData = models.SellingMargin;


                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetCustomerModels/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> GetCustomerModels(string application, string userId, string accountId, string folderId, DateTime fromDate,
            DateTime toDate, string displayCurrency)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "ChartData for GetCustomerModels";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        var models = _helperMarketing.GetCustomerModels(accountId, folderId, fromDate, toDate, displayCurrency);

                        result.HitRateByCustomerChartData = models.HitRateByCustomerChartData;
                        result.TotalDollarsByCustomerChartData = models.TotalDollarsByCustomerChartData;
                        result.MaterialMarginsByCompanyChartData = models.MaterialMargins;
                        result.SellingMarginsByCompanyChartData = models.SellingMargins;

                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetAllChartDataForCompany/{application}/{userId}/{companyId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> GetAllChartDataForCompany(string application, string userId, string companyId, DateTime fromDate,
            DateTime toDate, string displayCurrency)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "ChartData for GetAllChartDataForCompany";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        var models = _helperMarketing.GetAllChartDataForCompany(companyId, fromDate, toDate, displayCurrency);

                        result.TimelineData = models.TimelineData;
                        result.TotalDollarsBySalesPerson = models.TotalDollarsBySalesPerson;
                        result.TotalDollarsByMetalCategory = models.TotalDollarsByMetalCategory;

                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetAllChartDataForCompanyAsync/{application}/{userId}/{companyId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> GetAllChartDataForCompanyAsync(string application, string userId, string companyId, DateTime fromDate, DateTime toDate, string displayCurrency)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "ChartData for GetAllChartDataForCompanyAsync";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        var models = _helperMarketing.GetAllChartDataForCompany(companyId, fromDate, toDate, displayCurrency);

                        result.TimelineData = models.TimelineData;
                        result.TotalDollarsBySalesPerson = models.TotalDollarsBySalesPerson;
                        result.TotalDollarsByMetalCategory = models.TotalDollarsByMetalCategory;

                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetMaterialModels/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> GetMaterialModels(string application, string userId, string accountId, string folderId, DateTime fromDate,
            DateTime toDate,
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
                    var timedAction = "ChartData for GetMaterialModels";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        var models = _helperMarketing.GetMaterialModels(accountId, folderId, fromDate, toDate, displayCurrency);

                        result.HitRateByMetalCategoryChartData = models.HitRateByMetalCategoryChartData;
                        result.TotalDollarsByMetalCategoryChartData = models.TotalDollarsByMetalCategoryChartData;
                        result.MaterialMarginByMetalCategoryChartData = models.MaterialMargin;
                        result.SellingMarginByMetalCategoryChartData = models.SellingMargin;

                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetTimeLineAndSalesPersonModels/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> GetTimeLineAndSalesPersonModels(string application, string userId, string accountId, string folderId, DateTime fromDate,
            DateTime toDate,
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
                    var timedAction = "ChartData for GetTimeLineAndSalesPersonModels";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        var models = _helperMarketing.GetTimelineAndSalesPersonModels(accountId, folderId, fromDate, toDate, displayCurrency);

                        result.ChartQuoteHistoryModel = models.ChartQuoteHistoryModel;
                        result.HitRateBySalesPersonChartData = models.HitRateBySalesPersonChartData;
                        result.TotalDollarsBySalesPersonChartData = models.TotalDollarsBySalesPersonChartData;
                        result.MaterialMarginBySalesPersonChartData = models.MaterialMargin;
                        result.SellingMarginBySalesPersonChartData = models.SellingMargin;
                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetTimeLineAndMaterialModels/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> GetTimeLineAndMaterialModels(string application, string userId, string accountId, string folderId, DateTime fromDate,
            DateTime toDate,
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
                    var timedAction = "ChartData for GetTimeLineAndMaterialModels";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        var models = _helperMarketing.GetTimelineAndMaterialModels(accountId, folderId, fromDate, toDate, displayCurrency);

                        result.ChartQuoteHistoryModel = models.ChartQuoteHistoryModel;
                        result.HitRateByMetalCategoryChartData = models.HitRateByMetalCategoryChartData;
                        result.TotalDollarsByMetalCategoryChartData = models.TotalDollarsByMetalCategoryChartData;
                        result.MaterialMarginByMetalCategoryChartData = models.MaterialMargin;
                        result.SellingMarginByMetalCategoryChartData = models.SellingMargin;

                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetTimeLineAndCustomerModels/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}/{displayCurrency}")]
        public async Task<JsonResult> GetTimeLineAndCustomerModels(string application, string userId, string accountId, string folderId, DateTime fromDate,
            DateTime toDate,
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
                    var timedAction = "ChartData for GetTimeLineAndCustomerModels";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        var models = _helperMarketing.GetTimelineAndCustomerModels(accountId, folderId, fromDate, toDate, displayCurrency);

                        result.ChartQuoteHistoryModel = models.ChartQuoteHistoryModel;
                        result.HitRateByCustomerChartData = models.HitRateByCustomerChartData;
                        result.TotalDollarsByCustomerChartData = models.TotalDollarsByCustomerChartData;
                        result.MaterialMarginsByCustomerChartData = models.MaterialMargins;
                        result.SellingMarginsByCustomerChartData = models.SellingMargins;

                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetMarginBySalesPersonChartData/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}")]
        public async Task<JsonResult> GetMarginBySalesPersonChartData(string application, string userId, string accountId, string folderId, DateTime fromDate,
            DateTime toDate)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "ChartData for GetMarginBySalesPersonChartData";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        result.ChartData =
                            _helperMarketing.GetMarginBySalesPersonChartData(accountId, folderId, fromDate, toDate);
                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetMarginByMetalCategoryChartData/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}")]
        public async Task<JsonResult> GetMarginByMetalCategoryChartData(string application, string userId, string accountId, string folderId, DateTime fromDate,
            DateTime toDate)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "ChartData for GetMarginByMetalCategoryChartData";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        result.ChartData =
                            _helperMarketing.GetMarginByMetalCategoryChartData(accountId, folderId, fromDate, toDate);
                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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
        [Route("marketing/GetMarginByCustomerChartData/{application}/{userId}/{accountId}/{folderId}/{fromDate}/{toDate}")]
        public async Task<JsonResult> GetMarginByCustomerChartData(string application, string userId, string accountId, string folderId, DateTime fromDate,
            DateTime toDate)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var timedAction = "ChartData for GetMarginByCustomerChartData";
                    using (var timeKeeper = new TimeKeeper())
                    {
                        timeKeeper.StartAction(timedAction);

                        result.ChartData =
                            _helperMarketing.GetMarginByCustomerChartData(accountId, folderId, fromDate, toDate);
                        timeKeeper.StopAction(timedAction);
                        result.ElapsedTimeMilliseconds = timeKeeper.GetTotalElapsed(timedAction).Milliseconds;
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

    }

}



