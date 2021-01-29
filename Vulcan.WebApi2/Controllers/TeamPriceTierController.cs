using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.TeamSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class TeamPriceTierController : BaseController
    {
        private readonly IHelperTeamPriceTier _helperTeamPriceTier;

        public TeamPriceTierController(IHelperUser helperUser, IHelperTeamPriceTier helperTeamPriceTier) : base(helperUser)
        {
            _helperTeamPriceTier = helperTeamPriceTier;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("teamPriceTier/GetTeamPriceTierModel/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> GetTeamPriceTierModel(string application, string userId, string teamId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.TeamPriceTierModel = _helperTeamPriceTier.GetTeamPriceTierModel(teamId, application, userId);
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
        [Route("teamPriceTier/CloneTeamPriceDefinition/{application}/{userId}/{teamPriceDefinitionId}/{newStockGrade}/{newCondition}")]
        public async Task<JsonResult> CloneTeamPriceDefinition(string application, string userId, string teamPriceDefinitionId, string newStockGrade, string newCondition)
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
                    var teamPriceTier = new RepositoryBase<TeamPriceTier>().AsQueryable()
                        .FirstOrDefault(x => x.Team.Id == crmUser.ViewConfig.Team.Id);
                    if (teamPriceTier == null) throw new Exception("Team Price Tier was now found");
                    var priceDef =
                        teamPriceTier.PriceDefinitions.FirstOrDefault(x => x.Id == Guid.Parse(teamPriceDefinitionId));
                    if (priceDef == null) throw new Exception("Price Definition not found");
                    result.NewPriceDefinitionModel = priceDef.Clone(newStockGrade, newCondition);
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
        [Route("teamPriceTier/GetNewPriceDefinitionModel/{application}/{userId}")]
        public async Task<JsonResult> GetNewPriceDefinitionModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.TierPriceDefinitionModel = _helperTeamPriceTier.GetNewPriceDefinitionModel();
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
        [Route("teamPriceTier/GetNewBasePriceDimensionModel/{application}/{userId}")]
        public async Task<JsonResult> GetNewBasePriceDimensionModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.BasePriceDimensionModel = _helperTeamPriceTier.GetNewBasePriceDimensionModel();
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
        [Route("teamPriceTier/GetNewWeightDiscountModel/{application}/{userId}")]
        public async Task<JsonResult> GetNewWeightDiscountModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.NewWeightDiscountModel = _helperTeamPriceTier.GetNewWeightDiscountModel();
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
        [Route("teamPriceTier/SaveTeamPriceTier")]
        public async Task<JsonResult> SaveTeamPriceTier([FromBody] TeamPriceTierModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.TeamPriceTierModel = _helperTeamPriceTier.SaveTeamPriceTier(model);
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
