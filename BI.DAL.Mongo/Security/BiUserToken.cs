using System;
using System.Linq;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson.Serialization.Attributes;

namespace BI.DAL.Mongo.Security
{
    public class BiUserToken : BaseDocument
    {
        public static readonly MongoRawQueryHelper<BiUserToken> Helper = new MongoRawQueryHelper<BiUserToken>();
        public string TokenId => Id.ToString();
        public LdapUserRef User { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Expires { get; set; }
        private static readonly object _resetTokenLock = new object();
        public BiUserToken()
        {
        }

        public static BiUserToken Reset(LdapUser user)
        {
            BiUserToken token;
            lock (_resetTokenLock)
            {
                token = Helper.Find(Helper.FilterBuilder.Eq(x => x.User.Id, user.Id.ToString())).SingleOrDefault();

                if (token != null)
                {
                    Helper.DeleteOne(token.Id);
                }

                var tokenConfig = BiUserTokenConfig.Get();

                token = new BiUserToken()
                {
                    User = user.AsLdapUserRef(),
                    Expires = tokenConfig.GetExpireDate
                };

                BiUserToken.Helper.Upsert(token);
            }

            return token;
        }

        public static (BiUserToken token, bool expired) Get(LdapUser user)
        {
            var rep = new RepositoryBase<BiUserToken>();
            var token = BiUserToken.Helper.Find(BiUserToken.Helper.FilterBuilder.Eq(x=>x.User.Id,user.Id.ToString())).SingleOrDefault();

            if (token == null)
            {
                return (null, true);
            }
            var expired = (token.Expires <= DateTime.Now);

            return (token, expired);

        }

        public static BiUserToken Create(LdapUser user)
        {
            return Reset(user);
        }
    }
}
