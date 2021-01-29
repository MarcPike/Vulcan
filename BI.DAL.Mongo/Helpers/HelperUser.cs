using System;
using System.Collections.Generic;
using System.Linq;
using BI.DAL.Mongo.BiUserObjects;
using BI.DAL.Mongo.Models;
using BI.DAL.Mongo.Security;
using DAL.Common.DocClass;
using DAL.Common.Ldap.DAL.WindowsAuthentication.MongoDb;
using DAL.Common.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Driver;

namespace BI.DAL.Mongo.Helpers
{
    public class HelperUser : HelperBase, IHelperUser
    {
        private readonly IHelperPerson _helperPerson;

        public HelperUser()
        {
            _helperPerson = new HelperPerson();
        }

        public List<BiUserModel> GetAllBiUsers()
        {
            return BiUser.Helper.GetAll().Select(x=> new BiUserModel(x)).ToList();
        }

        public LdapUser LookupUserByNetworkId(string networkId)
        {
            networkId = networkId.ToUpper();
            return LdapUser.Helper.GetAll().FirstOrDefault(x => x.NetworkId.ToUpper() == networkId.ToUpper());

            //return new RepositoryBase<LdapUser>().AsQueryable().FirstOrDefault();
        }

        public BiUserModel GetBiUserModel(string userId)
        {
            var user = GetBiUser(userId);

            return new BiUserModel(user);
        }

        public BiUserModel SaveBiUser(BiUserModel model)
        {
            _helperPerson.ValidateModel(model);

            var user = GetBiUser(model.UserId);

            user.Person.Addresses = model.Person.Addresses.ToList();
            user.Person.EmailAddresses = model.Person.EmailAddresses.ToList();
            user.Person.PhoneNumbers = model.Person.PhoneNumbers.ToList();

            user.Person.FirstName = model.Person.FirstName;
            user.Person.LastName = model.Person.LastName;
            user.Person.MiddleName = model.Person.MiddleName;
            user.SaveToDatabase();
            return new BiUserModel(user);
        }

        public LdapUser GetLdapUser(string userId)
        {
            return LdapUser.Helper.FindById(userId);
        }


        public BiUser GetBiUser(string userId)
        {

            var user = BiUser.Helper.Find(x => x.User.Id == userId).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user;
        }


        public List<LdapUserRef> GetNewUserList()
        {
            var currentUserList = new RepositoryBase<BiUser>().AsQueryable().Select(x=>x.UserId).ToList();


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
                return user ?? BiUserToken.Helper.FindById(userId)?.User?.AsLdapUser();
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

        public (BiUserToken token, bool expired, LdapUser user) GetUserToken(string userId)
        {
            var user = GetUser(userId);
            if (user == null) throw new Exception("User not found");

            var tokenData = BiUserToken.Get(user);

            return (tokenData.token, tokenData.expired, user);
        }

        public BiUserToken GetNewUserToken(string userId)
        {
            var user = GetUser(userId);
            if (user == null) throw new Exception("User not found");

            var token = BiUserToken.Create(user);

            return token;
        }


    }
}