using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Security;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Reflection;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.SqlServer.Model;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;
using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperSecurity : HelperBase, IHelperSecurity
    {
        private readonly IHelperUser _helperUser = new HelperUser();

        public HelperSecurity()
        {

        }

        public string EncodeToBase64(string normalText)
        {
            try
            {
                byte[] encData_byte = new byte[normalText.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(normalText);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public string DecodeFrom64(string encodedData)
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

        public HrsUserToken Authenticate(string networkIdEncrypted, string passwordEncrypted)
        {
            var networkId = DecodeFrom64(networkIdEncrypted);
            var password = DecodeFrom64(passwordEncrypted);

            if (networkId.Contains("@"))
            {
                var ldapUser = LdapUser.Helper.Find(x=>x.Person.EmailAddresses.Any(e=>e.Address == networkId)).FirstOrDefault();
                if (ldapUser != null)
                {
                    networkId = ldapUser.NetworkId;
                }
            }

            if (!CheckAuthenticate(networkId, password))
            {
                //statusCode = HttpStatusCode.Unauthorized;
                throw new Exception("User could not be authenticated");

            }

            var user = _helperUser.LookupUserByNetworkId(networkId);
            var userToken = _helperUser.GetNewUserToken(user.Id.ToString());
            return userToken;

        }

        private bool CheckAuthenticate(string userName, string password)
        {
            try
            {
                using (LdapConnection connection = new LdapConnection("howcogroup.com:389"))
                {
                    connection.SessionOptions.ProtocolVersion = 3;
                    connection.SessionOptions.Signing = true;
                    connection.SessionOptions.Sealing = true;
                    //connection.AuthType = AuthType.Kerberos;
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
            catch (LdapException ex)
            {
                return false;
            }

            return true;
        }


        //private bool CheckAuthenticate(string networkId, string password)
        //{
        //    if (networkId.ToUpper() == "TESTUSER") return true;

        //    const int ldapVersion = Novell.Directory.Ldap.LdapConnection.Ldap_V3;
        //    var conn = new Novell.Directory.Ldap.LdapConnection();

        //    try
        //    {
        //        conn.Connect(HelperLdapConstants.Host, HelperLdapConstants.Port);
        //        conn.Bind(ldapVersion, $"{HelperLdapConstants.Domain}\\{networkId}", password);
        //    }
        //    catch (Exception ex)
        //    {
        //        var parameters = GetParametersDictionary();
        //        parameters.Add("networkId", networkId);
        //        //parameters.Add("normalText", normalText);
        //        Logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name,
        //            sendEmail: false, exception: ex, parameters: parameters);
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
        //            //parameters.Add("normalText", normalText);
        //            Logger.Log(className: GetClassName(), methodName: MethodBase.GetCurrentMethod().Name,
        //                sendEmail: false, exception: ex, parameters: parameters);
        //        }
        //    }

        //    return true;
        //}

        public HrsUserToken Impersonate(string adminUserIdEncrypted, string networkIdEncrypted)
        {
            var adminUserId = DecodeFrom64(adminUserIdEncrypted);
            var networkId = DecodeFrom64(networkIdEncrypted);


            var admin = _helperUser.GetHrsUser(adminUserId);

            try
            {

                if (!admin.SystemAdmin) throw new Exception("You are not authorized");

                var user = _helperUser.LookupUserByNetworkId(networkId);
                if (user == null) throw new Exception("User not found");

                var tokenData = _helperUser.GetUserToken(user.Id.ToString());
                ;

                if (tokenData.token == null)
                {
                    tokenData.token = _helperUser.GetNewUserToken(user.Id.ToString());
                }

                if (tokenData.expired)
                {
                    tokenData.token = _helperUser.GetNewUserToken(user.Id.ToString());
                }

                return tokenData.token;
            }
            catch (Exception ex)
            {
                Logger.Log(GetClassName(), MethodBase.GetCurrentMethod().Name, ex);
                throw ex;
            }

        }

        //public HrsUserModel AddUser(string userId, string hrsRoleTypeId, string hseRoleTypeId, EntityRef entity)
        //{
        //    var hrsUserCheck = new RepositoryBase<HrsUser>().AsQueryable().SingleOrDefault(x => x.UserId == userId);
        //    if (hrsUserCheck != null) throw new Exception("User already exists");

        //    var ldapUser = _helperUser.GetUser(userId);

        //    var hrsSecurityRole = (hrsRoleTypeId != "none") ? SecurityRoleFactory.GetSecurityRole(hrsRoleTypeId) : null;
        //    var hseSecurityRole = (hseRoleTypeId != "none") ? SecurityRoleFactory.GetSecurityRole(hseRoleTypeId) : null;

        //    if ((hrsSecurityRole == null) && (hseSecurityRole == null))
        //    {
        //        throw new Exception("You must define an HRS Role or an HSE Role");
        //    }

        //    var hrsUser = HrsUser.CreateHrsUser(ldapUser, hrsSecurityRole, hseSecurityRole);

        //    if (hrsUser.Employee == null)
        //    {
        //        throw new Exception("Could not find an active Employee record for this user");
        //    }

        //    var isSystemAdmin = (hrsSecurityRole != null) && (hrsSecurityRole.RoleType.Name.Contains("SystemAdmin"));
        //    if (!isSystemAdmin) isSystemAdmin = (hseSecurityRole != null) && (hseSecurityRole.RoleType.Name.Contains("SystemAdmin"));

        //    hrsUser.SystemAdmin = isSystemAdmin;
        //    hrsUser.Entity = entity;
        //    hrsUser.SaveToDatabase();

        //    return new HrsUserModel(hrsUser);
        //}

        public List<NewHrsUserModel> GetHrsNewUserList()
        {
            var hrsUsers = new MongoRawQueryHelper<HrsUser>().GetAll();
            var objectIds = hrsUsers.Select(x => x.Employee.Id).ToList();

            var queryHelper = new MongoRawQueryHelper<Employee>();
            //var filter = queryHelper.FilterBuilder.Where(x=> objectIds.All(i=> i !=  x.Id ));
            var filter = queryHelper.FilterBuilder.Empty;
            var project = queryHelper.ProjectionBuilder.Expression(x => new { x.Id, x.FirstName, x.LastName, x.MiddleName, x.PreferredName, x.Location, x.PayrollRegion, x.PayrollId, x.LdapUser });

            var data = queryHelper.FindWithProjection(filter, project).Select(x=> new NewHrsUserModel(new EmployeeRef()
            {
                Id = x.Id.ToString(),
                LastName = x.LastName,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                PreferredName = x.PreferredName,
                Location = x.Location,
                PayrollRegion = x.PayrollRegion,
                PayrollId = x.PayrollId
            },  x.LdapUser));

            //var missingUser = data.SingleOrDefault(x => x.Employee.PayrollId == "SG357");

            var results = data.Where(x => objectIds.All(i => i != x.Employee.Id)).OrderBy(x => x.Employee.LastName).ThenBy(x => x.Employee.FirstName).ToList();
            
            //missingUser = results.SingleOrDefault(x => x.Employee.PayrollId == "SG357");

            return results;

        }

        public HrsUserModel AddUser(string employeeId, string userId, string hrsRoleId, string hseRoleId, EntityRef entity)
        {
            var hrsUserCheck = new RepositoryBase<HrsUser>().AsQueryable().SingleOrDefault(x => x.Employee.Id == employeeId);
            if (hrsUserCheck != null) throw new Exception("User already exists");

            var queryHelperEmployee = new MongoRawQueryHelper<Employee>();
            var queryHelperLdapUser = new MongoRawQueryHelper<LdapUser>();

            var ldapUser = queryHelperLdapUser.FindById(userId);

            if (ldapUser == null)
            {
                throw new Exception("Invalid UserId specified");
            }

            var employee = queryHelperEmployee.FindById(employeeId);

            if (ldapUser.Location.Office == "<unknown>")
            {
                ldapUser.Location = employee.Location;
                queryHelperLdapUser.Upsert(ldapUser);
            }

            if (employee.LdapUser == null)
            {
                employee.LdapUser = ldapUser.AsLdapUserRef();
                queryHelperEmployee.Upsert(employee);
            }

            var queryHelperSecurityRole = new MongoRawQueryHelper<SecurityRole>();

            var hrsSecurityRole = (hrsRoleId != "none") ? queryHelperSecurityRole.FindById(hrsRoleId) : null;
            var hseSecurityRole = (hseRoleId != "none") ? queryHelperSecurityRole.FindById(hseRoleId) : null;

            if ((hrsSecurityRole == null) && (hseSecurityRole == null))
            {
                throw new Exception("You must define an HRS Role or an HSE Role");
            }

            var hrsUser = HrsUser.CreateHrsUser(ldapUser, hrsSecurityRole, hseSecurityRole, employee.AsEmployeeRef());
            
            var isSystemAdmin = (hrsSecurityRole != null) && (hrsSecurityRole.RoleType.Name.Contains("SystemAdmin"));
            if (!isSystemAdmin) isSystemAdmin = (hseSecurityRole != null) && (hseSecurityRole.RoleType.Name.Contains("SystemAdmin"));

            hrsUser.SystemAdmin = isSystemAdmin;
            hrsUser.Entity = entity;
            hrsUser.SaveToDatabase();

            return new HrsUserModel(hrsUser);
        }

        public void RemoveHrsUser(string userId)
        {
            var rep = new RepositoryBase<HrsUser>();
            var removeUser = rep.AsQueryable().FirstOrDefault(x => x.UserId == userId);
            if (removeUser != null)
            {
                rep.RemoveOne(removeUser);
            }

        }

        public AppPermissionModel CreateAppPermission(string label, string description)
        {

            var rep = new RepositoryBase<AppPermission>();


            if (rep.AsQueryable().Any(x => x.Label == label))
            {
                throw new Exception("Permission with this Label already exists");
            }

            var appPermission = rep.Upsert(new AppPermission()
            {
                Label = label,
                Description = description
            });

            return new AppPermissionModel(appPermission);
        }

        public void RemoveAppPermission(string appPermissionId)
        {

            var rep = new RepositoryBase<AppPermission>();

            var permId = ObjectId.Parse(appPermissionId);
            var appPermission = rep.AsQueryable().FirstOrDefault(x => x.Id == permId);
            if (appPermission != null)
                rep.RemoveOne(appPermission);

        }

        public List<AppPermissionModel> GetAllAppPermissions()
        {
            return  new RepositoryBase<AppPermission>().AsQueryable().ToList().Select(x => new AppPermissionModel(x)).ToList();
        }

        public List<SecurityRoleModel> GetAllRoles()
        {
            return new RepositoryBase<SecurityRole>().AsQueryable().
                OrderBy(x=>x.RoleType.Name).ToList().
                Select(x=> new SecurityRoleModel(x)).ToList();
        }

        public List<SecurityRoleTypeRef> GetAllRoleTypes()
        {
            return new RepositoryBase<SecurityRoleType>().AsQueryable().
                OrderBy(x => x.Name).ToList().
                Select(x => x.AsSecurityRoleTypeRef()).ToList();
        }

        public List<SystemModuleTypeRef> GetAllModuleTypes()
        {
            return new RepositoryBase<SystemModuleType>().AsQueryable().
                OrderBy(x => x.Name).ToList().
                Select(x => x.AsSystemModuleTypeRef()).ToList();
        }

        public List<SystemModuleModel> GetAllModules()
        {
            return new RepositoryBase<SystemModuleType>().AsQueryable().
                OrderBy(x => x.Name).ToList().
                Select(x => new SystemModuleModel(x.AsSystemModuleTypeRef())).ToList();
        }

        //private List<SecurityRoleTypeRef> GetAllRoleTypes()
        //{
        //    var roleTypes = new RepositoryBase<SecurityRoleType>().AsQueryable().ToList();
        //    var result = new List<SecurityRoleTypeRef>();
        //    foreach (var securityRoleType in roleTypes)
        //    {
        //        result.Add(new SecurityRoleTypeRef(securityRoleType));
        //    }

        //    return result;
        //}

        public List<SystemModuleTypeRef> GetAllHrsModuleTypes()
        {
            return new RepositoryBase<SystemModuleType>().AsQueryable().Where(x=>x.IsHrsModule).ToList()
                .Select(x=> x.AsSystemModuleTypeRef()).OrderBy(x=>x.Name).ToList();
        }

        public List<SystemModuleTypeRef> GetAllHseModuleTypes()
        {
            return new RepositoryBase<SystemModuleType>().AsQueryable().Where(x => x.IsHseModule).ToList()
                .Select(x => x.AsSystemModuleTypeRef()).OrderBy(x => x.Name).ToList();
        }


        public SecurityRoleTypeRef DefineNewRoleType(string roleName, bool isHrs, bool isHse, bool autoSave=true)
        {
            var rep = new RepositoryBase<SecurityRoleType>();

            var rolesWithSameName = rep.AsQueryable().Where(x => x.Name == roleName).ToList();
            foreach (var roleType in rolesWithSameName)
            {
                if (roleType.IsHrsRole && isHrs)
                {
                    throw new Exception("Hrs Role already exists with same name");
                }
                if (roleType.IsHseRole && isHse)
                {
                    throw new Exception("Hse Role already exists with same name");
                }
            }

            var securityRoleType = new SecurityRoleType()
            {
                Name = roleName,
                IsHrsRole = isHrs,
                IsHseRole = isHse,
               
            };
            autoSave = true;
            if (autoSave)
            {
                securityRoleType.SaveToDatabase();
            }
            return securityRoleType.AsSecurityRoleTypeRef();
        }



        public SystemModuleTypeRef DefineNewModuleType(string name, bool isHrsModule, bool isHseModule)
        {
            var rep = new RepositoryBase<SystemModuleType>();

            var modulesWithThisName = rep.AsQueryable().Where(x => x.Name == name).ToList();
            foreach (var systemModuleType in modulesWithThisName)
            {
                if (isHrsModule && systemModuleType.IsHrsModule)
                {
                    throw new Exception("Hrs module already exists with this name");
                }
                if (isHseModule && systemModuleType.IsHseModule)
                {
                    throw new Exception("Hse module already exists with this name");
                }
            }

            var newModuleType = new SystemModuleType()
            {
                IsHrsModule = isHrsModule,
                IsHseModule = isHseModule,
                Name = name
            };

            newModuleType = rep.Upsert(newModuleType);
            return newModuleType.AsSystemModuleTypeRef();
        }

        public SecurityRoleModel SaveRole(SecurityRoleModel model)
        {
            var queryHelperRoleType = new MongoRawQueryHelper<SecurityRoleType>();
            var queryHelperRole = new MongoRawQueryHelper<SecurityRole>();


            var roleTypeFilter = queryHelperRoleType.FilterBuilder.Where(x => x.Name == model.RoleType.Name);
            var roleType = queryHelperRoleType.Find(roleTypeFilter).FirstOrDefault();

            //var roleType = queryHelperRoleType.FindById(model.RoleType.Id);

            if (roleType == null)
            {
                //var filterDuplicateName = queryHelperRoleType.FilterBuilder.
                //    Where(x => x.Name == model.RoleType.Name);

                //var roleTypeNameAlreadyExists = queryHelperRoleType.Find(filterDuplicateName).FirstOrDefault();

                //if (roleTypeNameAlreadyExists != null) 
                //    throw new Exception($"Role name [{model.RoleType.Name}] already exists. Choose a different name.");

                roleType = new SecurityRoleType()
                {
                    Id = ObjectId.Parse(model.RoleType.Id),
                    Name = model.RoleType.Name,
                    IsHrsRole = model.RoleType.IsHrsRole,
                    IsHseRole = model.RoleType.IsHseRole
                };
                queryHelperRoleType.Upsert(roleType);
            }

            var role = queryHelperRole.FindById(model.Id) ?? new SecurityRole()
            {
                Id = ObjectId.Parse(model.Id),
                RoleType = roleType.AsSecurityRoleTypeRef(),
            };

            role.DirectReportsOnly = model.DirectReportsOnly;
            role.Modules = model.Modules.Select(x=> x.AsSystemModule()).ToList();
            queryHelperRole.Upsert(role);

            return new SecurityRoleModel(role);
        }

        public List<HrsUserModel> UsersInRole(string roleId)
        {
            var result = new List<HrsUserModel>();
            var id = ObjectId.Parse(roleId);
            var users = new RepositoryBase<HrsUser>().AsQueryable().Where(x =>
                x.HrsSecurity.RoleId == id || x.HseSecurity.RoleId == id).ToList();
            foreach (var hrsUser in users)
            {
                result.Add(new HrsUserModel(hrsUser));
            }

            return result;
        }

        public void RemoveRole(string roleId)
        {
            var queryHelperSecurityRole = new MongoRawQueryHelper<SecurityRole>();
            var queryHelperHrsUsers = new MongoRawQueryHelper<HrsUser>();
            var queryHelperSecurityRoleType = new MongoRawQueryHelper<SecurityRoleType>();

            var securityRole = queryHelperSecurityRole.FindById(roleId);

            var filterUsersUsingThisRole = queryHelperHrsUsers.FilterBuilder.Where(x =>
                (x.HrsSecurity.RoleId == securityRole.Id) ||
                (x.HseSecurity.RoleId == securityRole.Id));
            var usersUsingThisRole = queryHelperHrsUsers.Find(filterUsersUsingThisRole).ToList();
            if (usersUsingThisRole.Any())
            {
                throw new Exception($"{usersUsingThisRole.Count} are using this role. Cannot delete.");
            }


            var roleTypeId = securityRole.RoleType.Id;
            queryHelperSecurityRoleType.DeleteOne(roleTypeId);
            queryHelperSecurityRole.DeleteOne(securityRole.Id);


        }

        public void RemoveModuleType(string moduleTypeId)
        {
            var rep = new RepositoryBase<SystemModuleType>();
            var id = ObjectId.Parse(moduleTypeId);

            var moduleType = rep.AsQueryable().FirstOrDefault(x => x.Id == id);
            if (moduleType == null)
            {
                throw new Exception("Module not found");
            }

            var roleUsingThisModule = new RepositoryBase<SecurityRole>().AsQueryable()
                .FirstOrDefault(x => x.Modules.Any(m => m.ModuleType.Id == moduleTypeId));

            if (roleUsingThisModule != null)
            {
                throw new Exception($"{roleUsingThisModule.RoleType.Name} is using this module. Cannot remove.");
            }

            rep.RemoveOne(moduleType);
        }

        public bool GetHasHrsPermission(string userId, string moduleTypeId, string permissionLabel)
        {
            var hrsUser = _helperUser.GetHrsUser(userId);
            var securityRole = hrsUser.HrsSecurity.GetRole();

            var moduleType = new RepositoryBase<SystemModuleType>().Find(moduleTypeId);
            if (moduleType == null)
            {
                throw new Exception("Module Type not found");
            }

            if (securityRole == null) throw new Exception("No Role defined for user");

            var module = securityRole.GetModuleFor(moduleType.AsSystemModuleTypeRef());
            if (module == null) throw new Exception($"Missing module: {module}");

            var permissions = new RepositoryBase<UserAppPermissions>().AsQueryable()
                .FirstOrDefault(x=> x.User.Id == userId && x.SecurityRoleType.Id == hrsUser.HrsSecurity.RoleId.ToString() && x.ModuleType.Id == moduleTypeId);

            if (permissions == null) return false;

            if (permissions.RevokedAppPermissions.Any(x => x.Label == permissionLabel)) return false;

            if (permissions.GrantedAppPermissions.Any(x => x.Label == permissionLabel)) return true;

            return false;
        }

        public bool GetHasHsePermission(string userId, string moduleTypeId, string permissionLabel)
        {
            var hrsUser = _helperUser.GetHrsUser(userId);
            var securityRole = hrsUser.HseSecurity.GetRole();
            var moduleType = new RepositoryBase<SystemModuleType>().Find(moduleTypeId);
            if (moduleType == null)
            {
                throw new Exception("Module Type not found");
            }

            if (securityRole == null) throw new Exception("No Role defined for user");

            var permissions = new RepositoryBase<UserAppPermissions>().AsQueryable()
                .FirstOrDefault(x => x.User.Id == userId && x.SecurityRoleType.Id == hrsUser.HseSecurity.RoleId.ToString() && x.ModuleType.Id == moduleTypeId);

            if (permissions == null) return false;

            if (permissions.RevokedAppPermissions.Any(x => x.Label == permissionLabel)) return false;

            if (permissions.GrantedAppPermissions.Any(x => x.Label == permissionLabel)) return true;

            return false;
        }

        public SecurityRoleModel GetSecurityRoleModelForName(string roleName)
        {
            
            var queryHelper = new MongoRawQueryHelper<SecurityRole>();
            var filter = queryHelper.FilterBuilder.Where(x => x.RoleType.Name == roleName);
            var securityRole = queryHelper.Find(filter).FirstOrDefault();
            if (securityRole == null) throw new Exception($"No role found with name == {roleName}");
            return new SecurityRoleModel(securityRole);
        }

        public SecurityRole AddNewRole(SecurityRoleTypeRef roleType)
        {

            var queryHelperRole = new MongoRawQueryHelper<SecurityRole>();

            var role = new SecurityRole()
            {
                Id = ObjectId.GenerateNewId(),
                RoleType = roleType, 
                DirectReportsOnly = true
            };

            //role.DirectReportsOnly = model.DirectReportsOnly;
            //role.Modules = model.Modules.Select(x => x.AsSystemModule()).ToList();
            queryHelperRole.Upsert(role);
            return role;
        }



        public SecurityRoleModel GetSecurityRoleModelForRoleType(string roleTypeId)
        {
            var queryHelper = new MongoRawQueryHelper<SecurityRole>();
            var filter = queryHelper.FilterBuilder.Where(x => x.RoleType.Id == roleTypeId);
            var securityRole = queryHelper.Find(filter).FirstOrDefault();
            if (securityRole == null) throw new Exception($"No role found");
            return new SecurityRoleModel(securityRole);
        }

        //public SecurityRoleModel GetSecurityRole(string roleId)
        //{
        //    var securityRole = new MongoRawQueryHelper<SecurityRole>().FindById(roleId);
        //    return new SecurityRoleModel(securityRole);
        //}

        public SecurityRoleModel GetSecurityRoleModel(string roleId)
        {
            var id = ObjectId.Parse(roleId);
            var queryHelper = new MongoRawQueryHelper<SecurityRole>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Id == id);
            var securityRole = queryHelper.Find(filter).FirstOrDefault();
            return new SecurityRoleModel(securityRole);
        }

        public SystemModuleModel GetSystemModuleModel(string moduleTypeId)
        {
            var moduleType = new RepositoryBase<SystemModuleType>().Find(moduleTypeId);
            if (moduleType == null) throw new Exception("Module not found");

            return GetSystemModuleModel(moduleType);
        }

        public SystemModuleModel GetSystemModuleModel(SystemModuleType moduleType)
        {
            if (moduleType == null) throw new Exception("Module not found");

            return new SystemModuleModel(new SystemModule()
            {
                ModuleType = moduleType.AsSystemModuleTypeRef()
            });
        }

        private SystemModuleType GetModuleType(string moduleTypeId)
        {
            return new RepositoryBase<SystemModuleType>().Find(moduleTypeId);
        }

        public FileOperations GetFileOperationsForModule(string userId, string moduleTypeId)
        {
            var hrsUser = _helperUser.GetHrsUser(userId);
            var moduleType = GetModuleType(moduleTypeId);
            if (moduleType == null) throw new Exception("Module not found");

            var module = hrsUser.HrsSecurity.GetRole()?.Modules.FirstOrDefault(x => x.ModuleType.Id == moduleTypeId);
            if (module != null) return module.FileOperations;

            module = hrsUser.HseSecurity.GetRole()?.Modules.FirstOrDefault(x => x.ModuleType.Id == moduleTypeId);
            if (module != null) return module.FileOperations;

            throw new Exception("Module not available for user");
        }


        //public List<LocationRef> GetHrsUserRoleModuleLocations(string userId,
        //    string securityRoleTypeId, string systemModuleTypeId)
        //{
        //    var result = new List<LocationRef>();
        //    var hrsUser = _helperUser.GetHrsUser(userId);
        //    var securityRoleType = new RepositoryBase<SecurityRoleType>().Find(securityRoleTypeId);
        //    var systemModuleType = new RepositoryBase<SystemModuleType>().Find(systemModuleTypeId);

        //    if (hrsUser == null) throw new Exception("User not found");
        //    if (securityRoleType == null) throw new Exception("Role Type not found");
        //    if (systemModuleType == null) throw new Exception("Module Type not found");

        //    var hrsUserRef = hrsUser.AsHrsUserRef();
        //    var roleType = securityRoleType.AsSecurityRoleTypeRef();
        //    var moduleType = systemModuleType.AsSystemModuleTypeRef();

        //    var hrsUserRoleModuleLocations = new RepositoryBase<HrsUserRoleModuleLocations>().AsQueryable().
        //        FirstOrDefault(x=>x.HrsUser.Id == hrsUserRef.Id && x.RoleType.Id == roleType.Id && x.ModuleTypeId == moduleType.Id );

        //    if (hrsUserRoleModuleLocations != null)
        //    {
        //        result.AddRange(hrsUserRoleModuleLocations.Locations);
        //    }

        //    return result;
        //}

        //public List<LocationRef> AddHrsUserRoleModuleLocation(string userId,
        //    string securityRoleTypeId, string systemModuleTypeId, string locationId)
        //{
        //    var result = new List<LocationRef>();
        //    var hrsUser = _helperUser.GetHrsUser(userId);
        //    var securityRoleType = new RepositoryBase<SecurityRoleType>().Find(securityRoleTypeId);
        //    var systemModuleType = new RepositoryBase<SystemModuleType>().Find(systemModuleTypeId);
        //    var location = new RepositoryBase<Location>().Find(locationId);

        //    if (hrsUser == null) throw new Exception("User not found");
        //    if (securityRoleType == null) throw new Exception("Role Type not found");
        //    if (systemModuleType == null) throw new Exception("Module Type not found");
        //    if (location == null) throw new Exception("Location not found");

        //    var hrsUserRef = hrsUser.AsHrsUserRef();
        //    var roleType = securityRoleType.AsSecurityRoleTypeRef();
        //    var moduleType = systemModuleType.AsSystemModuleTypeRef();
        //    var locationRef = location.AsLocationRef();

        //    var hrsUserRoleModuleLocations = new RepositoryBase<HrsUserRoleModuleLocations>().AsQueryable().
        //        FirstOrDefault(x => x.HrsUser.Id == hrsUserRef.Id && x.RoleType.Id == roleType.Id && x.ModuleTypeId == moduleType.Id) 
        //        ?? new HrsUserRoleModuleLocations()
        //        {
        //            HrsUser = hrsUserRef,
        //            RoleType = roleType,
        //            ModuleTypeId = moduleType.Id
        //        };

        //    if (hrsUserRoleModuleLocations.Locations.All(x => x.Id != locationRef.Id))
        //    {
        //        hrsUserRoleModuleLocations.Locations.Add(locationRef);
        //        hrsUserRoleModuleLocations.SaveToDatabase();
        //    }

        //    return hrsUserRoleModuleLocations.Locations;
        //}

        //public List<LocationRef> RemoveHrsUserRoleModuleLocation(string userId,
        //    string securityRoleTypeId, string systemModuleTypeId, string locationId)
        //{
        //    var result = new List<LocationRef>();
        //    var hrsUser = _helperUser.GetHrsUser(userId);
        //    var securityRoleType = new RepositoryBase<SecurityRoleType>().Find(securityRoleTypeId);
        //    var systemModuleType = new RepositoryBase<SystemModuleType>().Find(systemModuleTypeId);
        //    var location = new RepositoryBase<Location>().Find(locationId);

        //    if (hrsUser == null) throw new Exception("User not found");
        //    if (securityRoleType == null) throw new Exception("Role Type not found");
        //    if (systemModuleType == null) throw new Exception("Module Type not found");
        //    if (location == null) throw new Exception("Location not found");

        //    var hrsUserRef = hrsUser.AsHrsUserRef();
        //    var roleType = securityRoleType.AsSecurityRoleTypeRef();
        //    var moduleType = systemModuleType.AsSystemModuleTypeRef();
        //    var locationRef = location.AsLocationRef();

        //    var hrsUserRoleModuleLocations = new RepositoryBase<HrsUserRoleModuleLocations>().AsQueryable().
        //                                         FirstOrDefault(x => x.HrsUser.Id == hrsUserRef.Id && x.RoleType.Id == roleType.Id && x.ModuleTypeId == moduleType.Id)
        //                                     ?? new HrsUserRoleModuleLocations()
        //                                     {
        //                                         HrsUser = hrsUserRef,
        //                                         RoleType = roleType,
        //                                         ModuleTypeId = moduleType.Id
        //                                     };

        //    var removeLocation = hrsUserRoleModuleLocations.Locations.FirstOrDefault(x => x.Id == locationRef.Id);

        //    if (removeLocation != null)
        //    {
        //        hrsUserRoleModuleLocations.Locations.Remove(removeLocation);
        //        hrsUserRoleModuleLocations.SaveToDatabase();
        //    }

        //    return hrsUserRoleModuleLocations.Locations;
        //}

        public SecurityRoleTypeRef GetRoleTypeForName(string roleName)
        {
            var securityRoleType = new RepositoryBase<SecurityRoleType>().AsQueryable().SingleOrDefault(x=>x.Name == roleName);

            if (securityRoleType == null) throw new Exception("RoleType not found");

            return securityRoleType.AsSecurityRoleTypeRef();
        }

        public SystemModuleTypeRef GetHrsModuleTypeForName(string moduleName)
        {
            var systemModuleType = new RepositoryBase<SystemModuleType>().AsQueryable().SingleOrDefault(x => x.Name == moduleName && x.IsHrsModule);

            if (systemModuleType == null) throw new Exception("ModuleType not found");

            return systemModuleType.AsSystemModuleTypeRef();
        }

        public SystemModuleTypeRef GetHseModuleTypeForName(string moduleName)
        {
            var systemModuleType = new RepositoryBase<SystemModuleType>().AsQueryable().SingleOrDefault(x => x.Name == moduleName && x.IsHseModule);

            if (systemModuleType == null) throw new Exception("ModuleType not found");

            return systemModuleType.AsSystemModuleTypeRef();
        }

        public UserAppPermissionsModel GetUserHrsAppPermissionForModule(string userId, string moduleTypeId)
        {
            var hrsUser = _helperUser.GetHrsUser(userId);
            if (hrsUser == null) throw new Exception("User not found");

            var moduleType = new RepositoryBase<SystemModuleType>().Find(moduleTypeId);
            if (moduleType == null) throw new Exception("Module not found");

            var securityRole = hrsUser.HrsSecurity.GetRole();

            var permissions = new RepositoryBase<UserAppPermissions>().AsQueryable()
                                  .SingleOrDefault(x=>x.SecurityRoleType == securityRole.RoleType && x.ModuleType.Id == moduleTypeId && x.User.Id == userId) ??
                              new UserAppPermissions()
                              {
                                  User = hrsUser.AsHrsUserRef(),
                                  SecurityRoleType = securityRole.RoleType,
                                  ModuleType = moduleType.AsSystemModuleTypeRef(),
                              };

            return new UserAppPermissionsModel(permissions);
        }

        public UserAppPermissionsModel GetUserHseAppPermissionForModule(string userId, string moduleTypeId)
        {
            var hrsUser = _helperUser.GetHrsUser(userId);
            if (hrsUser == null) throw new Exception("User not found");

            var moduleType = new RepositoryBase<SystemModuleType>().Find(moduleTypeId);
            if (moduleType == null) throw new Exception("Module not found");

            var securityRole = hrsUser.HseSecurity.GetRole();

            var permissions = new RepositoryBase<UserAppPermissions>().AsQueryable()
                                  .SingleOrDefault(x => x.SecurityRoleType == securityRole.RoleType && x.ModuleType.Id == moduleTypeId && x.User.Id == userId) ??
                              new UserAppPermissions()
                              {
                                  User = hrsUser.AsHrsUserRef(),
                                  SecurityRoleType = securityRole.RoleType,
                                  ModuleType = moduleType.AsSystemModuleTypeRef(),
                              };

            return new UserAppPermissionsModel(permissions);
        }

        public UserAppPermissionsModel SaveUserAppPermissionsForModule(UserAppPermissionsModel model)
        {
            var rep = new RepositoryBase<UserAppPermissions>();
            var userAppPermissions = rep.Find(model.Id);
            if (userAppPermissions == null)
            {
                userAppPermissions = new UserAppPermissions()
                {
                    Id = ObjectId.Parse(model.Id),
                    SecurityRoleType = model.SecurityRoleType,
                    ModuleType = model.ModuleType,
                    User = model.User
                };
            }

            userAppPermissions.GrantedAppPermissions = model.GrantedAppPermissions;
            userAppPermissions.RevokedAppPermissions = model.RevokedAppPermissions;
            userAppPermissions = rep.Upsert(userAppPermissions);
            return new UserAppPermissionsModel(userAppPermissions);
        }

        public HrsUserModel GetHrsUserModel(string userId)
        {
            var rep = new RepositoryBase<HrsUser>();
            var hrsUser = rep.AsQueryable().FirstOrDefault(x => x.UserId == userId);
            return new HrsUserModel(hrsUser);
        }

        public HrsUserModel SaveHrsUserModel(HrsUserModel model)
        {
            var rep = new RepositoryBase<HrsUser>();
            var hrsUser = rep.AsQueryable().FirstOrDefault(x => x.UserId == model.UserId);
            if (hrsUser == null) throw new Exception("User not found");


            var queryHelperSecurityRole = new MongoRawQueryHelper<SecurityRole>();

            if (model.HrsSecurity?.Role != null)
            {
                var securityRoleHrs = queryHelperSecurityRole.FindById(model.HrsSecurity.Role.Id);
                if (securityRoleHrs != null)
                {
                    hrsUser.HrsSecurity.RoleId = securityRoleHrs.Id;
                    hrsUser.HrsSecurity.Locations = model.HrsSecurity.Locations;
                    hrsUser.HrsSecurity.PayrollRegionsForCompensation = model.HrsSecurity.PayrollRegionsForCompensation;
                    hrsUser.HrsSecurity.MedicalLocations = model.HrsSecurity.MedicalLocations;
                }
            }
            else
            {
                hrsUser.HrsSecurity.RoleId = ObjectId.Empty;
            }

            if (model.HseSecurity?.Role != null)
            {

                var securityRoleHse = queryHelperSecurityRole.FindById(model.HseSecurity.Role.Id);
                if (securityRoleHse != null)
                {
                    hrsUser.HseSecurity.RoleId = securityRoleHse.Id;
                    hrsUser.HseSecurity.Locations = model.HseSecurity.Locations;
                    hrsUser.HseSecurity.MedicalLocations = model.HseSecurity.MedicalLocations;
                }

            }
            else
            {
                hrsUser.HseSecurity.RoleId = ObjectId.Empty;
            }

            hrsUser.AllowedPermissions = model.AllowedPermissions;
            hrsUser.RevokedPermissions = model.RevokedPermissions;

            hrsUser.SystemAdmin = model.SystemAdmin;
            hrsUser.Entity = model.Entity;
            hrsUser.Location = model.Location;
            hrsUser.ViewAllEntities = model.ViewAllEntities;
            rep.Upsert(hrsUser);

            return new HrsUserModel(hrsUser);

        }

        public SecurityRoleModel CreateNewRole(SecurityRoleTypeRef roleTypeRef)
        {
            var securityRole = new SecurityRole()
            {
                RoleType = roleTypeRef,
            };
            SecurityRole.Helper.Upsert(securityRole);
            return new SecurityRoleModel(securityRole);
        }
    }
}
