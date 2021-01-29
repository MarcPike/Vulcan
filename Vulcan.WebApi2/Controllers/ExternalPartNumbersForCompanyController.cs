using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DAL.QNG.Reader;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.DocClass.Companies;
using DAL.Vulcan.Mongo.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Vulcan.IMetal.Queries.ProductCodes;
using Vulcan.IMetal.Queries.PurchaseOrderItems;
using Vulcan.IMetal.Queries.StockItems;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class ExternalPartNumbersForCompanyController : BaseController
    {
        public ExternalPartNumbersForCompanyController(IHelperUser helperUser) : base(helperUser)
        {
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("companyPortal/GetExternalPartNumbersForCompany/{coid}/{companyNameStartsWith}")]
        public async Task<JsonResult> GetExternalPartNumbersForCompany(string coid, string companyNameStartsWith)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            try
            {
                await Task.Run(() =>
                {
                    result.PartNumbers = GetProducts.GetProductsFor(coid, companyNameStartsWith);
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
        [Route("companyPortal/GetCompaniesStartingWith/{coid}/{companyNameStartsWith}")]
        public async Task<JsonResult> GetCompaniesStartingWith(string coid, string companyNameStartsWith)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            try
            {
                await Task.Run(() =>
                {
                    var queryHelper = new MongoRawQueryHelper<Company>();
                    var companies = queryHelper
                        .Find(queryHelper.FilterBuilder.Where(x => x.Name.Contains(companyNameStartsWith))).ToList();


                    result.Companies = companies.OrderBy(x => x.Name).Select(x =>
                        new
                        {
                            Id = x.Id.ToString(),
                            x.Code,
                            x.Name
                        }).ToList();

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
        [Route("companyPortal/GetQuotesForCompany/{begDate}/{endDate}/{companyId}/{showExpired}")]
        public async Task<JsonResult> GetQuotesForCompany(DateTime begDate, DateTime endDate, string companyId,
            bool showExpired)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            result.Success = false;
            var helperQuote = new HelperQuote();
            try
            {
                await Task.Run(() =>
                {
                    var quotePipeline =
                        helperQuote.GetQuotesPipelineForCompany(begDate, endDate, companyId, showExpired);
                    var quotePipelineData = quotePipeline.Drafts;
                    quotePipelineData.AddRange(quotePipeline.Pending);
                    quotePipelineData.AddRange(quotePipeline.Won);
                    quotePipelineData.AddRange(quotePipeline.Lost);
                    quotePipelineData.AddRange(quotePipeline.Expired);

                    result.QuotePipelineData = quotePipelineData;
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
        [Route("companyPortal/GetAvailableProducts/{coid}")]
        public async Task<JsonResult> GetAvailableProducts(string coid)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            try
            {
                await Task.Run(() =>
                {
                    var uniqueProducts = StockItemsAdvancedQuery.AsQueryable(coid).Where(x => x.AvailableWeight > 0)
                        .Select(x => new
                        {
                            x.Coid,
                            x.ProductId,
                            x.ProductCode,
                            x.OuterDiameter,
                            x.InsideDiameter,
                            x.MetalCategory,
                            x.ProductCondition,
                            x.StockGrade
                        }).Distinct().ToList();
                    result.AvailableProducts = uniqueProducts.OrderBy(x => x.ProductCode);
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
        [Route("companyPortal/GetAvailableIncomingProducts/{coid}")]
        public async Task<JsonResult> GetAvailableIncomingProducts(string coid)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            try
            {
                await Task.Run(() =>
                {
                    var uniqueProducts = PurchaseOrderItemsAdvancedQuery.AsQueryable(coid)
                        .Select(x => new
                        {
                            x.Coid,
                            x.ProductId,
                            x.ProductCode,
                            x.OuterDiameter,
                            x.InsideDiameter,
                            x.MetalCategory,
                            x.ProductCondition,
                            x.StockGrade
                        }).Distinct().ToList();

                    result.AvailableIncomingProducts = uniqueProducts.OrderBy(x => x.ProductCode);
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
        [Route("companyPortal/GetAllHowcoProducts/{coid}")]
        public async Task<JsonResult> GetAllHowcoProducts(string coid)
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;

            try
            {
                await Task.Run(() =>
                {
                    result.AllHowcoProducts = ProductMasterAdvancedQuery.AsQueryable(coid).ToList();
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