using System;
using System.Collections.Generic;
using System.Linq;
//using DAL.iMetal.SearchResults;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.DocClass.Locations;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.DocClass.Security
{
    public class SecurityManager
    {

        public SecurityManager()
        {
            //_ldapReader = new LdapReader(this);
        }

        //public LdapReader LdapReader
        //{
        //    get { return _ldapReader; }
        //}

        private Application _application;
        private AppTask _appTask;
        private Role _role;
        private LdapUser _currentUser;
        private Login _currentLogin;
        private Login _lastLogin;

        public readonly List<LdapUser> NewUserList = new List<LdapUser>();
        public readonly List<LdapUser> ModifiedUserList = new List<LdapUser>();
        public readonly List<Location> NewLocationList = new List<Location>();

        private readonly RepositoryBase<LdapUser> _userRepository = new RepositoryBase<LdapUser>();
        private readonly RepositoryBase<Application> _appRepository = new RepositoryBase<Application>();
        private readonly RepositoryBase<Login> _loginRepository = new RepositoryBase<Login>();
        private readonly RepositoryBase<Location> _locationRepository = new RepositoryBase<Location>();
        //private readonly LdapReader _ldapReader;

        public Application Application => _application;

        public void SaveAll()
        {
            _appRepository.Upsert(_application);
        }

        public List<ApplicationRole> GetAllApplicationRoles()
        {
            var result = new List<ApplicationRole>();
            
            if (_application == null) return result;

            result.AddRange(_application.Roles.ToList());

            return result;
        }

        public SecurityManager ForApplication(string name)
        {
            var appRepository = new RepositoryBase<Application>();
            _application = appRepository.Collection.AsQueryable().SingleOrDefault(x => x.Name == name);

            if (_application != null) return this;

            _application = new Application() {Id= ObjectId.GenerateNewId(), Name = name};
            try
            {
                _appRepository.Upsert(_application);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return this;
        }

        public SecurityManager ForTask(string taskName)
        {
            if (_application == null) throw new Exception("Application must be defined first");

            _appTask = _application.Tasks.SingleOrDefault(x => x.Name == taskName);

            if (_appTask == null)
            {
                _appTask = new AppTask()
                {
                    Name = taskName
                };

                _application.Tasks.Add(_appTask);

                SaveAll();

            }
            _role = null;

            return this;

        }


        public LdapUser LookupUser(string networkId)
        {
            var user = _userRepository.Collection.AsQueryable().SingleOrDefault(x => x.NetworkId == networkId);
            if (user == null) throw new Exception($"No user found with network id == {networkId}");
            return user;
        }

        public void CheckIfUserAccountDefined(LdapUser user)
        {
            if (_application.Users.All(x=>x.Id != user.Id))
                throw new Exception($"User does not have permission for application: {_application.Name}");
        }

        public void RemoveUser(string networkId)
        {
            var user = _userRepository.Collection.AsQueryable().SingleOrDefault(x => x.NetworkId == networkId);
            if (user == null) throw new Exception($"No user found with for {networkId}.");

            var apps = _appRepository.Collection.AsQueryable();
            foreach (var app in apps.ToList())
            {
                foreach (var appTask in app.Tasks)
                {
                    foreach (var role in appTask.Roles.ToList())
                    {
                        foreach (var roleMember in role.RoleMembers.Where(x=>x.User.NetworkId == networkId).ToList())
                        {
                            role.RoleMembers.Remove(roleMember);
                        }
                    }

                    foreach (var userPermission in appTask.UserPermissions.Where(x=>x.User.NetworkId == networkId).ToList())
                    {
                        appTask.UserPermissions.Remove(userPermission);
                    }
                }
                _appRepository.Upsert(app);
            }
            _application.Users.Remove(user);
            SaveAll();

            RefreshEverything();

        }

        private void RefreshEverything()
        {
            if (_application != null)
                _application = _appRepository.Collection.AsQueryable().SingleOrDefault(x => x.Id == _application.Id);

            if ((_application != null) && (_appTask != null))
            {
                _appTask = _application.Tasks.SingleOrDefault(x => x.Id == _appTask.Id);
                if ((_appTask != null) && (_role != null))
                {
                    _role = _appTask.Roles.SingleOrDefault(x => x.Id == _role.Id);
                    
                }
                if (_currentUser != null)
                {
                    _currentUser = _userRepository.Collection.AsQueryable()
                        .SingleOrDefault(x => x.Id == _currentUser.Id);
                }

                if (_currentLogin != null)
                {
                    _currentLogin =
                        _loginRepository.Collection.AsQueryable().SingleOrDefault(x => x.Id == _currentLogin.Id);
                }
            }
        }

        public SecurityManager DefineThisRole(string roleName, bool isRevoked)
        {
            ValidateTask();

            _role = _appTask.Roles.FirstOrDefault(x => x.Name == roleName);

            if (_role == null)
            {
                _role = new Role()
                    {
                        Name = roleName,
                    };
                _appTask.Roles.Add(_role);

                SaveAll();
            }
            return this;
        }

        public SecurityManager SetRolePermission(string networkId, bool isRevoked)
        {
            ValidateRole();

            var user = LookupUser(networkId);

            var memberFound = _role.RoleMembers.SingleOrDefault(x => x.User.NetworkId == networkId && x.IsAdmin == false);
            if (memberFound == null)
            {
                _role.RoleMembers.Add(new RoleMember()
                {
                    User = user,
                    IsRevoked = isRevoked,
                    IsAdmin = false
                });

                SaveAll();

            }
            else if (memberFound.IsRevoked != isRevoked)
            {
                memberFound.IsRevoked = isRevoked;
                SaveAll();
            }

            return this;
        }

        public SecurityManager SetAdminForThisRole(string networkId, bool isRevoked)
        {
            ValidateRole();

            var user = LookupUser(networkId);

            var memberFound = _role.RoleMembers.SingleOrDefault(x => x.User.NetworkId == networkId && x.IsAdmin == true);
            if (memberFound == null)
            {
                _role.RoleMembers.Add(new RoleMember()
                {
                    User = user,
                    IsRevoked = isRevoked,
                    IsAdmin = true,
                });

                SaveAll();

            }
            else if (memberFound.IsRevoked != isRevoked)
            {
                memberFound.IsRevoked = isRevoked;
                SaveAll();
            }

            return this;
        }

        private void ValidateRole()
        {
            if (_role == null) throw new Exception("You must first define a Role");
        }

        public SecurityManager SetTaskPermission(string networkId, bool isRevoked)
        {
            ValidateTask();

            var user = LookupUser(networkId);
            if (user == null) throw new Exception($"No user found with network id == {networkId}");

            var userPermissionFound = _appTask.UserPermissions.SingleOrDefault(x => x.User.NetworkId == networkId);
            if (userPermissionFound != null)
            {
                if (userPermissionFound.IsRevoked != isRevoked)
                    userPermissionFound.IsRevoked = isRevoked;
                SaveAll();
                return this;
            }

            var userPermission = new UserPermission()
            {
                User = user,
                IsRevoked = isRevoked
            };
            _appTask.UserPermissions.Add(userPermission);
            SaveAll();
            return this;
        }

        private void ValidateTask()
        {
            if (_appTask == null) throw new Exception("You must first define a Task");
        }

        public bool HasPermission(string taskName)
        {
            ValidateCurrentUser();
            return HasPermission(taskName, _currentUser.NetworkId);
        }

        public bool HasPermission(string taskName, string networkId)
        {
            ValidateApplication();

            var task = _application.Tasks.SingleOrDefault(x => x.Name == taskName);
            if (task == null) throw new Exception($"No task defined with name == [{taskName}]");

            var userPermission =
                task.UserPermissions.SingleOrDefault(x => x.User.NetworkId == networkId && x.IsRevoked == false);
            if (userPermission == null)
            {
                var rolePermission =
                    task.Roles.SingleOrDefault(
                        x => x.RoleMembers.Any(y => y.User.NetworkId == networkId && y.IsRevoked == false));
                if (rolePermission == null) return false;
            }

            return true;

        }

        public void LoginWithWindowsCredentials()
        {
            ValidateApplication();

            var userName = Environment.UserName;
            var user = LookupUser(userName);
            if (user == null)
            {
                throw new Exception($"User {userName} could not be found");
            }
            CaptureLogin(user);

        }

        public void LoginAsUser(string networkId)
        {
            ValidateApplication();
            var user = LookupUser(networkId);
            if (user == null)
            {
                throw new Exception($"User {networkId} could not be found");
            }
            CaptureLogin(user);
        }

        public void Logout()
        {
            if (_currentLogin == null) throw new Exception("No login defined.");
            if (_currentUser == null) throw new Exception("No user logged in.");

            _currentLogin.LogoutTime = DateTime.UtcNow;

            _loginRepository.Upsert(_currentLogin);
            _lastLogin = _currentLogin;

            _currentLogin = null;
            _currentUser = null;

        }

        private void CaptureLogin(LdapUser user)
        {
            _currentLogin = new Login()
            {
                ApplicationName = _application.Name,
                User = user,
                Id = ObjectId.GenerateNewId(),
                LoginTime = DateTime.UtcNow
            };

            _loginRepository.Upsert(_currentLogin);
            _userRepository.Upsert(user);
            _currentUser = user;
        }

        public string GetCurrentUserName()
        {
            ValidateCurrentUser();
            return _currentUser.UserName;
        }

        public string GetCurrentNetworkId()
        {
            ValidateCurrentUser();
            return _currentUser.NetworkId;
        }

        private void ValidateCurrentUser()
        {
            if (_currentUser == null) throw new Exception("No user currently logged in.");
        }


        public Login GetCurrentLogin()
        {
            if (_currentLogin == null) throw new Exception("No current login exists.");
            return _currentLogin;
        }

        public List<Login> GetAllLoginsFor(string networkId)
        {
            var user = LookupUser(networkId);
            if (user == null)
            {
                throw new Exception($"User {networkId} could not be found");
            }

            return _loginRepository.AsQueryable().Where(x => x.User.NetworkId == networkId).ToList();
        }

        public List<Login> GetAllLoginsForThisApplication()
        {
            ValidateApplication();

            return _loginRepository.AsQueryable().Where(x => x.ApplicationName == _application.Name).ToList();
        }

        public TimeSpan GetRuntimeTotalFor(string networkId)
        {
            var result = new TimeSpan();
            var logins = GetAllLoginsFor(networkId);
            foreach (var login in logins)
            {
                if (login.LogoutTime != DateTime.MaxValue) 
                {
                    var timeSpan = result.Add(login.LogoutTime - login.LoginTime);
                    result += timeSpan;
                }

                // Add current if they have not logged out
                if ((login.Id == _currentLogin.Id) && (login.LogoutTime == DateTime.MaxValue))
                {
                    var timespan = result.Add(DateTime.UtcNow - login.LoginTime);
                    result += timespan;
                }
            }
            return result;
        }

        public List<AppTask> GetAppTasks(string appName)
        {
            _application = _appRepository.Collection.AsQueryable().SingleOrDefault(x => x.Name == appName);
            if (_application == null) throw new Exception($"No Application found with name of {appName}.");

            return _application.Tasks;
        }

        public AppTask GetTaskForThisApplication(string taskName)
        {
            ValidateApplication();
            return GetTaskCalled(taskName);
        }

        public List<string> GetTaskNamesForThisApplication()
        {
            ValidateApplication();
            return _application.Tasks.Select(x => x.Name).ToList();
        }

        private void ValidateApplication()
        {
            if (_application == null) throw new Exception("You have not defined your security model yet.");
        }

        public List<LdapUser> UsersWithoutPermissionFor(string taskName)
        {
            var task = GetTaskForThisApplication(taskName);
            return UsersWithoutPermissionFor(task);
        }

        public List<LdapUser> UsersWithoutPermissionFor(AppTask task)
        {
            return task.GetAllUsersWithoutPermission();
        }

        public List<LdapUser> UsersWithPermissionFor(string taskName)
        {
            var task = GetTaskForThisApplication(taskName);
            return UsersWithPermissionFor(task);
        }

        public List<LdapUser> UsersWithPermissionFor(AppTask task)
        {
            return task.GetAllUsersWithPermission();
        }

        private Application FindApplicationWithTask(AppTask task)
        {
            return _appRepository.Collection.AsQueryable().SingleOrDefault(x => x.Tasks.Any(t => t.Id == task.Id));
        }

        public void RemovePermissionFor(string taskName, string networkId)
        {
            var task = GetTaskForThisApplication(taskName);
            RemovePermissionFor(task,networkId);
        }

        public void RemovePermissionFor(AppTask task, string networkId)
        {
            task = _application.Tasks.Single(x => x.Id == task.Id);

            foreach (var role in task.Roles.ToList())
            {
                foreach (var roleMember in role.RoleMembers.Where(x=>x.User.NetworkId == networkId).ToList())
                {
                    role.RoleMembers.Remove(roleMember);
                }
            }

            foreach (var userPermission in task.UserPermissions.Where(x=>x.User.NetworkId == networkId).ToList())
            {
                task.UserPermissions.Remove(userPermission);
            }

            _appRepository.Upsert(_application);
        }

        public void RemoveRole(string taskName, string roleName)
        {
            var task = GetTaskCalled(taskName);
        }

        private AppTask GetTaskCalled(string taskName)
        {
            var task = _application.Tasks.SingleOrDefault(x => x.Name == taskName);
            if (task == null) throw new Exception($"No task found with name = {taskName}");
            return task;
        }

        public void RemoveApp(string appName)
        {
            var appFound = _appRepository.Collection.AsQueryable().SingleOrDefault(x => x.Name == appName);
            if (appFound == null) throw new Exception($"No app was found called {appName}");

            _appRepository.RemoveOne(appFound);
            RefreshEverything();
        }

        public bool AppExists(string appName)
        {
            return _appRepository.Collection.AsQueryable().SingleOrDefault(x => x.Name == appName) != null;
        }

        public List<Application> GetAllApps()
        {
            return _appRepository.Collection.AsQueryable().OrderBy(x => x.Name).ToList();
        }

        public void AddApplicationRole(string roleName)
        {
            if (_application.Roles.All(x => x.Role.Name != roleName))
            {
                _application.Roles.Add(
                    new ApplicationRole()
                    {
                        Role = new Role()
                        {
                            Name = roleName
                        }
                    });
            }
        }

        public void AddUsersToApplicationRole(string roleName, List<LdapUser> users)
        {
            if (_application.Roles.All(x=>x.Role.Name != roleName))
                throw new Exception("Role does not exist");

            var appRole = _application.Roles.Single(x => x.Role.Name == roleName);
            foreach (var user in users)
            {
                var roleMember = appRole.Role.RoleMembers.FirstOrDefault(x=>x.User.Id == user.Id);

                if (roleMember == null)
                {
                    appRole.Role.RoleMembers.Add(new RoleMember()
                    {
                        User = user,
                        IsAdmin = false,
                        CreatedByUserId = "admin"
                    });
                } else if (roleMember.IsRevoked)
                {
                    roleMember.IsRevoked = false;
                }

                if (roleName == "SalesPerson")
                {
                    var salesPerson = new CrmUser(user, CrmUserType.SalesPerson);
                    salesPerson.SaveToDatabase();
                }
                if (roleName == "Manager")
                {
                    var manager = new CrmUser(user, CrmUserType.Manager);
                    manager.SaveToDatabase();
                }
                if (roleName == "Director")
                {
                    var director = new CrmUser(user, CrmUserType.Director);
                    director.SaveToDatabase();
                }

            }
            SaveAll();
            
        }

        public void AddUserToApplicationRole(string roleName, LdapUser user)
        {
            if (_application.Roles.All(x => x.Role.Name != roleName))
                throw new Exception("Role does not exist");

            var appRole = _application.Roles.Single(x => x.Role.Name == roleName);
            var roleMember = appRole.Role.RoleMembers.FirstOrDefault(x => x.User.Id == user.Id);

            if (roleMember == null)
            {
                appRole.Role.RoleMembers.Add(new RoleMember()
                {
                    User = user,
                    IsAdmin = false,
                    CreatedByUserId = "admin"
                });
            }
            else if (roleMember.IsRevoked)
            {
                roleMember.IsRevoked = false;
            }
            SaveAll();
        }

        public List<string> GetUserRoles(LdapUser user)
        {
            var roleNames = new List<string>();
            var roles = GetAllApplicationRoles().ToList();
            foreach (var role in roles.ToList())
            {
                if (role.Role.RoleMembers.Any(x => x.User.Id == user.Id))
                {
                    roleNames.Add(role.Role.Name);
                }
            }
            return roleNames;
        }

        public List<AppTask> GetUserTasks(LdapUser user)
        {
            var result = new List<AppTask>();
            var tasks = _application.Tasks.ToList();
            foreach (var task in tasks.ToList())
            {
                if (task.UserPermissions.Any(x => x.User.Id == user.Id && x.IsRevoked == false))
                {
                    result.Add(task);
                }
            }
            return result;
        }

    }

}
