using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Croz.Models;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.WebApi2.Controllers
{
    /*
        void RemoveCrozGradeList(string region);
        void RemoveCrozProductionCostList(string region);

     */


    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class CrozController : BaseController
    {
        private IHelperCroz HelperCroz { get; }

        public CrozController(IHelperUser helperUser, IHelperCroz helperCroz) : base(helperUser)
        {
            HelperCroz = helperCroz;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("croz/SaveCrozCalcItem")]
        public async Task<JsonResult> SaveCrozCalcItem([FromBody] CrozCalcItemModel model)
        {

            dynamic result = new ExpandoObject();
            result.Success = false;
            try
            {
                CheckForModelErrors();
            }
            catch (Exception e)
            {

                result.ErrorMessage = e.Message;
                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);

            }

            var statusCode = CheckToken("vulcancrm", model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.CrozCalcItem = HelperCroz.SaveCalcItemModel(model);
                    statusCode = HttpStatusCode.OK;
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
        [Route("croz/SaveGradeList")]
        public async Task<JsonResult> SaveGradeList([FromBody] CrozGradeListModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", model.UserToken);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.GradeListModel = HelperCroz.SaveCrozGradeList(model);
                    statusCode = HttpStatusCode.OK;
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
        [Route("croz/SaveProductionCostList")]
        public async Task<JsonResult> SaveProductionCostList([FromBody] CrozProductionCostListModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", model.UserToken);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ProductionCostListModel = HelperCroz.SaveCrozProductionCostList(model);
                    statusCode = HttpStatusCode.OK;
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
        [Route("croz/GetGradeListForRegion/{userToken}/{region}")]
        public async Task<JsonResult> GetGradeListForRegion(string userToken, string region)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", userToken);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.GradeListModel = HelperCroz.GetCrozGradeListForRegion(userToken, region);
                    statusCode = HttpStatusCode.OK;
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
        [Route("croz/GetProductionCostListForRegion/{userToken}/{region}")]
        public async Task<JsonResult> GetProductionCostListForRegion(string userToken, string region)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", userToken);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ProductionCostListModel = HelperCroz.GetCrozProductionCostListForRegion(userToken, region);
                    statusCode = HttpStatusCode.OK;
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
        [Route("croz/GetAllGradeLists/{userToken}")]
        public async Task<JsonResult> GetAllGradeLists(string userToken)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", userToken);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.GradeListModels = HelperCroz.GetAllCrozGradeLists(userToken);
                    statusCode = HttpStatusCode.OK;
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
        [Route("croz/GetAllProductionCostLists/{userToken}")]
        public async Task<JsonResult> GetAllProductionCostLists(string userToken)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", userToken);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ProductionCostListModels = HelperCroz.GetAllCrozProductionCostLists(userToken);
                    statusCode = HttpStatusCode.OK;
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
        [Route("croz/RemoveGradeList/{userToken}/{region}")]
        public async Task<JsonResult> RemoveGradeList(string userToken, string region)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", userToken);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    HelperCroz.RemoveCrozGradeList(region);
                    statusCode = HttpStatusCode.OK;
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
        [Route("croz/RemoveProductionCostList/{userToken}/{region}")]
        public async Task<JsonResult> RemoveProductionCostList(string userToken, string region)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken("vulcancrm", userToken);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    HelperCroz.RemoveCrozProductionCostList(region);
                    statusCode = HttpStatusCode.OK;
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

    }
}
