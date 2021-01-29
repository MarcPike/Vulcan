using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BI.DAL.Mongo.Models;

namespace BI.DAL.Mongo.Helpers
{
    public interface IHelperQueries
    {
        List<BiQueryBaseModel> GetMyQueriesForType(string userId, string type);
        BiQueryBaseModel SaveMyQuery(BiQueryBaseModel model);
        void RemoveMyQuery(string userId, string queryId);
        BiQueryBaseModel GetMyQuery(string userId, string queryId);
    }
}
