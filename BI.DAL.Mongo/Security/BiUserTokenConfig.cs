using System;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization.Attributes;

namespace BI.DAL.Mongo.Security
{
    public class BiUserTokenConfig : BaseDocument
    {
        public static MongoRawQueryHelper<BiUserTokenConfig> Helper = new MongoRawQueryHelper<BiUserTokenConfig>();
        public int ExpireHours { get; set; } = 24;
        public int ExpireMinutes { get; set; } = 0;

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

        public static BiUserTokenConfig Get()
        {
            var config = Helper.GetAll().FirstOrDefault();
            if (config == null)
            {
                config = new BiUserTokenConfig();
                Helper.Upsert(config);
            }

            return config;
        }
    }
}