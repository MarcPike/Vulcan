using System;
using System.Linq;
using System.Text;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.CRM;
using MongoDB.Bson;
using Action = DAL.Vulcan.Mongo.DocClass.CRM.Action;

namespace DAL.Vulcan.Mongo.DocClass.Fetchers
{
    public static class ActionFetcher
    {
        public static ActionFetchResult GetUserActions(string salesPersonId)
        {
            var rep = new RepositoryBase<Action>();
            var id = ObjectId.Parse(salesPersonId);
            var result = new ActionFetchResult
            {
                Completed = rep.AsQueryable()
                    .Where(x => x.CrmUsers.Select(s=>s.AsCrmUser()).Any(s => s.Id == id) && x.PercentComplete == 100 &&
                                x.Failure == false).ToList(),
                Pending = rep.AsQueryable()
                    .Where(x => x.CrmUsers.Select(s => s.AsCrmUser()).Any(s => s.Id == id) && x.PercentComplete < 100 &&
                                x.Failure == false).ToList(),
                Failures = rep.AsQueryable()
                    .Where(x => x.CrmUsers.Select(s => s.AsCrmUser()).Any(s => s.Id == id) && x.Failure).ToList()
            };

            return result;
        }
    }
}
