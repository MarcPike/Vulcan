using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Ldap;

namespace DAL.WindowsAuthentication.MongoDb
{
    public static class UserAuthentication
    {
        //public static bool Authenticate(string networkId, string password)
        //{
        //    try
        //    {
        //        bool valid = false;
        //        using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "howcogroup.com"))
        //        {
        //            valid = context.ValidateCredentials(networkId, password);
        //        }
        //        return valid;

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Invalid Credentials for {networkId}: {e.Message}");
        //        return false;
        //    }
        //}

        public static bool Authenticate(string userName, string password)
        {
            try
            {
                using (var connection = new LdapConnection("howcogroup.com:389"))
                {
                    connection.SessionOptions.ProtocolVersion = 3;
                    connection.SessionOptions.Signing = true;
                    connection.SessionOptions.Sealing = true;
                    //connection.AuthType = AuthType.Negotiate;  //AuthType.Kerberos;
                    //connection.SessionOptions.SecureSocketLayer = true;

                    var credential = new NetworkCredential
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

        public static LdapUser GetUser(string networkId, string password)
        {
            if (!Authenticate(networkId, password)) return null;

            var rep = new RepositoryBase<LdapUser>();
            var userFound = rep.AsQueryable().FirstOrDefault(x => x.NetworkId == networkId);

            if (userFound != null) return userFound;

            RefreshAll();

            rep = new RepositoryBase<LdapUser>();
            userFound = rep.AsQueryable().FirstOrDefault(x => x.NetworkId == networkId);
            return userFound;
        }

        public static void RefreshAll()
        {
            var reader = new LdapReader();
            reader.RefreshUserListFromLdap();
        }
    }
}