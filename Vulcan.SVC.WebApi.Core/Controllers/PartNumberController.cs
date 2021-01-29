using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vulcan.SVC.WebApi.Core.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    //[Consumes("application/json")]
    public class PartNumberController: BaseController
    {

        public PartNumberController(IHelperUser helperUser) : base(helperUser)
        {

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("partNumbers/GetPartNumbersForTeam/{application}/{userId}/{teamId}")]
        public async Task<JsonResult> GetPartNumbersForTeam(string application, string userId, string teamId)
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
                    //var quoteItems = repQuote.AsQueryable()
                    //    .Where(x => x.Team.Id == teamId)
                    //        .SelectMany(x=>x.Items.Select(i=>i.AsQuoteItem())
                    //        .Where(p => !string.IsNullOrEmpty(p.PartNumber) && p.QuotePrice != null).ToList()).ToList();

                    var quotes = repQuote.AsQueryable().Where(x => x.Team.Id == teamId).ToList();
                    var quoteItems = quotes.SelectMany(x => x.Items.Select(i => i.AsQuoteItem())
                        .Where(p => !string.IsNullOrEmpty(p.PartNumber) && p.QuotePrice != null).ToList()).ToList();

                    var partNumbers = quoteItems.GroupBy(info =>
                            new {
                                info.PartNumber,
                                info.QuotePrice.StartingProduct.ProductCode,
                                info.QuotePrice.StartingProduct.ProductId,

                            })
                        .Select(group => new { group.Key.PartNumber, group.Key.ProductCode, group.Key.ProductId, Count = group.Count() }).OrderBy(x => x.PartNumber)
                        .ToList();

                    result.Parts = partNumbers;
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
