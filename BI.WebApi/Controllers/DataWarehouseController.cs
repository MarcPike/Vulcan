using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BI.DAL.DataWarehouse.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BI.WebApi.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class DataWarehouseController : BaseController
    {
        private readonly IHelperOds _helperOds;

        public DataWarehouseController(IHelperOds helperOds)
        {
            _helperOds = helperOds;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dataWarehouse/GetCoidList")]
        public JsonResult GetCoidList()
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                result.CoidList = _helperOds.GetCoidList();
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dataWarehouse/RefreshWarehouseCache")]
        public JsonResult RefreshWarehouseCache()
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                result.WarehouseCacheInfo = _helperOds.RefreshWarehouseCache();
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dataWarehouse/GetWarehousesForCoids/{coids}/{refreshCache}")]
        public JsonResult GetWarehousesForCoids(string coids, bool refreshCache)
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var coidList = coids.Split(",").ToList();
                result.Warehouses = _helperOds.GetWarehousesForCoids(coidList, refreshCache);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("dataWarehouse/GetBusinessRegionWarehousesForCoids/{coids}")]
        public JsonResult GetBusinessRegionWarehousesForCoids(string coids)
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var coidList = coids.Split(",").ToList();
                result.Warehouses = _helperOds.GetBusinessUnitWarehouses(coidList);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }



        [AllowAnonymous]
        [HttpGet]
        [Route("dataWarehouse/GetBranchesForCoids/{coids}")]
        public JsonResult GetBranchesForCoids(string coids)
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var coidList = coids.Split(",").ToList();
                result.Branches = _helperOds.GetBranchList(coidList);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }


    }
}
