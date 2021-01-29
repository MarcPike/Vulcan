using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Repository;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
{
    public class UserLogControllerMethodHistory
    {
        public string Controller { get; set; } = String.Empty;
        public string Method { get; set; } = String.Empty;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExecutedOnDateTime { get; set; }
    }
    public class CrmUserLog : BaseDocument
    {
        public CrmUserRef User { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LastConnected { get; set; } = DateTime.MinValue;
        public List<UserLogControllerMethodHistory> ControllerMethodHistory { get; set; } = new List<UserLogControllerMethodHistory>();

        public static void ConnectionMade(CrmUserRef user)
        {
            var rep = new RepositoryBase<CrmUserLog>();
            var log = rep.AsQueryable().FirstOrDefault(x => x.User.Id == user.Id) ?? new CrmUserLog()
            {
                User = user
            };
            log.LastConnected = DateTime.Now;
            rep.Upsert(log);
        }

        public static void ControllerMethodCalled(CrmUserRef user, string controller, string method)
        {
            var rep = new RepositoryBase<CrmUserLog>();
            var log = rep.AsQueryable().FirstOrDefault(x => x.User.Id == user.Id) ?? new CrmUserLog()
            {
                User = user
            };

            log.ControllerMethodHistory.Add(new UserLogControllerMethodHistory()
            {
                Controller = controller,
                Method = method,
                ExecutedOnDateTime = DateTime.Now
            });
            rep.Upsert(log);
        }
    }
}
