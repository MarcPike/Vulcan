using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using BI.DAL.Mongo.Security;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System.DirectoryServices.Protocols;
using BI.DAL.Mongo.AppPermissions;
using BI.DAL.Mongo.Models;
using BI.DAL.Mongo.BiUserObjects;

namespace BI.DAL.Mongo.Helpers
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

        public BiUserToken Authenticate(string networkIdEncrypted, string passwordEncrypted)
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

            var biUser = BiUser.Helper.Find(x => x.UserId == user.Id.ToString()).FirstOrDefault();

            if (biUser == null) throw new Exception("User has not been added");

            var userToken = _helperUser.GetNewUserToken(user.Id.ToString());
            return userToken;

        }

        public bool CheckAuthenticate(string userName, string password)
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
            catch (LdapException)
            {
                return false;
            }

            return true;
        }

        public BiUserToken Impersonate(string adminUserIdEncrypted, string networkIdEncrypted)
        {
            var adminUserId = DecodeFrom64(adminUserIdEncrypted);
            var networkId = DecodeFrom64(networkIdEncrypted);


            var admin = _helperUser.GetBiUser(adminUserId);

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



        public BiUserModel AddUser(string userId, bool isAdmin)
        {
            var biUserCheck = BiUser.Helper.Find(x => x.UserId == userId).FirstOrDefault();
            if (biUserCheck != null) throw new Exception("User already exists");

            var queryHelperLdapUser = new MongoRawQueryHelper<LdapUser>();

            var ldapUser = queryHelperLdapUser.FindById(userId);

            if (ldapUser == null)
            {
                throw new Exception("Invalid UserId specified");
            }

            var biUser = new BiUser(ldapUser, isAdmin);
            BiUser.Helper.Upsert(biUser);

            return new BiUserModel(biUser);
        }

        public BiUserModel GetNewBiUserModel()
        {
            return new BiUserModel();
        }

        public void RemoveBiUser(string userId)
        {
            var removeUser = BiUser.Helper.Find(x => x.UserId == userId).FirstOrDefault();
            if (removeUser != null)
            {
                BiUser.Helper.DeleteOne(removeUser.Id);
            }

        }

        public BiUserModel GetBiUserModel(string userId)
        {
            var biUser = BiUser.Helper.Find(x => x.User.Id == userId).FirstOrDefault();
            if (biUser == null) throw new Exception("User not found");

            return new BiUserModel(biUser);
        }


        public BiUserModel SaveBiUserModel(BiUserModel model)
        {
            var biUser = BiUser.Helper.Find(x => x.UserId == model.UserId).FirstOrDefault();
            if (biUser == null) throw new Exception("User not found");


            biUser.Person = model.Person;
            biUser.Location = model.Location;
            biUser.SystemAdmin = model.SystemAdmin;

            BiUser.Helper.Upsert(biUser);

            return new BiUserModel(biUser);

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
            return new RepositoryBase<AppPermission>().AsQueryable().ToList().Select(x => new AppPermissionModel(x)).ToList();
        }

        public bool GetHasAppPermission(string userId, string permissionLabel)
        {
            var biUser = _helperUser.GetBiUser(userId);

            return biUser.AppPermissions.Any(x => x.Label == permissionLabel);
        }

    }
}
