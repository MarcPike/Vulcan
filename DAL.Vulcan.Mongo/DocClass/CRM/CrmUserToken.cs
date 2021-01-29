using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Ldap;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class CrmUserTokenConfig : BaseDocument
    {

        public int ExpireHours { get; set; } = 0;
        public int ExpireMinutes { get; set; } = 2;

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime GetExpireDate
        {
            get
            {
                var result = DateTime.Now;
                result = result.AddHours(ExpireHours);
                result = result.AddMinutes(ExpireMinutes);
                return result;
            }
        }
    }
    public class CrmUserToken : BaseDocument
    {
        public string TokenId => Id.ToString();
        public UserRef User { get; set; }
        public CrmUserRef CrmUserRef { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Expires { get; set; }
        private static readonly object _resetTokenLock = new object();
        public CrmUserToken()
        {
        }

        public static CrmUserToken Reset(CrmUser crmUser)
        {
            CrmUserToken token;
            lock (_resetTokenLock)
            {
                var rep = new RepositoryBase<CrmUserToken>();
                token = rep.AsQueryable()
                    .SingleOrDefault(x => x.User.Id == crmUser.User.Id.ToString());

                if (token != null)
                {
                    rep.RemoveOne(token);
                }

                var repConfig = new RepositoryBase<CrmUserTokenConfig>();
                var tokenConfig = repConfig.AsQueryable().FirstOrDefault();
                if (tokenConfig == null)
                {
                    tokenConfig = new CrmUserTokenConfig();
                    repConfig.Upsert(tokenConfig);
                }
                

                token = new CrmUserToken()
                {
                    User = crmUser.User,
                    CrmUserRef = crmUser.AsCrmUserRef(),
                    Expires = tokenConfig.GetExpireDate
                };

                rep.Upsert(token);
            }

            return token;
        }

        public static (CrmUserToken token, bool expired) Get(CrmUser crmUser)
        {
            var rep = new RepositoryBase<CrmUserToken>();
            var token = rep.AsQueryable()
                .SingleOrDefault(x => x.User.Id == crmUser.User.Id.ToString());

            if (token == null)
            {
                return (null, true);
            }
            var expired = (token.Expires <= DateTime.Now);

            return (token, expired);

        }

        public static CrmUserToken Create(CrmUser crmUser)
        {
            return Reset(crmUser);
        }
    }
}
