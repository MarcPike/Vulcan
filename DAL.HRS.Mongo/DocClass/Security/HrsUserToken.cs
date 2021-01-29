using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Ldap;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Security
{
    public class HrsUserToken : BaseDocument
    {
        public static readonly MongoRawQueryHelper<HrsUserToken> Helper = new MongoRawQueryHelper<HrsUserToken>();
        public string TokenId => Id.ToString();
        public LdapUserRef User { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Expires { get; set; }
        private static readonly object _resetTokenLock = new object();
        public HrsUserToken()
        {
        }

        public static HrsUserToken Reset(LdapUser user)
        {
            HrsUserToken token;
            lock (_resetTokenLock)
            {
                token = HrsUserToken.Helper.Find(HrsUserToken.Helper.FilterBuilder.Eq(x => x.User.Id, user.Id.ToString())).SingleOrDefault();

                if (token != null)
                {
                    HrsUserToken.Helper.DeleteOne(token.Id);
                }

                var repConfig = new RepositoryBase<HrsUserTokenConfig>();
                var tokenConfig = repConfig.AsQueryable().FirstOrDefault();
                if (tokenConfig == null)
                {
                    tokenConfig = new HrsUserTokenConfig();
                    repConfig.Upsert(tokenConfig);
                }


                token = new HrsUserToken()
                {
                    User = user.AsLdapUserRef(),
                    Expires = tokenConfig.GetExpireDate
                };

                HrsUserToken.Helper.Upsert(token);
            }

            return token;
        }

        public static (HrsUserToken token, bool expired) Get(LdapUser user)
        {
            var rep = new RepositoryBase<HrsUserToken>();
            var token = HrsUserToken.Helper.Find(HrsUserToken.Helper.FilterBuilder.Eq(x=>x.User.Id,user.Id.ToString())).SingleOrDefault();

            if (token == null)
            {
                return (null, true);
            }
            var expired = (token.Expires <= DateTime.Now);

            return (token, expired);

        }

        public static HrsUserToken Create(LdapUser user)
        {
            return Reset(user);
        }
    }
}
