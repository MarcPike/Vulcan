using System;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.Ldap;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.Security
{
    public class Login: BaseDocument
    {
        public LdapUser User { get; set; }
        public string ApplicationName { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LoginTime { get; set; } = DateTime.UtcNow;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LogoutTime { get; set; } = DateTime.MaxValue;
    }
}