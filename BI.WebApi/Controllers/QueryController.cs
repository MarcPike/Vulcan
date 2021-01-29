using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BI.DAL.Mongo.BiQueries;
using BI.DAL.Mongo.BiUserObjects;
using BI.DAL.Mongo.Helpers;
using BI.DAL.Mongo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BI.WebApi.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class QueryController : BaseController
    {
        private readonly IHelperQueries _helperQueries;
        /*
                 List<BiQueryBaseModel> GetMyQueriesForType(string userId, string type);
        BiQueryBaseModel SaveMyQuery(BiQueryBaseModel model);
        void RemoveMyQuery(string userId, string queryId);

         */

        public QueryController(IHelperQueries helperQueries)
        {
            _helperQueries = helperQueries;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("queries/GetMyQueriesForType/{type}")]
        public JsonResult GetMyQueriesForType(string type)
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();

            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var biUser = _helperUser.GetBiUser(tokenData.UserId);
                result.Queries = _helperQueries.GetMyQueriesForType(biUser.UserId, type);
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
        [Route("queries/ThrowException")]
        public JsonResult ThrowException()
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                throw new Exception("This is a test");
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                SetStatusToBadRequestIfNeeded(ref tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("queries/SaveMyQuery")]
        public JsonResult SaveMyQuery([FromBody] BiQueryBaseModel model)
        {
            dynamic result = new ExpandoObject();
            var tokenData = GetTokenDataFromHeaders();

            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BiQueryBaseModel = _helperQueries.SaveMyQuery(model);
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("queries/RemoveMyQuery")]
        public JsonResult RemoveMyQuery(string queryId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;

            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                _helperQueries.RemoveMyQuery(tokenData.UserId, queryId);

                result.Success = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                resultStatusCode = HttpStatusCode.NotAcceptable;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("queries/CreateQuery/{type}")]
        public JsonResult CreateQuery(string type)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;

            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var biUser = _helperUser.GetBiUser(tokenData.UserId);
                var query = new BiQueryBase()
                {
                    User = biUser.AsBiUserRef(),
                    QueryType = type
                };
                BiQueryBase.Helper.Upsert(query);
                biUser.Queries.Add(query);
                BiUser.Helper.Upsert(biUser);

                result.BiQueryBaseModel = _helperQueries.GetMyQuery(biUser.UserId, query.Id.ToString());

                result.Success = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                resultStatusCode = HttpStatusCode.NotAcceptable;
            }
            return JsonResultWithStatusCode(result, resultStatusCode);

        }


    }
}
