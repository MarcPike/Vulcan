using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
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
    public class EmployeeController : BaseController
    {
        private readonly IHelperEmployee _helperEmployee;

        public EmployeeController(IHelperEmployee helperEmployee)
        {
            _helperEmployee = helperEmployee;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/Tester")]
        public IActionResult Tester()
        {
            return Ok("Working");
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("employees/EmployeeAuditHistoryForBenefits90Days")]
        public JsonResult EmployeeAuditHistoryForBenefits()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var fromDate = DateTime.Now.AddDays(-90);
                result.EmployeeBasicInfo = new EmployeeAuditTrailHistoryModel(fromDate, "Benefits", null);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/EmployeeAuditHistoryForCompensation90Days")]
        public JsonResult EmployeeAuditHistoryForCompensation()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var fromDate = DateTime.Now.AddDays(-90);
                result.EmployeeBasicInfo = new EmployeeAuditTrailHistoryModel(fromDate, "Compensation", null);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/EmployeeAuditHistoryForDiscipline90Days")]
        public JsonResult EmployeeAuditHistoryForDiscipline()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var fromDate = DateTime.Now.AddDays(-90);

                result.EmployeeBasicInfo = new EmployeeAuditTrailHistoryModel(fromDate, "Discipline", null);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/EmployeeAuditHistoryForEmployeeDetails90Days")]
        public JsonResult EmployeeAuditHistoryForEmployeeDetails()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var fromDate = DateTime.Now.AddDays(-90);

                result.EmployeeBasicInfo = new EmployeeAuditTrailHistoryModel(fromDate, "EmployeeDetails", hrsUser);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("employees/EmployeeAuditHistoryForPerformance90Days")]
        public JsonResult EmployeeAuditHistoryForPerformance()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var fromDate = DateTime.Now.AddDays(-90);
                result.EmployeeBasicInfo = new EmployeeAuditTrailHistoryModel(fromDate, "Performance", null);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetEmployeeBasicInfo/{employeeId}")]
        public JsonResult GetEmployeeBasicInfo(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.EmployeeBasicInfo = EmployeeBasicInfoModel.FindById(employeeId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetEmployeeDetailsGrid")]
        public JsonResult GetAllEmployeeDetailsGridData()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.Employees = _helperEmployee.GetEmployeeDetailsGrid(tokenData.UserId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetNewEmployeeModel")]
        public JsonResult GetNewEmployeeModel()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var employeeModel = _helperEmployee.GetNewEmployeeModel();

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                if (hrsUser.Employee == null)
                {
                    hrsUser.Employee = _helperEmployee.FindEmployeeForHrsUser(hrsUser)?.AsEmployeeRef();
                    hrsUser.SaveToDatabase();
                }

                if (hrsUser.Employee != null)
                {
                    var employee = Employee.Helper.FindById(hrsUser.Employee.Id);

                    if (employee != null && employee.PayrollRegion != null)
                    {
                        employeeModel.PayrollRegion = employee.PayrollRegion;
                        employeeModel.BusinessRegionCode = employee.BusinessRegionCode;
                    }

                    //employeeModel.PayrollRegion = hrsUser.Employee.AsEmployee()?.PayrollRegion;
                    employeeModel.ModifiedByUser = hrsUser.AsHrsUserRef();
                }

                employeeModel.Entity = hrsUser.Entity;

                result.EmployeeModel = employeeModel;
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetEmployeeModel/{employeeId}")]
        public JsonResult GetEmployee(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);
                var employeeModel = _helperEmployee.GetEmployeeModel(employeeId);
                employeeModel.ModifiedByUser = hrsUser.AsHrsUserRef();

                if (hrsUser.HrsSecurity.GetRole().Modules.All(x => x.ModuleType.Name != "Compensation"))
                    employeeModel.Compensation = null;

                result.EmployeeModel = employeeModel;


                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetEmployeeDirectReports/{id}")]
        public JsonResult GetEmployeeDirectReports(string id)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.DirectReports = _helperEmployee.GetEmployeeDirectReports(id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("employees/SaveEmployee")]
        public JsonResult SaveEmployee([FromBody] EmployeeModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                CheckForModelErrors();
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                var modelTerm = model.TerminationDate;
                var modelTermUtc = model.TerminationDate?.ToUniversalTime();

                model.TerminationDate = model.TerminationDate?.Date;

                model.Entity = model.Entity ?? hrsUser.Entity;

                model.ModifiedByUser = hrsUser.AsHrsUserRef();

                var module = hrsUser.HrsSecurity.GetRole().Modules
                    .FirstOrDefault(x => x.ModuleType.Name == "Employee Details");
                if (module == null) throw new Exception("You do not have access to Employee Details");

                if (!module.Modify) throw new Exception("You do not have security access to save an Employee");

                if (model.PayrollRegion == null) model.PayrollRegion = hrsUser.Employee?.PayrollRegion;

                if (model.BusinessRegionCode == null)
                {
                    var role = hrsUser.HrsSecurity?.GetRole();
                    if (role != null && role.RoleType.Name == "HR I - Administrator")
                    {
                        var busReasonCode = hrsUser.Employee?.AsEmployee()?.BusinessRegionCode;
                        model.BusinessRegionCode = busReasonCode;
                    }
                }

                result.EmployeeModel = _helperEmployee.SaveEmployee(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetEmployeesForHrsModule")]
        public JsonResult GetEmployeesForHrsModule(string moduleName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                var securityRole = hrsUser.HrsSecurity.GetRole();
                if (securityRole == null) throw new Exception("You do not have any Hrs Role defined");

                var module = securityRole.Modules.FirstOrDefault(x => x.ModuleType.Name == moduleName);
                if (module == null) throw new Exception("You do not have access to this Module");

                result.Employees = _helperEmployee.GetAllMyEmployees(hrsUser, securityRole, module);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetAllEmployeeReferencesOfPossibleManagers/{employeeId}")]
        public JsonResult GetAllEmployeeReferencesOfPossibleManagers(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                if (hrsUser.HrsSecurity.GetRole() == null) throw new Exception("You do not have any Hrs Role defined");

                result.EmployeeReferences = _helperEmployee.GetAllEmployeeReferencesOfPossibleManagers(employeeId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetAuditHistory/{employeeId}")]
        public JsonResult GetAuditHistory(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                if (hrsUser.HrsSecurity.GetRole() == null) throw new Exception("You do not have any Hrs Role defined");

                result.EmployeeAuditTrailHistory = _helperEmployee.GetAuditTrailForEmployee(employeeId);

                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetAllMyEmployeeReferencesForDirectReports/{employeeId}")]
        public JsonResult GetAllMyEmployeeReferencesForDirectReports(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                if (hrsUser.HrsSecurity.GetRole() == null) throw new Exception("You do not have any Hrs Role defined");

                result.EmployeeReferences = _helperEmployee.GetAllMyEmployeeReferencesForDirectReports(employeeId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetAllEmployeeReferencesForLocation/{locationId}")]
        public JsonResult GetAllEmployeeReferencesForLocation(string locationId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                if (hrsUser.HrsSecurity.GetRole() == null) throw new Exception("You do not have any Hrs Role defined");

                result.EmployeeReferences = _helperEmployee.GetAllEmployeeReferencesForLocation(locationId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetAllEmployeeReferencesUnsecured")]
        public JsonResult GetAllEmployeeReferencesUnsecured()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            //var tokenData = GetTokenDataFromHeaders();
            //var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                //ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                //var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                //if (hrsUser.HrsSecurity.GetRole() == null)
                //{
                //    throw new Exception("You do not have any Hrs Role defined");
                //}

                result.EmployeeReferences = _helperEmployee.GetAllEmployeeReferences();
                result.Success = true;
            }
            catch (Exception ex)
            {
                //var parameters = GetParametersDictionary();
                //parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetAllEmployeeReferences")]
        public JsonResult GetAllEmployeeReferences()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                //var hrsRole = hrsUser.HrsSecurity.GetRole();
                //var hseRole = hrsUser.HseSecurity.GetRole();

                //if ((hrsRole == null) && (hseRole == null))
                //{
                //    throw new Exception("You do not have any Hrs or Hse Role defined");
                //}

                result.EmployeeReferences = _helperEmployee.GetAllEmployeeReferences();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetEmployeeRefForEmployee/{employeeId}")]
        public JsonResult GetEmployeeRefForEmployee(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var employee = _helperEmployee.GetEmployee(employeeId);
                result.EmployeeReference = employee.AsEmployeeRef();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetAllEmployeeReferencesBasedOnSecurityRoleModule/{employeeId}/{moduleTypeName}/{hrsRole}")]
        public JsonResult GetAllEmployeeReferencesBasedOnSecurityRoleModule(string employeeId, string moduleTypeName,
            bool hrsRole)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            var resultStatusCode = tokenData.StatusCodeResult;
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                if (hrsUser.HrsSecurity.GetRole() == null) throw new Exception("You do not have any Hrs Role defined");

                result.EmployeeReferences =
                    _helperEmployee.GetAllEmployeeReferencesBasedOnSecurityRoleModule(employeeId, moduleTypeName,
                        hrsRole);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return JsonResultWithStatusCode(result, resultStatusCode);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetAllJobTitles")]
        public JsonResult GetAllJobTitles()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.JobTitles = _helperEmployee.GetAllJobTitles();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetJobTitleModel/{jobTitleId}")]
        public JsonResult GetJobTitleModel(string jobTitleId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.JobTitleModel = _helperEmployee.GetJobTitleModel(jobTitleId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetNewJobTitleModel")]
        public JsonResult GetNewJobTitleModel()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.JobTitleModel = new JobTitleModel(new JobTitle());
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("employees/SaveJobTitle")]
        public JsonResult SaveJobTitle([FromBody] JobTitleModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                result.JobTitleModel = _helperEmployee.SaveJobTitle(model);
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetEmployeeRolodex")]
        public async Task<JsonResult> GetEmployeeRolodex()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var rolodex = new EmployeeRolodex();
                await rolodex.FetchResults();
                result.Rolodex = rolodex.Rolodex;
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetEmployeeRolodexForOffice/{office}")]
        public async Task<JsonResult> GetEmployeeRolodexForOffice(string office)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var queryHelper = new MongoRawQueryHelper<Employee>();
                var rolodex = new EmployeeRolodex
                {
                    Filter = queryHelper.FilterBuilder.Where(x => x.Location.Office == office)
                };
                await rolodex.FetchResults();
                result.Rolodex = rolodex.Rolodex;
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetEmployeeRolodexForOffice/{country}")]
        public async Task<JsonResult> GetEmployeeRolodexForCountry(string country)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);

                var queryHelper = new MongoRawQueryHelper<Employee>();
                var rolodex = new EmployeeRolodex
                {
                    Filter = queryHelper.FilterBuilder.Where(x => x.Location.Country == country)
                };
                await rolodex.FetchResults();
                result.Rolodex = rolodex.Rolodex;
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("employees/RemoveEmployee/{employeeId}")]
        public async Task<JsonResult> RemoveEmployee(string employeeId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);
                var hrsUser = _helperUser.GetHrsUser(tokenData.UserId);

                if (!hrsUser.SystemAdmin) throw new Exception("Only SystemAdmins can remove an employee");

                _helperEmployee.RemoveEmployee(employeeId);

                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                parameters.Add("UserId", tokenData.UserId);
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
                result.ErrorMessage = ex.Message;
                result.Success = false;
            }

            return JsonResultWithStatusCode(result, tokenData.StatusCodeResult);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("employees/GetGlobalHeadcount")]
        public JsonResult GetGlobalHeadcount()
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var tokenData = GetTokenDataFromHeaders();
            try
            {
                ThrowExceptionForBadToken(tokenData.StatusCodeResult);


                result.GlobalHeadcount = _helperEmployee.GetGlobalHeadCount();
                result.Success = true;
            }
            catch (Exception ex)
            {
                var parameters = GetParametersDictionary();
                _logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex,
                    parameters: parameters);
            }

            return JsonResultWithStatusCode(result, HttpStatusCode.OK);
        }
    }
}