using DAL.Vulcan.Mongo.Base.Core.Repository;
using MongoDB.Bson;
using System.Linq;
using Action = DAL.Vulcan.Mongo.Core.DocClass.CRM.Action;

namespace DAL.Vulcan.Mongo.Core.DocClass.Fetchers
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
