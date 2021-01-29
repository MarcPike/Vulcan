using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Security;
using DAL.HRS.Mongo.DocClass.SecurityRole;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.Ldap.DAL.WindowsAuthentication.MongoDb;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperUser : HelperBase, IHelperUser
    {
        private readonly IHelperPerson _helperPerson;

        //private readonly RepositoryBase<LdapUser> _ldapRep = new RepositoryBase<LdapUser>();
        //private readonly RepositoryBase<HrsUserToken> _tokenRep = new RepositoryBase<HrsUserToken>();
        public HelperUser()
        {
            _helperPerson = new HelperPerson();
        }

        public List<HrsUserGridModel> GetAllHrsUsers()
        {
            var hrsUsers = HrsUser.Helper.GetAll();
            
            return hrsUsers.Select(x=> new HrsUserGridModel(x)).OrderBy(x=>x.FullName).ToList();
        }

        public LdapUser LookupUserByNetworkId(string networkId)
        {
            networkId = networkId.ToUpper();
            return LdapUser.Helper.GetAll().FirstOrDefault(x => x.NetworkId.ToUpper() == networkId.ToUpper());

            //return new RepositoryBase<LdapUser>().AsQueryable().FirstOrDefault();
        }

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
            user.SaveToDatabase();
            return new UserPersonModel(user);
        }

        public HrsUserModel SaveUser(HrsUserModel model)
        {
            var helperSecurity = new HelperSecurity();

            return helperSecurity.SaveHrsUserModel(model);
        }


        public HrsUser GetHrsUser(string userId)
        {
            var ldapUser = GetUser(userId);

            var user = new RepositoryBase<HrsUser>().AsQueryable().SingleOrDefault(x => x.UserId == ldapUser.Id.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user;
        }


        public List<LdapUserRef> GetNewUserList()
        {
            var currentUserList = new RepositoryBase<HrsUser>().AsQueryable().Select(x=>x.UserId).ToList();


            var allLdapUsers = new RepositoryBase<LdapUser>().AsQueryable().ToList();

            var otherLdapUsers = allLdapUsers
                .Where(x => currentUserList.All(u => u != x.Id.ToString())).ToList();

            var result = new List<LdapUserRef>();

            foreach (var ldapUser in otherLdapUsers)
            {
                try
                {
                    result.Add(ldapUser.AsLdapUserRef());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return result.
                OrderBy(x=>x.LastName).
                ThenBy(x=>x.FirstName).ToList();
        }


        public HrsUserModel ChangeHrsUserRole(string userId, string roleId)
        {
            var hrsUser = GetHrsUser(userId);

            var queryHelperHrsUser = new MongoRawQueryHelper<HrsUser>();
            var queryHelperSecurityRole = new MongoRawQueryHelper<SecurityRole>();

            var securityRole = queryHelperSecurityRole.FindById(roleId);
            if (securityRole == null) throw new Exception("Role not found");

            hrsUser.HrsSecurity.RoleId = securityRole.Id;
            queryHelperHrsUser.Upsert(hrsUser);

            return new HrsUserModel(hrsUser);
        }

        public HrsUserModel ChangeHseUserRole(string userId, string roleId)
        {
            var hrsUser = GetHrsUser(userId);

            var queryHelperHrsUser = new MongoRawQueryHelper<HrsUser>();
            var queryHelperSecurityRole = new MongoRawQueryHelper<SecurityRole>();

            var securityRole = queryHelperSecurityRole.FindById(roleId);
            if (securityRole == null) throw new Exception("Role not found");

            hrsUser.HseSecurity.RoleId = securityRole.Id;
            queryHelperHrsUser.Upsert(hrsUser);

            return new HrsUserModel(hrsUser);
        }


        public HrsUserModel CopyHrsSecurityRole(string fromUserId, string toUserId)
        {
            var fromHrsUser = GetHrsUser(fromUserId);
            var toHrsUser = GetHrsUser(toUserId);

            var securityRoleClone = fromHrsUser.HrsSecurity.GetRole();
            securityRoleClone.SaveToDatabase();
            toHrsUser.HrsSecurity.RoleId = securityRoleClone.Id;
            toHrsUser.SaveToDatabase();

            return new HrsUserModel(toHrsUser);
        }

        public HrsUserModel CopyHseSecurityRole(string fromUserId, string toUserId)
        {
            var fromHrsUser = GetHrsUser(fromUserId);
            var toHrsUser = GetHrsUser(toUserId);

            var securityRoleClone = fromHrsUser.HseSecurity.GetRole();
            securityRoleClone.SaveToDatabase();
            toHrsUser.HseSecurity.RoleId = securityRoleClone.Id;
            toHrsUser.SaveToDatabase();

            return new HrsUserModel(toHrsUser);
        }

        public HrsUserModel GetUserModel(string userId)
        {
            var ldapUser = GetUser(userId);

            var user = new RepositoryBase<HrsUser>().AsQueryable().SingleOrDefault(x => x.UserId == ldapUser.Id.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return new HrsUserModel(user);
        }

        public HrsUserModel GetHseUserModel(string userId)
        {
            var ldapUser = GetUser(userId);

            var user = new RepositoryBase<HrsUser>().AsQueryable().SingleOrDefault(x => x.UserId == ldapUser.Id.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return new HrsUserModel(user);
        }


        public LdapUser GetUser(string userId)
        {

            LdapUser user = null;

            user = SearchToken() ?? SearchLdap();

            if (user == null)
            {
                throw new Exception("No user exists");
            }

            //InitializePersonIfNeeded();

            return user;

            LdapUser SearchToken()
            {
                return user ?? HrsUserToken.Helper.FindById(userId)?.User?.AsLdapUser();
            }

            LdapUser SearchLdap()
            {
                return user ?? LdapUser.Helper.FindById(userId);
            }

        }

        public List<LdapUserRef> GetAllLdapUsers(string lastNameContains)
        {
            var ldapReader = new LdapReader();
            ldapReader.RefreshUserListFromLdap();

            var queryHelper = new MongoRawQueryHelper<LdapUser>();
            var filter = queryHelper.FilterBuilder.Where(x =>
                lastNameContains == string.Empty || x.LastName.Contains(lastNameContains));
            var project = queryHelper.ProjectionBuilder.Expression(x =>  new LdapUserRef(x));
            var result = queryHelper.FindWithProjection(filter, project);

            return result.OrderBy(x=>x.LastName).ThenBy(x=>x.FirstName).ToList();
        }

        public (HrsUserToken token, bool expired, LdapUser user) GetUserToken(string userId)
        {
            var user = GetUser(userId);
            if (user == null) throw new Exception("User not found");

            var tokenData = HrsUserToken.Get(user);

            return (tokenData.token, tokenData.expired, user);
        }

        public HrsUserToken GetNewUserToken(string userId)
        {
            var user = GetUser(userId);
            if (user == null) throw new Exception("User not found");

            var token = HrsUserToken.Create(user);

            return token;
        }


    }
}