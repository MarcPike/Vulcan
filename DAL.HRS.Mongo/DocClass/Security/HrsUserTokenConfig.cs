using System;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Security
{
    public class HrsUserTokenConfig : BaseDocument
    {

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
    }
}