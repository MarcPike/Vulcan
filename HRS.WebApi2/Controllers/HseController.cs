using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class HseController : BaseController
    {
        private readonly IHelperHse _helperHse;

        public HseController(IHelperHse helperHse)
        {
            _helperHse = helperHse;
        }

        /*
         *         List<BbsObserverRef> GetActiveObserverRefs();
        List<BbsObserverModel> GetObserverModels();
        BbsObserverModel SaveBbsObserver(BbsObserverModel model);
        void RemoveBbsObserver(string id);


         */

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetActiveObserverRefs")]
        public JsonResult GetActiveObserverRefs()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.ActiveObserverRefs = _helperHse.GetActiveObserverRefs();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetObserverModels")]
        public JsonResult GetObserverModels()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.ObserverModels = _helperHse.GetObserverModels();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("hse/SaveObserver")]
        public JsonResult SaveObserver([FromBody] BbsObserverModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.ObserverModel = _helperHse.SaveBbsObserver(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/RemoveObserver/{id}")]
        public JsonResult RemoveObserver(string id)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                _helperHse.RemoveBbsObserver(id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetHseObservations")]
        public JsonResult GetHseObservations()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.HseObservations = _helperHse.GetAllHseObservations();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetAllBbsDepartmentModels")]
        public JsonResult GetAllBbsDepartmentModels()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BbsDepartmentModels = _helperHse.GetAllBbsDepartmentModels();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetBbsObservationGrid")]
        public JsonResult GetBbsObservationGrid()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BbsObservationGrid = _helperHse.GetBbsObservationGrid();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetAllBbsTaskModels")]
        public JsonResult GetAllBbsTaskModels()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BbsTaskModels = _helperHse.GetAllBbsTaskModels();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetNewObservationItem()")]
        public JsonResult GetNewObservationItem()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.ObservationItem = _helperHse.GetNewObservationItem();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }



        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetAllBbsObservationModels")]
        public JsonResult GetAllBbsObservationModels()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BbsObservationModels = _helperHse.GetAllBbsObservationModels();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetBbsObservation/{id}")]
        public JsonResult GetBbsObservation(string id)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BbsObservationModel = _helperHse.GetBbsObservation(id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("ObservationId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetAllBbsDepartmentRefs")]
        public JsonResult GetAllBbsDepartmentRefs()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.DepartmentRefs = _helperHse.GetAllBbsDepartmentRefs();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetAllBbsTaskRefs")]
        public JsonResult GetAllBbsTaskRefs()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TaskRefs = _helperHse.GetAllBbsTaskRefs();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetAllBbsDepartmentSubCategoryRefs/{departmentId}")]
        public JsonResult GetAllBbsDepartmentSubCategoryRefs(string departmentId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.DepartmentSubCategories = _helperHse.GetAllBbsDepartmentSubCategoryRefs(departmentId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetAllBbsTaskSubCategoryRefs/{taskId}")]
        public JsonResult GetAllBbsTaskSubCategoryRefs(string taskId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TaskSubCategories = _helperHse.GetAllBbsTaskSubCategoryRefs(taskId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetNewBbsDepartmentModel")]
        public JsonResult GetNewBbsDepartmentModel()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BbsDepartmentModel = _helperHse.GetNewBbsDepartmentModel();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetNewBbsTaskModel")]
        public JsonResult GetNewBbsTaskModel()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BbsTaskModel = _helperHse.GetNewBbsTaskModel();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetNewBbsObservationModel")]
        public JsonResult GetNewBbsObservationModel()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BbsObservationModel = _helperHse.GetNewBbsObservationModel();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        

        [AllowAnonymous]
        [HttpPost]
        [Route("hse/SaveBbsDepartment")]
        public JsonResult SaveBbsDepartment([FromBody] BbsDepartmentModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BbsDepartmentModel = _helperHse.SaveBbsDepartment(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("hse/SaveBbsTask")]
        public JsonResult SaveBbsTask([FromBody] BbsTaskModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.BbsTaskModel = _helperHse.SaveBbsTask(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("hse/SaveBbsObservation")]
        public JsonResult SaveBbsObservation([FromBody] BbsObservationModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                result.BbsObservationModel = _helperHse.SaveBbsObservation(model, hrsUser.AsHrsUserRef());
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/RemoveBbsDepartment/{id}")]
        public JsonResult RemoveBbsDepartment(string id)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                _helperHse.RemoveBbsDepartment(id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/RemoveBbsTask/{id}")]
        public JsonResult RemoveBbsTask(string id)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                _helperHse.RemoveBbsTask(id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }
        [AllowAnonymous]
        [HttpGet]
        [Route("hse/RemoveBbsObservation/{id}")]
        public JsonResult RemoveBbsObservation(string id)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                _helperHse.RemoveBbsObservation(id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetIncidentSeverityByLocation")]
        public JsonResult GetIncidentSeverityByLocation()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);


                result.IncidentSeverityCount = _helperHse.GetIncidentSeverityByLocation();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("hse/GetIncidentsYearToYear")]
        public JsonResult GetIncidentYearToYear()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);


                result.IncidentsYearToYearCount = _helperHse.GetIncidentsYearToYear();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        //public List<RequiredActivityModel> GetAllRequiredActivitiesDueLessThanSixMonthsFromNow(string status, string type, HrsUser hrsUser)
        public List<RequiredActivityModel> GetAllRequiredActivitiesDueLessThanSixMonthsFromNow(HrsUser hrsUser)
        {
            var result = new List<RequiredActivityModel>();

            var beforeDate = DateTime.Now.Date.AddMonths(6);

            var empQueryHelper = new MongoRawQueryHelper<Employee>();
            var projectionEmployeeId = empQueryHelper.ProjectionBuilder.Expression(x => x.Id.ToString());

            var role = SecurityRoleHelper.GetHrsSecurityRole(hrsUser);
            var employee = hrsUser.Employee.AsEmployee();

            FilterDefinition<Employee> filter = empQueryHelper.FilterBuilder.Empty;

            if (role.DirectReportsOnly)
            {
                filter = EmployeeDirectReportsFilterGenerator.GetDirectReportsOnlyFilter(empQueryHelper, role, employee, filter);
            }
            else
            {
                var locations = hrsUser.HrsSecurity.Locations;
                if (!locations.Any()) return new List<RequiredActivityModel>();
                filter = EmployeeLocationsFilterGenerator.GetLocationsFilter(empQueryHelper, locations, filter);
            }

            var filteredEmployees = empQueryHelper.FindWithProjection(filter, projectionEmployeeId);


            var queryHelper = new MongoRawQueryHelper<RequiredActivity>();
            var filterRequiredActivity = queryHelper.FilterBuilder.In(x => x.Employee.Id, filteredEmployees);

            FilterDefinition<RequiredActivity> baseFilter = queryHelper.FilterBuilder.Where(x =>
                (x.CompletionDeadline <= beforeDate) || (x.RevisedCompletionDeadline <= beforeDate));

            //FilterDefinition<RequiredActivity> statusFilter = queryHelper.FilterBuilder.Empty;
            //FilterDefinition<RequiredActivity> typeFilter = queryHelper.FilterBuilder.Empty;


            //if (status != "all")
            //{
            //    RequiredActivityStatus requiredActivityStatus =
            //        (RequiredActivityStatus)Enum.Parse(typeof(RequiredActivityStatus), status);
            //    statusFilter = queryHelper.FilterBuilder.Where(x => x.Status == requiredActivityStatus);

            //}

            //if (type != "all")
            //{
            //    RequiredActivityType requiredActivityType =
            //        (RequiredActivityType)Enum.Parse(typeof(RequiredActivityType), type);
            //    typeFilter = queryHelper.FilterBuilder.Where(x => x.Type == requiredActivityType);
            //}



            var project = queryHelper.ProjectionBuilder.Expression(
                x => new RequiredActivityModel(x));

            result.AddRange(queryHelper.FindWithProjection(baseFilter & filterRequiredActivity, project));

            return result.OrderBy(x => x.CompletionDeadline).ToList();

        }
    }
}
