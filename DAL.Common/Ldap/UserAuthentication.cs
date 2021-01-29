using DAL.Common.DocClass;
//using DAL.Common.Ldap.DAL.WindowsAuthentication.MongoDb;
using DAL.Vulcan.Mongo.Base.Repository;
using System;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Security;
using DAL.Common.Ldap.DAL.WindowsAuthentication.MongoDb;

namespace DAL.Common.Ldap
{
    public static class UserAuthentication
    {
        //public static bool AuthenticateOld(string networkId, string password)
        //{
        //    try
        //    {
        //        bool valid = false;
        //        using (PrincipalContext context = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain, "howcogroup.com"))
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


        //private static SecureString ConvertToSecureString(string password)
        //{
        //    if (password == null)
        //        throw new ArgumentNullException("password");

        //    var securePassword = new SecureString();

        //    foreach (char c in password)
        //        securePassword.AppendChar(c);

        //    securePassword.MakeReadOnly();
        //    return securePassword;
        //}

        //public static bool Authenticate(string userName, string password)
        //{
        //    try
        //    {
        //        using (LdapConnection connection = new LdapConnection("howcogroup.com:636"))
        //        {
        //            connection.SessionOptions.SecureSocketLayer = true;
        //            connection.SessionOptions.ProtocolVersion = 3;
        //            connection.AuthType = AuthType.Negotiate;  //AuthType.Kerberos;

        //            //connection.AuthType = AuthType.Ntlm;
        //            NetworkCredential credential = new NetworkCredential(userName, password);

        //            Uri myUri = new Uri("http://www.howcogroup.com");

        //            WebRequest wreq = WebRequest.Create(myUri);
        //            wreq.Credentials = credential;

        //            connection.Credential = CredentialCache.DefaultNetworkCredentials;
        //            connection.Bind();
        //            connection.Dispose();
        //        }
        //    }
        //    catch (LdapException)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //public static LdapUser GetUser(string networkId, string password)
        //{
        //    if (!Authenticate(networkId, password)) return null;

        //    var rep = new RepositoryBase<LdapUser>();
        //    var userFound = rep.AsQueryable().FirstOrDefault(x => x.NetworkId == networkId);

        //    if (userFound != null) return userFound;

        //    RefreshAll();

        //    rep = new RepositoryBase<LdapUser>();
        //    userFound = rep.AsQueryable().FirstOrDefault(x => x.NetworkId == networkId);
        //    return userFound;
        //}

        public static void RefreshAll()
        {
            var reader = new LdapReader();
            reader.RefreshUserListFromLdap();
        }
    }
}

