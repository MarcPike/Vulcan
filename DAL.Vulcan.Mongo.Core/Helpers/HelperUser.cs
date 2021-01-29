using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using Location = DAL.Vulcan.Mongo.Core.DocClass.Locations.Location;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public class HelperUser : HelperBase, IHelperUser
    {
        private readonly HelperPerson _helperPerson = new HelperPerson();
        private readonly RepositoryBase<CrmUser> _repository = new RepositoryBase<DocClass.CRM.CrmUser>();
        private readonly RepositoryBase<CrmUserToken> _tokenRep = new RepositoryBase<CrmUserToken>();
        private readonly RepositoryBase<LdapUser> _ldapRep = new RepositoryBase<LdapUser>();
        public HelperUser()
        {
        }

        public IHelperPerson GetHelperPerson()
        {
            return _helperPerson;
        }

        public LdapUser LookupUserByNetworkId(string networkId)
        {
            return new RepositoryBase<LdapUser>().AsQueryable().FirstOrDefault(x => x.NetworkId.ToUpper() == networkId.ToUpper());
        }

        public void UserConnectedCommand(string application, string userId)
        {
            var user = GetCrmUser(application, userId);
            CrmUserLog.ConnectionMade(user.AsCrmUserRef());
        }

        //public void ControllerMethodCalled(CrmUserRef user, string controller, string method)
        //{
        //    CrmUserLog.ControllerMethodCalled(user, controller, method);
        //}

        //public void ControllerMethodCalled(string application, string userId, string controller, string method)
        //{
        //    var user = GetCrmUser(application, userId);
        //    CrmUserLog.ControllerMethodCalled(user.AsCrmUserRef(), controller, method);
        //}

        public CrmUserModel SaveCrmUserModel(CrmUserModel model)
        {
            var saveByUser = GetCrmUser(model.Application, model.UserId);

            var crmUser = GetCrmUser(model.Application, model.User.Id);

            if ((!saveByUser.IsAdmin) && (saveByUser.Id != crmUser.Id))
            {
                throw new Exception("Security level not adequate");
            }

            if ((!saveByUser.IsAdmin) && (model.IsAdmin != crmUser.IsAdmin))
            {
                throw new Exception("Security level not adequate");
            }

            if (crmUser == null) throw new Exception("User not found");

            crmUser.Contacts.ResyncWithList(model.Contacts);
            crmUser.Teams.ResyncWithList(model.Teams);
            crmUser.ViewConfig = model.ViewConfig;
            crmUser.UserType = (CrmUserType)Enum.Parse(typeof(CrmUserType), model.UserType, true);
            crmUser.IsAdmin = model.IsAdmin;
            crmUser.UseMyLocationForPdf = model.UseMyLocationForPdf;
            crmUser.ReadOnly = model.ReadOnly;

            var ldapUser = crmUser.User.AsUser();
            ldapUser.Person.UpdateFromModel(model.PersonalInfo);
            ldapUser.SaveToDatabase();
            crmUser.User = ldapUser.AsUserRef();
            crmUser.IsCalcAdmin = model.IsCalcAdmin;
            crmUser.SaveToDatabase();

            return new CrmUserModel(model.Application, model.UserId, crmUser);
        }

        public CrmUserModel GetCrmUserModel(string application, string userId)
        {
            var crmUser = GetCrmUser(application, userId);
            return new CrmUserModel(application,userId,crmUser);
        }

        public CrmUserModel ChangeUserLocation(string application, string userId, string userIdToModify, string moveToLocationId)
        {
            var crmUser = GetCrmUser(application, userId);
            if (!crmUser.IsAdmin)
            {
                throw new Exception("Only Admin can modify Location of user");
            }

            var location = new RepositoryBase<Location>().Find(moveToLocationId);

            if (location == null) throw new Exception("Location not found");

            var crmUserToModify = GetCrmUser(application, userIdToModify);

            var userToModify = crmUserToModify.User.AsUser();
            userToModify.Location = location.AsLocationRef();
            userToModify.SaveToDatabase();

            crmUserToModify = GetCrmUser(application, userIdToModify);
            return new CrmUserModel(application, userId, crmUserToModify);

        }

        public List<UserModel> GetAllAvailableNewUsersForApplication(string application, string lastName, List<UserModel> existingUsers)
        {
            lastName = lastName.ToLower();
            var allLdapUsers = new RepositoryBase<LdapUser>().AsQueryable().ToList();
            var allUsers = new List<UserModel>();
            foreach (var ldapUser in allLdapUsers.ToList())
            {
                if (ldapUser.LastName.ToLower().Contains(lastName))
                    allUsers.Add(new UserModel(ldapUser));
            }

            var existingUserIds = existingUsers.Select(x => x.Id).ToList();

            return allUsers.Where(x => !existingUserIds.Contains(x.Id)).ToList();
        }

        public List<UserModel> GetExistingUsersForApplication(string application)
        {
            var existingUsers = new List<UserModel>();
            var existing = new RepositoryBase<CrmUser>().AsQueryable().Where(x => x.Application == application).ToList();
            foreach (var crmUser in existing)
            {
                existingUsers.Add(new UserModel(crmUser));
            }
            return existingUsers;
        }

        public List<CrmUserRef> GetExistingUserReferencesForApplication(string application)
        {
            var users = new RepositoryBase<CrmUser>().AsQueryable().Where(x => x.Application == application).ToList();
            return users.Select(x=> new CrmUserRef(x)).ToList();
        }

        public TeamUserCompanyViewSelectionsModel GetTeamUserCompanyViewSelectionsModel(string application, string userId)
        {
            var crmUser = GetCrmUser(application, userId);
            var team = crmUser.ViewConfig.Team;
            if (team == null) throw new Exception("No Team is defined");

            var rep = new RepositoryBase<TeamUserCompanyViewSelections>();
            var view = rep.AsQueryable().SingleOrDefault(x => x.Team.Id == team.Id && x.User.Id == userId) 
                ?? new TeamUserCompanyViewSelections(crmUser.User,team);
            return new TeamUserCompanyViewSelectionsModel(application, userId, view);
        }

        public TeamUserCompanyViewSelectionsModel SaveTeamUserCompanyViewSelectionsModel(TeamUserCompanyViewSelectionsModel model)
        {
            var crmUser = GetCrmUser(model.Application, model.UserId);
            var team = crmUser.ViewConfig.Team;
            if (team == null) throw new Exception("No Team is defined");

            var view = TeamUserCompanyViewSelections.GetTeamUserCompanyViewSelections(crmUser.User, team);

            view.User = model.User;
            view.Alliances = model.Alliances;
            view.NonAlliances = model.NonAlliances;
            view.Prospects = model.Prospects;
            view.SaveToDatabase();
            TeamUserCompanyViewSelections.ReSynchTeam(team, crmUser.User);
            return new TeamUserCompanyViewSelectionsModel(model.Application,model.UserId,view);
        }

        public string GetUserCoid(CrmUser crmUser)
        {
            var user = crmUser.User.AsUser();
            var location = user.Location.AsLocation();
            var coid = location.GetCoid();
            return coid;
        }

        public CrmUserInfo GetCrmUserInfo(string application, string userId)
        {
            var crmUser = GetCrmUser(application, userId);
            if (crmUser == null) throw new ArgumentException("User not found");

            return new CrmUserInfo(crmUser);
        }

        //public TeamUserCompanyViewSelections SaveTeamUserView(string application, string userId)
        //{
        //    var crmUser = GetCrmUser(application, userId);
        //    var team = crmUser.ViewConfig.Team;
        //    if (team == null) throw new Exception("No Team is defined");

        //    var rep = new RepositoryBase<TeamUserCompanyViewSelections>();
        //    var companyViewSelections = rep.AsQueryable().SingleOrDefault(x => x.Team.Id == team.Id && x.User.Id == userId)
        //               ?? new TeamUserCompanyViewSelections(crmUser.User, team);
        //    return companyViewSelections;
        //}


        public UserPersonModel GetUserPersonModel(string userId)
        {
            var user = GetUser(userId);
            return new UserPersonModel(user);
        }

        public UserPersonModel SaveUserPersonModel(UserPersonModel model)
        {
            _helperPerson.ValidateModel(model);

            var user = GetUser(model.UserId);

            user.Person.Addresses = model.Addresses.Select(x => x.ToBaseValue()).ToList();
            user.Person.EmailAddresses = model.EmailAddresses.Select(x => x.ToBaseValue()).ToList();
            user.Person.PhoneNumbers = model.PhoneNumbers.Select(x => x.ToBaseValue()).ToList();

            user.Person.FirstName = model.FirstName;
            user.Person.LastName = model.LastName;
            user.Person.MiddleName = model.MiddleName;
            //user.Person.Notes.ResynchWithList(model.Notes);
            user.SaveToDatabase();
            return new UserPersonModel(user);
        }

        public DocClass.CRM.CrmUser GetSalesPerson(string application, string userId)
        {
            var crmUser = GetCrmUser(application, userId);
            if ((crmUser == null) || (crmUser.UserType != CrmUserType.SalesPerson))
            {
                throw new Exception("SalesPerson not found");
            }

            return crmUser;
        }


        public DocClass.CRM.CrmUser GetCrmUser(string application, string userId)
        {
            CrmUser crmUser = null;

            crmUser = SearchToken();
            crmUser = SearchUser();
            crmUser = SearchCrmUser();

            if (crmUser == null)
            {
                throw new Exception("User not found");
            }

            return crmUser;

            CrmUser SearchToken()
            {
                if (crmUser != null) return crmUser;
                var tokenUser = _tokenRep.Find(userId);
                if (tokenUser != null)
                {
                    crmUser = tokenUser.CrmUserRef.AsCrmUser();
                }

                return crmUser;
            }

            CrmUser SearchCrmUser()
            {
                return crmUser ??= _repository.AsQueryable()
                    .SingleOrDefault(x => x.Application == application && x.Id == ObjectId.Parse(userId));
            }

            CrmUser SearchUser()
            {
                return crmUser ??= _repository.AsQueryable()
                    .SingleOrDefault(x => x.Application == application && x.User.Id == userId);
            }

        }


        public LdapUser GetUser(string userId)
        {
            LdapUser user = null;

            user = SearchToken();
            user = SearchLdap();
            user = SearchCrmUser();

            if (user == null)
            {
                throw new Exception("No user exists");
            }

            //InitializePersonIfNeeded();

            return user;

            LdapUser SearchToken()
            {
                if (user != null) return user;
                var tokenUser = _tokenRep.Find(userId);
                if (tokenUser != null)
                {
                    user = tokenUser.User.AsUser();
                }

                return user;
            }

            LdapUser SearchLdap()
            {
                return user ?? _ldapRep.Find(userId);
            }

            LdapUser SearchCrmUser()
            {
                if (user != null) return user;
                var crmUser = _repository.Find(userId);
                if (crmUser != null)
                {
                    user = crmUser.User.AsUser();
                }

                return user;
            }

            //void InitializePersonIfNeeded()
            //{
            //    //if (user.Person != null)
            //    if (user.Person == null)
            //    {
            //        user.Person = new Person
            //        {
            //            FirstName = user.FirstName,
            //            LastName = user.LastName
            //        };
            //        _ldapRep.Upsert(user);
            //    }
            //}
        }

        public List<UserRef> GetAllEmployees(string application)
        {
            var users = new RepositoryBase<LdapUser>().AsQueryable().ToList();
            var result = new List<UserRef>();
            foreach (var user in users)
            {
                if (user.LastName != String.Empty)
                result.Add(new UserRef(user));
            }
            return result.OrderBy(x=>x.LastName).ThenBy(x=>x.LastName).ToList();
        }



        public List<CrmUserRef> GetAllSalesPersons(string application)
        {
            return _repository.AsQueryable()
                .Where(x => x.UserType != CrmUserType.Guest && x.UserType != CrmUserType.Resource).ToList()
                .Select(x => x.AsCrmUserRef()).ToList();
        }

        public List<CrmUserRef> GetAllManagers(string application)
        {
            return _repository.AsQueryable()
                .Where(x => x.UserType == CrmUserType.Manager && x.Application == application)
                .Select(x => new CrmUserRef(x)).ToList();
        }

        public List<CrmUserRef> GetAllDirectors(string application)
        {
            return _repository.AsQueryable()
                .Where(x => x.UserType == CrmUserType.Director && x.Application == application)
                .Select(x => new CrmUserRef(x)).ToList();
        }

        public List<CrmUserRef> GetAllAdmins(string application)
        {
            return _repository.AsQueryable()
                .Where(x => x.IsAdmin)
                .Select(x => new CrmUserRef(x)).ToList();
        }


        public (CrmUserToken token, bool expired, CrmUser crmUser) GetUserToken(string application, string userId)
        {
            var crmUser = GetCrmUser(application, userId);
            if (crmUser == null) throw new Exception("User not found");

            var tokenData = CrmUserToken.Get(crmUser);

            return (tokenData.token, tokenData.expired, crmUser);
        }

        public CrmUserToken GetNewUserToken(string application, string userId)
        {
            var crmUser = GetCrmUser(application, userId);
            if (crmUser == null) throw new Exception("User not found");

            var token = CrmUserToken.Create(crmUser);

            return token;
        }

        public void RemoveUser(string application, string userId)
        {
            var crmUser = GetCrmUser(application, userId);
            if (crmUser == null) throw new Exception("User not found");

            var userIdRemoved = crmUser.Id.ToString();

            _repository.RemoveOne(crmUser);
            foreach (var team in new RepositoryBase<Team>().AsQueryable().ToList())
            {
                foreach (var crmUserRef in team.CrmUsers.ToList())
                {
                    if (crmUserRef.Id == userIdRemoved)
                    {
                        team.CrmUsers.Remove(crmUserRef);
                        team.SaveToDatabase();
                    }
                }
            }
        }

        public CrmUserModel CreateAndOrSetUserAsUserType(string application, string userId, CrmUserType userType, bool isAdmin, bool readOnly, bool isCalcAdmin)
        {
            LdapUser user = GetUser(userId);
            CrmUser crmUser = null;
            try
            {
                crmUser = GetCrmUser(application, userId);
                crmUser.UserType = userType;
                crmUser.IsAdmin = isAdmin;
                crmUser.ReadOnly = readOnly;
                crmUser.IsCalcAdmin = isCalcAdmin;
                crmUser.SaveToDatabase();
            }
            catch (Exception)
            {
                crmUser = new CrmUser(user, userType)
                {
                    Application = application,
                    IsAdmin = isAdmin
                };
                crmUser.SaveToDatabase();
            }
            return new CrmUserModel(application,crmUser.User.Id,crmUser);
        }

    }
}