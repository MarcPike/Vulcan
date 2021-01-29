using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.Base.Core.Context
{
    public static class DatabaseCredentialsFactory
    {
        public static MongoCredential GetCredentialsFor(string databaseName, SecurityType securityType)
        {
            var username = string.Empty;
            var password = string.Empty;
            if (securityType == SecurityType.SalesPerson)
            {
                username = "salesPerson";
                password = "H0wc0sal3s";
            }
            else if (securityType == SecurityType.BiUser)
            {
                username = "biUser";
                password = "H0wc0BiUser";
            
            } else if (securityType == SecurityType.HrsUser)
            {
                username = "hrsUser";
                password = "H0wc0HrsUser";
            }
            else if (securityType == SecurityType.HrsAdmin)
            {
                username = "hrsAdmin";
                password = "H0wc0HrsAdmin";
            }
            else if (securityType == SecurityType.SecurityAdmin)
            {
                username = "securityAdmin";
                password = "H0wc0s3cur1ty@dm1n";
            } else if (securityType == SecurityType.ReadOnly)
            {
                username = "readonly";
                password = "r3ad0nly";
            } else if (securityType == SecurityType.SystemAdmin)
            {
                username = "admin";
                password = "Ev3ntH0r1z0n";
            }
            return MongoCredential.CreateCredential(databaseName, username, password);
            
        }
    }
}