using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Helpers;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HRS.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class TrainingController: BaseController
    {
        private readonly IHelperEmployee _helperEmployee;
        private readonly IHelperTraining _helperTraining;

        public TrainingController(IHelperEmployee helperEmployee, IHelperTraining helperTraining)
        {
            _helperEmployee = helperEmployee;
            _helperTraining = helperTraining;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("training/GetTrainingCourseDeActivateListModel")]
        public JsonResult GetTrainingCourseDeActivateListModel()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                
                result.TrainingCourseDeActivateListModel = _helperTraining.GetTrainingCourseDeActivateListModel(hrsUser.AsHrsUserRef());
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
        [Route("training/DeActivateTrainingCourses")]
        public JsonResult DeActivateTrainingCourses([FromBody] TrainingCourseDeActivateListModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingCourseDeActivateLog = _helperTraining.DeActivateTrainingCourses(model);
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
        [Route("training/DeActivateTrainingCourse/{trainingCourseId}")]
        public JsonResult DeActivateTrainingCourse(string trainingCourseId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);


                result.TrainingCourseDeActivateLog = _helperTraining.DeActivateTrainingCourse(trainingCourseId, hrsUser.AsHrsUserRef());
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                parameters.Add("TrainingCourseId", trainingCourseId);
                _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }
            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("training/GetDeactivatedTrainingCoursesHistory")]
        public JsonResult GetDeactivatedTrainingCoursesHistory()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingCourseDeActivateLogs = _helperTraining.GetDeactivatedTrainingCoursesHistory();
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
        [Route("training/GetModelForJobTitleCourseCopy/{sourceId}/{targetId}")]
        public JsonResult GetModelForJobTitleCourseCopy(string sourceId, string targetId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.JobTitleCourseCopyModel = _helperTraining.GetModelForJobTitleCourseCopy(sourceId, targetId);
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
        [Route("training/CopyJobTitleCourses")]
        public JsonResult CopyJobTitleCourses([FromBody] JobTitleCourseCopyModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                _helperTraining.CopyJobTitleCourses(model);
                result.JobTitleModel = new JobTitleModel(model.Target.AsJobTitle());
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
        [Route("training/CopyAllJobTitleCourses/{sourceId}/{targetId}")]
        public JsonResult CopyAllJobTitleCourses(string sourceId, string targetId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.JobTitleModel = _helperTraining.CopyAllJobTitleCourses(sourceId, targetId);
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
        [Route("training/GetTrainingCourses")]
        public JsonResult GetTrainingCourses()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingCourses = _helperTraining.GetTrainingCourses();
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
        [Route("training/GetTrainingCourseRefs")]
        public JsonResult GetTrainingCourseRefs()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var helper = new MongoRawQueryHelper<TrainingCourse>();
                var filter = helper.FilterBuilder.Empty;
                var trainingCourses = helper.Find(filter).OrderBy(x => x.Name);

                var trainingCourseRefs = trainingCourses.Select(x => x.AsTrainingCourseRef());

                result.TrainingCourseRefs = trainingCourseRefs;
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
        [Route("training/GetTrainingCourse/{courseId}")]
        public JsonResult GetTrainingCourse(string courseId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                result.TrainingCourseModel = _helperTraining.GetTrainingCourse(courseId);
                result.GroupClassifications = _helperTraining.GetGroupClassifications(hrsUser.Entity.Id);
                result.CourseTypes = _helperTraining.GetGroupCourseTypes(hrsUser.Entity.Id);
                result.VenueTypes = _helperTraining.GetVenueTypes(hrsUser.Entity.Id);
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
        [Route("training/GetTrainingCourseReferences")]
        public JsonResult GetTrainingCourseReferences()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingCourseReferences = _helperTraining.GetTrainingCourseReferences();
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
        [Route("training/GetTrainingCourseReferencesForLocation/{locationId}")]
        public JsonResult GetTrainingCourseReferencesForLocation(string locationId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingCourseReferences = _helperTraining.GetTrainingCourseReferencesForLocation(locationId);
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
        [Route("training/GetTrainingCoursesForLocation/{locationId}")]
        public JsonResult GetTrainingCoursesForLocation(string locationId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingCourses = _helperTraining.GetTrainingCoursesForLocation(locationId);
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
        [Route("training/GetAllTrainingEventsForEmployeesGrid")]
        public JsonResult GetAllTrainingEventsForEmployeesGrid()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                result.TrainingEventsForEmployeesGrid = _helperTraining.GetAllTrainingEventsForEmployeesGrid(hrsUser);
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
        [Route("training/GetTrainingEventsForEmployee/{employeeId}")]
        public JsonResult GetTrainingEventsForEmployee(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingEvents = _helperTraining.GetTrainingEventsForEmployee(employeeId);
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
        [Route("training/GetAllTrainingEvents")]
        public JsonResult GetAllTrainingEvents()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                result.TrainingEvents = _helperTraining.GetAllTrainingEvents(hrsUser);
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
        [Route("training/GetAllTrainingEventSupportingDocumentsNested")]
        public JsonResult GetAllTrainingEventSupportingDocumentsNested()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingEventsDocsNested = _helperTraining.GetTrainingEventSupportingDocumentsNested();
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
        [Route("training/GetAllTrainingEventSupportingDocumentsFlat")]
        public JsonResult GetAllTrainingEventSupportingDocumentsFlat()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingEventsDocsFlat = _helperTraining.GetTrainingEventSupportingDocumentsFlat();
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
        [Route("training/GetTrainingEventsForTrainingCourse/{trainingCourseId}")]
        public JsonResult GetTrainingEventsForTrainingCourse(string trainingCourseId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingEvents = _helperTraining.GetTrainingEventsForTrainingCourse(trainingCourseId);
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
        [Route("training/GetNewTrainingCourseModel")]
        public JsonResult GetNewTrainingCourseModel()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                result.TrainingCourseModel = _helperTraining.GetNewTrainingCourseModel();
                result.GroupClassifications = _helperTraining.GetGroupClassifications(hrsUser.Entity.Id);
                result.CourseTypes = _helperTraining.GetGroupCourseTypes(hrsUser.Entity.Id);
                result.VenueTypes = _helperTraining.GetVenueTypes(hrsUser.Entity.Id);

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
        [Route("training/SaveTrainingCourse")]
        public JsonResult SaveTrainingCourse([FromBody] TrainingCourseModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingCourseModel = _helperTraining.SaveTrainingCourse(model);
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
        [Route("training/GetNewTrainingEventModel")]
        public JsonResult GetNewTrainingEventModel()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                //result.TrainingCourses = _helperTraining.GetTrainingCourses();
                result.TrainingCourses = _helperTraining.GetTrainingCourseReferences();
                result.TrainingEventModel = _helperTraining.GetNewTrainingEventModel();
                //result.GroupClassifications = _helperTraining.GetGroupClassifications();
                //result.CourseTypes = _helperTraining.GetGroupCourseTypes();
                //result.VenueTypes = _helperTraining.GetVenueTypes();

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
        [Route("training/GetTrainingEventModel/{trainingEventId}")]
        public JsonResult GetTrainingEventModel(string trainingEventId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                result.TrainingEventModel = _helperTraining.GetTrainingEvent(trainingEventId);
                result.GroupClassifications = _helperTraining.GetGroupClassifications(hrsUser.Entity.Id);
                result.CourseTypes = _helperTraining.GetGroupCourseTypes(hrsUser.Entity.Id);
                result.VenueTypes = _helperTraining.GetVenueTypes(hrsUser.Entity.Id);

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
        [Route("training/SaveTrainingEvent")]
        public JsonResult SaveTrainingEvent([FromBody] TrainingEventModel model)
        {
            CheckForModelErrors();
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingEventModel = _helperTraining.SaveTrainingEvent(model);
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
        [Route("training/RemoveTrainingEvent/{trainingEventId}")]
        public JsonResult RemoveTrainingEvent(string trainingEventId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                _helperTraining.RemoveTrainingEvent(trainingEventId);
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
        [Route("training/GetNewTrainingAttendeeModel/{employeeId}")]
        public JsonResult GetNewTrainingAttendeeModel(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.TrainingAttendeeModel = _helperTraining.GetNewTrainingAttendeeModel(employeeId);
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
        [Route("training/GetRequiredActivitiesForEmployee/{employeeId}")]
        public JsonResult GetRequiredActivitiesForEmployee(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                result.RequiredActivities = _helperTraining.GetRequiredActivitiesForEmployee(employeeId);
                result.RequiredActivityTypePropertyValues = _helperTraining.GetRequiredActivityTypes(hrsUser.Entity.Id);
                result.RequiredActivityCompleteStatusTypes = _helperTraining.GetRequiredActivityCompleteStatusTypes(hrsUser.Entity.Id);
                result.RequiredActivityTypes = Enum.GetNames(typeof(RequiredActivityType)).ToList();
                result.RequiredActivityStatuses = Enum.GetNames(typeof(RequiredActivityStatus)).ToList();

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
        [Route("training/GetRequiredActivityModel/{requiredActivityId}")]
        public JsonResult GetRequiredActivityModel(string requiredActivityId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                result.RequiredActivityModel = _helperTraining.GetRequiredActivityModel(requiredActivityId);
                result.RequiredActivityTypePropertyValues = _helperTraining.GetRequiredActivityTypes(hrsUser.Entity.Id);
                result.RequiredActivityCompleteStatusTypes = _helperTraining.GetRequiredActivityCompleteStatusTypes(hrsUser.Entity.Id);
                result.RequiredActivityTypes = Enum.GetNames(typeof(RequiredActivityType)).ToList();
                result.RequiredActivityStatuses = Enum.GetNames(typeof(RequiredActivityStatus)).ToList();

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
        [Route("training/RemoveRequiredActivity/{requiredActivityId}")]
        public JsonResult RemoveRequiredActivity(string requiredActivityId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                _helperTraining.RemoveRequiredActivity(requiredActivityId);

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

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("training/SaveRequiredActivity")]
        //public JsonResult SaveRequiredActivity([FromBody] RequiredActivityModel model)
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var tokenData = GetTokenDataFromHeaders();
        //    try
        //    {
        //        ThrowExceptionForBadToken(tokenData.StatusCodeResult);
        //        var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

        //        var module = hrsUser.HrsSecurity.GetRole().Modules.FirstOrDefault(x => x.ModuleType.Name == "Required Activities");
                
        //        if (module == null) throw new Exception("You do not have access to this module");

        //        if (!module.Modify) throw new Exception("You do not have Modify access to this module");

        //        result.RequiredActivityModel = _helperTraining.SaveRequiredActivity(model);

        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var parameters = GetParametersDictionary();
        //        parameters.Add("UserId", tokenData.UserId);
        //        _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
        //        result.ErrorMessage = ex.Message;
        //        result.Success = false;
        //    }
        //    return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        //}


        [AllowAnonymous]
        [HttpGet]
        [Route("training/GetNewRequiredActivityModelForEmployee/{employeeId}")]
        public JsonResult GetNewRequiredActivityModelForEmployee(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                result.RequiredActivityModel = _helperTraining.GetNewRequiredActivityModelForEmployee(employeeId);
                result.RequiredActivityTypePropertyValues = _helperTraining.GetRequiredActivityTypes(hrsUser.Entity.Id);
                result.RequiredActivityCompleteStatusTypes = _helperTraining.GetRequiredActivityCompleteStatusTypes(hrsUser.Entity.Id);
                result.RequiredActivityTypes = Enum.GetNames(typeof(RequiredActivityType)).ToList();
                result.RequiredActivityStatuses = Enum.GetNames(typeof(RequiredActivityStatus)).ToList();

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
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("training/GetAllExpiringEvents")]
        //public JsonResult GetAllExpiringEvents()
        //{
        //    dynamic result = new ExpandoObject();
        //    result.Success = false;
        //    var tokenData = GetTokenDataFromHeaders();
        //    var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

        //    try
        //    {
        //        ThrowExceptionForBadToken(tokenData.StatusCodeResult);

        //        result.ExpipringEvents = _helperTraining.GetAllExpiringEvents(hrsUser);
        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var parameters = GetParametersDictionary();
        //        parameters.Add("UserId", tokenData.UserId);
        //        _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
        //        result.ErrorMessage = ex.Message;
        //        result.Success = false;
        //    }
        //    return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);

        //}



    }
}
