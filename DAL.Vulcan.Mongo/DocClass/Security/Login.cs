using System;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.Security
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