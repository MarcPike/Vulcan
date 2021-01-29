using DAL.Osiris;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.MetalogicsCacheData;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.Quotes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Vulcan.IMetal.Cache;
using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.Orders;
using Vulcan.IMetal.Helpers;
using Vulcan.IMetal.Queries.GeneralInfo;
using Vulcan.IMetal.Queries.Orders;
using Vulcan.IMetal.Queries.PurchaseOrderItems;
using Vulcan.IMetal.Queries.StockItems;
using Vulcan.IMetal.Queries.Warehouses;
using Vulcan.WebApi2.Models;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;
using PurchaseOrderItemsCache = DAL.Vulcan.Mongo.MetalogicsCacheData.PurchaseOrderItemsCache;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    //[Produces(@"application/json",@"application/pdf")]
    [Produces(@"application/json")]
    //[Consumes(@"application/json")]
    public class IMetalController: BaseController
    {
        private readonly IHelperApplication _helperApplication;
        private readonly IHelperCompany _helperCompany;
        //private readonly IStockItemsCache _stockItemsCache;
        //private readonly IPurchaseOrderItemsCache _purchaseOrderItemsCache;
        private readonly IHelperFile _helperFile;
        //private readonly IProductMasterCache _productMasterCache;
        private readonly IHelperCurrencyForIMetal _helperCurrencyForIMetal;

        private List<ProductsWithNoInventoryModel> NonStockItemsCache { get; set; } = new List<ProductsWithNoInventoryModel>();
        private DateTime NonStockItemsCacheTime { get; set; } = new DateTime(1980,1,1);

        public IMetalController(
            IHelperApplication helperApplication, 
            IHelperUser helperUser, 
            IHelperCompany helperCompany, 
            //IStockItemsCache stockItemsCache,
            //IStockItemsCache machinedComponentsCache,
            //IPurchaseOrderItemsCache purchaseOrderItemsCache,
            //IProductMasterCache productMasterCache,
            IHelperFile helperFile,
            IHelperCurrencyForIMetal helperCurrencyForIMetal) : base(helperUser)
        {
            _helperApplication = helperApplication;
            _helperCompany = helperCompany;
            //_stockItemsCache = stockItemsCache;
            //_purchaseOrderItemsCache = purchaseOrderItemsCache;
            _helperFile = helperFile;
            //_productMasterCache = productMasterCache;
            _helperCurrencyForIMetal = helperCurrencyForIMetal;
        }

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("imetal/debugStockItem/{coid}/{productId}")]
        //public ActionResult DebugStockItem(string coid, int productId)
        //{
        //    var stockItem = _stockItemsCache.GetForCoid(coid, false).StockItems.Where(x => x.ProductId == productId);

        //    var debugInfo = ObjectDumper.Dump(stockItem);
        //    byte[] bytes = Encoding.ASCII.GetBytes(debugInfo);
        //    char[] chars = Encoding.ASCII.GetChars(bytes);
        //    string resultText = new String(chars).Replace("?", "");

        //    return Content(debugInfo);
        //}

        [HttpGet]
        [AllowAnonymous]
        [Route(
            "imetal/GetExchangeRateForCOIDtoDisplayCurrency/{application}/{userId}/{coid}/{displayCurrency}")]
        public async Task<JsonResult> GetExchangeRateForCoidToDisplayCurrency(string application, string userId, string coid, string displayCurrency)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var fromCurrency = _helperCurrencyForIMetal.GetDefaultCurrencyForCoid(coid);
                    result.ExchangeRate =
                        _helperCurrencyForIMetal.ConvertValueFromCurrencyToCurrency(1, fromCurrency, displayCurrency);

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
            "imetal/GetListOfSupportedCurrencyTypes/{application}/{userId}")]
        public async Task<JsonResult> GetListOfSupportedCurrencyTypes(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.CurrencyTypes = _helperCurrencyForIMetal.GetSupportedDisplayCurrencyCodes();
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
        [Route("imetal/GetEstimatedWeightOfMadeupProduct/{application}/{userId}/{outerDiameter}/{insideDiameter}/{metalCategory}/{pieces}/{quantity}/{quantityType}")]
        public async Task<JsonResult> GetEstimatedWeightOfMadeupProduct(string application, string userId, decimal outerDiameter,
            decimal insideDiameter, string metalCategory, int pieces, decimal quantity, string quantityType)
        {

            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var orderQuantity = new OrderQuantity(pieces, quantity, quantityType);
                    metalCategory = metalCategory.ToUpper();
                    var weightTotals = MadeUpCost.CalculateEstimatedWeight(outerDiameter, insideDiameter, metalCategory, orderQuantity);

                    result.TotalPounds = weightTotals.TotalPounds;
                    result.TotalKilograms = weightTotals.TotalKilograms;

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
        //[Route("imetal/GetStockItemsCacheInfoForCoid/{application}/{userId}/{coid}")]
        //public async Task<JsonResult> GetStockItemsCacheInfoForCoid(string application, string userId, string coid)
        //{


        //    dynamic result = new ExpandoObject();
        //    result.Success = false;

        //    var statusCode = CheckToken(application, userId);
        //    try
        //    {
        //        await Task.Run(() =>
        //        {
        //            ThrowExceptionForBadToken(statusCode);

        //            if (!CheckCoid(coid))
        //            {
        //                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
        //                throw new Exception("[{coid}] is not a valid Coid");
        //            }

        //            result.CacheInfo = _stockItemsCache.GetInfoForCoid(coid);
        //            result.Success = true;
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
        //        result.ErrorMessage = e.Message;
        //        result.Success = false;
        //    }

        //    return JsonResultWithStatusCode(result, statusCode);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //[Route("imetal/GetPurchaseOrderItemsCacheInfoForCoid/{application}/{userId}/{coid}")]
        //public async Task<JsonResult> GetPurchaseOrderItemsCacheInfoForCoid(string application, string userId, string coid)
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var statusCode = CheckToken(application, userId);
        //    try
        //    {
        //        await Task.Run(() =>
        //        {
        //            ThrowExceptionForBadToken(statusCode);
        //            result.CacheInfo = _purchaseOrderItemsCache.GetInfoForCoid(coid);


        //            result.Success = true;
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
        //        result.ErrorMessage = e.Message;
        //        result.Success = false;
        //    }

        //    return JsonResultWithStatusCode(result, statusCode);
        //}


        [HttpGet]
        [AllowAnonymous]
        [Route("imetal/GetProductMastersForCoid/{application}/{userId}/{coid}")]

        public async Task<JsonResult> GetProductMastersForCoid(string application, string userId, string coid)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    if (!CheckCoid(coid)) throw new Exception($"Invalid COID = {coid}");



                    //result.ProductCodes = _productMasterCache.GetForCoid(coid, false).Values;
                    var cacheId = CacheSettings.GetCurrentProductMastersCacheId();
                    result.ProductCodes = ProductMastersCache.Helper.Find(x => x.Coid == coid && x.CacheId == cacheId)
                        .ToList().OrderBy(x => x.ProductCode).ToList();

                    //result.ProductCodes = ProductMasterAdvancedQuery.AsQueryable(coid).Select(x => x).ToList().OrderBy(x => x.ProductCode).ToList();
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
        [Route("imetal/GetProductCodesForCoid/{application}/{userId}/{coid}")]

        public async Task<JsonResult> GetProductCodesForCoid(string application, string userId, string coid)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    if (!CheckCoid(coid)) throw new Exception($"Invalid COID = {coid}");
                    ThrowExceptionForBadToken(statusCode);
                    var cacheId = CacheSettings.GetCurrentProductMastersCacheId();
                    var filter =
                        ProductMastersCache.Helper.FilterBuilder.
                            Where(x => x.Coid == coid && x.CacheId == cacheId);
                    var project = ProductMastersCache.Helper.ProjectionBuilder.
                        Expression(x => x.ProductCode);

                    result.ProductCodes = ProductMastersCache.Helper.
                        FindWithProjection(filter, project).ToList().
                        OrderBy(x=> x).ToList();
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
        [Route("imetal/LoadNewSalesGroupsFromIMetal/{application}/{userId}")]
        public async Task<JsonResult> LoadNewSalesGroupsFromIMetal(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var coidList = new List<string>() { "INC", "CAN", "MSA", "SIN", "EUR", "DUB" };
                    var rep = new RepositoryBase<DAL.Vulcan.Mongo.DocClass.CRM.SalesGroup>();

                    //rep.RemoveAllFromCollection();

                    var newSalesGroupsAdded = new List<DAL.Vulcan.Mongo.DocClass.CRM.SalesGroup>();

                    foreach (var coid in coidList)
                    {
                        var salesGroups = SalesGroupQuery.GetForCoid(coid);

                        foreach (var salesGroup in salesGroups)
                        {
                            if (!rep.AsQueryable().Any(x => x.Coid == coid && x.Code == salesGroup.Code))
                            {
                                var newSalesGroup = new DAL.Vulcan.Mongo.DocClass.CRM.SalesGroup()
                                {
                                    Coid = coid,
                                    Code = salesGroup.Code,
                                    Description = salesGroup.Description
                                };
                                rep.Upsert(newSalesGroup);
                                newSalesGroupsAdded.Add(newSalesGroup);
                            }
                        }
                    }
                    result.Success = true;
                    result.SalesGroupsAdded = newSalesGroupsAdded;
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
        [Route("imetal/GetPartNumbersForCoid/{application}/{userId}/{coid}")]
        public async Task<JsonResult> GetPartNumbersForCoid(string application, string userId, string coid)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var partNumbers = new RepositoryBase<CrmQuoteItem>().AsQueryable().Where(x => x.Coid == coid && x.PartNumber != string.Empty).GroupBy(info => info.PartNumber)
                        .Select(group => new { PartNumber = group.Key, QuotedCounter = group.Count() }).OrderBy(x => x.PartNumber)
                        .ToList();

                    result.PartNumbers = partNumbers;
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
        [Route("imetal/GetProductsWithNoInventory/{application}/{userId}/{coid}/{forceReload}")]
        public async Task<JsonResult> GetProductsWithNoInventory(string application, string userId, string coid, bool forceReload)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    if ((NonStockItemsCacheTime.AddMinutes(180) > DateTime.Now) || (forceReload == true))
                    {
                        result.NonStockItems = NonStockItemsCache.OrderBy(x => x.ProductCode);
                    }
                    else
                    {

                        NonStockItemsCache = new List<ProductsWithNoInventoryModel>();

                        var cacheIdProductMasters = CacheSettings.GetCurrentProductMastersCacheId();
                        var cacheIdStockItems = CacheSettings.GetCurrentStockItemsCacheId();
                        var cacheIdIncoming = CacheSettings.GetCurrentPurchaseOrderItemsCacheId();

                        var productMasters= ProductMastersCache.Helper.Find(x => x.CacheId == cacheIdProductMasters && x.Coid == coid).ToList().Where(x=>!x.StockType.Contains("Machined")).ToList();

                        var stockItemsProductIds = DAL.Vulcan.Mongo.MetalogicsCacheData.StockItemsCache.Helper
                            .Find(x => x.CacheId == cacheIdStockItems && x.AvailableQuantity > 0 && x.Coid == coid).ToList()
                            .Select(x => x.ProductId).ToList();

                        var purchaseOrderItemsProductIds = PurchaseOrderItemsCache.Helper
                            .Find(x => x.CacheId == cacheIdIncoming && x.Coid == coid).ToList().Select(x => x.ProductId).ToList();


                        foreach (var productMaster in productMasters)
                        {
                            var productId = productMaster.ProductId;
                            if ((stockItemsProductIds.All(x => x != productId)) && (purchaseOrderItemsProductIds.All(x => x != productId)))
                            {
                                NonStockItemsCache.Add(new ProductsWithNoInventoryModel()
                                {
                                    Coid = coid,
                                    ProductId = productId,
                                    ProductCode = productMaster.ProductCode,
                                    InsideDiameter = productMaster.InsideDiameter,
                                    OuterDiameter = productMaster.OuterDiameter,
                                    ProductCondition = productMaster.ProductCondition,
                                    ProductCategory = productMaster.ProductCategory,
                                    ProductSize = productMaster.ProductSize,
                                    MetalType = productMaster.MetalType,
                                    StockGrade = productMaster.StockGrade,
                                    StratificationRank = productMaster.StratificationRank
                                });
                            }
                        }
                        result.NonStockItems = NonStockItemsCache.OrderBy(x => x.ProductCode);
                        //result.ExchangeRates = ExchangeRate.GetRateList();

                        NonStockItemsCacheTime = DateTime.Now;
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
        [Route("imetal/GetOrdersForUserByDueDate/{application}/{userId}/{dueDateMin}/{dueDateMax}")]
        public async Task<JsonResult> GetOrdersForUserByDueDate(string application, string userId, DateTime dueDateMin, DateTime dueDateMax)
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
                    var user = GetUser(userId);
                    var locationRef = user.Location;
                    var location = locationRef.AsLocation();
                    var coid = location.GetCoid();
                    var crmUser = GetCrmUser(application, userId);
                    var companies = _helperCompany.GetCompaniesForUser(crmUser);

                    var context = ContextFactory.GetOrdersContextForCoid(coid, true);
                    {
                        //context.Connection.Open();

                        var companyCodes = companies.Select(x => x.Code).ToList();
                        var queryOrders = OrdersAdvancedQuery.AsQueryable(coid);

                        queryOrders = queryOrders.Where(x => companyCodes.Contains(x.CompanyCode));

                        queryOrders = queryOrders.Where(x => x.OrderDueDate >= dueDateMin && x.OrderDueDate <= dueDateMax);
                        queryOrders = queryOrders.Where(x => x.OrderSaleTypeCode == "ORD");
                        var results = queryOrders.ToList();

                        List<CompanyListModel> companyTotals = new List<CompanyListModel>();
                        List<SalesPersonListModel> salesPersonTotals = new List<SalesPersonListModel>();

                        var model = new List<OrderListModel>();
                        ProcessOrdersAccumulateTotals(results, coid, context, model, companyTotals, salesPersonTotals);
                        result.SalesPersonTotals = salesPersonTotals;
                        result.CompanyTotals = companyTotals;

                        result.Orders = model.OrderBy(x => x.DueDate);

                        result.Success = true;
                        //context.Connection.Close();

                    }

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
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Route("imetal/GetOrdersForUserBySaleDate/{application}/{userId}/{saleDateMin}/{saleDateMax}")]
        public async Task<JsonResult> GetOrdersForUserBySaleDate(string application, string userId, DateTime saleDateMin, DateTime saleDateMax)
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
                    var user = GetUser(userId);
                    var locationRef = user.Location;
                    var location = locationRef.AsLocation();
                    var coid = location.GetCoid();
                    var crmUser = GetCrmUser(application, userId);
                    var companies = _helperCompany.GetCompaniesForUser(crmUser);

                    if (companies.Count == 0)
                    {
                        throw new Exception("Your team has no orders due to no Companies or Company Groups defined");
                    }

                    var context = ContextFactory.GetOrdersContextForCoid(coid, true);
                    {
                        //context.Connection.Open();

                        var companyCodes = companies.Select(x => x.Code).ToList();
                        var queryOrders = OrdersAdvancedQuery.AsQueryable(coid);

                        queryOrders = queryOrders.Where(x => companyCodes.Contains(x.CompanyCode));

                        queryOrders = queryOrders.Where(x => x.OrderSaleDate >= saleDateMin && x.OrderSaleDate <= saleDateMax);
                        queryOrders = queryOrders.Where(x => x.OrderSaleTypeCode == "ORD");
                        var results = queryOrders.ToList();
                        List<CompanyListModel> companyTotals = new List<CompanyListModel>();
                        List<SalesPersonListModel> salesPersonTotals = new List<SalesPersonListModel>();

                        var model = new List<OrderListModel>();
                        ProcessOrdersAccumulateTotals(results, coid, context, model, companyTotals, salesPersonTotals);
                        result.SalesPersonTotals = salesPersonTotals;
                        result.CompanyTotals = companyTotals;

                        result.Orders = model.OrderBy(x => x.SaleDate);

                        result.Success = true;
                        //context.Connection.Close();

                    }

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
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Route("imetal/GetOrderDetailsForOrderNumber/{application}/{userId}/{coid}/{orderNumber}")]
        public async Task<JsonResult> GetOrderDetailsForOrderNumber(string application, string userId, string coid, int orderNumber)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.OrderDetails = OrderDetailsAdvancedQuery.GetProjectionForOrderNumber(coid, orderNumber);
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

        private static void ProcessOrdersAccumulateTotals(List<OrdersAdvancedQuery> orders, string coid, OrdersDataContext context, List<OrderListModel> model, List<CompanyListModel> companyTotals, List<SalesPersonListModel> salesPersonTotals)
        {

            foreach (var order in orders.ToList())
            {
                var margin = order.Margin;
                var orderModel = new OrderListModel(
                    order.Coid,
                    order.OrderNumber,
                    order.CompanyCode,
                    order.CompanyName,
                    order.CompanyShortName,
                    order.SalesPerson,
                    order.OrderDueDate,
                    order.OrderSaleDate,
                    margin);
                model.Add(orderModel);

                var companyModel = companyTotals.SingleOrDefault(x => x.Code == order.CompanyCode);

                if (companyModel == null)
                {
                    companyModel =
                        new CompanyListModel(order.CompanyCode, order.CompanyShortName, order.CompanyName, margin);
                    companyTotals.Add(companyModel);
                }
                else
                {
                    margin.AddTo(companyModel.Margin);
                }

                var salesPersonModel = salesPersonTotals.SingleOrDefault(x => x.SalesPerson == order.SalesPerson);

                if (salesPersonModel == null)
                {
                    salesPersonModel = new SalesPersonListModel(order.SalesPerson, margin);
                    salesPersonTotals.Add(salesPersonModel);
                }
                else
                {
                    margin.AddTo(salesPersonModel.Margin);
                }
            }
        }

        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Route("imetal/GetWareHousesForCoid/{application}/{userId}/{coid}")]
        public async Task<JsonResult> GetWareHousesForCoid(string application, string userId, string coid)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.WareHouses = WarehousesAdvancedQuery.GetForCoid(coid);
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
        [Route("imetal/GetStockItemsForProductId/{coid}/{productId}")]
        public async Task<JsonResult> GetStockItemsForProductId(string coid, int productId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            //var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    //ThrowExceptionForBadToken(statusCode);

                    var context = ContextFactory.GetStockItemsContextForCoid(coid);
                    //context.Connection.Open();

                    var stockItemQuery = StockItemsAdvancedQuery.AsQueryable(coid, context);

                    stockItemQuery = stockItemQuery.Where(x => x.ProductId == productId);
                    stockItemQuery = stockItemQuery.Where(x => x.AvailableWeight > 0 && x.AvailablePieces > 0);

                    result.StockItems = stockItemQuery.ToList();

                    //context.Connection.Close();

                    result.Success = true;

                });
            }
            catch (Exception e)
            {
                //SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("imetal/GetIncomingPurchaseOrderItemsForProductId/{coid}/{productId}")]
        public async Task<JsonResult> GetIncomingPurchaseOrderItemsForProductId(string coid, int productId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            //var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    //ThrowExceptionForBadToken(statusCode);

                    var context = ContextFactory.GetPurchaseOrdersContextForCoid(coid);
                    //context.Connection.Open();

                    var query = PurchaseOrderItemsAdvancedQuery.AsQueryable(coid, context);

                    query = query.Where(x => x.ProductId == productId);

                    result.PurchaseOrderItems = query.ToList();

                    //context.Connection.Close();

                    result.Success = true;
                });

            }
            catch (Exception e)
            {
                //SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }


        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("imetal/GetStockItems")]
        public async Task<JsonResult> GetStockItems([FromBody] StockItemsQueryModel model)
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
                _logger.Log(GetClassName(), $"GetStockItems:ModelError Occurred:{ex.Message}", ex, sendEmail: true, writeToEventLog: true);

                return JsonResultWithStatusCode(result, HttpStatusCode.BadRequest);
            }

            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    result.StockItemsQueryModel = model;


                    //var cacheInfo = _stockItemsCache.GetForCoid(model.Coid, model.RefreshCache);
                    var cacheId = CacheSettings.GetCurrentStockItemsCacheId();
                    var helper = DAL.Vulcan.Mongo.MetalogicsCacheData.StockItemsCache.Helper;
                    var filter = helper.FilterBuilder.Where(x => x.Coid == model.Coid && x.CacheId == cacheId);
                    var project = helper.ProjectionBuilder.
                        Expression(stockItem => new StockItemsCondensed()
                        {
                            Coid = stockItem.Coid,
                            ProductCategory = stockItem.ProductCategory,
                            ProductCondition = stockItem.ProductCondition,
                            AvailablePieces = stockItem.AvailablePieces,
                            AvailableLength = stockItem.AvailableLength,
                            TagNumber = stockItem.TagNumber,
                            HeatNumber = stockItem.HeatNumber,
                            MillCode = stockItem.MillCode,
                            WarehouseCode = stockItem.WarehouseCode,
                            Location = stockItem.Location,
                            ReceivedDate = stockItem.ReceivedDate,
                            StockHoldUser = stockItem.StockHoldUser,
                            StockHoldReason = stockItem.StockHoldReason,
                            ProductId = stockItem.ProductId,
                            AvailableQuantity = stockItem.AvailableQuantity,
                            InsideDiameter = stockItem.InsideDiameter,
                            OuterDiameter = stockItem.OuterDiameter,
                            ProductCode = stockItem.ProductCode,
                            CostPerWeight = stockItem.CostPerWeight,
                            StockItemId = stockItem.StockItemId,
                            Length = stockItem.Length,
                            AllocatedLength = stockItem.AllocatedLength,
                            ProductControlCode = stockItem.ProductControlCode,
                            IsMachinedPart = stockItem.IsMachinedPart,
                            IsZeroWeightService = stockItem.IsZeroWeightService,
                            TotalCost = stockItem.TotalCost,
                            PieceCost = stockItem.PieceCost,
                            MillName = stockItem.MillName,
                            WarehouseName = stockItem.WarehouseName,
                            StratificationRank = stockItem.StratificationRank
                        });


                    var cache = helper.FindWithProjection(filter, project).AsQueryable();
                    //var cache = cacheInfo.StockItems;
                    //if (cacheInfo.Refreshed)
                    //{
                    //    var salesPerson = _helperUser.GetCrmUser(model.Application, model.UserId).AsCrmUserRef();
                    //    StockItemCacheLogValue.Helper.Upsert(new StockItemCacheLogValue()
                    //    {
                    //        SalesPerson = salesPerson,
                    //        OccurredAt = cacheInfo.CachedDate,
                    //        StockItemsQueryModel = model
                    //    });
                    //}

                    var query = cache;

                    var exchangeFactor =
                        _helperCurrencyForIMetal.GetExchangeRateForCurrencyFromCoid(model.DisplayCurrency, model.Coid);

                    query = FilterIgnoreBecauseNoValuesToSearchFor(query);

                    query = FilterOutNoPieces(query);

                    query = FilterProductId(query);

                    query = FilterAllSearchValues(query);

                    query = FilterForProductBucket(query);

                    stopWatch.Stop();

                    //result.Id = _stockItemsCache.GetIdForCoid(model.Coid);
                    result.StockItems = query.Select(i => new
                    {
                        i.Coid,
                        Currency = model.DisplayCurrency,
                        CostPerLb = i.CostPerLb,
                        CostPerKg = i.CostPerKg,
                        ExchangeFactor = exchangeFactor,
                        CostPerLbConverted = i.CostPerLb * exchangeFactor,
                        CostPerKgConverted = i.CostPerKg * exchangeFactor,
                        i.HeatNumber,
                        i.InsideDiameter,
                        i.Length,
                        i.AvailablePieces,
                        i.AvailableQuantity,
                        i.AvailableLength,
                        i.AllocatedLength,
                        i.OuterDiameter,
                        i.ProductCategory,
                        i.ProductCode,
                        i.ProductCondition,
                        i.ProductSize,
                        i.ProductId,
                        i.ReceivedDate,
                        i.TagNumber,
                        i.StockHoldUser,
                        i.StockHoldReason,
                        i.StockItemId,
                        i.WarehouseCode,
                        i.Location,
                        i.MillCode,
                        i.StratificationRank
                    }).ToList().OrderByDescending(x => x.AvailableLength);

                    result.Elapsed =
                        $"{stopWatch.Elapsed.Seconds}.{stopWatch.Elapsed.Milliseconds} seconds.";

                    result.Success = true;


                });

            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }

            IQueryable<StockItemsCondensed> FilterForProductBucket(IQueryable<StockItemsCondensed> query)
            {
                if (model.ProductBucketId != string.Empty)
                {
                    var bucket = new RepositoryBase<ProductBucket>().Find(model.ProductBucketId);
                    if (bucket == null) throw new Exception("Product Bucket not found");

                    if ((bucket.CategoryConditionAndOrValue == AndOrValue.Or) && (bucket.ProductCategories.Any() && bucket.ProductCategories.Any()))
                    {
                        query = query.Where(x =>
                            bucket.ProductCategories.Contains(x.ProductCategory) ||
                            bucket.ProductConditions.Contains(x.ProductCondition));
                    } else if (bucket.ProductCategories.Any())
                    {
                        query = query.Where(x => bucket.ProductCategories.Contains(x.ProductCategory));
                    } else if (bucket.ProductConditions.Any())
                    {
                        query = query.Where(x => bucket.ProductConditions.Contains(x.ProductCondition));

                    }
                    //foreach (var bucketProductCondition in bucket.ProductConditions)
                    //{
                    //    query = query.Where(x => x.ProductCondition == bucketProductCondition);
                    //}

                    if (bucket.ShowOnlyWarehouseCodes.Any())
                    {
                        query = query.Where(x => bucket.ShowOnlyWarehouseCodes.Contains(x.WarehouseCode));

                        //foreach (var bucketShowOnlyWarehouseCode in bucket.ShowOnlyWarehouseCodes)
                        //{
                        //    query = query.Where(x => x.WarehouseCode == bucketShowOnlyWarehouseCode);
                        //}
                    }
                    else
                    {
                        if (bucket.IgnoreWarehouseCodes.Any())
                        {
                            query = query.Where(x => !bucket.IgnoreWarehouseCodes.Contains(x.WarehouseCode));
                        }
                        //foreach (var bucketIgnoreWarehouseCode in bucket.IgnoreWarehouseCodes)
                        //{
                        //    query = query.Where(x => x.WarehouseCode != bucketIgnoreWarehouseCode);
                        //}
                    }
                }

                return query;
            }

            return JsonResultWithStatusCode(result, statusCode);

            IQueryable<StockItemsCondensed> FilterIgnoreBecauseNoValuesToSearchFor(IQueryable<StockItemsCondensed> query)
            {
                if ((model.ProductId == 0) && (model.SearchAllValues.Count == 0))
                {
                    //query = query.Where(x => x.AvailablePieces < -10000);
                    return query;
                }
                else
                {
                    return query;
                }
            }

            IQueryable<StockItemsCondensed> FilterOutNoPieces(IQueryable<StockItemsCondensed> query)
            {
                query = query.Where(x => x.AvailablePieces > 0);
                return query;
            }


            IQueryable<StockItemsCondensed> FilterAllSearchValues(IQueryable<StockItemsCondensed> query)
            {
                if (model.SearchAllValues.Count > 0)
                {
                    query = query.Where(x =>
                        (x.WarehouseCode != null &&
                         model.SearchAllValues.Any(v =>
                             (
                                 (x.WarehouseCode + x.ProductCategory + x.ProductCondition).ToUpper()
                                 .Contains(v.ToUpper())
                             ))));

                }
                return query;
            }


            IQueryable<StockItemsCondensed> FilterProductId(IQueryable<StockItemsCondensed> query)
            {
                if (model.ProductId != 0)
                {
                    query = query.Where(x => x.ProductId == model.ProductId);
                }

                return query;
            }

        }

        //[AllowAnonymous]
        //[Microsoft.AspNetCore.Mvc.HttpPost]
        //[Route("imetal/GetMachinedParts")]
        //public async Task<JsonResult> GetMachinedParts([FromBody] StockItemsQueryModel model)
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var statusCode = CheckToken(model.Application, model.UserId);
        //    try
        //    {
        //        await Task.Run(() =>
        //        {
        //            ThrowExceptionForBadToken(statusCode);
        //            Stopwatch stopWatch = new Stopwatch();
        //            stopWatch.Start();

        //            result.StockItemsQueryModel = model;

        //            var cache = _stockItemsCache.GetForCoid(model.Coid, model.RefreshCache).MachinedParts;

        //            var query = cache.AsQueryable();

        //            var exchangeFactor =
        //                _helperCurrencyForIMetal.GetExchangeRateForCurrencyFromCoid(model.DisplayCurrency, model.Coid);

        //            //query = FilterIgnoreBecauseNoValuesToSearchFor(query);

        //            query = FilterOutNoPieces(query);

        //            //query = FilterProductId(query);

        //            //query = FilterAllSearchValues(query);

        //            //query = FilterForProductBucket(query);

        //            stopWatch.Stop();

        //            result.Id = _stockItemsCache.GetIdForCoid(model.Coid);
        //            result.StockItems = query.Select(i => new
        //            {
        //                i.Coid,
        //                Currency = model.DisplayCurrency,
        //                ExchangeFactor = exchangeFactor,
        //                i.HeatNumber,
        //                i.InsideDiameter,
        //                i.Length,
        //                i.AvailablePieces,
        //                i.AvailableQuantity,
        //                i.AvailableLength,
        //                i.AllocatedLength,
        //                i.OuterDiameter,
        //                i.ProductCategory,
        //                i.ProductCode,
        //                i.ProductCondition,
        //                i.ProductSize,
        //                i.ProductId,
        //                i.ReceivedDate,
        //                i.TagNumber,
        //                i.StockHoldUser,
        //                i.StockHoldReason,
        //                i.StockItemId,
        //                i.WarehouseCode,
        //                i.Location,
        //                i.MillCode,
        //                PieceCost = i.PieceCost * exchangeFactor,
        //                i.StratificationRank
        //            }).ToList().OrderByDescending(x => x.ProductCode);

        //            result.Elapsed =
        //                $"{stopWatch.Elapsed.Seconds}.{stopWatch.Elapsed.Milliseconds} seconds.";

        //            result.Success = true;

        //        });


        //    }
        //    catch (Exception e)
        //    {
        //        SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
        //        result.ErrorMessage = e.Message;
        //        result.Success = false;
        //    }

        //    /*
        //    IQueryable<StockItemsCondensed> FilterForProductBucket(IQueryable<StockItemsCondensed> query)
        //    {
        //        if (model.ProductBucketId != string.Empty)
        //        {
        //            var bucket = new RepositoryBase<ProductBucket>().Find(model.ProductBucketId);
        //            if (bucket == null) throw new Exception("Product Bucket not found");

        //            if ((bucket.CategoryConditionAndOrValue == AndOrValue.Or) && (bucket.ProductCategories.Any() && bucket.ProductCategories.Any()))
        //            {
        //                query = query.Where(x =>
        //                    bucket.ProductCategories.Contains(x.ProductCategory) ||
        //                    bucket.ProductConditions.Contains(x.ProductCondition));
        //            }
        //            else if (bucket.ProductCategories.Any())
        //            {
        //                query = query.Where(x => bucket.ProductCategories.Contains(x.ProductCategory));
        //            }
        //            else if (bucket.ProductConditions.Any())
        //            {
        //                query = query.Where(x => bucket.ProductConditions.Contains(x.ProductCondition));

        //            }
        //            //foreach (var bucketProductCondition in bucket.ProductConditions)
        //            //{
        //            //    query = query.Where(x => x.ProductCondition == bucketProductCondition);
        //            //}

        //            if (bucket.ShowOnlyWarehouseCodes.Any())
        //            {
        //                query = query.Where(x => bucket.ShowOnlyWarehouseCodes.Contains(x.WarehouseCode));

        //                //foreach (var bucketShowOnlyWarehouseCode in bucket.ShowOnlyWarehouseCodes)
        //                //{
        //                //    query = query.Where(x => x.WarehouseCode == bucketShowOnlyWarehouseCode);
        //                //}
        //            }
        //            else
        //            {
        //                if (bucket.IgnoreWarehouseCodes.Any())
        //                {
        //                    query = query.Where(x => !bucket.IgnoreWarehouseCodes.Contains(x.WarehouseCode));
        //                }
        //                //foreach (var bucketIgnoreWarehouseCode in bucket.IgnoreWarehouseCodes)
        //                //{
        //                //    query = query.Where(x => x.WarehouseCode != bucketIgnoreWarehouseCode);
        //                //}
        //            }
        //        }

        //        return query;
        //    }
            
        //    */
        //    return JsonResultWithStatusCode(result, statusCode);
            

        //    /*
        //    IQueryable<StockItemsCondensed> FilterIgnoreBecauseNoValuesToSearchFor(IQueryable<StockItemsCondensed> query)
        //    {
        //        if ((model.ProductId == 0) && (model.SearchAllValues.Count == 0))
        //        {
        //            //query = query.Where(x => x.AvailablePieces < -10000);
        //            return query;
        //        }
        //        else
        //        {
        //            return query;
        //        }
        //    }
        //    */
        //    IQueryable<StockItemsCondensed> FilterOutNoPieces(IQueryable<StockItemsCondensed> query)
        //    {
        //        query = query.Where(x => x.AvailablePieces > 0);
        //        return query;
        //    }

        //    /*
        //    IQueryable<StockItemsCondensed> FilterAllSearchValues(IQueryable<StockItemsCondensed> query)
        //    {
        //        if (model.SearchAllValues.Count > 0)
        //        {
        //            query = query.Where(x =>
        //                (x.WarehouseCode != null &&
        //                 model.SearchAllValues.Any(v =>
        //                     (
        //                         (x.WarehouseCode + x.ProductCategory + x.ProductCondition).ToUpper()
        //                         .Contains(v.ToUpper())
        //                     ))));

        //        }
        //        return query;
        //    }
        //    */

        //    /*
        //    IQueryable<StockItemsCondensed> FilterProductId(IQueryable<StockItemsCondensed> query)
        //    {
        //        if (model.ProductId != 0)
        //        {
        //            query = query.Where(x => x.ProductId == model.ProductId);
        //        }

        //        return query;
        //    }
        //    */
        //}

        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Route("imetal/GetStockItemQueryModel/{application}/{userId}")]
        public async Task<JsonResult> GetStockItemQueryModel(string application, string userId)
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
                    if (crmUser == null) throw new Exception("User not found");

                    result.StockItemQueryModel = new StockItemsQueryModel(application, crmUser.User.Id);
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
        [Route("imetal/GetPurchaseOrderItems")]
        public async Task<JsonResult> GetPurchaseOrderItems([FromBody] PurchaseOrderItemsQueryModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    var exchangeFactor =
                        _helperCurrencyForIMetal.GetExchangeRateForCurrencyFromCoid(model.DisplayCurrency, model.Coid);

                    var cacheId = CacheSettings.GetCurrentPurchaseOrderItemsCacheId();
                    var helper = DAL.Vulcan.Mongo.MetalogicsCacheData.PurchaseOrderItemsCache.Helper;
                    var filter = helper.FilterBuilder.Where(x => x.Coid == model.Coid && x.CacheId == cacheId);

                    //var cache = _purchaseOrderItemsCache.GetForCoid(model.Coid, model.RefreshCache).Values;
                    var cache = helper.Find(filter);
                    var query = cache.AsQueryable();



                    query = FilterOutNoPieces(query);

                    query = FilterProductId(query);

                    query = FilterForProductBucket(query);

                    sw.Stop();


                    var poItems = query.Select(x => new
                    {
                        x.Coid,
                        x.Buyer,
                        x.ControlPieces,
                        x.CostPerInch,
                        x.CostPerKg,
                        x.CostPerLb,
                        x.CreateDate,

                        Currency = model.DisplayCurrency,
                        ExchangeFactor = exchangeFactor,
                        CostPerLbConverted = x.CostPerLb * exchangeFactor,
                        CostPerKgConverted = x.CostPerKg * exchangeFactor,

                        x.Density,
                        x.Dim1StaticDimension,
                        x.Dim2StaticDimension,
                        x.Dim3StaticDimension,
                        x.DueDate,
                        x.OuterDiameter,
                        x.InsideDiameter,
                        x.FactorForKilograms,
                        x.FactorForLbs,
                        x.InternalStatusId,
                        x.ItemDueDate,
                        x.ItemNumber,
                        x.Length,
                        x.Location,
                        x.ProductCategory,
                        x.MetalCategory,
                        x.MetalType,
                        x.PoNumber,
                        x.StockGrade,
                        x.StockType,
                        x.ProductSize,
                        x.ProductCondition,
                        x.ProductCode,
                        x.ProductControlCode,
                        x.ProductDensity,
                        x.PurchaseCategoryCode,
                        x.PurchaseCategoryDescription,
                        x.PurchaseOrderHeaderId,
                        x.PurchaseOrderItemId,
                        x.ProductId,
                        x.ProductType,
                        x.PurchaseType,
                        x.RequestType,
                        x.Status,
                        x.StatusCode,
                        x.StatusDescription,
                        x.Supplier,
                        x.TheoWeight,
                        x.Thick,
                        x.Width,
                        x.WarehouseCode,
                        x.WarehouseName,
                        x.WarehouseShortName,
                        x.VolumeDensity,
                        x.StratificationRank,
                        OrderedPiecesUnit = x.OrderedPiecesUnit,
                        OrderedLengthUnit = x.OrderedLengthUnit,
                        OrderedWeightUnit = x.OrderedWeightUnit,
                        OrderedQuantityUnit = x.OrderedQuantityUnit,
                        OrderedPieces = x.OrderedPieces,
                        OrderedQuantity = x.OrderedQuantity,
                        OrderedLength = x.OrderedLength,
                        OrderedWeight = x.OrderedWeight,
                        OrderedWeightLbs = x.OrderedWeightLbs,
                        OrderedWeightKgs = x.OrderedWeightKgs,
                        AllocatedPiecesUnit = x.AllocatedPiecesUnit,
                        AllocatedLengthUnit = x.AllocatedLengthUnit,
                        AllocatedWeightUnit = x.AllocatedWeightUnit,
                        AllocatedQuantityUnit = x.AllocatedQuantityUnit,
                        AllocatedPieces = x.AllocatedPieces,
                        AllocatedQuantity = x.AllocatedQuantity,
                        AllocatedLength = x.AllocatedLength,
                        AllocatedWeight = x.AllocatedWeight,
                        AllocatedWeightLbs = x.AllocatedWeightLbs,
                        AllocatedWeightKgs = x.AllocatedWeightKgs,
                        DeliveredPiecesUnit = x.DeliveredPiecesUnit,
                        DeliveredLengthUnit = x.DeliveredLengthUnit,
                        DeliveredWeightUnit = x.DeliveredWeightUnit,
                        DeliveredQuantityUnit = x.DeliveredQuantityUnit,
                        DeliveredPieces = x.DeliveredPieces,
                        DeliveredQuantity = x.DeliveredQuantity,
                        DeliveredLength = x.DeliveredLength,
                        DeliveredWeight = x.DeliveredWeight,
                        DeliveredWeightLbs = x.DeliveredWeightLbs,
                        DeliveredWeightKgs = x.DeliveredWeightKgs,
                        BalancePiecesUnit = x.BalancePiecesUnit,
                        BalanceLengthUnit = x.BalanceLengthUnit,
                        BalanceWeightUnit = x.BalanceWeightUnit,
                        BalanceQuantityUnit = x.BalanceQuantityUnit,
                        BalancePieces = x.BalancePieces,
                        BalanceQuantity = x.BalanceQuantity,
                        BalanceLength = x.BalanceLength,
                        BalanceWeight = x.BalanceWeight,
                        BalanceWeightLbs = x.BalanceWeightLbs,
                        BalanceWeightKgs = x.BalanceWeightKgs,

                    }).ToList().OrderByDescending(x => x.BalanceLength);
                    result.Elapsed =
                        $"{poItems.Count()} rows. {sw.Elapsed.Seconds}.{sw.Elapsed.Milliseconds} seconds.";
                    //result.Id = _purchaseOrderItemsCache.GetIdForCoid(model.Coid);
                    result.PurchaseOrderItems = poItems;
                    result.PurchaseOrderItemsQueryModel = model;
                    //result.ExchangeRates = ExchangeRate.GetRateList();
                    result.Success = true;


                });
            }
            catch (Exception e)
            {
                SetStatusCodeToBadRequestIfNotUnauthorizedOrForbidden(ref statusCode);
                result.ErrorMessage = e.Message;
                result.Success = false;
            }

            IQueryable<PurchaseOrderItemsCache> FilterForProductBucket(IQueryable<PurchaseOrderItemsCache> query)
            {
                if (model.ProductBucketId != string.Empty)
                {
                    var bucket = new RepositoryBase<ProductBucket>().Find(model.ProductBucketId);
                    if (bucket == null) throw new Exception("Product Bucket not found");

                    if (bucket.ProductCategories.Any())
                    {
                        query = query.Where(x => bucket.ProductCategories.Contains(x.ProductCategory));
                    }

                    //foreach (var bucketProductCategory in bucket.ProductCategories)
                    //{
                    //    query = query.Where(x => x.ProductCategory == bucketProductCategory);
                    //}

                    if (bucket.ProductConditions.Any())
                    {
                        query = query.Where(x => bucket.ProductConditions.Contains(x.ProductCondition));

                    }
                    //foreach (var bucketProductCondition in bucket.ProductConditions)
                    //{
                    //    query = query.Where(x => x.ProductCondition == bucketProductCondition);
                    //}

                    if (bucket.ShowOnlyWarehouseCodes.Any())
                    {
                        query = query.Where(x => bucket.ShowOnlyWarehouseCodes.Contains(x.WarehouseCode));

                        //foreach (var bucketShowOnlyWarehouseCode in bucket.ShowOnlyWarehouseCodes)
                        //{
                        //    query = query.Where(x => x.WarehouseCode == bucketShowOnlyWarehouseCode);
                        //}
                    }
                    else
                    {
                        if (bucket.IgnoreWarehouseCodes.Any())
                        {
                            query = query.Where(x => !bucket.IgnoreWarehouseCodes.Contains(x.WarehouseCode));
                        }
                        //foreach (var bucketIgnoreWarehouseCode in bucket.IgnoreWarehouseCodes)
                        //{
                        //    query = query.Where(x => x.WarehouseCode != bucketIgnoreWarehouseCode);
                        //}
                    }
                }

                return query;
            }


            return JsonResultWithStatusCode(result, statusCode);


            IQueryable<PurchaseOrderItemsCache> FilterOutNoPieces(IQueryable<PurchaseOrderItemsCache> query)
            {
                query = query.Where(x => x.BalancePieces > 0);
                return query;
            }


            IQueryable<PurchaseOrderItemsCache> FilterProductId(IQueryable<PurchaseOrderItemsCache> query)
            {
                if (model.ProductId != 0)
                {
                    query = query.Where(x => x.ProductId == model.ProductId);
                }

                return query;
            }

        }

        [AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Route("imetal/GetPurchaseOrderItemsQueryModel/{application}/{userId}")]
        public async Task<JsonResult> GetPurchaseOrderItemsQueryModel(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            var crmUser = _helperUser.GetCrmUser(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    result.PurchaseOrderItemsQueryModel = new PurchaseOrderItemsQueryModel(application, crmUser.UserId);
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
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Route("imetal/GetProductCategoryListForCoid/{application}/{userId}/{coid}")]
        public async Task<JsonResult> GetProductCategoryListForCoid(string application, string userId, string coid)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ProductCategoryList = StockItemsAdvancedQuery.GetProductCategories(coid);

                    result.StockItemQueryModel = new StockItemsQueryModel();
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
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Route("imetal/GetProductConditionListForCoid/{application}/{userId}/{coid}")]
        public async Task<JsonResult> GetProductConditionListForCoid(string application, string userId, string coid)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.ProductConditionList = StockItemsAdvancedQuery.GetProductConditions(coid);

                    result.StockItemQueryModel = new StockItemsQueryModel();
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
        [Route("imetal/GetOsirisDocumentListForPortal")]
        public async Task<JsonResult> GetOsirisDocumentListForPortal([FromBody] OsirisDocumentListForPortal model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    var rep = new OsirisRepository();
                    result.OsirisDocuments = rep.GetOsirisDocumentList(model.Coid, model.TagNumber, model.HeatNumber);
                    result.Success = true;
                });
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, HttpStatusCode.OK);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("imetal/GetOsirisDocumentForPortal/{coid}/{dmtDocId}/{fileName}")]
        public IActionResult GetOsirisDocumentForPortal(string coid, long dmtDocId, string fileName)
        {
            try
            {
                var rep = new OsirisRepository();
                var memoryStream = rep.GetOsirisDocumentAsStream(coid, dmtDocId);
                return File(memoryStream, _helperFile.GetContentType(fileName), fileName);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }

        }



        [AllowAnonymous]
        [HttpPost]
        [Route("imetal/GetOsirisDocumentList")]
        public async Task<JsonResult> GetOsirisDocumentList([FromBody] OsirisDocumentListModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    var rep = new OsirisRepository();
                    result.OsirisDocuments = rep.GetOsirisDocumentList(model.Coid, model.TagNumber, model.HeatNumber);
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
        //[Produces("application/pdf")]
        [Route("imetal/GetOsirisDocument/{application}/{userId}/{coid}/{dmtDocId}/{fileName}")]
        public IActionResult GetOsirisDocument(string application, string userId, string coid, long dmtDocId, string fileName)
        {
            var statusCode = CheckToken(application, userId);
            try
            {
                ThrowExceptionForBadToken(statusCode);
                var rep = new OsirisRepository();
                var memoryStream = rep.GetOsirisDocumentAsStream(coid, dmtDocId);
                memoryStream.Position = 0;
                return File(memoryStream, _helperFile.GetContentType(fileName), fileName);
            }
            catch (Exception exception)
            {
                return BadRequest($"Error: {exception.Message}");
                //return statusCode != HttpStatusCode.Unauthorized ? StatusCode(500) : StatusCode((int)statusCode);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("imetal/GetProductConditionListForCoidAndProductCategory/{application}/{userId}/{coid}/{productCategory}")]
        public async Task<JsonResult> GetProductConditionListForCoidAndProductCategory(string application, string userId, string coid, string productCategory)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    var cacheId = CacheSettings.GetCurrentStockItemsCacheId();

                    result.ProductConditionList = DAL.Vulcan.Mongo.MetalogicsCacheData.StockItemsCache.Helper.Find(x=>x.CacheId == cacheId).ToList().Where(x=> x.ProductCategory.Contains(productCategory)).Select(x => x.ProductCondition).Distinct().OrderBy(x => x).ToList();

                    result.StockItemQueryModel = new StockItemsQueryModel();
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
