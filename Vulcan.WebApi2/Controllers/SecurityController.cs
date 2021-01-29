using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using DAL.WindowsAuthentication.MongoDb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.DirectoryServices.Protocols;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vulcan.WebApi2.Helpers;
using DAL.Vulcan.Mongo.Base.Models;

namespace Vulcan.WebApi2.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class SecurityController : BaseController
    {
        private readonly IHelperApplication _helperApplication;
        private readonly ILdapHelper _ldapHelper;
        private readonly IHelperPermissions _helperPermissions;
        //private VulcanLogger _logger;

        public SecurityController(
            IHelperApplication helperApplication, 
            IHelperTeam helperTeam,
            ILdapHelper ldapHelper,
            IHelperPermissions helperPermissions,
            IHelperUser helperUser) : base(helperUser)
        {
            _helperApplication = helperApplication;
            _ldapHelper = ldapHelper;
            _helperPermissions = helperPermissions;
            //_logger = new VulcanLogger("Security", new VulcanLoggerConfig());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("security/Heartbeat")]
        public async Task<JsonResult> HeartBeat()
        {
            dynamic result = new ExpandoObject();
            var statusCode = HttpStatusCode.OK;
            result.Success = false;
            try
            {
                await Task.Run(() =>
                {
                    var stringResult = new StringBuilder();
                    for (int i = 0; i < 200; i++)
                    {
                        stringResult.AppendLine("I am running ok!");
                    }

                    result.StringValue = stringResult.ToString();
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
        [Route("security/RefreshLdapUsers/{application}/{userId}")]
        public async Task<JsonResult> RefreshLdapUsers(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    UserAuthentication.RefreshAll();
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
        [Route("security/Impersonate/{application}/{adminUserIdEncrypted}/{networkIdEncrypted}")]
        public async Task<JsonResult> Impersonate(string application, string adminUserIdEncrypted, string networkIdEncrypted)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var adminUserId = DecodeFrom64(adminUserIdEncrypted);
            var networkId = DecodeFrom64(networkIdEncrypted);


            var admin = GetCrmUser(application, adminUserId);
            var statusCode = CheckToken(application, adminUserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);

                    if (!admin.IsAdmin) throw new Exception("You are not authorized");

                    var user = _helperUser.LookupUserByNetworkId(networkId);
                    if (user == null) throw new Exception("User not found");

                    var userToken = _helperUser.GetNewUserToken(application, user.Id.ToString());

                    result.UserToken = userToken;
                    result.Expired = userToken.Expires < DateTime.Now;

                    result.UserInfo = _helperUser.GetCrmUserInfo(application, user.Id.ToString());
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
        [Route("security/GetImpersonateList/{application}/{adminUserIdEncrypted}")]
        public async Task<JsonResult> GetImpersonateList(string application, string adminUserIdEncrypted)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var adminUserId = DecodeFrom64(adminUserIdEncrypted);
            var admin = GetCrmUser(application, adminUserId);
            var statusCode = CheckToken(application, adminUserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    if (!admin.IsAdmin) throw new Exception("You are not authorized");

                    result.UserList = new RepositoryBase<CrmUser>().AsQueryable()
                        .Select(x => new { x.User.UserName, x.User.NetworkId }).ToList();

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
        [Route("security/Authenticate")]
        public async Task<JsonResult> Authenticate([FromBody] DAL.Vulcan.Mongo.Base.Models.AuthenticationModel model)
        {
            dynamic result = new ExpandoObject();
            string networkId = string.Empty;
            string password = string.Empty;
            result.Success = false;
            var statusCode = HttpStatusCode.OK;
            try
            {
                await Task.Run(() =>
                {
                    networkId = DecodeFrom64(model.NetworkId);
                    password = DecodeFrom64(model.Password);

                    if (networkId.Contains("@"))
                    {
                        var ldapUser = new RepositoryBase<LdapUser>().AsQueryable().FirstOrDefault(x => x.Person.EmailAddresses.Any(e => e.Address == networkId));
                        if (ldapUser != null)
                        {
                            networkId = ldapUser.NetworkId;
                        }
                    }

                    if (!Authenticate(networkId, password))
                    {
                        statusCode = HttpStatusCode.Forbidden;
                        throw new Exception("User could not be authenticated");

                    }

                    var user = _helperUser.LookupUserByNetworkId(networkId);

                    result.UserToken = _helperUser.GetNewUserToken("vulcancrm", user.Id.ToString());
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


        //[AllowAnonymous]
        //[HttpGet]
        //[Route("security/Authenticate/{application}/{networkIdEncrypted}/{passwordEncrypted}")]
        //public async Task<JsonResult> Authenticate(string application, string networkIdEncrypted, string passwordEncrypted)
        //{
        //    dynamic result = new ExpandoObject();
        //    string networkId = string.Empty;
        //    string password = string.Empty;
        //    result.Success = false;
        //    var statusCode = HttpStatusCode.OK;
        //    try
        //    {
        //        networkId = DecodeFrom64(networkIdEncrypted);
        //        password = DecodeFrom64(passwordEncrypted);

        //        if (networkId.Contains("@"))
        //        {
        //            var ldapUser = new RepositoryBase<LdapUser>().AsQueryable().FirstOrDefault(x => x.Person.EmailAddresses.Any(e=>e.Address == networkId));
        //            if (ldapUser != null)
        //            {
        //                networkId = ldapUser.NetworkId;
        //            }
        //        }

        //        if (!Authenticate(networkId, password))
        //        {
        //            //statusCode = HttpStatusCode.Unauthorized;
        //            throw new Exception("User could not be authenticated");

        //        }

        //        var user = _helperUser.LookupUserByNetworkId(networkId);

        //        result.UserToken = _helperUser.GetNewUserToken(application, user.Id.ToString());
        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var parameters = GetParametersDictionary();
        //        parameters.Add("networkIdEncrypted", networkIdEncrypted);
        //        //parameters.Add("passwordEncrypted", passwordEncrypted);
        //        parameters.Add("networkId", networkId);
        //        //parameters.Add("password", password);
        //        //_logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: true, exception: ex, parameters: parameters);
        //        result.Success = false;
        //        result.ErrorMessage = ex.Message;
        //        //statusCode = HttpStatusCode.Unauthorized;
        //    }

        //    return JsonResultWithStatusCode(result, statusCode);
        //}

        [AllowAnonymous]
        [HttpGet]
        [Route("security/AddNewPermission/{application}/{userId}/{name}")]
        public async Task<JsonResult> AddNewPermission(string application, string userId, string name)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.PermissionModel = _helperPermissions.AddNewPermission(application, userId, name);
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
        [Route("security/RemovePermission/{application}/{userId}/{id}")]
        public async Task<JsonResult> RemovePermission(string application, string userId, string id)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    _helperPermissions.RemovePermission(id);
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
        [Route("security/GetPermissionModelForId/{application}/{userId}/{id}")]
        public async Task<JsonResult> GetPermissionModelForId(string application, string userId, string id)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.PermissionModel = _helperPermissions.GetPermissionModelForId(application, userId, id);
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
        [Route("security/GetPermissionModelForName/{application}/{userId}/{name}")]
        public async Task<JsonResult> GetPermissionModelForName(string application, string userId, string name)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.PermissionModel = _helperPermissions.GetPermissionModelForName(application, userId, name);
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
        [Route("security/SavePermissionModel")]
        public async Task<JsonResult> SavePermissionModel([FromBody] PermissionModel model)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(model.Application, model.UserId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.PermissionModel = _helperPermissions.SavePermissionModel(model);
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
        [Route("security/UserHasPermissionForPermissionId/{application}/{userId}/{permissionId}")]
        public async Task<JsonResult> UserHasPermissionForPermissionId(string application, string userId, string permissionId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.HasPermission = _helperPermissions.UserHasPermissionForPermissionId(application, userId, permissionId);
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
        [Route("security/UserHasPermissionForPermissionName/{application}/{userId}/{permissionName}")]
        public async Task<JsonResult> UserHasPermissionForPermissionName(string application, string userId, string permissionName)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.HasPermission = _helperPermissions.UserHasPermissionForPermissionName(application, userId, permissionName);
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
        [Route("security/GetAllPermissions/{application}/{userId}")]
        public async Task<JsonResult> GetAllPermissions(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.Permissions = _helperPermissions.GetAllPermissions(application, userId);
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
        [Route("security/GetAllPermissionsForUserId/{application}/{userId}")]
        public async Task<JsonResult> GetAllPermissionsForUserId(string application, string userId)
        {
            dynamic result = new ExpandoObject();
            result.Success = false;
            var statusCode = CheckToken(application, userId);
            try
            {
                await Task.Run(() =>
                {
                    ThrowExceptionForBadToken(statusCode);
                    result.Permissions = _helperPermissions.GetAllPermissionsForUserId(application, userId);
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

        private static bool Authenticate(string userName, string password)
        {
            try
            {
                using (LdapConnection connection = new LdapConnection("howcogroup.com:389"))
                {
                    connection.SessionOptions.ProtocolVersion = 3;
                    connection.SessionOptions.Signing = true;
                    connection.SessionOptions.Sealing = true;
                    //connection.AuthType = AuthType.Negotiate;  //AuthType.Kerberos;
                    //connection.SessionOptions.SecureSocketLayer = true;

                    NetworkCredential credential = new NetworkCredential
                    {
                        UserName = userName,
                        Password = password,
                        Domain = "howcogroup.com"
                    };

                    connection.Credential = credential;

                    connection.Bind();
                    connection.Dispose();
                }
            }
            catch (LdapException)
            {
                return false;
            }

            return true;
        }


        //private bool Authenticate(string networkId, string password)
        //{
        //    const int ldapVersion = Novell.Directory.Ldap.LdapConnection.Ldap_V3;
        //    var conn = new Novell.Directory.Ldap.LdapConnection();

        //    try
        //    {
        //        conn.Connect(_ldapHelper.Host, _ldapHelper.Port);
        //        conn.Bind(ldapVersion, $"{_ldapHelper.Domain}\\{networkId}", password);
        //    }
        //    catch (Exception ex)
        //    {
        //        var parameters = GetParametersDictionary();
        //        parameters.Add("networkId", networkId);
        //        //parameters.Add("password", password);
        //        _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: false, exception: ex, parameters: parameters);
        //        return false;
        //    }
        //    finally
        //    {
        //        try
        //        {
        //            if (conn.Connected) conn.Disconnect();
        //        }
        //        catch (Exception ex)
        //        {
        //            var parameters = GetParametersDictionary();
        //            parameters.Add("networkId", networkId);
        //            //parameters.Add("password", password);
        //            _logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name, sendEmail: false, exception: ex, parameters: parameters);
        //        }
        //    }
        //    return true;
        //}

        private string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        private string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

    }
}